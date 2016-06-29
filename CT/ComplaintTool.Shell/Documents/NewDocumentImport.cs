using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Documents;
using ComplaintTool.Common.Enum;
using ComplaintTool.Common;
using ComplaintTool.Shell.Common;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.Documents
{
    [Cmdlet(VerbsCommon.New, "DocumentImport")]
    public class NewDocumentImport : ComplaintCmdletBase
    {
        private readonly ILogger Logger = LogManager.GetLogger();
        private const string ARNParam = "ARN";

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Path to the incoming file.")]
        [Alias("File")]
        public string FilePath { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Payment Organization shortcut")]
        [Alias("Organization")]
        public string OrganizationId { get; set; }

        public override void Process()
        {
            try
            {
                if (!ValidateParam())
                    return;

                var docService = DocImportService.GetService(OrganizationId.ToUpper());
                docService.LoadKeysForParam(ARNParam);

                docService.Process(FilePath);

                if (base.IsWriteMode)
                    WriteObject(0);
            }
            catch(Exception ex)
            {
                Logger.LogComplaintException(ex);

                if (base.IsWriteMode)
                    WriteObject(-1);
            }
        }

        private bool ValidateParam()
        {
            if (!File.Exists(this.FilePath))
                throw new ArgumentException("File does not exists");

            if (!Organization.MC.ToString().Equals(OrganizationId) && !Organization.VISA.ToString().Equals(OrganizationId))
                throw new ArgumentException("Invalid Payment Organization");

            return true;
        }
    }
}
