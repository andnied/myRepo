using ComplaintTool.Common.Utils;
using ComplaintTool.Shell.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.Utils
{
    [Cmdlet(VerbsCommon.Get, "EncryptPassword")]
    public class GetEncryptPassword : ComplaintCmdletBase
    {
        private readonly ILogger Logger = LogManager.GetLogger();
        private string _password;

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Encrypt string password.")]
        [Alias("passwd")]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public override void Process()
        {
            try
            {
                var securePassword = Encryption.Encrypt(Password);
                WriteObject(securePassword);
            }
            catch (Exception ex)
            {
                Logger.LogComplaintException(ex);
            }
        }
    }
}
