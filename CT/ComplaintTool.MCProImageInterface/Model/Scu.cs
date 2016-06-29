using System;
using System.Xml.Serialization;

namespace ComplaintTool.MCProImageInterface.Model
{
    [Serializable, XmlRoot("scu")]
    public class Scu
    {

        [XmlElement(Order = 1, ElementName = "mti")]
        public string Mti { get; set; }

        [XmlElement(Order = 2, ElementName = "d002_primary_account_num")]
        public string Pan { get; set; }

        [XmlElement(Order = 3, ElementName = "d022_point_of_service_data_code")]
        public PointOfServiceDataCode PoS { get; set; }

        [XmlElement(Order = 4, ElementName = "d024_function_code")]
        public string FunctionCode { get; set; }

        [XmlElement(Order = 5, ElementName = "d031_acquirer_ref_data")]
        public AcquirerReferenceData ARD { get; set; }

        [XmlElement(Order = 6, ElementName = "d048_additional_data")]
        public AdditionalData AdditionalData { get; set; }

        [XmlElement(Order = 7, ElementName = "d095_card_issr_ref_data")]
        public string CardIssuerReferenceData { get; set; }
    }
}
