using System;
using System.IO;
using System.IO.Compression;
using ComplaintTool.Common.Extensions;

namespace ComplaintTool.Common.Utils
{
    /// <summary>
    /// Contains help methods for zip files operations.
    /// </summary>
    public static class Extract
    {
        public static bool IsIncomingMCZipFile(string filePath)
        {
            try
            {
                var headerArray = new byte[4];
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    fs.Read(headerArray, 0, headerArray.Length);
                    fs.Close();
                }
                var header = BitConverter.ToString(headerArray);
                return Globals.MCIncomingZipHeader.Equals(header);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsIncomingMCXmlFile(string filePath)
        {
            try
            {
                var ext = Path.GetExtension(filePath);
                return ext == ".xml" || ext == ".XML";
            }
            catch
            {
                return false;
            }
        }

        public static bool IsIncomingMCAuditFile(string filePath)
        {
            try
            {
                var ext = Path.GetExtension(filePath);
                return ext == ".txt" || ext == ".TXT";
            }
            catch
            {
                return false;
            }
        }

        public static string ExtractIncomingMCZipFile(string filePath)
        {
            string zipExtractionFolder = GetExtractionFolder(filePath);
            if (!Directory.Exists(zipExtractionFolder))
                Directory.CreateDirectory(zipExtractionFolder);
            FileUtil.CleanFolder(zipExtractionFolder);

            ZipFile.ExtractToDirectory(filePath, zipExtractionFolder);
            return zipExtractionFolder;
        }

        public static string GetExtractionFolder(string filePath)
        {
            bool hasExtension = !Path.GetExtension(filePath).IsEmpty();
            return Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + (hasExtension ? "" : "~"));
        }

        public static void CreateZip(string zipPath, string[] zipFileList)
        {
            using (var zipFile = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                foreach (var file in zipFileList)
                    zipFile.CreateEntryFromFile(file, Path.GetFileName(file), CompressionLevel.Optimal);
            }
        }
    }
}
