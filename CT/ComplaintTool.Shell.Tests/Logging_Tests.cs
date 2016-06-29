using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ComplaintTool.Common;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.Tests
{
    [TestClass]
    public class Logging_Tests
    {
        private static readonly ILogger _logger = LogManager.GetLogger();

        [TestMethod]
        public void Logger_Test()
        {
            _logger.LogComplaintEvent(560, "This is logging test");
            try
            {
                ThrowEx();
            }
            catch (Exception ex)
            {
                _logger.LogComplaintException(ex);
            }
        }

        public void ThrowEx()
        {
            System.IO.File.ReadAllLines(@"c:\test.test");
        }
    }
}
