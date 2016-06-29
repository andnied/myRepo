using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Visa;
using ComplaintTool.Shell.Common;
using ComplaintTool.Common;
using System.Runtime.CompilerServices;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.VISA
{
    [Cmdlet(VerbsCommon.New, "VisaIncomingRegistration")]
    public class NewVisaIncomingRegistration : ComplaintCmdletBase
    {
        private static readonly ILogger Logger = LogManager.GetLogger();

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Path to the incoming file.")]
        [Alias("File")]
        public string FilePath { get; set; }
        
        public override void Process()
        {
            try
            {
                var incomingFile = VisaIncomingBase.GetService("registration", FilePath);
                var result = incomingFile.Process();

                if (IsWriteMode)
                    WriteObject(result);
            }
            catch (Exception ex)
            {
                Logger.LogComplaintException(ex);
                
                if (IsWriteMode)
                    WriteObject(-1);
            }
        }
    }
}
