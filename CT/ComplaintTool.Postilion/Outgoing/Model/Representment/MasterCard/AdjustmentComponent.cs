using System;
using System.Xml.Serialization;

namespace ComplaintTool.Postilion.Outgoing.Model.Representment.MasterCard
{
    [Serializable, XmlRoot("AdjustmentComponent")]
    public class AdjustmentComponent
    {
        [XmlElement(Order = 1, ElementName = "OriginalAmounts")]
        public OriginalAmount OriginalAmounts { get; set; }
        //ProcessingCode
        private string _processingCode = string.Empty;
        [XmlElement(Order = 2, ElementName = "ProcessingCode")]
        public string ProcessingCode
        {
            get { return string.IsNullOrEmpty(_processingCode) ? new string('0', 6) : _processingCode.Trim().PadRight(6, '0'); }
            set { _processingCode = value; }
        }
        //AcquirerRefNr
        private string _acquirerRefNr = string.Empty;
        [XmlElement(Order = 3, ElementName = "AcquirerRefNr")]
        public string AcquirerRefNr
        {
            get { return string.IsNullOrEmpty(_acquirerRefNr) ? new string(' ', 23) : _acquirerRefNr.Trim().PadLeft(23, ' '); }
            set { _acquirerRefNr = value; }
        }
        //IssuerRefNr
        private string _issuerRefNr = string.Empty;
        [XmlElement(Order = 4, ElementName = "IssuerRefNr")]
        public string IssuerRefNr
        {
            get { return string.IsNullOrEmpty(_issuerRefNr) ? new string(' ', 10) : _issuerRefNr.Trim().PadRight(10, ' '); }
            set { _issuerRefNr = value; }
        }
        //DocumentationIndicator
        private string _documentationIndicator = string.Empty;
        [XmlElement(Order = 5, ElementName = "DocumentationIndicator")]
        public string DocumentationIndicator
        {
            get { return string.IsNullOrEmpty(_documentationIndicator) ? new string('0', 1) : _documentationIndicator.Trim().PadRight(1, '0'); }
            set { _documentationIndicator = value; }
        }        
        //MessageText
        private string _messageText = string.Empty;
        [XmlElement(Order = 6, ElementName = "MessageText")]
        public string MessageText
        {
            get { return _messageText; }
            set { _messageText = value; }
        } 
        //AcquiringInstIdCode //IKA
        [XmlElement(Order = 7, ElementName = "AcquiringInstIdCode")]
        public string AcquiringInstIdCode = @"016035";
        //AuthIdRsp
        private string _authIdRsp = string.Empty;
        [XmlElement(Order = 8, ElementName = "AuthIdRsp")]
        public string AuthIdRsp
        {
            get { return _authIdRsp; }
            set { _authIdRsp = value; }
        } 
        //MsgTypeId
        private string _msgTypeId = string.Empty;
        [XmlElement(Order = 9, ElementName = "MsgTypeId")]
        public string MsgTypeId
        {
            get { return _msgTypeId; }
            set { _msgTypeId = value; }
        } 
        //MsgId
        private string _msgId = string.Empty;
        [XmlElement(Order = 10, ElementName = "MsgId")]
        public string MsgId
        {
            get { return _msgId; }
            set { _msgId = value; }
        } 
        //Correction
        [XmlElement(Order = 11, ElementName = "Correction")] 
        public string Correction = @"0";
        //UcafCollectionInd
        private string _ucafCollectionInd = string.Empty;
        [XmlElement(Order = 12, ElementName = "UcafCollectionInd")]
        public string UcafCollectionInd
        {
            get { return _ucafCollectionInd; }
            set { _ucafCollectionInd = value; }
        }
        //ExtendedFields
        private string _extendedFields = string.Empty;
        [XmlElement(Order = 13, ElementName = "ExtendedFields")]
        public string ExtendedFields
        {
            get { return _extendedFields; }
            set { _extendedFields = value; }
        }
    }
}
