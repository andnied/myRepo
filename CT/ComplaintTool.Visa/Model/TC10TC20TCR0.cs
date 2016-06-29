namespace ComplaintService.Visa.Model
{
    public class TC10TC20TCR0
    {
        #region  Fields
        /// <summary>
        /// Transaction Code
        /// Length = 2 char
        /// Default: 2 x Char(' ')
        /// Position: 0-2
        /// </summary>
        //private string _transactionCode = string.Empty;
        public string TransactionCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_transactionCode) ? new string(' ', 2) : _transactionCode.PadRight(2, ' '); }
        //    set { _transactionCode = value; }
        //}

        /// <summary>
        /// Transaction Code Qualifier
        /// Length = 1 char
        /// Default: 1 x Char(' ')
        /// Position: 3
        /// </summary>
        //private string _transactionCodeQualifier;
        public string TransactionCodeQualifier { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_transactionCodeQualifier) ? new string(' ', 1) : _transactionCodeQualifier.PadRight(1, ' '); }
        //    set { _transactionCodeQualifier = value; }
        //}

        /// <summary>
        /// Transaction Component Sequence Number
        /// Length = 1 char
        /// Default: 1 x Char(' ')
        /// Position: 4
        /// </summary>
        //private string _transactionComponentSequenceNumber = string.Empty;
        public string TransactionComponentSequenceNumber { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_transactionComponentSequenceNumber) ? new string(' ', 1) : _transactionComponentSequenceNumber.PadRight(1, ' '); }
        //    set { _transactionComponentSequenceNumber = value; }
        //}

        /// <summary>
        /// Destination BIN
        /// Length = 6 char
        /// Default: 6 x Char(' ')
        /// Position: 5-10
        /// </summary>
        //private string _destinationBIN = string.Empty;
        public string DestinationBIN { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_destinationBIN) ? new string(' ', 6) : _destinationBIN.PadRight(6, ' '); }
        //    set { _destinationBIN = value; }
        //}

        /// <summary>
        /// Source BIN
        /// Length = 6 char
        /// Default: 6 x Char(' ')
        /// Position: 11-16
        /// </summary>
        //private string _sourceBIN = string.Empty;
        public string SourceBIN { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_sourceBIN) ? new string(' ', 6) : _sourceBIN.PadRight(6, ' '); }
        //    set { _sourceBIN = value; }
        //}

        /// <summary>
        /// Reason Code
        /// Length = 4 char
        /// Default: 4 x Char(' ')
        /// Position: 17-20
        /// </summary>
        //private string _reasonCode = string.Empty;
        public string ReasonCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_reasonCode) ? new string(' ', 4) : _reasonCode.PadRight(4, ' '); }
        //    set { _reasonCode = value; }
        //}

        /// <summary>
        /// Country Code
        /// Length = 3 char
        /// Default: 3 x Char(' ')
        /// Position: 21-23
        /// </summary>
        //private string _countryCode = string.Empty;
        public string CountryCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_countryCode) ? new string(' ', 3) : _countryCode.PadRight(3, ' '); }
        //    set { _countryCode = value; }
        //}

        /// <summary>
        /// Event Date
        /// Length = 4 char
        /// Default: 4 x Char(' ')
        /// Position: 24-27
        /// </summary>
        //private string _eventDate = string.Empty;
        public string EventDate { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_eventDate) ? new string(' ', 4) : _eventDate.PadRight(4, ' '); }
        //    set { _eventDate = value; }
        //}

        /// <summary>
        /// Account Number
        /// Length = 16 char
        /// Default: 16 x Char('0')
        /// Position: 28-43
        /// </summary>
        //private string _accountNumber = string.Empty;
        public string AccountNumber { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_accountNumber) ? new string('0', 16) : _accountNumber.PadLeft(16, '0'); }
        //    set { _accountNumber = value; }
        //}

        /// <summary>
        /// Account Number Extention
        /// Length = 3 char
        /// Default: 3 x Char(' ')
        /// Position: 44-46
        /// </summary>
        //private string _accountNumberExtension = string.Empty;
        public string AccountNumberExtension { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_accountNumberExtension) ? new string('0', 3) : _accountNumberExtension.PadRight(3, '0'); }
        //    set { _accountNumberExtension = value; }
        //}

        /// <summary>
        /// Destination Amount
        /// Length = 12 char
        /// Default: 12 x Char('0')
        /// Position: 47-58
        /// </summary>
        //private string _destinationAmount = string.Empty;
        public string DestinationAmount { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_destinationAmount) ? new string('0', 12) : _destinationAmount.PadLeft(12, '0'); }
        //    set { _destinationAmount = value; }
        //}

        /// <summary>
        /// Destination Currency Code
        /// Length = 3 char
        /// Default: 3 x Char(' ')
        /// Position: 59-61
        /// </summary>
        //private string _destinationCurrencyCode = string.Empty;
        public string DestinationCurrencyCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_destinationCurrencyCode) ? new string(' ', 3) : _destinationCurrencyCode.PadRight(3, ' '); }
        //    set { _destinationCurrencyCode = value; }
        //}

        /// <summary>
        /// Source Amount
        /// Length = 12 char
        /// Default: 12 x Char('0')
        /// Position: 62-73
        /// </summary>
        //private string _sourceAmount = string.Empty;
        public string SourceAmount { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_sourceAmount) ? new string('0', 12) : _sourceAmount.PadLeft(12, '0'); }
        //    set { _sourceAmount = value; }
        //}

        /// <summary>
        /// Source Currency Code
        /// Length = 3 char
        /// Default: 3 x Char(' ')
        /// Position: 74-76
        /// </summary>
        //private string _sourceCurrencyCode = string.Empty;
        public string SourceCurrencyCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_sourceCurrencyCode) ? new string(' ', 3) : _sourceCurrencyCode.PadRight(3, ' '); }
        //    set { _sourceCurrencyCode = value; }
        //}

        /// <summary>
        /// Message Text
        /// Length = 70 char
        /// Default: 70 x Char(' ')
        /// Position: 77-146
        /// </summary>
        //private string _messageText = string.Empty;
        public string MessageText { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_messageText) ? new string(' ', 70) : _messageText.PadRight(70, ' '); }
        //    set { _messageText = value; }
        //}

        /// <summary>
        /// Settlement Flag
        /// Length = 1 char
        /// Default: 1 x Char(' ')
        /// Position: 147
        /// </summary>
        //private string _settlementFlag = string.Empty;
        public string SettlementFlag { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_settlementFlag) ? new string(' ', 1) : _settlementFlag.PadRight(1, ' '); }
        //    set { _settlementFlag = value; }
        //}

        /// <summary>
        /// Transaction Identifier
        /// Length = 15 char
        /// Default: 15 x Char('0')
        /// Position: 148-162
        /// </summary>
        //private string _transactionIdentifier = string.Empty;
        public string TransactionIdentifier { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_transactionIdentifier) ? new string('0', 15) : _transactionIdentifier.PadLeft(15, '0'); }
        //    set { _transactionIdentifier = value; }
        //}

        /// <summary>
        /// Reserved1
        /// Length = 1 char
        /// Default: 1 x Char(' ')
        /// Position: 163
        /// </summary>
        //private string _reserved1 = string.Empty;
        public string Reserved1 { get; set; }
        //{ 
        //    get { return string.IsNullOrEmpty(_reserved1) ? new string(' ', 1) : _reserved1.PadRight(1, ' '); }
        //    set { _reserved1 = value; }
        //}

        /// <summary>
        /// Central Processing Date
        /// Length = 4 char
        /// Default: 4 x Char(' ')
        /// Position: 164-167
        /// </summary>
        //private string _centralProcessingDate = string.Empty;
        public string CentralProcessingDate { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_centralProcessingDate) ? new string(' ', 4) : _centralProcessingDate.PadRight(4, ' '); }
        //    set { _centralProcessingDate = value; }
        //}

        /// <summary>
        /// Reimbursement Attribute
        /// Length = 1 char
        /// Default: 1 x Char(' ')
        /// Position: 168
        /// </summary>
        //private string _reimbursementAttribute = string.Empty;
        public string ReimbursementAttribute { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_reimbursementAttribute) ? new string(' ', 1) : _reimbursementAttribute.PadRight(1, ' '); }
        //    set { _reimbursementAttribute = value; }
        //}

        #endregion

        public TC10TC20TCR0(string tcrLine)
        {
            TransactionCode = tcrLine.Substring(0, 2);
            TransactionCodeQualifier = tcrLine.Substring(2, 1);
            TransactionComponentSequenceNumber = tcrLine.Substring(3, 1);
            DestinationBIN = tcrLine.Substring(4, 6);
            SourceBIN = tcrLine.Substring(10, 6);
            ReasonCode = tcrLine.Substring(16, 4);
            CountryCode = tcrLine.Substring(20, 3);
            EventDate = tcrLine.Substring(23, 4);
            AccountNumber = tcrLine.Substring(27, 16);
            AccountNumberExtension = tcrLine.Substring(43, 3);
            DestinationAmount = tcrLine.Substring(46, 12);
            DestinationCurrencyCode = tcrLine.Substring(58, 3);
            SourceAmount = tcrLine.Substring(61, 12);
            SourceCurrencyCode = tcrLine.Substring(73, 3);
            MessageText = tcrLine.Substring(76, 70);
            SettlementFlag = tcrLine.Substring(146, 1);
            TransactionIdentifier = tcrLine.Substring(147, 15);
            Reserved1 = tcrLine.Substring(162, 1);
            CentralProcessingDate = tcrLine.Substring(163, 4);
            ReimbursementAttribute = tcrLine.Substring(167, 1);

            //using (StreamWriter w = File.AppendText("log.txt")) { w.Write(this + "\n"); }
        }

        public override string ToString()
        {
            var ret = "";
            var properties = this.GetType().GetProperties();
            foreach (var pi in properties)
                ret += string.Format("{0} = {1} | ", pi.Name, pi.GetValue(this, null));
            return ret;
        }
    }
}
