using ComplaintTool.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.DataAccess.Repos
{
    public class ParserKeyRepo : RepositoryBase
    {
        public ParserKeyRepo(DbContext dbContext):base(dbContext)
        {
        }

        public IEnumerable<ParserKey> GetKeys(string paramName, string organizationId)
        {
            return GetDbSet<ParserKey>().Where(x => x.Key == paramName && x.OrganizationId == organizationId).AsEnumerable<ParserKey>();
        }
    }
}
