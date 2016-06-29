using ComplaintTool.Common;
using ComplaintTool.Common.Config;
using ComplaintTool.Common.CTLogger;
using ComplaintTool.Common.Enum;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.EVOGermany.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.EVOGermany
{
    public class EVOGermanyOutgoingService : IComplaintProcess
    {
        private ILogger Logger = LogManager.GetLogger();
        private string _baseFolder;

        public EVOGermanyOutgoingService(string baseFolder)
        {
            _baseFolder = baseFolder;
        }

        public string OrganizationId
        {
            get { throw new NotImplementedException(); }
        }

        public string ProcessName
        {
            get { return Globals.EvoGermanyOutgoingService; }
        }

        public string ProcessFilePath
        {
            get { return _baseFolder; }
        }

        public string FilePath
        {
            get { return _baseFolder; }
        }

        public void Proccess()
        {
            var crbReportGenerator = new CrbReportGenerator(ProcessFilePath);
            crbReportGenerator.Generate();

            IEnumerable<CLFReport> reports;
            using (var unitOfWork = ComplaintUnitOfWork.Create())
            {
                reports = unitOfWork.Repo<CLFRepo>().FindReportsByStatus(ClfFileStatus.Imported);
            }

            foreach (var report in reports)
            {
                try
                {
                    var reportGenerator = new ClfReportGenerator(report, FilePath);
                    reportGenerator.Generate();
                }catch(Exception ex)
                {
                    Logger.LogComplaintException(ex);
                }
            }
        }
    }
}
