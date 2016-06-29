using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Processing.CloseCases
{
    public class CloseCasesProcessor
    {
        //Dla każdego: 1CB, 2CB (MC), RR, COLL jeżeli nie było żadnego CLF zamykamy po zdefiniowany czasie ostatniego wpisu końca statusu plus 2 dni.
        private readonly List<string> _stagesToCloseByDefiniedTimelastState = new List<string>() { "1CB", "2CB", "RR", "COLL" };
        ILogger Logger = LogManager.GetLogger();

        public void Process()
        {
            using (var unitOfWork = ComplaintUnitOfWork.Create())
            {
                //var complaints = GetComplaintsToClose(unitOfWork);
                //var amount=SwitchCasesToClose(complaints);
                //Logger.LogComplaintEvent(129, amount);
            }
        }

        //private IEnumerable<Complaint> GetComplaintsToClose(ComplaintUnitOfWork unitOfWork)
        //{
        //    List<Complaint> complaints = new List<Complaint>();

        //    var complaintStages = unitOfWork.Repo<StageRepo>().GetStagesForCloseComplaintsWithoucClf();
        //    complaintStages.Where(x => x.StageEndDate.HasValue && _stagesToCloseByDefiniedTimelastState.Contains(x.StageCode)).ToList().ForEach(x =>
        //    {
        //        var tempDate = x.StageEndDate.Value.AddDays(2);
        //        if (tempDate.Date.CompareTo(DateTime.Now.Date) < 0)
        //            complaints.Add(x.Complaint);
        //    });

        //    return complaints.Distinct();
        //}

        private int SwitchCasesToClose(IEnumerable<Complaint> complaints)
        {
            int amountCorrect=0;
            foreach (var complaint in complaints)
            {
                try
                {
                    using (var unitOfWork = ComplaintUnitOfWork.Create())
                    {
                        var updatedComplaint = CloseComplaint(complaint,unitOfWork);
                        var closeDate = complaint.CloseDate.Value.ToString("yyyy-MM-dd");
                        unitOfWork.Repo<ComplaintRepo>().AddComplaintAutomaticEventsWithLastFK(updatedComplaint);
                        amountCorrect++;
                        unitOfWork.Repo<ComplaintRepo>().AddComplaintNote(complaint.CaseId, 128, closeDate);
                        Logger.LogComplaintEvent(128, closeDate);
                        unitOfWork.Commit();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogComplaintException(ex);
                }
            }
            return amountCorrect;
        }

        private Complaint CloseComplaint(Complaint complaint,ComplaintUnitOfWork unitOfWork)
        {
            complaint.Close = true;
            complaint.CloseDate = DateTime.Now;
            unitOfWork.Repo<ComplaintRepo>().Update(complaint);
            return complaint;
        }
    }
}
