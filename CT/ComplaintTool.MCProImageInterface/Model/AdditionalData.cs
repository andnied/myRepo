using System.Xml.Serialization;

namespace ComplaintTool.MCProImageInterface.Model
{
    [XmlType]
    public class AdditionalData
    {
        /// <summary>
        /// SCU
        /// ipm.clearing_rtrvl.p0001_virtual_account_num
        /// </summary>
        [XmlElement(Order = 0, ElementName = "pds0001_paypass_map_svc_acct_num")]
        public string P0001PayPassMappingServiceAccountNumber { get; set; }

        [XmlElement(Order = 1, ElementName = "pds001_paypass_map_svc_acct_num")]
        public PayPassMappingServiceAccountNumber PayPassMappingServiceAccountNumber { get; set; }

        [XmlElement(Order = 2, ElementName = "pds023_terminal_type")]
        public string TerminalType { get; set; }

        [XmlElement(Order = 3, ElementName = "pds025_msg_reversal_ind")]
        public MessageReversalIndicator MessageReversalIndicator { get; set; }

        [XmlElement(Order = 4, ElementName = "pds148_currency_exponents")]
        public CurrencyExponents CurrencyExponents { get; set; }

        [XmlElement(Order = 5, ElementName = "pds149_curr_codes_amounts_orig")]
        public CurrencyCodesAmountsOriginal CurrencyCodesAmountsOriginal { get; set; }

        [XmlElement(Order = 6, ElementName = "pds165_settlement_indicator")]
        public SettlementIndicator SettlementIndicator { get; set; }
        
        [XmlElement(Order = 7, ElementName = "pds191_orig_msg_format")]
        public string OriginatingMessageFormat { get; set; }

        //optional
        [XmlElement(Order = 8, ElementName = "pds213_cleansed_merchant_name")]
        public CleansedMerchantData CleansedMerchantData { get; set; }

        [XmlElement(Order = 9, ElementName = "pds228_retrieval_doc_cd")]
        public string RetrievalDocumentCode { get; set; }

        [XmlElement(Order = 10, ElementName = "pds241_mcom_control_num")]
        public string MasterComControlNumber { get; set; }

        [XmlElement(Order = 11, ElementName = "pds242_mcom_svc_ind")]
        public string MasterComServiceIndicator { get; set; }

        //optional
        [XmlElement(Order = 12, ElementName = "pds243_mcom_rr_data")]
        public MasterComRetrievalResponseData MasterComRetrievalResponseData { get; set; }

        //optional
        [XmlElement(Order = 13, ElementName = "pds244_mastercom_chargeback_support_doc_dates")]
        public MasterComChargebackSupportDocDates MasterComChargebackSupportDocDates { get; set; }

        //optional
        [XmlElement(Order = 14, ElementName = "pds245_chargeback_sender_process_date")]
        public string MastercomArbitrationChargebackSenderProcessingDate { get; set; }

        [XmlElement(Order = 15, ElementName = "pds246_mcom_sender_memo")]
        public string MasterComSenderMemo { get; set; }

        [XmlElement(Order = 16, ElementName = "pds247_mcom_receiver_memo")]
        public string MasterComImageReceiverMemo { get; set; }

        //optional
        [XmlElement(Order = 17, ElementName = "pds249_mcom_database_id")]
        public string MasterComDatabaseID { get; set; }

        //optional
        [XmlElement(Order = 18, ElementName = "pds250_mastercom_endpoints")]
        public MasterComEndpoints MasterComEndpoints { get; set; }

        //optional
        // mistake in specification
        [XmlElement(Order = 19, ElementName = "pds251_mcom_fulfillment_document_code")]
        public string MasterComFulfillmentDocumentCode { get; set; }

        //optional
        [XmlElement(Order = 20, ElementName = "pds252_image_meta_data")]
        public MasterComImageMetadata MasterComImageMetadata { get; set; }
        
        //optional
        [XmlElement(Order = 21, ElementName = "pds248_mcom_image_review_memo")]
        public string MasterComImageReviewMemo { get; set; }

        [XmlElement(Order = 22, ElementName = "pds262_document_indicator")]
        public string DocumentIndicator { get; set; }
    }
}
