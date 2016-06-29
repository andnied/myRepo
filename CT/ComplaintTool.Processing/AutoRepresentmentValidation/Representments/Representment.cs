using ComplaintTool.Common.Enum;
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
    public abstract class Representment
    {
        protected const string Stage = "1CB";
        protected ComplaintUnitOfWork _unitOfWork;
        public RepresentmentCondition RepresentmentCondition { get; set; }
        public ProcessingStatus ProcessingStatus { get; set; }
        public MemberMessageText MemberMessageText { get; set; }

        protected Representment(ComplaintUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RepresentmentCondition = null;
            MemberMessageText = null;
            ProcessingStatus = ProcessingStatus.ManualProcess;
        }

        public abstract bool Calculate(Complaint complatint);

        protected ComplaintStage GetComplaintStageAndSetRepresentment(Complaint complaint,bool? is3DSecure=null,string reasonCode=null,int dayStep=0)
        {
            var complaintStage = _unitOfWork.Repo<StageRepo>().FindLastComplaintStage(Stage, complaint.CaseId);
            if (complaintStage == null)
                return null;

            RepresentmentCondition = _unitOfWork.Repo<RepresentmentRepo>().FindAutomaticRepresentmentCondition(complaint.OrganizationId, Stage, reasonCode ?? complaintStage.ReasonCode, is3DSecure,dayStep);
            return complaintStage;
        }
    }
}
