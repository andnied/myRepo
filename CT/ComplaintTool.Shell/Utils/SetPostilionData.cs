using ComplaintTool.Shell.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common;
using ComplaintTool.Processing.Postilion;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.Utils
{
    [Cmdlet(VerbsCommon.Set, "PostilionData")]
    public class SetPostilionData:ComplaintCmdletBase
    {
        private ILogger Logger = LogManager.GetLogger();

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Case ID")]
        [Alias("CaseId")]
        public string CaseId { get; set; }

        public override void Process()
        {
            try
            {
                var processor = new PostilionDataProcessor();
                processor.Process(CaseId);
            }catch(Exception ex)
            {
                Logger.LogComplaintException(ex);
            }
        }
    }
}
