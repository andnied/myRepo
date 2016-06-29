using System;

namespace ComplaintTool.Models
{
    public class PostilionData
    {
        #region Fields
        //public string PostID { get; set; }
        public long? PostID { get; set; }
        //public string PostTranCustID { get; set; }
        public long? PostTranCustID { get; set; }
        //public string TranAmountRsp { get; set; }
        public decimal? TranAmountRsp { get; set; }
        public string TranCurrencyCode { get; set; }
        //public string DatetimeTranLocal { get; set; }
        public DateTime? DatetimeTranLocal { get; set; }
        public string StructuredDataReq { get; set; }
        public string CardVerificationResult { get; set; }
        public string PosTerminalType { get; set; }
        public string AlphaCode { get; set; }
        public string CurrencyCode { get; set; }
        //public string NrDecimals { get; set; }
        public int? NrDecimals { get; set; }
        public string Name { get; set; }
        public string TranType { get; set; }
        public string ExtendedTranType { get; set; }
        public string PrimaryFileReference { get; set; }
        public string MessageType { get; set; }
        public string CardAcceptorIDCode { get; set; }
        public string PanReference { get; set; }
        public string AuthIDRsp { get; set; }
        public string UcafData { get; set; }
        //public string DatetimeRsp { get; set; }
        public DateTime? DatetimeRsp { get; set; }
        public string TerminalID { get; set; }
        public string PosCardPresent { get; set; }
        public string PosCardholderPresent { get; set; }
        public string PosCardDataInputMode { get; set; }
        public string ExpiryDate { get; set; }
        public string PosCardholderAuthEntity { get; set; }
        public string PosCardDataOutputAbility { get; set; }
        public string PAN { get; set; }
        public string PANEncrypted { get; set; }
        public string MerchantType { get; set; }
        public string CardAcceptorNameLoc { get; set; }
        public string POSEntryMode { get; set; }
        public string POSCardholderAuthMethod { get; set; }
        //public string SettleAmountImpact { get; set; }
        public decimal? SettleAmountImpact { get; set; }
        //public string TranAmountReq { get; set; }
        public decimal? TranAmountReq { get; set; }
        //public string PrevPostTranId { get; set; }
        public int? PrevPostTranId { get; set; }
        public string PosConditionCode { get; set; }
        public string RetrievalReferenceNr { get; set; }
        public string RspCodeRsp { get; set; }
        //public string TranNr { get; set; }
        public long? TranNr { get; set; }
        public string CardSeqNr { get; set; }
        public string ServiceRestrictionCode { get; set; }
        public string PosCardDataInputAbility { get; set; }
        public string PosCardholderAuthAbility { get; set; }
        public string PosCardCaptureAbility { get; set; }
        public string PosOperatingEnvironment { get; set; }
        public string PosTerminalOutputAbility { get; set; }
        public string PosPinCaptureAbility { get; set; }
        public string PosTerminalOperator { get; set; }
        public string SystemTraceAuditNr { get; set; }
        public string PrevMessageType { get; set; }
        public string PrevUcafData { get; set; }
        public string PrevPosEntryMode { get; set; }
        public string PrevPosCardPresent { get; set; }
        public string PtPosCardInputMode { get; set; }
        public string PrevPtPosCardInputMode { get; set; }
        public string ParticipantId { get; set; }

        #endregion
        public override string ToString()
        {
            var ret = "";
            var properties = GetType().GetProperties();
            foreach (var pi in properties)
                ret += string.Format("{0} = {1} | ", pi.Name, pi.GetValue(this, null));
            return ret;
        }

        public string GetStructuredDataValue(string fieldName)
        {
            var start = 0;
            const int lengthSub = 1;
            while (start < StructuredDataReq.Length)
            {
                var lengthOfLengthName = int.Parse(StructuredDataReq.Substring(start, lengthSub));
                var length = int.Parse(StructuredDataReq.Substring(start + lengthSub, lengthOfLengthName));
                var name = StructuredDataReq.Substring(start + lengthSub + lengthOfLengthName, length);
                var lengthOfLengthField = int.Parse(StructuredDataReq.Substring(start + lengthSub + lengthOfLengthName + length, lengthSub));
                var lengthField = int.Parse(StructuredDataReq.Substring(start + lengthSub + lengthOfLengthName + length + lengthSub, lengthOfLengthField));
                var field = StructuredDataReq.Substring(start + lengthSub + lengthOfLengthName + length + lengthSub + lengthOfLengthField, lengthField);
                if (name.Equals(fieldName))
                    return field;
                start = start + lengthSub + lengthOfLengthName + length + lengthSub + lengthOfLengthField + lengthField;
            }
            return " ";
        }

        public static string getYY(string month)
        {
            var dateTime = DateTime.Now;
            if (month.Length != 2)
                return " ";
            var postMM = int.Parse(dateTime.ToString("MM"));
            var postYY = int.Parse(dateTime.ToString("yy"));
            var tranMM = int.Parse(month);
            var tranYY = 0;
            if (postMM < 11)
            {
                if (tranMM <= postMM + 1)
                    tranYY = postYY;
                else
                    tranYY = postYY - 1;
            }
            else if (postMM == 11)
                tranYY = postYY;
            else if (tranMM == 1)
                tranYY = postYY + 1;
            else
                tranYY = postYY;
            tranYY = (tranYY + 100) % 100;

            return tranYY.ToString("00");
        }

    }
}
