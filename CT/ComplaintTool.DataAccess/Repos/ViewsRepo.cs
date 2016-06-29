using ComplaintTool.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.DataAccess.Repos
{
    public class ViewsRepo : RepositoryBase
    {
        public ViewsRepo(DbContext context)
            : base(context)
        { }

        public string GetPreviousTransactionAmount(long prevPostTranId)
        {
            var amt = GetDbSet<View_SELECTEDPOSTILIONDATA>().FirstOrDefault(x => x.post_tran_id == prevPostTranId).tran_amount_req;
            return amt.HasValue ? amt.ToString() : null;
        }

        public string GetCountryAlpha(string PAN)
        {
            object result;
            using (var cmd = base.GetContext<ComplaintEntities>().Database.Connection.CreateCommand())
            {
                cmd.Transaction = base.GetContext<ComplaintEntities>().Database.CurrentTransaction.UnderlyingTransaction;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter()
                {
                    Direction = System.Data.ParameterDirection.Input,
                    ParameterName = "@PAN",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Value = PAN
                });
                cmd.CommandText = "select country_alpha from [dbo].View_MCIPM_IP0040T1 where @PAN BETWEEN issuer_acct_range_low AND issuer_acct_range_high AND gcms_product_id != 'CIR'";
                result = cmd.ExecuteScalar();
            }

            return result.ToString();
        }
    }
}
