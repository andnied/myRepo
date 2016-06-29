using ComplaintTool.Common.CTLogger;
using ComplaintTool.Common.Enum;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using System;
using System.IO;
using System.Security.Principal;
using System.Text;
using ComplaintTool.Postilion.Outgoing.Model.Representment;

namespace ComplaintTool.Postilion.Outgoing
{
    public class RepresentmentExtracter : ExtracterAbstract
    {
        private static readonly ILogger Logger = LogManager.GetLogger();
        private DateTime _startDateTime;
        private DateTime _endDateTime;

        public RepresentmentExtracter(Common.Enum.Organization org, ComplaintUnitOfWork unitOfWork)
            : base(org, unitOfWork)
        {
        }

        public override System.Collections.ICollection GetExtract()
        {
            return ComplaintUnitOfWork.Get().Repo<RepresentmentRepo>().FindRepresentmentExtractsForExtract(Org.ToString(), 0);
        }

        public override string WriteExtractToFile(System.Collections.ICollection colForExtract, string folderPath)
        {
            _startDateTime = DateTime.UtcNow;
            var baseFolder = FileUtil.GetFolder(folderPath);
            var tempFolder = FileUtil.GetFolderTemp(folderPath);
            var tempFile = tempFolder + @"\" + FileName(ReplyMode.FeeCollection);

            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);

            if (File.Exists(tempFile))
                File.Delete(tempFile);

            using (var textWriter = new StreamWriter(tempFile, true, Encoding.GetEncoding(28605)))
            {
                switch (Org)
                {
                    case Common.Enum.Organization.MC:
                        var mc4103 = new MC4103();
                        textWriter.WriteLine(mc4103.GetHeader());
                        break;
                    case Common.Enum.Organization.VISA:
                        var visa4103 = new VISA4103();
                        textWriter.WriteLine(visa4103.GetHeader());
                        break;
                }

                foreach (var extr in colForExtract)
                {
                    var repExtr = (RepresentmentExtract)extr;
                    var encryptedRepExtract = UnitOfWork.Repo<RepresentmentRepo>().GetRepresentmentExtract(repExtr.RepresentmentExtractId);
                    textWriter.WriteLine(encryptedRepExtract.PostilionExtractWithBase64String);
                }
            }

            return MoveFileToBaseFolder(tempFile, baseFolder);
        }

        public override object CreatePostilionFile(string filePath)
        {
            var guid = UnitOfWork.Repo<FileRepo>().AddOutgoingFile(filePath);
            var windowsIdentity = WindowsIdentity.GetCurrent();
            _endDateTime = DateTime.UtcNow;

            var representmentPostilionFile = UnitOfWork.Repo<RepresentmentRepo>().Add(new RepresentmentPostilionFile()
            {
                FileName = Path.GetFileName(filePath),
                stream_id = guid,
                ProcesingStart = _startDateTime,
                ProcesingFinished = _endDateTime,
                IsSend = true,
                IsReceived = false,
                ErrorFlag = false,
                Status = 2,
                InsertDate = DateTime.UtcNow,
                InsertUser = windowsIdentity != null ? windowsIdentity.Name : "WindowsIdentity error."
            });

            Logger.LogComplaintEvent(117, "Postilion Representment", Path.GetFileName(filePath));

            return representmentPostilionFile;
        }

        //public override object WriteExtractToFileAndCreatePostFile(System.Collections.ICollection colForExtract, string folderPath)
        //{
        //    RepresentmentPostilionFile representmentPostilionFile = null;
        //    var unitOfWork = ComplaintUnitOfWork.Get();
        //    var processKey = Guid.NewGuid();
        //    var baseFolder = FileUtil.GetFolder(folderPath);
        //    var tempFolder = FileUtil.GetFolderTemp(folderPath);
        //    var tempFile = tempFolder + @"\" + base.FileName(Common.Enum.ReplyMode.Representment);
        //    var startDateTime = DateTime.UtcNow;

        //    if (!Directory.Exists(tempFolder))
        //        Directory.CreateDirectory(tempFolder);

        //    if (File.Exists(tempFile)) File.Delete(tempFile);

        //    this.WriteExtractToFile(tempFile, colForExtract);

        //    if (File.Exists(tempFile))
        //    {
        //        var baseFilePath = base.MoveFileToBaseFolder(tempFile, baseFolder);
        //        var guid = _unitOfWork.Repo<FileRepo>().AddOutgoingFile(baseFilePath);
        //        var endDateTime = DateTime.UtcNow;
        //        var windowsIdentity = WindowsIdentity.GetCurrent();

        //        representmentPostilionFile = _unitOfWork.Repo<RepresentmentRepo>().Add(new RepresentmentPostilionFile()
        //        {
        //            FileName = Path.GetFileName(baseFilePath),
        //            stream_id = processKey,
        //            ProcesingStart = startDateTime,
        //            ProcesingFinished = endDateTime,
        //            IsSend = true,
        //            IsReceived = false,
        //            ErrorFlag = false,
        //            Status = 2,
        //            InsertDate = DateTime.UtcNow,
        //            InsertUser = windowsIdentity != null ? windowsIdentity.Name : "WindowsIdentity error."
        //        });

        //        _logger.LogComplaintEvent(117, new object[] { "Postilion Representment", Path.GetFileName(baseFilePath) });
        //    }
        //    else
        //        return null;

        //    return representmentPostilionFile;
        //}

        //private void WriteExtractToFile(string filePath, System.Collections.ICollection colForExtract)
        //{
        //    using (var textWriter = new StreamWriter(filePath, true, Encoding.GetEncoding(28605)))
        //    {
        //        switch (_org)
        //        {
        //            case Common.Enum.Organization.MC:
        //                var mc4103 = new MC4103();
        //                textWriter.WriteLine(mc4103.GetHeader());
        //                break;
        //            case Common.Enum.Organization.VISA:
        //                var visa4103 = new VISA4103();
        //                textWriter.WriteLine(visa4103.GetHeader());
        //                break;
        //        }

        //        foreach (var extr in colForExtract)
        //        {
        //            var repExtr = (RepresentmentExtract)extr;
        //            textWriter.WriteLine(repExtr.PostilionExtractWithBase64String);
        //        }
        //    }
        //}

        public override string UpdateExtracts(System.Collections.ICollection colForExtract, object postilionFile)
        {
            if (!(postilionFile is RepresentmentPostilionFile))
            {
                throw new ArgumentException("Incorrect object type. Should be RepresentmentPostilionFile.");
            }

            var representmentPostilionFile = (RepresentmentPostilionFile)postilionFile;
            var fileName = Path.GetFileName(representmentPostilionFile.FileName);

            foreach (var extr in colForExtract)
            {
                ((RepresentmentExtract)extr).ProcessKey = representmentPostilionFile.stream_id;
                ((RepresentmentExtract)extr).RepresentmentPostilionFileId = representmentPostilionFile.RepresentmentPostilionFileId;
                ((RepresentmentExtract)extr).ErrorFlag = false;
                ((RepresentmentExtract)extr).Status = 2;
                UnitOfWork.Repo<RepresentmentRepo>().Update((RepresentmentExtract)extr);
            }

            if (representmentPostilionFile.stream_id != null)
                UnitOfWork.Repo<ProcessRepo>().AddProcessKey(
                    representmentPostilionFile.stream_id.Value
                    , fileName
                    , ProcessingStatus.Administration);

            return fileName;
        }

        public override void Notify(ILogger logger, string fileName)
        {
            logger.LogComplaintEvent(123, Org.ToString(), Path.GetFileName(fileName));
        }
    }
}
