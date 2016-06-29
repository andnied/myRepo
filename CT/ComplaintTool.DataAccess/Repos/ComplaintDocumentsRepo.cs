using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Models;

namespace ComplaintTool.DataAccess.Repos
{
    public class ComplaintDocumentsRepo:RepositoryBase
    {
        public ComplaintDocumentsRepo(DbContext dbContext) : base(dbContext) { }

        public void Add(ComplaintDocument complaintDocument)
        {
            GetDbSet<ComplaintDocument>().Add(complaintDocument);
        }

        public int GetCountByCaseId(string caseId)
        {
            return GetDbSet<ComplaintDocument>().Count(x => x.CaseId == caseId);
        }

        public IEnumerable<ComplaintDocument> GetByStatus(int status)
        {
            return GetDbSet<ComplaintDocument>().Include(x=>x.Complaint).Where(x => x.ExportStatus == status);
        }

        public ComplaintDocument GetByDocumentId(long documentId)
        {
            return GetDbSet<ComplaintDocument>().FirstOrDefault(x => x.DocumentId == documentId);
        }

    }
}
