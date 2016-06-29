using System;
using System.IO;
using ComplaintTool.Common;
using ComplaintTool.Common.Config;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;

namespace ComplaintTool.MCProImageInterface.Incoming.Expired
{
    public class SCUXmlFileProcessor : XmlFileProcessor
    {
        public SCUXmlFileProcessor(ComplaintUnitOfWork unitOfWork, XmlFileInfo xmlFile)
            : base(unitOfWork, xmlFile)
        {

        }

        public override bool Process(Guid bulkProcessKey)
        {
            string _expiredFilesFolder = FileUtil.GetFolderExpired(ComplaintConfig.GetParameter(Globals.MCProImageBaseFolderParam));
            var destinationFilePath = Path.Combine(
                _expiredFilesFolder,
                string.Format("{0}_{1:yyyyMMddHHmmss}{2}", 
                    Path.GetFileNameWithoutExtension(ProcessFilePath), 
                    DateTime.UtcNow, 
                    Path.GetExtension(ProcessFilePath)));
            return FileUtil.MoveFile(ProcessFilePath, destinationFilePath);
        }
    }
}
