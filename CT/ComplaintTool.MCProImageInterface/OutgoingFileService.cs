using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ComplaintTool.Common;
using ComplaintTool.Common.Extensions;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.MCProImageInterface.Outgoing;
using ComplaintTool.MCProImageInterface.Outgoing.Case;
using ComplaintTool.MCProImageInterface.Outgoing.Chargeback;
using ComplaintTool.MCProImageInterface.Outgoing.RR;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.MCProImageInterface
{
    public class OutgoingFileService : IComplaintProcess
    {
        public static readonly ILogger Logger = LogManager.GetLogger();

        #region Fields

        private readonly string _tempFolderPath;
        private readonly DateTime _started = DateTime.UtcNow;
        private readonly Guid _bulkProcessKey = Guid.NewGuid();
        private string _filePath;
        private List<string> _extractedCases = new List<string>();
        private List<string> _extractedFileNames = new List<string>(); 

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
                return _filePath;
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

        public OutgoingFileService(string tempFolderPath)
        {
            Guard.ThrowIf<ArgumentNullException>(tempFolderPath.IsEmpty(), "tempFolderPath");
            _tempFolderPath = tempFolderPath;
            // TODO sciezka tymczasowa
            _filePath = Path.Combine(tempFolderPath, _bulkProcessKey.ToString());
        } 

        #endregion

        #region CaseFiling

        public bool ExtractCaseFilingWeb(string caseId, string exportCode)
        {
            try
            {
                return ExtractCaseFiling(caseId, exportCode);
            }
            catch (Exception ex)
            {
                Logger.LogComplaintException(ex);
                return false;
            }
        }

        public bool ExtractCaseFiling(string caseId, string exportCode)
        {
            Guard.ThrowIf<ArgumentNullException>(caseId.IsEmpty(), "caseId");
            if (!OutgoingFileType.CheckExportCode(exportCode))
                throw ComplaintCaseFilingExtractException.InvalidExportCode(exportCode);

            bool status = false;
            using (var unitOfWork = ComplaintUnitOfWork.Get())
            {
                var lastStage = unitOfWork.Repo<ComplaintRepo>().FindLastStageForCaseFiling(caseId);
                if (lastStage == null)
                    throw ComplaintCaseFilingExtractException.StageNotFound(caseId);

                var record = unitOfWork.Repo<CaseFilingRepo>().FindRecordByCaseAndStageId(caseId, lastStage.StageId);
                if (record == null)
                    throw ComplaintCaseFilingExtractException.CaseFilingRecordNotFound(caseId, lastStage.StageId);

                var generator = CaseFilingRecordToXmlFileGenerator.Create(exportCode, record, lastStage, _tempFolderPath);
                string xmlContent = generator.Generate();
                if (xmlContent.IsEmpty())
                    throw ComplaintCaseFilingExtractException.XmlNotGenerated(exportCode, caseId);

                string tiffFile = null;
                var documents = unitOfWork.Repo<DocumentRepo>().FindDocumentsToExportForCaseFiling(caseId, exportCode, lastStage.StageId);
                if (documents.Any())
                {
                    var tiffGenerator = new TiffGenerator(unitOfWork, _tempFolderPath);
                    tiffFile = tiffGenerator.GenerateTiffFileForDocuments(documents, Path.GetFileNameWithoutExtension(generator.ProcessFilePath));
                }

                status = unitOfWork.Repo<FileRepo>().ProcessOutgoingFileForCaseFiling(
                    _bulkProcessKey,
                    generator.ProcessFilePath,
                    tiffFile,
                    generator.FileType,
                    xmlContent,
                    record,
                    documents);

                unitOfWork.Commit();
                Logger.LogComplaintEvent(155, exportCode, caseId, Path.GetFileNameWithoutExtension(generator.ProcessFilePath));
            }
            return status;
        } 

        #endregion

        #region Chargeback

        public void ExtractRepresentmentDocuments()
        {
            _extractedCases = new List<string>();
            _extractedFileNames = new List<string>();

            using (var unitOfWork = ComplaintUnitOfWork.Create())
            {
                var representments = unitOfWork.Repo<RepresentmentRepo>().FindRepresentmentsForExtract(this.OrganizationId);
                foreach (var representment in representments.Where(x => x.DocumentationIndicator == false))
                {
                    unitOfWork.Repo<RepresentmentRepo>().UpdateToExtracted(representment);
                }
                foreach (var representment in representments.Where(x => x.DocumentationIndicator == true))
                {
                    ProcessExtractRepresentment(representment);
                }

                if (_extractedCases.Any())
                    Logger.LogComplaintEvent(132, "representment", string.Join(",", _extractedCases), string.Join(",", _extractedFileNames));

                unitOfWork.Commit();
            }
        } 

        private void ProcessExtractRepresentment(Representment representment)
        {
            string tiffPath = null;
            string xmlPath = null;
            try
            {
                using (var unitOfWork = ComplaintUnitOfWork.Create())
                {
                    var tiffGenerator = new TiffGenerator(unitOfWork, _tempFolderPath);
                    tiffPath = tiffGenerator.GenerateTiffFileForRepresentment(representment);
                    if (!File.Exists(tiffPath))
                        throw ComplaintCaseFilingExtractException.TiffNotGenerated(tiffPath, representment.RepresentmentId);

                    // obslugujemy tylko plik SCU dla reprezentacji
                    var generator = new RepresentmentToSCUGenerator(unitOfWork, _tempFolderPath, tiffPath, representment);
                    xmlPath = generator.ProcessFilePath;
                    long counter = generator.Counter;
                    string xmlContent = generator.Generate();
                    if (xmlContent.IsEmpty())
                        throw ComplaintCaseFilingExtractException.XmlNotGenerated(OutgoingFileType.SCU, representment.CaseId);

                    unitOfWork.Repo<FileRepo>().ProcessOutgoingFileForRepresentment(
                        xmlPath,
                        tiffPath,
                        generator.FileType,
                        xmlContent,
                        counter,
                        representment
                        );

                    _extractedCases.Add(representment.CaseId);
                    _extractedFileNames.Add(Path.GetFileName(xmlPath));
                    _extractedFileNames.Add(Path.GetFileName(tiffPath));

                    unitOfWork.Commit();
                }
            }
            catch (Exception ex)
            {
                Logger.LogComplaintException(ex);
            }
            finally
            {
                FileUtil.CleanTempFiles(_tempFolderPath, 
                    representment.CaseId,
                    Path.GetFileNameWithoutExtension(xmlPath),
                    tiffPath);

                if (File.Exists(tiffPath))
                    File.Delete(tiffPath);

                if (File.Exists(xmlPath))
                    File.Delete(xmlPath);
            }
        }

        #endregion

        #region RRF

        public void ExtractRrfDocuments()
        {
            _extractedCases = new List<string>();
            _extractedFileNames = new List<string>();

            using (var unitOfWork = ComplaintUnitOfWork.Create())
            {
                var complaints = unitOfWork.Repo<ComplaintRepo>().FindComplaintsWithDocumentExport(this.OrganizationId);
                foreach (var complaint in complaints.ToList())
                    this.ExtractRrfDocumentForComplaint(unitOfWork, complaint);

                if (_extractedCases.Any())
                    Logger.LogComplaintEvent(132, "RRF", string.Join(",", _extractedCases), string.Join(",", _extractedFileNames));

                unitOfWork.Commit();
            }
        }

        public bool ExtractRrfDocument(string caseId)
        {
            using (var unitOfWork = ComplaintUnitOfWork.Create())
            {
                var complaint = unitOfWork.Repo<ComplaintRepo>().FindByCaseId(caseId);
                bool status = this.ExtractRrfDocumentForComplaint(unitOfWork, complaint);
                unitOfWork.Commit();
                return status;
            }
        }

        private bool ExtractRrfDocumentForComplaint(ComplaintUnitOfWork unitOfWork, Complaint complaint)
        {
            var tiffGenerator = new TiffGenerator(unitOfWork, _tempFolderPath);
            var tiffPath = tiffGenerator.GenerateTiffFileForComplaint(complaint);
            if (File.Exists(tiffPath))
            {
                var generator = new RrfToACUGenerator(unitOfWork, _tempFolderPath, tiffPath, complaint);
                if (generator != null)
                {
                    string xmlPath = generator.ProcessFilePath;
                    string fileType = generator.FileType;
                    long counter = generator.Counter;
                    string xmlContent = generator.Generate();

                    if (!xmlContent.IsEmpty())
                    {
                        // TODO weryfikacji sciezki
                        FileUtil.CleanTempFiles(_tempFolderPath,
                            complaint.CaseId,
                            Path.GetFileNameWithoutExtension(generator.ProcessFilePath),
                            tiffPath);

                        unitOfWork.Repo<FileRepo>().ProcessOutgoingFileForRrf(
                            xmlPath,
                            tiffPath,
                            fileType,
                            xmlContent,
                            counter,
                            complaint
                            );

                        if (File.Exists(tiffPath))
                            File.Delete(tiffPath);

                        if (File.Exists(xmlPath))
                            File.Delete(xmlPath);

                        _extractedCases.Add(complaint.CaseId);
                        _extractedFileNames.Add(Path.GetFileName(xmlPath));
                        _extractedFileNames.Add(Path.GetFileName(tiffPath));
                        return true;
                    }
                }
            }
            return false;
        } 

        #endregion
    }
}
