using ComplaintTool.Shell.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common;
using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.Common.CTLogger;
using ComplaintTool.MasterCard.Incoming;
using ComplaintTool.Models;
using ComplaintTool.Visa;

namespace ComplaintTool.Shell.Utils
{
    [Cmdlet(VerbsCommon.New, "ResumeIncomingByFileName")]
    public class NewResumeIncomingByFileName : ComplaintCmdletBase
    {
        private static readonly ILogger Logger = LogManager.GetLogger();

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Name of resumed incoming file.")]
        [Alias("File")]
        public string FileName { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "ARN.")]
        public string Arn { get; set; }
        
        public override void Process()
        {
            try
            {
                List<RegOrgIncomingFile> incomings = null;

                using (var unitOfWork = ComplaintUnitOfWork.Create())
                {
                    incomings = unitOfWork.Repo<RegOrgIncomingFilesRepo>().FindRegOrgsByFileName(FileName).ToList();
                }

                int count = incomings.Count;

                if (count == 0)
                    throw new Exception("Incoming with provided file name does not exist.");

                if (count > 1)
                    throw new Exception("Ambiguous file name provided (more than 1 row found). Only resuming file parser by file id possible.");

                var org = incomings[0].OrganizationId;
                var fileId = incomings[0].FileId;
                int result = 0;

                if (org == "MC")
                {
                    var process = new MasterCardFileProcessing(fileId, @"C:\Users\Default\AppData\Local\Temp", Arn);
                    result = process.Process();
                }
                else
                {
                    var process = VisaIncomingBase.GetService("processing", fileId: fileId, arn: Arn);
                    result = process.Process();
                }

                WriteObject(result);
            }
            catch (Exception ex)
            {
                Logger.LogComplaintException(ex);
                WriteObject(-1);
            }
        }
    }
}
