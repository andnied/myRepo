using System.Xml.Serialization;

namespace ComplaintTool.MCProImageInterface.Model
{
    [XmlType]
    public class PointOfServiceDataCode
    {
        [XmlElement(Order = 1, ElementName = "s1")]
        public string S1TerminalDataCardDataInputCapability { get; set; }

        [XmlElement(Order = 2, ElementName = "s2")]
        public string S2TerminalDataCardholderAuthenticationCapability { get; set; }

        [XmlElement(Order = 3, ElementName = "s3")]
        public string S3TerminalDataCardCaptureCapability { get; set; }

        [XmlElement(Order = 4, ElementName = "s4")]
        public string S4TerminalOperatingEnvironment { get; set; }

        [XmlElement(Order = 5, ElementName = "s5")]
        public string S5CardholderPresentData { get; set; }

        [XmlElement(Order = 6, ElementName = "s6")]
        public string S6CardPresentData { get; set; }

        [XmlElement(Order = 7, ElementName = "s7")]
        public string S7CardDataInputMode { get; set; }

        [XmlElement(Order = 8, ElementName = "s8")]
        public string S8CardholderAuthenticationMethod { get; set; }

        [XmlElement(Order = 9, ElementName = "s9")]
        public string S9CardholderAuthenticationEntity { get; set; }

        [XmlElement(Order = 10, ElementName = "s10")]
        public string S10CardDataOutputCapability { get; set; }

        [XmlElement(Order = 11, ElementName = "s11")]
        public string S11TerminalDataOutputCapability { get; set; }

        [XmlElement(Order = 12, ElementName = "s12")]
        public string S12PINCaptureCapability { get; set; }
    }
}
