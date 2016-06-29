using ComplaintTool.Shell.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Shell.Tests
{
    [TestClass]
    public class Processing_Test
    {
        [TestMethod]
        public void Set3DSecureValidationTest()
        {
            var process = new Set3DSecureValidation();
            process.IsWriteMode = false;
            process.Process();
        }

        [TestMethod]
        public void SetMasterCard3DSecureStatusTest()
        {
            var process = new SetMasterCard3DSecureStatus() { CaseIdParam="ESPLCB20160129400014"};
            process.IsWriteMode = false;
            process.Process();
        }

        [TestMethod]
        public void SetAutoRepresentmentValidationTest()
        {
            var process = new SetAutoRepresentmentValidation();
            process.IsWriteMode = false;
            process.Process();
        }

        [TestMethod]
        public void SetCLFValidationTest()
        {
            var process = new SetClfValidation();
            process.IsWriteMode = false;
            process.Process();
        }

        [TestMethod]
        public void SetCurrencyCodeTest()
        {
            var process = new SetCurrencyCode();
            process.IsWriteMode = false;
            process.Process();
        }

        [TestMethod]
        public void SetPostilionData()
        {
            var process = new SetPostilionData() { CaseId = "ESPLCB20160104400011" };
            process.IsWriteMode = false;
            process.Process();
            
            /*using(var unitOfWork=ComplaintTool.DataAccess.ComplaintUnitOfWork.Create())
            {
                var complaints=unitOfWork.Repo<ComplaintTool.DataAccess.Repos.ComplaintRepo>().FindWithoutPostilionData();
                var provider = new ComplaintTool.Processing.Postilion.PostilionDataProvider();
                foreach(var comp in complaints)
                {
                    try
                    {
                        var data = provider.GetData(comp);
                        if (data != null)
                            return;
                    }
                    catch { };
                }
            }
            */
        }

        [TestMethod]
        public void SetSupportingDocumentVerificationTest()
        {
            var process = new SetSupportingDocumentVerification();
            process.IsWriteMode = false;
            process.Process();
        }

        [TestMethod]
        public void SetTemporaryValidationTest()
        {
            var process = new SetTemporaryValidation();
            process.IsWriteMode = false;
            process.Process();
        }

        [TestMethod]
        public void SwitchCasesToCloseTest()
        {
            var process=new SwitchCasesToClose();
            process.IsWriteMode = false;
            process.Process();
        }
    }
}
