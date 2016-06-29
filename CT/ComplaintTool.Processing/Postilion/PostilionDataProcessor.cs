using ComplaintTool.Common.Enum;
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

namespace ComplaintTool.Processing.Postilion
{
    public class PostilionDataProcessor
    {
        private ComplaintUnitOfWork _unitOfWork;
        private delegate void ProcessComplaintDelegate(Complaint complaint);
        private ILogger Logger = LogManager.GetLogger();

        public void Process(string caseId)
        {
            List<Complaint> complaints = new List<Complaint>();
            using(_unitOfWork=ComplaintUnitOfWork.Create())
            {
                complaints = GetComplaints(caseId);
            }
            ExecuteTransForComplaints(complaints,new ProcessComplaintDelegate(SetPostlionData));
            //TODO SI LogMessage(...,complaints);
            ExecuteTransForComplaints(complaints, new ProcessComplaintDelegate(UpdateRecords));
            //TODO SI LogMessage(...,complaints);
        }

        private List<Complaint> GetComplaints(string caseId)
        {
            List<Complaint> complaints = new List<Complaint>();
            if (string.IsNullOrEmpty(caseId))
                complaints.AddRange(_unitOfWork.Repo<ComplaintRepo>().FindWithoutPostilionData());
            else
            {
                var complaint = _unitOfWork.Repo<ComplaintRepo>().FindByCaseId(caseId);
                complaints.Add(complaint);
            }
            return complaints;
        }

        private void ExecuteTransForComplaints(IEnumerable<Complaint> complaints,ProcessComplaintDelegate complaintDelegate)
        {
            using(_unitOfWork=ComplaintUnitOfWork.Create())
            {
                foreach(var complaint in complaints)
                {
                    complaintDelegate(complaint);
                }
                _unitOfWork.Commit();
            }
        }

        private void SetPostlionData(Complaint complaint)
        {
            var postilionProvider = new PostilionDataProvider(_unitOfWork.Repo<PostilionRepo>());
            var postilionData=postilionProvider.GetData(complaint);
            bool result=_unitOfWork.Repo<AutoRepresentmentRepo>().ComplementPostilionData(complaint, postilionData);
        }

        private void UpdateRecords(Complaint complaint)
        {
            var complaintRecord = _unitOfWork.Repo<AutoRepresentmentRepo>().UpdateComplaintRecord(complaint);
            var crbReportsItems = _unitOfWork.Repo<CRBRepo>().GetItemsByCaseIdAndStatus(complaint.CaseId, (int)CrbReportStatus.ManualModyfing);
            foreach(var crbReportItem in crbReportsItems)
            {
                crbReportItem.CBReportStatus = (int)CrbReportStatus.New;
                //crbReportItem.ComplaintRecord = complaintRecord;
                _unitOfWork.Repo<ComplaintRepo>().Update(crbReportItem);
            }
        }

        private void LogMessage(int numberMessage, List<Complaint> complaints)
        {
            var sb = new StringBuilder();
            complaints.ForEach(x => sb.AppendFormat("{0}  ", x.CaseId));
            Logger.LogComplaintEvent(numberMessage, sb.ToString());
        }
    }
}
