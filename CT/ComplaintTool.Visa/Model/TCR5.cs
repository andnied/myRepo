namespace ComplaintService.Visa.Model
{
    public class TCR5
    {
        #region Fields
        //private string _transactionCode = string.Empty;
        public string TransactionCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_transactionCode) ? new string(' ', 2) : _transactionCode.PadRight(2, ' '); }
        //    set { _transactionCode = value; }
        //}

        //private string _transactionCodeQualifier;
        public string TransactionCodeQualifier { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_transactionCodeQualifier) ? new string(' ', 1) : _transactionCodeQualifier.PadRight(1, ' '); }
        //    set { _transactionCodeQualifier = value; }
        //}

        //private string _transactionComponentSequenceNumber = string.Empty;
        public string TransactionComponentSequenceNumber { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_transactionComponentSequenceNumber) ? new string(' ', 1) : _transactionComponentSequenceNumber.PadRight(1, ' '); }
        //    set { _transactionComponentSequenceNumber = value; }
        //}

        //private string _transactionIdentifier = string.Empty;
        public string TransactionIdentifier { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_transactionIdentifier) ? new string(' ', 15) : _transactionIdentifier.PadRight(15, ' '); }
        //    set { _transactionIdentifier = value; }
        //}

        //private string _authorizedAmount = string.Empty;
        public string AuthorizedAmount { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_authorizedAmount) ? new string(' ', 12) : _authorizedAmount.PadRight(12, ' '); }
        //    set { _authorizedAmount = value; }
        //}

        //private string _authorizationCurrencyCode = string.Empty;
        public string AuthorizationCurrencyCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_authorizationCurrencyCode) ? new string(' ', 3) : _authorizationCurrencyCode.PadRight(3, ' '); }
        //    set { _authorizationCurrencyCode = value; }
        //}

        //private string _authorizationResponseCode = string.Empty;
        public string AuthorizationResponseCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_authorizationResponseCode) ? new string(' ', 2) : _authorizationResponseCode.PadRight(2, ' '); }
        //    set { _authorizationResponseCode = value; }
        //}

        //private string _validationCode = string.Empty;
        public string ValidationCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_validationCode) ? new string(' ', 4) : _validationCode.PadRight(4, ' '); }
        //    set { _validationCode = value; }
        //}

        //private string _excludedTransactionIdentifierReason = string.Empty;
        public string ExcludedTransactionIdentifierReason { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_excludedTransactionIdentifierReason) ? new string(' ', 1) : _excludedTransactionIdentifierReason.PadRight(1, ' '); }
        //    set { _excludedTransactionIdentifierReason = value; }
        //}

        //private string _CRSProcessingCode = string.Empty;
        public string CRSProcessingCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_CRSProcessingCode) ? new string(' ', 1) : _CRSProcessingCode.PadRight(1, ' '); }
        //    set { _CRSProcessingCode = value; }
        //}

        //private string _chargebackRightsIndicator = string.Empty;
        public string ChargebackRightsIndicator { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_chargebackRightsIndicator) ? new string(' ', 2) : _chargebackRightsIndicator.PadRight(2, ' '); }
        //    set { _chargebackRightsIndicator = value; }
        //}

        //private string _multipleClearingSequenceNumber = string.Empty;
        public string MultipleClearingSequenceNumber { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_multipleClearingSequenceNumber) ? new string(' ', 2) : _multipleClearingSequenceNumber.PadRight(2, ' '); }
        //    set { _multipleClearingSequenceNumber = value; }
        //}

        //private string _multipleClearingSequenceCount = string.Empty;
        public string MultipleClearingSequenceCount { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_multipleClearingSequenceCount) ? new string(' ', 2) : _multipleClearingSequenceCount.PadRight(2, ' '); }
        //    set { _multipleClearingSequenceCount = value; }
        //}

        //private string _marketSpecificAuthorizationDataIndicator = string.Empty;
        public string MarketSpecificAuthorizationDataIndicator { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_marketSpecificAuthorizationDataIndicator) ? new string(' ', 1) : _marketSpecificAuthorizationDataIndicator.PadRight(1, ' '); }
        //    set { _marketSpecificAuthorizationDataIndicator = value; }
        //}

        //private string _totalAuthorizedAmount = string.Empty;
        public string TotalAuthorizedAmount { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_totalAuthorizedAmount) ? new string(' ', 12) : _totalAuthorizedAmount.PadRight(12, ' '); }
        //    set { _totalAuthorizedAmount = value; }
        //}

        //private string _informationIndicator = string.Empty;
        public string InformationIndicator { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_informationIndicator) ? new string(' ', 1) : _informationIndicator.PadRight(1, ' '); }
        //    set { _informationIndicator = value; }
        //}

        //private string _merchantTelephoneNumber = string.Empty;
        public string MerchantTelephoneNumber { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_merchantTelephoneNumber) ? new string(' ', 14) : _merchantTelephoneNumber.PadRight(14, ' '); }
        //    set { _merchantTelephoneNumber = value; }
        //}

        //private string _additionalDataIndicator = string.Empty;
        public string AdditionalDataIndicator { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_additionalDataIndicator) ? new string(' ', 1) : _additionalDataIndicator.PadRight(1, ' '); }
        //    set { _additionalDataIndicator = value; }
        //}

        //private string _merchantVolumeIndicator = string.Empty;
        public string MerchantVolumeIndicator { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_merchantVolumeIndicator) ? new string(' ', 2) : _merchantVolumeIndicator.PadRight(2, ' '); }
        //    set { _merchantVolumeIndicator = value; }
        //}

        //private string _electronicCommerceGoodsIndicator = string.Empty;
        public string ElectronicCommerceGoodsIndicator { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_electronicCommerceGoodsIndicator) ? new string(' ', 2) : _electronicCommerceGoodsIndicator.PadRight(2, ' '); }
        //    set { _electronicCommerceGoodsIndicator = value; }
        //}

        //private string _merchantVerificationValue = string.Empty;
        public string MerchantVerificationValue { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_merchantVerificationValue) ? new string(' ', 10) : _merchantVerificationValue.PadRight(10, ' '); }
        //    set { _merchantVerificationValue = value; }
        //}

        //private string _interchangeFeeAmount = string.Empty;
        public string InterchangeFeeAmount { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_interchangeFeeAmount) ? new string(' ', 15) : _interchangeFeeAmount.PadRight(15, ' '); }
        //    set { _interchangeFeeAmount = value; }
        //}

        //private string _interchangeFeeSign = string.Empty;
        public string InterchangeFeeSign { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_interchangeFeeSign) ? new string(' ', 1) : _interchangeFeeSign.PadRight(1, ' '); }
        //    set { _interchangeFeeSign = value; }
        //}

        //private string _sourceCurrencyToBaseCurrencyExchangeRate = string.Empty;
        public string SourceCurrencyToBaseCurrencyExchangeRate { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_sourceCurrencyToBaseCurrencyExchangeRate) ? new string(' ', 8) : _sourceCurrencyToBaseCurrencyExchangeRate.PadRight(8, ' '); }
        //    set { _sourceCurrencyToBaseCurrencyExchangeRate = value; }
        //}

        //private string _baseCurrencyToDestinationCurrencyExchangeRate = string.Empty;
        public string BaseCurrencyToDestinationCurrencyExchangeRate { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_baseCurrencyToDestinationCurrencyExchangeRate) ? new string(' ', 8) : _baseCurrencyToDestinationCurrencyExchangeRate.PadRight(8, ' '); }
        //    set { _baseCurrencyToDestinationCurrencyExchangeRate = value; }
        //}

        //private string _optionalIssuerISAAmount = string.Empty;
        public string OptionalIssuerISAAmount { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_optionalIssuerISAAmount) ? new string(' ', 12) : _optionalIssuerISAAmount.PadRight(12, ' '); }
        //    set { _optionalIssuerISAAmount = value; }
        //}

        //private string _productID = string.Empty;
        public string ProductID { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_productID) ? new string(' ', 2) : _productID.PadRight(2, ' '); }
        //    set { _productID = value; }
        //}

        //private string _programID = string.Empty;
        public string ProgramID { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_programID) ? new string(' ', 6) : _programID.PadRight(6, ' '); }
        //    set { _programID = value; }
        //}

        //private string _DCCIndicator  = string.Empty;
        public string DCCIndicator { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_DCCIndicator) ? new string(' ', 1) : _DCCIndicator.PadRight(1, ' '); }
        //    set { _DCCIndicator = value; }
        //}

        //private string _accountTypeIdentification = string.Empty;
        public string AccountTypeIdentification { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_accountTypeIdentification) ? new string(' ', 4) : _accountTypeIdentification.PadRight(4, ' '); }
        //    set { _accountTypeIdentification = value; }
        //}

        //private string _reserved1 = string.Empty;
        public string Reserved1 { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_reserved1) ? new string(' ', 19) : _reserved1.PadRight(19, ' '); }
        //    set { _reserved1 = value; }
        //}

        //private string _cvv2ResultCode = string.Empty;
        public string CVV2ResultCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_cvv2ResultCode) ? new string(' ', 1) : _cvv2ResultCode.PadRight(1, ' '); }
        //    set { _cvv2ResultCode = value; }
        //}
        #endregion
        public TCR5(string tcrLine)
        {
            TransactionCode = tcrLine.Substring(0, 2);
            TransactionCodeQualifier = tcrLine.Substring(2, 1);
            TransactionComponentSequenceNumber = tcrLine.Substring(3, 1);
            TransactionIdentifier = tcrLine.Substring(4, 15);
            AuthorizedAmount = tcrLine.Substring(19, 12);
            AuthorizationCurrencyCode = tcrLine.Substring(31, 3);
            AuthorizationResponseCode = tcrLine.Substring(34, 2);
            ValidationCode = tcrLine.Substring(36, 4);
            ExcludedTransactionIdentifierReason = tcrLine.Substring(40, 1);
            CRSProcessingCode = tcrLine.Substring(41, 1);
            ChargebackRightsIndicator = tcrLine.Substring(42, 2);
            MultipleClearingSequenceNumber = tcrLine.Substring(44, 2);
            MultipleClearingSequenceCount = tcrLine.Substring(46, 2);
            MarketSpecificAuthorizationDataIndicator = tcrLine.Substring(48, 1);
            TotalAuthorizedAmount = tcrLine.Substring(49, 12);
            InformationIndicator = tcrLine.Substring(61, 1);
            MerchantTelephoneNumber = tcrLine.Substring(62, 14);
            AdditionalDataIndicator = tcrLine.Substring(76, 1);
            MerchantVolumeIndicator = tcrLine.Substring(77, 2);
            ElectronicCommerceGoodsIndicator = tcrLine.Substring(79, 2);
            MerchantVerificationValue = tcrLine.Substring(81, 10);
            InterchangeFeeAmount = tcrLine.Substring(91, 15);
            InterchangeFeeSign = tcrLine.Substring(106, 1);
            SourceCurrencyToBaseCurrencyExchangeRate = tcrLine.Substring(107, 8);
            BaseCurrencyToDestinationCurrencyExchangeRate = tcrLine.Substring(115, 8);
            OptionalIssuerISAAmount = tcrLine.Substring(123, 12);
            ProductID = tcrLine.Substring(135, 2);
            ProgramID = tcrLine.Substring(137, 6);
            DCCIndicator = tcrLine.Substring(143, 1);
            AccountTypeIdentification = tcrLine.Substring(144, 4);
            Reserved1 = tcrLine.Substring(148, 19);
            CVV2ResultCode = tcrLine.Substring(167, 1);
        }
    }
}
