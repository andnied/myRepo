using ComplaintTool.Postilion;
using ComplaintTool.Shell.Common;
using ComplaintTool.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.Postilion
{
    [Cmdlet(VerbsCommon.Get, "PostilionExtractedFiles")]
    public class GetPostilionExtractedFiles : ComplaintCmdletBase
    {
        private static readonly ILogger _logger = LogManager.GetLogger();

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "The base folder path where files are being processed.")]
        [Alias("Folder")]
        public string BaseFolderPath { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Organization (MC/VISA).")]
        [Alias("Folder")]
        public string Organization { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Extract type (representment/feecollection).")]
        [Alias("Folder")]
        public string ExtractType { get; set; }

        public override void Process()
        {
            try
            {
                ComplaintTool.Common.Enum.Organization org;
                ComplaintTool.Common.Enum.PostilionServiceEnum type;

                if (Organization.ToUpper() == "VISA")
                    org = ComplaintTool.Common.Enum.Organization.VISA;
                else if (Organization.ToUpper() == "MC")
                    org = ComplaintTool.Common.Enum.Organization.MC;
                else
                    throw new Exception("Invalid organization type. Expected MC/VISA.");

                if (ExtractType.ToLower() == "representment")
                    type = ComplaintTool.Common.Enum.PostilionServiceEnum.Representment;
                else if (ExtractType.ToLower() == "feecollection")
                    type = ComplaintTool.Common.Enum.PostilionServiceEnum.FeeCollection;
                else
                    throw new Exception("Invalid extract type. Expected representment/feecollection.");

                var postilionService = new PostilionOutgoingService(BaseFolderPath, org, type);
                postilionService.CreateExtract();
            }
            catch (Exception ex)
            {
                _logger.LogComplaintException(ex);
            }
        }
    }
}
