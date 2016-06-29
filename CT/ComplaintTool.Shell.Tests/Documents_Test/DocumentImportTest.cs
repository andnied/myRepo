using ComplaintTool.Common.Config;
using ComplaintTool.Models;
using ComplaintTool.Shell.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Shell.Tests.Documents_Test
{
    [TestClass]
    public class DocumentImportTest
    {
        private readonly string importDocTestFolderPath = Path.Combine("Documents_Test", "TestFolder");
        private string[] _testedFilesNames;
        private DateTime _startTime;

        [TestMethod]
        public void ImportTest()
        {
            //string testFolder = GetTestedFolder();
            //LoadTestFilesNames(testFolder);
            //_startTime=DateTime.Now;
            //var documentImport = new NewDocumentImport() { BaseFolderPath = testFolder, OrganizationId = "VISA", IsWriteMode = false };
            //documentImport.Process();
            //Validate();
        }

        private void LoadTestFilesNames(string folder)
        {
            var files = System.IO.Directory.GetFiles(folder);
            _testedFilesNames = files.Select(x => Path.GetFileName(x)).ToArray();
        }

        private string GetTestedFolder()
        {
            var projectPath = Directory.GetCurrentDirectory();
            var processingFolder = Path.Combine(projectPath, importDocTestFolderPath);
            return processingFolder;
        }

        
        //41733
        /*private void ReadFile(int docId, string folderPath)
        {
            using (var entities = new ComplaintEntities())
            {
                entities.Database.BeginTransaction();

                var proc = new Utils.StoredProcedure(entities);
                byte[] bytes;
                string fileName;
                proc.GetDocumentProc(docId, out fileName, out bytes);
                var fileDirectory = Path.Combine(folderPath, fileName);
                System.IO.File.WriteAllBytes(fileDirectory, bytes);
            }
        }*/
         
        private void Validate()
        {  
            using(var entities=new ComplaintEntities(ComplaintConfig.Instance.GetEntityConnectionString()))
            {
                CheckDocumentExist(entities);
            }
        }

        private void CheckDocumentExist(ComplaintEntities entities)
        {
            var processedComplaintDocs = entities.Set<ComplaintDocument>().Where(x => x.InsertDate.Value>=_startTime && x.InsertDate.Value <= DateTime.Now).ToList();
            processedComplaintDocs = processedComplaintDocs.Where(x => x.InsertDate.Value.DateTime >= _startTime && x.InsertDate.Value.DateTime <= DateTime.Now).ToList();
            
            foreach (var fileName in _testedFilesNames)
            {
                ComplaintDocument complaintDoc = processedComplaintDocs.FirstOrDefault(x => x.SourceFileName.Contains(fileName));
                if (complaintDoc == null || complaintDoc.stream_id == null)
                    Assert.Fail();
            }
        }

    }
}
