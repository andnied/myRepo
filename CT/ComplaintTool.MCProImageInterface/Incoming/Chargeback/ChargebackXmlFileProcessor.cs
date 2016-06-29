using System;
using System.Globalization;
using System.IO;
using System.Linq;
using ComplaintTool.Common;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;

namespace ComplaintTool.MCProImageInterface.Incoming.Chargeback
{
    abstract class ChargebackXmlFileProcessor<T> : XmlFileProcessor<T>
    {
        protected CaseFilingIncomingFile _incomingFile;
        protected Complaint _complaint;
        protected ComplaintStage _complaintStage;

        public ChargebackXmlFileProcessor(ComplaintUnitOfWork unitOfWork, XmlFileInfo xmlFile) 
            : base(unitOfWork, xmlFile)
        {
        }

        public virtual string TifFilePath
        {
            get { return _xmlFile.TifFiles.FirstOrDefault(); }
        }

        //public string Arn { get; protected set; }
        //public string CaseId { get; protected set; }
        //public long? StageId { get; protected set; }
        public string LengthFromXml { get; protected set; }
        public string HashFromXml { get; protected set; }
        public string TifFileNameFromXml { get; protected set; }

        protected override bool ProcessData(T data)
        {
            // sprawdza czy istnieje powiązany plik TIF
            if (!File.Exists(this.TifFilePath))
                return false;

            // sprawdza czy id pliku istnieje w bazie danych
            if (!_unitOfWork.Repo<NameListRepo>().CheckFileId(this.ProcessFilePath))
                return false;

            bool allowProcessing = false;
            // sprawdza czy plik xml istnieje w bazie danych
            if (_unitOfWork.Repo<FileRepo>().ExistsCaseFilingIncomingFile(this.ProcessFilePath))
            {
                // dopasowywuje dane z xml do sprawy
                if (this.MatchChargeback(data))
                {
                    this.CreateCaseFilingIncomingFile();
                    // sprawdza metadane TIF i XML
                    allowProcessing = this.CheckMetadata();
                }
            }

            if (allowProcessing)
            {
                return this.ProcessChargeback(data);
            }
            else
            {
                Logger.LogComplaintEvent(500, this.ProcessName, Path.GetFileName(this.ProcessFilePath));
                return false;
            }
        }

        protected abstract bool MatchChargeback(T data);
        protected abstract bool ProcessChargeback(T data);

        protected virtual bool CheckMetadata()
        {
            var fileInfo = new FileInfo(this.TifFilePath);
            var lengthX8 = fileInfo.Length.ToString("X8");
            var lengthX = fileInfo.Length.ToString("X");
            var length = fileInfo.Length.ToString(CultureInfo.InvariantCulture);

            var crc32 = new CRC32();
            var hash = string.Empty;
            using (var fileStream = File.Open(this.TifFilePath, FileMode.Open))
            {
                foreach (var crcByte in crc32.ComputeHash(fileStream))
                    hash = crcByte.ToString("x2").ToUpper() + hash;
                fileStream.Close();
            }

            try
            {
                var isLengthCorrect = this.LengthFromXml.Equals(lengthX) || this.LengthFromXml.Equals(lengthX8) || this.LengthFromXml.Equals(length);
                var isHashCorrect = this.HashFromXml.Equals(hash);
                var isNameCorrect = this.TifFileNameFromXml.ToUpper().Equals(fileInfo.Name.ToUpper());

                return isLengthCorrect && isHashCorrect && isNameCorrect;
            }
            catch
            {
                return false;
            }
        }

        protected virtual void CreateCaseFilingIncomingFile()
        {
            if (_complaint == null)
                throw new InvalidOperationException("Complaint not found!");
            if (_complaintStage == null)
                throw new InvalidOperationException("ComplaintStage not found!");

            _incomingFile = _unitOfWork.Repo<FileRepo>().CreateCaseFilingIncomingFile(
                        _complaint.CaseId,
                        _complaintStage.StageId,
                        _xmlFile.FileType,
                        _xmlFile.FileName,
                        _xmlFile.FileContent,
                        _incomingFileStreamId,
                        _bulkProcessKey);
        }
    }
}
