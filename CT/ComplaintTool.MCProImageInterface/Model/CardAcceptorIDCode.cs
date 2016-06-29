using System;
using System.Xml.Serialization;

namespace ComplaintTool.MCProImageInterface.Model
{
    [XmlType]
    public class CardAcceptorIDCode
    {
        [XmlElement(Order = 1, ElementName = "s1")]
        public string S1CardAcceptorName { get; set; }

        [XmlElement(Order = 2, ElementName = "s2")]
        public string S2CardAcceptorStreetAddress { get; set; }

        [XmlElement(Order = 3, ElementName = "s3")]
        public string S3CardAcceptorCity { get; set; }

        [XmlElement(Order = 4, ElementName = "s4")]
        public string S4MCardAcceptorPostalZIPCode { get; set; }

        [XmlElement(Order = 5, ElementName = "s5")]
        public string S5CardAcceptorStateProvinceOrRegionCode { get; set; }

        [XmlElement(Order = 6, ElementName = "s6")]
        public string S6CardAcceptorCountryCode { get; set; }

        public override string ToString()
        {
            return String.Format("{0}{1}{2}{3}{4}{5}",
                S1CardAcceptorName,
                S2CardAcceptorStreetAddress,
                S3CardAcceptorCity,
                S4MCardAcceptorPostalZIPCode,
                S5CardAcceptorStateProvinceOrRegionCode,
                S6CardAcceptorCountryCode);
        }
    }
}
