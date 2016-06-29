using System;
using System.IO;
using ComplaintTool.Common;
using ComplaintTool.Common.Config;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;

namespace ComplaintTool.MCProImageInterface.AuditFiles
{
    class MatchAuditFileProcessor : AuditFileProcessor
    {
        public MatchAuditFileProcessor(ComplaintUnitOfWork unitOfWork, MatchAuditFile file) 
            : base(unitOfWork, file)
        {
        }

        public override string FileDescription
        {
            get
            {
                return "Match File";
            }
        }

        protected override bool ProcessAudit(string messageKey, string messageValue)
        {
            var complaint = _unitOfWork.Repo<ComplaintRepo>().FindByARN(messageKey);
            if (complaint == null)
            {
                Logger.LogComplaintEvent(556, messageKey, Path.GetFileName(this.ProcessFilePath));
                return true; // prawidłowe zachowanie
            }

            var outgoing = _unitOfWork.Repo<FileRepo>().FindByComplaint(complaint);

            Audit newAudit = _unitOfWork.Repo<AuditRepo>().AddNewAudit(outgoing.StageId,
                complaint.CaseId,
                messageValue,
                _auditStreamId,
                _bulkProcessKey);

            string note = ComplaintConfig.Instance.Notifications[140].MessageText;
            _unitOfWork.Repo<NotificationRepo>().InsertFileStageNotificationForAudit(note,
                this.OrganizationId,
                complaint.CaseId,
                outgoing.StageId,
                newAudit.AuditId,
                outgoing.OutgoingId,
                this.FileDescription);
            return true;
        }
    }
}
