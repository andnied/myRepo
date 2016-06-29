using ComplaintTool.Common.Config;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using System;
using System.Linq;
using ComplaintTool.DataAccess.Utils;
using ComplaintTool.Postilion.Outgoing.Model.Representment.Visa;

namespace ComplaintTool.Postilion.Outgoing.Model.Representment
{
    public class Visa4103Extracter : Organization4103Extracter
    {

        public Visa4103Extracter(ComplaintUnitOfWork unitOfWork, Models.Representment representment)
            : base(unitOfWork, representment)
        {
        }

        protected override I4103 Set4103(Complaint complaint, ComplaintValue complaintValue, ComplaintStage complaintStage, ComplaintRecord complaintRecord)
        {
            var visa4103 = new VISA4103();
            var helper = new Helper();

            if (Representment == null)
                throw new Exception("Representment.SetVISA4103 Error. Complaints is null");

            var currencyCodeList = ComplaintDictionaires.Instance.GetCurrencyCode();

            if (complaint == null) return visa4103;

            string pan, panExtention;
            UnitOfWork.Repo<ComplaintRepo>().GetPanByCaseId(complaint.CaseId, out pan, out panExtention);
            visa4103._2 = pan;

            //ComplaintValue.InsertDate
            visa4103._7 = Representment.InsertDate.ToString();
            //32	const	6	"Set '400748' where position 2-7 of Complaint.ARN = '400748'
            //Set '414848'  where position 2-7 of Complaint.ARN = '414848'"
            visa4103._32 = complaint.BIN;

            #region ComplaintValues

            if (complaintValue != null)
            {
                var bookingCurrencyCode = short.Parse(complaintValue.BookingCurrencyCode);
                var currencyCode = currencyCodeList[bookingCurrencyCode];
                if (complaintValue.BookingAmount.HasValue)
                {
                    int exponent;
                    visa4103._4 = helper.AmountToString(currencyCode,
                        complaintValue.BookingAmount.Value,
                        12,
                        out exponent);
                }
                else
                {
                    throw new Exception("Representment.SetVISA4103 Error. ComplaintValues.BookingAmount is null. ValueId=" +
                                        Representment.ValueId);
                }
                visa4103._49 = complaintValue.BookingCurrencyCode;
                var complaintValue1Cb = complaint.ComplaintValues.FirstOrDefault();
                if (complaintValue1Cb != null)
                {
                    visa4103._50 = complaintValue1Cb.StageCurrencyCode;
                }
                else
                {
                    throw new Exception("Representment.SetVISA4103 Error. CaseId=" +
                                        Representment.CaseId +
                                        " complaintValue1Cb is null (FirstOrDefault()). CaseId=" +
                                        Representment.CaseId);
                }
            }
            else
            {
                throw new Exception("Representment.SetVISA4103 Error. CaseId=" + Representment.CaseId +
                                    " ComplaintValues is null. ValueId=" + Representment.ValueId);
            }

            #endregion

            #region ComplaintStages

            if (complaintStage != null)
            {
                //ComplaintStage.ReasonCode
                visa4103._56 = complaintStage.ReasonCode;
            }
            else
            {
                throw new Exception(
                    "Representment.SetVISA4103 Error. ComplaintStages is null. StageId=" +
                    Representment.StageId);
            }

            #endregion

            #region ComplaintRecord

            if (complaintRecord != null)
            {
                visa4103._11 = complaintRecord.SystemTraceAuditNr;
                //post_tran.datetime_tran_local
                visa4103._13 = complaintRecord.DatetimeTranLocal.ToString();
                //post_tran_cust.expiry_date
                visa4103._14 = complaintRecord.ExpiryDate;
                //post_tran_cust.merchant_type
                visa4103._18 = complaintRecord.MerchantType;
                //post_tran.pos_entry_mode
                visa4103._22 = complaintRecord.POSEntryMode;
                //post_tran_cust.card_seq_nr
                visa4103._23 = complaintRecord.CardSeqNr;
                //post_tran.pos_condition_code
                visa4103._25 = complaintRecord.PosConditionCode;
                //post_tran.RetrievalReferenceNumber 
                visa4103._37 = complaintRecord.RetrievalReferenceNr;
                //post_tran.auth_id_rsp
                visa4103._38 = complaintRecord.AuthIDRsp;
                //post_tran.rsp_code_rsp
                visa4103._39 = complaintRecord.RspCodeRsp;
                //post_tran_cust.terminal_id
                visa4103._41 = complaintRecord.TerminalID;
                //post_tran_cust.card_acceptor_id_code 
                visa4103._42 = complaintRecord.CardAcceptorIDCode;
                //post_tran_cust.card_acceptor_name_loc
                visa4103._43 = complaintRecord.CardAcceptorNameLoc;
                //_123

                #region posDataCode

                var posDataCode = new PosDataCode
                {
                    //Position 1: post_tran_cust.pos_card_data_input_ability
                    PosCardDataInputAbility = complaintRecord.PosCardDataInputAbility,
                    //Position 2: post_tran_cust.pos_cardholder_auth_ability
                    PosCardholderAuthAbility = complaintRecord.PosCardholderAuthAbility,
                    //Position 3: post_tran_cust.pos_card_capture_ability
                    PosCardCaptureAbility = complaintRecord.PosCardCaptureAbility,
                    //Position 4: post_tran_cust.pos_operating_environment
                    PosOperatingEnvironment = complaintRecord.PosOperatingEnvironment,
                    //Position 5: post_tran_cust.pos_cardholder_present
                    PosCardholderPresent = complaintRecord.PosCardholderPresent,
                    //Position 6: post_tran_cust.pos_card_present
                    PosCardPresent = complaintRecord.PosCardPresent,
                    //Position 7: post_tran_cust.pos_card_data_input_mode
                    PosCardDataInputMode = complaintRecord.PosCardDataInputMode,
                    //Position 8: post_tran_cust.pos_cardholder_auth_method
                    PosCardholderAuthMethod = complaintRecord.PosCardholderAuthMethod,
                    //Position 9: post_tran_cust.pos_cardholder_auth_entity
                    PosCardholderAuthEntity = complaintRecord.PosCardholderAuthEntity,
                    //Position 10: post_tran_cust.pos_card_data_output_ability
                    PosCardDataOutputAbility = complaintRecord.PosCardDataOutputAbility,
                    //Position 11: post_tran_cust.pos_terminal_output_ability
                    PosTerminalOutputAbility = complaintRecord.PosTerminalOutputAbility,
                    //Position 12: post_tran_cust.pos_pin_capture_ability
                    PosPinCaptureAbility = complaintRecord.PosPinCaptureAbility,
                    //Position 13: post_tran_cust.pos_terminal_operator
                    PosTerminalOperator = complaintRecord.PosTerminalOperator,
                    //Position 14-15: post_tran_cust.pos_terminal_type
                    PosTerminalType = complaintRecord.PosTerminalType
                };

                if (posDataCode.ToString().Length != 15)
                {
                    //Position 1: post_tran_cust.pos_card_data_input_ability
                    var error = "PosCardDataInputAbility=" + posDataCode.PosCardDataInputAbility + ";";
                    //Position 2: post_tran_cust.pos_cardholder_auth_ability
                    error += "PosCardholderAuthAbility=" + posDataCode.PosCardholderAuthAbility + ";";
                    //Position 3: post_tran_cust.pos_card_capture_ability
                    error += "PosCardCaptureAbility=" + posDataCode.PosCardCaptureAbility + ";";
                    //Position 4: post_tran_cust.pos_operating_environment
                    error += "PosOperatingEnvironment=" + posDataCode.PosOperatingEnvironment + ";";
                    //Position 5: post_tran_cust.pos_cardholder_present
                    error += "PosCardholderPresent=" + posDataCode.PosCardholderPresent + ";";
                    //Position 6: post_tran_cust.pos_card_present
                    error += "PosCardPresent=" + posDataCode.PosCardPresent + ";";
                    //Position 7: post_tran_cust.pos_card_data_input_mode
                    error += "PosCardDataInputMode=" + posDataCode.PosCardDataInputMode + ";";
                    //Position 8: post_tran_cust.pos_cardholder_auth_method
                    error += "PosCardholderAuthMethod=" + posDataCode.PosCardholderAuthMethod + ";";
                    //Position 9: post_tran_cust.pos_cardholder_auth_entity
                    error += "PosCardholderAuthEntity=" + posDataCode.PosCardholderAuthEntity + ";";
                    //Position 10: post_tran_cust.pos_card_data_output_ability
                    error += "PosCardDataOutputAbility=" + posDataCode.PosCardDataOutputAbility + ";";
                    //Position 11: post_tran_cust.pos_terminal_output_ability
                    error += "PosTerminalOutputAbility=" + posDataCode.PosTerminalOutputAbility + ";";
                    //Position 12: post_tran_cust.pos_pin_capture_ability
                    error += "PosPinCaptureAbility=" + posDataCode.PosPinCaptureAbility + ";";
                    //Position 13: post_tran_cust.pos_terminal_operator
                    error += "PosTerminalOperator=" + posDataCode.PosTerminalOperator + ";";
                    //Position 14-15: post_tran_cust.pos_terminal_type
                    error += "PosTerminalType=" + posDataCode.PosTerminalType + ";";
                    throw new Exception("Representment.SetVISA4103 Error. CaseId=" + Representment.CaseId +
                                        " posDataCode length is corupted. posDataCode: " + error);
                }

                #endregion

                visa4103._123 = posDataCode;

            }
            else
            {
                throw new Exception(
                    "Representment.SetVISA41033 Error. ComplaintRecord is null. RecordsId = " +
                    Representment.RecordsId);
            }

            #endregion

            #region MockData

            #region AdjustmentComponent

            var adjustmentComponent = new AdjustmentComponent
            {
                OriginalAmounts = new OriginalAmount
                {
                    //    TranAmount
                    TranAmount = visa4103._4,
                    //    SettleAmount
                    SettleAmount = visa4103._4,
                    //    TranCurrencyCode
                    TranCurrencyCode = visa4103._49,
                    //    SettleCurrencyCode
                    SettleCurrencyCode = visa4103._50
                },
                //ProcessingCode
                //ProcessingCode = complaintRecord.TranType.Substring(2) + "0000",
                ProcessingCode = complaintRecord.TranType + "0000",
                //AcquirerRefNr
                AcquirerRefNr = complaint.ARN,
                //AcquiringInstIdCode
                AcquiringInstIdCode = complaint.BIN,
                //AuthIdRsp
                AuthIdRsp = complaintRecord.AuthIDRsp
            };
            //DocumentationIndicator
            if (complaintStage.DocumentationIndicator.HasValue)
            {
                adjustmentComponent.DocumentationIndicator = complaintStage.DocumentationIndicator.Value
                    ? "1"
                    : "0";
            }
            else
            {
                adjustmentComponent.DocumentationIndicator = "0";
            }
            //MessageText
            var memberMessageText = Utils.RegexReplace(complaintStage.MemberMessageText);
            memberMessageText = memberMessageText.Length > 70
                ? memberMessageText.Substring(0, 70)
                : memberMessageText;
            adjustmentComponent.MessageText = memberMessageText;
            //MsgTypeId
            adjustmentComponent.MsgTypeId = Representment.IsReversal.HasValue ? Representment.IsReversal.Value ? "14" : "3" : "3";
            //MsgId
            adjustmentComponent.MsgId = complaint.PostTranId.HasValue
                ? complaint.PostTranId.ToString()
                : null;
            //Correction
            adjustmentComponent.Correction = "0";
            //ExtendedFields
            //var participantId = Tool.GetParticipantId(complaint.BIN);
            var participantId = complaint.ParticipantId ?? UnitOfWork.Repo<PostilionRepo>().GetParticipantId(complaint.BIN);
            if (!string.IsNullOrWhiteSpace(participantId))
                adjustmentComponent.ExtendedFields = string.Format("PARTICIPANT_ID:{0};", participantId);
            else
            {
                throw new Exception("Representment.SetVISA4103 Error. CaseId=" + complaint.CaseId +
                                    " PARTICIPANT_ID is null. BIN=" + complaint.BIN);
            }

            #endregion

            #region baseII

            var baseII = new BaseII
            {
                //ChargebackRefNr
                ChargebackRefNr = complaintRecord.KKOCbReference,
                //TranId
                TranId = complaintRecord.TransactionId,
                //MultiClearingSeqNr
                MultiClearingSeqNr = complaintRecord.MultiClearingSeqNr,
                //MultiClearingSeqCnt
                MultiClearingSeqCnt = complaintRecord.MultiClearingSeqCnt,
                //AuthSourceCode
                AuthSourceCode = complaintRecord.AuthSourceCode,
                //AVSRspCode
                AVSRspCode = complaintRecord.AVSRspCode,
                //MarketSpecAuth
                MarketSpecAuth = complaintRecord.MarketSpecAuth,
                //AuthRspCode
                AuthRspCode = complaintRecord.AuthRspCode
            };

            #endregion

            var mockData = new MockData
            {
                CaseID = complaint.CaseId,
                AdjustmentComponent = adjustmentComponent,
                BaseII = baseII
            };
            visa4103._127_022 = mockData;

            #endregion
            visa4103._127_033 = Representment.IsReversal.HasValue ? Representment.IsReversal.Value ? "4111" : "4103" : "4103";
            return visa4103;
        }
    }
}
