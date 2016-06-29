using ComplaintTool.MasterCard;
using ComplaintTool.Common;
using ComplaintTool.MasterCard.Incoming;
using ComplaintTool.Shell.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.MasterCard
{
    [Cmdlet(VerbsCommon.New, "MasterCardRegistration")]
    public class NewMasterCardRegistration : ComplaintCmdletBase
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
                var incomingFile = new MasterCardFileRegistration(FilePath);
                var result = incomingFile.Process();

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
