using System;
using System.Globalization;

namespace ComplaintTool.Models
{
    public class ClfRecord
    {
        #region Field
        /// <summary>
        /// Date
        /// format: dd.MM.YYYY
        /// Data Type: A
        /// Length = 10 char
        /// Default: 10 x Char(' ')
        /// </summary>
        private DateTime? _date;
        public string Date
        {
            get { return !_date.HasValue ? new String(' ', 10) : _date.Value.ToString("dd.MM.YYYY"); }
            set { _date = DateTime.Parse(value); }
        }

        /// <summary>
        /// Activity Type
        /// Data type: A
        /// Length = 1 char
        /// Default: 1 x Char(' ')
        /// </summary>
        private string _activityType = string.Empty;
        public string ActivityType
        {
            get { return string.IsNullOrEmpty(_activityType) ? new String(' ', 1) : _activityType.PadRight(1, ' '); }
            set { _activityType = value; }
        }

        /// <summary>
        /// Info Acquirer
        /// Data type: A
        /// Length = 255 char
        /// Default: 255 x Char(' ')
        /// </summary>
        private string _infoAcquirer = string.Empty;
        public string InfoAcquirer
        {
            get { return string.IsNullOrEmpty(_infoAcquirer) ? new String(' ', 255) : _infoAcquirer.PadRight(255, ' '); }
            set { _infoAcquirer = value; }
        }

        /// <summary>
        /// Response Code
        /// Data type: A
        /// Length = 2 char
        /// Default: 2 x Char(' ')
        /// </summary>
        private string _responseCode = string.Empty;
        public string ResponseCode
        {
            get { return string.IsNullOrEmpty(_responseCode) ? new String(' ', 2) : _responseCode.PadRight(2, ' '); }
            set { _responseCode = value; }
        }

        /// <summary>
        /// Response Information
        /// Data type: A
        /// Length = 255 char
        /// Default: 255 x Char(' ')
        /// </summary>
        private string _responseInformation = string.Empty;
        public string ResponseInformation
        {
            get { return string.IsNullOrEmpty(_responseInformation) ? new String(' ', 255) : _responseInformation.PadRight(255, ' '); }
            set { _responseInformation = value; }
        }

        /// <summary>
        /// Comments Processor
        /// Data type: A
        /// Length = 255 char
        /// Default: 255 x Char(' ')
        /// </summary>
        private string _commentsProcessor = string.Empty;
        public string CommentsProcessor
        {
            get { return string.IsNullOrEmpty(_commentsProcessor) ? new String(' ', 255) : _commentsProcessor.PadRight(255, ' '); }
            set { _commentsProcessor = value; }
        }

        /// <summary>
        /// Good Delivery Date
        /// format: dd.MM.YYYY
        /// Data Type: A
        /// Length = 10 char
        /// Default: 10 x Char('')
        /// </summary>
        private DateTime? _goodDeliveryDate;
        public string GoodDeliveryDate
        {
            get { return !_goodDeliveryDate.HasValue ? new String(' ', 10) : _goodDeliveryDate.Value.ToString("dd.MM.YYYY"); }
            set { _goodDeliveryDate = DateTime.Parse(value); }
        }

        /// <summary>
        /// Place Holder
        /// Data type: A
        /// Length = 20 char
        /// Default: 20 x Char(' ')
        /// </summary>
        private string _placeHolder = string.Empty;
        public string PlaceHolder
        {
            get { return string.IsNullOrEmpty(_placeHolder) ? new String(' ', 20) : _placeHolder.PadRight(20, ' '); }
            set { _placeHolder = value; }
        }

        /// <summary>
        /// Brand
        /// Data type: A
        /// Length = 25 char
        /// Default: 25 x Char(' ')
        /// </summary>
        private string _brand = string.Empty;
        public string Brand
        {
            get { return string.IsNullOrEmpty(_brand) ? new String(' ', 25) : _brand.PadRight(25, ' '); }
            set { _brand = value; }
        }

        /// <summary>
        /// Stage
        /// Data type: A
        /// Lenght = 4 char 
        /// Default: 4 x Char(' ')
        /// </summary>        
        private string _stage = string.Empty;
        public string Stage
        {
            get { return string.IsNullOrEmpty(_stage) ? new String(' ', 4) : _stage.PadRight(4, ' '); }
            set { _stage = value; }
        }

        /// <summary>
        /// RC
        /// Data type: A
        /// Lenght = 4 char 
        /// Default: 4 x Char(' ')
        /// </summary>        
        private string _rc = string.Empty;
        public string RC
        {
            get { return string.IsNullOrEmpty(_rc) ? new String(' ', 4) : _rc.PadRight(4, ' '); }
            set { _rc = value; }
        }

        /// <summary>
        /// Case Identification
        /// Data type: A
        /// Lenght = 20 char 
        /// Default: 20 x Char(' ')
        /// </summary>        
        private string _caseIdentification = string.Empty;
        public string CaseIdentification
        {
            get { return string.IsNullOrEmpty(_caseIdentification) ? new String(' ', 20) : _caseIdentification.PadRight(20, ' '); }
            set { _caseIdentification = value; }
        }

        /// <summary>
        /// Chargeback Amount
        /// Data type: N
        /// Lenght = 12 char 
        /// Default: 12 x Char('0')
        /// </summary>        
        private decimal? _chargebackAmount;
        public string ChargebackAmount
        {
            get { return !_chargebackAmount.HasValue ? new String('0', 12) : _chargebackAmount.Value.ToString(CultureInfo.InvariantCulture).PadLeft(12, '0'); }
            set { _chargebackAmount = decimal.Parse(value); }
        }

        /// <summary>
        /// MID
        /// Data type: A
        /// Lenght = 16 char 
        /// Default: 16 x Char(' ')
        /// </summary>        
        private string _mid = string.Empty;
        public string MID
        {
            get { return string.IsNullOrEmpty(_mid) ? new String(' ', 16) : _mid.PadRight(16, ' '); }
            set { _mid = value; }
        }

        /// <summary>
        /// Card Number
        /// Data type: N
        /// Lenght = 20 char 
        /// Default: 20 x Char('0')
        /// </summary>        
        private decimal? _cardNumber;
        public string CardNumber
        {
            get { return !_cardNumber.HasValue ? new String('0', 20) : _cardNumber.Value.ToString(CultureInfo.InvariantCulture).PadLeft(20, '0'); }
            set { _cardNumber = decimal.Parse(value); }
        }

        /// <summary>
        /// ARN
        /// Data type: N
        /// Lenght = 23 char 
        /// Default: 23 x Char(' ')
        /// </summary>        
        private string _arn = string.Empty;
        public string ARN
        {
            get { return string.IsNullOrEmpty(_arn) ? new String('0', 23) : _arn.PadLeft(23, '0'); }
            set { _arn = value; }
        }

        /// <summary>
        /// Place Holder 2
        /// Data type: A
        /// Length = 20 char
        /// Default: 20 x Char(' ')
        /// </summary>
        private string _placeHolder2 = string.Empty;
        public string PlaceHolder2
        {
            get { return string.IsNullOrEmpty(_placeHolder2) ? new String(' ', 20) : _placeHolder2.PadRight(20, ' '); }
            set { _placeHolder2 = value; }
        }

        /// <summary>
        /// Refund Date
        /// format: dd.MM.YYYY
        /// Data Type: A
        /// Length = 10 char
        /// Default: 10 x Char(' ')
        /// </summary>
        private DateTime? _refundDate;
        public string RefundDate
        {
            get { return !_refundDate.HasValue ? new String(' ', 10) : _refundDate.Value.ToString("dd.MM.YYYY"); }
            set { _refundDate = DateTime.Parse(value); }
        }

        /// <summary>
        /// Refund Amount
        /// Data type: N
        /// Lenght = 12 char 
        /// Default: 12 x Char('0')
        /// </summary>        
        private decimal? _refundAmount;
        public string RefundAmount
        {
            get { return !_refundAmount.HasValue ? new String('0', 12) : _refundAmount.Value.ToString(CultureInfo.InvariantCulture).PadLeft(12, '0'); }
            set { _refundAmount = decimal.Parse(value); }
        }

        /// <summary>
        /// Refund Currency
        /// Data type: A
        /// Lenght = 3 char 
        /// Default: 3 x Char(' ')
        /// </summary>        
        private string _refundCurrency = string.Empty;
        public string RefundCurrency
        {
            get { return string.IsNullOrEmpty(_refundCurrency) ? new String(' ', 3) : _refundCurrency.PadRight(3, ' '); }
            set { _refundCurrency = value; }
        }

        /// <summary>
        /// Item ID
        /// Data type: A
        /// Lenght = 20 char 
        /// Default: 20 x Char(' ')
        /// </summary>        
        private string _itemId = string.Empty;
        public string ItemId
        {
            get { return string.IsNullOrEmpty(_itemId) ? new String(' ', 20) : _itemId.PadRight(20, ' '); }
            set { _itemId = value; }
        }
        #endregion

        public string GetRecordLine()
        {
            return Date +
                ActivityType +
                InfoAcquirer +
                ResponseCode +
                ResponseInformation +
                CommentsProcessor +
                GoodDeliveryDate +
                PlaceHolder +
                Brand +
                Stage +
                RC +
                CaseIdentification +
                ChargebackAmount +
                MID +
                CardNumber +
                ARN +
                PlaceHolder2 +
                RefundDate +
                RefundAmount +
                RefundCurrency +
                ItemId;
        }
    }
}
