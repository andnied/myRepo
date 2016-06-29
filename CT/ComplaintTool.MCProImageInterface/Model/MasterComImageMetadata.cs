using System.Xml.Serialization;

namespace ComplaintTool.MCProImageInterface.Model
{
    [XmlType]
    public class MasterComImageMetadata
    {
        [XmlElement(Order = 1, ElementName = "s1")]
        public string S1MasterComTIFFImageDataLengthHex { get; set; }

        [XmlElement(Order = 2, ElementName = "s2")]
        public string S2MasterComTIFFImageDataOffsetHex { get; set; }

        [XmlElement(Order = 3, ElementName = "s3")]
        public string S3MasterComTIFFImageCRC32 { get; set; }

        [XmlElement(Order = 4, ElementName = "s4")]
        public string S4MasterComTIFFImageFilename { get; set; }
    }
}
