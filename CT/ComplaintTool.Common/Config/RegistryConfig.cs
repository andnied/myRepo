using System.Text;
namespace ComplaintTool.Common.Config
{
    public class RegistryConfig : IConfig
    {
        private const int DefaultConnectionTimeout = 180;
        private int? _connectionTimeout;

        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public bool IntegratedSecurity { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public int ConnectionTimeout 
        {
            get { return _connectionTimeout ?? DefaultConnectionTimeout; }
            set { _connectionTimeout = value; }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendFormat("ServerName={0};\n", ServerName);
            builder.AppendFormat("DatabaseName={0};\n", DatabaseName);
            builder.AppendFormat("IntegratedSecurity={0};\n", IntegratedSecurity);
            builder.AppendFormat("UserID={0};\n", UserID);
            builder.AppendFormat("Password={0};\n", Password);
            builder.AppendFormat("ConnectionTimeout={0};\n", ConnectionTimeout);
            return builder.ToString();
        }
    }
}
