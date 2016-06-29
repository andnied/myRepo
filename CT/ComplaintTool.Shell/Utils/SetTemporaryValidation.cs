using ComplaintTool.Shell.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common;
using ComplaintTool.Processing.TemporaryValidation;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.Utils
{
    [Cmdlet(VerbsCommon.Set,"TemporaryValidation")]
    public class SetTemporaryValidation:ComplaintCmdletBase
    {
        private ILogger Logger = LogManager.GetLogger();

        public override void Process()
        {
            try
            {
                var processor = new TemporaryValidationProcessor();
                processor.Process();
            }catch(Exception ex)
            {
                Logger.LogComplaintException(ex);
            }
        }
    }
}
