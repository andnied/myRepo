using ComplaintTool.Common.CTLogger;
using ComplaintTool.Common.Enum;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using ComplaintTool.Postilion.Incoming.Model;

namespace ComplaintTool.Postilion.Incoming
{
    public class PostilionProcessingFiles : PostilionIncomingBase
    {
        private static readonly ILogger _logger = LogManager.GetLogger();
        private readonly long _id;

        #region IComplaintProcess

        public override string ProcessFilePath
        {
            get { throw new NotImplementedException(); }
        }

        public override string FilePath
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        public PostilionProcessingFiles(long id)
        {
            _id = id;
        }

        #region MainProcess

        public override long Process()
        {
            try
            {
                ResponsePostilionFile responseFile;

                using (var unitOfWork = ComplaintUnitOfWork.Create())
                {
                    responseFile = unitOfWork.Repo<PostilionFilesRepo>().FindResponsePostilionFileById(_id);

                    if (responseFile == null)
                        throw new Exception(string.Format("Incoming file with id {0} not found.", _id));

                    responseFile.ProcesingStart = DateTime.UtcNow;
                    responseFile.ErrorFlag = true;
                    responseFile.Status = 1;

                    unitOfWork.Repo<PostilionFilesRepo>().Update(responseFile);
                    unitOfWork.Commit();
                }

                int messageCount;
                int errorCount;

                using (var unitOfWork = ComplaintUnitOfWork.Create())
                {
                    var messageList = unitOfWork.Repo<FileRepo>().GetReponsePostilionFile(responseFile.ResponsePostilionFileId);
                    var exceptionList = new List<string>();

                    if (messageList.Count > 0)
                    {
                        foreach (var message in messageList)
                            ProcessMessage(message, null, ref exceptionList);

                        responseFile.ProcesingFinished = DateTime.Now;
                        responseFile.ErrorFlag = false;
                        responseFile.Status = 3;

                        unitOfWork.Repo<PostilionFilesRepo>().Update(responseFile);
                        unitOfWork.Commit();

                        messageCount = messageList.Count;
                        errorCount = exceptionList.Count;
                    }
                    else
                        throw new Exception(string.Format("Postilion response file: {0} is empty.", responseFile.FileName));
                }

                _logger.LogComplaintEvent(124, string.Format("{0} with {1} messages and {2} errors.", responseFile.FileName, messageCount, errorCount));

                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogComplaintException(ex);
                return -1;
            }
        }

        #endregion

        #region PrivateMethods

        private static void ProcessMessage(string message, ComplaintUnitOfWork unitOfWork, ref List<string> exceptionList)
        {
            try
            {
                var lines = message.Split(';');
                var sourceFileNameWithoutExtension = lines[0].Trim();
                var tempCaseId = lines[1].Trim();
                var postilionStatus = lines[2].Trim();
                var mode = DetectFileMode(sourceFileNameWithoutExtension);
                var sourceFileName = sourceFileNameWithoutExtension.ToLower() + ".csv";
                var incoming = PostilionIncomingAbstract.GetInstance(mode, unitOfWork);
                incoming.ProcessMessage(sourceFileNameWithoutExtension, tempCaseId, postilionStatus, sourceFileName);
            }
            catch (Exception ex)
            {
                _logger.LogComplaintException(ex);
                exceptionList.Add(string.Format("Error for message: {0} , exception: {1}", message, ex));
            }
        }

        private static ReplyMode DetectFileMode(string message)
        {
            var charMode = message.Substring(1, 1);
            switch (charMode)
            {
                case "1":
                    return ReplyMode.Representment;
                default:
                    return ReplyMode.FeeCollection;
            }
        }

        #endregion
    }
}
