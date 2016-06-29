using System.Data.Entity;
using System.Linq;
using ComplaintTool.Models;
using System.Collections.Generic;

namespace ComplaintTool.DataAccess.Repos
{
    public class CaseFilingRepo : RepositoryBase
    {
        public CaseFilingRepo(DbContext context) 
            : base(context)
        {
        }

        public CaseFilingRecord GetRecordById(long id)
        {
            return GetDbSet<CaseFilingRecord>().Single(x => x.CaseFilingRecordId == id);
        }

        public CaseFilingRecord FindRecordByCaseAndStageId(string caseId, long stageId)
        {
            return GetDbSet<CaseFilingRecord>()
                .Include(x => x.Complaint)
                .SingleOrDefault(x => x.CaseId == caseId && x.StageId == stageId);
        }

        public void AddCaseFilingRecord(CaseFilingRecord record, CaseFilingIncomingFile file)
        {
            if (record.CaseId != file.CaseId) // moze lepiej bedzie rzucic wyjatkiem
                record.CaseId = file.CaseId;
            if (record.StageId != file.StageId)
                record.StageId = file.StageId;
            record.CaseFilingIncomingFile = file;
            record.InsertDate = GetCurrentDateTime();
            record.InsertUser = GetCurrentUser();
            GetDbSet<CaseFilingRecord>().Add(record);
            Commit();
        }

        public void AddCaseFilingRecord(CaseFilingRecord record, CaseFilingOutgoingFile file)
        {
            if (record.CaseId != file.CaseId) // moze lepiej bedzie rzucic wyjatkiem
                record.CaseId = file.CaseId;
            if (record.StageId != file.StageId)
                record.StageId = file.StageId;
            record.CaseFilingOutgoingFile = file;
            record.InsertDate = GetCurrentDateTime();
            record.InsertUser = GetCurrentUser();
            GetDbSet<CaseFilingRecord>().Add(record);
            Commit();
        }

        public IEnumerable<Complaint> FindComplaintsByMasterCardCaseId(string mcCaseId)
        {
            return GetDbSet<CaseFilingRecord>()
                .Where(x => x.MasterCardCaseID == mcCaseId)
                .Select(x => x.Complaint)
                .ToList();
        }

        public bool CheckIfExistsAnyComplaintWithMasterCardCaseId(string mcCaseId)
        {
            return GetDbSet<CaseFilingRecord>().Any(x => x.MasterCardCaseID == mcCaseId);
        }
    }
}
