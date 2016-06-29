using System.IO;

namespace ComplaintTool.Common.Utils
{
    public class CRC32Util
    {
        public static string GenerateHashForTif(string tiffPath)
        {
            var crc32 = new CRC32();
            var hash = string.Empty;
            using (var fileStream = File.Open(tiffPath, FileMode.Open))
            {
                foreach (var crcByte in crc32.ComputeHash(fileStream))
                    hash = crcByte.ToString("x2").ToUpper() + hash;
                fileStream.Close();
            }

            hash = hash.TrimStart('0');
            return hash;
        }
    }
}
