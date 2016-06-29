using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ComplaintTool.Shell.Postilion;
using ComplaintTool.Models;
using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Repos;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace ComplaintTool.Shell.Tests.Postilion_Test
{
    [TestClass]
    public class Postilion_Test
    {
        private readonly string _postilionIncomingPath = Path.Combine("Postilion_Test", "Incoming");
        private readonly string _postilionOutgoingPath = @"C:\TEST\Complaint Tests\Postilion Outgoing";
        private DateTime _startDate;
        private DateTime _endDate;

        #region Incoming

        [TestMethod]
        public void PostilionIncomingResponseTest()
        {
            var files = Directory.GetFiles(_postilionIncomingPath, "*.response", SearchOption.TopDirectoryOnly).ToList();
            var cmd = new NewPostilionResponseFile()
            {
                FilePath = ""
            };

            cmd.Process();
            Assert.IsTrue(ValidateResponses(files));
        }

        [TestMethod]
        public void PostilionIncomingProcessingTest()
        {
            var files = GetFilesForProcessing();
            var cmd = new NewPostilionProcessingFiles();

            cmd.Process();
            Assert.IsTrue(ValidateProcessing(files));
        }

        #region Helper

        private bool ValidateResponses(List<string> files)
        {
            var fileNames = files.Select(f => Path.GetFileName(f));
            List<ResponsePostilionFile> importedFiles = null;

            using (var entities = new ComplaintEntities())
            {
                importedFiles = (from p in entities.ResponsePostilionFiles
                                 where fileNames.Contains(p.FileName) && !(p.ErrorFlag.Value) && p.Status == 0
                                 select p).ToList();
            }

            if (files.Count != importedFiles.Count)
                return false;

            return true;
        }

        private List<ResponsePostilionFile> GetFilesForProcessing()
        {
            List<ResponsePostilionFile> files = null;

            using (var entities = new ComplaintEntities())
            {
                files = (from r in entities.ResponsePostilionFiles
                         where r.ErrorFlag == false && r.Status == 0 && r.IsReceived == true && r.stream_id.HasValue
                         select r).ToList();
            }

            return files;
        }

        private bool ValidateProcessing(List<ResponsePostilionFile> files)
        {
            List<ResponsePostilionFile> processedFiles = null;
            var fileNames = files.Select(f => f.FileName);

            using (var entities = new ComplaintEntities())
            {
                processedFiles = (from r in entities.ResponsePostilionFiles
                                  where !(r.ErrorFlag.Value) 
                                  && r.Status == 3 
                                  && r.ProcesingStart.HasValue 
                                  && r.ProcesingFinished.HasValue
                                  && fileNames.Contains(r.FileName)
                                  select r).ToList();
            }

            if (files.Count < processedFiles.Count)
                return false;

            return true;
        }

        //[TestMethod]
        //public void GetFile()
        //{
        //    using (var entities = new ComplaintEntities())
        //    {
        //        Guid? streamId;

        //        using (var command = entities.Database.Connection.CreateCommand())
        //        {
        //            entities.Database.Connection.Open();
        //            command.CommandType = CommandType.Text;
        //            command.CommandText = "select [stream_id] from [File].[CLFReport] where [CLFReportId] = 4";
        //            streamId = (Guid?)command.ExecuteScalar();
        //        }

        //        ObjectParameter stream = new ObjectParameter("file_stream", typeof(byte[]));
        //        entities.GetIncomingFile(streamId, stream);

        //        File.WriteAllBytes(Path.Combine(_postilionIncomingPath, streamId.ToString() + ".csv"), (byte[])stream.Value);
        //    }
        //}

        #endregion

        #endregion

        #region Outgoing

        [TestMethod]
        public void TestPostilionVisaRepresentment()
        {
            //var extracts = GetREExtracts("VISA");
            var cmd = new GetPostilionExtractedFiles()
            {
                BaseFolderPath = _postilionOutgoingPath,
                Organization = "VISA",
                ExtractType = "representment"
            };

            _startDate = DateTime.Now;
            cmd.Process();
            _endDate = DateTime.Now;

            Assert.IsTrue(ValidateExtracts<RepresentmentExtract>());
        }

        [TestMethod]
        public void TestPostilionMCRepresentment()
        {
            //var extracts = GetREExtracts("MC");
            var cmd = new GetPostilionExtractedFiles()
            {
                BaseFolderPath = _postilionOutgoingPath,
                Organization = "MC",
                ExtractType = "representment"
            };

            _startDate = DateTime.Now;
            cmd.Process();
            _endDate = DateTime.Now;

            Assert.IsTrue(ValidateExtracts<RepresentmentExtract>());
        }

        [TestMethod]
        public void TestPostilionVisaFeeCollection()
        {
            var cmd = new GetPostilionExtractedFiles()
            {
                BaseFolderPath = _postilionOutgoingPath,
                Organization = "VISA",
                ExtractType = "feecollection"
            };

            _startDate = DateTime.Now;
            cmd.Process();
            _endDate = DateTime.Now;

            Assert.IsTrue(ValidateExtracts<FeeCollectionExtract>());
        }

        [TestMethod]
        public void TestPostilionMCFeeCollection()
        {
            var cmd = new GetPostilionExtractedFiles()
            {
                BaseFolderPath = _postilionOutgoingPath,
                Organization = "MC",
                ExtractType = "feecollection"
            };

            _startDate = DateTime.Now;
            cmd.Process();
            _endDate = DateTime.Now;

            Assert.IsTrue(ValidateExtracts<FeeCollectionExtract>());
        }

        private bool ValidateExtracts<T>() where T : class
        {
            var files = Directory.GetFiles(_postilionOutgoingPath, "*.csv", SearchOption.TopDirectoryOnly).Where(f => File.GetLastWriteTime(f) > _startDate && File.GetLastWriteTime(f) < _endDate);
            if (files.Count() == 0)
                return false;

            using (var entities = new ComplaintEntities())
            {
                ICollection postFiles = null;

                if (typeof(T) == typeof(RepresentmentExtract))
                {
                    postFiles = (from r in entities.RepresentmentPostilionFiles
                                        where r.InsertDate > _startDate && r.InsertDate < _endDate
                                        select r).ToList();
                }
                else if (typeof(T) == typeof(FeeCollectionExtract))
                {
                    postFiles = (from f in entities.FeeCollectionPostilionFiles
                                        where f.InsertDate > _startDate && f.InsertDate < _endDate
                                        select f).ToList();
                }

                if (postFiles.Count == 0)
                    return false;

                var procKeys = (from p in entities.ProcessKeys
                               where p.InsertDate > _startDate && p.InsertDate < _endDate
                               select p).ToList();

                if (procKeys.Count == 0)
                    return false;
            }

            return true;
        }

        //private List<RepresentmentExtract> GetREExtracts(string org)
        //{
        //    using (var entities = new ComplaintEntities())
        //    {
        //        return (from r in entities.RepresentmentExtracts
        //                    where r.Status == 0 && r.Representment.Complaint.OrganizationId == org
        //                    select r).ToList();
        //    }
        //}

        //private List<FeeCollectionExtract> GetFEExtracts(string org)
        //{
        //    using (var entities = new ComplaintEntities())
        //    {
        //        return (from f in entities.FeeCollectionExtracts
        //                where f.Status == 0 && f.FeeCollection.Complaint.OrganizationId == org
        //                select f).ToList();
        //    }
        //}

        #endregion
    }
}
