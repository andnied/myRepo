using ComplaintTool.Common;
using ComplaintTool.Common.Config;
using ComplaintTool.Common.CTLogger;
using ComplaintTool.Common.Extensions;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.Visa.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Visa.Incoming
{
    public class VisaIncomingRegistration : VisaIncomingBase
    {
        #region Fields

        private static readonly ILogger _logger = LogManager.GetLogger();
        private readonly string _filePath;
        private readonly string[] _visaExtensions = ComplaintConfig.Instance.Parameters["VisaExt"].ParameterValue.Split('|');

        #endregion

        #region IComplaintProcess

        public override string ProcessFilePath
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(this.FilePath), Globals.TempFolderName, Path.GetFileName(this.FilePath));
            }
        }

        public override string FilePath
        {
            get
            {
                return _filePath;
            }
        }

        #endregion

        #region Constructor

        public VisaIncomingRegistration(string filePath)
        {
            Guard.ThrowIf<ArgumentNullException>(filePath.IsEmpty(), "filePath");
            Guard.ThrowIf<FileNotFoundException>(!File.Exists(filePath), "filePath");
            _filePath = filePath;
        }

        #endregion

        #region MainProcess

        public override int Process()
        {
            try
            {
                RegOrgIncomingFile regOrg = null;

                if (!(ProcessUtil.MoveFileForProcessing(this)))
                    throw new Exception("Cannot copy file to temporary directory");

                if (!(_visaExtensions.Contains(Path.GetExtension(ProcessFilePath))))
                {
                    _logger.LogComplaintEvent(523, new object[] { "Visa", Path.GetFileName(ProcessFilePath) });
                    return -1;
                }

                var incomingId = this.GetIncomingId();

                using (var unitOfWork = ComplaintUnitOfWork.Create())
                {
                    if (unitOfWork.Repo<RegOrgIncomingFilesRepo>().RegOrgExists(incomingId))
                    {
                        _logger.LogComplaintEvent(500, "Visa", Path.GetFileName(ProcessFilePath));
                        return -1;
                    }

                    var streamId = unitOfWork.Repo<FileRepo>().AddIncomingFile(ProcessFilePath);
                    var fileCreationTime = new FileInfo(ProcessFilePath).CreationTime;

                    regOrg = unitOfWork.Repo<RegOrgIncomingFilesRepo>().AddRegOrg(new RegOrgIncomingFile()
                    {
                        Name = Path.GetFileName(ProcessFilePath),
                        OrganizationId = this.OrganizationId,
                        ErrorFlag = false,
                        IncomingFileID = incomingId,
                        stream_id = streamId,
                        CreationDateTime = fileCreationTime,
                        ProcesingStart = DateTime.UtcNow,
                        Status = 2,
                        ProcessingMode = 0
                    });

                    unitOfWork.Commit();
                }

                if (File.Exists(ProcessFilePath))
                    File.Delete(ProcessFilePath);

                return regOrg.FileId;
            }
            catch (Exception ex)
            {
                _logger.LogComplaintException(ex);
                return -1;
            }
        }

        #endregion

        #region PrivateMethods

        private string GetIncomingId()
        {
            var line = string.Empty;

            using (var reader = new StreamReader(ProcessFilePath))
            {
                line = reader.ReadLine();
            }

            var tc90 = new TC90(line);
            var processingDate = ComplaintTool.Common.Utils.Convert.JulianDateToDateTimeForVisa(tc90.ProcessingDate).ToString("yyyyMMdd");
            var incomingId = string.Format("{0}{1}{2}", tc90.ProcessingBIN, processingDate, tc90.IncomingFileID);

            return incomingId;
        }

        #endregion
    }
}
