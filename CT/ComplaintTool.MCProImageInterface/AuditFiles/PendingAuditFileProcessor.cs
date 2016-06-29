using System;
using System.IO;
using System.Linq;
using ComplaintTool.Common;
using ComplaintTool.Common.Config;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;

namespace ComplaintTool.MCProImageInterface.AuditFiles
{
    class PendingAuditFileProcessor : AuditFileProcessor
    {
        public PendingAuditFileProcessor(ComplaintUnitOfWork unitOfWork, PendingAuditFile auditFile) 
            : base(unitOfWork, auditFile)
        {
        }

        public override string FileDescription
        {
            get
            {
                return "Pending File";
            }
        }

        protected override bool ProcessAudit(string messageKey, string messageValue)
        {
            DateTime currentDate = DateTime.UtcNow;

            var outgoing = _unitOfWork.Repo<FileRepo>().FindOutgoingFileByName(messageKey);
            if (outgoing == null)
            {
                Logger.LogComplaintEvent(555, messageKey, Path.GetFileName(this.ProcessFilePath));
                return true; // prawidłowe zachowanie
            }

            Audit newAudit = _unitOfWork.Repo<AuditRepo>().AddNewAudit(outgoing.StageId,
                outgoing.CaseId,
                messageValue,
                _auditStreamId,
                _bulkProcessKey);

            int days = (currentDate - outgoing.InsertDate).Days;
            string note = string.Format(ComplaintConfig.Instance.Notifications[147].MessageText, messageKey, days, currentDate);
            _unitOfWork.Repo<NotificationRepo>().InsertFileStageNotificationForAudit(note,
                this.OrganizationId,
                outgoing.CaseId,
                outgoing.StageId,
                newAudit.AuditId,
                outgoing.OutgoingId,
                this.FileDescription);
            return true;
        }
    }
}
