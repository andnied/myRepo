using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace ComplaintTool.Common.Config
{
    [Serializable, XmlRoot("XMLConfig")]
    public class XmlConfig : IConfig
    {
        [XmlElement(Order = 1, ElementName = "ServerName")]
        public string ServerName { get; set; }

        [XmlElement(Order = 2, ElementName = "DatabaseName")]
        public string DatabaseName { get; set; }

        [XmlElement(Order = 3, ElementName = "IntegratedSecurity")]
        public bool IntegratedSecurity { get; set; }

        [XmlElement(Order = 4, ElementName = "UserID")]
        public string UserID { get; set; }

        [XmlElement(Order = 5, ElementName = "Password")]
        public string Password { get; set; }

        [DefaultValue(180), XmlElement(Order = 6, ElementName = "ConnectionTimeout")]
        public int ConnectionTimeout { get; set; }
    }
}
