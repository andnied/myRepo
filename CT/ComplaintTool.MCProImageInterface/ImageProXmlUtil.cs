using System.Xml;
using System.Xml.Linq;

namespace ComplaintTool.MCProImageInterface
{
    static class ImageProXmlUtil
    {
        public static string GetFileType(string filePath)
        {
            try
            {
                return XDocument.Load(filePath).Root.Name.ToString();
            }
            catch
            {
                return "";
            }
        }

        public static bool IsXml(string filePath)
        {
            bool isXml = false;
            try
            {
                var xml = XDocument.Load(filePath);
                isXml = true;
            }
            catch { }
            return isXml;
        }

        public static string ToImageProEncoding(this string xmlContent)
        {
            return xmlContent.Replace("utf-8", "UTF-16").Replace("UTF-8", "UTF-16");
        }
    }
}
