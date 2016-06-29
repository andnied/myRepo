using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace ComplaintTool.Models
{
    public class ChbRecord
    {

        #region Field
        //ItemId
        public Int64 ItemId { get; set; }
        /// <summary>
        /// Record Type 
        /// Data type: A
        /// Lenght = 2 char 
        /// Default: FR
        /// </summary>
        private string _recordType = string.Empty;
        public string RecordType {
            get {
                return string.IsNullOrEmpty(_recordType) ? @"FR" : _recordType;
            }
            set { _recordType = value; }
        }
        /// <summary>
        /// Brand
        /// Data type: A
        /// Lenght = 25 char 
        /// Default: 25 x Char(' ')
        /// </summary>
        private string _brand = string.Empty;
        [RequiredAttribute]
        public string Brand
        {
            get { return string.IsNullOrEmpty(_brand) ? new string(' ',25) : _brand.PadRight(25, ' '); }
            set { _brand = value; }
        }
        /// <summary>
        /// MID = Merchant Number
        /// ATM Transaktionen - AWL VU Nummer 10 stellig  
        /// POS & ECOM Transaktionen - DeuCS VU Nummer 9 bis 15 stellig"
        /// Data type: A
        /// Lenght = 16 char 
        /// Default: 16 x Char(' ')
        /// </summary>
        private string _mId = string.Empty;
        [RequiredAttribute]
        public string Mid
        {
            get { return string.IsNullOrEmpty(_mId) ? new string(' ', 16) : _mId.Trim().PadRight(16, ' '); }
            set { _mId = value; }
        }
        /// <summary>
        /// Narritive
        /// Data type: A
        /// Lenght = 30 char 
        /// Default: 30 x Char(' ')
        /// </summary>
        private string _narritive = string.Empty;
        [RequiredAttribute]
        public string Narritive
        {
            get { return string.IsNullOrEmpty(_narritive) ? new string(' ', 30) : _narritive.Trim().PadRight(30, ' '); }
            set { _narritive = value; }
        }
        /// <summary>
        /// CaseId
        /// Data type: A
        /// Lenght = 20 char 
        /// Default: 20 x Char(' ')
        /// </summary>        
        private string _caseId = string.Empty;
        [RequiredAttribute]
        public string CaseId
        {
            get { return string.IsNullOrEmpty(_caseId) ? new string(' ', 20) : _caseId.PadRight(20, ' '); }
            set { _caseId = value; }
        }

        /// <summary>
        /// PAN
        /// Data type: N
        /// Lenght = 20 char 
        /// Default: 20 x Char('0')
        /// </summary>        
        private string _pan = string.Empty;
        [RequiredAttribute]
        public string Pan
        {
            get { return string.IsNullOrEmpty(_pan) ? new string('0', 20) : _pan.PadLeft(20, '0'); }
            set { _pan = value; }
        }
        /// <summary>
        /// Transaction Amount Sign
        /// Data type: A
        /// Lenght = 1 char 
        /// Default: 1 x Char(' ')
        /// </summary>        
        private string _transactionAmountSign = string.Empty;
        [RequiredAttribute]
        public string TransactionAmountSign
        {
            get { return string.IsNullOrEmpty(_transactionAmountSign) ? new string(' ', 1) : _transactionAmountSign.PadRight(1, ' '); }
            set { _transactionAmountSign = value; }
        }
        /// <summary>
        /// Transaction Amount
        /// Data type: N
        /// Lenght = 12 char 
        /// Default: 12 x Char('0')
        /// </summary>        
        private decimal? _transactionAmount;
        [RequiredAttribute]
        public string TransactionAmount
        {
            get { return !_transactionAmount.HasValue ? new string('0', 12) : _transactionAmount.Value.ToString(CultureInfo.InvariantCulture).PadLeft(12, '0'); }
            set
            {
                decimal decimalValue;
                _transactionAmount = decimal.TryParse(value, out decimalValue) ? decimalValue : 0;
            }
        }
        /// <summary>
        /// Transaction Amount Exponent
        /// Data type: N
        /// Lenght = 1 char 
        /// Default: 1 x Char('0')
        /// </summary>        
        private int? _transactionAmountExponent;
        [RequiredAttribute]
        public string TransactionAmountExponent
        {
            get { return !_transactionAmountExponent.HasValue ? new string('0', 1) : _transactionAmountExponent.Value.ToString(CultureInfo.InvariantCulture).PadLeft(1, '0'); }
            set
            {
                int intValue;
                _transactionAmountExponent = int.TryParse(value, out intValue) ? intValue : 0;
            }
        }
        /// <summary>
        /// Transaction Currency Code
        /// Data type: A
        /// Lenght = 3 char 
        /// Default: 3 x Char(' ')
        /// </summary>        
        private string _transactionCurrencyCode = string.Empty;
        [RequiredAttribute]
        public string TransactionCurrencyCode
        {
            get { return string.IsNullOrEmpty(_transactionCurrencyCode) ? new string(' ', 3) : _transactionCurrencyCode.PadRight(3, ' '); }
            set { _transactionCurrencyCode = value; }
        }
        /// <summary>
        /// Authorisation Code
        /// Data type: A
        /// Lenght = 6 char 
        /// Default: 6 x Char(' ')
        /// </summary>        
        private string _authorisationCode = string.Empty;
        [RequiredAttribute]
        public string AuthorisationCode
        {
            get { return string.IsNullOrEmpty(_authorisationCode) ? new string(' ', 6) : _authorisationCode == "000000" ? new string(' ', 6) : _authorisationCode.PadRight(6, ' '); }
            set { _authorisationCode = value; }
        }
        /// <summary>
        /// Reason Code
        /// Data type: N
        /// Lenght = 4 char 
        /// Default: 4 x Char('0')
        /// </summary>        
        private int? _reasonCode;
        [RequiredAttribute]
        public string ReasonCode
        {
            get { return !_reasonCode.HasValue ? new string('0', 4) : _reasonCode.Value.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0'); }
            set
            {
                if (value.Length > 4) value = value.Substring(0, 4);
                int intValue;
                _reasonCode = int.TryParse(value, out intValue) ? intValue : 0;
            }
        }
        /// <summary>
        /// Transaction Date
        /// format: dd.MM.YYYY hh:mm:ss
        /// Data type: A
        /// Lenght = 19 char 
        /// Default: 19 x Char(' ')
        /// </summary>        
        private DateTime? _transactionDate;
        [RequiredAttribute]
        public string TransactionDate
        {
            get { return !_transactionDate.HasValue ? new string(' ', 19) : _transactionDate.Value.ToString("dd.MM.yyyy HH:mm:ss"); }
            set
            {
                if (value != null)
                _transactionDate = DateTime.Parse(value);
            }
        }
        /// <summary>
        /// Stage Amount Sign
        /// Data type: A
        /// Lenght = 1 char 
        /// Default: 1 x Char(' ')
        /// </summary>        
        private string _stageAmountSign = string.Empty;
        [RequiredAttribute]
        public string StageAmountSign
        {
            get { return string.IsNullOrEmpty(_stageAmountSign) ? new string(' ', 1) : _stageAmountSign.PadRight(1, ' '); }
            set { _stageAmountSign = value; }
        }
        /// <summary>
        /// Stage Amount
        /// Data type: N
        /// Lenght = 12 char 
        /// Default: 12 x Char('0')
        /// </summary>        
        private decimal? _stageAmount;
        [RequiredAttribute]
        public string StageAmount
        {
            get { return !_stageAmount.HasValue ? new string('0', 12) : _stageAmount.Value.ToString(CultureInfo.InvariantCulture).PadLeft(12, '0'); }
            set
            {
                decimal decimalValue;
                _stageAmount = decimal.TryParse(value, out decimalValue) ? decimalValue : 0;
            }
        }
        /// <summary>
        /// Stage Amount Exponent
        /// Data type: N
        /// Lenght = 1 char 
        /// Default: 1 x Char('0')
        /// </summary>        
        private int? _stageAmountExponent;
        [RequiredAttribute]
        public string StageAmountExponent
        {
            get { return !_stageAmountExponent.HasValue ? new string('0', 1) : _stageAmountExponent.Value.ToString(CultureInfo.InvariantCulture).PadLeft(1, '0'); }
            set
            {
                int intValue;
                _stageAmountExponent = int.TryParse(value, out intValue) ? intValue : 0;
            }
        }
        /// <summary>
        /// Stage Currency Code
        /// Data type: A
        /// Lenght = 3 char 
        /// Default: 3 x Char(' ')
        /// </summary>        
        private string _stageCurrencyCode = string.Empty;
        [RequiredAttribute]
        public string StageCurrencyCode
        {
            get { return string.IsNullOrEmpty(_stageCurrencyCode) ? new string(' ', 3) : _stageCurrencyCode.PadRight(3, ' '); }
            set { _stageCurrencyCode = value; }
        }
        /// <summary>
        /// Stage
        /// Data type: A
        /// Lenght = 4 char 
        /// Default: 4 x Char(' ')
        /// </summary>        
        private string _stage = string.Empty;
        [RequiredAttribute]
        public string Stage
        {
            get { return string.IsNullOrEmpty(_stage) ? new string(' ', 4) : _stage.PadRight(4, ' '); }
            set { _stage = value; }
        }
        /// <summary>
        /// Transaction Date
        /// format: dd.MM.YYYY 
        /// Data type: A
        /// Lenght = 10 char 
        /// Default: 10 x Char(' ')
        /// </summary>        
        private DateTime? _stageDate;
        [RequiredAttribute]
        public string StageDate
        {
            get { return !_stageDate.HasValue ? new string(' ', 10) : _stageDate.Value.ToString("dd.MM.yyyy"); }
            set
            {
                if (value != null)
                _stageDate = Convert.ToDateTime(value);
            }
            //DateTime.Parse(value); }
        }
        /// <summary>
        /// ARN
        /// Data type: N
        /// Lenght = 23 char 
        /// Default: 23 x Char(' ')
        /// </summary>        
        private string _arn = string.Empty;
        [RequiredAttribute]
        public string ARN
        {
            get { return string.IsNullOrEmpty(_arn) ? new string('0', 23) : _arn.PadLeft(23, '0'); }
            set { _arn = value; }
        }
        /// <summary>
        /// Booking Amount Sign
        /// Data type: A
        /// Lenght = 1 char 
        /// Default: 1 x Char(' ')
        /// </summary>        
        private string _bookingAmountSign = string.Empty;
        [RequiredAttribute]
        public string BookingAmountSign
        {
            get { return string.IsNullOrEmpty(_bookingAmountSign) ? new string(' ', 1) : _bookingAmountSign.PadRight(1, ' '); }
            set { _bookingAmountSign = value; }
        }
        /// <summary>
        /// Booking Amount
        /// Data type: N
        /// Lenght = 12 char 
        /// Default: 12 x Char('0')
        /// </summary>        
        private decimal? _bookingAmount;
        [RequiredAttribute]
        public string BookingAmount
        {
            get { return !_bookingAmount.HasValue ? new string('0', 12) : _bookingAmount.Value.ToString(CultureInfo.InvariantCulture).PadLeft(12, '0'); }
            set
            {
                decimal decimalValue;
                _bookingAmount = decimal.TryParse(value, out decimalValue) ? decimalValue : 0;
            }
        }
        /// <summary>
        /// Booking Amount Exponent
        /// Data type: N
        /// Lenght = 1 char 
        /// Default: 1 x Char('0')
        /// </summary>        
        private int? _bookingAmountExponent;
        [RequiredAttribute]
        public string BookingAmountExponent
        {
            get { return !_bookingAmountExponent.HasValue ? new string('0', 1) : _bookingAmountExponent.Value.ToString(CultureInfo.InvariantCulture).PadLeft(1, '0'); }
            set
            {
                int intValue;
                _bookingAmountExponent = int.TryParse(value, out intValue) ? intValue : 0;
            }
        }
        /// <summary>
        /// Booking Currency Code
        /// Data type: A
        /// Lenght = 3 char 
        /// Default: 3 x Char(' ')
        /// </summary>        
        private string _bookingCurrencyCode = string.Empty;
        [RequiredAttribute]
        public string BookingCurrencyCode
        {
            get { return string.IsNullOrEmpty(_bookingCurrencyCode) ? new string(' ', 3) : _bookingCurrencyCode.PadRight(3, ' '); }
            set { _bookingCurrencyCode = value; }
        }
        /// <summary>
        /// Transaction Date
        /// format: dd.MM.YYYY 
        /// Data type: A
        /// Lenght = 10 char 
        /// Default: 10 x Char(' ')
        /// </summary>        
        private DateTime? _settlementDate;
        [RequiredAttribute]
        public string SettlementDate
        {
            get { return !_settlementDate.HasValue ? new string(' ', 10) : _settlementDate.Value.ToString("dd.MM.yyyy"); }
            set
            {
                if (value != null)
                _settlementDate = DateTime.Parse(value);
            }
        }
        /// <summary>
        /// CVV Flag
        /// Data type: N
        /// Lenght = 1 char 
        /// Default: 1 x Char('0')
        /// </summary>        
        private int? _cVvFlag;
        [RequiredAttribute]
        public string CvvFlag
        {
            get { return !_cVvFlag.HasValue ? new string('0', 1) : _cVvFlag.Value.ToString(CultureInfo.InvariantCulture).PadLeft(1, '0'); }
            set
            {
                int intValue;
                _cVvFlag = int.TryParse(value, out intValue) ? intValue : 0;
            }
        }
        /// <summary>
        /// CVC Flag
        /// Data type: A
        /// Lenght = 1 char 
        /// Default: 1 x Char(' ')
        /// </summary>        
        private string _cvcFlag = string.Empty;
        [RequiredAttribute]
        public string CvcFlag
        {
            get { return string.IsNullOrEmpty(_cvcFlag) ? new string(' ', 1) : _cvcFlag.PadRight(1, ' '); }
            set { _cvcFlag = value; }
        }
        /// <summary>
        /// Euro Booking Amount Sign
        /// Data type: A
        /// Lenght = 1 char 
        /// Default: 1 x Char(' ')
        /// </summary>        
        private string _euroBookingAmountSign = string.Empty;
        public string EuroBookingAmountSign
        {
            get { return string.IsNullOrEmpty(_euroBookingAmountSign) ? new string(' ', 1) : _euroBookingAmountSign.PadRight(1, ' '); }
            set { _euroBookingAmountSign = value; }
        }
        /// <summary>
        /// Euro Booking Amount
        /// Data type: N
        /// Lenght = 12 char 
        /// Default: 12 x Char('0')
        /// </summary>        
        private decimal? _euroBookingAmount;
        public string EuroBookingAmount
        {
            get { return !_euroBookingAmount.HasValue ? new string('0', 12) : _euroBookingAmount.Value.ToString(CultureInfo.InvariantCulture).PadLeft(12, '0'); }
            set
            {
                decimal decimalValue;
                _euroBookingAmount = decimal.TryParse(value, out decimalValue) ? decimalValue : 0;
            }
        }
        /// <summary>
        /// Euro Booking Amount Exponent
        /// Data type: N
        /// Lenght = 1 char 
        /// Default: 1 x Char('0')
        /// </summary>        
        private int? _euroBookingAmountExponent;
        public string EuroBookingAmountExponent
        {
            get { return !_euroBookingAmountExponent.HasValue ? new string('0', 1) : _euroBookingAmountExponent.Value.ToString(CultureInfo.InvariantCulture).PadLeft(1, '0'); }
            set
            {
                int intValue;
                _euroBookingAmountExponent = int.TryParse(value, out intValue) ? intValue : 0;
            }
        }
        /// <summary>
        /// Member Message Text
        /// Data type: A
        /// Lenght = 256 char 
        /// Default: 256 x Char(' ')
        /// </summary>        
        private string _memberMessageText = string.Empty;
        [RequiredAttribute]
        public string MemberMessageText
        {
            get { return string.IsNullOrEmpty(_memberMessageText) ? new string(' ', 256) : _memberMessageText.PadRight(256, ' '); }
            set { _memberMessageText = value; }
        }
        /// <summary>
        /// KKO CB Reference
        /// Data type: A
        /// Lenght = 10 char 
        /// Default: 10 x Char(' ')
        /// </summary>        
        private string _kkocbReference = string.Empty;
        [RequiredAttribute]
        public string KkocbReference
        {
            get { return string.IsNullOrEmpty(_kkocbReference) ? new string(' ', 10) : _kkocbReference.PadRight(10, ' '); }
            set { _kkocbReference = value; }
        }
        /// <summary>
        /// Transaction ID
        /// Data type: A
        /// Lenght = 15 char 
        /// Default: 15 x Char(' ')
        /// </summary>        
        private string _transactionId = string.Empty;
        public string TransactionId
        {
            get { return string.IsNullOrEmpty(_transactionId) ? new string(' ', 15) : _transactionId.PadRight(15, ' '); }
            set { _transactionId = value; }
        }
        /// <summary>
        /// RR ID
        /// Data type: A
        /// Lenght = 12 char 
        /// Default: 12 x Char(' ')
        /// </summary>        
        private string _rrid = string.Empty;
        public string Rrid
        {
            get { return string.IsNullOrEmpty(_rrid) ? new string(' ', 12) : _rrid.PadRight(12, ' '); }
            set { _rrid = value; }
        }
        /// <summary>
        /// Partial Flag
        /// Data type: A
        /// Lenght = 1 char 
        /// Default: 1 x Char(' ')
        /// </summary>        
        private string _partialFlag = string.Empty;
        [RequiredAttribute]
        public string PartialFlag
        {
            get { return string.IsNullOrEmpty(_partialFlag) ? new string(' ', 1) : _partialFlag.PadRight(1, ' '); }
            set { _partialFlag = value; }
        }
        /// <summary>
        /// Return Reason Code 1
        /// Data type: A
        /// Lenght = 3 char 
        /// Default: 3 x Char(' ')
        /// </summary>        
        private string _returnReasonCode1 = string.Empty;
        public string ReturnReasonCode1
        {
            get { return string.IsNullOrEmpty(_returnReasonCode1) ? new string(' ', 3) : _returnReasonCode1.PadRight(3, ' '); }
            set { _returnReasonCode1 = value; }
        }
        /// <summary>
        /// Return Reason Code 2
        /// Data type: A
        /// Lenght = 3 char 
        /// Default: 3 x Char(' ')
        /// </summary>        
        private string _returnReasonCode2 = string.Empty;
        public string ReturnReasonCode2
        {
            get { return string.IsNullOrEmpty(_returnReasonCode2) ? new string(' ', 3) : _returnReasonCode2.PadRight(3, ' '); }
            set { _returnReasonCode2 = value; }
        }
        /// <summary>
        /// Return Reason Code 3
        /// Data type: A
        /// Lenght = 3 char 
        /// Default: 3 x Char(' ')
        /// </summary>        
        private string _returnReasonCode3 = string.Empty;
        public string ReturnReasonCode3
        {
            get { return string.IsNullOrEmpty(_returnReasonCode3) ? new string(' ', 3) : _returnReasonCode3.PadRight(3, ' '); }
            set { _returnReasonCode3 = value; }
        }
        /// <summary>
        /// Return Reason Code 4
        /// Data type: A
        /// Lenght = 3 char 
        /// Default: 3 x Char(' ')
        /// </summary>        
        private string _returnReasonCode4 = string.Empty;
        public string ReturnReasonCode4
        {
            get { return string.IsNullOrEmpty(_returnReasonCode4) ? new string(' ', 3) : _returnReasonCode4.PadRight(3, ' '); }
            set { _returnReasonCode4 = value; }
        }
        /// <summary>
        /// Return Reason Code 5
        /// Data type: A
        /// Lenght = 3 char 
        /// Default: 3 x Char(' ')
        /// </summary>        
        private string _returnReasonCode5 = string.Empty;
        public string ReturnReasonCode5
        {
            get { return string.IsNullOrEmpty(_returnReasonCode5) ? new string(' ', 3) : _returnReasonCode5.PadRight(3, ' '); }
            set { _returnReasonCode5 = value; }
        }
        /// <summary>
        /// MPI-Log-Flag
        /// Data type: A
        /// Lenght = 1 char 
        /// Default: 1 x Char(' ')
        /// </summary>        
        private string _mpiLogFlag = string.Empty;
        [RequiredAttribute]
        public string MpiLogFlag
        {
            get { return string.IsNullOrEmpty(_mpiLogFlag) ? new string(' ', 1) : _mpiLogFlag.PadRight(1, ' '); }
            set { _mpiLogFlag = value; }
        }
        #endregion

        public string GetRecordLine()
        {
            return RecordType + 
                   Brand +
                   Mid + 
                   Narritive +
                   CaseId + 
                   Pan + 
                   TransactionAmountSign +
                   TransactionAmount +
                   TransactionAmountExponent +
                   TransactionCurrencyCode +
                   AuthorisationCode +
                   ReasonCode +
                   TransactionDate +
                   StageAmountSign +
                   StageAmount +
                   StageAmountExponent +
                   StageCurrencyCode +
                   Stage +
                   StageDate +
                   ARN +
                   BookingAmountSign +
                   BookingAmount +
                   BookingAmountExponent +
                   BookingCurrencyCode +
                   SettlementDate +
                   CvvFlag +
                   CvcFlag +
                   EuroBookingAmountSign +
                   EuroBookingAmount +
                   EuroBookingAmountExponent +
                   MemberMessageText +
                   KkocbReference +
                   TransactionId +
                   Rrid +
                   PartialFlag +
                   ReturnReasonCode1 +
                   ReturnReasonCode2 +
                   ReturnReasonCode3 +
                   ReturnReasonCode4 +
                   ReturnReasonCode5 +
                   MpiLogFlag;
        }

        public string GetNullProperties()
        {
            StringBuilder sb=new StringBuilder();
            var type=this.GetType();
            foreach(var propInfo in type.GetProperties())
            {
                object data=propInfo.GetValue(this);
                if (data == null || string.IsNullOrWhiteSpace(data.ToString()))
                {
                    if (Attribute.IsDefined(propInfo, typeof(RequiredAttribute)))
                    {
                        sb.Append(propInfo.Name);
                        sb.Append(",");
                    }
                }
            }
            string nullValues = sb.ToString();
            return nullValues.Length>0?nullValues.Substring(0,nullValues.Length-1):null;
        }
    }
}
