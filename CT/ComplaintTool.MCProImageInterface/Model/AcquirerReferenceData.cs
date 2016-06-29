using System;
using System.Xml.Serialization;

namespace ComplaintTool.MCProImageInterface.Model
{
    [XmlType]
    public class AcquirerReferenceData
    {
        [XmlElement(Order = 1, ElementName = "s1")]
        public string S1MixedUse { get; set; }

        [XmlElement(Order = 2, ElementName = "s2")]
        public string S2AcquirersBIN { get; set; }

        [XmlElement(Order = 3, ElementName = "s3")]
        public string S3JulianProcessingDate { get; set; }

        [XmlElement(Order = 4, ElementName = "s4")]
        public string S4AcquirersSequenceNumber { get; set; }

        [XmlElement(Order = 5, ElementName = "s5")]
        public string S5CheckDigitNumeric { get; set; }

        public override string ToString()
        {
            return String.Format("{0}{1}{2}{3}{4}", S1MixedUse, S2AcquirersBIN, S3JulianProcessingDate, S4AcquirersSequenceNumber, S5CheckDigitNumeric);
        }
    }
}
