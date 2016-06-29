using System.Xml.Serialization;

namespace ComplaintTool.MCProImageInterface.Model
{
    [XmlType]
    public class ProcessingCode
    {
        [XmlElement(Order = 1, ElementName = "s1")]
        public string S1CardholderTransactionType { get; set; }

        [XmlElement(Order = 2, ElementName = "s2")]
        public string S2CardholderFromAccountType { get; set; }

        [XmlElement(Order = 3, ElementName = "s3")]
        public string S3CardholderToAccountTypeCode { get; set; }
    }
}
