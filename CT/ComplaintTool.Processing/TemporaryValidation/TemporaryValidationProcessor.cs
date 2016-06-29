using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.Processing.TemporaryValidation.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Processing.TemporaryValidation
{
    public class TemporaryValidationProcessor
    {
        private ILogger Logger = LogManager.GetLogger();

        public void Process()
        {
            List<string> names = new List<string>();
            using(var unitOfWork=ComplaintUnitOfWork.Create())
            {
                foreach(var org in Enum.GetValues(typeof(Common.Enum.Organization)))
                {
                    var organization = GetOrganization(org.ToString(), unitOfWork);
                    var fileNames = organization.GetFilesNames((int)ComplaintTool.Common.Enum.ProcessingStatus.New, (int)ComplaintTool.Common.Enum.ProcessingStatus.ManualProcess);
                    names.AddRange(fileNames);
                }
                var statistics = names.GroupBy(x => x).Select(x => new { FileName = x.Key, Amount = x.Count() });
                statistics.ToList().ForEach(x => Logger.LogComplaintEvent(116, x.FileName, x.Amount));
                unitOfWork.Commit();
            }
        }

        private IOrganization GetOrganization(string organizationId, ComplaintUnitOfWork unitOfWork)
        {
            if (ComplaintTool.Common.Enum.Organization.MC.ToString().Equals(organizationId))
                return new McOrganization(unitOfWork);
            if (ComplaintTool.Common.Enum.Organization.VISA.ToString().Equals(organizationId))
                return new VisaOrganization(unitOfWork);

            throw new Exception(string.Format("Cannot find implementation TemporaryValidation for Organization: {0}", organizationId));
        }
    }
}
