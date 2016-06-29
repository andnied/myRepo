using ComplaintTool.EVOGermany;
using ComplaintTool.Shell.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.EvoGermany
{
    [Cmdlet(VerbsCommon.Get, "EvoGermanyOutgoingFiles")]
    public class GetEvoGermanyOutgoingFiles:ComplaintCmdletBase
    {
        private readonly ILogger Logger = LogManager.GetLogger();

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "The destination folder where outgoing file will be saved.")]
        [Alias("Folder")]
        public string DestinationFolder { get; set; }

        public override void Process()
        {
            try
            {
                var service = new EVOGermanyOutgoingService(DestinationFolder);
                service.Proccess();
                base.ProcessRecord();
            }catch(Exception ex)
            {
                Logger.LogComplaintException(ex);
            }
        }
    }
}
