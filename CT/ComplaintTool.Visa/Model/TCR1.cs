namespace ComplaintService.Visa.Model
{
    public class TCR1
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

        //private string _issuerWorkstationBIN = string.Empty;
        public string IssuerWorkstationBIN { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_issuerWorkstationBIN) ? new string(' ', 6) : _issuerWorkstationBIN.PadRight(6, ' '); }
        //    set { _issuerWorkstationBIN = value; }
        //}

        //private string _acquirerWorkstationBIN = string.Empty;
        public string AcquirerWorkstationBIN { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_acquirerWorkstationBIN) ? new string(' ', 6) : _acquirerWorkstationBIN.PadRight(6, ' '); }
        //    set { _acquirerWorkstationBIN = value; }
        //}

        //private string _chargebackReferenceNumber = string.Empty;
        public string ChargebackReferenceNumber { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_chargebackReferenceNumber) ? new string(' ', 6) : _chargebackReferenceNumber.PadRight(6, ' '); }
        //    set { _chargebackReferenceNumber = value; }
        //}

        //private string _documentationIndicator = string.Empty;
        public string DocumentationIndicator { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_documentationIndicator) ? new string(' ', 1) : _documentationIndicator.PadRight(1, ' '); }
        //    set { _documentationIndicator = value; }
        //}

        //private string _memberMessageText = string.Empty;
        public string MemberMessageText { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_memberMessageText) ? new string(' ', 50) : _memberMessageText.PadRight(50, ' '); }
        //    set { _memberMessageText = value; }
        //}

        //private string _specialConditionIndicators = string.Empty;
        public string SpecialConditionIndicators { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_specialConditionIndicators) ? new string(' ', 2) : _specialConditionIndicators.PadRight(2, ' '); }
        //    set { _specialConditionIndicators = value; }
        //}

        //private string _feeProgramIndicator = string.Empty;
        public string FeeProgramIndicator { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_feeProgramIndicator) ? new string(' ', 3) : _feeProgramIndicator.PadRight(3, ' '); }
        //    set { _feeProgramIndicator = value; }
        //}

        //private string _issuerCharge = string.Empty;
        public string IssuerCharge { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_issuerCharge) ? new string(' ', 1) : _issuerCharge.PadRight(1, ' '); }
        //    set { _issuerCharge = value; }
        //}

        //private string _reserved = string.Empty;
        public string Reserved { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_reserved) ? new string(' ', 1) : _reserved.PadRight(1, ' '); }
        //    set { _reserved = value; }
        //}

        //private string _cardAcceptorID = string.Empty;
        public string CardAcceptorID { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_cardAcceptorID) ? new string(' ', 15) : _cardAcceptorID.PadRight(15, ' '); }
        //    set { _cardAcceptorID = value; }
        //}

        //private string _terminalID = string.Empty;
        public string TerminalID { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_terminalID) ? new string(' ', 8) : _terminalID.PadRight(8, ' '); }
        //    set { _terminalID = value; }
        //}

        //private string _nationalReimbursementFee = string.Empty;
        public string NationalReimbursementFee { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_nationalReimbursementFee) ? new string(' ', 12) : _nationalReimbursementFee.PadRight(12, ' '); }
        //    set { _nationalReimbursementFee = value; }
        //}

        //private string _commerceAndPaymentIndicator = string.Empty;
        public string CommerceAndPaymentIndicator { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_commerceAndPaymentIndicator) ? new string(' ', 1) : _commerceAndPaymentIndicator.PadRight(1, ' '); }
        //    set { _commerceAndPaymentIndicator = value; }
        //}

        //private string _specialChargebackIndicator = string.Empty;
        public string SpecialChargebackIndicator { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_specialChargebackIndicator) ? new string(' ', 1) : _specialChargebackIndicator.PadRight(1, ' '); }
        //    set { _specialChargebackIndicator = value; }
        //}

        //private string _interfaceTraceNumber = string.Empty;
        public string InterfaceTraceNumber { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_interfaceTraceNumber) ? new string(' ', 6) : _interfaceTraceNumber.PadRight(6, ' '); }
        //    set { _interfaceTraceNumber = value; }
        //}

        //private string _unattendedAcceptanceTerminalIndicator = string.Empty;
        public string UnattendedAcceptanceTerminalIndicator { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_unattendedAcceptanceTerminalIndicator) ? new string(' ', 1) : _unattendedAcceptanceTerminalIndicator.PadRight(1, ' '); }
        //    set { _unattendedAcceptanceTerminalIndicator = value; }
        //}

        //private string _prepaidCardIndicator = string.Empty;
        public string PrepaidCardIndicator { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_prepaidCardIndicator) ? new string(' ', 1) : _prepaidCardIndicator.PadRight(1, ' '); }
        //    set { _prepaidCardIndicator = value; }
        //}

        //private string _serviceDevelopmentField = string.Empty;
        public string ServiceDevelopmentField { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_serviceDevelopmentField) ? new string(' ', 1) : _serviceDevelopmentField.PadRight(1, ' '); }
        //    set { _serviceDevelopmentField = value; }
        //}

        //private string _avsResponseCode = string.Empty;
        public string AvsResponseCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_avsResponseCode) ? new string(' ', 1) : _avsResponseCode.PadRight(1, ' '); }
        //    set { _avsResponseCode = value; }
        //}

        //private string _authorizationSourceCode = string.Empty;
        public string AuthorizationSourceCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_authorizationSourceCode) ? new string(' ', 1) : _authorizationSourceCode.PadRight(1, ' '); }
        //    set { _authorizationSourceCode = value; }
        //}

        //private string _purchaseIdentifierFormat = string.Empty;
        public string PurchaseIdentifierFormat { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_purchaseIdentifierFormat) ? new string(' ', 1) : _purchaseIdentifierFormat.PadRight(1, ' '); }
        //    set { _purchaseIdentifierFormat = value; }
        //}

        //private string _accountSelection = string.Empty;
        public string AccountSelection { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_accountSelection) ? new string(' ', 1) : _accountSelection.PadRight(1, ' '); }
        //    set { _accountSelection = value; }
        //}

        //private string _installmentPaymentCount = string.Empty;
        public string InstallmentPaymentCount { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_installmentPaymentCount) ? new string(' ', 2) : _installmentPaymentCount.PadRight(2, ' '); }
        //    set { _installmentPaymentCount = value; }
        //}

        //private string _purchaseIdentifier = string.Empty;
        public string PurchaseIdentifier { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_purchaseIdentifier) ? new string(' ', 25) : _purchaseIdentifier.PadRight(25, ' '); }
        //    set { _purchaseIdentifier = value; }
        //}

        //private string _cashback = string.Empty;
        public string Cashback { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_cashback) ? new string(' ', 9) : _cashback.PadRight(9, ' '); }
        //    set { _cashback = value; }
        //}

        //private string _chipConditionCode = string.Empty;
        public string ChipConditionCode { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_chipConditionCode) ? new string(' ', 1) : _chipConditionCode.PadRight(1, ' '); }
        //    set { _chipConditionCode = value; }
        //}

        //private string _posEnvironment = string.Empty;
        public string POSEnvironment { get; set; }
        //{
        //    get { return string.IsNullOrEmpty(_posEnvironment) ? new string(' ', 1) : _posEnvironment.PadRight(1, ' '); }
        //    set { _posEnvironment = value; }
        //}
        #endregion
        public TCR1(string tcrLine)
        {
            TransactionCode = tcrLine.Substring(0, 2);
            TransactionCodeQualifier = tcrLine.Substring(2, 1);
            TransactionComponentSequenceNumber = tcrLine.Substring(3, 1);
            IssuerWorkstationBIN = tcrLine.Substring(4, 6);
            AcquirerWorkstationBIN = tcrLine.Substring(10, 6);
            ChargebackReferenceNumber = tcrLine.Substring(16, 6);
            DocumentationIndicator = tcrLine.Substring(22, 1);
            MemberMessageText = tcrLine.Substring(23, 50);
            SpecialConditionIndicators = tcrLine.Substring(73, 2);
            FeeProgramIndicator = tcrLine.Substring(75, 3);
            IssuerCharge = tcrLine.Substring(78, 1);
            Reserved = tcrLine.Substring(79, 1);
            CardAcceptorID = tcrLine.Substring(80, 15);
            TerminalID = tcrLine.Substring(95, 8);
            NationalReimbursementFee = tcrLine.Substring(103, 12);
            CommerceAndPaymentIndicator = tcrLine.Substring(115, 1);
            SpecialChargebackIndicator = tcrLine.Substring(116, 1);
            InterfaceTraceNumber = tcrLine.Substring(117, 6);
            UnattendedAcceptanceTerminalIndicator = tcrLine.Substring(123, 1);
            PrepaidCardIndicator = tcrLine.Substring(124, 1);
            ServiceDevelopmentField = tcrLine.Substring(125, 1);
            AvsResponseCode = tcrLine.Substring(126, 1);
            AuthorizationSourceCode = tcrLine.Substring(127, 1);
            PurchaseIdentifierFormat = tcrLine.Substring(128, 1);
            AccountSelection = tcrLine.Substring(129, 1);
            InstallmentPaymentCount = tcrLine.Substring(130, 2);
            PurchaseIdentifier = tcrLine.Substring(132, 25);
            Cashback = tcrLine.Substring(157, 9);
            ChipConditionCode = tcrLine.Substring(166, 1);
            POSEnvironment = tcrLine.Substring(167, 1);
            
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
