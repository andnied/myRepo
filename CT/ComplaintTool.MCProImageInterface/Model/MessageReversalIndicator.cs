using System.Xml.Serialization;

namespace ComplaintTool.MCProImageInterface.Model
{
    [XmlType]
    public class MessageReversalIndicator
    {
        [XmlElement(Order = 1, ElementName = "s1")]
        public string S1MessageReversalIndicator { get; set; }

        [XmlElement(Order = 2, ElementName = "s2")]
        public string S2CentralSiteProcessingDateOfOriginalMessage { get; set; }

        public override string ToString()
        {
            return string.Format("{0}{1}", S1MessageReversalIndicator, S2CentralSiteProcessingDateOfOriginalMessage);
        }
    }
}
