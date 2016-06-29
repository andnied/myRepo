using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ComplaintTool.MCProImageInterface.Model
{
    [Serializable, XmlRoot("fimn")]
    public class Fimn
    {
        [Required]
        [XmlElement(Order = 1, ElementName = "case_id")]
        public string CaseID { get; set; }

        [Required]
        [XmlElement(Order = 2, ElementName = "filing_ica")]
        public string FilingICA { get; set; }

        [Required]
        [XmlElement(Order = 3, ElementName = "filed_against_ica")]
        public string FiledAgainstICA { get; set; }

        [Required]
        [XmlElement(Order = 4, ElementName = "case_type")]
        public string CaseType { get; set; }

        [Required]
        [XmlElement(Order = 5, ElementName = "cardholder_num")]
        public string CardholderNumber { get; set; }

        [Required]
        [XmlElement(Order = 6, ElementName = "virtual_account_num")]
        public string VirtualAccountNumber { get; set; }

        [Required]
        [XmlElement(Order = 7, ElementName = "arn")]
        public string ARN { get; set; }

        [Required]
        [XmlElement(Order = 8, ElementName = "cb_ref_num")]
        public string ChargebackReferenceNumber { get; set; }

        [Required]
        [XmlElement(Order = 9, ElementName = "members_file_num")]
        public string MembersFileNumber { get; set; }

        [Required]
        [XmlElement(Order = 10, ElementName = "merchant_name")]
        public string MerchantName { get; set; }

        [Required]
        [XmlElement(Order = 11, ElementName = "dispute_amount")]
        public string DisputeAmount { get; set; }

        [Required]
        [XmlElement(Order = 12, ElementName = "dispute_amount_curr_cd")]
        public string DisputeAmountCurrencyCode { get; set; }

        [Required]
        [XmlElement(Order = 13, ElementName = "resp_due_dttm")]
        public string ResponseDueDateTime { get; set; }

        [Required]
        [XmlElement(Order = 14, ElementName = "file_as")]
        public string FileAs { get; set; }

        [Required]
        [XmlElement(Order = 15, ElementName = "case_state")]
        public string CaseState { get; set; }

        [Required]
        [XmlElement(Order = 16, ElementName = "image_length")]
        public string ImageLength { get; set; }

        [Required]
        [XmlElement(Order = 17, ElementName = "submitted_dttm")]
        public string SubmittedDateTime { get; set; }

        [Required]
        [XmlElement(Order = 18, ElementName = "submitted_by")]
        public string SubmittedBy { get; set; }

        [Required]
        [XmlElement(Order = 19, ElementName = "total_pages_num")]
        public string TotalPagesNumber { get; set; }

        [Required]
        [XmlElement(Order = 20, ElementName = "chargeback_reason_code")]
        public string ChargebackReasonCode { get; set; }

        [Required]
        [XmlElement(Order = 21, ElementName = "violation_code")]
        public string ViolationCode { get; set; }

        [Required]
        [XmlElement(Order = 22, ElementName = "violation_date")]
        public string ViolationDate { get; set; }

        [Required]
        [XmlElement(Order = 23, ElementName = "filing_region")]
        public string FilingRegion { get; set; }

        [Required]
        [XmlElement(Order = 24, ElementName = "filing_against_region")]
        public string FilingAgainstRegion { get; set; }

        [Required]
        [XmlElement(Order = 25, ElementName = "clnsd_merch_name")]
        public string CleansedMerchantName { get; set; }

        [Required]
        [XmlElement(Order = 26, ElementName = "clnsd_merch_ph_no")]
        public string CleansedMerchantPhoneNumber { get; set; }

        [Required]
        [XmlElement(Order = 27, ElementName = "billing_currency_cd")]
        public string BillingCurrencyCode { get; set; }

        [Required]
        [XmlElement(Order = 28, ElementName = "d022_point_of_service_data_code")]
        public PointOfServiceDataCode PointOfServiceDataCode { get; set; }
    }
}
