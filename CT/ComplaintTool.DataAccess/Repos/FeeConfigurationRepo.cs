using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FeeConfiguration = ComplaintTool.Models.FeeConfiguration;
using Organization = ComplaintTool.Common.Enum.Organization;

namespace ComplaintTool.DataAccess.Repos
{
    public class FeeConfigurationRepo : RepositoryBase
    {
        public FeeConfigurationRepo(DbContext context)
            : base(context)
        {

        }

        public List<FeeConfiguration> GetFeeConfiguration(Organization organization, string countryCode, int feeTypeId)
        {
            return GetDbSet<FeeConfiguration>().Where(
                    x => x.CountryCode.Equals(countryCode) && x.OrganizationId.Equals(Organization.MC.ToString())
                        && x.FeeTypeId.Equals(feeTypeId)).ToList();
        }
    }
}
