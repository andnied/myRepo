using ComplaintTool.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common;
using System.Data.Entity.SqlServer;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.DataAccess.Repos
{
    public class DictionaryRepo:RepositoryBase
    {
        private ILogger Logger = LogManager.GetLogger();
        public DictionaryRepo(DbContext dbContext):base(dbContext) { }

        public void UpdateCurrencyCodeTable()
        {
            var viewCodes = GetDbSet<View_CurrencyCode>().ToList();
            var convertedCodes = viewCodes.Select(x => new { Alphabetical = x.Alphabetical, DecimalPrecision = x.DecimalPrecision, Name = x.Name, Numeric=Convert.ToInt16(x.Numeric) });
            var results = from viewCode in convertedCodes
                          join tc in GetDbSet<CurrencyCode>() on viewCode.Numeric equals tc.Numeric into tableCode
                          from tabCode in tableCode.DefaultIfEmpty()
                          select new { ViewCode = viewCode, TableCode = tabCode };

            var usedNumerics = new List<short>();

            foreach(var codeContainer in results)
            {
                if (usedNumerics.Contains(codeContainer.TableCode.Numeric))
                    continue;
                var tableCode = codeContainer.TableCode;
                var viewCode = codeContainer.ViewCode;

                if(tableCode== null)
                {
                    Logger.LogComplaintException(new Exception(string.Format("Cannot find currency record in [Dictionary].CurrencyCode for numeric {0}", viewCode.Numeric)));
                    continue;
                }
                usedNumerics.Add(viewCode.Numeric);
                UpdateCurrencyCode(tableCode,viewCode.Alphabetical,viewCode.DecimalPrecision,viewCode.Name);
            }
        }

        private void UpdateCurrencyCode(CurrencyCode destinationCode, string alphabetical,int decimalPrec,string name)
        {
            destinationCode.Alphabetical = alphabetical;
            destinationCode.DecimalPrecision = decimalPrec;
            destinationCode.Name = name;
            destinationCode.InsertDate=DateTime.UtcNow;
            var currentUser = WindowsIdentity.GetCurrent();
            destinationCode.InsertUser=currentUser != null ? currentUser.Name : "ComplaintServices";
            Update(destinationCode);
        }
    }
}
