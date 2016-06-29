using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Processing.AutoRepresentmentValidation.Representments
{
    public class VisaTransactionNotRecognizedRepresentment:Representment
    {
        private readonly string[] VISATranNotRecCodes = { "33", "34" };
        private const string StageCodeR = "RR";

        public VisaTransactionNotRecognizedRepresentment(ComplaintUnitOfWork unitOfWork) : base(unitOfWork) { }

        public override bool Calculate(Complaint complaint)
        {
            var complaintStage=_unitOfWork.Repo<StageRepo>().FindLastComplaintStage(Stage, complaint.CaseId, new string[] { "75" });
            if(complaintStage==null)
                return false;

            var complaintStageRr = _unitOfWork.Repo<StageRepo>().FindLastComplaintStage(StageCodeR, complaint.CaseId, VISATranNotRecCodes);
            if (complaintStageRr == null)
                return false;

            RepresentmentCondition = _unitOfWork.Repo<RepresentmentRepo>().FindAutomaticRepresentmentCondition(Common.Enum.Organization.VISA.ToString(), Stage, complaintStage.ReasonCode);
            if (RepresentmentCondition == null) return false;

            ProcessingStatus = Common.Enum.ProcessingStatus.AutomaticRepresentment;
            var message=_unitOfWork.Repo<MessageRepo>().FindMemberMessageTextById(RepresentmentCondition.MessageId);

            if (message != null)
                MemberMessageText = new MemberMessageText() { MessageText = string.Format(message.MessageText, complaintStageRr.ReasonCode) };

            return true;
        }
    }
}
