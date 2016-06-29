using System.Xml.Serialization;

namespace ComplaintTool.MCProImageInterface.Model
{
    [XmlType]
    public class MasterComRetrievalResponseData
    {
        [XmlElement(Order = 1, ElementName = "s1")]
        public string S1MasterComIssuerRetrievalRequestDate { get; set; }

        [XmlElement(Order = 2, ElementName = "s2")]
        public string S2MasterComAcquirerRetrievalResponseCode { get; set; }

        [XmlElement(Order = 3, ElementName = "s3")]
        public string S3MasterComAcquirerRetrievalResponseSentDate { get; set; }

        [XmlElement(Order = 4, ElementName = "s4")]
        public string S4MasterComIssuerResponseCode { get; set; }

        [XmlElement(Order = 5, ElementName = "s5")]
        public string S5MasterComIssuerResponseDate { get; set; }

        [XmlElement(Order = 6, ElementName = "s6")]
        public string S6MasterComIssuerRejectReasons { get; set; }

        [XmlElement(Order = 7, ElementName = "s7")]
        public string S7MasterComImageReviewDecision { get; set; }

        [XmlElement(Order = 8, ElementName = "s8")]
        public string S8MasterComImageReviewDate { get; set; }
    }
}
