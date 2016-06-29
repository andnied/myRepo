using ComplaintTool.Common;
using ComplaintTool.Common.Utils;
using ComplaintTool.Common.Extensions;
using ComplaintTool.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Repos;
using System.Globalization;
using ComplaintTool.Common.Config;
using ComplaintTool.Common.CTLogger;
using eService.MCParser.Parsers;
using eService.MCParser.Model;

namespace ComplaintTool.MasterCard.Incoming
{
    public class MasterCardFileRegistration : IComplaintProcess// : MasterCardIncomingBase
    {
        #region Fields

        private static readonly ILogger _logger = LogManager.GetLogger();
        private readonly string _filePath;
        private const string IncomingHeader = "00-00-00-4A-31-36-34-34-80-00";
        private readonly string[] _masterCardExt;
        private DateTime _started;
        private DateTime _finished;

        #endregion

        #region IComplaintProcess

        public string OrganizationId
        {
            get
            {
                return Common.Enum.Organization.MC.ToString();
            }
        }

        public string ProcessName
        {
            get
            {
                return Globals.MCIncomingInterfaceProcessName;
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

        #region Constructor

        public MasterCardFileRegistration(string filePath)
        {
            Guard.ThrowIf<ArgumentNullException>(filePath.IsEmpty(), "filePath");
            _masterCardExt = ComplaintConfig.Instance.Parameters["MasterCardExt"].ParameterValue.Split('|');
            _filePath = filePath;
        }

        #endregion

        #region MainProcess

        public int Process(string arn = null)
        {
            try
            {
                //TODO: change extensions in db
                //if (!(_masterCardExt.Contains(Path.GetExtension(_filePath))))
                //{
                //    _logger.LogComplaintEvent(523, new object[] { "MasterCard", Path.GetFileName(ProcessFilePath) });
                //    return;
                //}

                if (!(ProcessUtil.MoveFileForProcessing(this)))
                    throw new Exception("Cannot copy file to temporary directory");

                if (IncomingHeader.Equals(ReadHeader()))
                {
                    using (var unitOfWork = ComplaintUnitOfWork.Create())
                    {
                        var id = this.GetId();
                        if (!(unitOfWork.Repo<RegOrgIncomingFilesRepo>().RegOrgExists(id)))
                        {
                            var streamId = unitOfWork.Repo<FileRepo>().AddIncomingFile(ProcessFilePath);
                            var regOrg = CreateIncomingFile(id, streamId);

                            unitOfWork.Repo<RegOrgIncomingFilesRepo>().AddRegOrg(regOrg);
                            unitOfWork.Commit();

                            _logger.LogComplaintEvent(101, new object[] { "MasterCard", regOrg.Name });

                            if (File.Exists(ProcessFilePath))
                                File.Delete(ProcessFilePath);

                            return regOrg.FileId;
                        }
                        else
                        {
                            _logger.LogComplaintEvent(500, new object[] { "MasterCard", Path.GetFileName(ProcessFilePath) });
                            return -1;
                        }
                    }
                }
                else
                {
                    _logger.LogComplaintEvent(503, new object[] { "MasterCard", Path.GetFileName(ProcessFilePath), ProcessFilePath });
                    return -1;
                }
            }
            catch (Exception ex)
            {
                _logger.LogComplaintEvent(517, new object[] { "MasterCard", ex.Message });
                _logger.LogComplaintException(ex);
                return -1;
            }
        }

        #endregion

        #region PrivateMethods

        private string GetId()
        {
            var parser = new Blk2ipmAsc();
            return parser.GetIncomingFileId(ProcessFilePath);
        }

        private string ReadHeader()
        {
            var headerArray = new byte[10];

            using (var fs = new FileStream(ProcessFilePath, FileMode.Open, FileAccess.Read))
            {
                fs.Read(headerArray, 0, headerArray.Length);
                fs.Close();
            }

            return BitConverter.ToString(headerArray);
        }

        private RegOrgIncomingFile CreateIncomingFile(string id, Guid streamId)
        {
            RegOrgIncomingFile regOrg = null;
            this._started = DateTime.UtcNow;
            var fileCreationTime = new FileInfo(ProcessFilePath).CreationTime;

            regOrg = new RegOrgIncomingFile()
            {
                Name = Path.GetFileName(ProcessFilePath),
                OrganizationId = this.OrganizationId,
                ErrorFlag = false,
                Status = 2,
                IncomingFileID = id.ToString(CultureInfo.InvariantCulture),
                stream_id = streamId,
                CreationDateTime = fileCreationTime,
                ProcessingMode = 0,
                ProcesingStart = this._started
            };

            return regOrg;
        }

        #endregion
    }
}
