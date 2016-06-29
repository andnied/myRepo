using ComplaintTool.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using ComplaintTool.DataAccess.Utils;

namespace ComplaintTool.DataAccess.Repos
{
    public class AutoRepresentmentRepo : RepositoryBase
    {
        public AutoRepresentmentRepo(DbContext context) : base(context) { }

        public ComplaintRecord UpdateComplaintRecord(ComplaintRecord complaintRecord)
        {
            ComplaintRecord newComplaintRecord = null;
            try
            {
                var complaint = complaintRecord.Complaint;
                var panFirst6 = complaint.PANMask.Substring(0, 6);
                var panLast4 = complaint.PANMask.Substring(12, 4);

                var data = new PostilionRepo(base.GetContext<ComplaintEntities>()).GetPostilionData(complaint.ARN, panFirst6, panLast4);

                if (data == null)
                    throw new Exception("Update.UpdateComplaintRecord Error. Postilion data not found."
                                        + " ARN = " + complaint.ARN
                                        + " PAN = " + complaint.PANMask);

                newComplaintRecord = complaintRecord;

                #region Postilion Data

                newComplaintRecord.PosTerminalType = data.PosTerminalType;
                newComplaintRecord.PosCardDataInputMode = data.PosCardDataInputMode;
                newComplaintRecord.PosCardPresent = data.PosCardPresent;
                newComplaintRecord.PosCardholderPresent = data.PosCardholderPresent;
                newComplaintRecord.PosCardholderAuthMethod = data.POSCardholderAuthMethod;

                var giccRevDate = data.GetStructuredDataValue("eS:GiccRevDate");
                if (!string.IsNullOrWhiteSpace(giccRevDate))
                {
                    var day = giccRevDate.Substring(2, 2);
                    var month = giccRevDate.Substring(0, 2);
                    var year = PostilionData.getYY(month);
                    var hour = giccRevDate.Substring(4, 2);
                    var minutes = giccRevDate.Substring(6, 2);
                    var seconds = giccRevDate.Substring(8, 2);
                    newComplaintRecord.GiccRevDate = DateTime.ParseExact(
                        string.Format("20{0}-{1}-{2} {3}:{4}:{5}",
                            year, month, day, hour, minutes, seconds),
                        "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.CurrentUICulture.DateTimeFormat);
                }

                if (data.SettleAmountImpact.HasValue)
                    newComplaintRecord.SettleAmountImpact = data.SettleAmountImpact.Value;

                if (data.TranAmountReq.HasValue)
                    newComplaintRecord.TranAmountReq = data.TranAmountReq.Value;

                if (data.PrevPostTranId.HasValue)
                    newComplaintRecord.PrevPostTranId = data.PrevPostTranId.Value;

                var giccMerchantName = data.GetStructuredDataValue("eS:GiccMerchantName");
                if (!string.IsNullOrWhiteSpace(giccMerchantName))
                    newComplaintRecord.Narritive = giccMerchantName;

                newComplaintRecord.SystemTraceAuditNr = data.SystemTraceAuditNr;
                newComplaintRecord.DatetimeTranLocal = data.DatetimeTranLocal;
                newComplaintRecord.ExpiryDate = data.ExpiryDate;
                newComplaintRecord.MerchantType = data.MerchantType;
                newComplaintRecord.POSEntryMode = data.POSEntryMode;
                newComplaintRecord.CardSeqNr = data.CardSeqNr;
                newComplaintRecord.PosConditionCode = data.PosConditionCode;
                newComplaintRecord.RetrievalReferenceNr = data.RetrievalReferenceNr;
                newComplaintRecord.AuthIDRsp = data.AuthIDRsp;
                newComplaintRecord.RspCodeRsp = data.RspCodeRsp;
                newComplaintRecord.ServiceRestrictionCode = data.ServiceRestrictionCode;
                newComplaintRecord.TerminalID = data.TerminalID;
                newComplaintRecord.CardAcceptorIDCode = data.CardAcceptorIDCode;
                newComplaintRecord.CardAcceptorNameLoc = data.CardAcceptorNameLoc;
                newComplaintRecord.PosCardDataInputAbility = data.PosCardDataInputAbility;
                newComplaintRecord.PosCardholderAuthAbility = data.PosCardholderAuthAbility;
                newComplaintRecord.PosCardCaptureAbility = data.PosCardCaptureAbility;
                newComplaintRecord.PosOperatingEnvironment = data.PosOperatingEnvironment;
                newComplaintRecord.PosCardholderAuthEntity = data.PosCardholderAuthEntity;
                newComplaintRecord.PosCardDataOutputAbility = data.PosCardDataOutputAbility;
                newComplaintRecord.PosTerminalOutputAbility = data.PosTerminalOutputAbility;
                newComplaintRecord.PosPinCaptureAbility = data.PosPinCaptureAbility;
                newComplaintRecord.PosTerminalOperator = data.PosTerminalOperator;
                newComplaintRecord.UcafData = data.UcafData;
                newComplaintRecord.TranAmountRsp = data.TranAmountRsp.ToString();
                newComplaintRecord.TranType = data.TranType;

                newComplaintRecord.PrevMessageType = data.PrevMessageType;
                newComplaintRecord.PrevUcafData = data.PrevUcafData;
                newComplaintRecord.StructuredDataReq = data.StructuredDataReq;
                var datetimeRsp = data.DatetimeRsp;
                newComplaintRecord.DatetimeRsp = System.Convert.ToDateTime(datetimeRsp);

                #endregion

                //Redmine #18094
                //if (!newComplaintRecord.IncomingId.HasValue)
                //    throw new Exception("Update.UpdateComplaintRecord Error. RecordId = " + recordId +
                //                        " IncomingId is null.");

                if (newComplaintRecord.IncomingId.HasValue)
                {
                    #region Incoming Data

                    if (complaint.OrganizationId == Common.Enum.Organization.MC.ToString())
                    {
                        var incoming = GetDbSet<IncomingTranMASTERCARD>().FirstOrDefault(
                            x => x.IncomingId == newComplaintRecord.IncomingId);
                        if (incoming == null)
                            throw new Exception("Update.UpdateComplaintRecord Error. RecordId = " +
                                                complaintRecord.RecordId +
                                                " IncomingTranMASTERCARD is null. IncomingId = " +
                                                newComplaintRecord.IncomingId);
                        //--incoming MC
                        //@ProcessingCode nvarchar(2)
                        newComplaintRecord.ProcessingCode = incoming.ProcessingCode;
                        //@KKOCbReference nvarchar(10)
                        newComplaintRecord.KKOCbReference = incoming.KKOCbReference;
                        //@TranLifeCycleID nvarchar(16)
                        newComplaintRecord.TranLifeCycleID = incoming.TranLifeCycleID;
                        //@IRD nvarchar(2)
                        newComplaintRecord.IRD = incoming.IRD;
                        //@AssignedID nvarchar(6)
                        newComplaintRecord.AssignedID = incoming.AssignedID;
                        //@FraudNotificationDate nvarchar(8)
                        newComplaintRecord.FraudNotificationDate = incoming.FraudNotificationDate;
                    }
                    if (complaint.OrganizationId == Common.Enum.Organization.VISA.ToString())
                    {
                        var incoming =
                            GetDbSet<IncomingTranVISA>().FirstOrDefault(
                                x => x.IncomingId == newComplaintRecord.IncomingId);
                        if (incoming == null)
                            throw new Exception("Update.UpdateComplaintRecord Error. RecordId = " +
                                                complaintRecord.RecordId +
                                                " IncomingTranVISAs is null. IncomingId = " +
                                                newComplaintRecord.IncomingId);
                        //--incoming VISA
                        //,@TransactionId nvarchar(15)
                        newComplaintRecord.TransactionId = incoming.TransactionId;
                        //,@MultiClearingSeqNr nvarchar(2)
                        newComplaintRecord.MultiClearingSeqNr = incoming.MultiClearingSeqNr;
                        //,@MultiClearingSeqCnt nvarchar(2)
                        newComplaintRecord.MultiClearingSeqCnt = incoming.MultiClearingSeqCnt;
                        //,@AuthSourceCode nvarchar(1)
                        newComplaintRecord.AuthSourceCode = incoming.AuthSourceCode;
                        //,@AVSRspCode nvarchar(1)
                        newComplaintRecord.AVSRspCode = incoming.AVSRspCode;
                        //,@MarketSpecAuth nvarchar(1)
                        newComplaintRecord.MarketSpecAuth = incoming.MarketSpecAuth;
                        //,@AuthRspCode nvarchar(2)                            
                        newComplaintRecord.AuthRspCode = incoming.AuthRspCode;
                    }

                    #endregion
                }

                newComplaintRecord.InsertMode = 2;
                var windowsIdentity = WindowsIdentity.GetCurrent();
                newComplaintRecord.InsertDate = DateTime.UtcNow;
                newComplaintRecord.InsertUser = windowsIdentity != null
                    ? windowsIdentity.Name
                    : "WindowsIdentity error.";
                GetDbSet<ComplaintRecord>().Add(newComplaintRecord);
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return newComplaintRecord;
        }

        public bool ComplementPostilionData(Complaint complaint, PostilionData data)
        {
            string transAmountSign = null;
            string transAmount = null;
            string transCurrencyCode = null;
            string transDate = null;
            string ecommerce = null;
            string expiryDate = null;
            string transAmountExp = null;


            if (complaint.OrganizationId.Equals(Common.Enum.Organization.VISA.ToString()))
            {
                #region Refill for IncomingTranVISA

                var incomingTranVisa = GetDbSet<IncomingTranVISA>().FirstOrDefault(x => x.CaseId.Equals(complaint.CaseId));

                if (incomingTranVisa != null)
                {
                    //incomingId = incomingTranVisa.IncomingId;
                    incomingTranVisa.PostTranId = data.PostID.HasValue
                        ? data.PostID.ToString()
                        : string.Empty;

                    var sign = data.PosTerminalType ?? "";
                    incomingTranVisa.PosTerminalType = sign;
                    if (!string.IsNullOrWhiteSpace(sign) &&
                        (sign.Equals("90") || sign.Equals("91") || sign.Equals("92") || sign.Equals("93") ||
                         sign.Equals("94") || sign.Equals("95") || sign.Equals("96")))
                        sign = "Y";
                    else
                        sign = "N";
                    incomingTranVisa.ECommerce = sign;
                    incomingTranVisa.MPILogFlag = sign;

                    var tranType = data.TranType ?? "";
                    var msgType = data.MessageType ?? "";
                    //incomingTranVisa.MessageType = data.MessageType;
                    if (!string.IsNullOrWhiteSpace(tranType))
                    {
                        if (((tranType.Equals("00") || tranType.Equals("09")) &&
                             (!msgType.Equals("0400") || !msgType.Equals("0420"))) ||
                            (tranType.Equals("20") && (msgType.Equals("0400") || msgType.Equals("0420"))))
                            incomingTranVisa.TransactionAmountSign = "-";


                        if (((tranType.Equals("00") || tranType.Equals("09")) &&
                             (msgType.Equals("0400") || msgType.Equals("0420"))) ||
                            (tranType.Equals("20") && (!msgType.Equals("0400") || !msgType.Equals("0420"))))
                            incomingTranVisa.TransactionAmountSign = "+";

                    }

                    incomingTranVisa.CVCFlag = data.CardVerificationResult;
                    //incomingTranVisa.CardAcceptorIDCode = data.CardAcceptorIDCode;
                    incomingTranVisa.TransactionCurrencyCode = data.CurrencyCode;
                    incomingTranVisa.TransactionAmountExponent = data.NrDecimals.HasValue
                        ? data.NrDecimals.ToString()
                        : string.Empty;
                    //incomingTranVisa.MaskedPAN = data.PAN;
                    //incomingTranVisa.EncryptedPAN = data.PANEncrypted;
                    incomingTranVisa.POSEntryMode = data.POSEntryMode;
                    incomingTranVisa.PosCardDataImputMode = data.PosCardDataInputMode;
                    incomingTranVisa.PosCardPresent = data.PosCardPresent;
                    incomingTranVisa.PosCardholderPresent = data.PosCardholderPresent;
                    incomingTranVisa.PosCardholderAuthMethod = data.POSCardholderAuthMethod;
                    incomingTranVisa.ExpiryDate = data.ExpiryDate;
                    incomingTranVisa.SettleAmountImpact = data.SettleAmountImpact.HasValue
                        ? data.SettleAmountImpact.ToString()
                        : string.Empty;
                    incomingTranVisa.TranAmountReq = data.TranAmountReq.HasValue
                        ? data.TranAmountReq.ToString()
                        : string.Empty;
                    incomingTranVisa.PrevPostTranId = data.PrevPostTranId.HasValue
                        ? data.PrevPostTranId.ToString()
                        : string.Empty;

                    if (!string.IsNullOrEmpty(incomingTranVisa.ACode))
                        incomingTranVisa.ACode = data.AuthIDRsp;

                    if (!string.IsNullOrWhiteSpace(data.StructuredDataReq))
                    {
                        incomingTranVisa.Narritive = data.GetStructuredDataValue("eS:GiccMerchantName");
                        incomingTranVisa.CVVFlag = data.GetStructuredDataValue("eS:GiccCVV2P");
                        incomingTranVisa.GiccMCC = data.GetStructuredDataValue("eS:GiccMCC");
                        incomingTranVisa.GiccDomesticMCC = data.GetStructuredDataValue("eS:GiccDomesticMCC");
                        incomingTranVisa.GiccRevDate = data.GetStructuredDataValue("eS:GiccRevDate");
                    }

                    var tranAmountReq = data.TranAmountReq.ToString();
                    //if (tranAmountReq != null)
                    //{
                    //    if (tranAmountReq.Contains("-"))
                    //        tranAmountReq = tranAmountReq.Substring(1);
                    //}

                    var settleAmountImpact = data.SettleAmountImpact.ToString();
                    //if (settleAmountImpact != null)
                    //{
                    //    if (settleAmountImpact.Contains("-"))
                    //        settleAmountImpact = settleAmountImpact.Substring(1);
                    //}

                    if (msgType.Equals("0400") || msgType.Equals("0420"))
                    {
                        if (!string.IsNullOrWhiteSpace(incomingTranVisa.GiccRevDate))
                        {
                            var day = incomingTranVisa.GiccRevDate.Substring(2, 2);
                            var month = incomingTranVisa.GiccRevDate.Substring(0, 2);
                            var year = PostilionData.getYY(month);
                            var hour = incomingTranVisa.GiccRevDate.Substring(4, 2);
                            var minutes = incomingTranVisa.GiccRevDate.Substring(6, 2);
                            var seconds = incomingTranVisa.GiccRevDate.Substring(8, 2);
                            var tranDatetime = "20" + year + "-" + month + "-" + day + " " + hour + ":" +
                                               minutes + ":" + seconds + ":000";
                            incomingTranVisa.TransactionDateTimeLocal =
                                System.Convert.ToDateTime(tranDatetime).ToString("yyyy-MM-dd HH:mm:ss:fff");
                        }

                        if (!string.IsNullOrWhiteSpace(settleAmountImpact) &&
                            !settleAmountImpact.Equals("0"))
                            incomingTranVisa.TransactionAmount = settleAmountImpact;
                    }
                    else
                    {
                        incomingTranVisa.TransactionDateTimeLocal = data.DatetimeTranLocal.HasValue
                            ? data.DatetimeTranLocal.Value.ToString("yyyy-MM-dd HH:mm:ss:fff")
                            : string.Empty;
                        incomingTranVisa.TransactionAmount = tranAmountReq;
                    }
                    if ((string.IsNullOrWhiteSpace(incomingTranVisa.TransactionAmount) ||
                         incomingTranVisa.TransactionAmount.Equals("0"))
                        && !string.IsNullOrWhiteSpace(incomingTranVisa.PrevPostTranId))
                        incomingTranVisa.TransactionAmount = new PostilionRepo(base.GetContext<ComplaintEntities>()).GetPreviousTransactionAmount(incomingTranVisa.PrevPostTranId);

                    transAmountSign = incomingTranVisa.TransactionAmountSign;
                    transAmount = incomingTranVisa.TransactionAmount;
                    transCurrencyCode = incomingTranVisa.TransactionCurrencyCode;
                    transDate = incomingTranVisa.TransactionDateTimeLocal;
                    ecommerce = incomingTranVisa.ECommerce;
                    expiryDate = incomingTranVisa.ExpiryDate;
                    transAmountExp = incomingTranVisa.TransactionAmountExponent;


                }

                #endregion
            }
            else
            {
                #region Refill for IncomingTranMASTERCARD



                var incomingTranMasterCard =
                    GetDbSet<IncomingTranMASTERCARD>().FirstOrDefault(x => x.CaseId.Equals(complaint.CaseId));

                if (incomingTranMasterCard != null)
                {
                    //incomingId = incomingTranMasterCard.IncomingId;
                    incomingTranMasterCard.PostTranId = data.PostID.HasValue
                        ? data.PostID.ToString()
                        : string.Empty;

                    var sign = data.PosTerminalType ?? "";
                    incomingTranMasterCard.PosTerminalType = sign;
                    if (!string.IsNullOrWhiteSpace(sign) &&
                        (sign.Equals("90") || sign.Equals("91") || sign.Equals("92") || sign.Equals("93") ||
                         sign.Equals("94") || sign.Equals("95") || sign.Equals("96")))
                        sign = "Y";
                    else
                        sign = "N";
                    incomingTranMasterCard.ECommerce = sign;
                    incomingTranMasterCard.MPILogFlag = sign;

                    var tranType = data.TranType ?? "";
                    var msgType = data.MessageType ?? "";
                    //incomingTranVisa.MessageType = data.MessageType;
                    if (!string.IsNullOrWhiteSpace(tranType))
                    {
                        if (((tranType.Equals("00") || tranType.Equals("09")) &&
                             (!msgType.Equals("0400") || !msgType.Equals("0420"))) ||
                            (tranType.Equals("20") && (msgType.Equals("0400") || msgType.Equals("0420"))))
                            incomingTranMasterCard.TransactionAmountSign = "-";


                        if (((tranType.Equals("00") || tranType.Equals("09")) &&
                             (msgType.Equals("0400") || msgType.Equals("0420"))) ||
                            (tranType.Equals("20") && (!msgType.Equals("0400") || !msgType.Equals("0420"))))
                            incomingTranMasterCard.TransactionAmountSign = "+";
                    }

                    incomingTranMasterCard.CVCFlag = data.CardVerificationResult;
                    //incomingTranVisa.CardAcceptorIDCode = data.CardAcceptorIDCode;
                    incomingTranMasterCard.TransactionCurrencyCode = data.CurrencyCode;
                    incomingTranMasterCard.TransactionAmountExponent = data.NrDecimals.HasValue
                        ? data.NrDecimals.ToString()
                        : string.Empty;
                    //incomingTranVisa.MaskedPAN = data.PAN;
                    //incomingTranVisa.EncryptedPAN = data.PANEncrypted;
                    incomingTranMasterCard.POSEntryMode = data.POSEntryMode;
                    incomingTranMasterCard.PosCardDataImputMode = data.PosCardDataInputMode;
                    incomingTranMasterCard.PosCardPresent = data.PosCardPresent;
                    incomingTranMasterCard.PosCardholderPresent = data.PosCardholderPresent;
                    incomingTranMasterCard.PosCardholderAuthMethod = data.POSCardholderAuthMethod;
                    incomingTranMasterCard.ExpiryDate = data.ExpiryDate;
                    incomingTranMasterCard.SettleAmountImpact = data.SettleAmountImpact.HasValue
                        ? data.SettleAmountImpact.ToString()
                        : string.Empty;
                    incomingTranMasterCard.TranAmountReq = data.TranAmountReq.HasValue
                        ? data.TranAmountReq.ToString()
                        : string.Empty;
                    incomingTranMasterCard.PrevPostTranId = data.PrevPostTranId.HasValue
                        ? data.PrevPostTranId.ToString()
                        : string.Empty;

                    if (string.IsNullOrEmpty(incomingTranMasterCard.ACode))
                        incomingTranMasterCard.ACode = data.AuthIDRsp;

                    if (!string.IsNullOrWhiteSpace(data.StructuredDataReq))
                    {
                        incomingTranMasterCard.Narritive = data.GetStructuredDataValue("eS:GiccMerchantName");
                        incomingTranMasterCard.CVVFlag = data.GetStructuredDataValue("eS:GiccCVV2P");
                        incomingTranMasterCard.GiccMCC = data.GetStructuredDataValue("eS:GiccMCC");
                        incomingTranMasterCard.GiccDomesticMCC =
                            data.GetStructuredDataValue("eS:GiccDomesticMCC");
                        incomingTranMasterCard.GiccRevDate = data.GetStructuredDataValue("eS:GiccRevDate");
                    }

                    var tranAmountReq = data.TranAmountReq.ToString();
                    //if (tranAmountReq != null)
                    //{
                    //    if (tranAmountReq.Contains("-"))
                    //        tranAmountReq = tranAmountReq.Substring(1);
                    //}

                    var settleAmountImpact = data.SettleAmountImpact.ToString();
                    //if (settleAmountImpact != null)
                    //{
                    //    if (settleAmountImpact.Contains("-"))
                    //        settleAmountImpact = settleAmountImpact.Substring(1);
                    //}

                    if (msgType.Equals("0400") || msgType.Equals("0420"))
                    {
                        if (!string.IsNullOrWhiteSpace(incomingTranMasterCard.GiccRevDate))
                        {
                            var day = incomingTranMasterCard.GiccRevDate.Substring(2, 2);
                            var month = incomingTranMasterCard.GiccRevDate.Substring(0, 2);
                            var year = PostilionData.getYY(month);
                            var hour = incomingTranMasterCard.GiccRevDate.Substring(4, 2);
                            var minutes = incomingTranMasterCard.GiccRevDate.Substring(6, 2);
                            var seconds = incomingTranMasterCard.GiccRevDate.Substring(8, 2);
                            var tranDatetime = "20" + year + "-" + month + "-" + day + " " + hour + ":" +
                                               minutes + ":" + seconds + ":000";
                            incomingTranMasterCard.TransactionDateTimeLocal =
                                System.Convert.ToDateTime(tranDatetime).ToString("yyyy-MM-dd HH:mm:ss:fff");
                        }

                        if (!string.IsNullOrWhiteSpace(settleAmountImpact) &&
                            !settleAmountImpact.Equals("0"))
                            incomingTranMasterCard.TransactionAmount = settleAmountImpact;
                    }
                    else
                    {
                        incomingTranMasterCard.TransactionDateTimeLocal = data.DatetimeTranLocal.HasValue
                            ? data.DatetimeTranLocal.Value.ToString("yyyy-MM-dd HH:mm:ss:fff")
                            : string.Empty;
                        incomingTranMasterCard.TransactionAmount = tranAmountReq;
                    }
                    if ((string.IsNullOrWhiteSpace(incomingTranMasterCard.TransactionAmount) ||
                         incomingTranMasterCard.TransactionAmount.Equals("0"))
                        && !string.IsNullOrWhiteSpace(incomingTranMasterCard.PrevPostTranId))
                        incomingTranMasterCard.TransactionAmount =
                            new PostilionRepo(base.GetContext<ComplaintEntities>()).GetPreviousTransactionAmount(incomingTranMasterCard.PrevPostTranId);

                    transAmountSign = incomingTranMasterCard.TransactionAmountSign;
                    transAmount = incomingTranMasterCard.TransactionAmount;
                    transCurrencyCode = incomingTranMasterCard.TransactionCurrencyCode;
                    transDate = incomingTranMasterCard.TransactionDate;
                    ecommerce = incomingTranMasterCard.ECommerce;
                    expiryDate = incomingTranMasterCard.ExpiryDate;
                    transAmountExp = incomingTranMasterCard.TransactionAmountExponent;
                    Update(incomingTranMasterCard);
                }

                #endregion
            }

            #region Refill for Complaint

            var tempComplaint = GetDbSet<Complaint>().FirstOrDefault(x => x.CaseId == complaint.CaseId);

            if (data.PostID.HasValue)
                tempComplaint.PostTranId = data.PostID.Value;

            tempComplaint.PANMask = data.PAN.Replace('*', 'X');

            if (!string.IsNullOrWhiteSpace(expiryDate))
            {
                tempComplaint.PANExpirationDateYear = expiryDate.Substring(0, 2);
                tempComplaint.PANExpirationDateMonth = expiryDate.Substring(2, 2);
            }

            if (string.IsNullOrWhiteSpace(tempComplaint.ACode))
                tempComplaint.ACode = data.AuthIDRsp;

            tempComplaint.TransactionAmountSign = transAmountSign;

            if (!string.IsNullOrWhiteSpace(transAmount) && !string.IsNullOrWhiteSpace(transAmountExp))
                tempComplaint.TransactionAmount =
                    Helper.ConvertStringAmountToDecimal(transAmount, transAmountExp);

            tempComplaint.TransactionCurrencyCode = transCurrencyCode;
            if (tempComplaint.OrganizationId.Equals(Common.Enum.Organization.MC.ToString()))
            {
                tempComplaint.TransactionDate = transDate != null
                    ? DateTime.ParseExact(transDate, "yyMMddHHmmss",
                        CultureInfo.CurrentUICulture.DateTimeFormat)
                    : tempComplaint.TransactionDate;
            }
            else
            {
                tempComplaint.TransactionDate = transDate != null
                    ? DateTime.ParseExact(transDate, "yyyy-MM-dd HH:mm:ss:fff",
                        CultureInfo.CurrentUICulture.DateTimeFormat)
                    : tempComplaint.TransactionDate;
            }

            tempComplaint.MessageType = data.MessageType;
            tempComplaint.POSEntryMode = data.POSEntryMode;
            tempComplaint.ECommerce = ecommerce;
            tempComplaint.ParticipantId = data.ParticipantId;
            tempComplaint.PostilionData = true;

            if (tempComplaint.MID == null)
                tempComplaint.MID = data.CardAcceptorIDCode;

            Update(tempComplaint);
            #endregion

            #region Add new ComplaintRecord - old version

            //using (var entities = new ComplaintEntities())
            //{
            //    var complaintRecord =
            //        entities.ComplaintRecords.OrderByDescending(x => x.RecordId)
            //            .FirstOrDefault(x => x.CaseId.Equals(complaint.CaseId));
            //    var newComplaintRecord = complaintRecord;

            //    if (newComplaintRecord == null) return;
            //    newComplaintRecord.IncomingId = incomingId;
            //    newComplaintRecord.PosTerminalType = data.PosTerminalType;
            //    newComplaintRecord.PosCardDataInputMode = data.PosCardDataInputMode;
            //    newComplaintRecord.PosCardPresent = data.PosCardPresent;
            //    newComplaintRecord.PosCardholderPresent = data.PosCardholderPresent;
            //    newComplaintRecord.PosCardholderAuthMethod = data.POSCardholderAuthMethod;

            //    var giccRevDate = data.GetStructuredDataValue("eS:GiccRevDate");
            //    if (!string.IsNullOrWhiteSpace(giccRevDate))
            //    {
            //        var day = giccRevDate.Substring(2, 2);
            //        var month = giccRevDate.Substring(0, 2);
            //        var year = PostilionData.getYY(month);
            //        var hour = giccRevDate.Substring(4, 2);
            //        var minutes = giccRevDate.Substring(6, 2);
            //        var seconds = giccRevDate.Substring(8, 2);
            //        newComplaintRecord.GiccRevDate = DateTime.ParseExact(
            //            string.Format("20{0}-{1}-{2} {3}:{4}:{5}",
            //                year, month, day, hour, minutes, seconds),
            //            "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.CurrentUICulture.DateTimeFormat);
            //    }

            //    if (data.SettleAmountImpact.HasValue)
            //        newComplaintRecord.SettleAmountImpact = data.SettleAmountImpact.Value;

            //    if (data.TranAmountReq.HasValue)
            //        newComplaintRecord.TranAmountReq = data.TranAmountReq.Value;

            //    if (data.PrevPostTranId.HasValue)
            //        newComplaintRecord.PrevPostTranId = data.PrevPostTranId.Value;

            //    var giccMerchantName = data.GetStructuredDataValue("eS:GiccMerchantName");
            //    if (!string.IsNullOrWhiteSpace(giccMerchantName))
            //        newComplaintRecord.Narritive = giccMerchantName;

            //    newComplaintRecord.InsertDate = DateTime.UtcNow;
            //    newComplaintRecord.InsertUser = "ComplaintServices";

            //    entities.ComplaintRecords.Add(newComplaintRecord);
            //    entities.SaveChanges();
            //}

            #endregion

            return true;
        }

        public ComplaintRecord UpdateComplaintRecord(Complaint complaint)
        {
            var complaintRecord = complaint.ComplaintRecords.OrderByDescending(x => x.RecordId).FirstOrDefault();
            
            if (complaintRecord != null)
                return UpdateComplaintRecord(complaintRecord);
            throw new Exception(string.Format("Could not find complaint record for caseId {0}", complaint.CaseId));
        }
    }
}
