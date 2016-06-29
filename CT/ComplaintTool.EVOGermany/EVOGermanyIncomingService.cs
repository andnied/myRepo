using ComplaintTool.Common;
using ComplaintTool.Common.Utils;
using ComplaintTool.Common.Extensions;
using ComplaintTool.DataAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.DataAccess.Repos;
using System.Globalization;
using ComplaintTool.Common.Enum;
using ComplaintTool.Models;
using ComplaintTool.Common.Config;
using System.Data.Entity.Validation;
using ComplaintTool.EVOGermany.Incoming;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.EVOGermany
{
    public class EVOGermanyIncomingService : IComplaintProcess
    {
        #region Fields

        private static readonly ILogger _logger = LogManager.GetLogger();
        private readonly string _filePath;
        private IEVOGermanyIncoming _fileProcessor;

        #endregion
        //TODO: org id?
        #region IComplaintProcess

        public string OrganizationId
        {
            get
            {
                return "";
            }
        }

        public string ProcessName
        {
            get
            {
                return Globals.EvoGermanyIncomingService;
            }
        }

        public string ProcessFilePath
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(this.FilePath), Globals.TempFolderName, Path.GetFileName(this.FilePath));
            }
        }

        public string FilePath
        {
            get
            {
                return _filePath;
            }
        }

        #endregion

        #region Constructors

        public EVOGermanyIncomingService(string filePath, string fileType)
        {
            Guard.ThrowIf<ArgumentNullException>(filePath.IsEmpty(), "filePath");
            _filePath = filePath;

            if (fileType == "PDF")
                _fileProcessor = new EVOGermanyPdf();
            else if (fileType == "CLF")
                _fileProcessor = new EVOGermanyClf();
            else
                throw new ArgumentException("Invalid file type.");
        }

        #endregion

        #region MainProcess

        public int ProcessFile()
        {
            try
            {
                if (!(_fileProcessor.Validate(_filePath)))
                    return -1;

                if (!(ProcessUtil.MoveFileForProcessing(this)))
                    throw new Exception("Cannot copy file to temporary directory");

                var result = _fileProcessor.ProcessFile(ProcessFilePath);

                if (File.Exists(ProcessFilePath))
                    File.Delete(ProcessFilePath);

                return result;
            }
            catch (Exception ex)
            {
                _fileProcessor.NotifyMainException(ex);
                _logger.LogComplaintException(ex);
                return -1;
            }
        }

        #endregion
    }
}
