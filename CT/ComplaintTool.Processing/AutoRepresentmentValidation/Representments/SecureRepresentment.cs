using ComplaintTool.Common.Config;
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
    public class SecureCheckRepresentment:Representment
    {
        public SecureCheckRepresentment(ComplaintUnitOfWork unitOfWork):base(unitOfWork)
        { }

        private const string ReasonCodeObjectName = "3DSecureReasonCode";
        private const string CorrectValueObject = "Y";

        public override bool Calculate(Complaint complaint)
        {
            if (!complaint.Is3DSecure.HasValue || !complaint.Is3DSecure.Value)
                return false;

            var complaintStage = GetComplaintStageAndSetRepresentment(complaint, true);
            if (RepresentmentCondition == null)
                return false;

            var reasonCodeMapValue = ComplaintDictionaires.GetMappingValue(ReasonCodeObjectName, complaintStage.ReasonCode);
            if (!CorrectValueObject.Equals(reasonCodeMapValue)) return false;

            return CreateMessageText(complaint);
        }

        protected bool CreateMessageText(Complaint complaint)
        {
            var complaintRecord = _unitOfWork.Repo<ComplaintRepo>().FindLastComplaintRecord(complaint.CaseId);

            if (complaintRecord == null) return false;

            if (string.IsNullOrEmpty(complaintRecord.PrevUcafData)) return false;

            var authUcafFirstSubfield = complaintRecord.PrevUcafData.Substring(0, 1);

            var datetimeRsp = complaintRecord.DatetimeRsp;
            var authIdRsp = complaintRecord.AuthIDRsp;

            if (!datetimeRsp.HasValue) return false;

            if (string.IsNullOrEmpty(authIdRsp))
            {
                var structDataReq = complaintRecord.StructuredDataReq;

                if (string.IsNullOrEmpty(structDataReq)) return false;
                
                var giccAuthCode = _unitOfWork.Repo<PostilionRepo>().GetStructuredDataFieldValue("eS:GiccAuthCode", structDataReq);

                if (string.IsNullOrEmpty(giccAuthCode)) return false; 

                authIdRsp = giccAuthCode;
            }

            if (authIdRsp.All(x => x.Equals('0'))) return false;

            ProcessingStatus = ComplaintTool.Common.Enum.ProcessingStatus.AutomaticRepresentment;
            var msgId = RepresentmentCondition.MessageId;
            var msgText = _unitOfWork.Repo<MessageRepo>().FindMemberMessageTextById(msgId);

            if (msgText != null)
            {
                MemberMessageText = new MemberMessageText
                {
                    MessageText = string.Format(msgText.MessageText,
                        datetimeRsp.Value.ToString("MMddyy"),
                        authIdRsp,
                        authUcafFirstSubfield)
                };
            }
            return true;
        }
    }
}
