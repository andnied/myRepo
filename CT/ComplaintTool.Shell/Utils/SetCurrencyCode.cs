using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.Shell.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common;
using System.Management.Automation;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.Utils
{
    [Cmdlet(VerbsCommon.Set, "CurrencyCode")]
    public class SetCurrencyCode:ComplaintCmdletBase
    {
        private ILogger Logger = LogManager.GetLogger();

        public override void Process()
        {
            try
            {
                using (var unitOfWork = ComplaintUnitOfWork.Create())
                {
                    //unitOfWork.Repo<DictionaryRepo>().UpdateCurrencyCodeTable();
                    unitOfWork.Commit();
                }
            }catch(Exception ex)
            {
                Logger.LogComplaintException(ex);
            }
        }
    }
}
