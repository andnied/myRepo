using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Visa.Model
{
    public class TC90
    {
        #region Fields
        //private string _transactionCode = string.Empty;
        public string TransactionCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_transactionCode) ? new string(' ', 2) : _transactionCode.PadRight(2, ' '); }
        //    set { _transactionCode = value; }
        //}

        //private string _processingBIN = string.Empty;
        public string ProcessingBIN { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_processingBIN) ? new string(' ', 6) : _processingBIN.PadRight(6, ' '); }
        //    set { _processingBIN = value; }
        //}

        //private string _processingDate = string.Empty;
        public string ProcessingDate { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_processingDate) ? new string(' ', 5) : _processingDate.PadRight(5, ' '); }
        //    set { _processingDate = value; }
        //}

        //private string _reserved1 = string.Empty;
        public string Reserved1 { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_reserved1) ? new string(' ', 6) : _reserved1.PadRight(6, ' '); }
        //    set { _reserved1 = value; }
        //}

        //private string _settlementDate = string.Empty;
        public string SettlementDate { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_settlementDate) ? new string(' ', 5) : _settlementDate.PadRight(5, ' '); }
        //    set { _settlementDate = value; }
        //}

        //private string _reserved2 = string.Empty;
        public string Reserved2 { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_reserved2) ? new string(' ', 2) : _reserved2.PadRight(2, ' '); }
        //    set { _reserved2 = value; }
        //}

        //private string _releaseNumber = string.Empty;
        public string ReleaseNumber { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_releaseNumber) ? new string(' ', 3) : _releaseNumber.PadRight(3, ' '); }
        //    set { _releaseNumber = value; }
        //}

        //private string _testOption = string.Empty;
        public string TestOption { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_testOption) ? new string(' ', 4) : _testOption.PadRight(4, ' '); }
        //    set { _testOption = value; }
        //}

        //private string _reserved3 = string.Empty;
        public string Reserved3 { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_reserved3) ? new string(' ', 29) : _reserved3.PadRight(29, ' '); }
        //    set { _reserved3 = value; }
        //}

        //private string _securityCode = string.Empty;
        public string SecurityCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_securityCode) ? new string(' ', 8) : _securityCode.PadRight(8, ' '); }
        //    set { _securityCode = value; }
        //}

        //private string _reserved4 = string.Empty;
        public string Reserved4 { get; set; }
        //{ 
        //    get { return string.IsNullOrEmpty(_reserved4) ? new string(' ', 6) : _reserved4.PadRight(6, ' '); }
        //    set { _reserved4 = value; }
        //}

        //private string _incomingFileID = string.Empty;
        public string IncomingFileID { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_incomingFileID) ? new string(' ', 3) : _incomingFileID.PadRight(3, ' '); }
        //    set { _incomingFileID = value; }
        //}

        //private string _reserved5 = string.Empty;
        public string Reserved5 { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_reserved5) ? new string(' ', 89) : _reserved5.PadRight(89, ' '); }
        //    set { _reserved5 = value; }
        //}
        #endregion
        public TC90(string tcLine)
        {
            TransactionCode = tcLine.Substring(0, 2);
            ProcessingBIN = tcLine.Substring(2, 6);
            ProcessingDate = tcLine.Substring(8, 5);
            Reserved1 = tcLine.Substring(13, 6);
            SettlementDate = tcLine.Substring(19, 5);
            Reserved2 = tcLine.Substring(24, 2);
            ReleaseNumber = tcLine.Substring(26, 3);
            TestOption = tcLine.Substring(29, 4);
            Reserved3 = tcLine.Substring(33, 29);
            SecurityCode = tcLine.Substring(62, 8);
            Reserved4 = tcLine.Substring(70, 6);
            IncomingFileID = tcLine.Substring(76, 3);
            Reserved5 = tcLine.Substring(79, 89);
        }
    }
}
