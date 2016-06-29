using ComplaintTool.Common;
using ComplaintTool.Common.Utils;
using ComplaintTool.Common.Extensions;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using System;
using System.IO;
using System.Linq;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Postilion.Incoming
{
    public class PostilionResponseFiles : PostilionIncomingBase
    {
        #region Fields

        private static readonly ILogger Logger = LogManager.GetLogger();
        private readonly string _filePath;

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

        #region Constructors

        public PostilionResponseFiles(string filePath)
        {
            Guard.ThrowIf<ArgumentNullException>(filePath.IsEmpty(), "filePath");
            Guard.ThrowIf<FileNotFoundException>(!File.Exists(filePath), "filePath");
            _filePath = filePath;
        }

        #endregion

        #region MainProcess

        public override long Process()
        {
            try
            {
                if (!(ProcessUtil.MoveFileForProcessing(this)))
                    throw new Exception("Cannot copy file to temporary directory");

                ResponsePostilionFile responsePostilionFile;
                var fileName = Path.GetFileName(ProcessFilePath);

                if (Path.GetExtension(fileName) != ".response")
                {
                    Logger.LogComplaintEvent(523, "Postilion Incoming", Path.GetFileName(ProcessFilePath));
                    return -1;
                }

                using (var unitOfWork = ComplaintUnitOfWork.Create())
                {
                    if (unitOfWork.Repo<PostilionFilesRepo>().FindResponsePostilionFiles(fileName).Any())
                    {
                        Logger.LogComplaintEvent(500, "Postilion Incoming", ProcessFilePath);
                        return -1;
                    }

                    var streamId = unitOfWork.Repo<FileRepo>().AddIncomingFile(ProcessFilePath);
                    responsePostilionFile = CreateResponseFile(streamId);

                    unitOfWork.Repo<PostilionFilesRepo>().Add(responsePostilionFile);
                    unitOfWork.Commit();
                }

                if (File.Exists(ProcessFilePath))
                    File.Delete(ProcessFilePath);

                return responsePostilionFile.ResponsePostilionFileId;
            }
            catch (Exception ex)
            {
                Logger.LogComplaintException(ex);
                return -1;
            } 
        }

        #endregion

        #region PrivateMethods

        private ResponsePostilionFile CreateResponseFile(Guid streamId)
        {
            var responsePostilionFile = new ResponsePostilionFile()
            {
                FileName = Path.GetFileName(ProcessFilePath),
                stream_id = streamId,
                ProcesingStart = DateTime.UtcNow,
                IsSend = false,
                IsReceived = true,
                ErrorFlag = false,
                Status = 0
            };

            return responsePostilionFile;
        }

        #endregion
    }
}
