using ComplaintTool.Processing.SupporitngDocumentsVerifivation;
using ComplaintTool.Shell.Common;
using System;
using System.Management.Automation;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.Utils
{
    [Cmdlet(VerbsCommon.Set, "SupportingDocumentVerification")]
    public class SetSupportingDocumentVerification:ComplaintCmdletBase
    {
        private readonly ILogger _logger = LogManager.GetLogger();
        public override void Process()
        {
            try
            {
                var processor = new SupportingDocumentsProcessor();
                processor.Process();
            }catch(Exception ex)
            {
                _logger.LogComplaintException(ex);
            }
        }
    }
}
