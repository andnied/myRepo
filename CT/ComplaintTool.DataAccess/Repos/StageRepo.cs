using ComplaintTool.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.DataAccess.Repos
{
    public class StageRepo:RepositoryBase
    {
        public StageRepo(DbContext dbContext) : base(dbContext) { }

        public void AddComplaintStageDocument(ComplaintStageDocument complaintStageDocument)
        {
            GetDbSet<ComplaintStageDocument>().Add(complaintStageDocument);
        }

        public void AddComplaintStage(ComplaintStage complaintStage)
        {
            GetDbSet<ComplaintStage>().Add(complaintStage);
        }

        public ComplaintStage FindComplaintStage(string stageCode)
        {
            return GetDbSet<ComplaintStage>().FirstOrDefault(x => x.StageCode == stageCode);
        }

        public ComplaintStage FindLastComplaintStage(string stageCode,string caseId)
        {
            return GetDbSet<ComplaintStage>().Where(x=>x.CaseId==caseId).OrderByDescending(x=>x.StageId).FirstOrDefault(x => x.StageCode == stageCode);
        }

        public ComplaintStage FindLastComplaintStage(string caseId)
        {
            return GetDbSet<ComplaintStage>().Where(x => x.CaseId == caseId).OrderByDescending(x => x.StageId).FirstOrDefault();
        }

        public ComplaintStage FindLastComplaintStage(string stageCode, string caseId,string [] reasonCodes)
        {
            return GetDbSet<ComplaintStage>().Where(x => x.CaseId == caseId).OrderByDescending(x => x.StageId).FirstOrDefault(x => x.StageCode == stageCode&&reasonCodes.Contains(x.ReasonCode));
        }

        public IEnumerable<ComplaintStage> FindComplaintStages(string caseId, string stageCode,string reasonCode)
        {
            return GetDbSet<ComplaintStage>().Where(x => x.CaseId == caseId && x.StageCode == stageCode && x.ReasonCode == reasonCode).ToList();
        }

        public bool ExistComplaintStages(string caseId, string stageCode,string reasonCode)
        {
            return GetDbSet<ComplaintStage>().Any(x => x.CaseId == caseId && x.StageCode == stageCode && x.ReasonCode == reasonCode);
        }

        //public IEnumerable<ComplaintStage> GetStagesForCloseComplaintsWithoucClf()
        //{
        //     IQueryable<ComplaintStage> complaintStages= GetDbSet<ComplaintStage>().Include(x => x.Complaint).Include(x=>x.Complaint.ComplaintValues)
        //        .Include(x=>x.Complaint.ComplaintStages).Include(x=>x.Complaint.ComplaintRecords)
        //        .Where(x => x.Complaint.Close.HasValue && !x.Complaint.Close.Value);

        //     IQueryable<CLFReportItem> clfReportItems = GetDbSet<CLFReportItem>();
        //     List<ComplaintStage> stagesWithoutClf = complaintStages.Where(x => clfReportItems.Select(y => y.Complaint.CaseId).Contains(x.Complaint.CaseId)).ToList();
        //      return stagesWithoutClf;
        //}

        public StageDefinition FindStageDefinition(string stageCode, bool isManual)
        {
            return GetDbSet<StageDefinition>()
                .FirstOrDefault(x => x.StageCode.Equals(stageCode) && x.Manual == isManual && x.IsActive);
        }

        public InternalStageDefinition FindInternalStageDefinition(string internalStageCode)
        {
            return
                GetDbSet<InternalStageDefinition>()
                    .FirstOrDefault(x => x.InternalStageCode.Equals(internalStageCode) && x.IsActive);
        }

    }
}
