using ComplaintService.Visa.Model;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Visa.Model
{
    public class IncomingRecordVisa
    {
        #region Fields
        public string BIN { get; set; }
        public string IncomingDate { get; set; }
        public string SettlementDate { get; set; }
        public string TransactionCode { get; set; }
        public string PAN { get; set; }
        public string PANExtention { get; set; }
        public string ARN { get; set; }
        public string TransactionDate { get; set; }
        public string OriginalTransactionAmount { get; set; }
        public string OriginalTransactionCurrencyCode { get; set; }
        public string BookingAmountSign { get; set; }
        public string BookingAmount { get; set; }
        public string BookingAmountExponent { get; set; }
        public string BookingCurrencyCode { get; set; }
        public string StageAmountSign { get; set; }
        public string StageAmount { get; set; }
        public string StageAmountExponent { get; set; }
        public string StageCurrencyCode { get; set; }
        public string MerchantName { get; set; }
        public string MerchantCity { get; set; }
        public string MerchantCountryCode { get; set; }
        public string MCC { get; set; }
        public string UsageCode { get; set; }
        public string RrId { get; set; }
        public string CentralProcessingDate { get; set; }
        public string TransactionId { get; set; }
        public string AuthorisationCode { get; set; }
        public string KkoCbReference { get; set; }
        public string TID { get; set; }
        public string MOTOECI { get; set; }
        public string DestinationBIN { get; set; }
        public string SourceBIN { get; set; }
        public string ReasonCode { get; set; }
        public string MemberMessageText { get; set; }
        public string MessageText { get; set; }
        public string PartialFlag { get; set; }
        public string ReturnReasonCode1 { get; set; }
        public string ReturnReasonCode2 { get; set; }
        public string ReturnReasonCode3 { get; set; }
        public string ReturnReasonCode4 { get; set; }
        public string ReturnReasonCode5 { get; set; }
        public string CaseId { get; set; }
        public string Brand { get; set; }
        public string Narritive { get; set; }
        public string TransactionAmountSign { get; set; }
        public string TransactionAmount { get; set; }
        public string TransactionAmountExponent { get; set; }
        public string TransactionCurrencyCode { get; set; }
        public string Stage { get; set; }
        public string StageDate { get; set; }
        public string CVVFlag { get; set; }
        public string CVCFlag { get; set; }
        public string ECommerce { get; set; }
        public string MPILogFlag { get; set; }
        public string DocumentationIndicator { get; set; }
        public string GiccMCC { get; set; }
        public string GiccDomesticMCC { get; set; }
        public string PostID { get; set; }
        public string CardAcceptorIDCode { get; set; }
        public string TransactionDateTimeLocal { get; set; }
        public string MessageType { get; set; }
        public string MaskedPAN { get; set; }
        public string EncryptedPAN { get; set; }
        public string POSEntryMode { get; set; }
        public string PosTerminalType { get; set; }
        public string PosCardDataInputMode { get; set; }
        public string PosCardPresent { get; set; }
        public string PosCardholderPresent { get; set; }
        public string PosCardholderAuthMethod { get; set; }
        public string GiccRevDate { get; set; }
        public string ExpiryDate { get; set; }
        public string SettleAmountImpact { get; set; }
        public string TranAmountReq { get; set; }
        public string PrevPostTranId { get; set; }
        public string MultipleClearingSeqNr { get; set; }
        public string MultipleClearingSeqCnt { get; set; }
        public string AuthorizationSourceCode { get; set; }
        public string AVSResponseCode { get; set; }
        public string MarketSpecAuth { get; set; }
        public string AuthorizationResponseCode { get; set; }
        public string PosCardholderAuthEntity { get; set; }
        public string PosCardDataOutputAbility { get; set; }
        //,@SystemTraceAuditNr nvarchar(6)
        public string SystemTraceAuditNr { get; set; }
        //,@DatetimeTranLocal datetimeoffset(7)
        public string DatetimeTranLocal { get; set; }
        //,@MerchantType nvarchar(4)
        public string MerchantType { get; set; }
        //,@CardSeqNr nvarchar(3)
        public string CardSeqNr { get; set; }
        //,@PosConditionCode nvarchar(2)
        public string PosConditionCode { get; set; }
        //,@RetrievalReferenceNr nvarchar(12)
        public string RetrievalReferenceNr { get; set; }
        //,@AuthIDRsp nvarchar(10)
        public string AuthIDRsp { get; set; }
        //,@RspCodeRsp nvarchar(6)
        public string RspCodeRsp { get; set; }
        //,@ServiceRestrictionCode nvarchar(3)
        public string ServiceRestrictionCode { get; set; }
        //,@TerminalID nvarchar(8)
        public string TerminalID { get; set; }
        //,@CardAcceptorNameLoc nvarchar(40)
        public string CardAcceptorNameLoc { get; set; }
        //,@PosCardDataInputAbility nvarchar(1)
        public string PosCardDataInputAbility { get; set; }
        //,@PosCardholderAuthAbility nvarchar(1)
        public string PosCardholderAuthAbility { get; set; }
        //,@PosCardCaptureAbility nvarchar(1)
        public string PosCardCaptureAbility { get; set; }
        //,@PosOperatingEnvironment nvarchar(1)
        public string PosOperatingEnvironment { get; set; }
        //,@PosTerminalOutputAbility nvarchar(1)
        public string PosTerminalOutputAbility { get; set; }
        //,@PosPinCaptureAbility nvarchar(1)
        public string PosPinCaptureAbility { get; set; }
        //,@PosTerminalOperator nvarchar(1)
        public string PosTerminalOperator { get; set; }
        //,@UcafData nvarchar(33)
        public string UcafData { get; set; }
        //,@TranAmountRsp numeric(16,0)
        public string TranAmountRsp { get; set; }
        //,@TranType  nvarchar(2)
        public string TranType { get; set; }

        public DateTime BusinessDate { get; set; }
        public int FileId { get; set; }
        public string SourceCountryCode { get; set; }

        public string PrevMessageType { get; set; }
        public string PrevUcafData { get; set; }
        public string StructuredDataReq { get; set; }
        public DateTime DatetimeRsp { get; set; }
        public string ParticipantId { get; set; }

        #endregion

        private readonly ComplaintUnitOfWork _unitOfWork;

        public IncomingRecordVisa(ComplaintUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddInformation(string line, string tc, string tcr)
        {
            var postilionRepo = _unitOfWork.Repo<PostilionRepo>();

            var searchACodeInPos = false;
            if (tcr.Equals("00"))
            {
                var dateNow = DateTime.Now;
                //IncomingDate = dateNow.ToString("yyyyMMdd");
                StageDate = dateNow.ToString("yyyyMMdd");
                if (tc.Equals("52"))
                {
                    var tcrObject = new TC52TCR0(line);
                    ARN = tcrObject.ARN;
                    BIN = tcrObject.ARN.Substring(1, 6);
                    TransactionDate = tcrObject.PurchaseDate;
                    OriginalTransactionAmount = tcrObject.TransactionAmount;
                    OriginalTransactionCurrencyCode = tcrObject.TransactionCurrencyCode;
                    MerchantName = tcrObject.MerchantName;
                    MerchantCity = tcrObject.MerchantCity;
                    MerchantCountryCode = tcrObject.MerchantCountryCode;
                    MCC = tcrObject.MerchantCategoryCode;
                    RrId = tcrObject.RetrievalRequestID;
                    ReasonCode = tcrObject.RequestReasonCode;
                    PAN = tcrObject.AccountNumber;
                    PANExtention = tcrObject.AccountNumberExtension;
                    TransactionCode = tcrObject.TransactionCode;
                    CentralProcessingDate = tcrObject.CentralProcessingDate;
                }
                if (tc.Equals("10") || tc.Equals("20"))
                {
                    var tcrObject = new TC10TC20TCR0(line);
                    PAN = tcrObject.AccountNumber;
                    PANExtention = tcrObject.AccountNumberExtension;
                    BookingAmount = tcrObject.DestinationAmount;
                    BookingCurrencyCode = tcrObject.DestinationCurrencyCode;
                    StageAmount = tcrObject.SourceAmount;
                    StageCurrencyCode = tcrObject.SourceCurrencyCode;
                    TransactionId = tcrObject.TransactionIdentifier;
                    DestinationBIN = tcrObject.DestinationBIN;
                    SourceBIN = tcrObject.SourceBIN;
                    ReasonCode = tcrObject.ReasonCode;
                    MessageText = tcrObject.MessageText;
                    TransactionCode = tcrObject.TransactionCode;
                    CentralProcessingDate = tcrObject.CentralProcessingDate;
                }
                if (tc.Equals("15") || tc.Equals("16") || tc.Equals("35") || tc.Equals("36") || tc.Equals("01") || tc.Equals("02") || tc.Equals("03"))
                {
                    var tcrObject = new TCR0(line);
                    ARN = tcrObject.ARN;
                    BIN = tcrObject.ARN.Substring(1, 6);
                    TransactionDate = tcrObject.PurchaseDate;
                    BookingAmount = tcrObject.DestinationAmount;
                    BookingCurrencyCode = tcrObject.DestinationCurrencyCode;
                    StageAmount = tcrObject.SourceAmount;
                    StageCurrencyCode = tcrObject.SourceCurrencyCode;
                    MerchantName = tcrObject.MerchantCity;
                    MerchantCity = tcrObject.MerchantCity;
                    MerchantCountryCode = tcrObject.MerchantCountryCode;
                    MCC = tcrObject.MerchantCategoryCode;
                    UsageCode = tcrObject.UsageCode;
                    if (!string.IsNullOrWhiteSpace(tcrObject.AuthorizationCode))
                        AuthorisationCode = tcrObject.AuthorizationCode;
                    else
                        searchACodeInPos = true;
                    ReasonCode = tcrObject.ReasonCode;
                    PAN = tcrObject.AccountNumber;
                    PANExtention = tcrObject.AccountNumberExtension;
                    TransactionCode = tcrObject.TransactionCode;
                    CentralProcessingDate = tcrObject.CentralProcessingDate;
                }
                if (!string.IsNullOrEmpty(ARN))
                {
                    PostilionData data;

                    if (!string.IsNullOrWhiteSpace(PAN))
                    {
                        var panFirst6 = PAN.Substring(0, 6);
                        var panLast4 = PAN.Substring(12, 4);
                        data = postilionRepo.GetPostilionData(ARN, panFirst6, panLast4);
                    }
                    else
                        data = postilionRepo.GetPostilionData(ARN, null, null);

                    if (data != null)
                    {
                        PostID = data.PostID.HasValue ? data.PostID.ToString() : string.Empty;

                        var sign = data.PosTerminalType ?? "";
                        PosTerminalType = sign;
                        if (!string.IsNullOrWhiteSpace(sign) && (sign.Equals("90") || sign.Equals("91") || sign.Equals("92") || sign.Equals("93") ||
                            sign.Equals("94") || sign.Equals("95") || sign.Equals("96")))
                            sign = "Y";
                        else
                            sign = "N";
                        ECommerce = sign;
                        MPILogFlag = sign;

                        var tranType = data.TranType ?? "";
                        var msgType = data.MessageType ?? "";
                        MessageType = data.MessageType;
                        if (!string.IsNullOrWhiteSpace(tranType))
                        {
                            if (((tranType.Equals("00") || tranType.Equals("09")) && (!msgType.Equals("0400") || !msgType.Equals("0420"))) ||
                            (tranType.Equals("20") && (msgType.Equals("0400") || msgType.Equals("0420"))))
                                TransactionAmountSign = "-";
                            if (((tranType.Equals("00") || tranType.Equals("09")) && (msgType.Equals("0400") || msgType.Equals("0420"))) ||
                           (tranType.Equals("20") && (!msgType.Equals("0400") || !msgType.Equals("0420"))))
                                TransactionAmountSign = "+";
                        }

                        CVCFlag = data.CardVerificationResult;
                        CardAcceptorIDCode = data.CardAcceptorIDCode;
                        TransactionCurrencyCode = data.CurrencyCode;
                        TransactionAmountExponent = data.NrDecimals.HasValue ? data.NrDecimals.ToString() : string.Empty;
                        MaskedPAN = data.PAN;
                        EncryptedPAN = data.PANEncrypted;
                        POSEntryMode = data.POSEntryMode;
                        PosCardDataInputMode = data.PosCardDataInputMode;
                        PosCardPresent = data.PosCardPresent;
                        PosCardholderPresent = data.PosCardholderPresent;
                        PosCardholderAuthMethod = data.POSCardholderAuthMethod;
                        ExpiryDate = data.ExpiryDate;
                        SettleAmountImpact = data.SettleAmountImpact.ToString();
                        TranAmountReq = data.TranAmountReq.HasValue ? data.TranAmountReq.ToString() : string.Empty;
                        PrevPostTranId = data.PrevPostTranId.HasValue ? data.PrevPostTranId.ToString() : string.Empty;
                        PosCardholderAuthEntity = data.PosCardholderAuthEntity;
                        PosCardDataOutputAbility = data.PosCardDataOutputAbility;

                        //,@SystemTraceAuditNr nvarchar(6)
                        SystemTraceAuditNr = data.SystemTraceAuditNr;
                        //,@DatetimeTranLocal datetimeoffset(7)
                        DatetimeTranLocal = data.DatetimeTranLocal.HasValue ? data.DatetimeTranLocal.ToString() : string.Empty;
                        //,@MerchantType nvarchar(4)
                        MerchantType = data.MerchantType;
                        //,@CardSeqNr nvarchar(3)
                        CardSeqNr = data.CardSeqNr;
                        //,@PosConditionCode nvarchar(2)
                        PosConditionCode = data.PosConditionCode;
                        //,@RetrievalReferenceNr nvarchar(12)
                        RetrievalReferenceNr = data.RetrievalReferenceNr;
                        //,@AuthIDRsp nvarchar(10)
                        AuthIDRsp = data.AuthIDRsp;
                        //,@RspCodeRsp nvarchar(6)
                        RspCodeRsp = data.RspCodeRsp;
                        //,@ServiceRestrictionCode nvarchar(3)
                        ServiceRestrictionCode = data.ServiceRestrictionCode;
                        //,@TerminalID nvarchar(8)
                        TerminalID = data.TerminalID;
                        //,@CardAcceptorNameLoc nvarchar(40)
                        CardAcceptorNameLoc = data.CardAcceptorNameLoc;
                        //,@PosCardDataInputAbility nvarchar(1)
                        PosCardDataInputAbility = data.PosCardDataInputAbility;
                        //,@PosCardholderAuthAbility nvarchar(1)
                        PosCardholderAuthAbility = data.PosCardholderAuthAbility;
                        //,@PosCardCaptureAbility nvarchar(1)
                        PosCardCaptureAbility = data.PosCardCaptureAbility;
                        //,@PosOperatingEnvironment nvarchar(1)
                        PosOperatingEnvironment = data.PosOperatingEnvironment;
                        //,@PosTerminalOutputAbility nvarchar(1)
                        PosTerminalOutputAbility = data.PosTerminalOutputAbility;
                        //,@PosPinCaptureAbility nvarchar(1)
                        PosPinCaptureAbility = data.PosPinCaptureAbility;
                        //,@PosTerminalOperator nvarchar(1)
                        PosTerminalOperator = data.PosTerminalOperator;
                        //,@UcafData nvarchar(33)
                        UcafData = data.UcafData;
                        //,@TranAmountRsp numeric(16,0)
                        TranAmountRsp = data.TranAmountRsp.HasValue ? data.TranAmountRsp.ToString() : string.Empty;
                        //,@TranType  nvarchar(2)
                        TranType = data.TranType;

                        PrevMessageType = data.PrevMessageType;
                        PrevUcafData = data.PrevUcafData;
                        StructuredDataReq = data.StructuredDataReq;

                        var datetimeRsp = data.DatetimeRsp;
                        DatetimeRsp = Convert.ToDateTime(datetimeRsp);

                        ParticipantId = data.ParticipantId;

                        if (searchACodeInPos || tc.Equals("10") || tc.Equals("20") || tc.Equals("52"))
                            AuthorisationCode = data.AuthIDRsp;

                        if (!string.IsNullOrWhiteSpace(data.StructuredDataReq))
                        {
                            Narritive = data.GetStructuredDataValue("eS:GiccMerchantName");
                            CVVFlag = data.GetStructuredDataValue("eS:GiccCVV2P");
                            GiccMCC = data.GetStructuredDataValue("eS:GiccMCC");
                            GiccDomesticMCC = data.GetStructuredDataValue("eS:GiccDomesticMCC");
                            GiccRevDate = data.GetStructuredDataValue("eS:GiccRevDate");
                        }

                        var tranAmountReq = data.TranAmountReq.ToString();
                        var settleAmountImpact = data.SettleAmountImpact.ToString();

                        if (msgType.Equals("0400") || msgType.Equals("0420"))
                        {
                            if (!string.IsNullOrWhiteSpace(GiccRevDate))
                            {
                                var day = GiccRevDate.Substring(2, 2);
                                var month = GiccRevDate.Substring(0, 2);
                                var year = PostilionData.getYY(month);
                                var hour = GiccRevDate.Substring(4, 2);
                                var minutes = GiccRevDate.Substring(6, 2);
                                var seconds = GiccRevDate.Substring(8, 2);
                                var tranDatetime = "20" + year + "-" + month + "-" + day + " " + hour + ":" + minutes + ":" + seconds + ":000";
                                TransactionDateTimeLocal = Convert.ToDateTime(tranDatetime).ToString("yyyy-MM-dd HH:mm:ss:fff");
                            }

                            if (!string.IsNullOrWhiteSpace(settleAmountImpact) && !settleAmountImpact.Equals("0"))
                                TransactionAmount = settleAmountImpact;
                        }
                        else
                        {
                            TransactionDateTimeLocal = data.DatetimeTranLocal.HasValue ? data.DatetimeTranLocal.Value.ToString("yyyy-MM-dd HH:mm:ss:fff") : string.Empty;
                            TransactionAmount = tranAmountReq;
                        }
                        if ((string.IsNullOrWhiteSpace(TransactionAmount) || TransactionAmount.Equals("0"))
                            && !string.IsNullOrWhiteSpace(PrevPostTranId))
                            TransactionAmount = postilionRepo.GetPreviousTransactionAmount(PrevPostTranId);
                    }
                }
                else
                {
                    MPILogFlag = "N";
                    ECommerce = "N";
                }
            }
            if (tcr.Equals("01"))
            {
                if (tc.Equals("52"))
                {
                    var tcrObject = new TC52TCR1(line);
                    TransactionId = tcrObject.TransactionIdentifier;
                }
                if (tc.Equals("15") || tc.Equals("16") || tc.Equals("35") || tc.Equals("36") || tc.Equals("01") || tc.Equals("02") || tc.Equals("03"))
                {
                    var tcrObject = new TCR1(line);
                    KkoCbReference = tcrObject.ChargebackReferenceNumber;
                    TID = tcrObject.TerminalID;
                    MOTOECI = tcrObject.CommerceAndPaymentIndicator;
                    MemberMessageText = tcrObject.MemberMessageText;
                    PartialFlag = tcrObject.SpecialChargebackIndicator;
                    DocumentationIndicator = tcrObject.DocumentationIndicator;
                    AuthorizationSourceCode = tcrObject.AuthorizationSourceCode;
                    AVSResponseCode = tcrObject.AvsResponseCode;
                }
            }
            if (tcr.Equals("05") && (tc.Equals("15") || tc.Equals("16") || tc.Equals("35") || tc.Equals("36")))
            {
                var tcrObject = new TCR5(line);
                TransactionId = tcrObject.TransactionIdentifier;
                MultipleClearingSeqNr = tcrObject.MultipleClearingSequenceNumber;
                MultipleClearingSeqCnt = tcrObject.MultipleClearingSequenceCount;
                MarketSpecAuth = tcrObject.MarketSpecificAuthorizationDataIndicator;
                AuthorizationResponseCode = tcrObject.AuthorizationResponseCode;
            }
            if (tcr.Equals("09") && (tc.Equals("01") || tc.Equals("02") || tc.Equals("03")))
            {
                var tcrObject = new TCR9(line);
                StageAmount = tcrObject.OriginalSourceAmount;
                StageCurrencyCode = tcrObject.OriginalSourceCurrencyCode;
                DestinationBIN = tcrObject.DestinationBIN;
                SourceBIN = tcrObject.SourceBIN;
                ReturnReasonCode1 = tcrObject.ReturnReasonCode1;
                ReturnReasonCode2 = tcrObject.ReturnReasonCode2;
                ReturnReasonCode3 = tcrObject.ReturnReasonCode3;
                ReturnReasonCode4 = tcrObject.ReturnReasonCode4;
                ReturnReasonCode5 = tcrObject.ReturnReasonCode5;
                TransactionCode = tcrObject.TransactionCode;
            }
            if (PAN.All(x => x.Equals('0')))
                PAN = null;
            var decade = DateTime.Now.Year.ToString().Substring(2, 1);
            BusinessDate = ComplaintTool.Common.Utils.Convert.JulianDateToDateTimeForVisa(decade + CentralProcessingDate);

            if (TransactionAmountExponent != null)
            {
                var amountLength = TransactionAmount.Length;
                var exp = int.Parse(TransactionAmountExponent);
                if (amountLength < exp)
                    TransactionAmount = TransactionAmount.PadLeft(12, '0');
            }
        }

        public override string ToString()
        {
            var ret = "";
            var properties = GetType().GetProperties();
            foreach (PropertyInfo pi in properties)
                ret += string.Format("{0} = {1} | ", pi.Name, pi.GetValue(this, null));
            return ret;
        }
    }
}
