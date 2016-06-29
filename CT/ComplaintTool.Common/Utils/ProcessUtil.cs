using System.IO;
using System.Linq;
using ComplaintTool.Common.Config;
using ComplaintTool.Common.Extensions;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Common.Utils
{
    public static class ProcessUtil
    {
        private static readonly ILogger Logger = LogManager.GetLogger();

        public static bool MoveFileForProcessing(IComplaintProcess process)
        {
            if (!File.Exists(process.FilePath) || File.Exists(process.ProcessFilePath)) return false;
            var processingDirectory = Path.GetDirectoryName(process.ProcessFilePath);
            if (processingDirectory != null && !Directory.Exists(processingDirectory))
                Directory.CreateDirectory(processingDirectory);

            Logger.LogComplaintEvent(109, process.ProcessName, Path.GetFileName(process.FilePath), process.ProcessFilePath);
            File.Copy(process.FilePath, process.ProcessFilePath, true);
            File.Delete(process.FilePath);
            return true;
        }

        public static bool Clean(IComplaintProcess process)
        {
            if (!File.Exists(process.ProcessFilePath))
                return false;
            File.Delete(process.ProcessFilePath);

            string zipExtractionFolder = Extract.GetExtractionFolder(process.ProcessFilePath);
            if (Directory.Exists(zipExtractionFolder) && !Directory.GetFiles(zipExtractionFolder).Any())
                Directory.Delete(zipExtractionFolder);
            return true;
        }
    }
}
