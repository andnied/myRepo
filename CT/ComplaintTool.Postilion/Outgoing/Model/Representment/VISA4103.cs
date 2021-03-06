﻿using System;
using ComplaintTool.Postilion.Outgoing.Model.Representment.Visa;

namespace ComplaintTool.Postilion.Outgoing.Model.Representment
{
    public class VISA4103 : I4103
    {
        private string _pan = string.Empty;

        public string _2
        {
            get { return _pan; }
            set { _pan = value; }
        }
        //const
        public string _3
        {
            get { return @"910000"; }
        }
        //4	ComplaintValue.StageAmount	12	-
        private string _stageAmount;
        public string _4
        {
            get { return _stageAmount; }
            set { _stageAmount = value; }
        }

        //ComplaintValue.InsertDate
        private DateTime _insertDate;
        public string _7
        {
            get { return _insertDate.ToString("MMddHHmmss"); }
            set { _insertDate = Convert.ToDateTime(value); }
        }

        //ostatnie 6 cyfr z numeru post_tran_id / post_tran.system_trace_audit_nr
        private string _postTranID;
        public string _11
        {
            get { return _postTranID; }
            set { _postTranID = value; }
        }

        //const
        public string _12
        {
            get { return @"000000"; }
        }

        //post_tran.datetime_tran_local
        private DateTime _datetimeTranLocal;
        public string _13
        {
            get { return _datetimeTranLocal.ToString("MMdd"); }
            set { _datetimeTranLocal = Convert.ToDateTime(value); }
        }

        //14	post_tran_cust.expiry_date	4	YYMM	"UWAGA: Wysyłane tylko wtedy gdy post_tran_cust.expiry_date != NULL"
        private string _expiryDate;
        public string _14
        {
            get { return string.IsNullOrEmpty(_expiryDate) ? null : _expiryDate; }
            set { _expiryDate = value; }
        } 
        //18	post_tran_cust.merchant_type	4	-	-
        private string _merchantType;
        public string _18
        {
            get { return string.IsNullOrEmpty(_merchantType) ? null : _merchantType; }
            set { _merchantType = value; }
        }  
        //22	post_tran.pos_entry_mode	3	-
        //post_tran.pos_entry_mode	3
        private string _posEntryMode;
        public string _22
        {
            get { return string.IsNullOrEmpty(_posEntryMode) ? new string('0', 3) : _posEntryMode.PadLeft(3, '0'); }
            set { _posEntryMode = value; }
        }
        //23	post_tran_cust.card_seq_nr	3	
        //post_tran_cust.card_seq_nr	3
        private string _cardSeqNr;
        public string _23
        {
            get { return string.IsNullOrEmpty(_cardSeqNr) ? string.Empty : _cardSeqNr.PadLeft(3, '0'); }
            set { _cardSeqNr = value; }
        }
        //25	post_tran.pos_condition_code	2	
        //post_tran.pos_condition_code	2
        private string _posConditionCode;
        public string _25
        {
            get { return string.IsNullOrEmpty(_posConditionCode) ? new string('0', 2) : _posConditionCode.PadLeft(2, '0'); }
            set { _posConditionCode = value; }
        }        
        //32	const	6	"Set '400748' where position 2-7 of Complaint.ARN = '400748'Set '414848'  where position 2-7 of Complaint.ARN = '414848'"
        private string _bin;
        public string _32
        {
            get { return string.IsNullOrEmpty(_bin) ? null : _bin; }
            set { _bin = value; }
        }  
        //37	post_tran.retrieval_reference_nr	12	-
        //post_tran.RetrievalReferenceNumber 	12
        private string _retrievalReferenceNumber;
        public string _37
        {
            get { return string.IsNullOrEmpty(_retrievalReferenceNumber) ? new string('0', 12) : _retrievalReferenceNumber.PadLeft(12, '0'); }
            set { _retrievalReferenceNumber = value; }
        }
        //38	post_tran.auth_id_rsp	6	
        //post_tran.auth_id_rsp
        private string _authIDRsp;
        public string _38
        {
            get { return string.IsNullOrEmpty(_authIDRsp) ? new string(' ', 6) : _authIDRsp.PadRight(6, ' '); }
            set { _authIDRsp = value; }
        }
        //39	post_tran.rsp_code_rsp	2	-
        //post_tran.rsp_code_rsp 2
        private string _rspCodeRsp;
        public string _39
        {
            get { return string.IsNullOrEmpty(_rspCodeRsp) ? new string('0', 2) : _rspCodeRsp.PadLeft(2, '0'); }
            set { _rspCodeRsp = value; }
        }
        //41	post_tran_cust.terminal_id	8	-
        //post_tran_cust.terminal_id	8
        private string _terminalID;
        public string _41
        {
            get { return string.IsNullOrEmpty(_terminalID) ? string.Empty : _terminalID; }
            set { _terminalID = value; }
        }        
        //42	post_tran_cust.card_acceptor_id_code 	15	
        //post_tran_cust.card_acceptor_id_code 	15
        private string _cardAcceptorIDCode;
        public string _42
        {
            get { return string.IsNullOrEmpty(_cardAcceptorIDCode) ? string.Empty : _cardAcceptorIDCode; }
            set { _cardAcceptorIDCode = value; }
        }  
        //43	post_tran_cust.card_acceptor_name_loc	40	
        //post_tran_cust.card_acceptor_name_loc	40
        private string _cardAcceptorNameLoc;
        public string _43
        {
            get { return string.IsNullOrEmpty(_cardAcceptorNameLoc) ? string.Empty : _cardAcceptorNameLoc.PadRight(40, ' '); }
            set { _cardAcceptorNameLoc = value; }
        }
        //49	ComplaintValue.StageCurrencyCode ze stage'a 2PR	3	-
        //ComplaintValue.StageCurrencyCode	3
        private string _stageCurrencyCode;
        public string _49
        {
            get { return string.IsNullOrEmpty(_stageCurrencyCode) ? new string('0', 3) : _stageCurrencyCode.PadLeft(3, '0'); }
            set { _stageCurrencyCode = value; }
        }
        //50	ComplaintValue.StageCurrencyCode ze stage'a 1CB	3	-
        //ComplaintValue.StageCurrencyCode1CB ze stage'a 1CB	3
        private string _stageCurrencyCode1Cb;
        public string _50
        {
            get { return string.IsNullOrEmpty(_stageCurrencyCode1Cb) ? new string('0', 3) : _stageCurrencyCode1Cb.PadLeft(3, '0'); }
            set { _stageCurrencyCode1Cb = value; }
        }
        //56	Complaint.ReasonCode ze stage'a 2PR	4	
        //Complaint.ReasonCode	4
        private string _reasonCode;
        public string _56
        {
            get { return string.IsNullOrEmpty(_reasonCode) ? new string('0', 4) : _reasonCode.PadLeft(4, '0'); }
            set { _reasonCode = value; }
        }
        //100	const	1	2
        public string _100
        {
            get { return @"2"; }
        }
        //Position 1: post_tran_cust.pos_card_data_input_ability
        //Position 2: post_tran_cust.pos_cardholder_auth_ability
        //Position 3: post_tran_cust.pos_card_capture_ability
        //Position 4: post_tran_cust.pos_operating_environment
        //Position 5: post_tran_cust.pos_cardholder_present
        //Position 6: post_tran_cust.pos_card_present
        //Position 7: post_tran_cust.pos_card_data_input_mode
        //Position 8: post_tran_cust.pos_cardholder_auth_method
        //Position 9: post_tran_cust.pos_cardholder_auth_entity
        //Position 10: post_tran_cust.pos_card_data_output_ability
        //Position 11: post_tran_cust.pos_terminal_output_ability
        //Position 12: post_tran_cust.pos_pin_capture_ability
        //Position 13: post_tran_cust.pos_terminal_operator
        //Position 14-15: post_tran_cust.pos_terminal_type
        private PosDataCode _posDataCode = null;
        public object _123
        {
            get { return _posDataCode.ToString(); }
            set { _posDataCode = (PosDataCode)value; }
        }
        //BaseII	996		
        //AdjustmentComponent	996		
        //ComplaintStage.CaseId	996  16CaseID220ESPLCB20150327400002"
        private MockData _mockData;
        public object _127_022
        {
            get { return _mockData.ToString(); }
            set { _mockData = (MockData)value; }
        }
        //127.033	const	4	4103
        private string _constMsgTypeId;
        public string _127_033
        {
            //get { return @"4103"; }
            get { return string.IsNullOrEmpty(_constMsgTypeId) ? string.Empty : _constMsgTypeId; }
            set { _constMsgTypeId = value; }
        }

        public string GetHeader()
        {
            return @"2;3;4;7;11;12;13;14;18;22;23;25;32;37;38;39;41;42;43;49;50;56;100;123;127.022;127.033";
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
                    + _14 + ";"
                    + _18 + ";"
                    + _22 + ";"
                    + _23 + ";"
                    + _25 + ";"
                    + _32 + ";"
                    + _37 + ";"
                    + _38 + ";"
                    + _39 + ";"
                    + _41 + ";"
                    + _42 + ";"
                    + _43 + ";"
                    + _49 + ";"
                    + _50 + ";"
                    + _56 + ";"
                    + _100 + ";"
                    + _123 + ";"
                    + _127_022 + ";"
                    + _127_033;
        }

        public string GetRecordWhiteBase64String()
        {
            return _2 + ";"
                    + _3 + ";"
                    + _4 + ";"
                    + _7 + ";"
                    + _11 + ";"
                    + _12 + ";"
                    + _13 + ";"
                    + _14 + ";"
                    + _18 + ";"
                    + _22 + ";"
                    + _23 + ";"
                    + _25 + ";"
                    + _32 + ";"
                    + _37 + ";"
                    + _38 + ";"
                    + _39 + ";"
                    + _41 + ";"
                    + _42 + ";"
                    + _43 + ";"
                    + _49 + ";"
                    + _50 + ";"
                    + _56 + ";"
                    + _100 + ";"
                    + _123 + ";"
                    + _mockData.ToBase64String() + ";"
                    + _127_033;
        }
    }
}
