using ComplaintTool.Postilion.Incoming;
using ComplaintTool.Common;
using ComplaintTool.Shell.Common;
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
    [Cmdlet(VerbsCommon.New, "PostilionProcessingFile")]
    public class NewPostilionProcessingFiles : ComplaintCmdletBase
    {
        private static readonly ILogger _logger = LogManager.GetLogger();

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "File Id.")]
        [Alias("FileId")]
        public long FileId { get; set; }

        public override void Process()
        {
            try
            {
                var incomingService = PostilionIncomingBase.GetService("processing", fileId: FileId);
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
