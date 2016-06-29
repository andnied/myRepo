using System;
using System.Xml.Serialization;

namespace ComplaintTool.Postilion.Outgoing.Model.Representment.MasterCard
{
    [Serializable, XmlRoot("Ipm")]
    public class Ipm
    {
        //TranLifeCycleID
        private string _tranLifeCycleId = string.Empty;
        [XmlElement(Order = 1, ElementName = "TranLifeCycleID")]
        public string TranLifeCycleID
        {
            get { return string.IsNullOrEmpty(_tranLifeCycleId) ? null : _tranLifeCycleId.PadRight(16, ' '); }
            set { _tranLifeCycleId = value; }
        }
        //IRD
        private string _ird = string.Empty;
        [XmlElement(Order = 2, ElementName = "IRD")]
        public string IRD
        {
            get { return string.IsNullOrEmpty(_ird) ? new string('0', 2) : _ird.PadLeft(2, '0'); }
            set { _ird = value; }
        }
        //McAssignedId
        private string _mcAssignedId = string.Empty;
        [XmlElement(Order = 3, ElementName = "McAssignedId")]
        public string McAssignedId
        {
            get { return string.IsNullOrEmpty(_mcAssignedId) ? null : _mcAssignedId.PadRight(6, ' '); }
            set { _mcAssignedId = value; }
        }
        //FunctionCode
        private string _functionCode = string.Empty;
        [XmlElement(Order = 4, ElementName = "FunctionCode")]
        public string FunctionCode
        {
            get { return string.IsNullOrEmpty(_functionCode) ? string.Empty : _functionCode.PadLeft(3, '0'); }
            set { _functionCode = value; }
        }
        //AmountsOriginal
        private string _amountsOriginal = string.Empty;
        [XmlElement(Order = 5, ElementName = "AmountsOriginal")]
        public string AmountsOriginal
        {
            get { return string.IsNullOrEmpty(_amountsOriginal) ? new string('0', 12) + new string('0', 12) : _amountsOriginal.PadLeft(12, '0') + new string('0', 12); }
            set { _amountsOriginal = value; }
        }
        //FraudNotificationInfo
        private string _fraudNotificationInfo = string.Empty;
        [XmlElement(Order = 6, ElementName = "FraudNotificationInfo")]
        public string FraudNotificationInfo
        {
            get { return string.IsNullOrEmpty(_fraudNotificationInfo) ? null : _fraudNotificationInfo.PadLeft(8, '0'); }
            set { _fraudNotificationInfo = value; }
        }
    }
}
