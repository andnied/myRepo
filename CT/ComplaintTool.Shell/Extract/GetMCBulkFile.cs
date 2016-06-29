using System;
using System.Management.Automation;
using ComplaintTool.Common;
using ComplaintTool.Common.Config;
using ComplaintTool.MCProImageInterface;
using ComplaintTool.Shell.Common;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.Extract
{
    [Cmdlet(VerbsCommon.Get, "MCBulkFile")]
    public class GetMCBulkFile : ComplaintCmdletBase
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
            HelpMessage = "ProcessKey of the extracted bulk file.")]
        [Alias("FileProcessKey")]
        public string ProcessKey { get; set; }

        public override void Process()
        {
            try
            {
                var processKey = Guid.Parse(ProcessKey);
                var packageService = new OutgoingPackageService(processKey, DestinationFolder);
                string fileName = packageService.GetBulkExtract();
                WriteComplaintObject(fileName);
            }
            catch (Exception ex)
            {
                _logger.LogComplaintException(ex);
            }
        }
    }
}
