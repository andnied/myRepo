using System;
using System.Management.Automation;
using ComplaintTool.Common;
using ComplaintTool.Common.Config;
using ComplaintTool.Models;
using ComplaintTool.MCProImageInterface;
using ComplaintTool.Shell.Common;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.Extract
{
    [Cmdlet(VerbsCommon.New, "MCBulkFile")]
    public class NewMCBulkFile : ComplaintCmdletBase
    {
        private static readonly ILogger _logger = LogManager.GetLogger();

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "The destination folder where the exctract file will be saved.")]
        [Alias("Folder")]
        public string DestinationFolder { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 1,
            HelpMessage = "Endpoint name for which will be generated bulk extract.")]
        [Alias("Endpoint")]
        public string EndpointName { get; set; }

        public override void Process()
        {
            try
            {
                var packageService = new OutgoingPackageService(DestinationFolder);
                var fileName = packageService.NewBulkExtract(EndpointName);
                WriteComplaintObject(fileName);
            }
            catch (Exception ex)
            {
                _logger.LogComplaintException(ex);
            }
        }
    }
}
