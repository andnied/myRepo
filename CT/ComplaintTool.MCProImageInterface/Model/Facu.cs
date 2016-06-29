using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ComplaintTool.MCProImageInterface.Model
{
    [Serializable, XmlRoot("facu")]
    public class Facu
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

        [XmlElement(Order = 9, ElementName = "d025_msg_reason_code")]
        public string MessageReasonCode { get; set; }

        [XmlElement(Order = 10, ElementName = "members_file_num")]
        public string MembersFileNumber { get; set; }

        [XmlElement(Order = 11, ElementName = "merchant_name")]
        public string MerchantName { get; set; }

        [XmlElement(Order = 12, ElementName = "dispute_amount")]
        public string DisputeAmount { get; set; }

        [XmlElement(Order = 13, ElementName = "dispute_amount_curr_cd")]
        public string DisputeAmountCurrencyCode { get; set; }

        [XmlElement(Order = 14, ElementName = "sender_settlement_amt")]
        public string SenderSettlementAmount { get; set; }

        [XmlElement(Order = 15, ElementName = "sender_settlement_amt_curr_cd")]
        public string SenderSettlementAmountCurrencyCode { get; set; }

        [XmlElement(Order = 16, ElementName = "resp_due_dttm")]
        public string ResponseDueDateTime { get; set; }

        [XmlElement(Order = 17, ElementName = "resp_code")]
        public string ResponseCode { get; set; }

        [XmlElement(Order = 18, ElementName = "memo_txt")]
        public string Memo { get; set; }

        [XmlElement(Order = 19, ElementName = "file_as")]
        public string FileAs { get; set; }

        [XmlElement(Order = 20, ElementName = "case_owner")]
        public string CaseOwner { get; set; }

        [XmlElement(Order = 21, ElementName = "submitted_dttm")]
        public string SubmittedDateTime { get; set; }

        [XmlElement(Order = 22, ElementName = "submitted_by")]
        public string SubmittedBy { get; set; }
    }
}
