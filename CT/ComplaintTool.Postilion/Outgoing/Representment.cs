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
using NLog;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.Common.Enum;
using System.Globalization;
using ComplaintService.Postilion.Model.Representment;
using ComplaintTool.DataAccess.Model;

namespace ComplaintTool.Postilion.Outgoing
{
    public class Representment : IComplaintProcess
    {
        private readonly EventLog _eventLog = new EventLog { Log = Globals.EventLogName, Source = Globals.EventLogSource };
        private readonly ComplaintUnitOfWork _unitOfWork = ComplaintUnitOfWork.Get();
        private readonly string _baseFolder; //= FileUtil.GetFolder(ComplaintConfig.GetParameter(Globals.VisaOutgoingInterfaceProcessName));
        private readonly string _tempFolder; //= FileUtil.GetFolderTemp(ComplaintConfig.GetParameter(Globals.VisaOutgoingInterfaceProcessName));
        private DateTime _startDateTime;
        private DateTime _endDateTime;

        #region IComplaintProcess
        public string OrganizationId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string ProcessName
        {
            get
            {
                return Globals.VisaOutgoingInterfaceProcessName;
            }
        }

        public string ProcessFilePath
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string FilePath
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public EventLog EventLog
        {
            get
            {
                return _eventLog;
            }
        }

        public Logger Logger
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region Constructors

        public Representment(string folderPath)
        {
            Guard.ThrowIf<ArgumentNullException>(folderPath.IsEmpty(), "folderPath");
            _baseFolder = folderPath;
            _tempFolder = Path.Combine(Path.GetDirectoryName(this._baseFolder), Globals.TempFolderName);
            // TODO sciezka tymczasowa
            //_filePath = Path.Combine(folderPath, _bulkProcessKey.ToString());
        }

        #endregion

        public void Process(ComplaintTool.Common.Enum.Organization type) //TODO: czy to dobrze ze wszystkie representments sie robia na jednej transkacji?
        {
            int count = 0;
            int errorCount = 0;
            _startDateTime = DateTime.UtcNow;
            string tempFileName = null;
            List<RepresentmentExtract> representmentExtractList = null;

            using (_unitOfWork)
            {
                var representments = _unitOfWork.Repo<RepresentmentRepo>().FindRepresentmentsForExtract(type.ToString(), 0);

                if (representments.Any())
                {
                    if (!Directory.Exists(_tempFolder))
                        Directory.CreateDirectory(_tempFolder);

                    tempFileName = _tempFolder + this.GetFileName(type, ReplyMode.Representment);
                    if (File.Exists(tempFileName)) File.Delete(tempFileName);

                    representmentExtractList = new List<RepresentmentExtract>();

                    #region GenerateFile
                    using (TextWriter textWriter = new StreamWriter(tempFileName, true, new UTF8Encoding(false)))
                    {
                        var mc4103 = new MC4103(); //
                        textWriter.WriteLine(mc4103.GetHeader()); //

                        foreach (var rep in representments)
                        {
                            var mc = new MasterCard.Outgoing.MasterCard(); //

                            bool isException;
                            bool isPostilionError = false;
                            mc4103 = mc.GetMC4103Data(rep, out isException, out isPostilionError); //almost the same for visa

                            if (!isException)
                            {
                                var lineWhiteBase64String = mc4103.GetRecordWhiteBase64String();
                                var line = mc4103.GetRecord();
                                textWriter.WriteLine(lineWhiteBase64String);

                                representmentExtractList.Add(new RepresentmentExtract
                                {
                                    CaseId = rep.CaseId,
                                    RepresentmentId = rep.RepresentmentId,
                                    PostilionExtractClearString = line,
                                    PostilionExtractWithBase64String = lineWhiteBase64String,
                                    ErrorFlag = true,
                                    Status = 0
                                });

                                rep.Status = 2;
                                _unitOfWork.Repo<RepresentmentRepo>().Update(rep);
                                count++;

                                _unitOfWork.Repo<ComplaintRepo>().AddComplaintNote(rep.CaseId,
                                    String.Format("Representment case: {0} extract to file: {1}"
                                    , rep.CaseId
                                    , Path.GetFileName(tempFileName)));
                            }
                            else
                            {
                                if (!isPostilionError)
                                {
                                    rep.Status = 10;
                                    _unitOfWork.Repo<RepresentmentRepo>().Update(rep);
                                }
                                errorCount++;
                                _eventLog.WriteEntry("Representment case " + rep.CaseId +
                                                     " is corupted. View Nlog in data base.",
                                    EventLogEntryType.Error,
                                    600);
                                _unitOfWork.Repo<ComplaintRepo>().AddComplaintNote(rep.CaseId,
                                    String.Format("Representment case: {0} is corupted."
                                    , rep.CaseId));
                            }
                        }
                    }
                    #endregion

                    _unitOfWork.Commit();
                }
                else
                {
                    //TODO: handling
                    //Logger.Info(String.Format(ProcessConfig.DictionaryNotification[122].MessageText, "MasterCard"));
                    //_eventLog.WriteEntry(
                    //    String.Format(ProcessConfig.DictionaryNotification[122].MessageText, "MasterCard"),
                    //    EventLogEntryType.Information, ProcessConfig.DictionaryNotification[122].MessageEventNamber);
                    //Mailing.SendEmail("Representment process.",
                    //    String.Format(ProcessConfig.DictionaryNotification[122].MessageText, "MasterCard"),
                    //    ProcessConfig.DictionaryNotification[122].MailingGroups);
                    return;
                }
            }

            if (representmentExtractList.Count != representmentExtractList.Count(x => x.Status == 10))
            {
                using (_unitOfWork)
                {
                    var fileName = _baseFolder + @"\" + Path.GetFileName(tempFileName);
                    if (File.Exists(fileName)) File.Delete(fileName);
                    File.Copy(tempFileName, fileName);

                    var streamId = _unitOfWork.Repo<FileRepo>().AddOutgoingFile(fileName);
                    if (File.Exists(tempFileName)) File.Delete(tempFileName);
                    this._endDateTime = DateTime.UtcNow;

                    var representmentPostilionFile = new RepresentmentPostilionFile()
                    {
                        FileName = Path.GetFileName(fileName),
                        stream_id = streamId,
                        ProcesingStart = _startDateTime,
                        ProcesingFinished = _endDateTime,
                        IsSend = true,
                        IsReceived = false,
                        ErrorFlag = false,
                        Status = 2
                    };
                    _unitOfWork.Repo<RepresentmentRepo>().Add(representmentPostilionFile);

                    foreach (var repExtr in representmentExtractList)
                    {
                        repExtr.RepresentmentPostilionFileId = representmentPostilionFile.RepresentmentPostilionFileId;
                        repExtr.ErrorFlag = false;
                        repExtr.Status = 2;
                        _unitOfWork.Repo<RepresentmentRepo>().Add(repExtr);
                    }

                    _unitOfWork.Commit();
                }

                //TODO
                //_eventLog.WriteEntry(
                //           String.Format(ProcessConfig.DictionaryNotification[119].MessageText, @"MasterCard"),
                //           EventLogEntryType.Information,
                //           ProcessConfig.DictionaryNotification[119].MessageEventNamber);
                //Mailing.SendEmail("Representment process.",
                //    String.Format(ProcessConfig.DictionaryNotification[123].MessageText, "MasterCard", Path.GetFileName(tempFileName)),
                //    ProcessConfig.DictionaryNotification[123].MailingGroups);
            }
            else
            {
                if (File.Exists(tempFileName)) File.Delete(tempFileName);
                //throw new Exception("MasterCard representment data is corupted.");
                throw new Exception("MasterCard Representment data is corrupted: all items have the status 10.");
            }

            //TODO
            //Logger.Info(String.Format(ProcessConfig.DictionaryNotification[123].MessageText, "MasterCard", Path.GetFileName(tempFileName)));
            //_eventLog.WriteEntry(
            //    String.Format(ProcessConfig.DictionaryNotification[123].MessageText, "MasterCard", Path.GetFileName(tempFileName)),
            //    EventLogEntryType.Information, ProcessConfig.DictionaryNotification[123].MessageEventNamber);
            //Mailing.SendEmail("Representment process.",
            //    String.Format(ProcessConfig.DictionaryNotification[123].MessageText, "MasterCard", Path.GetFileName(tempFileName)),
            //    ProcessConfig.DictionaryNotification[123].MailingGroups);
        }

        private string GetFileName(ComplaintTool.Common.Enum.Organization organization, ReplyMode replyMode)
        {
            var count = 0;

            using (_unitOfWork)
            {
                switch (replyMode)
                {
                    case ReplyMode.Representment:
                        count = _unitOfWork.Repo<RepresentmentRepo>().GetRepresentmentPostilionFilesCount();
                        break;
                    case ReplyMode.FeeCollection:
                        count = _unitOfWork.Repo<RepresentmentRepo>().GetFeeCollectionPostilionFilesCount();
                        break;
                }
            }

            count++;
            return ((int)organization).ToString(CultureInfo.InvariantCulture) + ((int)replyMode).ToString(CultureInfo.InvariantCulture) + count.ToString(CultureInfo.InvariantCulture).PadLeft(10, '0') + ".csv";
        }
    }
}
