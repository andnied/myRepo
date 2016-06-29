using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using System;
using System.Collections.Generic;
//using System.Data.Entity.Validation;
using System.Linq;

namespace ComplaintTool.Processing.AutoRepresentmentValidation.Validating
{
    public class AutoRepresentmentValidating
    {
        private readonly ComplaintUnitOfWork _unitOfWork;

        public AutoRepresentmentValidating(ComplaintUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<ValidatingItem> ValidateRepresentmentData(ComplaintRecord complaintRecord)
        {
            var notValidatingItem = new List<ValidatingItem>();
            try
            {
                var complaint = complaintRecord.Complaint;
                var panFirst6 = complaint.PANMask.Substring(0, 6);
                var panLast4 = complaint.PANMask.Substring(12, 4);
                var data = _unitOfWork.Repo<PostilionRepo>().GetPostilionData(complaint.ARN, panFirst6, panLast4);

                if (data == null)
                    throw new Exception("Update.UpdateComplaintRecord Error. Postilion data not found."
                                        + " ARN = " + complaint.ARN
                                        + " PAN = " + complaint.PANMask);

                #region base data

                //SystemTraceAuditNr;
                if (
                    !(complaintRecord.SystemTraceAuditNr ?? string.Empty).Equals(data.SystemTraceAuditNr ?? string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("SystemTraceAuditNr", complaintRecord.SystemTraceAuditNr),
                        new ValidatingItem.VItem("SystemTraceAuditNr", data.SystemTraceAuditNr)));
                }
                //DatetimeTranLocal.ToString();
                if (complaintRecord.DatetimeTranLocal != data.DatetimeTranLocal)
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("DatetimeTranLocal",
                            complaintRecord.DatetimeTranLocal.HasValue
                                ? complaintRecord.DatetimeTranLocal.ToString()
                                : string.Empty),
                        new ValidatingItem.VItem("DatetimeTranLocal",
                            data.DatetimeTranLocal.HasValue ? data.DatetimeTranLocal.ToString() : string.Empty)));
                }
                //ExpiryDate;
                if (!(complaintRecord.ExpiryDate ?? string.Empty).Equals(data.ExpiryDate ?? string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("ExpiryDate", complaintRecord.ExpiryDate),
                        new ValidatingItem.VItem("ExpiryDate", data.ExpiryDate)));
                }
                //MerchantType;
                if (!(complaintRecord.MerchantType ?? string.Empty).Equals(data.MerchantType ?? string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("MerchantType", complaintRecord.MerchantType),
                        new ValidatingItem.VItem("MerchantType", data.MerchantType)));
                }
                //POSEntryMode;
                if (!(complaintRecord.POSEntryMode ?? string.Empty).Equals(data.POSEntryMode ?? string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("POSEntryMode", complaintRecord.POSEntryMode),
                        new ValidatingItem.VItem("POSEntryMode", data.POSEntryMode)));
                }
                //CardSeqNr;
                if (!(complaintRecord.CardSeqNr ?? string.Empty).Equals(data.CardSeqNr ?? string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("CardSeqNr", complaintRecord.CardSeqNr),
                        new ValidatingItem.VItem("CardSeqNr", data.CardSeqNr)));
                }
                //PosConditionCode;
                if (!(complaintRecord.PosConditionCode ?? string.Empty).Equals(data.PosConditionCode ?? string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("PosConditionCode", complaintRecord.PosConditionCode),
                        new ValidatingItem.VItem("PosConditionCode", data.PosConditionCode)));
                }
                //RetrievalReferenceNr;
                if (
                    !(complaintRecord.RetrievalReferenceNr ?? string.Empty).Equals(data.RetrievalReferenceNr ??
                                                                                   string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("RetrievalReferenceNr", complaintRecord.RetrievalReferenceNr),
                        new ValidatingItem.VItem("RetrievalReferenceNr", data.RetrievalReferenceNr)));
                }
                //AuthIDRsp;
                if (!(complaintRecord.AuthIDRsp ?? string.Empty).Equals(data.AuthIDRsp ?? string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("AuthIDRsp", complaintRecord.AuthIDRsp),
                        new ValidatingItem.VItem("AuthIDRsp", data.AuthIDRsp)));
                }
                //RspCodeRsp;
                if (!(complaintRecord.RspCodeRsp ?? string.Empty).Equals(data.RspCodeRsp ?? string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("RspCodeRsp", complaintRecord.RspCodeRsp),
                        new ValidatingItem.VItem("RspCodeRsp", data.RspCodeRsp)));
                }
                //ServiceRestrictionCode;
                if (
                    !(complaintRecord.ServiceRestrictionCode ?? string.Empty).Equals(data.ServiceRestrictionCode ??
                                                                                     string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("ServiceRestrictionCode", complaintRecord.ServiceRestrictionCode),
                        new ValidatingItem.VItem("ServiceRestrictionCode", data.ServiceRestrictionCode)));
                }
                //TerminalID;
                if (!(complaintRecord.TerminalID ?? string.Empty).Equals(data.TerminalID ?? string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("TerminalID", complaintRecord.TerminalID),
                        new ValidatingItem.VItem("TerminalID", data.TerminalID)));
                }
                //CardAcceptorIDCode;
                if (
                    !(complaintRecord.CardAcceptorIDCode ?? string.Empty).Equals(data.CardAcceptorIDCode ?? string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("CardAcceptorIDCode", complaintRecord.CardAcceptorIDCode),
                        new ValidatingItem.VItem("CardAcceptorIDCode", data.CardAcceptorIDCode)));
                }
                //CardAcceptorNameLoc;
                if (
                    !(complaintRecord.CardAcceptorNameLoc ?? string.Empty).Equals(data.CardAcceptorNameLoc ??
                                                                                  string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("CardAcceptorNameLoc", complaintRecord.CardAcceptorNameLoc),
                        new ValidatingItem.VItem("CardAcceptorNameLoc", data.CardAcceptorNameLoc)));
                }
                //PosCardDataInputAbility,
                if (
                    !(complaintRecord.PosCardDataInputAbility ?? string.Empty).Equals(data.PosCardDataInputAbility ??
                                                                                      string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("PosCardDataInputAbility", complaintRecord.PosCardDataInputAbility),
                        new ValidatingItem.VItem("PosCardDataInputAbility", data.PosCardDataInputAbility)));
                }
                //PosCardholderAuthAbility,
                if (
                    !(complaintRecord.PosCardholderAuthAbility ?? string.Empty).Equals(data.PosCardholderAuthAbility ??
                                                                                       string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("PosCardholderAuthAbility",
                            complaintRecord.PosCardholderAuthAbility),
                        new ValidatingItem.VItem("PosCardholderAuthAbility", data.PosCardholderAuthAbility)));
                }
                //PosCardCaptureAbility,
                if (
                    !(complaintRecord.PosCardCaptureAbility ?? string.Empty).Equals(data.PosCardCaptureAbility ??
                                                                                    string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("PosCardCaptureAbility", complaintRecord.PosCardCaptureAbility),
                        new ValidatingItem.VItem("PosCardCaptureAbility", data.PosCardCaptureAbility)));
                }
                //PosOperatingEnvironment,
                if (
                    !(complaintRecord.PosOperatingEnvironment ?? string.Empty).Equals(data.PosOperatingEnvironment ??
                                                                                      string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("PosOperatingEnvironment", complaintRecord.PosOperatingEnvironment),
                        new ValidatingItem.VItem("PosOperatingEnvironment", data.PosOperatingEnvironment)));
                }
                //PosCardholderPresent,
                if (
                    !(complaintRecord.PosCardholderPresent ?? string.Empty).Equals(data.PosCardholderPresent ??
                                                                                   string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("PosCardholderPresent", complaintRecord.PosCardholderPresent),
                        new ValidatingItem.VItem("PosCardholderPresent", data.PosCardholderPresent)));
                }
                //PosCardPresent,
                if (!(complaintRecord.PosCardPresent ?? string.Empty).Equals(data.PosCardPresent ?? string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("PosCardPresent", complaintRecord.PosCardPresent),
                        new ValidatingItem.VItem("PosCardPresent", data.PosCardPresent)));
                }
                //PosCardDataInputMode,
                if (
                    !(complaintRecord.PosCardDataInputMode ?? string.Empty).Equals(data.PosCardDataInputMode ??
                                                                                   string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("PosCardDataInputMode", complaintRecord.PosCardDataInputMode),
                        new ValidatingItem.VItem("PosCardDataInputMode", data.PosCardDataInputMode)));
                }
                //PosCardholderAuthMethod,
                if (
                    !(complaintRecord.PosCardholderAuthMethod ?? string.Empty).Equals(data.POSCardholderAuthMethod ??
                                                                                      string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("PosCardholderAuthMethod", complaintRecord.PosCardholderAuthMethod),
                        new ValidatingItem.VItem("POSCardholderAuthMethod", data.POSCardholderAuthMethod)));
                }
                //PosCardholderAuthEntity,
                if (
                    !(complaintRecord.PosCardholderAuthEntity ?? string.Empty).Equals(data.PosCardholderAuthEntity ??
                                                                                      string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("PosCardholderAuthEntity", complaintRecord.PosCardholderAuthEntity),
                        new ValidatingItem.VItem("PosCardholderAuthEntity", data.PosCardholderAuthEntity)));
                }
                //PosCardDataOutputAbility,
                if (
                    !(complaintRecord.PosCardDataOutputAbility ?? string.Empty).Equals(data.PosCardDataOutputAbility ??
                                                                                       string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("PosCardDataOutputAbility",
                            complaintRecord.PosCardDataOutputAbility),
                        new ValidatingItem.VItem("PosCardDataOutputAbility", data.PosCardDataOutputAbility)));
                }
                //PosTerminalOutputAbility,
                if (
                    !(complaintRecord.PosTerminalOutputAbility ?? string.Empty).Equals(data.PosTerminalOutputAbility ??
                                                                                       string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("PosTerminalOutputAbility",
                            complaintRecord.PosTerminalOutputAbility),
                        new ValidatingItem.VItem("PosTerminalOutputAbility", data.PosTerminalOutputAbility)));
                }
                //PosPinCaptureAbility,
                if (
                    !(complaintRecord.PosPinCaptureAbility ?? string.Empty).Equals(data.PosPinCaptureAbility ??
                                                                                   string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("PosPinCaptureAbility", complaintRecord.PosPinCaptureAbility),
                        new ValidatingItem.VItem("PosPinCaptureAbility", data.PosPinCaptureAbility)));
                }
                //PosTerminalOperator,
                if (
                    !(complaintRecord.PosTerminalOperator ?? string.Empty).Equals(data.PosTerminalOperator ??
                                                                                  string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("PosTerminalOperator", complaintRecord.PosTerminalOperator),
                        new ValidatingItem.VItem("PosTerminalOperator", data.PosTerminalOperator)));
                }
                //PosTerminalType
                if (!(complaintRecord.PosTerminalType ?? string.Empty).Equals(data.PosTerminalType ?? string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("PosTerminalType", complaintRecord.PosTerminalType),
                        new ValidatingItem.VItem("PosTerminalType", data.PosTerminalType)));
                }
                //AuthIDRsp;
                if (!(complaintRecord.AuthIDRsp ?? string.Empty).Equals(data.AuthIDRsp ?? string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("AuthIDRsp", complaintRecord.AuthIDRsp),
                        new ValidatingItem.VItem("AuthIDRsp", data.AuthIDRsp)));
                }
                //UcafData
                if (!(complaintRecord.UcafData ?? string.Empty).Equals(data.UcafData ?? string.Empty))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("UcafData", complaintRecord.UcafData),
                        new ValidatingItem.VItem("UcafData", data.UcafData)));
                }
                //TranAmountRsp
                var tranAmountRsp = data.TranAmountRsp != null ? data.TranAmountRsp.ToString() : string.Empty;
                if (!(complaintRecord.TranAmountRsp ?? string.Empty).Equals(tranAmountRsp))
                {
                    notValidatingItem.Add(new ValidatingItem(
                        new ValidatingItem.VItem("TranAmountRsp", complaintRecord.TranAmountRsp),
                        new ValidatingItem.VItem("TranAmountRsp", data.TranAmountRsp.ToString())));
                }

                #endregion

                if (!complaintRecord.IncomingId.HasValue)
                    throw new Exception("Update.UpdateComplaintRecord Error. RecordId = " + complaintRecord.RecordId +
                                        " IncomingId is null.");

                #region Incoming Data

                if (complaint.OrganizationId == Common.Enum.Organization.MC.ToString())
                {
                    var incoming =
                        _unitOfWork.Repo<IncomingTranRepo>().FindTranMASTERCARDById(complaintRecord.IncomingId);

                    if (incoming == null)
                        throw new Exception("Update.UpdateComplaintRecord Error. RecordId = " + complaintRecord.RecordId +
                                            " IncomingTranMASTERCARD is null. IncomingId = " +
                                            complaintRecord.IncomingId);
                    //MASTERCARD - INCOMING
                    //TranLifeCycleID;
                    if (
                        !(complaintRecord.TranLifeCycleID ?? string.Empty).Equals(incoming.TranLifeCycleID ??
                                                                                  string.Empty))
                    {
                        notValidatingItem.Add(new ValidatingItem(
                            new ValidatingItem.VItem("TranLifeCycleID", complaintRecord.TranLifeCycleID),
                            new ValidatingItem.VItem("TranLifeCycleID", incoming.TranLifeCycleID)));
                    }
                    //IRD;
                    if (!(complaintRecord.IRD ?? string.Empty).Equals(incoming.IRD ?? string.Empty))
                    {
                        notValidatingItem.Add(new ValidatingItem(
                            new ValidatingItem.VItem("IRD", complaintRecord.IRD),
                            new ValidatingItem.VItem("IRD", incoming.IRD)));
                    }
                    //AssignedID;
                    if (!(complaintRecord.AssignedID ?? string.Empty).Equals(incoming.AssignedID ?? string.Empty))
                    {
                        notValidatingItem.Add(new ValidatingItem(
                            new ValidatingItem.VItem("AssignedID", complaintRecord.AssignedID),
                            new ValidatingItem.VItem("AssignedID", incoming.AssignedID)));
                    }
                    //FunctionCode;
                    if (!(complaintRecord.FunctionCode ?? string.Empty).Equals(incoming.FunctionCode ?? string.Empty))
                    {
                        notValidatingItem.Add(new ValidatingItem(
                            new ValidatingItem.VItem("FunctionCode", complaintRecord.FunctionCode),
                            new ValidatingItem.VItem("FunctionCode", incoming.FunctionCode)));
                    }
                    //FraudNotificationDate;
                    if (
                        !(complaintRecord.FraudNotificationDate ?? string.Empty).Equals(
                            incoming.FraudNotificationDate ?? string.Empty))
                    {
                        notValidatingItem.Add(new ValidatingItem(
                            new ValidatingItem.VItem("FraudNotificationDate", complaintRecord.FraudNotificationDate),
                            new ValidatingItem.VItem("FraudNotificationDate", incoming.FraudNotificationDate)));
                    }

                }
                if (complaint.OrganizationId == Common.Enum.Organization.VISA.ToString())
                {
                    var incoming = _unitOfWork.Repo<IncomingTranRepo>().FindTranVISAById(complaintRecord.IncomingId);
                    if (incoming == null)
                        throw new Exception("Update.UpdateComplaintRecord Error. RecordId = " + complaintRecord.RecordId +
                                            " IncomingTranVISAs is null. IncomingId = " +
                                            complaintRecord.IncomingId);
                    //--incoming VISA
                    //,@TransactionId nvarchar(15)
                    if (!(complaintRecord.TransactionId ?? string.Empty).Equals(incoming.TransactionId ?? string.Empty))
                    {
                        notValidatingItem.Add(new ValidatingItem(
                            new ValidatingItem.VItem("TransactionId", complaintRecord.TransactionId),
                            new ValidatingItem.VItem("TransactionId", incoming.TransactionId)));
                    }
                    //,@MultiClearingSeqNr nvarchar(2)
                    if (
                        !(complaintRecord.MultiClearingSeqNr ?? string.Empty).Equals(incoming.MultiClearingSeqNr ??
                                                                                     string.Empty))
                    {
                        notValidatingItem.Add(new ValidatingItem(
                            new ValidatingItem.VItem("MultiClearingSeqNr", complaintRecord.MultiClearingSeqNr),
                            new ValidatingItem.VItem("MultiClearingSeqNr", incoming.MultiClearingSeqNr)));
                    }
                    //,@MultiClearingSeqCnt nvarchar(2)
                    if (
                        !(complaintRecord.MultiClearingSeqCnt ?? string.Empty).Equals(incoming.MultiClearingSeqCnt ??
                                                                                      string.Empty))
                    {
                        notValidatingItem.Add(new ValidatingItem(
                            new ValidatingItem.VItem("MultiClearingSeqCnt", complaintRecord.MultiClearingSeqCnt),
                            new ValidatingItem.VItem("MultiClearingSeqCnt", incoming.MultiClearingSeqCnt)));
                    }
                    //,@AuthSourceCode nvarchar(1)
                    if (
                        !(complaintRecord.AuthSourceCode ?? string.Empty).Equals(incoming.AuthSourceCode ?? string.Empty))
                    {
                        notValidatingItem.Add(new ValidatingItem(
                            new ValidatingItem.VItem("AuthSourceCode", complaintRecord.AuthSourceCode),
                            new ValidatingItem.VItem("AuthSourceCode", incoming.AuthSourceCode)));
                    }
                    //,@AVSRspCode nvarchar(1)
                    if (!(complaintRecord.AVSRspCode ?? string.Empty).Equals(incoming.AVSRspCode ?? string.Empty))
                    {
                        notValidatingItem.Add(new ValidatingItem(
                            new ValidatingItem.VItem("AVSRspCode", complaintRecord.AVSRspCode),
                            new ValidatingItem.VItem("AVSRspCode", incoming.AVSRspCode)));
                    }
                    //,@MarketSpecAuth nvarchar(1)
                    if (
                        !(complaintRecord.MarketSpecAuth ?? string.Empty).Equals(incoming.MarketSpecAuth ?? string.Empty))
                    {
                        notValidatingItem.Add(new ValidatingItem(
                            new ValidatingItem.VItem("MarketSpecAuth", complaintRecord.MarketSpecAuth),
                            new ValidatingItem.VItem("MarketSpecAuth", incoming.MarketSpecAuth)));
                    }
                    //,@AuthRspCode nvarchar(2)                            
                    if (!(complaintRecord.AuthRspCode ?? string.Empty).Equals(incoming.AuthRspCode ?? string.Empty))
                    {
                        notValidatingItem.Add(new ValidatingItem(
                            new ValidatingItem.VItem("AuthRspCode", complaintRecord.AuthRspCode),
                            new ValidatingItem.VItem("AuthRspCode", incoming.AuthRspCode)));
                    }
                }

                #endregion
            }
            //catch (DbEntityValidationException ex)
            //{
            //    var errorMessages = ex.EntityValidationErrors
            //        .SelectMany(x => x.ValidationErrors)
            //        .Select(x => x.ErrorMessage);
            //    var fullErrorMessage = string.Join("; ", errorMessages);
            //    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
            //    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            //}
            catch (Exception ex)
            {
                throw ex;
            }
            return notValidatingItem;
        }
    }
}
