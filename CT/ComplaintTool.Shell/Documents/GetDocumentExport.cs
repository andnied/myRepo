using ComplaintTool.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common;
using ComplaintTool.Shell.Common;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.Documents
{
    [Cmdlet(VerbsCommon.Get,"DocumentExport")]
    public class GetDocumentExport:ComplaintCmdletBase
    {
        private readonly ILogger Logger = LogManager.GetLogger();

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "The destination folder where merchants files will be saved.")]
        [Alias("MFolder")]
        public string MerchantFolder { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "The destination folder where documents files will be saved.")]
        [Alias("DFolder")]
        public string DocumentFolder { get; set; }


        public override void Process()
        {
            try
            {
                var service = new DocExportService(MerchantFolder,DocumentFolder);
                service.Process();
            }catch(Exception ex)
            {
                Logger.LogComplaintException(ex);
            }
        }
    }
}
