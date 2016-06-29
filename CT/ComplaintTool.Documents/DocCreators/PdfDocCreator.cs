using ComplaintTool.Common;
using ComplaintTool.Common.Config;
using Microsoft.Reporting.WebForms;
using System;
using System.Globalization;

namespace ComplaintTool.Documents.DocCreators
{
    public class PdfDocCreator:IDocCreator
    {
        private ReportViewer _reportViewer;

        public string Extension
        {
            get { return "pdf"; }
        }

        public void LoadConfiguration()
        {
            var serviceUrl=ComplaintConfig.GetParameter(Globals.ReportsServicesUrlParam);
            var templateDictionary = ComplaintConfig.GetParameter(Globals.ReportsTemplateDictionaryParam);
            var reportTemplateName = ComplaintConfig.GetParameter(Globals.ReportsTemplateNameParam);

            _reportViewer = new ReportViewer { ProcessingMode = ProcessingMode.Remote };
            var serverReport = _reportViewer.ServerReport;
            serverReport.ReportServerUrl = new Uri(serviceUrl);
            serverReport.ReportPath = string.Format(@"/{0}/{1}", templateDictionary, reportTemplateName);
            serverReport.Timeout = GetTimeout();
        }

        public byte[] CreateDocument(string caseId, long stageId)
        {
            const string deviceInfo = @"<DeviceInfo><OutputFormat>PDF</OutputFormat></DeviceInfo>";
            var caseIdParameter = new ReportParameter { Name = "CaseId" };
            caseIdParameter.Values.Add(caseId);
            var stageIdParameter = new ReportParameter { Name = "StageId" };
            stageIdParameter.Values.Add(stageId.ToString(CultureInfo.InvariantCulture));

            _reportViewer.ServerReport.SetParameters( new[] { caseIdParameter, stageIdParameter });
            return _reportViewer.ServerReport.Render("PDF",deviceInfo);
        }

        private static int GetTimeout()
        {
            int time;
            var timeoutParam = ComplaintConfig.GetParameter(Globals.ReportsServicesTimeoutParam);

            if (int.TryParse(timeoutParam, out time))
                return time * 1000;

            return 100000;
        }
    }
}
