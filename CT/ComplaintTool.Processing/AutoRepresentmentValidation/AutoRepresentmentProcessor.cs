using ComplaintTool.Common.Enum;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.Processing.AutoRepresentmentValidation.Organizations;
using ComplaintTool.Processing.AutoRepresentmentValidation.Switches;

using System;
using Model = ComplaintTool.Processing.AutoRepresentmentValidation.Representments;

namespace ComplaintTool.Processing.AutoRepresentmentValidation
{
    public class AutoRepresentmentProcessor
    {
        public void Process()
        {
            using(var unitOfWork = ComplaintUnitOfWork.Create())
            {
                var complaints = unitOfWork.Repo<ComplaintRepo>().FindNotClose((int)ProcessingStatus.New);

                foreach(var complaint in complaints)
                {
                    var organization = GetOrganization(complaint, unitOfWork);
                    var representment=organization.CheckRules(complaint);

                    var procSwitch = GetSwitch(representment);
                    procSwitch.Switch(complaint.CaseId);
                }
            }
        }

        private IProcessingSwitch GetSwitch(Model.Representment representment)
        {
            switch (representment.ProcessingStatus)
            {
                case ProcessingStatus.AutomaticRepresentment:
                    return new AutomaticProcessingSwitch(representment);
                case ProcessingStatus.ManualProcess:
                    return new ManualProcessingSwitch();

                default: throw new Exception(string.Format("Not definied behavior for this process {0}",representment.ProcessingStatus));
            }
        }

        private IOrganization GetOrganization(Complaint complaint,ComplaintUnitOfWork unitOfWork)
        {
            if (complaint.OrganizationId.Equals(Common.Enum.Organization.MC.ToString()))
                return new McOrganization(unitOfWork);

            if (complaint.OrganizationId.Equals(Common.Enum.Organization.VISA.ToString()))
                return new VisaOrganization(unitOfWork);

            throw new Exception(string.Format("Could not find implementation, for organization: {0} in complaint CaseId: {1}", complaint.OrganizationId,complaint.CaseId));

        }
    }
}
