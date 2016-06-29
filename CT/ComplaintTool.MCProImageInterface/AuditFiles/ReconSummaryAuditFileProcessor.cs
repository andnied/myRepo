using ComplaintTool.DataAccess;

namespace ComplaintTool.MCProImageInterface.AuditFiles
{
    class ReconSummaryAuditFileProcessor : AuditFileProcessor
    {
        public ReconSummaryAuditFileProcessor(ComplaintUnitOfWork unitOfWork, ReconSummaryAuditFile auditFile) 
            : base(unitOfWork, auditFile)
        {
        }

        public override string FileDescription
        {
            get
            {
                return "ReconSummary File";
            }
        }

        protected override bool ProcessAudit(string messageKey, string messageValue)
        {
            return true;
        }
    }
}
