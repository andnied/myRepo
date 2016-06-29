using System;
using System.IO;
using System.Linq;
using ComplaintTool.Common.Config;
using ComplaintTool.Common.Enum;
using ComplaintTool.Common.Extensions;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.MCProImageInterface.Model;

namespace ComplaintTool.MCProImageInterface.Incoming.Case
{
    class FIMFAXmlFileProcessor : CaseFilingXmlFileProcessor<Fimfa>
    {
        public override string FileDescription
        {
            get
            {
                return IncomingFileType.FIMFA;
            }
        }

        public FIMFAXmlFileProcessor(ComplaintUnitOfWork unitOfWork, XmlFileInfo xmlFile) 
            : base(unitOfWork, xmlFile)
        {
        }

        protected override bool MatchCase(Fimfa fimfa)
        {
            string arn = fimfa.ARN;
            if (arn.IsEmpty()) throw ComplaintCaseFilingMatchException.EmptyARN(FileDescription);
            
            if (!_unitOfWork.Repo<CaseFilingRepo>().CheckIfExistsAnyComplaintWithMasterCardCaseId(fimfa.CaseID))
                throw ComplaintCaseFilingMatchException.MasterCardCaseIdNotExists(fimfa.CaseID);

            _complaint = _unitOfWork.Repo<ComplaintRepo>().FindByARN(arn);
            if (_complaint == null)
                throw ComplaintCaseFilingMatchException.CaseNotFound(arn, FileDescription);

            _complaintStage = _unitOfWork.Repo<ComplaintRepo>().FindLastStageForCaseFiling(_complaint.CaseId);
            if (_complaintStage == null)
                throw ComplaintCaseFilingMatchException.StageNotFound(arn, FileDescription);

            _caseFilingRecord = new CaseFilingRecord();
            _caseFilingRecord.MasterCardCaseID = fimfa.CaseID;
            _caseFilingRecord.FilingICA = fimfa.FilingICA;
            _caseFilingRecord.FiledAgainstICA = fimfa.FiledAgainstICA;
            _caseFilingRecord.CaseType = fimfa.CaseType;
            _caseFilingRecord.PrimaryAccountNumber = fimfa.CardholderNumber;
            _caseFilingRecord.CBReferenceNumber = fimfa.ChargebackReferenceNumber;
            _caseFilingRecord.ResponseDueDate = CaseFilingUtil.ConvertToMComDate(fimfa.ResponseDueDateTime);
            _caseFilingRecord.SubmittedDate = CaseFilingUtil.ConvertToMComDate(fimfa.SubmittedDateTime);
            _caseFilingRecord.RebuttalDate = CaseFilingUtil.ConvertToMComDate(fimfa.RebuttedDateTime);
            _caseFilingRecord.RejectedDate = CaseFilingUtil.ConvertToMComDate(fimfa.RejectedDateTime);
            _caseFilingRecord.AcceptedDate = CaseFilingUtil.ConvertToMComDate(fimfa.AcceptedDateTime);
            _caseFilingRecord.MemoText = fimfa.Memo;
            _caseFilingRecord.ReasonCode = fimfa.ChargebackReasonCode;
            _caseFilingRecord.ViolationCode = fimfa.ViolationCode;
            _caseFilingRecord.ViolationDate = CaseFilingUtil.ConvertToMComDate(fimfa.ViolationDate);
            _caseFilingRecord.FilingRegion = fimfa.FilingRegion;
            _caseFilingRecord.FiledAgainstRegion = fimfa.FilingAgainstRegion;
            _caseFilingRecord.CurrencyCode = fimfa.DisputeAmountCurrencyCode;
            _caseFilingRecord.CaseId = _complaint.CaseId;
            _caseFilingRecord.CaseReceiver = false;
            return true;
        }

        protected override bool ProcessCase(Fimfa data)
        {
            Guard.ThrowIf<ArgumentNullException>(_complaint == null, "complaint");
            Guard.ThrowIf<ArgumentNullException>(_complaintStage == null, "complaintStage");
            Guard.ThrowIf<ArgumentNullException>(_caseFilingRecord == null, "caseFilingRecord");

            // dodaje nowy Incoming
            _caseFilingIncomingFile = _unitOfWork.Repo<FileRepo>().CreateCaseFilingIncomingFile(
                        _complaint.CaseId,
                        _complaintStage.StageId,
                        _xmlFile.FileType,
                        _xmlFile.FileName,
                        _xmlFile.FileContent,
                        _incomingFileStreamId,
                        _bulkProcessKey);

            // dodaje nowy CaseFilingRecord
            _unitOfWork.Repo<CaseFilingRepo>().AddCaseFilingRecord(_caseFilingRecord, _caseFilingIncomingFile);

            UpdateStageDescription();
            AddComplaintDocuments();

            // wprowadza do bazy danych nową notyfikację
            string note = string.Format(ComplaintConfig.Instance.Notifications[156].MessageText, FileDescription, _complaint.CaseId);
            _unitOfWork.Repo<NotificationRepo>().InsertFileStageNotificationForIncoming(note, OrganizationId, _caseFilingIncomingFile, FileDescription);

            return true;
        }

        private void UpdateStageDescription()
        {
            Guard.ThrowIf<ArgumentNullException>(_complaintStage == null, "complaintStage");
            Guard.ThrowIf<ArgumentNullException>(_caseFilingRecord == null, "caseFilingRecord");
            GuardOnlyOneHaveValue(_caseFilingRecord.RebuttalDate, _caseFilingRecord.RejectedDate, _caseFilingRecord.AcceptedDate);

            string stageDescription = null;
            string currentStageCode = _complaintStage.StageCode;

            if (_caseFilingRecord.RebuttalDate.HasValue)
            {
                stageDescription = string.Format("{0}-REB", currentStageCode);
            }
            if (_caseFilingRecord.RejectedDate.HasValue)
            {
                stageDescription = string.Format("{0}-REJ", currentStageCode);
            }
            if (_caseFilingRecord.AcceptedDate.HasValue)
            {
                stageDescription = string.Format("{0}-ACC", currentStageCode);
            }

            _complaintStage.StageDescription = stageDescription;
        }

        private void GuardOnlyOneHaveValue(params DateTime?[] values)
        {
            var count = values.Where(x => x.HasValue).Count();
            if (count == 0)
            {
                throw ComplaintCaseFilingUpdateException.InvalidMissingDateValues(FileDescription);
            }
            else if (count > 1)
            {
                throw ComplaintCaseFilingUpdateException.InvalidTooManyDateValues(FileDescription);
            }
        }

        private void AddComplaintDocuments()
        {
            foreach (var tifFile in _xmlFile.TifFiles)
            {
                string pdfPath = tifFile.Replace(".tif", ".pdf").Replace(".TIF", ".pdf");
                TiffToPdf.TiffToPDF(tifFile, pdfPath);
                _unitOfWork.Repo<DocumentRepo>().AddDocumentToComplaint(_complaint.CaseId, 
                    pdfPath, FileSource.MC, _caseFilingIncomingFile);
                File.Delete(pdfPath);
            }
        }
    }
}
