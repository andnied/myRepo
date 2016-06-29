using ComplaintTool.Processing.CloseCases;
using ComplaintTool.Shell.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.Utils
{
    [Cmdlet(VerbsCommon.Switch, "CasesToClose")]
    public class SwitchCasesToClose:ComplaintCmdletBase
    {
        ILogger Logger = LogManager.GetLogger();

        public override void Process()
        {
            try
            {
                var processor = new CloseCasesProcessor();
                processor.Process();
            }catch(Exception ex)
            {
                Logger.LogComplaintException(ex);
            }
        }
    }
}
