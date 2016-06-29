using ComplaintTool.Shell.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common;
using ComplaintTool.Processing._3DSecure;
using ComplaintTool.Common.Enum;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.Utils
{
    [Cmdlet(VerbsCommon.Set, "MasterCard3DSecureStatus")]
    public class SetMasterCard3DSecureStatus : ComplaintCmdletBase
    {
        private readonly ILogger Logger = LogManager.GetLogger();

        [Parameter(
           Mandatory = true,
           ValueFromPipelineByPropertyName = true,
           ValueFromPipeline = true,
           Position = 0,
           HelpMessage = "Case ID.")]
        [Alias("CaseId")]
        public string CaseIdParam { get; set; }

        public override void Process()
        {
            try
            {
                var process = new Secure3DProcessor(Organization.MC);
                process.UpdateSecureAndComplaintRecord(CaseIdParam);
            }catch(Exception ex)
            {
                Logger.LogComplaintException(ex);
            }
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(CaseIdParam))
                throw new ArgumentException("CaseId is null or empty");
        }
    }
}
