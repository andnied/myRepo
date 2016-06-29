using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ComplaintTool.MCProImageInterface.Model
{
    [Serializable, XmlRoot("famu")]
    public class Famu
    {
        [XmlElement(Order = 1, ElementName = "case_id")]
        public string CaseID { get; set; }

        [XmlElement(Order = 2, ElementName = "filing_ica")]
        public string FilingICA { get; set; }

        [XmlElement(Order = 3, ElementName = "filed_against_ica")]
        public string FilingAgainstICA { get; set; }

        [XmlElement(Order = 4, ElementName = "case_type")]
        public string CaseType { get; set; }

        [XmlElement(Order = 5, ElementName = "cardholder_num")]
        public string CardholderNumber { get; set; }

        [XmlElement(Order = 6, ElementName = "virtual_account_num")]
        public string VirtualAccountNumber { get; set; }

        [XmlElement(Order = 7, ElementName = "arn")]
        public string ARN { get; set; }

        [XmlElement(Order = 8, ElementName = "cb_ref_num")]
        public string ChargebackReferenceNumber { get; set; }

        [XmlElement(Order = 9, ElementName = "members_file_num")]
        public string MembersFileNumber { get; set; }

        [XmlElement(Order = 10, ElementName = "merchant_name")]
        public string MerchantName { get; set; }

        [XmlElement(Order = 11, ElementName = "dispute_amount")]
        public string DisputeAmount { get; set; }

        [XmlElement(Order = 12, ElementName = "dispute_amount_curr_cd")]
        public string DisputeAmountCurrencyCode { get; set; }

        [XmlElement(Order = 13, ElementName = "resp_due_dttm")]
        public string ResponseDueDateTime { get; set; }

        [XmlElement(Order = 14, ElementName = "file_as")]
        public string FileAs { get; set; }

        [XmlElement(Order = 15, ElementName = "image_length")]
        public string ImageLength { get; set; }

        [XmlElement(Order = 16, ElementName = "submitted_dttm")]
        public string SubmittedDateTime { get; set; }

        [XmlElement(Order = 17, ElementName = "submitted_by")]
        public string SubmittedBy { get; set; }

        [XmlElement(Order = 18, ElementName = "total_pages_num")]
        public string TotalPagesNumber { get; set; }

        [XmlElement(Order = 19, ElementName = "rebutted_by")]
        public string RebuttedBy { get; set; }

        [XmlElement(Order = 20, ElementName = "rebutted_dttm")]
        public string RebuttedDateTime { get; set; }

        [XmlElement(Order = 21, ElementName = "rejected_by")]
        public string RejectedBy { get; set; }

        [XmlElement(Order = 22, ElementName = "rejected_dttm")]
        public string RejectedDateTime { get; set; }

        [XmlElement(Order = 23, ElementName = "acpt_user_by")]
        public string AcceptedUserBy { get; set; }

        [XmlElement(Order = 24, ElementName = "acpt_dttm")]
        public string AcceptedDateTime { get; set; }

        [XmlElement(Order = 25, ElementName = "escalated_dttm")]
        public string EscalatedDateTime { get; set; }

        [XmlElement(Order = 26, ElementName = "memo_txt")]
        public string Memo { get; set; }

        [XmlElement(Order = 27, ElementName = "chargeback_reason_code")]
        public string ChargebackReasonCode { get; set; }

        [XmlElement(Order = 28, ElementName = "violation_code")]
        public string ViolationCode { get; set; }

        [XmlElement(Order = 29, ElementName = "violation_date")]
        public string ViolationDate { get; set; }

        [XmlElement(Order = 30, ElementName = "withdrawn_by")]
        public string WithdrawnBy { get; set; }

        [XmlElement(Order = 31, ElementName = "withdrawn_dttm")]
        public string WithdrawnDateTime { get; set; }

        [XmlElement(Order = 32, ElementName = "filing_region")]
        public string FilingRegion { get; set; }

        [XmlElement(Order = 33, ElementName = "filing_against_region")]
        public string FilingAgainstRegion { get; set; }

        [XmlElement(Order = 34, ElementName = "clnsd_merch_name")]
        public string CleansedMerchantName { get; set; }

        [XmlElement(Order = 35, ElementName = "clnsd_merch_ph_no")]
        public string CleansedMerchantPhoneNumber { get; set; }

        [XmlElement(Order = 36, ElementName = "withdrawing_ica")]
        public string WithdrawingICA { get; set; }

        [XmlElement(Order = 37, ElementName = "escalated_by")]
        public string EscalatedBy { get; set; }
    }
}
