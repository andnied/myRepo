using System.Xml.Serialization;

namespace ComplaintTool.Postilion.Outgoing.Model.FeeCollection.Visa
{
    public class AdjustmentComponent
    {
        //    FeeCollectionControlNr	Generator - do doddania nowe pole
        [XmlElement(Order = 1, ElementName = "FeeCollectionControlNr")]
        public string FeeCollectionControlNr { get; set; }
        //    MessageText	z complaint tabeli
        private string _messageText = string.Empty;
        [XmlElement(Order = 2, ElementName = "MessageText")]
        public string MessageText
        {
            get { return _messageText; }
            set { _messageText = value; }
        } 
        //    DestinationInstitutionId	NULL
        private string _destinationInstitutionId = string.Empty;
        [XmlElement(Order = 3, ElementName = "DestinationInstitutionId")]
        public string DestinationInstitutionId
        {
            get { return _destinationInstitutionId; }
            set { _destinationInstitutionId = value; }
        } 
        //    AcquiringInstIdCode	"const value Set '400748' where position 2-7 of Complaint.ARN = '400748' Set '414848'  where position 2-7 of Complaint.ARN = '414848'"
        private string _acquiringInstIdCode = string.Empty;
        [XmlElement(Order = 4, ElementName = "AcquiringInstIdCode")]
        public string AcquiringInstIdCode
        {
            get { return _acquiringInstIdCode; }
            set { _acquiringInstIdCode = value; }
        } 

        //    AcquiringInstCountryCode	const?
        private string _acquiringInstCountryCode = new string(' ', 3);
        [XmlElement(Order = 5, ElementName = "AcquiringInstCountryCode")]
        public string AcquiringInstCountryCode
        {
            get { return _acquiringInstCountryCode; }
            set { _acquiringInstCountryCode = value; }
        }
        //    ReceivingInstCountryCode	const? 
        private string _receivingInstCountryCode = new string(' ', 3);
        [XmlElement(Order = 6, ElementName = "ReceivingInstCountryCode")]
        public string ReceivingInstCountryCode
        {
            get { return _receivingInstCountryCode; }
            set { _receivingInstCountryCode = value; }
        }
        //    ReceivingInstIdCode	6 pierwszych pozycji z IncomingTranMASTERCARD.PAN 
        private string _receivingInstIdCode = new string(' ', 3);
        [XmlElement(Order = 7, ElementName = "ReceivingInstIdCode")]
        public string ReceivingInstIdCode
        {
            get { return _receivingInstIdCode; }
            set { _receivingInstIdCode = value; }
        }
        //    Role	const '1'
        [XmlElement(Order = 8, ElementName = "Role")]
        public string Role = @"1";
        //    MsgTypeId	"const dla 4105 value '5' dl;a 4106 value '6'"
        private string _msgTypeId = string.Empty;
        [XmlElement(Order = 9, ElementName = "MsgTypeId")]
        public string MsgTypeId
        {
            get { return _msgTypeId; }
            set { _msgTypeId = value; }
        } 
        //    MsgId	post_tran.post_tran_id
        private string _msgId = string.Empty;
        [XmlElement(Order = 10, ElementName = "MsgId")]
        public string MsgId
        {
            get { return _msgId; }
            set { _msgId = value; }
        } 
        //    Correction	const '0'
        [XmlElement(Order = 11, ElementName = "Correction")]
        public string Correction = @"0";
        //    ExtendedFields	"Where BIN = '400748' then const value: 'PARTICIPANT_ID = 19' Where BIN = '414848' then const value: 'PARTICIPANT_ID = 20'"
        private string _extendedFields = string.Empty;
        [XmlElement(Order = 12, ElementName = "ExtendedFields")]
        public string ExtendedFields
        {
            get { return _extendedFields; }
            set { _extendedFields = value; }
        } 
    }
}
