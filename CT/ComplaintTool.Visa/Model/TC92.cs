namespace ComplaintService.Visa.Model
{
    public class TC92
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

        //private string _BIN = string.Empty;
        public string BIN { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_BIN) ? new string(' ', 6) : _BIN.PadRight(6, ' '); }
        //    set { _BIN = value; }
        //}

        //private string _processingDate = string.Empty;
        public string ProcessingDate { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_processingDate) ? new string(' ', 5) : _processingDate.PadRight(5, ' '); }
        //    set { _processingDate = value; }
        //}

        //private string _destinationAmount = string.Empty;
        public string DestinationAmount { get; set; }
        //{ 
        //    get { return string.IsNullOrEmpty(_destinationAmount) ? new string(' ', 15) : _destinationAmount.PadRight(15, ' '); }
        //    set { _destinationAmount = value; }
        //}

        //private string _numberOfMonetaryTransactions = string.Empty;
        public string NumberOfMonetaryTransactions { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_numberOfMonetaryTransactions) ? new string(' ', 12) : _numberOfMonetaryTransactions.PadRight(12, ' '); }
        //    set { _numberOfMonetaryTransactions = value; }
        //}

        //private string _batchNumber = string.Empty;
        public string BatchNumber { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_batchNumber) ? new string(' ', 6) : _batchNumber.PadRight(6, ' '); }
        //    set { _batchNumber = value; }
        //}

        //private string _numberOfTCRs = string.Empty;
        public string NumberOfTCRs { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_numberOfTCRs) ? new string(' ', 12) : _numberOfTCRs.PadRight(12, ' '); }
        //    set { _numberOfTCRs = value; }
        //}

        //private string _fileContinuationCount = string.Empty;
        public string FileContinuationCount { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_fileContinuationCount) ? new string(' ', 6) : _fileContinuationCount.PadRight(6, ' '); }
        //    set { _fileContinuationCount = value; }
        //}

        //private string _centerBatchID = string.Empty;
        public string CenterBatchID { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_centerBatchID) ? new string(' ', 8) : _centerBatchID.PadRight(8, ' '); }
        //    set { _centerBatchID = value; }
        //}

        //private string _numberOfTransactions = string.Empty;
        public string NumberOfTransactions { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_numberOfTransactions) ? new string(' ', 9) : _numberOfTransactions.PadRight(9, ' '); }
        //    set { _numberOfTransactions = value; }
        //}

        //private string _reserved1 = string.Empty;
        public string Reserved1 { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_reserved1) ? new string(' ', 18) : _reserved1.PadRight(18, ' '); }
        //    set { _reserved1 = value; }
        //}

        //private string _sourceAmount = string.Empty;
        public string SourceAmount { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_sourceAmount) ? new string(' ', 15) : _sourceAmount.PadRight(15, ' '); }
        //    set { _sourceAmount = value; }
        //}

        //private string _reserved2 = string.Empty;
        public string Reserved2 { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_reserved2) ? new string(' ', 15) : _reserved2.PadRight(15, ' '); }
        //    set { _reserved2 = value; }
        //}

        //private string _reserved3 = string.Empty;
        public string Reserved3 { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_reserved3) ? new string(' ', 15) : _reserved3.PadRight(15, ' '); }
        //    set { _reserved3 = value; }
        //}

        //private string _reserved4 = string.Empty;
        public string Reserved4 { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_reserved4) ? new string(' ', 15) : _reserved4.PadRight(15, ' '); }
        //    set { _reserved4 = value; }
        //}

        //private string _reserved5 = string.Empty;
        public string Reserved5 { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_reserved5) ? new string(' ', 7) : _reserved5.PadRight(7, ' '); }
        //    set { _reserved5 = value; }
        //}
        #endregion
        public TC92(string tcLine)
        {
            TransactionCode = tcLine.Substring(0, 2);
            TransactionCodeQualifier = tcLine.Substring(2, 1);
            TransactionComponentSequenceNumber = tcLine.Substring(3,  1);
            BIN = tcLine.Substring(4, 6);
            ProcessingDate = tcLine.Substring(10, 5);
            DestinationAmount = tcLine.Substring(15, 15);
            NumberOfMonetaryTransactions = tcLine.Substring(30, 12);
            BatchNumber = tcLine.Substring(42, 6);
            NumberOfTCRs = tcLine.Substring(48, 12);
            FileContinuationCount = tcLine.Substring(60, 6);
            CenterBatchID = tcLine.Substring(66, 8);
            NumberOfTransactions = tcLine.Substring(74, 9);
            Reserved1 = tcLine.Substring(83, 18);
            SourceAmount = tcLine.Substring(101, 15);
            Reserved2 = tcLine.Substring(116, 15);
            Reserved3 = tcLine.Substring(131, 15);
            Reserved4 = tcLine.Substring(146, 15);
            Reserved5 = tcLine.Substring(161, 7);
        }
    }
}
