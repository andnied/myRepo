using ComplaintTool.Shell.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common;
using ComplaintTool.Processing._3DSecure;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.Utils
{
    [Cmdlet(VerbsCommon.Set, "3DSecureValidation")]
    public class Set3DSecureValidation:ComplaintCmdletBase
    {
        private readonly ILogger Logger = LogManager.GetLogger();
        
        public override void Process()
        {
            try
            {
                var process = new Secure3DProcessor(ComplaintTool.Common.Enum.Organization.MC);
                process.ValidateAndUpdate3dSecures();
                WriteComplaintObject("MasterCard 3DSecure validation completed");

                process = new Secure3DProcessor(ComplaintTool.Common.Enum.Organization.VISA);
                process.ValidateAndUpdate3dSecures();
                WriteComplaintObject("Visa 3DSecure validation completed");
            }catch(Exception ex)
            {
                Logger.LogComplaintException(ex);
            }
        }
    }
}
