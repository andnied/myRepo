using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ComplaintTool.Shell.EvoGermany;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using ComplaintTool.Models;

namespace ComplaintTool.Shell.Tests
{
    [TestClass]
    public class EvoGermany_Test
    {
        private readonly string _incomingPath = Path.Combine("EVOGermany_Test", "Incoming");
        private DateTime _start;
        private DateTime _end;

        [TestMethod]
        public void EvoGermanyIncomingPdfTest()
        {
            //var files = Directory.GetFiles(_incomingPath, "*", SearchOption.TopDirectoryOnly).Where(f => Path.GetExtension(f).ToUpper() == ".PDF");
            //var cmd = new NewEvoGermanyIncomingFile()
            //{
            //    BaseFolderPath = _incomingPath,
            //    FileType = "pdf"
            //};

            //_start = DateTime.Now;
            //cmd.Process();
            //_end = DateTime.Now;

            //Assert.IsTrue(ValidatePdfIncoming(files));
        }

        [TestMethod]
        public void EvoGermanyIncomingClfTest()
        {
            //var files = Directory.GetFiles(_incomingPath, "*", SearchOption.TopDirectoryOnly)
            //    .Where(f => Path.GetExtension(f).ToUpper() == ".CSV")
            //    .Select(f => f.Replace(Path.GetFileName(f), Path.Combine("Temp", Path.GetFileName(f))));

            //ClearClfs(files);

            //var cmd = new NewEvoGermanyIncomingFile()
            //{
            //    BaseFolderPath = _incomingPath,
            //    FileType = "clf"
            //};

            //_start = DateTime.UtcNow;
            //cmd.Process();
            //_end = DateTime.UtcNow.AddSeconds(1);

            //Assert.IsTrue(ValidateClfIncoming(files));
        }

        [TestMethod]
        public void OutgoingSuccessTest()
        {
            var cmd = new GetEvoGermanyOutgoingFiles();
            cmd.Process();
        }

        private bool ValidatePdfIncoming(IEnumerable<string> files)
        {
            var fileNames = files.Select(f => Path.GetFileNameWithoutExtension(f));
            List<ComplaintDocument> docs = null;

            using (var entities = new ComplaintEntities())
            {
                docs = (from d in entities.ComplaintDocuments
                        where fileNames.Contains(d.SourceFileName) && d.InsertDate > _start && d.InsertDate < _end
                        select d).ToList();
            }

            return files.Count() == docs.Count;
        }

        private void ClearClfs(IEnumerable<string> files)
        {
            using (var entities = new ComplaintEntities())
            {
                var reports = (from r in entities.CLFReports.Include(r => r.CLFReportItems)
                           where files.Contains(r.FileName)
                           select r).ToList();

                var items = reports.SelectMany(r => r.CLFReportItems);

                entities.CLFReportItems.RemoveRange(items);
                entities.CLFReports.RemoveRange(reports);
                entities.SaveChanges();
            }
        }

        private bool ValidateClfIncoming(IEnumerable<string> files)
        {
            List<CLFReport> reports = null;

            using (var entities = new ComplaintEntities())
            {
                reports = (from r in entities.CLFReports.Include(r => r.CLFReportItems)
                           where files.Contains(r.FileName) && r.ProcesingStart >= _start && r.ProcesingStart <= _end
                           select r).ToList();
            }

            if (reports.Count == 0)
                return false;

            foreach (var report in reports)
                if (report.CLFReportItems.Count == 0)
                    return false;

            return true;
        }
    }
}
