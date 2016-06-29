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
    class ErrorAuditFileProcessor : AuditFileProcessor
    {
        public ErrorAuditFileProcessor(ComplaintUnitOfWork unitOfWork, ErrorAuditFile file) 
            : base(unitOfWork, file)
        {
        }

        public override string FileDescription
        {
            get
            {
                return "Error File";
            }
        }

        protected override bool ProcessAudit(string messageKey, string messageValue)
        {
            var errors = messageValue.Substring(
                    messageValue.LastIndexOf("List Of Errors:",
                        StringComparison.Ordinal) + 16);

            // szuka w bazie danych pliku wychodzącego na podstawie nazwy pliku
            var outgoing = _unitOfWork.Repo<FileRepo>().FindOutgoingFileByName(messageKey);
            if (outgoing == null)
            {
                Logger.LogComplaintEvent(555, messageKey, Path.GetFileName(this.ProcessFilePath));
                return true; // prawidłowe zachowanie
            }

            // dodaje do bazy danych nowy wpis audytowy
            Audit newAudit = _unitOfWork.Repo<AuditRepo>().AddNewAudit(outgoing.StageId,
                outgoing.CaseId,
                messageValue,
                _auditStreamId,
                _bulkProcessKey,
                errors);

            // wprowadza do bazy danych nową notyfikację
            string note = string.Format(ComplaintConfig.Instance.Notifications[139].MessageText, errors);
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
