using ComplaintTool.Common.Enum;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Processing.CLFValidation
{
    public class ClfItemsValidator
    {
        private Dictionary<string, CRBReport> _crbReports;
        private ComplaintUnitOfWork _unitOfWork;
        private const string RESPONSE_CODE = "66";
        private const string COMMENTS_PROCESSOR = "Past time limit";
        private ILogger Logger = LogManager.GetLogger();
        private const string note = "CLF autorejected as 'Past time limit";

        public ClfItemsValidator()
        {
            _crbReports = new Dictionary<string, CRBReport>();
        }
        //public void ValidateItems()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    using (_unitOfWork = ComplaintUnitOfWork.Create())
        //    {
        //        var clfItems = _unitOfWork.Repo<CLFRepo>().FindIncomingItemsWithStages((int)ClfReportStatus.New);
        //        foreach (var clf in clfItems)
        //        {
        //            sb.AppendFormat("{0}\t", clf.IncomingItem.CaseId);
        //            ValidateItem(clf);
        //        }
        //        _unitOfWork.Commit();
        //        Logger.LogComplaintEvent(153);
        //    }
        //}

        //private void ValidateItem(ClfItems clf)
        //{
        //    if (clf.IncomingItem.CLFReport == null || !clf.IncomingItem.CLFReport.ProcesingStart.HasValue)
        //        return;

        //    var crbReport = GetReport(clf.IncomingItem.CaseId);
        //    AddNewClfItem(clf.IncomingItem);
        //    CloseComplaint(clf.IncomingItem.Complaint);
        //    AddAutomaticEvent(clf.IncomingItem.Complaint);
        //    _unitOfWork.Repo<ComplaintRepo>().AddComplaintNote(clf.IncomingItem.CaseId,note);
        //}

        //private void AddAutomaticEvent(Complaint complaint)
        //{
        //    var complaintAutomaticEvent = new ComplaintAutomaticEvent
        //    {
        //        AutomaticKey = Guid.NewGuid(),
        //        AutomaticProcess = AutomaticProcess.CLFAutoReject.ToString(),
        //        CaseId = complaint.CaseId,
        //        ComplaintValue=complaint.ComplaintValues.OrderByDescending(x => x.ValueId).FirstOrDefault(),
        //        ComplaintRecord=complaint.ComplaintRecords.OrderByDescending(x=>x.RecordId).FirstOrDefault(),
        //        ComplaintStage=complaint.ComplaintStages.OrderByDescending(x=>x.StageId).FirstOrDefault(),
        //        InsertDate = DateTime.UtcNow,
        //        InsertUser = GetCurrentUser()
        //    };
        //    _unitOfWork.Repo<ComplaintRepo>().AddComplaintAutomaticEvent(complaintAutomaticEvent);
        //}

        private CRBReport GetReport(string caseId)
        {
            if(_crbReports.ContainsKey(caseId))
                return _crbReports[caseId];

            var crbReport=_unitOfWork.Repo<CRBRepo>().FindLastReport(caseId);
            _crbReports.Add(caseId, crbReport);
            return crbReport;
        }

        private void AddNewClfItem(CLFReportItem clfReportItem)
        {
            var newClfReportItem = clfReportItem;
            newClfReportItem.ResponseCode = RESPONSE_CODE;
            newClfReportItem.CommentsProcessor = COMMENTS_PROCESSOR;
            newClfReportItem.CLFReportId = null;
            newClfReportItem.InsertDate = DateTime.UtcNow;
            newClfReportItem.InsertUser = GetCurrentUser();
            newClfReportItem.CLFItemIdSource = clfReportItem.CLFItemId;
            _unitOfWork.Repo<CLFRepo>().Add(newClfReportItem);
        }

        private void CloseComplaint(Complaint complaint)
        {
            if (complaint == null) return;
            complaint.Close = true;
            complaint.CloseDate = DateTimeOffset.Now;
            _unitOfWork.Repo<ComplaintRepo>().Update<Complaint>(complaint);
        }

        private string GetCurrentUser()
        {
            var identity = WindowsIdentity.GetCurrent();
            var currentUser = identity != null ? identity.Name : "ComplaintServices";
            return currentUser;
        }
    }
}
