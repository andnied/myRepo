using System;
using System.Data.Entity;
using ComplaintTool.Models;

namespace ComplaintTool.DataAccess.Repos
{
    public class AuditRepo : RepositoryBase
    {
        public AuditRepo(DbContext context) 
            : base(context)
        {
        }

        public Audit AddNewAudit(long? stageId, 
            string caseId,
            string desc,          
            Guid auditStreamId,
            Guid bulkProcessKey,
            string errDesc = null)
        {
            var currDateTime = GetCurrentDateTime();
            var newAudit = new Audit
            {
                StageId = stageId,
                CaseId = caseId,
                Description = desc,
                ErrorDescription = errDesc,
                Status = 0,
                stream_id = auditStreamId,
                ProcessKey = bulkProcessKey,
                IncomingDate = currDateTime,
                InsertDate = currDateTime,
                InsertUser = GetCurrentUser()
            };
            AddAudit(newAudit);
            Commit();
            return newAudit;
        }
    }
}
