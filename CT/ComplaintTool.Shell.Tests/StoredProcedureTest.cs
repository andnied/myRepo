using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Repos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComplaintTool.Shell.Tests
{
    [TestClass]
    public class StoredProcedureTest
    {
        [TestMethod]
        public void GetPanByCaseIdTest()
        {
            using (var unitOfWork = ComplaintUnitOfWork.Create())
            {
                string pan = null, panExtention;
                unitOfWork.Repo<ComplaintRepo>().GetPanByCaseId("ESPLCB20160225100163", out panExtention, out panExtention);
                Assert.IsNotNull(pan);
            }
        }
    }
}
