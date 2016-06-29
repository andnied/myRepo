using System.Xml.Serialization;

namespace ComplaintTool.Postilion.Outgoing.Model.Representment
{
    [XmlType]
    public class OriginalAmount
    {
        private string _tranAmount = string.Empty;
        [XmlElement(Order = 1, ElementName = "TranAmount")]
        public string TranAmount
        {
            get { return string.IsNullOrEmpty(_tranAmount) ? new string('0', 12) : _tranAmount.Trim().PadLeft(12, '0'); }
            set { _tranAmount = value; }
        }
        private string _settleAmount = string.Empty;
        [XmlElement(Order = 2, ElementName = "SettleAmount")]
        public string SettleAmount
        {
            get { return string.IsNullOrEmpty(_settleAmount) ? new string('0', 12) : _settleAmount.Trim().PadLeft(12, '0'); }
            set { _settleAmount = value; }
        }
        private string _tranCurrencyCode = string.Empty;
        [XmlElement(Order = 3, ElementName = "TranCurrencyCode")]
        public string TranCurrencyCode
        {
            get { return string.IsNullOrEmpty(_tranCurrencyCode) ? new string('0', 3) : _tranCurrencyCode.Trim().PadLeft(3, '0'); }
            set { _tranCurrencyCode = value; }
        }
        private string _settleCurrencyCode = string.Empty;
        [XmlElement(Order = 4, ElementName = "SettleCurrencyCode")]
        public string SettleCurrencyCode
        {
            get { return string.IsNullOrEmpty(_settleCurrencyCode) ? new string('0', 3) : _settleCurrencyCode.Trim().PadLeft(3, '0'); }
            set { _settleCurrencyCode = value; }
        }
    }
}
