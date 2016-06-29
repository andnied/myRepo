using System;
using System.Xml.Serialization;

namespace ComplaintTool.MCProImageInterface.Model
{
    [Serializable, XmlRoot("rmw")]
    public class Rmw
    {
        [XmlElement(Order = 1, ElementName = "mti")]
        public string Mti { get; set; }

        [XmlElement(Order = 2, ElementName = "d002_primary_account_num")]
        public string Pan { get; set; }

        [XmlElement(Order = 3, ElementName = "d003_processing_code")]
        public ProcessingCode ProcessingCode { get; set; }

        [XmlElement(Order = 4, ElementName = "d004_transaction_amount")]
        public string AmountTransaction { get; set; }

        //optional
        [XmlElement(Order = 5, ElementName = "d012_date_time_trans")]
        public DateAndTimeLocalTransaction DateAndTimeLocalTransaction { get; set; }

        //optional
        [XmlElement(Order = 6, ElementName = "d022_point_of_service_data_code")]
        public PointOfServiceDataCode PoS { get; set; }

        //optional
        [XmlElement(Order = 7, ElementName = "d024_function_code")]
        public string FunctionCode { get; set; }

        [XmlElement(Order = 8, ElementName = "d025_msg_reason_code")]
        public string MessageReasonCode { get; set; }

        //optional
        [XmlElement(Order = 9, ElementName = "d026_card_acceptor_business_code_mcc")]
        public string CardAcceptorBusinessCodeMCC { get; set; }

        [XmlElement(Order = 10, ElementName = "d030_amounts_original")]
        public AmountsOriginal AmountsOriginal { get; set; }

        [XmlElement(Order = 11, ElementName = "d031_acquirer_ref_data")]
        public AcquirerReferenceData ARD { get; set; }

        [XmlElement(Order = 12, ElementName = "d033_forwarding_inst_id")]
        public string ForwardingInstitutionIDCode { get; set; }

        //optional
        [XmlElement(Order = 13, ElementName = "d038_approval_code")]
        public string ApprovalCode { get; set; }

        //optional
        [XmlElement(Order = 14, ElementName = "d041_card_acceptor_terminal_id")]
        public string CardAcceptorTerminalID { get; set; }

        //optional
        [XmlElement(Order = 15, ElementName = "d042_card_acceptor_code")]
        public string CardAcceptorCode { get; set; }

        //optional
        [XmlElement(Order = 16, ElementName = "d043_card_acceptor_id_code")]
        public CardAcceptorIDCode CardAcceptorIDCode { get; set; }

        //optional
        [XmlElement(Order = 17, ElementName = "d048_additional_data")]
        public AdditionalData AdditionalData { get; set; }

        [XmlElement(Order = 18, ElementName = "d049_currency_code_transaction")]
        public string CurrencyCodeTransaction { get; set; }

        //optional
        [XmlElement(Order = 19, ElementName = "d071_msg_num")]
        public string MessageNumber { get; set; }

        //optional
        [XmlElement(Order = 20, ElementName = "d072_data_record")]
        public string DataRecord { get; set; }

        [XmlElement(Order = 21, ElementName = "d093_trans_dest_inst_id")]
        public string TransactionDestinationInstitutionIDCode { get; set; }

        [XmlElement(Order = 22, ElementName = "d094_trans_orig_inst_id")]
        public string TransactionOriginatorInstitutionIDCode { get; set; }

        [XmlElement(Order = 23, ElementName = "d095_card_issr_ref_data")]
        public string CardIssuerReferenceData { get; set; }

        [XmlElement(Order = 24, ElementName = "d100_receiving_inst_id")]
        public string ReceivingInstitutionIDCode { get; set; }

    }
}
