using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.IO;
using System.Linq;
using ComplaintTool.Common.Enum;
using ComplaintTool.Models;

namespace ComplaintTool.DataAccess.Repos
{
    public class DocumentRepo : RepositoryBase
    {
        private readonly Dictionary<string, int> _currentCount = new Dictionary<string,int>();

        public DocumentRepo(DbContext context) 
            : base(context)
        {
        }

        private int GetNextCount(string caseId)
        {
            // TODO zalozyc lock na tabeli
            if (!_currentCount.ContainsKey(caseId))
            {
                int count = GetDbSet<ComplaintDocument>().Count(x => x.CaseId == caseId) + 1;
                _currentCount.Add(caseId, count);
            }
            return _currentCount[caseId]++;
        }

        public void AddDocumentToComplaint(string caseId, string filePath, FileSource fileSource, CaseFilingIncomingFile incomingFile)
        {
            var currentDate = GetCurrentDateTime();
            var currentUser = GetCurrentUser();

            Guid fileStreamId = Guid.NewGuid();
            byte[] fileStream = File.ReadAllBytes(filePath);
            string fileName = GetNewDocumentName(caseId, fileSource, filePath);
            // proc
            base.AddDocument(fileStreamId, fileStream, fileName);

            var complaintDocument = new ComplaintDocument
            {
                CaseId = caseId,
                StageId = incomingFile.StageId,
                stream_id = fileStreamId,
                FileName = fileName,
                Description = incomingFile.FileType,
                SourceFileName = Path.GetFileName(filePath),
                SourceIncoming = fileSource.ToDescription(),
                Incoming = true,
                Manual = false,
                Export = false,
                InsertDate = currentDate,
                InsertUser = currentUser
            };
            GetDbSet<ComplaintDocument>().Add(complaintDocument);

            var docItem = new CaseFilingIncomingFileDocumentItem
            {
                CaseFilingIncomingFile = incomingFile,
                ComplaintDocument = complaintDocument,
                InsertDate = currentDate,
                InsertUser = currentUser
            };

            GetDbSet<CaseFilingIncomingFileDocumentItem>().Add(docItem);
            Commit();
        }

        public string GetNewDocumentName(string caseId, FileSource fileSource, string orginalFileName)
        {
            var count = GetNextCount(caseId);
            var fileName = string.Format(@"{0}_{1}_{2}_{3}{4}"
                , caseId
                , count.ToString(CultureInfo.InvariantCulture).PadLeft(3, '0')
                , fileSource.ToString().PadRight(4, '_')
                , DateTime.UtcNow.ToString("yyyyMMddHHmm")
                , Path.GetExtension(orginalFileName));

            return fileName;
        }

        public void GenerateRepresentmentDocument(Representment representment, string tempFolderPath)
        {
            foreach (var repDoc in representment.RepresentmentDocuments.ToList())
            {
                this.GenerateDocument(repDoc.DocumentId, tempFolderPath);
            }
        }

        //public string GenerateDocument(long documentId, string dirPath)
        //{
        //    string fileName = null;
        //    byte[] fileStream = null;
        //    GetDocument(documentId, out fileName, out fileStream);

        //    if (!Directory.Exists(dirPath))
        //        Directory.CreateDirectory(dirPath);
        //    fileName = fileName.ToString().Replace("_", "");
        //    var pdfPath = dirPath + @"\" + fileName;

        //    File.WriteAllBytes(pdfPath, fileStream);
        //    return pdfPath;
        //}

        public Tuple<byte[], string> GenerateDocument(long documentId, string dirPath)
        {
            string fileName = null;
            byte[] fileStream = null;

            GetDocument(documentId, out fileName, out fileStream);

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            fileName = fileName.ToString().Replace("_", "");
            var pdfPath = dirPath + @"\" + fileName;

            File.WriteAllBytes(pdfPath, fileStream);

            return new Tuple<byte[], string>(fileStream, fileName);
        }

        #region DocumentExport

        public string GenerateDocumentExport(Complaint complaint, string tempFolderPath)
        {
            return GenerateDocumentExport(complaint.DocumentExports, tempFolderPath);
        }

        public string GenerateDocumentExport(IEnumerable<DocumentExport> docs, string tempFolderPath)
        {
            if (!Directory.Exists(tempFolderPath))
                Directory.CreateDirectory(tempFolderPath);

            var dirPath = string.Format(@"{0}\{1}", tempFolderPath, Guid.NewGuid());
            if (Directory.Exists(dirPath))
                throw new InvalidOperationException("Directory {0} for GenerateDocumentExport already exists!");

            Directory.CreateDirectory(dirPath);
            foreach (var doc in docs.ToList())
            {
                this.GenerateDocument(doc.DocumentId, dirPath);
            }

            return dirPath;
        }

        public IEnumerable<DocumentExport> FindDocumentsToExport(string orgId)
        {
            return GetDbSet<DocumentExport>()
                .Include(d => d.Complaint)
                .Where(x => x.Status == 0 && x.OrganizationId == orgId)
                .ToList();
        }

        public IEnumerable<DocumentExport> FindDocumentsToExportForCaseFiling(string caseId, string exportCode, long stageId)
        {
            var documentExports = GetDbSet<DocumentExport>();
            var complaintDocuments = GetDbSet<ComplaintDocument>();

            return (from de in documentExports
                    join cd in complaintDocuments
                    on de.DocumentId equals cd.DocumentId
                    where de.CaseId == caseId
                    where de.ExportCode == exportCode
                    where de.Status == 0
                    where cd.StageId == stageId
                    select de).ToList();
        }

        #endregion

        public long Add(DocumentExtractExport docExp)
        {
            return base.GetDbSet<DocumentExtractExport>().Add(docExp).DocumentExportExtractId;
        }

        public void Add(DocumentExportReference docRef)
        {
            base.GetDbSet<DocumentExportReference>().Add(docRef);
        }
    }
}
