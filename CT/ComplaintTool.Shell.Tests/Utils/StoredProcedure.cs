using System.Data.Entity;

namespace ComplaintTool.Shell.Tests.Utils
{
    public class StoredProcedure:ComplaintTool.DataAccess.Repos.RepositoryBase
    {
        private DbContext _context;

        public StoredProcedure(DbContext context):base(context)
        {
            _context = context;
        }

        public void GetDocumentProc(long docId, out string fileName, out byte[] fileStream)
        {
            GetDocument(docId, out fileName, out fileStream);
        }
    }
}
