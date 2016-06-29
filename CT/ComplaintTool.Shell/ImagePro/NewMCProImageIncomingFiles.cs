using System;
using System.IO;
using System.Management.Automation;
using ComplaintTool.Common;
using ComplaintTool.MCProImageInterface;
using ComplaintTool.Shell.Common;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.ImagePro
{
    [Cmdlet(VerbsCommon.New, "MCProImageIncomingFile")]
    public class NewMCProImageIncomingFiles : ComplaintCmdletBase
    {
        private static readonly ILogger _logger = LogManager.GetLogger();

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Path to the incoming file.")]
        [Alias("File")]
        public string FilePath { get; set; }

        public override void Process()
        {
            try
            {
                var incomingFile = new IncomingFileService(FilePath);
                if (incomingFile.Process())
                {
                    if (base.IsWriteMode)
                        WriteObject(0);
                }
                else
                {
                    if (base.IsWriteMode)
                        WriteObject(-1);
                }
            }
            catch (Exception ex)
            {
                _logger.LogComplaintException(ex);

                if (base.IsWriteMode)
                    WriteObject(-1);
            }
        }
    }
}
