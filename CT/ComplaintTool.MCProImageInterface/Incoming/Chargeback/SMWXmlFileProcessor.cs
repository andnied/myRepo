using System;
using System.IO;
using ComplaintTool.Common;
using ComplaintTool.Common.Config;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.MCProImageInterface.Model;

namespace ComplaintTool.MCProImageInterface.Incoming.Chargeback
{
    class SMWXmlFileProcessor : ChargebackXmlFileProcessor<Smw>
    {
        public SMWXmlFileProcessor(ComplaintUnitOfWork unitOfWork, XmlFileInfo xmlFile)
            : base(unitOfWork, xmlFile)
        {
        }

        protected override bool MatchChargeback(Smw data)
        {
            string arn = data.ARD.ToString();
            this.LengthFromXml = data.AdditionalData.MasterComImageMetadata.S1MasterComTIFFImageDataLengthHex;
            this.HashFromXml = data.AdditionalData.MasterComImageMetadata.S3MasterComTIFFImageCRC32;
            this.TifFileNameFromXml = data.AdditionalData.MasterComImageMetadata.S4MasterComTIFFImageFilename;

            _complaint = _unitOfWork.Repo<ComplaintRepo>().FindByARN(arn);
            if (_complaint == null) return false;

            _complaintStage = _unitOfWork.Repo<ComplaintRepo>().FindLastStageByCaseId(_complaint.CaseId);
            if (_complaintStage == null) return false;

            return true;
        }

        protected override bool ProcessChargeback(Smw data)
        {
            // TODO sprawdzić newTempPath
            //string tifPath = @"\" + incomingImagePro.FileName.Replace(".xml", ".TIF").Replace(".XML", ".TIF");
            string tifPath = this.TifFilePath;

            // dodaje nowe id do listy nazw
            _unitOfWork.Repo<NameListRepo>().AddNewIdInNameList(tifPath, this.OrganizationId, _incomingFile);

            //Complaint complaint = _complaintRepo.FindByCaseId(incomingImagePro.CaseId);
            string baseFolder = FileUtil.GetFolder(ComplaintConfig.GetParameter(Globals.MCProImageBaseFolderParam));
            string arn = _complaint.ARN ?? "All";
            string docsTempDir = string.Format(@"{0}\{1}\{2}\{3}", baseFolder, Globals.TempDocsDirectoryName, DateTime.UtcNow.ToString("yyyyMMdd"), arn);

            if (!Directory.Exists(docsTempDir))
                Directory.CreateDirectory(docsTempDir);

            string stageCode = _unitOfWork.Repo<ComplaintRepo>().GetStageById(_incomingFile.StageId).StageCode ?? "";
            string docTempPath = string.Format(@"{0}\{1}_{2}", docsTempDir, stageCode, Path.GetFileName(tifPath));
            if (File.Exists(tifPath))
                File.Move(tifPath, docTempPath);

            string msgNote = string.Format(ComplaintConfig.Instance.Notifications[137].MessageText, _incomingFile.FileName);
            string fileDesc = "SMW file - Chargeback acknowledgment";
            _unitOfWork.Repo<NotificationRepo>().InsertFileStageNotificationForIncoming(msgNote, this.OrganizationId, _incomingFile, fileDesc);

            string info = string.Format("Document {0} is moved to {1} directory.", Path.GetFileName(tifPath), Globals.TempDocsDirectoryName);
            Logger.LogComplaintEvent(135, ProcessName, _incomingFile.FileName, _incomingFile.CaseId, info);
            return true;
        }
    }
}
