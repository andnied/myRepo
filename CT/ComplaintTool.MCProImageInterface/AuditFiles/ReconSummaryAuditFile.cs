using System;
using System.IO;
using System.Text;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.MCProImageInterface.AuditFiles
{
    class ReconSummaryAuditFile : AuditFileBase
    {
        private readonly ILogger _logger = LogManager.GetLogger();

        public ReconSummaryAuditFile(string filePath) 
            : base(filePath)
        {
        }

        public override bool Parse()
        {
            var endPointNumber = string.Empty;
            var emailMessage = new StringBuilder("");
            for (var i = 0; i < _lines.Length; i++)
            {
                var line = _lines[i].Trim();
                if (line.Length <= 0) continue;

                if (line.Contains("GFT Endpoint :"))
                {
                    var startNumber = line.IndexOf(":", StringComparison.Ordinal) + 1;
                    if (startNumber < line.Length) endPointNumber = line.Substring(startNumber, line.Length - startNumber).Trim();
                }
                emailMessage.AppendFormat("{0}{1}", line, i < _lines.Length - 1 ? "," : "");
            }
            if (string.IsNullOrEmpty(FilePath)) return false;
            var fileName = Path.GetFileName(FilePath);
            var dateString = string.Format("{0}-{1}-{2}", fileName.Substring(19, 2), fileName.Substring(17, 2), fileName.Substring(15, 2));
            _logger.LogComplaintEvent(563, endPointNumber, dateString + "\n\n" + emailMessage);

            return true;
        }
    }
}
