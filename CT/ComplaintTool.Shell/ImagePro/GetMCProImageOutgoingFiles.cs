using System;
using System.Management.Automation;
using ComplaintTool.Common;
using ComplaintTool.Common.Extensions;
using ComplaintTool.MCProImageInterface;
using ComplaintTool.Shell.Common;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.ImagePro
{
    [Cmdlet(VerbsCommon.Get, "MCProImageOutgoingFiles")]
    public class GetMCProImageOutgoingFiles : ComplaintCmdletBase
    {
        public const string CaseFiling = "CaseFiling";
        public const string RRF = "RRF";
        public const string Chargeback = "Chargeback";
        private static readonly ILogger _logger = LogManager.GetLogger();

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "The base folder path where process save outgoing files.")]
        [Alias("Folder")]
        public string TempFolderPath { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 1,
            HelpMessage = "The process name to execute.")]
        [Alias("Process")]
        [ValidateSet(new string[] { CaseFiling, RRF, Chargeback })]
        public string ProcessName { get; set; }

        public override void Process()
        {
            try
            {
                var outgoingService = new OutgoingFileService(TempFolderPath);
                if (this.ProcessName.IsEmpty())
                    this.ProcessName = Chargeback;

                switch (this.ProcessName)
                {
                    case RRF:
                        throw new NotSupportedException();
                    //outgoingService.ExtractRrfDocuments();
                    //break;
                    case Chargeback:
                        outgoingService.ExtractRepresentmentDocuments();
                        break;
                    case CaseFiling:
                        outgoingService.ExtractCaseFiling("ESPLCB20150302100013", "FICN");
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogComplaintException(ex);
            }
        }
    }
}
