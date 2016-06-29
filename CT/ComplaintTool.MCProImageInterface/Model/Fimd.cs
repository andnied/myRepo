using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ComplaintTool.MCProImageInterface.Model
{
    [Serializable, XmlRoot("fimd")]
    public class Fimd
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

        [XmlElement(Order = 18, ElementName = "image_length")]
        public string ImageLength { get; set; }

        [XmlElement(Order = 19, ElementName = "submitted_dttm")]
        public string SubmittedDateTime { get; set; }

        [XmlElement(Order = 20, ElementName = "submitted_by")]
        public string SubmittedBy { get; set; }

        [XmlElement(Order = 21, ElementName = "total_pages_num")]
        public string TotalPagesNumber { get; set; }

        [XmlElement(Order = 22, ElementName = "rebutted_by")]
        public string RebuttedBy { get; set; }

        [XmlElement(Order = 23, ElementName = "rebutted_dttm")]
        public string RebuttedDateTime { get; set; }

        [XmlElement(Order = 24, ElementName = "rejected_by")]
        public string RejectedBy { get; set; }

        [XmlElement(Order = 25, ElementName = "rejected_dttm")]
        public string RejectedDateTime { get; set; }

        [XmlElement(Order = 26, ElementName = "acpt_user_by")]
        public string AcceptedBy { get; set; }

        [XmlElement(Order = 27, ElementName = "acpt_dttm")]
        public string AcceptedDateTime { get; set; }

        [XmlElement(Order = 28, ElementName = "escalated_dttm")]
        public string EscalatedDateTime { get; set; }

        [XmlElement(Order = 29, ElementName = "chargeback_reason_code")]
        public string ChargebackReasonCode { get; set; }

        [XmlElement(Order = 30, ElementName = "violation_code")]
        public string ViolationCode { get; set; }

        [XmlElement(Order = 31, ElementName = "violation_date")]
        public string ViolationDateTime { get; set; }

        [XmlElement(Order = 32, ElementName = "withdrawn_by")]
        public string WithdrawnBy { get; set; }

        [XmlElement(Order = 33, ElementName = "withdrawn_dttm")]
        public string WithdrawnDateTime { get; set; }

        [XmlElement(Order = 34, ElementName = "filing_region")]
        public string FilingRegion { get; set; }

        [XmlElement(Order = 35, ElementName = "filing_against_region")]
        public string FilingAgainstRegion { get; set; }

        [XmlElement(Order = 36, ElementName = "clnsd_merch_name")]
        public string CleansedMerchantName { get; set; }

        [XmlElement(Order = 37, ElementName = "clnsd_merch_ph_no")]
        public string CleansedMerchantPhoneNumber { get; set; }

        [XmlElement(Order = 38, ElementName = "billing_currency_cd")]
        public string BillingCurrencyCode { get; set; }

        [XmlElement(Order = 39, ElementName = "sender_bill")]
        public string SenderBill { get; set; }

        [XmlElement(Order = 40, ElementName = "withdrawing_ica")]
        public string WithdrawingICA { get; set; }

        [XmlElement(Order = 41, ElementName = "ruling")]
        public string Ruling { get; set; }

        [XmlElement(Order = 42, ElementName = "ruling_dttm")]
        public string RulingDateTime { get; set; }

        [XmlElement(Order = 43, ElementName = "escalated_by")]
        public string EscalatedBy { get; set; }

        [XmlElement(Order = 44, ElementName = "compliance_case_fee")]
        public string ComplianceCaseFee { get; set; }

        [XmlElement(Order = 45, ElementName = "filing_fee")]
        public string FilingFee { get; set; }

        [XmlElement(Order = 46, ElementName = "tech_violation_fee")]
        public string TechViolationFee { get; set; }

        [XmlElement(Order = 47, ElementName = "hubsite_fee")]
        public string HubsiteFee { get; set; }

        [XmlElement(Order = 48, ElementName = "admin_fee")]
        public string AdminFee { get; set; }

        [XmlElement(Order = 49, ElementName = "case_decision_appeal_fee")]
        public string CaseDecisionAppealFee { get; set; }

        [XmlElement(Order = 50, ElementName = "case_withdraw_fee")]
        public string CaseWithdrawFee { get; set; }

        [XmlElement(Order = 51, ElementName = "mem_mediation_transaction_fee")]
        public string MemMediationTransactionFee { get; set; }

        [XmlElement(Order = 52, ElementName = "ruling_amount")]
        public string RulingAmount { get; set; }

        [XmlElement(Order = 53, ElementName = "ruling_amount_curr_cd")]
        public string RulingAmountCurrencyCode { get; set; }

        [XmlElement(Order = 54, ElementName = "d022_point_of_service_data_code")]
        public PointOfServiceDataCode PointOfServiceDataCode { get; set; }
    }
}
