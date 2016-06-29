using ComplaintTool.Common.Enum;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using System;
using ComplaintTool.DataAccess.Utils;

namespace ComplaintTool.Processing.AutoRepresentmentValidation.Representments
{
    public class PastTimeFrameRepresentment:Representment
    {

        public PastTimeFrameRepresentment(ComplaintUnitOfWork unitOfWork):base(unitOfWork)
        {
        }

        public override bool Calculate(Complaint complaint)
        {
            var complaintStage=GetComplaintStageAndSetRepresentment(complaint);
            if (RepresentmentCondition == null)
                return false;

            var julianDateFromArn = complaint.ARN.Substring(7, 4);
            if (!complaint.TransactionDate.HasValue) return false;
            var message=_unitOfWork.Repo<MessageRepo>().FindMemberMessageTextById(RepresentmentCondition.MessageId);
            var date = Helper.JulianDateToDateTime(julianDateFromArn, complaint.TransactionDate.Value.Date);
            var stageDate = GetStageDate(complaint, complaintStage);
            if (!stageDate.HasValue) return false;
            var differenceDate = Math.Abs((date - stageDate.Value).TotalDays);

            if (differenceDate > RepresentmentCondition.DayStep)
            {
                ProcessingStatus = ProcessingStatus.AutomaticRepresentment;
                return true;
            }
            return false;
        }

        private DateTime? GetStageDate(Complaint complaint, ComplaintStage complaintStage)
        {
            if (complaint.OrganizationId.Equals(Common.Enum.Organization.VISA.ToString()))
                return complaintStage.BusinessDate;
            else if (complaint.OrganizationId.Equals(Common.Enum.Organization.MC.ToString()))
                return complaintStage.SettlementDate;

            throw new Exception(string.Format("Not definied organization for Complaint CaseId: {0}", complaint.OrganizationId));
        }
        
    }
}
