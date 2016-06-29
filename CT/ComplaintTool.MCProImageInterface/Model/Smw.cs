using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ComplaintTool.MCProImageInterface.Model
{
    [Serializable, XmlRoot("smw")]
    public class Smw
    {
        [XmlElement(Order = 1, ElementName = "mti")]
        public string Mti { get; set; }

        [XmlElement(Order = 2, ElementName = "d002_primary_account_num")]
        public string Pan { get; set; }

        [XmlElement(Order = 3, ElementName = "d024_function_code")]
        public string FunctionCode { get; set; }

        [XmlElement(Order = 4, ElementName = "d031_acquirer_ref_data")]
        public AcquirerReferenceData ARD { get; set; }

        [XmlElement(Order = 5, ElementName = "d048_additional_data")]
        public AdditionalData AdditionalData { get; set; }

        [XmlElement(Order = 6, ElementName = "d095_card_issr_ref_data")]
        public string CardIssuerReferenceData { get; set; }
    }
}
