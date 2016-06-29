using System;
using ComplaintTool.Common.Config;
using ComplaintTool.Common.Extensions;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.MCProImageInterface.Model;

namespace ComplaintTool.MCProImageInterface.Incoming.Case
{
    /// <summary>
    /// Process FICN response.
    /// </summary>
    class FIMNXmlFileProcessor : CaseFilingXmlFileProcessor<Fimn>
    {
        public FIMNXmlFileProcessor(ComplaintUnitOfWork unitOfWork, XmlFileInfo xmlFile) 
            : base(unitOfWork, xmlFile)
        {
        }

        protected override bool MatchCase(Fimn fimn)
        {
            string arn = fimn.ARN;
            if (arn.IsEmpty()) throw ComplaintCaseFilingMatchException.EmptyARN(IncomingFileType.FIMN);

            // TODO sprawdzenie czy istnieje FICN w outgoingu
            // 20160118 - Adam Kutera -> nie sprawdzamy FICN
            //if (!_unitOfWork.Repo<FileRepo>().ExistsCaseFilingOutgointFile(mcCaseId, OutgoingFileType.FICN))
            //    return false;

            _complaint = _unitOfWork.Repo<ComplaintRepo>().FindByARN(arn);
            if (_complaint == null) 
                throw ComplaintCaseFilingMatchException.CaseNotFound(arn, IncomingFileType.FIMN);

            _complaintStage = _unitOfWork.Repo<ComplaintRepo>().FindLastStageForCaseFiling(_complaint.CaseId);
            if (_complaintStage == null)
                throw ComplaintCaseFilingMatchException.StageNotFound(arn, IncomingFileType.FIMN);

            _caseFilingRecord = new CaseFilingRecord();
            _caseFilingRecord.MasterCardCaseID = fimn.CaseID;
            _caseFilingRecord.FilingICA = fimn.FilingICA;
            _caseFilingRecord.FiledAgainstICA = fimn.FiledAgainstICA;
            _caseFilingRecord.CaseType = fimn.CaseType;
            _caseFilingRecord.PrimaryAccountNumber = fimn.CardholderNumber;
            _caseFilingRecord.CBReferenceNumber = fimn.ChargebackReferenceNumber;
            _caseFilingRecord.ResponseDueDate = CaseFilingUtil.ConvertToMComDate(fimn.ResponseDueDateTime);
            _caseFilingRecord.SubmittedDate = CaseFilingUtil.ConvertToMComDate(fimn.SubmittedDateTime);
            _caseFilingRecord.ReasonCode = fimn.ChargebackReasonCode;
            _caseFilingRecord.ViolationCode = fimn.ViolationCode;
            _caseFilingRecord.ViolationDate = CaseFilingUtil.ConvertToMComDate(fimn.ViolationDate);
            _caseFilingRecord.FilingRegion = fimn.FilingRegion;
            _caseFilingRecord.FiledAgainstRegion = fimn.FilingAgainstRegion;
            _caseFilingRecord.CurrencyCode = fimn.DisputeAmountCurrencyCode;
            _caseFilingRecord.CaseId = _complaint.CaseId;
            _caseFilingRecord.CaseReceiver = false;
            return true;
        }

        protected override bool ProcessCase(Fimn fimn)
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

            // wprowadza do bazy danych nową notyfikację
            string note = string.Format(ComplaintConfig.Instance.Notifications[156].MessageText, IncomingFileType.FIMN, _complaint.CaseId);
            _unitOfWork.Repo<NotificationRepo>().InsertFileStageNotificationForIncoming(note, OrganizationId, _caseFilingIncomingFile, FileDescription);

            return true;
        }
    }
}
