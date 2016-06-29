namespace ComplaintService.Visa.Model
{
    public class TC52TCR0
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
        public string TransactionCodeQualifier {get; set;}
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
        /// Account Number
        /// Length = 16 char
        /// Default: 16 x Char('0')
        /// Position: 5-20
        /// </summary>
        //private string _accountNumber = string.Empty;
        public string AccountNumber { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_accountNumber) ? new string('0', 16) : _accountNumber.PadLeft(16, '0'); }
        //    set { _accountNumber = value; }
        //}

        /// <summary>
        /// Account Number Extension
        /// Length = 3 char
        /// Default: 3 x Char(' ')
        /// Position: 21-23
        /// </summary>
        //private string _accountNumberExtension = string.Empty;
        public string AccountNumberExtension { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_accountNumberExtension) ? new string(' ', 3) : _accountNumberExtension.PadRight(3, ' '); }
        //    set { _accountNumberExtension = value; }
        //}

        /// <summary>
        /// Acquirer Reference number
        /// Length = 23 char
        /// Default: 23 x Char('0')
        /// Position: 24-46
        /// </summary>
        //private string _ARN = string.Empty;
        public string ARN { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_ARN) ? new string('0', 23) : _ARN.PadLeft(23, '0'); }
        //    set { _ARN = value; }
        //}

        /// <summary>
        /// Acquirer Business ID
        /// Length = 8 char
        /// Default: 8 x Char(' ')
        /// Position: 47-54
        /// </summary>
        //private string _acquirerBusinessID = string.Empty;
        public string AcquirerBusinessID { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_acquirerBusinessID) ? new string(' ', 8) : _acquirerBusinessID.PadRight(8, ' '); }
        //    set { _acquirerBusinessID = value; }
        //}

        /// <summary>
        /// Purchase Date
        /// Length = 4 char
        /// Default: 4 x Char(' ')
        /// Position: 55-58
        /// </summary>
        //private string _purchaseDate = string.Empty;
        public string PurchaseDate { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_purchaseDate) ? new string(' ', 4) : _purchaseDate.PadRight(4, ' '); }
        //    set { _purchaseDate = value; }
        //}

        /// <summary>
        /// Transaction Amount
        /// Length = 12 char
        /// Default: 12 x Char('0')
        /// Position: 55-58
        /// </summary>
        //private string _transactionAmount = string.Empty;
        public string TransactionAmount { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_transactionAmount) ? new string(' ', 12) : _transactionAmount.PadLeft(12, ' '); }
        //    set { _transactionAmount = value; }
        //}

        /// <summary>
        /// Transaction Currency Code
        /// Length = 3 char
        /// Default: 3 x Char(' ')
        /// Position: 71-73
        /// </summary>
        //private string _transactionCurrencyCode = string.Empty;
        public string TransactionCurrencyCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_transactionCurrencyCode) ? new string(' ', 3) : _transactionCurrencyCode.PadRight(3, ' '); }
        //    set { _transactionCurrencyCode = value; }
        //}

        /// <summary>
        /// Merchant Name
        /// Length = 25 char
        /// Default: 25 x Char(' ')
        /// Position: 74-98
        /// </summary>
        //private string _merchantName = string.Empty;
        public string MerchantName { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_merchantName) ? new string(' ', 25) : _merchantName.PadRight(25, ' '); }
        //    set { _merchantName = value; }
        //}

        /// <summary>
        /// Merchant City
        /// Length = 13 char
        /// Default: 13 x Char(' ')
        /// Position: 99-111
        /// </summary>
        //private string _merchantCity = string.Empty;
        public string MerchantCity { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_merchantCity) ? new string(' ', 13) : _merchantCity.PadRight(13, ' '); }
        //    set { _merchantCity = value; }
        //}

        /// <summary>
        /// Merchant Country Code
        /// Length = 3 char
        /// Default: 3 x Char(' ')
        /// Position: 112-114
        /// </summary>
        //private string _merchantCountryCode = string.Empty;
        public string MerchantCountryCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_merchantCountryCode) ? new string(' ', 3) : _merchantCountryCode.PadRight(3, ' '); }
        //    set { _merchantCountryCode = value; }
        //}

        /// <summary>
        /// Merchant Category Code
        /// Length = 4 char
        /// Default: 4 x Char(' ')
        /// Position: 115-118
        /// </summary>
        //private string _merchantCategoryCode = string.Empty;
        public string MerchantCategoryCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_merchantCategoryCode) ? new string(' ', 4) : _merchantCategoryCode.PadRight(4, ' '); }
        //    set { _merchantCategoryCode = value; }
        //}

        /// <summary>
        /// Merchant ZIP Code
        /// Length = 5 char
        /// Default: 5 x Char(' ')
        /// Position: 119-123
        /// </summary>
        //private string _merchantZIPCode = string.Empty;
        public string MerchantZIPCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_merchantZIPCode) ? new string(' ', 5) : _merchantZIPCode.PadRight(5, ' '); }
        //    set { _merchantZIPCode = value; }
        //}

        /// <summary>
        /// Merchant State Province Code
        /// Length = 3 char
        /// Default: 3 x Char(' ')
        /// Position: 124-126
        /// </summary>
        //private string _merchantStateProvinceCode = string.Empty;
        public string MerchantStateProvinceCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_merchantStateProvinceCode) ? new string(' ', 3) : _merchantStateProvinceCode.PadRight(3, ' '); }
        //    set { _merchantStateProvinceCode = value; }
        //}

        /// <summary>
        /// Issuer Control Number
        /// Length = 9 char
        /// Default: 9 x Char(' ')
        /// Position: 127-135
        /// </summary>
        //private string _issuerControlNumber = string.Empty;
        public string IssuerControlNumber { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_issuerControlNumber) ? new string(' ', 9) : _issuerControlNumber.PadRight(9, ' '); }
        //    set { _issuerControlNumber = value; }
        //}

        /// <summary>
        /// Request Reason Code
        /// Length = 2 char
        /// Default: 2 x Char(' ')
        /// Position: 136-137
        /// </summary>
        //private string _requestReasonCode = string.Empty;
        public string RequestReasonCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_requestReasonCode) ? new string(' ', 2) : _requestReasonCode.PadRight(2, ' '); }
        //    set { _requestReasonCode = value; }
        //}

        /// <summary>
        /// Settlement Flag
        /// Length = 1 char
        /// Default: 1 x Char(' ')
        /// Position: 138
        /// </summary>
        //private string _settlementFlag = string.Empty;
        public string SettlementFlag { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_settlementFlag) ? new string(' ', 1) : _settlementFlag.PadRight(1, ' '); }
        //    set { _settlementFlag = value; }
        //}

        /// <summary>
        /// National Reimbursement Fee
        /// Length = 12 char
        /// Default: 12 x Char(' ')
        /// Position: 139-150
        /// </summary>
        //private string _nationalReimbursementFee = string.Empty;
        public string NationalReimbursementFee { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_nationalReimbursementFee) ? new string(' ', 12) : _nationalReimbursementFee.PadRight(12, ' '); }
        //    set { _nationalReimbursementFee = value; }
        //}

        /// <summary>
        /// Account Selection
        /// Length = 1 char
        /// Default: 1 x Char(' ')
        /// Position: 151
        /// </summary>
        //private string _accountSelection = string.Empty;
        public string AccountSelection { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_accountSelection) ? new string(' ', 1) : _accountSelection.PadRight(1, ' '); }
        //    set { _accountSelection = value; }
        //}

        /// <summary>
        /// Retrieval Request ID
        /// Length = 12 char
        /// Default: 12 x Char(' ')
        /// Position: 152-163
        /// </summary>
        //private string _retrievalRequestID = string.Empty;
        public string RetrievalRequestID { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_retrievalRequestID) ? new string(' ', 12) : _retrievalRequestID.PadRight(12, ' '); }
        //    set { _retrievalRequestID = value; }
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
        /// Default: 1 x Char('0')
        /// Position: 168
        /// </summary>
        //private string _reimbursementAttribute = string.Empty;
        public string ReimbursementAttribute { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_reimbursementAttribute) ? new string('0', 1) : _reimbursementAttribute.PadLeft(1, '0'); }
        //    set { _reimbursementAttribute = value; }
        //}

        #endregion

        public TC52TCR0(string tcrLine)
        {
            TransactionCode = tcrLine.Substring(0, 2);
            TransactionCodeQualifier = tcrLine.Substring(2, 1);
            TransactionComponentSequenceNumber = tcrLine.Substring(3, 1);
            AccountNumber = tcrLine.Substring(4, 16);
            AccountNumberExtension = tcrLine.Substring(20, 3);
            ARN = tcrLine.Substring(23, 23);
            AcquirerBusinessID = tcrLine.Substring(46, 8);
            PurchaseDate = tcrLine.Substring(54, 4);
            TransactionAmount = tcrLine.Substring(58, 12);
            TransactionCurrencyCode = tcrLine.Substring(70, 3);
            MerchantName = tcrLine.Substring(73, 25);
            MerchantCity = tcrLine.Substring(98, 13);
            MerchantCountryCode = tcrLine.Substring(111, 3);
            MerchantCategoryCode = tcrLine.Substring(114, 4);
            MerchantZIPCode = tcrLine.Substring(118, 5);
            MerchantStateProvinceCode = tcrLine.Substring(123, 3);
            IssuerControlNumber = tcrLine.Substring(126, 9);
            RequestReasonCode = tcrLine.Substring(135, 2);
            SettlementFlag = tcrLine.Substring(137, 1);
            NationalReimbursementFee = tcrLine.Substring(138, 12);
            AccountSelection = tcrLine.Substring(150, 1);
            RetrievalRequestID = tcrLine.Substring(151, 12);
            CentralProcessingDate = tcrLine.Substring(163, 4);
            ReimbursementAttribute = tcrLine.Substring(167, 1);
        }
    }
}
