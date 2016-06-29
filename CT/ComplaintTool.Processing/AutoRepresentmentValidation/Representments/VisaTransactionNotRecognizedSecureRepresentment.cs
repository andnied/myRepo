using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Models;

namespace ComplaintTool.Processing.AutoRepresentmentValidation.Representments
{
    public class VisaTransactionNotRecognizedSecureRepresentment:Representment
    {
        public VisaTransactionNotRecognizedSecureRepresentment(ComplaintUnitOfWork unitOfWork) : base(unitOfWork) { }

        public override bool Calculate(Complaint complatint)
        {
            if (!complatint.Is3DSecure.HasValue||!complatint.Is3DSecure.Value)
                return false;

            var complaintStage=GetComplaintStageAndSetRepresentment(complatint,dayStep:1);
            if (RepresentmentCondition == null) return false;
            ProcessingStatus = Common.Enum.ProcessingStatus.AutomaticRepresentment;
            MemberMessageText = _unitOfWork.Repo<MessageRepo>().FindMemberMessageTextById(RepresentmentCondition.MessageId);
            return true;
        }
    }
}
