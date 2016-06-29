namespace ComplaintService.Visa.Model
{
    public class TCR0
    {
        #region Fields
        /// <summary>
        /// Transaction Code
        /// Positions: 0-2
        /// Length = 2 char
        /// Default: 2 x Char(' ')
        /// </summary>
        //private string _transactionCode =  string.Empty;
        public string TransactionCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_transactionCode) ? new string(' ', 2) : _transactionCode.PadRight(2, ' '); }
        //    set { _transactionCode = value; }
        //}

        /// <summary>
        /// Transaction Code Qualifier
        /// Positions: 2-3
        /// Length = 1 char
        /// Default: 1 x Char(' ')
        /// </summary>
        //private string _transactionCodeQualifier;
        public string TransactionCodeQualifier { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_transactionCodeQualifier) ? new string(' ', 1) : _transactionCodeQualifier.PadRight(1, ' '); }
        //    set { _transactionCodeQualifier = value; }
        //}

        /// <summary>
        /// Transaction Component Sequence Number
        /// Positions: 3-4
        /// Length = 1 char
        /// Default: 1 x Char(' ')
        /// </summary>
        //private string _transactionComponentSequenceNumber = string.Empty;
        public string TransactionComponentSequenceNumber { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_transactionComponentSequenceNumber) ? new string(' ', 1) : _transactionComponentSequenceNumber.PadRight(1, ' '); }
        //    set { _transactionComponentSequenceNumber = value; }
        //}

        //private string _accountNumber = string.Empty;
        public string AccountNumber { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_accountNumber) ? new string('0', 16) : _accountNumber.PadLeft(16, '0'); }
        //    set { _accountNumber = value; }
        //}

        //private string _accountNumberExtension = string.Empty;
        public string AccountNumberExtension { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_accountNumberExtension) ? new string(' ', 3) : _accountNumberExtension.PadRight(3, ' '); }
        //    set { _accountNumberExtension = value; }
        //}

        //private string _floorLimitIndicator = string.Empty;
        public string FloorLimitIndicator { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_floorLimitIndicator) ? new string(' ', 1) : _floorLimitIndicator.PadRight(1, ' '); }
        //    set { _floorLimitIndicator = value; }
        //}

        //private string _CRBExceptionFileIndicator = string.Empty;
        public string CRBExceptionFileIndicator { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_CRBExceptionFileIndicator) ? new string(' ', 1) : _CRBExceptionFileIndicator.PadRight(1, ' '); }
        //    set { _CRBExceptionFileIndicator = value; }
        //}

        //private string _PCASIndicator = string.Empty;
        public string PCASIndicator { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_PCASIndicator) ? new string(' ', 1) : _PCASIndicator.PadRight(1, ' '); }
        //    set { _PCASIndicator = value; }
        //}

        //private string _ARN = string.Empty;
        public string ARN { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_ARN) ? new string('0', 23) : _ARN.PadLeft(23, '0'); }
        //    set { _ARN = value; }
        //}

        //private string _acquirerBusinessID = string.Empty;
        public string AcquirerBusinessID { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_acquirerBusinessID) ? new string(' ', 8) : _acquirerBusinessID.PadRight(8, ' '); }
        //    set { _acquirerBusinessID = value; }
        //}

        //private string _purchaseDate = string.Empty;
        public string PurchaseDate { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_purchaseDate) ? new string(' ', 4) : _purchaseDate.PadRight(4, ' '); }
        //    set { _purchaseDate = value; }
        //}

        //private string _destinationAmount = string.Empty;
        public string DestinationAmount { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_destinationAmount) ? new string(' ', 12) : _destinationAmount.PadRight(12, ' '); }
        //    set { _destinationAmount = value; }
        //}

        //private string _destinationCurrencyCode =  string.Empty;
        public string DestinationCurrencyCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_destinationCurrencyCode) ? new string(' ', 3) : _destinationCurrencyCode.PadRight(3, ' '); }
        //    set { _destinationCurrencyCode = value; }
        //}

        //private string _sourceAmount =  string.Empty;
        public string SourceAmount { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_sourceAmount) ? new string(' ', 12) : _sourceAmount.PadRight(12, ' '); }
        //    set { _sourceAmount = value; }
        //}

        //private string _sourceCurrencyCode = string.Empty;
        public string SourceCurrencyCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_sourceCurrencyCode) ? new string(' ', 3) : _sourceCurrencyCode.PadRight(3, ' '); }
        //    set { _sourceCurrencyCode = value; }
        //}

        //private string _merchantName = string.Empty;
        public string MerchantName { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_merchantName) ? new string(' ', 25) : _merchantName.PadRight(25, ' '); }
        //    set { _merchantName = value; }
        //}

        //private string _merchantCity = string.Empty;
        public string MerchantCity { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_merchantCity) ? new string(' ', 13) : _merchantCity.PadRight(13, ' '); }
        //    set { _merchantCity = value; }
        //}

        //private string _merchantCountryCode = string.Empty;
        public string MerchantCountryCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_merchantCountryCode) ? new string(' ', 3) : _merchantCountryCode.PadRight(3, ' '); }
        //    set { _merchantCountryCode = value; }
        //}

        //private string _merchantCategoryCode = string.Empty;
        public string MerchantCategoryCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_merchantCategoryCode) ? new string(' ', 4) : _merchantCategoryCode.PadRight(4, ' '); }
        //    set { _merchantCategoryCode = value; }
        //}

        //private string _merchantZIPCode = string.Empty;
        public string MerchantZIPCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_merchantZIPCode) ? new string(' ', 5) : _merchantZIPCode.PadRight(5, ' '); }
        //    set { _merchantZIPCode = value; }
        //}

        //private string _merchantStateProvinceCode = string.Empty;
        public string MerchantStateProvinceCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_merchantStateProvinceCode) ? new string(' ', 3) : _merchantStateProvinceCode.PadRight(3, ' '); }
        //    set { _merchantStateProvinceCode = value; }
        //}

        //private string _requestedPaymentService = string.Empty;
        public string RequestedPaymentService { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_requestedPaymentService) ? new string(' ', 1) : _requestedPaymentService.PadRight(1, ' '); }
        //    set { _requestedPaymentService = value; }
        //}

        //private string _reserved1 = string.Empty;
        public string Reserved1 { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_reserved1) ? new string(' ', 1) : _reserved1.PadRight(1, ' '); }
        //    set { _reserved1 = value; }
        //}

        //private string _usageCode = string.Empty;
        public string UsageCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_usageCode) ? new string(' ', 1) : _usageCode.PadRight(1, ' '); }
        //    set { _usageCode = value; }
        //}

        //private string _reasonCode = string.Empty;
        public string ReasonCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_reasonCode) ? new string(' ', 2) : _reasonCode.PadRight(2, ' '); }
        //    set { _reasonCode = value; }
        //}

        //private string _settlementFlag = string.Empty;
        public string SettlementFlag { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_settlementFlag) ? new string(' ', 1) : _settlementFlag.PadRight(1, ' '); }
        //    set { _settlementFlag = value; }
        //}

        //private string _authorizationCharacteristicsIndicator = string.Empty;
        public string AuthorizationCharacteristicsIndicator { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_authorizationCharacteristicsIndicator) ? new string(' ', 1) : _authorizationCharacteristicsIndicator.PadRight(1, ' '); }
        //    set { _authorizationCharacteristicsIndicator = value; }
        //}

        //private string _authorizationCode = string.Empty;
        public string AuthorizationCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_authorizationCode) ? new string(' ', 6) : _authorizationCode.PadRight(6, ' '); }
        //    set { _authorizationCode = value; }
        //}

        //private string _POSTerminalCapability = string.Empty;
        public string POSTerminalCapability { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_POSTerminalCapability) ? new string(' ', 1) : _POSTerminalCapability.PadRight(1, ' '); }
        //    set { _POSTerminalCapability = value; }
        //}

        //private string _internationalFeeIndicator = string.Empty;
        public string InternationalFeeIndicator { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_internationalFeeIndicator) ? new string(' ', 1) : _internationalFeeIndicator.PadRight(1, ' '); }
        //    set { _internationalFeeIndicator = value; }
        //}

        //private string _cardHolderIDMethod = string.Empty;
        public string CardHolderIDMethod { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_cardHolderIDMethod) ? new string(' ', 1) : _cardHolderIDMethod.PadRight(1, ' '); }
        //    set { _cardHolderIDMethod = value; }
        //}

        //private string _collectionOnlyFlag = string.Empty;
        public string CollectionOnlyFlag { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_collectionOnlyFlag) ? new string(' ', 1) : _collectionOnlyFlag.PadRight(1, ' '); }
        //    set { _collectionOnlyFlag = value; }
        //}

        //private string _POSEntryMode = string.Empty;
        public string POSEntryMode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_POSEntryMode) ? new string(' ', 2) : _POSEntryMode.PadRight(2, ' '); }
        //    set { _POSEntryMode = value; }
        //}

        //private string _centralProcessingDate = string.Empty;
        public string CentralProcessingDate { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_centralProcessingDate) ? new string(' ', 4) : _centralProcessingDate.PadRight(4, ' '); }
        //    set { _centralProcessingDate = value; }
        //}

        //private string _reimbursementAttribute = string.Empty;
        public string ReimbursementAttribute { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_reimbursementAttribute) ? new string(' ', 1) : _reimbursementAttribute.PadRight(1, ' '); }
        //    set { _reimbursementAttribute = value; }
        //}
        #endregion
        public TCR0(string tcrLine)
        {
            TransactionCode = tcrLine.Substring(0, 2); 
            TransactionCodeQualifier = tcrLine.Substring(2, 1); 
            TransactionComponentSequenceNumber = tcrLine.Substring(3, 1); 
            AccountNumber = tcrLine.Substring(4, 16); 
            AccountNumberExtension = tcrLine.Substring(20, 3); 
            FloorLimitIndicator = tcrLine.Substring(23, 1); 
            CRBExceptionFileIndicator = tcrLine.Substring(24, 1); 
            PCASIndicator = tcrLine.Substring(25, 1); 
            ARN = tcrLine.Substring(26, 23); 
            AcquirerBusinessID = tcrLine.Substring(49, 8); 
            PurchaseDate = tcrLine.Substring(57, 4); 
            DestinationAmount = tcrLine.Substring(61, 12); 
            DestinationCurrencyCode = tcrLine.Substring(73, 3);
            SourceAmount = tcrLine.Substring(76, 12);
            SourceCurrencyCode = tcrLine.Substring(88, 3);
            MerchantName = tcrLine.Substring(91, 25);
            MerchantCity = tcrLine.Substring(116, 13);
            MerchantCountryCode = tcrLine.Substring(129, 3);
            MerchantCategoryCode = tcrLine.Substring(132, 4);
            MerchantZIPCode = tcrLine.Substring(136, 5);
            MerchantStateProvinceCode = tcrLine.Substring(141, 3);
            RequestedPaymentService = tcrLine.Substring(144, 1);
            Reserved1 = tcrLine.Substring(145, 1);
            UsageCode = tcrLine.Substring(146, 1);
            ReasonCode = tcrLine.Substring(147, 2);
            SettlementFlag = tcrLine.Substring(149, 1);
            AuthorizationCharacteristicsIndicator = tcrLine.Substring(150, 1);
            AuthorizationCode = tcrLine.Substring(151, 6);
            POSTerminalCapability = tcrLine.Substring(157, 1);
            InternationalFeeIndicator = tcrLine.Substring(158, 1);
            CardHolderIDMethod = tcrLine.Substring(159, 1);
            CollectionOnlyFlag = tcrLine.Substring(160, 1);
            POSEntryMode = tcrLine.Substring(161, 2);
            CentralProcessingDate = tcrLine.Substring(163, 4);
            ReimbursementAttribute = tcrLine.Substring(167, 1);

            
            //using (StreamWriter w = File.AppendText("log.txt")) { w.Write(this + "\n"); }
        }

        public override string ToString()
        {
            var ret = "";
            var properties = GetType().GetProperties();
            foreach (var pi in properties)
                ret += string.Format("{0} = {1} | ", pi.Name, pi.GetValue(this, null));
            return ret;
        }
    }
}
