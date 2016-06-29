using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common.Config;
using ComplaintTool.Common.Utils;
using ComplaintTool.Models;
using ComplaintTool.MCProImageInterface.Model;
using ComplaintTool.Shell.ImagePro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComplaintTool.Shell.Tests
{
    [TestClass]
    public class ImagePro_Tests
    {
        [TestInitialize]
        public void Initialize()
        {

        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void NewMCProImage_IncomingFile_Process()
        {
            //PrepareImageProIncoming(TestHelper.GetMethodDestinationFolder());

            //var cmd = new NewMCProImageIncomingFiles();
            //cmd.BaseFolderPath = TestHelper.GetMethodDestinationFolder();
            //cmd.Process();
        }

        [TestMethod]
        public void GetMCProImage_OutgoingFile_Extract_CaseFiling()
        {
            var cmd = new GetMCProImageOutgoingFiles();
            cmd.TempFolderPath = TestHelper.GetMethodDestinationFolder();
            cmd.ProcessName = GetMCProImageOutgoingFiles.CaseFiling;
            cmd.Process();
        }

        [TestMethod]
        public void GetMCProImage_OutgoingFile_Extract_Chargeback()
        {
            ResetRepresentmentDocumentStatusForTest();

            var cmd = new GetMCProImageOutgoingFiles();
            cmd.TempFolderPath = TestHelper.GetMethodDestinationFolder();
            cmd.ProcessName = GetMCProImageOutgoingFiles.Chargeback;
            cmd.Process();

            Assert.IsTrue(CheckRepresentmentDocumentStatus());
        }

        [TestMethod]
        public void GetMCProImage_OutgoingFile_Extract_Rrf()
        {
            var cmd = new GetMCProImageOutgoingFiles();
            cmd.TempFolderPath = TestHelper.GetMethodDestinationFolder();
            cmd.ProcessName = GetMCProImageOutgoingFiles.RRF;
            cmd.Process();
        }

        public static void ResetRepresentmentDocumentStatusForTest()
        {
            using (var ctx = new ComplaintEntities(TestHelper.GetConnectionString()))
            {
                string orgId = ComplaintTool.Common.Enum.Organization.MC.ToString();
                var representments = ctx.Representments
                    .Include("RepresentmentDocuments")
                    .Include("Complaint")
                    .Include("RepresentmentExtracts")
                    .Where(x => x.Complaint.OrganizationId == orgId)
                    .Where(x => x.Status != 3)
                    .Where(x => x.RepresentmentDocuments.Any())
                    .ToList();

                foreach (var r in representments)
                {
                    r.DocumentationIndicator = true;
                    r.Status = 2;
                    foreach (var rd in r.RepresentmentDocuments)
                    {
                        rd.Status = 0;
                        ctx.Entry(rd).State = System.Data.Entity.EntityState.Modified;
                    }
                    foreach (var re in r.RepresentmentExtracts)
                    {
                        re.PostilionStatus = "00";
                        ctx.Entry(re).State = System.Data.Entity.EntityState.Modified;
                    }
                    ctx.Entry(r).State = System.Data.Entity.EntityState.Modified;
                }
                ctx.SaveChanges();
            }
        }

        public static bool CheckRepresentmentDocumentStatus()
        {
            using (var ctx = new ComplaintEntities(TestHelper.GetConnectionString()))
            {
                string orgId = ComplaintTool.Common.Enum.Organization.MC.ToString();
                var representments = ctx.Representments
                    .Include("RepresentmentDocuments")
                    .Include("Complaint")
                    .Include("RepresentmentExtracts");

                var notProcessedRepresentment = representments
                    .Where(x => x.Complaint.OrganizationId == orgId)
                    .Where(x => x.Status != 3)
                    .Where(x => x.RepresentmentDocuments.Any())
                    .ToList();

                if (notProcessedRepresentment.Any())
                    return false;

                var processedRepresentment = representments
                    .Where(x => x.Complaint.OrganizationId == orgId)
                    .Where(x => x.Status == 3)
                    .Where(x => x.RepresentmentDocuments.Any())
                    .ToList();

                foreach (var r in processedRepresentment.SelectMany(x => x.RepresentmentDocuments))
                {
                    // TODO test
                }
            }

            return true;
        }

        public static void PrepareImageProIncoming(string dirPath)
        {
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            using (var ctx = new ComplaintEntities(TestHelper.GetConnectionString()))
            {
                var amwFiles = ctx.CaseFilingIncomingFiles
                    .Take(10)
                    .Where(x => x.FileType == "AMW")
                    .Select(x => new { FileName = x.FileName, FileContent = x.FileContent })
                    .ToList();
                var rmwFiles = ctx.CaseFilingIncomingFiles
                    .Take(10)
                    .Where(x => x.FileType == "RMW")
                    .Select(x => new { FileName = x.FileName, FileContent = x.FileContent })
                    .ToList();
                var smwFiles = ctx.CaseFilingIncomingFiles
                    .Take(10)
                    .Where(x => x.FileType == "SMW")
                    .Select(x => new { FileName = x.FileName, FileContent = x.FileContent })
                    .ToList();

                var fileStream = new ObjectParameter("file_stream", typeof(byte[]));
                // TODO znalezc sposob na pobranie dowolnego bez użycia na sztywno guid
                var sampleTif = ctx.GetIncomingFile(new Guid("4468BD90-D9E4-4D96-B7A0-A7B7F54294F5"), fileStream);
                var stream = (byte[])fileStream.Value;

                int i = 1;
                foreach (var f in amwFiles)
                {
                    try
                    {
                        var obj = XmlUtil.Deserialize<Amw>(f.FileContent, false);
                        var fileName = Path.Combine(dirPath, Path.GetFileNameWithoutExtension(f.FileName));
                        XmlUtil.SerializeToFile(obj, fileName + ".xml");
                        File.WriteAllBytes(fileName + ".tif", stream);
                        i++;
                    }
                    catch { }
                }
                foreach (var f in rmwFiles)
                {
                    try
                    {
                        var obj = XmlUtil.Deserialize<Rmw>(f.FileContent, false);
                        var fileName = Path.Combine(dirPath, Path.GetFileNameWithoutExtension(f.FileName));
                        XmlUtil.SerializeToFile(obj, fileName + ".xml");
                        File.WriteAllBytes(fileName + ".tif", stream);
                        i++;
                    }
                    catch { }
                }
                foreach (var f in smwFiles)
                {
                    try
                    {
                        var obj = XmlUtil.Deserialize<Smw>(f.FileContent, false);
                        var fileName = Path.Combine(dirPath, Path.GetFileNameWithoutExtension(f.FileName));
                        XmlUtil.SerializeToFile(obj, fileName + ".xml");
                        File.WriteAllBytes(fileName + ".tif", stream);
                        i++;
                    }
                    catch { }
                }

                var zipPath = Path.Combine(dirPath, DateTime.Now.ToFileTime().ToString());
                var zipFileList = Directory.GetFiles(dirPath, "*.xml").Union(Directory.GetFiles(dirPath, "*.tif")).ToArray();
                ComplaintTool.Common.Utils.Extract.CreateZip(zipPath, zipFileList);
                foreach (var file in zipFileList)
                {
                    if (File.Exists(file))
                        File.Delete(file);
                }
            }
        }
    }
}
