using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using ComplaintTool.Models;
using System.Collections.Generic;
using System.Globalization;
using ComplaintTool.Common.Enum;
using ComplaintTool.Common.Extensions;

namespace ComplaintTool.DataAccess.Repos
{
    public class FileRepo : RepositoryBase
    {
        public const string TempDocsDirectoryName = "Acknowledgments";

        public FileRepo(DbContext context) 
            : base(context)
        {
        }
        
        #region Insert Files To FileStream

        //public Guid InsertZIPfileToIncomingFiles(string filePath, Guid bulkStreamId)
        //{
        //    var fileStream = File.ReadAllBytes(filePath);
        //    var fileName = string.Format("{0}_{1}", DateTime.UtcNow.ToString("yyyyMMddhhmmssfff"), Path.GetFileName(filePath));
        //    // procedura składowana
        //    var result = this.GetContext<ComplaintEntities>().AddIncomingFile(bulkStreamId, fileStream, fileName)
        //        .FirstOrDefault();
        //    return result.GetValueOrDefault();
        //}

        public Guid AddIncomingFile(string filePath)
        {
            var fileStreamId = Guid.NewGuid();
            var fileStream = File.ReadAllBytes(filePath);
            var fileName = string.Format("{0}_{1}", DateTime.UtcNow.ToString("yyyyMMddhhmmssfff"), Path.GetFileName(filePath));
            // procedura składowana
            AddIncomingFile(fileStreamId, fileStream, fileName);
            return fileStreamId;
        }

        public Guid AddDocumentFile(string filePath)
        {                   
            var fileStream = File.ReadAllBytes(filePath);
            var documentName = Path.GetFileName(filePath);
            return AddDocumentFile(documentName, fileStream);
        }

        public Guid AddDocumentFile(string documentName,byte[] bytes)
        {
            var fileStreamId = Guid.NewGuid();
            var fileName = string.Format("{0}_{1}", DateTime.UtcNow.ToString("yyyyMMddhhmmssfff"), documentName);
            AddDocument(fileStreamId, bytes, fileName);
            return fileStreamId;
        }

        public Guid AddOutgoingFile(string filePath)
        {
            var fileStreamId = Guid.NewGuid();
            var fileStream = File.ReadAllBytes(filePath);
            var fileName = string.Format("{0}_{1}", DateTime.UtcNow.ToString("yyyyMMddhhmmssfff"), Path.GetFileName(filePath));
            // procedura składowana
            base.AddOutgoingFile(fileStreamId, fileStream, fileName);
            return fileStreamId;
        }

        public Guid AddOutgoingFile(byte[] stream, string fileName)
        {
            var fileStreamId = Guid.NewGuid();
            fileName = string.Format("{0}_{1}", DateTime.UtcNow.ToString("yyyyMMddhhmmssfff"), fileName);
            
            base.AddOutgoingFile(fileStreamId, stream, fileName);
            return fileStreamId;
        }

        public IEnumerable<string> GetIncomingRegOrgFile(int fileId)
        {
            var stream = (byte[])GetOrgIncomingFile(fileId);
            IEnumerable<string> lines;

            using (var memoryStram = new MemoryStream())
            {
                memoryStram.Write(stream, 0, stream.Length);
                memoryStram.Position = 0;

                using (var streamReader = new StreamReader(memoryStram))
                {
                    lines = streamReader.ReadToEnd().Split('\n').ToList();
                }
            }

            return lines;
        }

        public void GetIncomingRegOrgStream(int fileId, string tempFilePath)
        {
            var fileStream = new ObjectParameter("file_Stream", typeof(Byte[]));
            GetContext<ComplaintEntities>().GetOrgIncomingFile(fileId, fileStream);

            var tempFolder = Path.GetDirectoryName(tempFilePath);
            if (fileStream.Value == DBNull.Value) return;
            if (tempFolder != null && !Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);

            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);

            File.WriteAllBytes(tempFilePath, (byte[])fileStream.Value);
        }

        public string AddDocumentToComplaint(string fileName, string caseId, Guid streamId, FileSource fileSource)
        {
            var count = GetContext<ComplaintEntities>().ComplaintDocuments.Count(x => x.CaseId == caseId);
            var regFileName = String.Format(@"{0}_{1}_{2}_{3}{4}"
                                , caseId
                                , (++count).ToString(CultureInfo.InvariantCulture).PadLeft(3, '0')
                                , fileSource.ToString().PadRight(4, '_')
                                , DateTime.UtcNow.ToString("yyyyMMddHHmm")
                                , Path.GetExtension(fileName));

            AddDocumentToComplaint(caseId, null, streamId, regFileName, fileName,
                                            fileSource.ToString(), null, true, false, null, null);

            return regFileName;
        }

        public List<string> GetReponsePostilionFile(long fileId)
        {
            var fileStream = new ObjectParameter("file_Stream", typeof (byte[]));
            GetContext<ComplaintEntities>().GetResponsePostilionFile(fileId, fileStream);

            var messageList = new List<string>();

            if (fileStream.Value == DBNull.Value) return messageList;

            using (var memoryStram = new MemoryStream())
            {
                memoryStram.Write((byte[])fileStream.Value, 0, ((byte[])fileStream.Value).Length);
                memoryStram.Position = 0;

                using (var streamReader = new StreamReader(memoryStram))
                {
                    string line;

                    while ((line = streamReader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        messageList.Add(line);
                    }
                }
            }

            return messageList;
        }

        public object GetCLFReportStream(int id)
        {
            var fileStream = new ObjectParameter("file_Stream", typeof(Byte[]));
            GetContext<ComplaintEntities>().GetCLFReportFile(id, fileStream);

            return fileStream.Value;
        }

        #endregion

        #region CaseFilingIncomingFiles

        public CaseFilingIncomingBulkFile AddCaseFilingIncomingBulkFile(string orgId,
            string filePath,
            Guid bulkStreamId,
            Guid bulkProcessKey)
        {

            var currDateTime = GetCurrentDateTime();
            var newBulkFile = new CaseFilingIncomingBulkFile
            {
                OrganizationId = orgId,
                Name = Path.GetFileName(filePath),
                ErrorFlag = true,
                Status = 2,
                stream_id = bulkStreamId,
                ProcessKey = bulkProcessKey,
                ProcesingStart = currDateTime,
                InsertDate = currDateTime,
                InsertUser = GetCurrentUser()
            };
            GetDbSet<CaseFilingIncomingBulkFile>().Add(newBulkFile);
            Commit();
            return newBulkFile;
        }

        public void UpdateStatusInCaseFilingIncomingBulkFile(int zipFileId)
        {
            var bulkFile = GetDbSet<CaseFilingIncomingBulkFile>().FirstOrDefault(x => x.FileId == zipFileId);
            if (bulkFile != null)
            {
                bulkFile.ProcesingFinished = GetCurrentDateTime();
                bulkFile.ErrorFlag = false;
                bulkFile.Status = 3;
                Commit();
            }
        }

        public CaseFilingIncomingFile CreateCaseFilingIncomingFile(string caseId,
            long stageId,
            string fileType,
            string fileName,
            string fileContent,
            Guid incomingFileStreamId,
            Guid bulkProcessKey)
        {
            var incomingFile = new CaseFilingIncomingFile
            {
                CaseId = caseId,
                StageId = stageId,
                FileType = fileType.ToUpper(),
                FileName = fileName,
                FileContent = fileContent,
                ProcessKey = bulkProcessKey,
                stream_id = incomingFileStreamId,
                InsertDate = GetCurrentDateTime(),
                InsertUser = GetCurrentUser()
            };

            incomingFile.IncomingId = AddCaseFilingIncomingFile(incomingFile);
            //Commit();
            return incomingFile;
        }

        //public void AddCaseFilingIncomingFile(Guid incomingFileStreamId, CaseFilingIncomingFile incomingFile)
        //{
        //    incomingFile.stream_id = incomingFileStreamId;
        //    GetDbSet<CaseFilingIncomingFile>().Add(incomingFile);
        //    Commit();
        //}

        public bool ExistsCaseFilingIncomingFile(string filePath)
        {
            var xmlFileName = Path.GetFileName(filePath);
            return GetDbSet<CaseFilingIncomingFile>().Any(x => x.FileName.Equals(xmlFileName));
        }

        #endregion

        #region CaseFilingOutgoingFiles

        public CaseFilingOutgoingFile CreateCaseFilingOutgoingFile(string caseId,
            long lastStageId,
            string fileType,
            string filePath,
            string fileContent,
            Guid fileStreamId,
            Guid bulkProcessKey)
        {
            var file = new CaseFilingOutgoingFile
            {
                CaseId = caseId,
                StageId = lastStageId,
                FileType = fileType.ToUpper(),
                FileName = Path.GetFileName(filePath),
                FileContent = fileContent,
                ProcessKey = bulkProcessKey,
                stream_id = fileStreamId,
                InsertDate = GetCurrentDateTime(),
                InsertUser = GetCurrentUser()
            };

            file.OutgoingId = AddCaseFilingOutgoingFile(file);
            //Commit();
            return file;
        }

        //public void AddCaseFilingOutgoingFile(Guid fileStreamId, CaseFilingOutgoingFile file)
        //{
        //    file.stream_id = fileStreamId;
        //    GetDbSet<CaseFilingOutgoingFile>().Add(file);
        //    Commit();
        //}

        public CaseFilingOutgoingFile FindOutgoingFileByName(string fileName)
        {
            return GetDbSet<CaseFilingOutgoingFile>()
                .FirstOrDefault(x => x.FileName.ToUpper().Equals(fileName.ToUpper()));
        }

        public CaseFilingOutgoingFile FindByComplaint(Complaint complaint)
        {
            var caseId = complaint.CaseId;
            return GetDbSet<CaseFilingOutgoingFile>().Where(x => x.CaseId == caseId)
                .OrderByDescending(x => x.OutgoingId)
                .FirstOrDefault();
        }

        public bool ExistsCaseFilingOutgointFile(string caseId, string fileType)
        {
            return GetDbSet<CaseFilingOutgoingFile>()
                .Any(x => x.CaseId == caseId && x.FileType == fileType);
        }

        #endregion

        #region FileStream

        public byte[] GetOutgoingFile(Guid streamId)
        {
            var fileStream = new ObjectParameter("file_Stream", typeof(byte[]));
            GetContext<ComplaintEntities>().GetOutgoingFile(streamId, fileStream);
            return fileStream.Value != DBNull.Value ? (byte[])fileStream.Value : null;
        } 

        #endregion

        #region ProcessOutgoingFile
        
        public void ProcessOutgoingFileForRepresentment(string xmlPath, string tiffPath, string fileType, string fileContent, long counter, Representment representment)
        {
            var fileName = Path.GetFileNameWithoutExtension(xmlPath);
            var xmlStreamId = Guid.NewGuid();
            var tifStreamId = Guid.NewGuid();
            var currDateTime = GetCurrentDateTime();
            var complaint = GetDbSet<Complaint>().Find(representment.CaseId);
            var entities = GetContext<ComplaintEntities>();
            var xmlFileStream = File.ReadAllBytes(xmlPath);
            var tifFileStream = File.ReadAllBytes(tiffPath);
            var xmlOutgoingName = string.Format("{0:yyyyMMddhhmmssfff}_{1}", currDateTime, Path.GetFileName(xmlPath));
            var tifOutgoingName = string.Format("{0:yyyyMMddhhmmssfff}_{1}", currDateTime, Path.GetFileName(tiffPath));

            //Add XML file to outgoing files
            AddOutgoingFile(xmlStreamId, xmlFileStream, xmlOutgoingName);

            //Add TIF file to outgoing files
            AddOutgoingFile(tifStreamId, tifFileStream, tifOutgoingName);

            #region Add XML to OutgoingPackage
            string bin = null;
            string organizationId = null;
            if (complaint != null)
            {
                bin = complaint.ARN.Substring(1, 6);
                organizationId = complaint.OrganizationId;
            }

            var newXmlOutPackageItem = new OutgoingPackageItem
            {
                OrganizationId = organizationId,
                FileName = Path.GetFileName(xmlPath),
                stream_id = xmlStreamId,
                Status = 0,
                BIN = bin,
                InsertDate = currDateTime,
                InsertUser = GetCurrentUser()
            };

            entities.OutgoingPackageItems.Add(newXmlOutPackageItem); 
            #endregion

            #region Add TIF to OutgoingPackage

            var newTifOutPackageItem = new OutgoingPackageItem
            {
                OrganizationId = organizationId,
                FileName = Path.GetFileName(tiffPath),
                stream_id = tifStreamId,
                Status = 0,
                BIN = bin,
                InsertDate = DateTime.UtcNow,
                InsertUser = GetCurrentUser()
            };

            entities.OutgoingPackageItems.Add(newTifOutPackageItem); 
            #endregion

            #region Add XML to CaseFilingOutgoingFile

            var newCaseFilingOut = CreateCaseFilingOutgoingFile(
                representment.CaseId,
                representment.StageId,
                fileType,
                xmlPath,
                fileContent,
                xmlStreamId,
                Guid.Empty
                );
            var caseFilingOutId = newCaseFilingOut.OutgoingId;
            //var newCaseFilingOut = new CaseFilingOutgoingFile
            //{
            //    CaseId = representment.CaseId,
            //    StageId = representment.StageId,
            //    stream_id = xmlStreamId,
            //    FileType = fileType,
            //    FileName = Path.GetFileName(xmlPath),
            //    FileContent = fileContent,
            //    //ProcessKey = processKey,
            //    InsertDate = currDateTime,
            //    InsertUser = GetCurrentUser()
            //};
            ////entities.CaseFilingOutgoingFiles.Add(newCaseFilingOut);
            //var caseFilingOutId = AddCaseFilingOutgoingFile(newCaseFilingOut);

            #endregion

            #region Add new name counter

            var newNameList = new NameList
            {
                FileName = fileName,
                OrganizationId = organizationId,
                Counter = counter,
                InsertUser = GetCurrentUser(),
                InsertDate = currDateTime
            };
            entities.NameLists.Add(newNameList); 
            #endregion

            #region Add new document extracts, update representment status and add new CaseFilingDocumentItem

            foreach (var repDoc in representment.RepresentmentDocuments.ToList())
            {
                var newExtractDoc = new RepresentmentExtractDocument
                {
                    CaseId = representment.CaseId,
                    DocumentId = repDoc.DocumentId,
                    ErrorFlag = false,
                    ExportDate = DateTime.UtcNow,
                    ExportStatus = 2,
                    ExportUser = repDoc.InsertUser,
                    Extract_stream_id = null,
                    RepresentmentDocumentsId = repDoc.RepresentmentDocumentsId,
                    Status = 2,
                    TIF_stream_id = tifStreamId,
                    XML_stream_id = xmlStreamId
                };
                entities.RepresentmentExtractDocuments.Add(newExtractDoc);

                var newDocItem = new CaseFilingOutgoingFileDocumentItem
                {
                    OutgoingId = caseFilingOutId,
                    DocumentId = repDoc.DocumentId,
                    InsertDate = currDateTime,
                    InsertUser = GetCurrentUser()
                };
                entities.CaseFilingOutgoingFileDocumentItems.Add(newDocItem);
            }

            #endregion

            representment.Status = 3;
            Commit();
        }

        public void ProcessOutgoingFileForRrf(string xmlPath, string tiffPath, string fileType, string fileContent, long counter, Complaint complaint)
        {
            var currDateTime = GetCurrentDateTime();
            var xmlStreamId = Guid.NewGuid();
            var tifStreamId = Guid.NewGuid();
            var xmlFileStream = File.ReadAllBytes(xmlPath);
            var tifFileStream = File.ReadAllBytes(tiffPath);
            var xmlOutgoingName = string.Format("{0:yyyyMMddhhmmssfff}_{1}", currDateTime, Path.GetFileName(xmlPath));
            var tifOutgoingName = string.Format("{0:yyyyMMddhhmmssfff}_{1}", currDateTime, Path.GetFileName(tiffPath));
            var entities = GetContext<ComplaintEntities>();

            //Add XML file to outgoing files
            AddOutgoingFile(xmlStreamId, xmlFileStream, xmlOutgoingName);

            //Add TIF file to outgoing files 
            AddOutgoingFile(tifStreamId, tifFileStream, tifOutgoingName);

            #region Add XML to OutgoingPackage
            
            string bin = null;
            string organizationId = null;
            
            if (complaint != null)
            {
                bin = complaint.ARN.Substring(1, 6);
                organizationId = complaint.OrganizationId;
            }

            var newXmlOutPackageItem = new OutgoingPackageItem
            {
                OrganizationId = organizationId,
                FileName = Path.GetFileName(xmlPath),
                stream_id = xmlStreamId,
                Status = 0,
                BIN = bin,
                InsertDate = DateTime.UtcNow,
                InsertUser = GetCurrentUser()
            };

            entities.OutgoingPackageItems.Add(newXmlOutPackageItem);

            #endregion

            #region Add TIF to OutgoingPackage

            var newTifOutPackageItem = new OutgoingPackageItem
            {
                OrganizationId = organizationId,
                FileName = Path.GetFileName(tiffPath),
                stream_id = tifStreamId,
                Status = 0,
                BIN = bin,
                InsertDate = DateTime.UtcNow,
                InsertUser = GetCurrentUser()
            };

            entities.OutgoingPackageItems.Add(newTifOutPackageItem);

            #endregion

            long caseFilingOutId = 0;

            #region Add XML to CaseFilingOutgoingFile

            var lastRrfStage =
                    entities.ComplaintStages.Where(x => x.CaseId == complaint.CaseId && x.StageCode == "RRF")
                        .OrderByDescending(x => x.StageId).FirstOrDefault();

            if (lastRrfStage != null && complaint != null)
            {
                var newCaseFilingOut = CreateCaseFilingOutgoingFile(
                complaint.CaseId,
                lastRrfStage.StageId,
                fileType,
                xmlPath,
                fileContent,
                xmlStreamId,
                Guid.Empty
                );
                caseFilingOutId = newCaseFilingOut.OutgoingId;

                //var newCaseFilingOut = new CaseFilingOutgoingFile
                //{
                //    CaseId = complaint.CaseId,
                //    StageId = lastRrfStage.StageId,
                //    stream_id = xmlStreamId,
                //    FileType = fileType,
                //    FileName = Path.GetFileName(xmlPath),
                //    FileContent = fileContent,
                //    //ProcessKey = processKey,
                //    InsertDate = currDateTime,
                //    InsertUser = GetCurrentUser()
                //};
                ////entities.CaseFilingOutgoingFiles.Add(newCaseFilingOut);
                //caseFilingOutId = AddCaseFilingOutgoingFile(newCaseFilingOut);
            }

            #endregion

            #region Add new name counter

            var newNameList = new NameList
            {
                FileName = Path.GetFileNameWithoutExtension(xmlPath),
                OrganizationId = organizationId,
                Counter = counter,
                InsertUser = GetCurrentUser(),
                InsertDate = currDateTime
            };
            entities.NameLists.Add(newNameList);

            #endregion

            #region Add new document extracts, update document status and add new CaseFilingDocumentItem

            if (complaint != null)
            {
                foreach (var doc in complaint.DocumentExports.ToList())
                {
                    var newExtractDoc = new DocumentExtractExport
                    {
                        CaseId = complaint.CaseId,
                        ErrorFlag = false,
                        ExportDate = DateTimeOffset.Now,
                        ExportStatus = 2,
                        ExportUser = doc.InsertUser,
                        Status = 2,
                        XML_stream_id = xmlStreamId,
                        TIF_stream_id = tifStreamId,
                        Extract_stream_id = null,
                        InsertDate = currDateTime,
                        InsertUser = GetCurrentUser(),
                        FileNameWithPath = null
                    };
                    entities.DocumentExtractExports.Add(newExtractDoc);
                    var newExtractId = newExtractDoc.DocumentExportExtractId;

                    var newRef = new DocumentExportReference
                    {
                        DocumentExportExtractId = newExtractId,
                        DocumentExportId = doc.DocumentExportId
                    };
                    entities.DocumentExportReferences.Add(newRef);

                    var newDocItem = new CaseFilingOutgoingFileDocumentItem
                    {
                        OutgoingId = caseFilingOutId,
                        DocumentId = doc.DocumentId,
                        InsertDate = currDateTime,
                        InsertUser = GetCurrentUser()
                    };
                    entities.CaseFilingOutgoingFileDocumentItems.Add(newDocItem);
                }
            }

            #endregion

            Commit();
        }

        public bool ProcessOutgoingFileForCaseFiling(Guid bulkProcessKey, string xmlPath, string tiffPath, string fileType, 
            string xmlContent, CaseFilingRecord record, IEnumerable<DocumentExport> caseFilingDocs)
        {
            var entities = GetContext<ComplaintEntities>();
            var currDateTime = GetCurrentDateTime();

            // dodaje xml-a do outgoing-ów
            var xmlStreamId = Guid.NewGuid();  
            var xmlFileStream = File.ReadAllBytes(xmlPath);
            var xmlOutgoingName = string.Format("{0:yyyyMMddhhmmssfff}_{1}", currDateTime, Path.GetFileName(xmlPath));
            AddOutgoingFile(xmlStreamId, xmlFileStream, xmlOutgoingName);

            // dodaje tifa do outgoing-ów
            Guid tifStreamId = Guid.Empty;
            if (!tiffPath.IsEmpty())
            {
                tifStreamId = Guid.NewGuid();
                var tifFileStream = File.ReadAllBytes(tiffPath);
                var tifOutgoingName = string.Format("{0:yyyyMMddhhmmssfff}_{1}", currDateTime, Path.GetFileName(tiffPath));
                
                AddOutgoingFile(tifStreamId, tifFileStream, tifOutgoingName);
            }

            #region Add XML to OutgoingPackage

            string bin = null;
            string organizationId = null;
            if (record.Complaint != null)
            {
                bin = record.Complaint.ARN.Substring(1, 6);
                organizationId = record.Complaint.OrganizationId;
            }

            var newXmlOutPackageItem = new OutgoingPackageItem
            {
                OrganizationId = organizationId,
                FileName = Path.GetFileName(xmlPath),
                stream_id = xmlStreamId,
                Status = 0,
                BIN = bin,
                InsertDate = currDateTime,
                InsertUser = GetCurrentUser()
            };

            entities.OutgoingPackageItems.Add(newXmlOutPackageItem);

            #endregion

            #region Add TIF to OutgoingPackage

            if (!tifStreamId.IsEmpty())
            {
                var newTifOutPackageItem = new OutgoingPackageItem
                {
                    OrganizationId = organizationId,
                    FileName = Path.GetFileName(tiffPath),
                    stream_id = tifStreamId,
                    Status = 0,
                    BIN = bin,
                    InsertDate = DateTime.UtcNow,
                    InsertUser = GetCurrentUser()
                };

                entities.OutgoingPackageItems.Add(newTifOutPackageItem);
            }

            #endregion

            #region Add XML to CaseFilingOutgoingFile

            var newCaseFilingOut = CreateCaseFilingOutgoingFile(
                record.CaseId,
                record.StageId,
                fileType,
                xmlPath,
                xmlContent,
                xmlStreamId,
                bulkProcessKey
                );
            var caseFilingOutId = newCaseFilingOut.OutgoingId;

            //var newCaseFilingOut = new CaseFilingOutgoingFile
            //{
            //    CaseId = record.CaseId,
            //    StageId = record.StageId,
            //    stream_id = xmlStreamId,
            //    FileType = fileType,
            //    FileName = Path.GetFileName(xmlPath),
            //    FileContent = xmlContent,
            //    ProcessKey = bulkProcessKey,
            //    InsertDate = currDateTime,
            //    InsertUser = GetCurrentUser()
            //};
            //entities.CaseFilingOutgoingFiles.Add(newCaseFilingOut);
            //var caseFilingOutId = newCaseFilingOut.OutgoingId;

            #endregion

            #region Add new document extracts, update representment status and add new CaseFilingDocumentItem

            foreach (var doc in caseFilingDocs.ToList())
            {
                var newDocItem = new CaseFilingOutgoingFileDocumentItem
                {
                    OutgoingId = caseFilingOutId,
                    DocumentId = doc.DocumentId,
                    InsertDate = currDateTime,
                    InsertUser = GetCurrentUser()
                };
                entities.CaseFilingOutgoingFileDocumentItems.Add(newDocItem);
            }

            #endregion

            Commit();
            return true;
        }

        #endregion
    }
}
