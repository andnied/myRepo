using ComplaintTool.Common;
using ComplaintTool.Common.CTLogger;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Visa.Outgoing
{
    public class DocumentExporter : IVisaOutgoingService
    {
        private static readonly ILogger _logger = LogManager.GetLogger();
        private string _filePath = string.Empty;

        public System.Collections.ICollection GetRecords(string orgId)
        {
            IEnumerable<DocumentExport> documentExports = null;

            using (var unitOfWork = ComplaintUnitOfWork.Create())
            {
                documentExports = unitOfWork.Repo<DocumentRepo>().FindDocumentsToExport(orgId);
            }

            return documentExports.ToList();
        }

        public void ProcessRecord(object record, ComplaintUnitOfWork unitOfWork, string path)
        {
            var docExp = (DocumentExport)record;

            var complaint = docExp.Complaint;
            if (complaint == null) return;

            var arn = complaint.ARN;
            var date = DateTime.Now.ToString("yyyyMMdd");
            var dirPath = path + @"\" + date + @"\" + arn;

            //_filePath = unitOfWork.Repo<DocumentRepo>().GenerateDocument(docExp.DocumentId, dirPath);
            var streamPair = unitOfWork.Repo<DocumentRepo>().GenerateDocument(docExp.DocumentId, dirPath);
            var pdfStreamId = unitOfWork.Repo<FileRepo>().AddOutgoingFile(streamPair.Item1, streamPair.Item2);

            var newExtractId = unitOfWork.Repo<DocumentRepo>().Add(new DocumentExtractExport()
            {
                CaseId = complaint.CaseId,
                ErrorFlag = false,
                ExportDate = DateTimeOffset.Now,
                ExportStatus = 2,
                ExportUser = docExp.InsertUser,
                Status = 2,
                PDF_stream_id = pdfStreamId,
                InsertDate = DateTime.UtcNow,
                InsertUser = WindowsIdentity.GetCurrent().Name,
                FileNameWithPath = _filePath = streamPair.Item2
            });

            unitOfWork.Repo<DocumentRepo>().Add(new DocumentExportReference()
            {
                DocumentExportExtractId = newExtractId,
                DocumentExportId = docExp.DocumentExportId
            });

            docExp.Status = 2;

            unitOfWork.Repo<DocumentRepo>().Update(docExp);
            unitOfWork.Commit();

            _logger.LogComplaintEvent(111, _filePath);
        }

        public void Notify(Exception ex)
        {
            _logger.LogComplaintEvent(510, _filePath, ex.ToString());
        }
    }
}
