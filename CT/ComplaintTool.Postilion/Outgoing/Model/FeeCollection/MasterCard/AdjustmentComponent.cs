using System;
using System.Xml.Serialization;

namespace ComplaintTool.Postilion.Outgoing.Model.FeeCollection.MasterCard
{
    [Serializable, XmlRoot("AdjustmentComponent")]
    public class AdjustmentComponent
    {
        [XmlElement(Order = 1, ElementName = "FeeCollectionControlNr")]
        public string FeeCollectionControlNr { get; set; }
        //MessageText	ComplaintStage.MemberMessageText	
        private string _messageText = string.Empty;
        [XmlElement(Order = 2, ElementName = "MessageText")]
        public string MessageText
        {
            get { return _messageText; }
            set { _messageText = value; }
        } 
        //FulfillmentDocumentCode	ComplaintStage.DocumentationIndicator	
        private string _fulfillmentDocumentCode = string.Empty;
        [XmlElement(Order = 3, ElementName = "FulfillmentDocumentCode")]
        public string FulfillmentDocumentCode
        {
            get { return _fulfillmentDocumentCode; }
            set { _fulfillmentDocumentCode = value; }
        } 
        //MasterCardFunctionCode	ComplaintRecord.FunctionCode	PCA / PCAR
        private string _masterCardFunctionCode = string.Empty;
        [XmlElement(Order = 4, ElementName = "MasterCardFunctionCode")]
        public string MasterCardFunctionCode
        {
            get { return _masterCardFunctionCode; }
            set { _masterCardFunctionCode = value; }
        } 
        //DestinationInstitutionId	Skonsultować z Jędrzejem lubŚwidzickim???	BIN instytucji dla której leci fee
        private string _destinationInstitutionId = string.Empty;
        [XmlElement(Order = 5, ElementName = "DestinationInstitutionId")]
        public string DestinationInstitutionId
        {
            get { return _destinationInstitutionId; }
            set { _destinationInstitutionId = value; }
        } 
        //DateAction	"minus jeden dzień od daty wprowadzenia fee. TBC na podst. nowego trace na AH - konieczne ponowienie prośby do administr o wystawieni"	
        private string _dateAction = string.Empty;
        [XmlElement(Order = 6, ElementName = "DateAction")]
        public string DateAction
        {
            get { return _dateAction; }
            set { _dateAction = value; }
        } 
        //AcquiringInstIdCode	16035'	
        private string _acquiringInstIdCode = @"16035";
        [XmlElement(Order = 7, ElementName = "AcquiringInstIdCode")]
        public string AcquiringInstIdCode
        {
            get { return _acquiringInstIdCode; }
            set { _acquiringInstIdCode = value; }
        }
        //AcquiringInstCountryCode	cosnt 3 space	
        [XmlElement(Order = 8, ElementName = "AcquiringInstCountryCode")]
        public string AcquiringInstCountryCode = new string(' ',3);
        //ReceivingInstCountryCode	cosnt 3 space	
        [XmlElement(Order = 9, ElementName = "ReceivingInstCountryCode")]
        public string ReceivingInstCountryCode = new string(' ', 3);
        //ReceivingInstIdCode	NULL	
        [XmlElement(Order = 10, ElementName = "ReceivingInstIdCode")]
        public string ReceivingInstIdCode = new string(' ', 3);
        //Role	1	
        [XmlElement(Order = 11, ElementName = "Role")]
        public string Role = @"1";
        //MsgTypeId	"const dla 4105 value '5' dla 4106 value '6'"	
        private string _msgTypeId = string.Empty;
        [XmlElement(Order = 12, ElementName = "MsgTypeId")]
        public string MsgTypeId
        {
            get { return _msgTypeId; }
            set { _msgTypeId = value; }
        } 
        //MsgId	post_tran.post_tran_id	
        private string _msgId = string.Empty;
        [XmlElement(Order = 13, ElementName = "MsgId")]
        public string MsgId
        {
            get { return _msgId; }
            set { _msgId = value; }
        } 
        //Correction	const '0'	
        [XmlElement(Order = 14, ElementName = "Correction")]
        public string Correction = @"0";
        //ExtendedFields	const value = 'PARTICIPANT_ID:20'	
        private string _extendedFields = string.Empty;
        [XmlElement(Order = 15, ElementName = "ExtendedFields")]
        public string ExtendedFields
        {
            get { return _extendedFields; }
            set { _extendedFields = value; }
        }
    }
}
