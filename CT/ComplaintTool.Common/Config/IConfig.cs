namespace ComplaintTool.Common.Config
{
    public interface IConfig
    {
        string ServerName { get; set; }
        string DatabaseName { get; set; }
        bool IntegratedSecurity { get; set; }
        string UserID { get; set; }
        string Password { get; set; }
        int ConnectionTimeout { get; set; }
    }
}
