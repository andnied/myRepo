using System.Data.Entity;
using System.Linq;
using ComplaintTool.Models;

namespace ComplaintTool.DataAccess.Repos
{
    public class GoodFaithLetterRepo : RepositoryBase
    {
        public GoodFaithLetterRepo(DbContext context) : base(context)
        {
        }

        public GoodFaithLetter GetGflByCaseId(string caseId)
        {
            return GetDbSet<GoodFaithLetter>().FirstOrDefault(x => x.CaseId.Equals(caseId));
        }

        public bool IsGfl(string caseId)
        {
            return GetDbSet<GoodFaithLetter>().Any(x => x.CaseId.Equals(caseId));
        }

        public void AddGoodFaithLetterStage(GoodFaithLetterStage goodFaithLetterStage)
        {
            goodFaithLetterStage.InsertDate = GetCurrentDateTime();
            goodFaithLetterStage.InsertUser = GetCurrentUser();
            GetDbSet<GoodFaithLetterStage>().Add(goodFaithLetterStage);
        }

        public GoodFaithLetterStage GetGoodFaithLetterStage(long stageId)
        {
            return GetDbSet<GoodFaithLetterStage>().FirstOrDefault(x => x.StageId == stageId);
        }

        public GoodFaithLetterValue GetGoodFaithLetterValue(long valueId)
        {
            return GetDbSet<GoodFaithLetterValue>().FirstOrDefault(x => x.ValueId == valueId);
        }

        public GoodFaithLetterRecord GetGoodFaithLetterRecord(long recordId)
        {
            return GetDbSet<GoodFaithLetterRecord>().FirstOrDefault(x => x.RecordId == recordId);
        }
    }
}
