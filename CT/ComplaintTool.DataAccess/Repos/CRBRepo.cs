using ComplaintTool.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.DataAccess.Repos
{
    public class CRBRepo:RepositoryBase
    {
        public CRBRepo(DbContext dbContext) : base(dbContext) { }

        public IEnumerable<CRBReportItem> GetReportItemsByStatus(int? status)
        {
            return GetDbSet<CRBReportItem>().Include(x=>x.CRBReport).Where(x => x.CBReportStatus == status);
        }

        public int GetNextNumberInYear(int year)
        {
            var crbReport=GetDbSet<CRBReport>().Where(x => x.CurrentYear == year).OrderByDescending(x=>x.NumberInCurrentYear).FirstOrDefault();
            if (crbReport != null)
                return crbReport.NumberInCurrentYear + 1;
            else return 1;
        }

        public void AddCRBReport(CRBReport crbReport)
        {
            GetDbSet<CRBReport>().Add(crbReport);
        }

        public CRBReport FindLastReport(string caseId)
        {
            var crbReport = GetDbSet<CRBReportItem>().Include(x => x.CRBReport).Where(x => x.CaseId == caseId).OrderByDescending(x => x.CRBReportId).Select(x => x.CRBReport);
            return crbReport.FirstOrDefault();
        }

        public IEnumerable<CRBReportItem> GetItemsByCaseIdAndStatus(string caseId, int reportStatus)
        {
            return GetDbSet<CRBReportItem>().Where(x => x.CaseId == caseId && x.CBReportStatus == reportStatus).ToList();
        }
    }
}
