using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Visa;
using ComplaintTool.Visa.Outgoing;
using ComplaintTool.Shell.Common;
using ComplaintTool.Common;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.VISA
{
    [Cmdlet(VerbsCommon.Get, "VisaOutgoingFiles")]
    public class GetVisaOutgoingFiles : ComplaintCmdletBase
    {
        private readonly ILogger Logger = LogManager.GetLogger();

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "The base folder path where process saves outgoing files to.")]
        [Alias("Folder")]
        public string BaseFolderPath { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "The process type (extract/export).")]
        [Alias("Process Type")]
        public string Type { get; set; }

        public override void Process()
        {
            try
            {
                VisaOutgoingService visaOutgoingService = null;

                if (Type == "extract" || Type == "export")
                    visaOutgoingService = new VisaOutgoingService(this.BaseFolderPath, this.Type);
                else
                    throw new Exception("Invalid process type. Please choose between 'extract' and 'export'.");

                visaOutgoingService.Process();
            }
            catch (Exception ex)
            {
                Logger.LogComplaintException(ex);
            }
        }
    }
}
