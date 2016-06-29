using System.Xml.Serialization;

namespace ComplaintTool.MCProImageInterface.Model
{
    [XmlType]
    public class CleansedMerchantData
    {
        [XmlElement(Order = 1, ElementName = "s1")]
        public string S1CardAcceptorName { get; set; }

        [XmlElement(Order = 2, ElementName = "s2")]
        public string S2CardAcceptorStreetAddress { get; set; }

        [XmlElement(Order = 3, ElementName = "s3")]
        public string S3CardAcceptorCity { get; set; }

        [XmlElement(Order = 4, ElementName = "s4")]
        public string S4CardAcceptorPostalZIPCode { get; set; }

        [XmlElement(Order = 5, ElementName = "s5")]
        public string S5CardAcceptorStateProvinceOrRegionCode { get; set; }

        [XmlElement(Order = 6, ElementName = "s6")]
        public string S6CardAcceptorCountryCode { get; set; }

        [XmlElement(Order = 7, ElementName = "s7")]
        public string S7CardAcceptorPhoneNumber { get; set; }
    }
}
