using ComplaintTool.Shell.Common;
using ComplaintTool.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Visa;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.VISA
{
    [Cmdlet(VerbsCommon.New, "VisaIncomingProcessing")]
    public class NewVisaIncomingProcessing : ComplaintCmdletBase
    {
        private static readonly ILogger Logger = LogManager.GetLogger();

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Path to the incoming file.")]
        [Alias("File")]
        public int FileId { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "ARN")]
        [Alias("ARN")]
        public string Arn { get; set; }
        
        public override void Process()
        {
            try
            {
                var incomingFile = VisaIncomingBase.GetService("processing", fileId: FileId, arn: Arn);
                var result = incomingFile.Process();

                if (IsWriteMode)
                    WriteObject(result);
            }
            catch (Exception ex)
            {
                Logger.LogComplaintException(ex);

                if (IsWriteMode)
                    WriteObject(-1);
            }
        }
    }
}
