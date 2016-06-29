using ComplaintTool.Shell.Common;
using System;
using System.Management.Automation;
using ComplaintTool.Processing.AutoRepresentmentValidation;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.Utils
{
    [Cmdlet(VerbsCommon.Set, "AutoRepresentmentValidation")]
    public class SetAutoRepresentmentValidation : ComplaintCmdletBase
    {
        private readonly ILogger _logger = LogManager.GetLogger();
        public override void Process()
        {
            try
            {
                var processor = new AutoRepresentmentProcessor();
                processor.Process();
            }catch(Exception ex)
            {
                _logger.LogComplaintException(ex);
            }
        }
    }
}
