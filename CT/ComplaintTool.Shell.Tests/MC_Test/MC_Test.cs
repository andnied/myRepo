using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ComplaintTool.Shell.MasterCard;
using System.IO;
using ComplaintTool.Common.Config;
using System.Collections.Generic;
using System.Linq;
using ComplaintTool.Models;

namespace ComplaintTool.Shell.Tests.MC_Test
{
    [TestClass]
    public class MC_Test
    {
        private readonly string _mcIncomingPath = Path.Combine("MC_Test", "Incoming");
        private readonly string _mcParser = Path.Combine("MC_Test", "MasterCard_IPM_Parser.jar");
        private readonly string[] _masterCardExt = ComplaintConfig.Instance.Parameters["MasterCardExt"].ParameterValue.Split('|');

        [TestMethod]
        public void MCIncomingRegistrationTest()
        {
            var files = Directory.GetFiles(_mcIncomingPath, "*", SearchOption.TopDirectoryOnly).Where(f => _masterCardExt.Contains(Path.GetExtension(f))).ToList();
            //var startTime = DateTime.UtcNow;

            foreach (var file in files)
            {
                var cmd = new NewMasterCardRegistration()
                {
                    FilePath = file
                };

                cmd.Process();
            }
            //var endTime = DateTime.UtcNow;

            //Assert.IsTrue(ValidateRegistration(files, startTime, endTime.AddSeconds(1)));
        }

        [TestMethod]
        public void MCIncomingProcessingTest()
        {
            //var regOrgs = GetRegOrgsForProcessing();

            var cmd = new GetMasterCardProcessing()
            {
                IsWriteMode = false,
                FileId = 6688
            };

            cmd.Process();

            //Assert.IsTrue(ValidateProcessing(regOrgs));
        }

        #region Helper

        private bool ValidateRegistration(List<string> files, DateTime start, DateTime end)
        {
            List<RegOrgIncomingFile> regOrgs = null;

            using (var entities = new ComplaintEntities())
            {
                regOrgs = (from r in entities.RegOrgIncomingFiles
                           where r.ProcesingStart.Value >= start && r.ProcesingStart.Value <= end && r.stream_id.HasValue
                           select r).ToList();
            }

            if (regOrgs == null || regOrgs.Count == 0)
                return false;

            foreach (var file in files)
                if (!(regOrgs.Any(r => r.Name == Path.GetFileName(file))))
                    return false;

            return true;
        }

        private List<RegOrgIncomingFile> GetRegOrgsForProcessing()
        {
            List<RegOrgIncomingFile> regOrgs = null;

            using (var entities = new ComplaintEntities())
            {
                regOrgs = entities.RegOrgIncomingFiles.Where(r => 
                    r.ErrorFlag == false && r.Status == 2 &&
                    r.OrganizationId == "MC" && r.stream_id.HasValue && 
                    r.Name.EndsWith(".ipm"))
                    .ToList();
            }

            return regOrgs;
        }

        private bool ValidateProcessing(List<RegOrgIncomingFile> regOrgs)
        {
            var fileIds = regOrgs.Select(r => r.FileId).ToList();
            List<RegOrgIncomingFile> newRegOrgs = null;

            using (var entities = new ComplaintEntities())
            {
                newRegOrgs = (from r in entities.RegOrgIncomingFiles
                              where fileIds.Contains(r.FileId)
                              select r).ToList();
            }

            if (newRegOrgs.Count != regOrgs.Count)
                return false;

            foreach (var regOrg in newRegOrgs)
                if (!(regOrg.ParsingStarted.HasValue &&
                    regOrg.ParsingFinished.HasValue &&
                    regOrg.ProcesingStart.HasValue &&
                    regOrg.Status == 3 &&
                    !(regOrg.ErrorFlag)))
                    return false;

            return true;
        }

        #endregion
    }
}
