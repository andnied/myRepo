using BazaMvp.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaMvp.DataAccess.Repos
{
    public class HelperRepo : RepositoryBase
    {
        public HelperRepo(DbContext context)
            : base(context)
        { }

        public IEnumerable<VisaImportRule> GetVisaImportRules()
        {
            return GetDbSet<VisaImportRule>().ToList();
        }

        public IEnumerable<string> GetCurrencyCodes()
        {
            return GetDbSet<CurrencyCode>().Select(c => c.Code).ToList();
        }

        public IEnumerable<CryteriaBase> GetCryteria()
        {
            return GetDbSet<CryteriaBase>().ToList();
        }

        public void UpdateCryteria(CryteriaBase cryteria)
        {
            base.Update(cryteria);
        }
    }
}
