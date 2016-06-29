using ComplaintTool.MasterCard.Incoming;
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

namespace ComplaintTool.Shell.MasterCard
{
    [Cmdlet(VerbsCommon.Get, "MasterCardProcessing")]
    public class GetMasterCardProcessing : ComplaintCmdletBase
    {
        private static readonly ILogger _logger = LogManager.GetLogger();

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "File Id.")]
        public int FileId { get; set; }

        public override void Process()
        {
            try
            {
                var process = new MasterCardFileProcessing(FileId, @"C:\Users\Default\AppData\Local\Temp");
                var result = process.Process();
                
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
