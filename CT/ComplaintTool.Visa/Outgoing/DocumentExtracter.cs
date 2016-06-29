using ComplaintTool.Common;
using ComplaintTool.Common.Config;
using ComplaintTool.Common.CTLogger;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Visa.Outgoing
{
    public class DocumentExtracter : IVisaOutgoingService
    {
        private static readonly ILogger _logger = LogManager.GetLogger();

        public System.Collections.ICollection GetRecords(string orgId)
        {
            IEnumerable<Representment> representments = null;

            using (var unitOfWork = ComplaintUnitOfWork.Create())
            {
                representments = unitOfWork.Repo<RepresentmentRepo>().FindRepresentmentsForExtract(orgId, 2);
            }
            
            return representments.ToList();
        }

        public void ProcessRecord(object record, ComplaintUnitOfWork unitOfWork, string path)
        {
            var rep = (Representment)record;

            if (!rep.DocumentationIndicator)
            {
                rep.Status = 3;
                unitOfWork.Repo<RepresentmentRepo>().Update(rep);
                unitOfWork.Commit();
                return;
            }

            string arn = rep.Complaint.ARN;
            if (arn == null) return;

            var date = DateTime.Now.ToString("yyyyMMdd");
            var dirPath = path + @"\" + date + @"\" + arn;
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            foreach (var repDoc in rep.RepresentmentDocuments.Where(d => d.Status == 0).ToList())
            {
                var streamPair = unitOfWork.Repo<DocumentRepo>().GenerateDocument(repDoc.DocumentId, dirPath);
                var pdfStreamId = unitOfWork.Repo<FileRepo>().AddOutgoingFile(streamPair.Item1, streamPair.Item2);

                unitOfWork.Repo<RepresentmentRepo>().Add(new RepresentmentExtractDocument()
                {
                    CaseId = rep.CaseId,
                    DocumentId = repDoc.DocumentId,
                    ErrorFlag = false,
                    ExportDate = DateTimeOffset.Now,
                    ExportStatus = 2,
                    ExportUser = repDoc.InsertUser,
                    RepresentmentDocumentsId = repDoc.RepresentmentDocumentsId,
                    Status = 2,
                    PDF_stream_id = pdfStreamId
                });

                repDoc.Status = 2;
                unitOfWork.Repo<RepresentmentRepo>().Update(repDoc);
            }

            rep.Status = 3;

            unitOfWork.Repo<RepresentmentRepo>().Update(rep);
            unitOfWork.Repo<ComplaintRepo>().AddComplaintNote(rep.CaseId, string.Format("{0} Folder {1}", ComplaintConfig.Instance.Notifications[121].MessageText, rep.CaseId));
            unitOfWork.Commit();

            _logger.LogComplaintEvent(121);
        }

        public void Notify(Exception ex)
        {
            _logger.LogComplaintEvent(540, "VisaDocumentsExtract", ex.ToString());
        }
    }
}
