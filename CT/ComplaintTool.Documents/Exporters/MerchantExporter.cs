using ComplaintTool.Common;
using ComplaintTool.Common.Config;
using ComplaintTool.Common.Enum;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.Documents.DocCreators;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Principal;
using System.Linq;
using ComplaintTool.Common.Utils;
using ComplaintTool.Common.CTLogger;


namespace ComplaintTool.Documents.Exporters
{
    public class MerchantExporter:IExporter
    {
        private IDocCreator _docCreator;
        private ComplaintUnitOfWork _unitOfWork = null;
        private const string DOCH = "DOCH";
        private static readonly ILogger Logger = LogManager.GetLogger();
        private const string note = "Automatic document added to complaint, CaseId: {0}, file name: {1}, document name: {2}";
        private string _baseFolder;

        public MerchantExporter(string baseFolder)
        {
            _docCreator = new PdfDocCreator();
            _baseFolder = baseFolder;
        }

        public string ProcessName
        {
            get { return Globals.MerchantExportProcess; }
        }

        public void Export()
        {
            IEnumerable<CaseItem> caseItemsList = new List<CaseItem>();
            var unitOfWork = ComplaintUnitOfWork.Create();

            caseItemsList = unitOfWork.Repo<ComplaintRepo>().GetCaseItemsList();
            if (caseItemsList.Count() == 0)
                return;

            _docCreator.LoadConfiguration();

            foreach (var caseItem in caseItemsList)
            {
                ExportItem(caseItem);
            }
        }

        private void ExportItem(CaseItem caseItem)
        {
            using (_unitOfWork = ComplaintUnitOfWork.Create())
            {
                try
                {
                    var bytes = _docCreator.CreateDocument(caseItem.CaseId, caseItem.StageId);
                    string fileName = SaveDocument(caseItem, bytes);
                    Guid streamId = _unitOfWork.Repo<FileRepo>().AddDocumentFile(fileName, bytes);
                    var complaintDocument = InsertComplaintDocument(caseItem, streamId, fileName);
                    InsertComplaintStageDocument(caseItem, complaintDocument.DocumentId);
                    _unitOfWork.Repo<ComplaintRepo>().AddComplaintNote(caseItem.CaseId, string.Format(note, caseItem.CaseId, complaintDocument.FileName, fileName));
                    _unitOfWork.Commit();
                    Logger.LogComplaintEvent(154,new object[]{fileName,caseItem.CaseId});
                }
                catch (Exception ex)
                {
                    Logger.LogComplaintException(ex);
                }
            }
        }

        private string SaveDocument(CaseItem caseItem, byte[] bytes)
        {         
            string targetFolderName=CreateTargetFolder();
            string stageName = ComplaintDictionaires.GetMapping(DOCH, caseItem.StageCode);
            var fileName = string.Format(@"{0}_{1}.{2}", stageName, caseItem.CaseId, _docCreator.Extension);
            var filePath = Path.Combine(targetFolderName,fileName);
                                          
            if (File.Exists(filePath))
                File.Delete(filePath);

            using (var fs = File.Create(filePath))
            {
                fs.Write(bytes, 0, bytes.Length);
            }

            return fileName;
        }

        private ComplaintDocument InsertComplaintDocument(CaseItem caseItem, Guid streamId, string fileName)
        {
            string documentName = _unitOfWork.Repo<DocumentRepo>().GetNewDocumentName(caseItem.CaseId, FileSource.ESRV, Path.GetFileName(fileName));
            var complaintDocument = new ComplaintDocument
            {
                CaseId = caseItem.CaseId,
                StageId = caseItem.StageId,
                stream_id = streamId,
                FileName = documentName,
                SourceIncoming = FileSource.ESRV.ToString(),
                SourceFileName = fileName,
                Incoming = false,
                Manual = false,
                IsAutomatic = true,
                Export = true,
                ExportStatus = (int)DocumentExportStatus.Exported,
                ExportDate = DateTime.Now,
                ExportUser = WindowsIdentity.GetCurrent() != null ? WindowsIdentity.GetCurrent().Name : string.Empty,
                InsertDate = DateTime.Now,
                InsertUser = WindowsIdentity.GetCurrent() != null ? WindowsIdentity.GetCurrent().Name : string.Empty
            };
            _unitOfWork.Repo<ComplaintDocumentsRepo>().Add(complaintDocument);

            return complaintDocument;
        }

        private void InsertComplaintStageDocument(CaseItem caseItem,long documentId)
        {
            var complaintStageDocument = new ComplaintStageDocument
            {
                CaseId = caseItem.CaseId,
                StageId = caseItem.StageId,
                StageCode = caseItem.StageCode,
                DocumentId = documentId,
                InsertDate = DateTime.Now,
                InsertUser = WindowsIdentity.GetCurrent() != null? WindowsIdentity.GetCurrent().Name: string.Empty
            };

            _unitOfWork.Repo<StageRepo>().AddComplaintStageDocument(complaintStageDocument);
        }

        private string CreateTargetFolder()
        {
            string targetFolder = _baseFolder + @"\"
                                               +
                                               DateTime.Now.Year.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0')
                                               +
                                               DateTime.Now.Month.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0')
                                               + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');

            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);

            return targetFolder;
        }
    }
}
