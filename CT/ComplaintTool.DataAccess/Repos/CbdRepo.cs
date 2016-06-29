using System.Data.Entity;
using System.Linq;
using ComplaintTool.Models;

namespace ComplaintTool.DataAccess.Repos
{
    public class CbdRepo : RepositoryBase
    {
        public CbdRepo(DbContext context)
            : base(context)
        {

        }

        public string GetMerchantNameByMID(string mid)
        {
            return GetDbSet<View_CBT_KLIENCI>().Single(x => x.KLN_MID == mid).KLN_NAZWA;
        }
    }
}
