using System;
using ComplaintTool.Common;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.MCProImageInterface
{
    public abstract class FileProcessorBase : IComplaintProcess
    {
        public static readonly ILogger Logger = LogManager.GetLogger();
        protected readonly ComplaintUnitOfWork _unitOfWork;

        public FileProcessorBase(ComplaintUnitOfWork unitOfWork)
        {
            Guard.ThrowIf<ArgumentNullException>(unitOfWork == null, "unitOfWork");
            _unitOfWork = unitOfWork;
        }

        #region IComplaintProcess

        public string OrganizationId
        {
            get
            {
                return ComplaintTool.Common.Enum.Organization.MC.ToString();
            }
        }

        public string ProcessName
        {
            get
            {
                return Globals.MCProImageInterfaceProcessName;
            }
        }

        #endregion

        public abstract string FilePath { get; }
        public abstract string ProcessFilePath { get; }
        public abstract string FileDescription { get; }
        public abstract bool Process(Guid bulkProcessKey);
        public abstract void Clean();
    }
}
