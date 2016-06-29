using ComplaintTool.DataAccess;
using Model=ComplaintTool.Processing.AutoRepresentmentValidation.Representments;
using System;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using System.Security.Principal;
using ComplaintTool.Common.Enum;
using ComplaintTool.Postilion.Outgoing.Model.Representment;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Processing.AutoRepresentmentValidation.Switches
{
    public class AutomaticSwitchCompleter
    {
        private readonly ComplaintUnitOfWork _unitOfWork;
        private readonly Model.Representment _representment;
        private readonly ILogger _logger = LogManager.GetLogger();

        public AutomaticSwitchCompleter(ComplaintUnitOfWork unitOfWork, Model.Representment representment)
        {
            _unitOfWork = unitOfWork;
            _representment = representment;
        }

        public void Complete(Complaint complaint, ComplaintRecord complaintRecord)
        {
            var complaintStage = AddRepresentmentInComplaintStage(complaint);
            UpdateComplaintRecord(complaintStage, complaintRecord);
            var complaintValue = AddNewRepresentmentInComplaintValue(complaint, complaintStage);
            CloseComplaint(complaint);
            var representment = AddRecordForPostilionExtract(complaint, complaintStage, complaintValue, complaintRecord);
            AddNewComplaintAutomaticEvent(complaint, complaintValue, complaintStage, complaintRecord);
            CreateExtractForPostilion(complaint, complaintValue, complaintStage, complaintRecord,representment);
        }

        private ComplaintStage AddRepresentmentInComplaintStage(Complaint complaint)
        {
            var complaintStage = _unitOfWork.Repo<StageRepo>().FindLastComplaintStage(complaint.CaseId);
            if (complaintStage == null)
                throw new Exception(string.Format("Can not find Complaint Stage for CaseId: {0}", complaint.CaseId));
            var stageDefinition =
                _unitOfWork.Repo<StageRepo>()
                    .FindStageDefinition(_representment.RepresentmentCondition.DestinationStageCode, true);
            var newComplaintStage = complaintStage;
            newComplaintStage.ReasonCode = _representment.RepresentmentCondition.DestinationReasonCode;
            newComplaintStage.StageCode = _representment.RepresentmentCondition.DestinationStageCode;
            newComplaintStage.StageDate = DateTime.UtcNow;
            newComplaintStage.MemberMessageText = _representment.MemberMessageText != null ? _representment.MemberMessageText.MessageText : string.Empty;
            newComplaintStage.Status = 1;
            newComplaintStage.DocumentationIndicator = _representment.RepresentmentCondition.DocumentationIndicator;
            newComplaintStage.StageDefinition = stageDefinition;
            newComplaintStage.StageEndDate = Common.Utils.Convert.CountExpirationDate(DateTime.Now,
                complaint.OrganizationId, _representment.RepresentmentCondition.DestinationStageCode);
            newComplaintStage.InsertDate = DateTime.UtcNow;
            newComplaintStage.InsertUser = GetCurrentUser();
            _unitOfWork.Repo<StageRepo>().AddComplaintStage(newComplaintStage);

            return newComplaintStage;
        }

        private void UpdateComplaintRecord(ComplaintStage complaintStage, ComplaintRecord complaintRecord)
        {
            //complaintRecord.ComplaintStage = complaintStage;
            complaintRecord.StageCode = complaintStage.StageCode;
            _unitOfWork.Repo<ComplaintRepo>().Update(complaintRecord);
        }

        private ComplaintValue AddNewRepresentmentInComplaintValue(Complaint complaint, ComplaintStage complaintStage)
        {
            var complaintValue = _unitOfWork.Repo<ComplaintRepo>().FindLastComplaintValue(complaint.CaseId);
            if (complaintValue == null)
                throw new Exception(string.Format("Can not find Complaint Value for CaseId: {0}", complaint.CaseId));

            var newComplaintValue = complaintValue;
            //newComplaintValue.ComplaintStage = complaintStage;
            newComplaintValue.InsertDate = DateTime.UtcNow;
            newComplaintValue.InsertUser = GetCurrentUser();
            newComplaintValue = _unitOfWork.Repo<ComplaintRepo>().AddComplaintValue(newComplaintValue);
            return newComplaintValue;
        }

        private void CloseComplaint(Complaint complaint)
        {
            complaint.ProcessingStatus = (int)_representment.ProcessingStatus;
            complaint.DocumentationIndicator = "0";
            complaint.Close = true;
            complaint.CloseDate = DateTime.UtcNow;
            _unitOfWork.Repo<ComplaintRepo>().Update(complaint);
        }

        private Representment AddRecordForPostilionExtract(Complaint complaint, ComplaintStage complaintStage, ComplaintValue complaintValue, ComplaintRecord complaintRecord)
        {
            var representment = new Representment
                {
                    CaseId = complaint.CaseId,
                    StageCode = complaintStage.StageCode,
                    IsAutomatic = true,
                    //ComplaintValue=complaintValue,
                    //ComplaintRecord=complaintRecord,
                    //ComplaintStage=complaintStage,
                    DocumentationIndicator = _representment.RepresentmentCondition.DocumentationIndicator,
                    Status = 0,
                    IsReversal = false,
                    InsertDate = DateTime.UtcNow,
                    InsertUser = GetCurrentUser()
                };

            _unitOfWork.Repo<RepresentmentRepo>().Add(representment);
            return representment;
        }

        private void AddNewComplaintAutomaticEvent(Complaint complaint, ComplaintValue complaintValue, ComplaintStage complaintStage,ComplaintRecord complaintRecord)
        {
            var automaticKey = Guid.NewGuid();
            var complaintAutomaticEvent = new ComplaintAutomaticEvent
            {
                AutomaticKey = automaticKey,
                AutomaticProcess = AutomaticProcess.Representment.ToString(),
                CaseId = complaint.CaseId,
                //ComplaintValue=complaintValue,
                //ComplaintRecord=complaintRecord,
                //ComplaintStage=complaintStage,
                InsertUser = GetCurrentUser(),
                InsertDate = DateTime.UtcNow
            };
            _unitOfWork.Repo<ComplaintRepo>().AddComplaintAutomaticEvent(complaintAutomaticEvent);
        }

        private void CreateExtractForPostilion(Complaint complaint,ComplaintValue complaintValue,ComplaintStage complaintStage,ComplaintRecord complaintRecord,Representment representment)
        {
                var extracter = GetExtracter(complaint, representment);
                extracter.Set4103Extract(complaint, complaintValue, complaintStage, complaintRecord);
                var msg = _representment.MemberMessageText != null ? _representment.MemberMessageText.MessageText : string.Empty;
                _logger.LogComplaintEvent(105, new object[] { complaint.CaseId,"Autorepresentment process: "+msg});
        }

        private Organization4103Extracter GetExtracter(Complaint complaint, Representment representment)
        {
            if (Common.Enum.Organization.MC.ToString().Equals(complaint.OrganizationId))
                return new MC4103Extracter(_unitOfWork, representment);
            if (Common.Enum.Organization.VISA.ToString().Equals(complaint.OrganizationId))
                return new Visa4103Extracter(_unitOfWork, representment);

            throw new Exception(string.Format("Not exist implementation for organization: {0} for complaint CaseId= {1}", complaint.OrganizationId,complaint.CaseId));
        }

        private static string GetCurrentUser()
        {
            var identity = WindowsIdentity.GetCurrent();
            return identity != null ? identity.Name : "ComplaintServices";
        }
    }
}
