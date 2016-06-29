using System.Data.Entity;
using System.Linq;
using ComplaintTool.Models;

namespace ComplaintTool.DataAccess.Repos
{
    public class BINListRepo : RepositoryBase
    {
        public BINListRepo(DbContext context)
            : base(context)
        {
        }

        public BINList FindBINListByBin(string bin)
        {
            return GetDbSet<BINList>().FirstOrDefault(b => b.BIN == bin);
        }

        public BINList FindBINListByBin(string bin, bool productionStatus)
        {
            return GetDbSet<BINList>().FirstOrDefault(b => b.BIN == bin && b.ProductionStatus == productionStatus);
        }

        public string GetSourceCountryCodeByBin(string bin)
        {
            var binList = FindBINListByBin(bin);
            return binList != null ? binList.SourceCountryCode : null;
        }

        public string GetParticipantId(string bin)
        {
            var binList = FindBINListByBin(bin);
            return binList != null ? binList.ParticipantId : null;
        }
    }
}
