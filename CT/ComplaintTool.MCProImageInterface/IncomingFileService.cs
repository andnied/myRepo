using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ComplaintTool.Common;
using ComplaintTool.Common.Extensions;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.MCProImageInterface.AuditFiles;
using ComplaintTool.MCProImageInterface.Incoming;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.MCProImageInterface
{
    public class IncomingFileService : IComplaintProcess
    {
        public static readonly ILogger Logger = LogManager.GetLogger();

        #region Fields

        private readonly string _filePath;
        private readonly DateTime _started = DateTime.UtcNow;
        private readonly Guid _bulkProcessKey = Guid.NewGuid();
        private readonly Dictionary<string, bool> _processingStatus = new Dictionary<string, bool>();
        private Guid _bulkStreamId;

        #endregion

        #region IComplaintProcess

        public string OrganizationId
        {
            get
            {
                return ComplaintTool.Common.Enum.Organization.MC.ToString();
            }
        }

        public string ProcessName
        {
            get
            {
                return Globals.MCProImageInterfaceProcessName;
            }
        }

        public string ProcessFilePath
        {
            get
            {
                return Path.Combine(
                    Path.GetDirectoryName(FilePath),
                    Globals.TempFolderName, // Temp
                    Path.GetFileName(FilePath));
                    //string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(this.FilePath), _bulkProcessKey, Path.GetExtension(this.FilePath)));
            }
        }

        public string FilePath
        {
            get
            {
                return _filePath;
            }
        }

        #endregion

        #region Constructors
        public IncomingFileService(string filePath)
        {
            Guard.ThrowIf<ArgumentNullException>(filePath.IsEmpty(), "filePath");
            _filePath = filePath;
        }
        #endregion

        #region Processing

        public bool Process()
        {
            bool processed = false;
            // sprawdza czy plik jest poprawnym plikiem zip
            if (Extract.IsIncomingMCZipFile(FilePath))
            {
                // przenosi plik do tempa
                if (ProcessUtil.MoveFileForProcessing(this))
                {
                    // dodaje plik zip do bazy danych (jako FileStream oraz CaseFilingIncomingBulk)
                    int zipFileId = SaveBulk();

                    // rozpakowywuje plik zip
                    string extractedZipFolderPath = Extract.ExtractIncomingMCZipFile(ProcessFilePath);

                    // procesuje rozpakowane pliki audytowe
                    var extractedAuditFiles = Directory.GetFiles(extractedZipFolderPath, "*.txt")
                        .Union(Directory.GetFiles(extractedZipFolderPath, "*.TXT"));
                    extractedAuditFiles.ToList().ForEach(x => ProcessFile(x));

                    // procesuje rozpakowane pliki xml
                    var extractedXmlFiles = Directory.GetFiles(extractedZipFolderPath, "*.xml")
                        .Union(Directory.GetFiles(extractedZipFolderPath, "*.XML"));
                    extractedXmlFiles.ToList().ForEach(x => ProcessFile(x));

                    // TODO co jeśli nie wszystkie się powiodły?
                    processed = _processingStatus.All(x => x.Value);
                    if (processed)
                    {
                        // zatwierdza bulka w bazie danych
                        CommitBulk(zipFileId);
                        // ustawia notyfikacje
                        Logger.LogComplaintEvent(101, ProcessName, Path.GetFileName(ProcessFilePath));
                    }
                    // sprzata
                    ProcessUtil.Clean(this);
                }
            }
            return processed;
        }

        #endregion

        #region ProcessFile

        private bool ProcessFile(string filePath)
        {
            bool processed = false;
            try
            {
                // tworzy nowa transakcje
                using (var unitOfWork = ComplaintUnitOfWork.Create())
                {
                    FileProcessorBase processor = null;
                    if (Extract.IsIncomingMCAuditFile(filePath))
                    {
                        _processingStatus.Add(filePath, processed);
                        processor = AuditFileProcessor.Create(unitOfWork, filePath);
                    }
                    else if (Extract.IsIncomingMCXmlFile(filePath))
                    {
                        _processingStatus.Add(filePath, processed);
                        processor = XmlFileProcessor.Create(unitOfWork, filePath);
                    }
                    else
                        throw ComplaintCaseFilingIncomingException.UnsupportedIncomingFile(filePath);

                    ProcessUtil.MoveFileForProcessing(processor);
                    processed = processor.Process(_bulkProcessKey);
                    _processingStatus[filePath] = processed;

                    if (processed)
                    {
                        processor.Clean();
                        unitOfWork.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                processed = false;
                Logger.LogComplaintException(ex);
            }
            return processed;
        } 

        #endregion

        #region Helpers

        private int SaveBulk()
        {
            // tworzy nowa transakcje
            using (var unitOfWork = ComplaintUnitOfWork.Create())
            {
                _bulkStreamId = unitOfWork.Repo<FileRepo>().AddIncomingFile(ProcessFilePath);
                var file = unitOfWork.Repo<FileRepo>().AddCaseFilingIncomingBulkFile(
                    OrganizationId,
                    ProcessFilePath,
                    _bulkStreamId,
                    _bulkProcessKey);
                unitOfWork.Commit();
                return file.FileId;
            }
        } 

        private void CommitBulk(int zipFileId)
        {
            // tworzy nowa transakcje
            using (var unitOfWork = ComplaintUnitOfWork.Create())
            {
                unitOfWork.Repo<FileRepo>().UpdateStatusInCaseFilingIncomingBulkFile(zipFileId);
                unitOfWork.Commit();
            }
        }

        #endregion
    }
}
