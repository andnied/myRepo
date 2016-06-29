using System;
using System.IO;
using System.Management.Automation;
using ComplaintTool.Common;
using ComplaintTool.Common.Config;
using ComplaintTool.Common.Utils;
using ComplaintTool.Shell.Common;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.Administration
{
    [Cmdlet(VerbsCommon.New, "ConfigFile")]
    public class NewConfigFile : ComplaintCmdletBase
    {
        private const string Msg = "Parameter {0} error : {1}";
        private const string ParamNotEmptyMsg = "parameter cannot be empty";
        private static readonly ILogger Logger = LogManager.GetLogger();

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0)]
        [Alias("sname")]
        public string ServerName { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 1)]
        [Alias("dbname")]
        public string DatabaseName { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 2)]
        [Alias("is")]
        public bool IntegratedSecurity { get; set; }

        private string _userId = string.Empty;
        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 3)]
        [Alias("user")]
        public string UserId { get { return _userId; } set { _userId = value; } }

        private string _password = string.Empty;
        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 4)]
        [Alias("passwd")]
        public string Password { get { return _password; } set { _password = value; } }

        public override void Process()
        {
            try
            {
                if (!IntegratedSecurity)
                {
                    if (string.IsNullOrWhiteSpace(UserId))
                        throw new ArgumentException(string.Format(Msg, "UserID", ParamNotEmptyMsg));
                    if (string.IsNullOrWhiteSpace(Password))
                        throw new ArgumentException(string.Format(Msg, "Password", ParamNotEmptyMsg));
                }

                IConfig conf = new RegistryConfig();
                conf.ServerName = ServerName;
                conf.DatabaseName = DatabaseName;
                conf.IntegratedSecurity = IntegratedSecurity;
                if (!conf.IntegratedSecurity)
                {
                    conf.UserID = UserId;
                    conf.Password = Encryption.Encrypt(Password);
                }
                ComplaintConfig.SetConfig(conf);
                WriteComplaintObject("Config created successfully");
            }
            catch (Exception ex)
            {
                Logger.LogComplaintException(ex);
            }
        }
    }
}
