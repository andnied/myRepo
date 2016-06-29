using System;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using ComplaintTool.Common.Enum;
using ComplaintTool.Common;
using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.Models;
using ComplaintTool.Common.Utils;
using ComplaintTool.Common.Config;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Documents.Exporters
{
    public class DocumentExporter : IExporter
    {
        private static readonly ILogger Logger = LogManager.GetLogger();

        private string _baseFolder;
        private bool _organizationDivision;

        public DocumentExporter(string baseFolder, bool organizationDivision)
        {
            _baseFolder = baseFolder;
            _organizationDivision = organizationDivision;
        }


        public string ProcessName
        {
            get
            {
                return Globals.DocumentToFolderProcess;
            }
        }

        public void Export()
        {
            using (var unitOfWork = ComplaintUnitOfWork.Create())
            {
                var complaintDocuments = unitOfWork.Repo<ComplaintDocumentsRepo>().GetByStatus((int)DocumentExportStatus.NotExported).ToList();

                foreach(var complaintDocument in complaintDocuments)
                {
                    try
                    {
                        string organizationId = complaintDocument.Complaint.OrganizationId;
                        string baseFolder = GetFolderPath(organizationId);
                        unitOfWork.Repo<DocumentRepo>().GenerateDocument(complaintDocument.DocumentId, baseFolder);
                        UpdateStatus(complaintDocument,DocumentExportStatus.Exported);
                        Logger.LogComplaintEvent(111, new object []{ complaintDocument.FileName});
                    }catch(Exception ex)
                    {
                        UpdateStatus(complaintDocument,DocumentExportStatus.FailedExported);
                        Logger.LogComplaintException(ex);
                    }
                }
            }
        }

        private void UpdateStatus(ComplaintDocument document,DocumentExportStatus status)
        {
            int statusExport=(int)status;
            using(var unitOfWork = ComplaintUnitOfWork.Create())
            {
                var complaintDocument=unitOfWork.Repo<ComplaintDocumentsRepo>().GetByDocumentId(document.DocumentId);
                if(complaintDocument==null)return;
                complaintDocument.ExportStatus=statusExport;
                complaintDocument.ExportDate=DateTime.UtcNow;
                unitOfWork.Repo<DocumentRepo>().Update(complaintDocument);
                unitOfWork.Commit();
            }
        }

        private string GetFolderPath(string OrganizationId)
        {
            return _organizationDivision ? Path.Combine(_baseFolder, OrganizationId) : _baseFolder;
        }
    }
}
