using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Models
{
    public class IncomingRecordVisaMid
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
    }
}
