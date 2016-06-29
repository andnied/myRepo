using System.Xml.Serialization;

namespace ComplaintTool.MCProImageInterface.Model
{
    [XmlType]
    public class SettlementIndicator
    {
        [XmlElement(Order = 1, ElementName = "s1")]
        public string S1SettlementIndicator { get; set; }

        [XmlElement(Order = 2, ElementName = "s2")]
        public string S2SettlementAgreementInformation { get; set; }
    }
}
