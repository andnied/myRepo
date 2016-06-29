using System;
using System.Management.Automation;
using ComplaintTool.Common.CTLogger;
using ComplaintTool.Processing.IncomingRecords;
using ComplaintTool.Shell.Common;

namespace ComplaintTool.Shell.Utils
{
    [Cmdlet(VerbsCommon.Find, "IncomingRecordsInFile")]
    public class FindIncomingRecordsInFile : ComplaintCmdletBase
    {
        private readonly ILogger _logger = LogManager.GetLogger();

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "The name of verified file.")]
        [Alias("File")]
        public string FileName { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 1,
            HelpMessage = "Path of the result file")]
        [Alias("ResultFilePath")]
        public string ResultPath { get; set; }

        public override void Process()
        {
            try
            {
                var incomingProcessor = new IncomingRecordsProcessor(FileName, ResultPath);
                incomingProcessor.Process();
            }
            catch (Exception e)
            {
                _logger.LogComplaintException(e);
            }
        }
    }
}
