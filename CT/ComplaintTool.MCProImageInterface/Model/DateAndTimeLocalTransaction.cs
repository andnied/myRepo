using System.Xml.Serialization;

namespace ComplaintTool.MCProImageInterface.Model
{
    [XmlType]
    public class DateAndTimeLocalTransaction
    {
        [XmlElement(Order = 1, ElementName = "s1")]
        public string S1Date { get; set; }

        [XmlElement(Order = 2, ElementName = "s2")]
        public string S2Time { get; set; }

        public override string ToString()
        {
            return (string.IsNullOrEmpty(S1Date) || string.IsNullOrEmpty(S2Time)) ? null : string.Format("{0}{1}", S1Date, S2Time);
        }
    }
}
