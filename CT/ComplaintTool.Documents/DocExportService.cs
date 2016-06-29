using ComplaintTool.Common;
using ComplaintTool.Common.CTLogger;
using ComplaintTool.Documents.Exporters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Documents
{
    public class DocExportService
    {
        private static readonly ILogger Logger = LogManager.GetLogger();
        private string _processName;
        private List<IExporter> _exporters; 

        public DocExportService(string merchantFolder,string documentFolder)
        {
            _exporters = new List<IExporter>() { new MerchantExporter(merchantFolder), new DocumentExporter(documentFolder,true) };
        }

        public void Process()
        {
            foreach (var exporter in _exporters)
            {
                _processName = exporter.ProcessName;
                exporter.Export();
            }
        }
    }
}
