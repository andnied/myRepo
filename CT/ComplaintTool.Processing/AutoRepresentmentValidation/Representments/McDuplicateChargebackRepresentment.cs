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
    public class McDuplicateChargebackRepresentment:Representment
    {
        private const string StageR = "1CBR";

        public McDuplicateChargebackRepresentment(ComplaintUnitOfWork unitOfWork):base(unitOfWork)
        { }

        public override bool Calculate(Complaint complaint)
        {
            var complaintStage = GetComplaintStageAndSetRepresentment(complaint,reasonCode: "ALL");

            if (RepresentmentCondition == null)
                return false;

            var stages = _unitOfWork.Repo<StageRepo>().FindComplaintStages(complaint.CaseId, Stage, complaintStage.ReasonCode);
            bool stageRExist = _unitOfWork.Repo<StageRepo>().ExistComplaintStages(complaint.CaseId, StageR, complaintStage.ReasonCode);
            if (!ValidateStages(stages, stageRExist))
                return false;

            var firstStage = stages.First();
            if (!ValidateFirstStage(stages, firstStage))
                return false;

            var firstValue = _unitOfWork.Repo<ComplaintRepo>().GetComplaintValueByStageId(firstStage.StageId);
            if (firstValue == null || !firstValue.BookingAmount.HasValue)
                return false;

            if (!AreAmountsTheSame(stages, firstValue))
                return false;

            ProcessingStatus = ComplaintTool.Common.Enum.ProcessingStatus.AutomaticRepresentment;
            CreateMessage(complaint);

            return true;
        }

        private bool ValidateStages(IEnumerable<ComplaintStage> stages,bool stageRExist)
        {
            if (stages.Count()==0||(stages.Count() >= 2 && stageRExist))
                return false;

            var allHasValue = stages.All(x => x.IncomingId.HasValue);
            if (!allHasValue) return false;

            return true;
        }

        private bool ValidateFirstStage(IEnumerable<ComplaintStage> stages,ComplaintStage firstStage)
        {        
            if (!firstStage.IncomingId.HasValue) return false;

            var allTheSame = stages.All(x => x.IncomingId == firstStage.IncomingId);
            if (allTheSame) return false;

            return true;
        }

        private bool AreAmountsTheSame(IEnumerable<ComplaintStage> stages, ComplaintValue firstValue)
        {
            var complaintValues = _unitOfWork.Repo<ComplaintRepo>().GetValuesForStage(stages);
            var areAmountsTheSame = true;

            foreach (var value in complaintValues)
            {
                if (value == null) continue;

                if (!value.BookingAmount.HasValue)
                {
                    areAmountsTheSame = false;
                    break;
                }

                if (value.BookingAmount.Value.Equals(firstValue.BookingAmount.Value)) continue;
                areAmountsTheSame = false;
                break;
            }

            return areAmountsTheSame;
        }

        private void CreateMessage(Complaint complaint)
        {
            IncomingTranMASTERCARD incomingRecMC=null;

            var message=_unitOfWork.Repo<MessageRepo>().FindMemberMessageTextById(RepresentmentCondition.MessageId);
            if (message == null)
                return;

            if (complaint.IncomingId.HasValue)
                incomingRecMC = _unitOfWork.Repo<IncomingTranRepo>().FindTranMASTERCARDById(complaint.IncomingId);

            if (incomingRecMC == null)
            {
                SetMemberMessageText(message.MessageText, complaint.ARN);
                return;
            }

            var incomingFile = _unitOfWork.Repo<RegOrgIncomingFilesRepo>().FindRegOrgIncomingFile(incomingRecMC.FileId);
            if(incomingFile==null)
            {
                SetMemberMessageText(message.MessageText, complaint.ARN);
                return;
            }

            if (string.IsNullOrEmpty(incomingFile.IncomingFileID))
            {
                SetMemberMessageText(message.MessageText, complaint.ARN);
                return;
            }  

            var date = DateTime.ParseExact(incomingFile.IncomingFileID.Substring(3, 6), "yyMMdd", null);
            SetMemberMessageText(message.MessageText, complaint.ARN, date);

        }

        private void SetMemberMessageText(string messageText,string arn,DateTime? date=null)
        {
            MemberMessageText =new MemberMessageText
            {
                MessageText = string.Format(messageText, arn, date.HasValue ? date.Value.ToString("MMddyy"):string.Empty)
            };
        }
    }
}
