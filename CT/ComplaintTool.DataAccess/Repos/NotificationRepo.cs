using System.Data.Entity;
using ComplaintTool.Common.Enum;
using ComplaintTool.Models;

namespace ComplaintTool.DataAccess.Repos
{
    public class NotificationRepo : RepositoryBase
    {
        public NotificationRepo(DbContext context) 
            : base(context)
        {
        }

        public FilesStageNotification InsertFileStageNotificationForAudit(string note, 
            string orgId,
            string caseId, 
            long stageId, 
            long? auditId, 
            long? outgoingId,
            string fileDescription)
        {
            var currentDate = GetCurrentDateTime();
            var newNotification = new FilesStageNotification
            {
                OrganizationId = orgId,
                CaseId = caseId,
                StageId = stageId, 
                AuditId = auditId,
                OutgoingId = outgoingId,
                NoteDate = currentDate,
                Note = note,
                FileDescription = fileDescription,
                InsertDate = currentDate,
                InsertUser = GetCurrentUser()
            };
            GetDbSet<FilesStageNotification>().Add(newNotification);
            Commit();
            return newNotification;
        }

        public FilesStageNotification InsertFileStageNotificationForIncoming(string note, string orgId, CaseFilingIncomingFile incomingImagePro, string fileDescription)
        {
            var currentDate = GetCurrentDateTime();
            var newNotification = new FilesStageNotification
            {
                OrganizationId = orgId,
                CaseId = incomingImagePro.CaseId,
                StageId = incomingImagePro.StageId,
                CaseFilingIncomingFile = incomingImagePro,
                NoteDate = currentDate,
                Note = note,
                FileDescription = fileDescription,
                InsertDate = currentDate,
                InsertUser = GetCurrentUser()
            };
            GetDbSet<FilesStageNotification>().Add(newNotification);
            Commit();
            return newNotification;
        }

        public void AddNotification(string message, MessageType messageType)
        {
            if (message.Length > 512)
                message = message.Substring(0, 512);

            var note = new Notification
            {
                MessageDate = GetCurrentDateTime(),
                Message = message,
                MessageType = messageType.ToString()
            };
            GetDbSet<Notification>().Add(note);
            Commit();
        }
    }
}
