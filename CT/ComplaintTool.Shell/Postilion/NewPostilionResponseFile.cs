using ComplaintTool.Postilion.Incoming;
using ComplaintTool.Shell.Common;
using ComplaintTool.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.Postilion
{
    [Cmdlet(VerbsCommon.New, "PostilionResponseFile")]
    public class NewPostilionResponseFile : ComplaintCmdletBase
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
                var incomingService = PostilionIncomingBase.GetService("response", FilePath);
                var result = incomingService.Process();

                if (base.IsWriteMode)
                    WriteObject(result);
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
