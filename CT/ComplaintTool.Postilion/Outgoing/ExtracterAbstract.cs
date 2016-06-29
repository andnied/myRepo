using ComplaintTool.Common.CTLogger;
using ComplaintTool.Common.Enum;
using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Repos;
using System.Globalization;
using System.IO;

namespace ComplaintTool.Postilion.Outgoing
{
    public abstract class ExtracterAbstract
    {
        public static ExtracterAbstract GetExtracter(PostilionServiceEnum type, Organization org, ComplaintUnitOfWork unitOfWork)
        {
            switch (type)
            {
                case PostilionServiceEnum.FeeCollection:
                    return new FeeCollectionExtracter(org, unitOfWork);
                case PostilionServiceEnum.Representment:
                    return new RepresentmentExtracter(org, unitOfWork);
            }

            return null;
        }

        #region AbstractMethods

        public abstract System.Collections.ICollection GetExtract();

        public abstract string WriteExtractToFile(System.Collections.ICollection colForExtract, string folderPath);

        public abstract object CreatePostilionFile(string filePath);

        //public abstract object WriteExtractToFileAndCreatePostFile(System.Collections.ICollection colForExtract, string folderPath);

        public abstract string UpdateExtracts(System.Collections.ICollection colForExtract, object postilionFile);

        public abstract void Notify(ILogger logger, string fileName);

        #endregion

        #region ProtectedMethods

        protected Organization Org;
        protected ComplaintUnitOfWork UnitOfWork;

        protected ExtracterAbstract(Organization org, ComplaintUnitOfWork unitOfWork)
        {
            Org = org;
            UnitOfWork = unitOfWork;
        }

        protected string FileName(ReplyMode replyMode)
        {
            var count = 0;
            var unitOfWork = ComplaintUnitOfWork.Get();

            switch (replyMode)
            {
                case ReplyMode.Representment:
                    count = unitOfWork.Repo<RepresentmentRepo>().GetRepresentmentPostilionFilesCount();
                    break;
                case ReplyMode.FeeCollection:
                    count = unitOfWork.Repo<FeeCollectionRepo>().GetFeeCollectionPostilionFilesCount();
                    break;
            }

            count++;

            return ((int)Org).ToString(CultureInfo.InvariantCulture) + ((int)replyMode).ToString(CultureInfo.InvariantCulture) + count.ToString(CultureInfo.InvariantCulture).PadLeft(10, '0') + ".csv";
        }
        
        protected string MoveFileToBaseFolder(string filePath, string baseFolder)
        {
            var newFilePath = baseFolder + @"\" + Path.GetFileName(filePath);
            if (File.Exists(newFilePath)) File.Delete(newFilePath);
            File.Copy(filePath, newFilePath);
            if (File.Exists(filePath)) File.Delete(filePath);

            return newFilePath;
        }
        
        #endregion
    }
}
