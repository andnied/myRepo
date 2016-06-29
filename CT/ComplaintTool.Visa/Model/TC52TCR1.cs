namespace ComplaintService.Visa.Model
{
    public class TC52TCR1
    {
        #region Fields
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
        /// Reserved
        /// Length = 12 char
        /// Default: 12 x Char(' ')
        /// Position: 5-16
        /// </summary>
        //private string _reserved1 = string.Empty;
        public string Reserved1 { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_reserved1) ? new string(' ', 12) : _reserved1.PadRight(12, ' '); }
        //    set { _reserved1 = value; }
        //}

        /// <summary>
        /// Fax Number
        /// Length = 16 char
        /// Default: 16 x Char(' ')
        /// Position: 17-32
        /// </summary>
        //private string _faxNumber = string.Empty;
        public string FaxNumber { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_faxNumber) ? new string(' ', 16) : _faxNumber.PadRight(16, ' '); }
        //    set { _faxNumber = value; }
        //}

        /// <summary>
        /// Interface Trace Number
        /// Length = 16 char
        /// Default: 16 x Char(' ')
        /// Position: 33-38
        /// </summary>
        //private string _interfaceTraceNumber = string.Empty;
        public string InterfaceTraceNumber { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_interfaceTraceNumber) ? new string(' ', 6) : _interfaceTraceNumber.PadRight(6, ' '); }
        //    set { _interfaceTraceNumber = value; }
        //}

        /// <summary>
        /// Requested Fulfillment Method
        /// Length = 1 char
        /// Default: 1 x Char(' ')
        /// Position: 39
        /// </summary>
        //private string _requestedFulfillmentMethod = string.Empty;
        public string RequestedFulfillmentMethod { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_requestedFulfillmentMethod) ? new string(' ', 1) : _requestedFulfillmentMethod.PadRight(1, ' '); }
        //    set { _requestedFulfillmentMethod = value; }
        //}

        /// <summary>
        /// Established Fulfillment Method
        /// Length = 1 char
        /// Default: 1 x Char(' ')
        /// Position: 40
        /// </summary>
        //private string _establishedFulfillmentMethod = string.Empty;
        public string EstablishedFulfillmentMethod { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_establishedFulfillmentMethod) ? new string(' ', 1) : _establishedFulfillmentMethod.PadRight(1, ' '); }
        //    set { _establishedFulfillmentMethod = value; }
        //}

        /// <summary>
        /// Issuer RFC BIN
        /// Length = 6 char
        /// Default: 6 x Char(' ')
        /// Position: 40
        /// </summary>
        //private string _issuerRFCBIN = string.Empty;
        public string IssuerRFCBIN { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_issuerRFCBIN) ? new string(' ', 6) : _issuerRFCBIN.PadRight(6, ' '); }
        //    set { _issuerRFCBIN = value; }
        //}

        /// <summary>
        /// Issuer RFC Sub-Address
        /// Length = 7 char
        /// Default: 7 x Char(' ')
        /// Position: 47-53
        /// </summary>
        //private string _issuerRFCSubAddress = string.Empty;
        public string IssuerRFCSubAddress { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_issuerRFCSubAddress) ? new string(' ', 7) : _issuerRFCSubAddress.PadRight(7, ' '); }
        //    set { _issuerRFCSubAddress = value; }
        //}

        /// <summary>
        /// Issuer Billing Currency Code
        /// Length = 3 char
        /// Default: 3 x Char(' ')
        /// Position: 54-56
        /// </summary>
        //private string _issuerBillingCurrencyCode = string.Empty;
        public string IssuerBillingCurrencyCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_issuerBillingCurrencyCode) ? new string(' ', 3) : _issuerBillingCurrencyCode.PadRight(3, ' '); }
        //    set { _issuerBillingCurrencyCode = value; }
        //}

        /// <summary>
        /// Issuer Billing Transaction Amount
        /// Length = 12 char
        /// Default: 12 x Char(' ')
        /// Position: 57-68
        /// </summary>
        //private string _issuerBillingTransactionAmount= string.Empty;
        public string IssuerBillingTransactionAmount { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_issuerBillingTransactionAmount) ? new string(' ', 12) : _issuerBillingTransactionAmount.PadRight(12, ' '); }
        //    set { _issuerBillingTransactionAmount = value; }
        //}

        /// <summary>
        /// Transaction Identifier
        /// Length = 15 char
        /// Default: 15 x Char(' ')
        /// Position: 69-83
        /// </summary>
        //private string _trasnactionIdentifier = string.Empty;
        public string TransactionIdentifier { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_trasnactionIdentifier) ? new string(' ', 15) : _trasnactionIdentifier.PadRight(15, ' '); }
        //    set { _trasnactionIdentifier = value; }
        //}

        /// <summary>
        /// Excluded Transaction Identifier Reason
        /// Length = 1 char
        /// Default: 1 x Char(' ')
        /// Position: 84
        /// </summary>
        //private string _excludedTransactionIdentifierReason = string.Empty;
        public string ExcludedTransactionIdentifierReason { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_excludedTransactionIdentifierReason) ? new string(' ', 1) : _excludedTransactionIdentifierReason.PadRight(1, ' '); }
        //    set { _excludedTransactionIdentifierReason = value; }
        //}

        /// <summary>
        /// CRS Processing Code
        /// Length = 1 char
        /// Default: 1 x Char(' ')
        /// Position: 85
        /// </summary>
        //private string _crsProcessingCode = string.Empty;
        public string CRSProcessingCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_crsProcessingCode) ? new string(' ', 1) : _crsProcessingCode.PadRight(1, ' '); }
        //    set { _crsProcessingCode = value; }
        //}

        /// <summary>
        /// Multiple Clearing Sequence Number
        /// Length = 2 char
        /// Default: 2 x Char(' ')
        /// Position: 86-87
        /// </summary>
        //private string _multipleClearingSequenceNumber = string.Empty;
        public string MultipleClearingSequenceNumber { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_multipleClearingSequenceNumber) ? new string(' ', 2) : _multipleClearingSequenceNumber.PadRight(2, ' '); }
        //    set { _multipleClearingSequenceNumber = value; }
        //}

        /// <summary>
        /// Reserved2
        /// Length = 81 char
        /// Default: 81 x Char(' ')
        /// Position: 88-168
        /// </summary>
        //private string _reserved2 = string.Empty;
        public string Reserved2 { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_reserved2) ? new string(' ', 81) : _reserved2.PadRight(81, ' '); }
        //    set { _reserved2 = value; }
        //}
        #endregion
        public TC52TCR1(string tcrLine)
        {
            TransactionCode = tcrLine.Substring(0, 2);
            TransactionCodeQualifier = tcrLine.Substring(2, 1);
            TransactionComponentSequenceNumber = tcrLine.Substring(3, 1);
            Reserved1 = tcrLine.Substring(4, 12);
            FaxNumber = tcrLine.Substring(16, 16);
            InterfaceTraceNumber = tcrLine.Substring(32, 6);
            RequestedFulfillmentMethod = tcrLine.Substring(38, 1);
            EstablishedFulfillmentMethod = tcrLine.Substring(39, 1);
            IssuerRFCBIN = tcrLine.Substring(40, 6);
            IssuerRFCSubAddress = tcrLine.Substring(46, 7);
            IssuerBillingCurrencyCode = tcrLine.Substring(53, 3);
            IssuerBillingTransactionAmount = tcrLine.Substring(56, 12);
            TransactionIdentifier = tcrLine.Substring(68, 15);
            ExcludedTransactionIdentifierReason = tcrLine.Substring(83, 1);
            CRSProcessingCode = tcrLine.Substring(84, 1);
            MultipleClearingSequenceNumber = tcrLine.Substring(85, 2);
            Reserved2 = tcrLine.Substring(87, 81);
        }
    }
}
