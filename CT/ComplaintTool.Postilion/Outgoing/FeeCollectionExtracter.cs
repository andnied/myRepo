using ComplaintTool.Common.CTLogger;
using ComplaintTool.Common.Enum;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using System;
using System.Collections;
using System.IO;
using System.Security.Principal;
using System.Text;
using ComplaintTool.Postilion.Outgoing.Model.FeeCollection;

namespace ComplaintTool.Postilion.Outgoing
{
    class FeeCollectionExtracter : ExtracterAbstract
    {
        private static readonly ILogger Logger = LogManager.GetLogger();
        private DateTime _startDateTime;
        private DateTime _endDateTime;

        public FeeCollectionExtracter(Common.Enum.Organization org, ComplaintUnitOfWork unitOfWork)
            : base(org, unitOfWork)
        {
        }
        
        //public override System.Collections.ICollection GetExtract()
        //{
        //    return ComplaintUnitOfWork.Get().Repo<FeeCollectionRepo>().FindFeeCollectionExtractsForExtract(Org.ToString(), 0);
        //}

        public override ICollection GetExtract()
        {
            throw new NotImplementedException();
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
                        var mc4105 = new MC4105();
                        textWriter.WriteLine(mc4105.GetHeader());
                        break;
                    case Common.Enum.Organization.VISA:
                        var visa4105 = new VISA4105();
                        textWriter.WriteLine(visa4105.GetHeader());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                foreach (var extr in colForExtract)
                {
                    var feeExtr = (FeeCollectionExtract) extr;
                    var encryptedFeeExtract =
                        UnitOfWork.Repo<FeeCollectionRepo>().GetFeeCollectionExtract(feeExtr.FeeCollectionExtractId);
                    textWriter.WriteLine(encryptedFeeExtract.PostilionExtractWithBase64String);
                }
            }

            return MoveFileToBaseFolder(tempFile, baseFolder);
        }

        public override object CreatePostilionFile(string filePath)
        {
            var guid = UnitOfWork.Repo<FileRepo>().AddOutgoingFile(filePath);
            var windowsIdentity = WindowsIdentity.GetCurrent();
            _endDateTime = DateTime.UtcNow;

            var feeCollectionPostilionFile = UnitOfWork.Repo<FeeCollectionRepo>().Add(new FeeCollectionPostilionFile()
            {
                FileName = Path.GetFileName(filePath), stream_id = guid, ProcesingStart = _startDateTime, ProcesingFinished = _endDateTime, IsSend = true, IsReceived = false, ErrorFlag = false, Status = 2, InsertDate = DateTime.UtcNow, InsertUser = windowsIdentity != null ? windowsIdentity.Name : "WindowsIdentity error."
            });

            Logger.LogComplaintEvent(117, "Postilion FeeCollection", Path.GetFileName(filePath));

            return feeCollectionPostilionFile;
        }

        //public override object WriteExtractToFileAndCreatePostFile(System.Collections.ICollection colForExtract, string folderPath)
        //{
        //    FeeCollectionPostilionFile feeCollectionPostilionFile = null;
        //    var processKey = Guid.NewGuid();
        //    var baseFolder = FileUtil.GetFolder(folderPath);
        //    var tempFolder = FileUtil.GetFolderTemp(folderPath);
        //    var tempFile = tempFolder + @"\" + base.FileName(Common.Enum.ReplyMode.FeeCollection);
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

        //        feeCollectionPostilionFile = _unitOfWork.Repo<FeeCollectionRepo>().Add(new FeeCollectionPostilionFile()
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

        //        _logger.LogComplaintEvent(117, new object[] { "Postilion FeeCollection", Path.GetFileName(baseFilePath) });
        //    }
        //    else
        //        return null;

        //    return feeCollectionPostilionFile;
        //}

        //private void WriteExtractToFile(string filePath, System.Collections.ICollection colForExtract)
        //{
        //    using (var textWriter = new StreamWriter(filePath, true, Encoding.GetEncoding(28605)))
        //    {
        //        switch (_org)
        //        {
        //            case Common.Enum.Organization.MC:
        //                var mc4105 = new MC4105();
        //                textWriter.WriteLine(mc4105.GetHeader());
        //                break;
        //            case Common.Enum.Organization.VISA:
        //                var visa4105 = new VISA4105();
        //                textWriter.WriteLine(visa4105.GetHeader());
        //                break;
        //        }

        //        foreach (var extr in colForExtract)
        //        {
        //            var feeExtr = (FeeCollectionExtract)extr;
        //            textWriter.WriteLine(feeExtr.PostilionExtractWithBase64String);
        //        }
        //    }
        //}

        public override string UpdateExtracts(System.Collections.ICollection colForExtract, object postilionFile)
        {
            if (!(postilionFile is FeeCollectionPostilionFile))
            {
                throw new ArgumentException("Incorrect object type. Should be FeeCollectionPostilionFile.");
            }

            var feeCollectionPostilionFile = (FeeCollectionPostilionFile) postilionFile;
            var fileName = Path.GetFileName(feeCollectionPostilionFile.FileName);

            foreach (var extr in colForExtract)
            {
                ((FeeCollectionExtract) extr).ProcessKey = feeCollectionPostilionFile.stream_id;
                ((FeeCollectionExtract) extr).FeeCollectionPostilionFileId = feeCollectionPostilionFile.FeeCollectionPostilionFileId;
                ((FeeCollectionExtract) extr).ErrorFlag = false;
                ((FeeCollectionExtract) extr).Status = 2;
                UnitOfWork.Repo<FeeCollectionRepo>().Update((FeeCollectionExtract) extr);
            }

            if (feeCollectionPostilionFile.stream_id != null)
                UnitOfWork.Repo<ProcessRepo>().AddProcessKey(feeCollectionPostilionFile.stream_id.Value, fileName, ProcessingStatus.Administration);

            return fileName;
        }

        public override void Notify(ILogger logger, string fileName)
        {
            logger.LogComplaintEvent(127, Org.ToString(), Path.GetFileName(fileName));
        }
    }
}
