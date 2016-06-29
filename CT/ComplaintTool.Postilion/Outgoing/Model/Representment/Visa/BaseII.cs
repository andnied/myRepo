using System;
using System.Xml.Serialization;

namespace ComplaintTool.Postilion.Outgoing.Model.Representment.Visa
{
    [Serializable, XmlRoot("BaseII")]
    public class BaseII
    {
        //ChargebackRefNr
        private string _chargebackRefNr = string.Empty;
        [XmlElement(Order = 1, ElementName = "ChargebackRefNr")]
        public string ChargebackRefNr
        {
            get { return string.IsNullOrEmpty(_chargebackRefNr) ? null : _chargebackRefNr.Trim().PadLeft(6, '0'); }
            set { _chargebackRefNr = value; }
        }
        //TranId
        private string _tranId = string.Empty;
        [XmlElement(Order = 2, ElementName = "TranId")]
        public string TranId
        {
            get { return string.IsNullOrEmpty(_tranId) ? null : _tranId.Trim().PadLeft(15, '0'); }
            set { _tranId = value; }
        }
        //MultiClearingSeqNr
        private string _multiClearingSeqNr = string.Empty;
        [XmlElement(Order = 3, ElementName = "MultiClearingSeqNr")]
        public string MultiClearingSeqNr
        {
            get { return string.IsNullOrEmpty(_multiClearingSeqNr) ? null : _multiClearingSeqNr.Trim().PadLeft(2, '0'); }
            set { _multiClearingSeqNr = value; }
        }
        //MultiClearingSeqCnt
        private string _multiClearingSeqCnt = string.Empty;
        [XmlElement(Order = 4, ElementName = "MultiClearingSeqCnt")]
        public string MultiClearingSeqCnt
        {
            get { return string.IsNullOrEmpty(_multiClearingSeqCnt) ? null : _multiClearingSeqCnt.Trim().PadLeft(2, '0'); }
            set { _multiClearingSeqCnt = value; }
        }
        //AuthSourceCode
        private string _authSourceCode = string.Empty;
        [XmlElement(Order = 5, ElementName = "AuthSourceCode")]
        public string AuthSourceCode
        {
            get { return string.IsNullOrEmpty(_authSourceCode) ? null : _authSourceCode.Trim().PadLeft(1, '0'); }
            set { _authSourceCode = value; }
        }
        //AVSRspCode
        private string _avsRspCode = string.Empty;
        [XmlElement(Order = 6, ElementName = "AVSRspCode")]
        public string AVSRspCode
        {
            get { return string.IsNullOrEmpty(_avsRspCode) ? null : _avsRspCode.Trim().PadRight(1, ' '); }
            set { _avsRspCode = value; }
        }
        //MarketSpecAuth
        private string _marketSpecAuth = string.Empty;
        [XmlElement(Order = 7, ElementName = "MarketSpecAuth")]
        public string MarketSpecAuth
        {
            get { return string.IsNullOrEmpty(_marketSpecAuth) ? null : _marketSpecAuth.Trim().PadRight(1, ' '); }
            set { _marketSpecAuth = value; }
        }
        //AuthRspCode
        private string _authRspCode = string.Empty;
        [XmlElement(Order = 8, ElementName = "AuthRspCode")]
        public string AuthRspCode
        {
            get { return string.IsNullOrEmpty(_authRspCode) ? null : _authRspCode.Trim().PadRight(2, ' '); }
            set { _authRspCode = value; }
        }
    }
}
