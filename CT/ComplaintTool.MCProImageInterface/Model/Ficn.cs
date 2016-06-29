using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ComplaintTool.MCProImageInterface.Model
{
    [Serializable, XmlRoot("ficn")]
    public class Ficn
    {
        [XmlElement(Order = 1, ElementName = "case_type")]
        public string CaseType { get; set; }

        [XmlElement(Order = 2, ElementName = "cardholder_num")]
        public string CardholderNumber { get; set; }

        [XmlElement(Order = 3, ElementName = "chargeback_reason_code")]
        public string ChargebackReasonCode { get; set; }

        [XmlElement(Order = 4, ElementName = "dispute_amount")]
        public string DisputeAmount { get; set; }

        [XmlElement(Order = 5, ElementName = "dispute_amount_curr_cd")]
        public string DisputeAmountCurrencyCode { get; set; }

        [XmlElement(Order = 6, ElementName = "arn")]
        public string ARN { get; set; }

        [XmlElement(Order = 7, ElementName = "cb_ref_num")]
        public string ChargebackReferenceNumber { get; set; }

        [XmlElement(Order = 8, ElementName = "filing_ica")]
        public string FilingICA { get; set; }

        [XmlElement(Order = 9, ElementName = "filed_against_ica")]
        public string FiledAgainstICA { get; set; }

        [XmlElement(Order = 10, ElementName = "merchant_name")]
        public string MerchantName { get; set; }

        [XmlElement(Order = 11, ElementName = "members_file_num")]
        public string MembersFileNum { get; set; }

        [XmlElement(Order = 12, ElementName = "file_as")]
        public string FileAs { get; set; }

        [XmlElement(Order = 13, ElementName = "submitted_by")]
        public string SubmittedBy { get; set; }

        [XmlElement(Order = 14, ElementName = "submitted_dttm")]
        public string SubmittedDateTime { get; set; }

        [XmlElement(Order = 15, ElementName = "memo_txt")]
        public string Memo { get; set; }

        [XmlElement(Order = 16, ElementName = "unjust_enrchmt_chbk_dt")]
        public string UnjustEnrchmtChbkDt { get; set; }

        [XmlElement(Order = 17, ElementName = "unjust_enrchmt_cr_dt")]
        public string UnjustEnrchmtCrDt { get; set; }

        [XmlElement(Order = 18, ElementName = "virtual_account_num")]
        public string VirtualAccountNum { get; set; }

        [XmlElement(Order = 19, ElementName = "violation_date")]
        public string ViolationDate { get; set; }

        [XmlElement(Order = 20, ElementName = "violation_code")]
        public string ViolationCode { get; set; }

        [XmlElement(Order = 21, ElementName = "resp_due_dttm")]
        public string RespDueDttm { get; set; }
    }
}
