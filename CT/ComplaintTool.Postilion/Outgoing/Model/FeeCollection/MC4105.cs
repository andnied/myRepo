using System;
using System.Globalization;
using ComplaintTool.Postilion.Outgoing.Model.FeeCollection.MasterCard;

namespace ComplaintTool.Postilion.Outgoing.Model.FeeCollection
{
    public class MC4105 : I4105
    {
        //2	IncomingTranVISA.PAN	IncomingTranMASTERCARD.PAN	16 / 19	-	"UWAGA: W przypadku gdy IncomingTranVISA.PANExtension != '000' należy rozszerzyć do 19 znaków wysyłanie."
        private string _pan = string.Empty;
        public string _2
        {
            get { return _pan; }
            set { _pan = value; }
        }
        //3	const		6	910000	
        private string _processingCode = @"910000";
        public string _3
        {
            get { return _processingCode; }
            set { _processingCode = value; }
        }
        //4	ComplaintValue.StageAmount		12	-	Kwota wprowadzona przez użytkownika
        public string _4 { get; set; }

        //7	ComplaintValue.InsertDate		10	MMDDhhmmss	"Data wprowadzenia reprezentacji.UWAGA: Potrzeba dostosowania formatu."
        private DateTime _insertDate;
        public string _7
        {
            get { return _insertDate.ToString("MMddHHmmss"); }
            set { _insertDate = Convert.ToDateTime(value); }
        }
        //11	counter		6	Rozpoczynamy od 000001	
        private int _counter;
        public string _11
        {
            get { return _counter.ToString(CultureInfo.InvariantCulture).PadLeft(6,'0'); }
            set { _counter = int.Parse(value); }
        }
        //12	ComplaintValue.InsertDate		6	hhmmss	"UWAGA:Konieczność dostosowania formatu.Czas wprowadzenia fee"
        private DateTime _datetimeTranLocal;
        public string _12
        {
            get { return _datetimeTranLocal.ToString("HHmmss"); }
            set { _datetimeTranLocal = Convert.ToDateTime(value); }
        }
        //13	ComplaintValue.InsertDate		4	MMDD	"UWAGA:Konieczność dostosowania formatu.Data wprowadzenia fee"
        public string _13
        {
            get { return _datetimeTranLocal.ToString("MMdd"); }
            set { _datetimeTranLocal = Convert.ToDateTime(value); }
        }
        //22	post_tran.pos_entry_mode		3	-	-
        private string _posEntryMode;
        public string _22
        {
            get { return string.IsNullOrEmpty(_posEntryMode) ? new string('0', 3) : _posEntryMode.PadLeft(3, '0'); }
            set { _posEntryMode = value; }
        }
        //25	post_tran.pos_condition_code		2		
        private string _posConditionCode;
        public string _25
        {
            get { return string.IsNullOrEmpty(_posConditionCode) ? new string('0', 2) : _posConditionCode.PadLeft(2, '0'); }
            set { _posConditionCode = value; }
        }        
        //32	const		6	"For MasterCard:'16035'For Visa:Set '400748' where position 2-7 of Complaint.ARN = '400748'Set '414848'  where position 2-7 of Complaint.ARN = '414848'"	
        private string _ica = @"16035";
        public string _32
        {
            set { _ica = value; }
            get { return _ica; }
        }
        //37	post_tran.retrieval_reference_nr		12	-	
        private string _retrievalReferenceNumber;
        public string _37
        {
            get { return string.IsNullOrEmpty(_retrievalReferenceNumber) ? new string('0', 12) : _retrievalReferenceNumber.PadLeft(12, '0'); }
            set { _retrievalReferenceNumber = value; }
        }
        //39	post_tran.rsp_code_rsp		2	-	
        private string _rspCodeRsp;
        public string _39
        {
            get { return string.IsNullOrEmpty(_rspCodeRsp) ? new string('0', 2) : _rspCodeRsp.PadLeft(2, '0'); }
            set { _rspCodeRsp = value; }
        }
        //41	const		8	-	"Wypełnione spacjami Zostało wysłane pytanie do AŚW czy możemy w ogóle nie wysyłać tego pola"
        private string _cardAcceptorTerminalId = new string(' ', 8);
        public string _41
        {
            get { return _cardAcceptorTerminalId; }
            set { _cardAcceptorTerminalId = value; }
        }
        //42	const		15		"Wypełnione spacjami Zostało wysłane pytanie do AŚW czy możemy w ogóle nie wysyłać tego pola"
        private string _cardAcceptorIdCode = new string(' ', 15);
        public string _42
        {
            get { return _cardAcceptorIdCode; }
            set { _cardAcceptorIdCode = value; }
        }
        //43	const		40		"Wypełnione spacjami Zostało wysłane pytanie do AŚW czy możemy w ogóle nie wysyłać tego pola"
        private string _cardAcceptorNameLocation = new string(' ', 40);
        public string _43
        {
            get { return _cardAcceptorNameLocation; }
            set { _cardAcceptorNameLocation = value; }
        }
        //49	ComplaintValue.StageCurrencyCode		3	-	Waluta wprowadzona przez użytkownika
        private string _stageCurrencyCode;
        public string _49
        {
            get { return string.IsNullOrEmpty(_stageCurrencyCode) ? new string('0', 3) : _stageCurrencyCode.PadLeft(3, '0'); }
            set { _stageCurrencyCode = value; }
        }
        //56	Complaint.ReasonCode		4		Reason code wprowadzony/ wybrany przez użytkownika
        private string _reasonCode;
        public string _56
        {
            get { return string.IsNullOrEmpty(_reasonCode) ? new string('0', 4) : _reasonCode.PadLeft(4, '0'); }
            set { _reasonCode = value; }
        }
        //100	const		1	"For MasterCard:'1'For Visa:'2'"	
        private string _receivingInstitutionIdCode = @"1";
        public string _100
        {
            get { return _receivingInstitutionIdCode; }
            set { _receivingInstitutionIdCode = value; }
        }
        //123	const		15	10000000200	// 000010000000200
        private string _posDataCode = @"000010000000200";
        public string _123
        {
            get { return _posDataCode; }
            set { _posDataCode = value; }
        }
        //127.022		AdjustmentComponent
        private MockData _mockData;
        public object _127_022
        {
            get { return _mockData.ToString(); }
            set { _mockData = (MockData)value; }
        }
        //127.033		const		4 -	4105
        public string _127_033 { get; set; }

        public string GetHeader()
        {
            return @"2;3;4;7;11;12;13;22;25;32;37;39;41;42;43;49;56;100;123;127.022;127.033";
        }

        public string GetRecord()
        {
            return  _2 + ";"
                    + _3 + ";"
                    + _4 + ";"
                    + _7 + ";"
                    + _11 + ";"
                    + _12 + ";"
                    + _13 + ";"
                    + _22 + ";"
                    + _25 + ";"
                    + _32 + ";"
                    + _37 + ";"
                    + _39 + ";"
                    + _41 + ";"
                    + _42 + ";"
                    + _43 + ";"
                    + _49 + ";"
                    + _56 + ";"
                    + _100 + ";"
                    + _123 + ";"
                    + _127_022 + ";"
                    + _127_033;
        }

        public string GetRecordWhiteBase64String()
        {
            return  _2 + ";"
                    + _3 + ";"
                    + _4 + ";"
                    + _7 + ";"
                    + _11 + ";"
                    + _12 + ";"
                    + _13 + ";"
                    + _22 + ";"
                    + _25 + ";"
                    + _32 + ";"
                    + _37 + ";"
                    + _39 + ";"
                    + _41 + ";"
                    + _42 + ";"
                    + _43 + ";"
                    + _49 + ";"
                    + _56 + ";"
                    + _100 + ";"
                    + _123 + ";"
                    + _mockData.ToBase64String() + ";"
                    + _127_033;
        }
    }
}
