using System;
using System.Collections.Generic;
using System.Linq;
using ComplaintTool.Common.Config;
using ComplaintTool.Common.Utils;
using ComplaintTool.Models;
using ComplaintTool.Shell.Administration;
using ComplaintTool.Shell.Extract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComplaintTool.Shell.Tests
{
    [TestClass]
    public class Extract_Tests
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
        public void NewMCBulkFile_Tracy_Process()
        {
            var cmd = new NewMCBulkFile
            {
                IsWriteMode = false,
                DestinationFolder = TestHelper.GetClassDestinationFolder(),
                EndpointName = TestHelper.Tracy
            };
            cmd.Process();
        }

        [TestMethod]
        public void NewMCBulkFile_Radiant_Process()
        {
            var cmd = new NewMCBulkFile
            {
                IsWriteMode = false,
                DestinationFolder = TestHelper.GetClassDestinationFolder(),
                EndpointName = TestHelper.Radiant
            };
            cmd.Process();
        }

        [TestMethod]
        public void GetMCBulkFile_Process()
        {
            foreach (var processKey in GetProcessKeys())
            {
                var cmd = new GetMCBulkFile()
                {
                    IsWriteMode = false,
                    DestinationFolder = TestHelper.GetClassDestinationFolder(),
                    ProcessKey = processKey.ToString(),
                };
                cmd.Process();
            }
        }

        [TestMethod]
        public void NewConfigFile_Process()
        {
            if (ComplaintConfig.Instance.Conf == null)
            {
                var cmd = new NewConfigFile
                {
                    ServerName = TestHelper.TestDatabaseServer,
                    DatabaseName = TestHelper.TestDatabaseName,
                    IntegratedSecurity = TestHelper.TestDatabaseIntegratedSecurity
                };
                cmd.Process();
            }
        }

        public static IEnumerable<Guid> GetProcessKeys()
        {
            using (var ctx = new ComplaintEntities(TestHelper.GetConnectionString()))
            {
                return ctx.ProcessKeys.Select(x => x.ProcessKey1).ToList();
            }
        }
    }
}
