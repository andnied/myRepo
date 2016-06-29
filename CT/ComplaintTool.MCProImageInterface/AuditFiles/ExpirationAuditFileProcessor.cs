using System;
using System.IO;
using ComplaintTool.Common.Config;
using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Repos;

namespace ComplaintTool.MCProImageInterface.AuditFiles
{
    class ExpirationAuditFileProcessor : AuditFileProcessor
    {
        public ExpirationAuditFileProcessor(ComplaintUnitOfWork unitOfWork, ExpirationAuditFile auditFile) 
            : base(unitOfWork, auditFile)
        {
        }

        public override string FileDescription
        {
            get
            {
                return "Expiration File";
            }
        }

        protected override bool ProcessAudit(string messageKey, string messageValue)
        {
            var currentDate = DateTime.UtcNow;

            var outgoing = _unitOfWork.Repo<FileRepo>().FindOutgoingFileByName(messageKey);
            if (outgoing == null)
            {
                Logger.LogComplaintEvent(555, messageKey, Path.GetFileName(ProcessFilePath));
                return true; // prawidłowe zachowanie
            }

            var newAudit = _unitOfWork.Repo<AuditRepo>().AddNewAudit(outgoing.StageId,
                outgoing.CaseId,
                messageValue,
                _auditStreamId,
                _bulkProcessKey);

            var days = (currentDate - outgoing.InsertDate).Days;
            var note = string.Format(ComplaintConfig.Instance.Notifications[148].MessageText, outgoing.FileName, days, currentDate);
            _unitOfWork.Repo<NotificationRepo>().InsertFileStageNotificationForAudit(note,
                OrganizationId,
                outgoing.CaseId,
                outgoing.StageId,
                newAudit.AuditId,
                outgoing.OutgoingId,
                FileDescription);
            return true;
        }
    }
}
