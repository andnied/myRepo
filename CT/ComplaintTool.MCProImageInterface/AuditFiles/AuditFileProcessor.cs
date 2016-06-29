using System;
using System.IO;
using System.Linq;
using ComplaintTool.Common;
using ComplaintTool.Common.Extensions;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Repos;

namespace ComplaintTool.MCProImageInterface.AuditFiles
{
    public abstract class AuditFileProcessor : FileProcessorBase
    {
        #region Consts
        public const string Error = "ERROR";
        public const string Match = "MATCH";
        public const string Expiration = "EXPIRATION";
        public const string Pending = "PENDING";
        public const string ReconSummary = "RECONSUMMARY";
        #endregion

        protected readonly AuditFileBase _auditFile;
        protected Guid _bulkProcessKey;
        protected Guid _auditStreamId;

        public AuditFileProcessor(ComplaintUnitOfWork unitOfWork, AuditFileBase auditFile)
            : base(unitOfWork)
        {
            Guard.ThrowIf<ArgumentNullException>(auditFile == null, "auditFile");
            _auditFile = auditFile;
        }

        public override string FilePath
        {
            get
            {
                return _auditFile.FilePath;
            }
        }

        public override string ProcessFilePath
        {
            get
            {
                return _auditFile.FilePath;
            }
        }

        public override bool Process(Guid bulkProcessKey)
        {
            Guard.ThrowIf<ArgumentNullException>(bulkProcessKey.IsEmpty(), "bulkProcessKey");
            _bulkProcessKey = bulkProcessKey;

            // dodaje plik audytowany do bazy danych
            _auditStreamId = _unitOfWork.Repo<FileRepo>().AddIncomingFile(this.ProcessFilePath);

            bool status = _auditFile.Parse();
            if (!status)
                return false;

            bool processed = _auditFile.Messages.All(x => this.ProcessAudit(x.Key, x.Value));
            if (processed)
            {
                Logger.LogComplaintEvent(149, Path.GetFileNameWithoutExtension(FilePath));
            }
            return processed;
        }

        public override void Clean()
        {
            if (File.Exists(ProcessFilePath))
                File.Delete(ProcessFilePath);
        }

        protected abstract bool ProcessAudit(string messageKey, string messageValue);

        #region Factory Method

        public static AuditFileProcessor Create(ComplaintUnitOfWork unitOfWork, string filePath)
        {
            string auditFileName = Path.GetFileName(filePath).ToUpper();

            if (auditFileName.Contains(Error))
                return new ErrorAuditFileProcessor(unitOfWork, new ErrorAuditFile(filePath));

            else if (auditFileName.Contains(Match))
                return new MatchAuditFileProcessor(unitOfWork, new MatchAuditFile(filePath));

            else if (auditFileName.Contains(Pending))
                return new PendingAuditFileProcessor(unitOfWork, new PendingAuditFile(filePath));

            else if (auditFileName.Contains(Expiration))
                return new ExpirationAuditFileProcessor(unitOfWork, new ExpirationAuditFile(filePath));

            else if (auditFileName.Contains(ReconSummary))
                return new ReconSummaryAuditFileProcessor(unitOfWork, new ReconSummaryAuditFile(filePath));

            else throw ComplaintCaseFilingIncomingException.UnsupportedAuditFile(filePath);
        }

        #endregion
    }
}
