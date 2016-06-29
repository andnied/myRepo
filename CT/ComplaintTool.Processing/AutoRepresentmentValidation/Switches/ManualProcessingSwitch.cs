using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.Processing.AutoRepresentmentValidation.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Model = ComplaintTool.Processing.AutoRepresentmentValidation.Representments;

namespace ComplaintTool.Processing.AutoRepresentmentValidation.Switches
{
    public class ManualProcessingSwitch:IProcessingSwitch
    {
        private readonly string[] SuspendStages = { "1CB", "2CB" };

        public ManualProcessingSwitch()
        {
        }

        public void Switch(string caseId)
        {
            using (var unitOfWork = ComplaintUnitOfWork.Create())
            {
                var complaint = unitOfWork.Repo<ComplaintRepo>().FindByCaseId(caseId);
                complaint.ProcessingStatus = (int)Common.Enum.ProcessingStatus.ManualProcess;
                unitOfWork.Repo<ComplaintRepo>().Update(complaint);
                if (Common.Enum.Organization.MC.ToString().Equals(complaint.OrganizationId))
                    AddToSuspuend(complaint, unitOfWork);
                unitOfWork.Commit();
            }
        }

        private void AddToSuspuend(Complaint complaint,ComplaintUnitOfWork unitOfWork)
        {
            var stage = unitOfWork.Repo<StageRepo>().FindLastComplaintStage(complaint.CaseId);
            if (stage == null)
                return;

            if (SuspendStages.Contains(stage.StageCode))
            {
                var dateTimeNow = DateTime.Now;
                var newSuspend = new ComplaintSuspended
                {
                    CaseId = complaint.CaseId,
                    SuspendedDate = dateTimeNow,
                    SuspendedDay = 8,
                    SuspendedEndDate = dateTimeNow.AddDays(8),
                    InsertDate = DateTime.UtcNow,
                    InsertUser = GetCurrentUser()
                };

                unitOfWork.Repo<ComplaintRepo>().AddComplaintSuspend(newSuspend);
            }
        }

        private static string GetCurrentUser()
        {
            var identity = WindowsIdentity.GetCurrent();
            return identity != null ? identity.Name : "ComplaintServices";
        }
    }
}
