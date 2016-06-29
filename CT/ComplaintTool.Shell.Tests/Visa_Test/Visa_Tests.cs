using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ComplaintTool.Shell.VISA;
using System.IO;
using ComplaintTool.Models;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data;
using System.Data.SqlClient;
using ComplaintTool.Visa.Model;
using System.Data.Entity;
using System.Security.Principal;

namespace ComplaintTool.Shell.Tests.Visa_Test
{
    [TestClass]
    public class Visa_Tests
    {
        #region Incoming

        private readonly string _visaIncomingPath = Path.Combine("Visa_Test", "Incoming");

        //[TestMethod]
        //public void VisaIncomingRegistrationTest()
        //{
        //    var cmd = new NewVisaIncomingRegistration()
        //    {
        //        BaseFolderPath = _visaIncomingPath,
        //        _debug = false
        //    };

        //    cmd.Process();
        //}

        //[TestMethod]
        //public void VisaIncomingProcessingTest()
        //{
        //    var cmd = new NewVisaIncomingProcessing();

        //    cmd.Process();
        //}

        [TestMethod]
        public void VisaIncomingRegistrationWithCheckTest()
        {
            //var path = _visaIncomingPath;
            //var incIds = new List<string>();
            //var ids = new List<int>();

            //var cmd = new NewVisaIncomingRegistration()
            //{
            //    BaseFolderPath = path
            //};

            //foreach (var file in Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly))
            //{
            //    var incId = GetIncomingId(file);
            //    incIds.Add(incId);
            //    ids.Add(GetFileId(incId));
            //}

            //ClearIncomingFiles(ids);

            //incIds.ForEach(id => Assert.IsFalse(GetFileId(id) != 0, "Could not clear incoming files."));

            //cmd.Process();

            //incIds.ForEach(id => Assert.IsTrue(IncomingExists(id), "Incoming file with id = '{0}' does not exist.", id));

            foreach (var file in Directory.GetFiles(_visaIncomingPath))
            {
                new NewVisaIncomingRegistration() { IsWriteMode = false, FilePath = file }.Process();
            }
        }

        [TestMethod]
        public void VisaIncomingProcessingWithCheckTest()
        {
            new NewVisaIncomingProcessing() { IsWriteMode = false, FileId = 6697 }.Process();
            //var ids = GetIdsForProcessing();
            //var cmd = new NewVisaIncomingProcessing();
            //cmd.Process();
            //ids.ForEach(id => Assert.IsTrue(IsParsed(id), string.Format("Visa Incoming with fileId = {0} not parsed successfully.", id)));
        }

        private int GetFileId(string incId)
        {
            int res;

            using (var entities = new ComplaintEntities())
            {
                res = (from r in entities.RegOrgIncomingFiles
                          where r.IncomingFileID == incId
                          select r.FileId).FirstOrDefault();
            }

            return res;
        }

        private void ClearIncomingFiles(List<int> fileIds)
        {
            using (var entities = new ComplaintEntities())
            {
                entities.RegOrgIncomingFiles.RemoveRange((from r in entities.RegOrgIncomingFiles
                                                          where fileIds.Contains(r.FileId)
                                                          select r).ToList());

                using (var command = entities.Database.Connection.CreateCommand())
                {
                    entities.Database.Connection.Open();
                    command.CommandType = CommandType.Text;
                    command.CommandText = string.Format("delete from [dbo].[IncomingFiles] where [stream_id] in (select [stream_id] from [File].[RegOrgIncomingFiles] where [fileid] in ({0}))", string.Join(",", fileIds));
                    command.ExecuteNonQuery();
                }

                entities.SaveChanges();
            }
        }

        private string GetIncomingId(string path)
        {
            var line = string.Empty;

            using (var reader = new StreamReader(path))
            {
                line = reader.ReadLine();
            }

            var tc90 = new TC90(line);
            var processingDate = ComplaintTool.Common.Utils.Convert.JulianDateToDateTimeForVisa(tc90.ProcessingDate).ToString("yyyyMMdd");
            var incomingId = string.Format("{0}{1}{2}", tc90.ProcessingBIN, processingDate, tc90.IncomingFileID);

            return incomingId;
        }

        private bool IncomingExists(string incId)
        {
            object res = null;

            using (var ent = new ComplaintEntities())
            {
                using (var command = ent.Database.Connection.CreateCommand())
                {
                    ent.Database.Connection.Open();
                    command.CommandType = CommandType.Text;
                    command.CommandText = string.Format("select count(r.[fileId]) from [File].[RegOrgIncomingFiles] r inner join [dbo].[IncomingFiles] f on r.[stream_id] = f.[stream_id] where r.[IncomingFileId] = '{0}'", incId);
                    res = command.ExecuteScalar();
                }
            }

            return (int)res == 1;
        }

        private List<int> GetIdsForProcessing()
        {
            List<int> res;

            using (var ent = new ComplaintEntities())
            {
                res = (from r in ent.RegOrgIncomingFiles
                       where r.ErrorFlag == false && r.Status == 2 && r.OrganizationId == "VISA" && r.stream_id.HasValue
                       select r.FileId).ToList();
            }

            return res;
        }

        private bool IsParsed(int fileId)
        {
            RegOrgIncomingFile res = null;

            using (var ent = new ComplaintEntities())
            {
                res = ent.RegOrgIncomingFiles.FirstOrDefault(r => r.FileId == fileId);
            }

            return res != null ? res.ErrorFlag == false && res.Status == 3 : false;
        }

        //[TestMethod]
        //public void IncomingTestWithParametersCheck()
        //{
        //    string path = @"C:\TEST\Complaint Tests\VISA Incoming";

        //    if (!(Directory.Exists(path)))
        //        Directory.CreateDirectory(path);

        //    var fileIds = GetIdsFromDbStreams();

        //    if (fileIds.Count > 0)
        //    {
        //        ClearFolder(path);

        //        foreach (var fileId in fileIds)
        //            SaveFileStream(fileId, path);

        //        ClearIncomingFiles(fileIds);
        //    }

        //    var cmd = new NewVisaIncomingRegistration()
        //    {
        //        BaseFolderPath = path,
        //        _debug = false
        //    };

        //    var result = cmd.Invoke();
        //    result.GetEnumerator().MoveNext();
        //}

        //private void ClearFolder(string path)
        //{
        //    foreach (var file in Directory.GetFiles(path,"*", SearchOption.AllDirectories))
        //    {
        //        File.Delete(file);
        //    }
        //}

        //private List<int> GetIdsFromDbStreams()
        //{
        //    List<int> res = null;

        //    using (var entities = new ComplaintEntities())
        //    {
        //        res = (
        //            from r in entities.RegOrgIncomingFiles
        //            where r.OrganizationId == "VISA" && r.stream_id != null && r.Name.ToUpper().Contains(".txt")
        //            select r.FileId
        //            ).ToList();
        //    }

        //    return res;
        //}

        //private void SaveFileStream(int fileId, string path)
        //{
        //    var fileStream = new ObjectParameter("file_Stream", typeof(byte[]));

        //    using (var entities = new ComplaintEntities())
        //    {
        //        entities.GetOrgIncomingFile(fileId, fileStream);
        //    }

        //    File.WriteAllBytes(Path.Combine(path, Guid.NewGuid().ToString() + ".txt"), (byte[])fileStream.Value);
        //}

        #endregion

        #region Outgoing

        private readonly string _visaOutgoingPath = Path.Combine("Visa_Test", "Outgoing");

        [TestMethod]
        public void OutgoingExtractTest()
        {
            var reps = GetRepsForExtract();
            Directory.GetFiles(_visaOutgoingPath, "*.pdf", SearchOption.AllDirectories).ToList().ForEach(f => File.Delete(f));

            var cmd = new GetVisaOutgoingFiles()
            {
                BaseFolderPath = _visaOutgoingPath,
                Type = "extract"
            };

            cmd.Process();

            Assert.IsTrue(ValidateExtracts(reps));
        }

        [TestMethod]
        public void OutgoingExportTest()
        {
            var docs = GetDocsForExport();

            var cmd = new GetVisaOutgoingFiles()
            {
                BaseFolderPath = _visaOutgoingPath,
                Type = "export"
            };

            cmd.Process();

            Assert.IsTrue(ValidateExports(docs));
        }

        private List<Representment> GetRepsForExtract()
        {
            List<Representment> res = null;

            using (var entities = new ComplaintEntities())
            {
                res = (from r in entities.Representments.Include(r => r.RepresentmentDocuments).Include(r => r.Complaint)
                           where r.Status == 2 && r.Complaint.OrganizationId == "VISA" && r.DocumentationIndicator
                           select r).ToList();
            }

            return res;
        }

        private bool ValidateExtracts(List<Representment> reps)
        {
            foreach (var rep in reps)
            {
                var arn = rep.Complaint.ARN;
                var date = DateTime.Now.ToString("yyyyMMdd");
                var dirPath = _visaOutgoingPath + @"\" + date + @"\" + arn;

                if (!(Directory.Exists(dirPath)) || Directory.GetFiles(dirPath, "*.pdf", SearchOption.TopDirectoryOnly).Count() < rep.RepresentmentDocuments.Where(d => d.Status == 0).ToList().Count)
                    return false;

                List<RepresentmentExtractDocument> extractDocs = null;
                var repDocIds = rep.RepresentmentDocuments.Select(d => d.DocumentId).ToList();

                using (var entities = new ComplaintEntities())
                {
                    extractDocs = (from e in entities.RepresentmentExtractDocuments
                                   where e.CaseId == rep.CaseId
                                   select e).Where(e => repDocIds.Contains(e.DocumentId)).ToList();
                }

                foreach (var repDoc in rep.RepresentmentDocuments)
                {
                    if (!(extractDocs.Any(e => e.RepresentmentDocumentsId == repDoc.RepresentmentDocumentsId)))
                        return false;
                }
            }

            return true;
        }

        private List<DocumentExport> GetDocsForExport()
        {
            List<DocumentExport> doc = null;

            using (var entities = new ComplaintEntities())
            {
                doc = (from d in entities.DocumentExports.Include(d => d.Complaint)
                       where d.Status == 0 && d.OrganizationId == "VISA"
                       select d).ToList();
            }

            return doc;
        }

        private bool ValidateExports(List<DocumentExport> docs)
        {
            foreach (var doc in docs)
            {
                var arn = doc.Complaint.ARN;
                var date = DateTime.Now.ToString("yyyyMMdd");
                var dirPath = _visaOutgoingPath + @"\" + date + @"\" + arn;

                if (!(Directory.Exists(dirPath)) || Directory.GetFiles(dirPath, "*.pdf", SearchOption.TopDirectoryOnly).Count() < 1)
                    return false;

                List<DocumentExtractExport> docExports = null;
                List<DocumentExportReference> docExportRefs = null;

                using (var entities = new ComplaintEntities())
                {
                    docExports = (from e in entities.DocumentExtractExports
                                  where e.CaseId == doc.CaseId && e.ExportUser == doc.InsertUser
                                  select e).ToList();

                    docExportRefs = (from r in entities.DocumentExportReferences
                                     where r.DocumentExportId == doc.DocumentExportId
                                     select r).ToList();
                }

                if (docExports == null || docExportRefs == null || docExports.Count == 0 || docExportRefs.Count == 0)
                    return false;
            }

            return true;
        }

        //[TestMethod]
        //public void OutgoingWithFileStreamTest()
        //{
        //    var oldPath = @"C:\TEST\Complaint Tests\_Visa Outgoing";
        //    var newPath = @"C:\TEST\Complaint Tests\VISA Outgoing auto";
        //    var oldFiles = Directory.GetFiles(oldPath, "*.pdf", SearchOption.AllDirectories);

        //    if (!(oldFiles.Any()))
        //        throw new Exception("Outgoing files from old process not found.");

        //    var cmd = new GetVisaOutgoingFiles()
        //    {
        //        BaseFolderPath = newPath,
        //        Type = "extract"
        //    };

        //    var result = cmd.Invoke();
        //    result.GetEnumerator().MoveNext();

        //    cmd = new GetVisaOutgoingFiles()
        //    {
        //        BaseFolderPath = newPath,
        //        Type = "export"
        //    };

        //    result = cmd.Invoke();
        //    result.GetEnumerator().MoveNext();

        //    var newFiles = Directory.GetFiles(oldPath, "*.pdf", SearchOption.AllDirectories);

        //    if (!(newFiles.Any()))
        //        throw new Exception("Outgoing files from new process not found.");

        //    int counter;
        //    Assert.IsTrue(FilesAreTheSame(oldFiles, newFiles, out counter), "Outgoing Visa files are not the same.");
        //    Assert.IsTrue(counter > 0, "None files have been compared.");
        //}

        //private bool FilesAreTheSame(string[] oldFiles, string[] newFiles, out int counter)
        //{
        //    counter = 0;

        //    foreach (var newFile in newFiles)
        //    {
        //        var oldFile = oldFiles.FirstOrDefault(f => Path.GetFileName(f) == Path.GetFileName(newFile));
        //        if (oldFile == null)
        //            continue;

        //        counter++;
        //        var newStream = File.ReadAllBytes(newFile);
        //        var oldStream = File.ReadAllBytes(oldFile);

        //        if (!(Enumerable.SequenceEqual(newStream, oldStream)))
        //            return false;
        //    }

        //    return true;
        //}

        //[TestMethod]
        //public void RecordsPerFileTest()
        //{
        //    var path = @"C:\TEST\Complaint Tests\_VISA Outgoing";

        //    Assert.IsTrue(RecordsPerFileExist(Directory.GetFiles(path, "*.pdf", SearchOption.AllDirectories)));
        //}

        //private bool RecordsPerFileExist(string[] files)
        //{
        //    using (var complaint = new ComplaintEntities())
        //    {
        //        foreach (var file in files)
        //        {
        //            var creationStamp = new FileInfo(file).CreationTimeUtc.ToString("yyyyMMddhhmm");
        //            var fileName = Path.GetFileName(file);
        //            var name = string.Format("{0}*_{1}", creationStamp, fileName);

        //            //var res = from o in complaint
        //        }
        //    }            

        //    return true;
        //}

        #endregion
    }
}
