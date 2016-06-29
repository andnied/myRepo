using ComplaintTool.Common.Config;
using ComplaintTool.Shell.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.Administration
{
    [Cmdlet(VerbsCommon.Show,"ConfigFile")]
    public class ShowConfigFile:ComplaintCmdletBase
    {
        private static ILogger Logger = LogManager.GetLogger();

        public override void Process()
        {
            try
            {
                if (IsWriteMode)
                {
                    WriteObject(ComplaintConfig.ReadConfigAsText());
                }
            }catch(Exception ex)
            {
                Logger.LogComplaintException(ex);
            }
        }
    }
}
