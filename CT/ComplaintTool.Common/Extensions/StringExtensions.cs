using System.IO;
namespace ComplaintTool.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) || value.Trim() == string.Empty;
        }

        public static bool FileDoesNotExist(this string value)
        {
            return !(File.Exists(value));
        }

        public static bool FileWrongFormat(this string value, string format)
        {
            return Path.HasExtension(value) ? Path.GetExtension(value) != format : string.IsNullOrEmpty(format) ? false : true;
        }
    }
}
