using System;
using System.Diagnostics;
using System.IO;
using ComplaintTool.Common;
using ComplaintTool.Common.Extensions;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.MCProImageInterface.Outgoing
{
    public abstract class DocumentGenerator : IComplaintProcess
    {
        public static readonly ILogger Logger = LogManager.GetLogger();

        private readonly string _tempFolderPath = null;
        private readonly string _filePath = null;
        private readonly string _tifFilePath = null;
        private readonly long _counter;
        protected readonly ComplaintUnitOfWork _unitOfWork;

        public string TempFolderPath
        {
            get { return _tempFolderPath; }
        }

        public string TifFilePath
        {
            get { return _tifFilePath; }
        }

        public long Counter
        {
            get { return _counter; }
        }

        #region IComplaintProcess

        public string OrganizationId
        {
            get { return ComplaintTool.Common.Enum.Organization.MC.ToString(); }
        }

        public string ProcessName
        {
            get
            {
                return Globals.MCProImageInterfaceProcessName;
            }
        }

        public string FilePath
        {
            get
            {
                return _filePath;
            }
        }

        public string ProcessFilePath
        {
            get
            {
                return _filePath;
            }
        }

        #endregion

        public DocumentGenerator(ComplaintUnitOfWork unitOfWork, string tempFolderPath, string tifFilePath)
        {
            Guard.ThrowIf<ArgumentNullException>(unitOfWork == null, "unitOfWork");
            Guard.ThrowIf<ArgumentNullException>(tempFolderPath.IsEmpty(), "tempFolderPath");
            Guard.ThrowIf<ArgumentNullException>(tifFilePath.IsEmpty(), "tifFilePath");
            _unitOfWork = unitOfWork;
            _tempFolderPath = tempFolderPath;
            _tifFilePath = tifFilePath;
            _counter = _unitOfWork.Repo<NameListRepo>().GetNextCounter();
            _filePath = Path.Combine(tempFolderPath, string.Format("{0}.xml", _unitOfWork.Repo<NameListRepo>().GetNewIdentifier(this.FileType, _counter)));
        }

        public abstract string FileType { get; }
        public abstract string Generate();  
    }
}
