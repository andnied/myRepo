using System;
using System.IO;
using System.Linq;
using ComplaintTool.Common.Extensions;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Common.Utils
{
    public static class FileUtil
    {
        private static readonly ILogger _logger = LogManager.GetLogger();

        public static void CleanFolder(string folderPath)
        {
            var extractedFiles = Directory.GetFiles(folderPath);
            foreach (var file in extractedFiles)
            {
                if (file == null) continue;
                if (Path.GetExtension(file).ToLower().Equals(".db")) continue;
                File.Delete(file);
            }
        }

        public static void CleanTempFiles(string tempFolderPath, string caseId, string fileName, string tiffPath)
        {
            try
            {
                if (!caseId.IsEmpty() && !fileName.IsEmpty() && !tiffPath.IsEmpty())
                {
                    var oldName = Path.Combine(tempFolderPath, caseId, ".tif");
                    tiffPath = Path.Combine(tempFolderPath, fileName, ".tif"); //TODO: Do weryfikacji

                    if (File.Exists(oldName))
                        File.Move(oldName, tiffPath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogComplaintException(ex);
            }

            var pdfFileList =
                Directory.GetFiles(tempFolderPath, "*.pdf").Union(Directory.GetFiles(tempFolderPath, "*.PDF"));

            foreach (var pdf in pdfFileList)
                File.Delete(pdf);

            var pngFileList = Directory.GetFiles(tempFolderPath, "*.png").Union(Directory.GetFiles(tempFolderPath, "*.PNG"));

            foreach (var png in pngFileList)
                File.Delete(png);
        }

        public static bool MoveFile(string sourceFilePath, string destFilePath)
        {
            if (!File.Exists(sourceFilePath))
                return false;

            string destDir = Path.GetDirectoryName(destFilePath);
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);

            File.Copy(sourceFilePath, destFilePath, true);
            File.Delete(sourceFilePath);
            return true;
        }

        public static string GetFolder(string uriString)
        {
            return
                Path.GetFullPath(
                    new Uri(uriString).LocalPath)
                    .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                    .ToUpperInvariant();
        }

        public static string GetFolderTemp(string uriString)
        {
            return Path.Combine(GetFolder(uriString), Globals.TempFolderName);
        }

        public static string GetFolderExpired(string uriString)
        {
            return Path.Combine(GetFolder(uriString), Globals.ExpiredFolderName);
        }
    }
}
