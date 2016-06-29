using System.IO;
using ComplaintTool.Common.Config;
using ComplaintTool.Common.Enum;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.MCProImageInterface.Model;

namespace ComplaintTool.MCProImageInterface.Incoming.Chargeback
{
    class RMWXmlFileProcessor : ChargebackXmlFileProcessor<Rmw>
    {
        private ComplaintRecord _incomingComplaintRecord;

        public RMWXmlFileProcessor(ComplaintUnitOfWork unitOfWork, XmlFileInfo xmlFile) 
            : base(unitOfWork, xmlFile)
        {
        }

        protected override bool MatchChargeback(Rmw data)
        {
            var arn = data.ARD.ToString();
            var issuerRefData = data.CardIssuerReferenceData;
            
            LengthFromXml = data.AdditionalData.MasterComImageMetadata.S1MasterComTIFFImageDataLengthHex;
            HashFromXml = data.AdditionalData.MasterComImageMetadata.S3MasterComTIFFImageCRC32;
            TifFileNameFromXml = data.AdditionalData.MasterComImageMetadata.S4MasterComTIFFImageFilename;

            var incoming = _unitOfWork.Repo<IncomingTranRepo>()
                .FindIncomingMcForImageProProcess(arn, issuerRefData, data.Mti, data.FunctionCode);
            if (incoming == null) return false;

            _complaint = _unitOfWork.Repo<ComplaintRepo>().FindByARN(arn);
            if (_complaint == null) return false;

            _incomingComplaintRecord = _unitOfWork.Repo<ComplaintRepo>().FindRecordByIssuerReference(issuerRefData);
            if (_incomingComplaintRecord != null)
            {
                // TODO do weryfikacji
                _complaintStage = _unitOfWork.Repo<ComplaintRepo>().GetStageById(_incomingComplaintRecord.StageId.GetValueOrDefault());
            }

            return true;
        }

        protected override bool ProcessChargeback(Rmw data)
        {
            var pdfPath = FilePath.Replace(".xml", ".pdf").Replace(".XML", ".pdf");
            TiffToPdf.TiffToPDF(TifFilePath, pdfPath);

            if (File.Exists(pdfPath))
            {
                _unitOfWork.Repo<DocumentRepo>().AddDocumentToComplaint(_complaint.CaseId, pdfPath, FileSource.MC, _incomingFile);
            }

            _unitOfWork.Repo<ComplaintRepo>().UnsuspendCase(_incomingFile.CaseId);
            var note = string.Format(ComplaintConfig.Instance.Notifications[136].MessageText, _incomingFile.FileName);
            _unitOfWork.Repo<NotificationRepo>().InsertFileStageNotificationForIncoming(note, OrganizationId, _incomingFile, "RMW file - Supporting document");

            Logger.LogComplaintEvent(118, ProcessName, _incomingFile.FileName, Path.GetFileName(TifFilePath), _incomingFile.CaseId, Path.GetFileName(pdfPath));
            return true;
        }
    }
}
