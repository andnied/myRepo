using ComplaintTool.Shell.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common;
using ComplaintTool.Common.Config;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.Utils
{
    [Cmdlet(VerbsCommon.Set,"Echo")]
    public class SetEcho:ComplaintCmdletBase
    {
        private ILogger Logger = LogManager.GetLogger();

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "On/Off echo string.")]
        public bool? IsEcho { get; set; }

        public override void Process()
        {
            try
            {
                if (IsEcho.HasValue)
                    ComplaintConfig.Instance.IsEcho = IsEcho.Value;

                if (IsWriteMode)
                    WriteObject(string.Format("Echo: {0}", (ComplaintConfig.Instance.IsEcho.HasValue && ComplaintConfig.Instance.IsEcho.Value) ? "On" : "Off"));
            }
            catch (Exception ex)
            {
                Logger.LogComplaintException(ex);
            }
        }
    }
}
