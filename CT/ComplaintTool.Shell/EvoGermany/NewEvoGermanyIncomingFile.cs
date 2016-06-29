using ComplaintTool.Shell.Common;
using ComplaintTool.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.EVOGermany;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.EvoGermany
{
    [Cmdlet(VerbsCommon.New, "EvoGermanyIncomingFile")]
    public class NewEvoGermanyIncomingFile : ComplaintCmdletBase
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

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Incoming file type (pdf/clf).")]
        [Alias("Folder")]
        public string FileType { get; set; }
        
        public override void Process()
        {
            try
            {
                string type = FileType.ToLower() == "pdf" ? "PDF" : FileType.ToLower() == "clf" ? "CLF" : null;

                if (type == null)
                    throw new Exception("Incorrect EvoGermany incoming file type. Expected pdf/clf.");

                var evoGermanyIncomingService = new EVOGermanyIncomingService(FilePath, type);
                var result = evoGermanyIncomingService.ProcessFile();

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
