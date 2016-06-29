namespace ComplaintService.Visa.Model
{
    public class TCR9
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

        //private string _destinationBIN = string.Empty;
        public string DestinationBIN { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_destinationBIN) ? new string(' ', 6) : _destinationBIN.PadRight(6, ' '); }
        //    set { _destinationBIN = value; }
        //}

        //private string _sourceBIN = string.Empty;
        public string SourceBIN { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_sourceBIN) ? new string(' ', 6) : _sourceBIN.PadRight(6, ' '); }
        //    set { _sourceBIN = value; }
        //}

        //private string _originalTransactionCode = string.Empty;
        public string OriginalTransactionCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_originalTransactionCode) ? new string(' ', 2) : _originalTransactionCode.PadRight(2, ' '); }
        //    set { _originalTransactionCode = value; }
        //}

        //private string _originalTransactionCodeQualifier = string.Empty;
        public string OriginalTransactionCodeQualifier { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_originalTransactionCodeQualifier) ? new string(' ', 1) : _originalTransactionCodeQualifier.PadRight(1, ' '); }
        //    set { _originalTransactionCodeQualifier = value; }
        //}

        //private string _originalTransactionComponentSequenceNumber = string.Empty;
        public string OriginalTransactionComponentSequenceNumber { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_originalTransactionComponentSequenceNumber) ? new string(' ', 1) : _originalTransactionComponentSequenceNumber.PadRight(1, ' '); }
        //    set { _originalTransactionComponentSequenceNumber = value; }
        //}

        //private string _sourceBatchDate = string.Empty;
        public string SourceBatchDate { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_sourceBatchDate) ? new string(' ', 5) : _sourceBatchDate.PadRight(5, ' '); }
        //    set { _sourceBatchDate = value; }
        //}

        //private string _sourceBatchNumber = string.Empty;
        public string SourceBatchNumber { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_sourceBatchNumber) ? new string(' ', 6) : _sourceBatchNumber.PadRight(6, ' '); }
        //    set { _sourceBatchNumber = value; }
        //}

        //private string _itemSequenceNumber = string.Empty;
        public string ItemSequenceNumber { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_itemSequenceNumber) ? new string(' ', 4) : _itemSequenceNumber.PadRight(4, ' '); }
        //    set { _itemSequenceNumber = value; }
        //}

        //private string _returnReasonCode1 = string.Empty;
        public string ReturnReasonCode1 { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_returnReasonCode1) ? new string(' ', 3) : _returnReasonCode1.PadRight(3, ' '); }
        //    set { _returnReasonCode1 = value; }
        //}

        //private string _originalSourceAmount = string.Empty;
        public string OriginalSourceAmount { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_originalSourceAmount) ? new string(' ', 12) : _originalSourceAmount.PadRight(12, ' '); }
        //    set { _originalSourceAmount = value; }
        //}

        //private string _originalSourceCurrencyCode = string.Empty;
        public string OriginalSourceCurrencyCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_originalSourceCurrencyCode) ? new string(' ', 3) : _originalSourceCurrencyCode.PadRight(3, ' '); }
        //    set { _originalSourceCurrencyCode = value; }
        //}

        //private string _originalSettlementFlag = string.Empty;
        public string OriginalSettlementFlag { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_originalSettlementFlag) ? new string(' ', 1) : _originalSettlementFlag.PadRight(1, ' '); }
        //    set { _originalSettlementFlag = value; }
        //}

        //private string _crsReturnFlag = string.Empty;
        public string CRSReturnFlag { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_crsReturnFlag) ? new string(' ', 1) : _crsReturnFlag.PadRight(1, ' '); }
        //    set { _crsReturnFlag = value; }
        //}

        //private string _returnReasonCode2 = string.Empty;
        public string ReturnReasonCode2 { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_returnReasonCode2) ? new string(' ', 3) : _returnReasonCode2.PadRight(3, ' '); }
        //    set { _returnReasonCode2 = value; }
        //}

        //private string _returnReasonCode3 = string.Empty;
        public string ReturnReasonCode3 { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_returnReasonCode3) ? new string(' ', 3) : _returnReasonCode3.PadRight(3, ' '); }
        //    set { _returnReasonCode3 = value; }
        //}

        //private string _returnReasonCode4 = string.Empty;
        public string ReturnReasonCode4 { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_returnReasonCode4) ? new string(' ', 3) : _returnReasonCode1.PadRight(4, ' '); }
        //    set { _returnReasonCode4 = value; }
        //}

        //private string _returnReasonCode5 = string.Empty;
        public string ReturnReasonCode5 { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_returnReasonCode5) ? new string(' ', 3) : _returnReasonCode5.PadRight(3, ' '); }
        //    set { _returnReasonCode5 = value; }
        //}

        //private string _reserved = string.Empty;
        public string Reserved { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_reserved) ? new string(' ', 101) : _reserved.PadRight(101, ' '); }
        //    set { _reserved = value; }
        //}
        #endregion
        public TCR9(string tcrLine)
        {
            TransactionCode = tcrLine.Substring(0, 2);
            TransactionCodeQualifier = tcrLine.Substring(2, 1);
            TransactionComponentSequenceNumber = tcrLine.Substring(3, 1);
            DestinationBIN = tcrLine.Substring(4, 6);
            SourceBIN = tcrLine.Substring(10, 6);
            OriginalTransactionCode = tcrLine.Substring(16, 2);
            OriginalTransactionCodeQualifier = tcrLine.Substring(18, 1);
            OriginalTransactionComponentSequenceNumber = tcrLine.Substring(19, 1);
            SourceBatchDate = tcrLine.Substring(20, 5);
            SourceBatchNumber = tcrLine.Substring(25, 6);
            ItemSequenceNumber = tcrLine.Substring(31, 4);
            ReturnReasonCode1 = tcrLine.Substring(35, 3);
            OriginalSourceAmount = tcrLine.Substring(38, 12);
            OriginalSourceCurrencyCode = tcrLine.Substring(50, 3);
            OriginalSettlementFlag = tcrLine.Substring(53, 1);
            CRSReturnFlag = tcrLine.Substring(54, 1);
            ReturnReasonCode2 = tcrLine.Substring(55, 3);
            ReturnReasonCode3 = tcrLine.Substring(58, 3);
            ReturnReasonCode4 = tcrLine.Substring(61, 3);
            ReturnReasonCode5 = tcrLine.Substring(64, 3);
            Reserved = tcrLine.Substring(67, 101);
        }
    }
}
