using System.Data.Entity;
using System.Linq;
using ComplaintTool.Models;

namespace ComplaintTool.DataAccess.Repos
{
    public class RecoveryCardRepo : RepositoryBase
    {
        public RecoveryCardRepo(DbContext context) : base(context)
        {
        }

        public RecoveryCard GetRecoveryCard(string caseId)
        {
            return GetDbSet<RecoveryCard>().FirstOrDefault(x => x.CaseId.Equals(caseId));
        }

        public bool IsRecoveryCard(string caseId)
        {
            return GetDbSet<RecoveryCard>().Any(x => x.CaseId.Equals(caseId));
        }

        public void AddRecoveryCardStage(RecoveryCardStage recoveryCardStage)
        {
            recoveryCardStage.InsertDate = GetCurrentDateTime();
            recoveryCardStage.InsertUser = GetCurrentUser();
            GetDbSet<RecoveryCardStage>().Add(recoveryCardStage);
        }

        public RecoveryCardList GetRecoveryCardList(string caseId)
        {
            return GetDbSet<RecoveryCardList>().FirstOrDefault(x => x.CaseId.Equals(caseId));
        }

        public RecoveryCardStage GetRecoveryCardStage(long stageId)
        {
            return GetDbSet<RecoveryCardStage>().FirstOrDefault(x => x.StageId == stageId);
        }

        public RecoveryCardValue GetRecoveryCardValue(long valueId)
        {
            return GetDbSet<RecoveryCardValue>().FirstOrDefault(x => x.ValueId == valueId);
        }

        public RecoveryCardRecord GetRecoveryCardRecord(long recordId)
        {
            return GetDbSet<RecoveryCardRecord>().FirstOrDefault(x => x.RecordId == recordId);
        }
    }
}
