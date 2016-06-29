using System;
using System.Xml.Serialization;

namespace ComplaintTool.MCProImageInterface.Model
{
    [Serializable, XmlRoot("vma")]
    public class Vma
    {
        [XmlElement(Order = 1, ElementName = "mti")]
        public string Mti { get; set; }

        [XmlElement(Order = 2, ElementName = "d002_primary_account_num")]
        public string Pan { get; set; }

        [XmlElement(Order = 3, ElementName = "d003_processing_code")]
        public ProcessingCode ProcessingCode { get; set; }

        [XmlElement(Order = 4, ElementName = "d012_date_time_trans")]
        public ProcessingCode DateAndTimeLocalTransaction { get; set; }

        //optional
        [XmlElement(Order = 5, ElementName = "d022_point_of_service_data_code")]
        public PointOfServiceDataCode PoS { get; set; }

        //optional
        [XmlElement(Order = 6, ElementName = "d024_function_code")]
        public string FunctionCode { get; set; }

        [XmlElement(Order = 7, ElementName = "d025_msg_reason_code")]
        public string MessageReasonCode { get; set; }

        [XmlElement(Order = 8, ElementName = "d030_amounts_original")]
        public AmountsOriginal AmountsOriginal { get; set; }

        [XmlElement(Order = 9, ElementName = "d031_acquirer_ref_data")]
        public AcquirerReferenceData ARD { get; set; }

        //optional
        [XmlElement(Order = 10, ElementName = "d032_acq_inst_id")]
        public string AcquiringInstitutionIDCode { get; set; }

        [XmlElement(Order = 11, ElementName = "d033_forwarding_inst_id")]
        public string ForwardingInstitutionIDCode { get; set; }

        //optional
        [XmlElement(Order = 12, ElementName = "d037_retrieval_ref_num")]
        public string RetrievalReferenceNumber { get; set; }

        //optional
        [XmlElement(Order = 13, ElementName = "d048_additional_data")]
        public AdditionalData AdditionalData { get; set; }

        //optional
        [XmlElement(Order = 14, ElementName = "d071_msg_num")]
        public string MessageNumber { get; set; }

        [XmlElement(Order = 15, ElementName = "d093_trans_dest_inst_id")]
        public string TransactionDestinationInstitutionIDCode { get; set; }

        [XmlElement(Order = 16, ElementName = "d094_trans_orig_inst_id")]
        public string TransactionOriginatorInstitutionIDCode { get; set; }

        //optional
        [XmlElement(Order = 17, ElementName = "d095_card_issuer_ref_data")]
        public string CardIssuerReferenceData { get; set; }

        [XmlElement(Order = 18, ElementName = "d100_receiving_inst_id")]
        public string ReceivingInstitutionIDCode { get; set; }
    }
}
