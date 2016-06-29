using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Models;

namespace ComplaintTool.DataAccess.Repos
{
    public class CRBReportRepo : RepositoryBase
    {
        public CRBReportRepo(DbContext context)
            : base(context)
        {
        }

        public IEnumerable<CRBReportItem> FindBCRBReportsByStatus(int status)
        {
            return GetDbSet<CRBReportItem>().Where(r => r.CBReportStatus == status).ToList();
        }

        public void Add(CRBReportItem CRBReport)
        {
            base.GetDbSet<CRBReportItem>().Add(CRBReport);
        }
    }
}
