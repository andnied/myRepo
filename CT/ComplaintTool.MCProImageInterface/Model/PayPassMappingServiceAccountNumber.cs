using System.Xml.Serialization;

namespace ComplaintTool.MCProImageInterface.Model
{
    [XmlType]
    public class PayPassMappingServiceAccountNumber
    {
        [XmlElement(Order = 1, ElementName = "s1")]
        public string S1AccountNumberType { get; set; }

        [XmlElement(Order = 2, ElementName = "s2")]
        public string S2AccountNumber { get; set; }
    }
}
