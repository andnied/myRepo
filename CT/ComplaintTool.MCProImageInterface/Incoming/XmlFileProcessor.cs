using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Transactions;
using ComplaintTool.Common;
using ComplaintTool.Common.Extensions;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.MCProImageInterface.Model;

namespace ComplaintTool.MCProImageInterface.Incoming
{
    public abstract class XmlFileProcessor : FileProcessorBase
    {
        protected readonly XmlFileInfo _xmlFile;
        protected Guid _bulkProcessKey;
        protected Guid _incomingFileStreamId;

        public override string FilePath
        {
            get { return _xmlFile.FilePath; }
        }

        public override string ProcessFilePath
        {
            get { return _xmlFile.FilePath; }
        }

        public override string FileDescription
        {
            // typ pliku???
            get { return _xmlFile.FileType; }
        }

        public XmlFileProcessor(ComplaintUnitOfWork unitOfWork, XmlFileInfo xmlFile)
            : base(unitOfWork)
        {
            Guard.ThrowIf<ArgumentNullException>(xmlFile == null, "xmlFile");
            _xmlFile = xmlFile;
        }

        protected virtual void SaveFile()
        {
            // tworzy nowa transakcje
            using (var unitOfWork = ComplaintUnitOfWork.Create())
            {
                _incomingFileStreamId = unitOfWork.Repo<FileRepo>().AddIncomingFile(this.ProcessFilePath);
                unitOfWork.Commit();
            }
        }

        public override void Clean()
        {
            DeleteFile(ProcessFilePath);
            foreach (var tiff in _xmlFile.TifFiles)
                DeleteFile(tiff);
        }

        private void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        #region Factory Method

        public static XmlFileProcessor Create(ComplaintUnitOfWork unitOfWork, string filePath)
        {
            try
            {
                var xmlFile = new XmlFileInfo(filePath);
                Type processorType = FindProcessorType(xmlFile.FileType);
                return (XmlFileProcessor)Activator.CreateInstance(processorType, new object[] { unitOfWork, xmlFile });
            }
            catch (Exception ex)
            {
                throw ComplaintCaseFilingIncomingException.UnsupportedIncomingFile(filePath, ex);
            }
        }

        private static Type FindProcessorType(string fileType)
        {
            var nsList = typeof(XmlFileProcessor).Assembly.GetTypes()
                .Select(t => t.Namespace)
                .Distinct();
            foreach (var ns in nsList)
            {
                var type = Type.GetType(string.Format("{0}.{1}XmlFileProcessor", ns, fileType.ToUpper()));
                if (type != null)
                    return type;
            }
            return null;
        }

        #endregion
    }

    public abstract class XmlFileProcessor<T> : XmlFileProcessor
    {
        public XmlFileProcessor(ComplaintUnitOfWork unitOfWork, XmlFileInfo xmlFile) 
            : base(unitOfWork, xmlFile)
        {
        }

        public override bool Process(Guid bulkProcessKey)
        {
            Guard.ThrowIf<ArgumentNullException>(bulkProcessKey.IsEmpty(), "bulkProcessKey");
            _bulkProcessKey = bulkProcessKey;

            // dodaje stream do bazy danych
            this.SaveFile();

            // deserializuje plik xml
            var data = XmlUtil.Deserialize<T>(_xmlFile.FileContent);
            return this.ProcessData(data);
        }

        protected abstract bool ProcessData(T data);
    }
}
