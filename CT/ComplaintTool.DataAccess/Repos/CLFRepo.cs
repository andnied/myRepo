using ComplaintTool.Common.Enum;
using ComplaintTool.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ComplaintTool.DataAccess.Repos
{
    public class CLFRepo : RepositoryBase
    {
        public CLFRepo(DbContext context)
            : base(context)
        {
        }

        public bool CLFReportExists(string filePath, bool errorFlag, int status)
        {
            return GetDbSet<CLFReport>().Any(c => c.FileName == filePath && c.ErrorFlag == errorFlag && c.Status >= status);
        }

        public CLFReport Add(CLFReport report)
        {
            return GetDbSet<CLFReport>().Add(report);
        }

        public CLFReportItem Add(CLFReportItem reportItem)
        {
            return GetDbSet<CLFReportItem>().Add(reportItem);
        }

        public CLFReport FindReport(long clfReportId)
        {
            return GetDbSet<CLFReport>().FirstOrDefault(x => x.CLFReportId == clfReportId);
        }

        public IEnumerable<CLFReport> FindReportsByStatus(ClfFileStatus fileStatus)
        {
            return GetDbSet<CLFReport>().Where(x => x.Status == (int)fileStatus).ToList();
        }

        public IEnumerable<CLFReportItem> FindItemsForReport(long reportId)
        {
            return GetDbSet<CLFReportItem>().Where(x => x.CLFReportId == reportId).ToList();
        }

        //public IEnumerable<ClfItems> FindIncomingItemsWithStages(int reportStatus)
        //{
        //    var itemResult = from item in GetDbSet<CLFReportItem>().Include(x => x.CLFReport).Include(x => x.Complaint).Include(x => x.Complaint.ComplaintValues)
        //                 .Include(x => x.Complaint.ComplaintStages).Include(x => x.Complaint.ComplaintRecords)
        //                     /*join stage in GetDbSet<StageDefinition>().Where(x => x.IsActive.HasValue && x.IsActive.Value)
        //                     on item.Stage equals stage.StageCode*/
        //                     where item.CLFReportStatus == reportStatus && item.CLFItemIdSource == null
        //                     select item; //&& stage.DayStepCLF.HasValue
        //    //select new ClfItems { IncomingItem = item, StageDefinition = null/*stage*/ };
        //    var resultList = new List<ClfItems>();
        //    itemResult.ToList().ForEach(x =>
        //        {
        //            var stageDef = GetDbSet<StageDefinition>().FirstOrDefault(y => y.IsActive && y.StageCode == x.Stage && y.DayStepCLF.HasValue);
        //            if (stageDef != null)
        //                resultList.Add(new ClfItems() { IncomingItem = x, StageDefinition = stageDef });
        //        }
        //    );

        //    return resultList;
        //}

        public IEnumerable<CLFReportItem> FindItemsToResponse(IEnumerable<CLFReportItem> reportItems)
        {
            var reportItemsToResponse = new List<CLFReportItem>();
            foreach (var reportItem in reportItems)
            {
                var reportItemToResponse = GetDbSet<CLFReportItem>().Include(x => x.CLFReport).FirstOrDefault(y => y.CLFItemIdSource == reportItem.CLFItemId);
                if (reportItemToResponse != null)
                    reportItemsToResponse.Add(reportItemToResponse);
            }

            return reportItemsToResponse;
        }

        public IEnumerable<ClfItems> GetIncomingAndOutgoingItems(int reportStatus)
        {
            var result = from incomingItem in GetDbSet<CLFReportItem>().Include(x => x.CLFReport)
                         join outgoingItem in GetDbSet<CLFReportItem>().Include(x => x.CLFReport)
                         on incomingItem.CLFItemId equals outgoingItem.CLFItemIdSource
                         where incomingItem.CLFReportStatus == reportStatus && incomingItem.CLFItemIdSource == null
                         select new ClfItems { OutgoingItem = outgoingItem, IncomingItem = incomingItem };

            return result.ToList();
        }
    }
}
