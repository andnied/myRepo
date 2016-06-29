using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ComplaintTool.MCProImageInterface.Model
{
    [Serializable, XmlRoot("famn")]
    public class Famn
    {
        [XmlElement(Order = 1, ElementName = "case_id")]
        public string CaseID { get; set; }

        [XmlElement(Order = 2, ElementName = "filing_ica")]
        public string FilingICA { get; set; }

        [XmlElement(Order = 3, ElementName = "filed_against_ica")]
        public string FiledAgainstICA { get; set; }

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

        [XmlElement(Order = 13, ElementName = "sender_settlement_amt")]
        public string SenderSettlementAmount { get; set; }

        [XmlElement(Order = 14, ElementName = "sender_settlement_amt_curr_cd")]
        public string SenderSettlementAmountCurrencyCode { get; set; }

        [XmlElement(Order = 15, ElementName = "resp_due_dttm")]
        public string ResponseDueDateTime { get; set; }

        [XmlElement(Order = 16, ElementName = "file_as")]
        public string FileAs { get; set; }

        [XmlElement(Order = 17, ElementName = "case_state")]
        public string CaseState { get; set; }

        [XmlElement(Order = 18, ElementName = "filing_response_code")]
        public string FilingResponseCode { get; set; }

        [XmlElement(Order = 19, ElementName = "image_length")]
        public string ImageLength { get; set; }

        [XmlElement(Order = 20, ElementName = "submitted_dttm")]
        public string SubmittedDateTime { get; set; }

        [XmlElement(Order = 21, ElementName = "submitted_by")]
        public string SubmittedBy { get; set; }

        [XmlElement(Order = 22, ElementName = "total_pages_num")]
        public string TotalPagesNumber { get; set; }

        [XmlElement(Order = 23, ElementName = "memo_txt")]
        public string Memo { get; set; }
        // TODO wyjanic te dwa pola + dodac do bazy danych
        [XmlElement(Order = 24, ElementName = "unjust_enrchmt_chbk_dt")]
        public string UnjustEnrchmtChbkDt { get; set; }

        [XmlElement(Order = 25, ElementName = "unjust_enrchmt_cr_dt")]
        public string UnjustEnrchmtCrDt { get; set; }

        [XmlElement(Order = 26, ElementName = "chargeback_reason_code")]
        public string ChargebackReasonCode { get; set; }

        [XmlElement(Order = 27, ElementName = "violation_code")]
        public string ViolationCode { get; set; }

        [XmlElement(Order = 28, ElementName = "violation_date")]
        public string ViolationDate { get; set; }

        [XmlElement(Order = 29, ElementName = "filing_region")]
        public string FilingRegion { get; set; }

        [XmlElement(Order = 30, ElementName = "filing_against_region")]
        public string FilingAgainstRegion { get; set; }
    }
}