using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ComplaintTool.Shell.Documents;
using ComplaintTool.Models;
using ComplaintTool.Common.Config;
using System.Collections;
using System.Linq;
using ComplaintTool.Common.Enum;
using ComplaintTool.Common;
using ComplaintTool.Common.Utils;
using System.IO;
using ComplaintTool.Shell.Tests.Utils;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Data.Entity;

namespace ComplaintTool.Shell.Tests.Documents_Test
{
    [TestClass]
    public class DocumentExportTests
    {
        private const string ExportDocumentsFolder = @"C:\ComplaintHelpers\Documents";
        private const string ExportMerchantFolder = @"C:\ComplaintHelpers\Merchants";

        [TestMethod]
        public void ImportTest()
        {
            //var cmd = new NewDocumentImport()
            //{
            //    BaseFolderPath = @"C:\ComplaintHelpers\Incoming",
            //    OrganizationId = "VISA"
            //};

            //var result = cmd.Invoke();
            //result.GetEnumerator().MoveNext();
        }

        [TestMethod]
        public void ExportTest()
        {
            byte[] dbBytes;
            string fileName = null;
            List<ComplaintDocument> complaintDocuments;

            using (var entities = new ComplaintEntities(ComplaintConfig.Instance.GetEntityConnectionString()))
            {
                complaintDocuments = entities.ComplaintDocuments.Where(x => x.ExportStatus == (int)DocumentExportStatus.NotExported).ToList();
            }
            var cmd = new GetDocumentExport() { DocumentFolder = ExportDocumentsFolder, MerchantFolder = ExportMerchantFolder };
            cmd.Process();

            using (var entities = new ComplaintEntities(ComplaintConfig.Instance.GetEntityConnectionString()))
            {
                entities.Database.BeginTransaction();
                var processedComplaintDocuments = GetComplaintDocumentsAfterProcess(complaintDocuments, entities);

                if (complaintDocuments.Count != processedComplaintDocuments.Count)
                    Assert.Fail();

                var storedProcedure = new StoredProcedure(entities);
                foreach (var complaintDoc in processedComplaintDocuments)
                {
                    storedProcedure.GetDocumentProc(complaintDoc.DocumentId, out fileName, out dbBytes);
                    if (!Validate(complaintDoc, dbBytes))
                        Assert.Fail();
                }
            }
        }

        private List<ComplaintDocument> GetComplaintDocumentsAfterProcess(List<ComplaintDocument> documentsBeforeProcess, ComplaintEntities entities)
        {

            entities.Database.BeginTransaction();
            var processedComplaintDocuments = (from compDoc in documentsBeforeProcess
                                               join docUpd in entities.ComplaintDocuments.Include(x => x.Complaint) on compDoc.DocumentId equals docUpd.DocumentId
                                               select docUpd).ToList();
            return processedComplaintDocuments;
        }

        private bool Validate(ComplaintDocument complaintDocument, byte[] bytes)
        {
            if (complaintDocument.ExportStatus != 2)
                return false;
            var fileBytes = ReadFile(ExportDocumentsFolder, complaintDocument.FileName, complaintDocument.Complaint.OrganizationId);

            return FileComparer.Compare(fileBytes, bytes);
        }

        private byte[] ReadFile(string basePath,string fileName,string organizationId=null)
        {
            var file = fileName.ToString().Replace("_", "");
            var dirPath = string.IsNullOrEmpty(organizationId)? basePath:Path.Combine(basePath,organizationId);
            var pdfPath = dirPath + @"\" + file;
            return File.ReadAllBytes(pdfPath);
        }

    }
}
