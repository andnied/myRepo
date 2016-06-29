using System;
using System.Collections.Generic;
using System.IO;
using ComplaintTool.Common.Utils;

namespace ComplaintTool.MCProImageInterface.Incoming
{
    public class XmlFileInfo
    {
        private readonly string _filePath;
        private readonly string _fileType;    
        private readonly string _fileContent;
        private readonly string[] _tifFiles;

        public string FilePath { get { return _filePath; } }
        public string[] TifFiles { get { return _tifFiles; } }
        public string FileType { get { return _fileType; } }
        public string FileContent { get { return _fileContent; } }
        public string FileName { get { return Path.GetFileName(this.FilePath); } }

        public XmlFileInfo(string filePath)
        {
            if (!Extract.IsIncomingMCXmlFile(filePath))
                throw new InvalidOperationException(string.Format("Invalid xml file [{0}]", filePath));

            _filePath = filePath;
            _fileType = ImageProXmlUtil.GetFileType(filePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            using (var streamReader = new StreamReader(stream))
            {
                string fileContent = streamReader.ReadToEnd();
                _fileContent = fileContent.ToImageProEncoding();
            }

            _tifFiles = this.GetCorrespondingTifFiles();
        }

        private string[] GetCorrespondingTifFiles()
        {
            List<string> _tifFiles = new List<string>();
            string searchTifFileName = this.FilePath.Replace(".xml", ".TIF").Replace(".XML", ".TIF");
            if (File.Exists(searchTifFileName)) _tifFiles.Add(searchTifFileName);

            for (int counter = 0; counter < 1000; counter++)
            {
                string counterPart = counter.ToString().PadLeft(3, '0');
                searchTifFileName = this.FilePath
                    .Replace(".xml", string.Format("_{0}.TIF", counterPart))
                    .Replace(".XML", string.Format("_{0}.TIF", counterPart));
                if (File.Exists(searchTifFileName))
                    _tifFiles.Add(searchTifFileName);
            }
            return _tifFiles.ToArray();
        }
    }
}
