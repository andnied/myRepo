using System;
using System.IO;
using ComplaintTool.Common;
using ComplaintTool.Common.Config;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;

namespace ComplaintTool.MCProImageInterface.Incoming.Expired
{
    class ACUXmlFileProcessor : XmlFileProcessor
    {
        public ACUXmlFileProcessor(ComplaintUnitOfWork unitOfWork, XmlFileInfo xmlFile)
            : base(unitOfWork, xmlFile)
        {

        }

        public override bool Process(Guid bulkProcessKey)
        {
            var expiredFilesFolder = FileUtil.GetFolderExpired(ComplaintConfig.GetParameter(Globals.MCProImageBaseFolderParam));
            var destinationFilePath = Path.Combine(
                expiredFilesFolder,
                string.Format("{0}_{1:yyyyMMddHHmmss}{2}",
                    Path.GetFileNameWithoutExtension(ProcessFilePath),
                    DateTime.UtcNow,
                    Path.GetExtension(ProcessFilePath)));
            var msg = Path.GetFileName(ProcessFilePath);
            foreach (var tifFile in _xmlFile.TifFiles)
            {
                var tifDestinationFilePath = Path.Combine(expiredFilesFolder, string.Format("{0}_{1:yyyyMMddHHmmss}{2}",
                    Path.GetFileNameWithoutExtension(tifFile), DateTime.UtcNow, Path.GetExtension(tifFile)));
                FileUtil.MoveFile(tifFile, tifDestinationFilePath);
                msg = msg + ", " + Path.GetFileName(tifFile);
            }
            var objects = new object[] { msg };
            Logger.LogComplaintEvent(159, objects);
            return FileUtil.MoveFile(ProcessFilePath, destinationFilePath);
        }
    }
}
