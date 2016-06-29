using ComplaintService.Postilion.Model.Representment;
using ComplaintService.Postilion.Model.Representment.MasterCard;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Model;
using ComplaintTool.DataAccess.Repos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.MasterCard.Outgoing
{
    public class MasterCard
    {
        private readonly ComplaintUnitOfWork _unitOfWork = ComplaintUnitOfWork.Get();

        public MC4103 GetMC4103Data(Representment representment, out bool isException, out bool isPostilionError)
        {
            isPostilionError = false;

            try
            {
                isException = false;
                var helper = new Helper();
                var postilionRepo = new PostilionRepo();

                var currencyCodeList = helper.GetCurrencyCode();
                var mc4103 = new MC4103();

                using (_unitOfWork)
                {
                    var complaint = _unitOfWork.Repo<ComplaintRepo>().FindByCaseId(representment.CaseId);

                    if (complaint != null)
                    {
                        #region complaintValues
                        if (complaint != null)
                        {
                            if (!string.IsNullOrEmpty(complaint.PANExtention))
                            {
                                if (complaint.PANExtention == "000")
                                    mc4103._2 = complaint.PAN;
                                else
                                    mc4103._2 = complaint.PAN + complaint.PANExtention;
                            }
                            else
                            {
                                mc4103._2 = complaint.PAN;
                            }
                            //ComplaintValue.InsertDate
                            mc4103._7 = representment.InsertDate.ToString();
                            //post_tran_id 6 cyfr z numeru  
                            var complaintValue = _unitOfWork.Repo<ComplaintRepo>().FindComplaintValueByValueId(representment.ValueId);

                            if (complaintValue != null)
                            {
                                var bookingCurrencyCode = short.Parse(complaintValue.BookingCurrencyCode);
                                var currencyCode = currencyCodeList[bookingCurrencyCode];
                                if (complaintValue.BookingAmount.HasValue)
                                {
                                    var exponent = 0;
                                    mc4103._4 = helper.AmountToString(currencyCode,
                                        complaintValue.BookingAmount.Value,
                                        12,
                                        out exponent);
                                }
                                else
                                {
                                    throw new Exception("GetMC4103Data Error. CaseId=" + representment.CaseId +
                                                            " ComplaintValues.BookingAmount is null. ValueId=" +
                                                            representment.ValueId);
                                }

                                mc4103._49 = complaintValue.BookingCurrencyCode;
                                var complaintValue1Cb = complaint.ComplaintValues.FirstOrDefault();
                                if (complaintValue1Cb != null)
                                {
                                    mc4103._50 = complaintValue1Cb.StageCurrencyCode;
                                }
                                else
                                {
                                    throw new Exception("GetMC4103Data Error. CaseId=" + representment.CaseId +
                                                        " complaintValue1Cb is null (FirstOrDefault()). CaseId=" +
                                                        representment.CaseId);
                                }
                            }
                            else
                            {
                                throw new Exception("GetMC4103Data Error. CaseId=" + representment.CaseId +
                                                        " ComplaintValues is null. ValueId=" + representment.ValueId);
                            }
                        }
                        #endregion

                        #region complaintStage
                        var complaintStage = _unitOfWork.Repo<ComplaintRepo>().GetStageById(representment.StageId);
                        mc4103._56 = complaintStage.ReasonCode;
                        #endregion

                        #region postilionData
                        var postilionData = postilionRepo.GetPostilionData(complaint.ARN, complaint.PANMask.Substring(0, 6), complaint.PANMask.Substring(12, 4));

                        if (postilionData != null)
                        {
                            this.AssignValuesFromPostilionData(ref mc4103, postilionData, representment.CaseId);

                        }
                        else
                        {
                            isPostilionError = true;
                            throw new Exception("GetMC4103Data Error. CaseId=" + representment.CaseId +
                                                " POSTILIONDATA is null." +
                                                " ARN=" + complaint.ARN +
                                                " PAN=" + complaint.PAN +
                                                " PostTranId=" + complaint.PostTranId);
                        }
                        #endregion

                        #region MockData

                        var adjustmentComponent = new AdjustmentComponent();
                        #region AdjustmentComponent

                        //OriginalAmounts	
                        adjustmentComponent.OriginalAmounts = new OriginalAmount
                        {
                            //    TranAmount
                            TranAmount = mc4103._4,
                            //    SettleAmount
                            SettleAmount = mc4103._4,
                            //    TranCurrencyCode
                            TranCurrencyCode = mc4103._49,
                            //    SettleCurrencyCode
                            SettleCurrencyCode = mc4103._50
                        };
                        var incomingTranMastercard = _unitOfWork.Repo<IncomingTranRepo>().FindTranMASTERCARDByCaseId(complaintStage.CaseId);
                        //IncomingTranMASTERCARD
                        if (incomingTranMastercard != null)
                        {
                            //ProcessingCode
                            adjustmentComponent.ProcessingCode = incomingTranMastercard.ProcessingCode.Substring(2) + "0000";
                            //IssuerRefNr
                            adjustmentComponent.IssuerRefNr = incomingTranMastercard.KKOCbReference;
                        }
                        else
                        {
                            throw new Exception("GetMC4103Data Error. CaseId=" + complaintStage.CaseId +
                                                " incomingTranMASTERCARD is null. IncomingId=" + complaintStage.IncomingId);
                        }
                        //AcquirerRefNr
                        adjustmentComponent.AcquirerRefNr = complaint.ARN;
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
                        var memberMessageText = helper.RegexReplace(complaintStage.MemberMessageText);
                        memberMessageText = memberMessageText.Length > 100
                            ? memberMessageText.Substring(0, 100)
                            : memberMessageText;
                        adjustmentComponent.MessageText = memberMessageText;
                        //AuthIdRsp
                        adjustmentComponent.AuthIdRsp = postilionData.AuthIDRsp;
                        //MsgId
                        adjustmentComponent.MsgId = complaint.PostTranId.Value.ToString(CultureInfo.InvariantCulture);
                        //UcafCollectionInd
                        adjustmentComponent.UcafCollectionInd = !String.IsNullOrWhiteSpace(postilionData.UcafData) ? postilionData.UcafData.Substring(0, 1) : "0";
                        //ExtendedFields
                        string participantId = postilionRepo.GetParticipantId(complaint.BIN);
                        if (!string.IsNullOrWhiteSpace(participantId))
                            adjustmentComponent.ExtendedFields = participantId;
                        else
                        {
                            throw new Exception("GetMC4103Data Error. CaseId=" + representment.CaseId +
                                            " PARTICIPANT_ID is null. BIN=" + complaint.BIN);
                        }
                        #endregion

                        var ipm = new Ipm();
                        #region Ipm
                        //TranLifeCycleID
                        if (!string.IsNullOrWhiteSpace(incomingTranMastercard.TranLifeCycleID))
                            ipm.TranLifeCycleID = incomingTranMastercard.TranLifeCycleID;
                        //IRD
                        if (!string.IsNullOrWhiteSpace(incomingTranMastercard.IRD))
                            ipm.IRD = incomingTranMastercard.IRD;
                        //McAssignedId
                        if (!string.IsNullOrWhiteSpace(incomingTranMastercard.AssignedID))
                            ipm.McAssignedId = incomingTranMastercard.AssignedID;
                        //FunctionCode
                        if (!string.IsNullOrWhiteSpace(incomingTranMastercard.FunctionCode))
                            ipm.FunctionCode = incomingTranMastercard.FunctionCode;
                        //AmountsOriginal
                        if (postilionData.TranAmountRsp.HasValue)
                        {
                            ipm.AmountsOriginal = helper.AmountToString(postilionData.TranAmountRsp.Value, 12);
                        }
                        //FraudNotificationInfo
                        ipm.FraudNotificationInfo = incomingTranMastercard.FraudNotificationDate;
                        #endregion

                        var mockData = new MockData
                        {
                            CaseID = complaint.CaseId,
                            AdjustmentComponent = adjustmentComponent,
                            Ipm = ipm
                        };
                        mc4103._127_022 = mockData;

                        #endregion
                    }
                    else
                    {
                        throw new Exception("GetMC4103Data Error. CaseId=" + representment.CaseId +
                                                " Complaints is null. CaseId=" + representment.CaseId);
                    }
                }

                return mc4103;
            }
            catch (Exception ex)
            {
                isException = true;
                return null;
            }
        }

        private void AssignValuesFromPostilionData(ref MC4103 mc4103, PostilionData postilionData, string caseId)
        {
                mc4103._11 = postilionData.SystemTraceAuditNr;
                //post_tran.datetime_tran_local
                mc4103._12 = postilionData.DatetimeTranLocal.ToString();
                //post_tran_cust.expiry_date
                mc4103._14 = postilionData.ExpiryDate;
                //post_tran_cust.merchant_type
                mc4103._18 = postilionData.MerchantType;
                //post_tran.pos_entry_mode
                mc4103._22 = postilionData.POSEntryMode;
                //post_tran_cust.card_seq_nr
                mc4103._23 = postilionData.CardSeqNr;
                //post_tran.pos_condition_code
                mc4103._25 = postilionData.PosConditionCode;
                //post_tran.RetrievalReferenceNumber 
                mc4103._37 = postilionData.RetrievalReferenceNr;
                //post_tran.auth_id_rsp
                mc4103._38 = postilionData.AuthIDRsp;
                //post_tran.rsp_code_rsp
                mc4103._39 = postilionData.RspCodeRsp;
                //post_tran_cust.service_restriction_code
                mc4103._40 = postilionData.ServiceRestrictionCode;
                //post_tran_cust.terminal_id
                mc4103._41 = postilionData.TerminalID;
                //post_tran_cust.card_acceptor_id_code 
                mc4103._42 = postilionData.CardAcceptorIDCode;
                //post_tran_cust.card_acceptor_name_loc
                mc4103._43 = postilionData.CardAcceptorNameLoc;
                //_123

                #region posDataCode

                var posDataCode = new PosDataCode
                {
                    //Position 1: post_tran_cust.pos_card_data_input_ability
                    PosCardDataInputAbility = postilionData.PosCardDataInputAbility,
                    //Position 2: post_tran_cust.pos_cardholder_auth_ability
                    PosCardholderAuthAbility = postilionData.PosCardholderAuthAbility,
                    //Position 3: post_tran_cust.pos_card_capture_ability
                    PosCardCaptureAbility = postilionData.PosCardCaptureAbility,
                    //Position 4: post_tran_cust.pos_operating_environment
                    PosOperatingEnvironment = postilionData.PosOperatingEnvironment,
                    //Position 5: post_tran_cust.pos_cardholder_present
                    PosCardholderPresent = postilionData.PosCardholderPresent,
                    //Position 6: post_tran_cust.pos_card_present
                    PosCardPresent = postilionData.PosCardPresent,
                    //Position 7: post_tran_cust.pos_card_data_input_mode
                    PosCardDataInputMode = postilionData.PosCardDataInputMode,
                    //Position 8: post_tran_cust.pos_cardholder_auth_method
                    PosCardholderAuthMethod = postilionData.POSCardholderAuthMethod,
                    //Position 9: post_tran_cust.pos_cardholder_auth_entity
                    PosCardholderAuthEntity = postilionData.PosCardholderAuthEntity,
                    //Position 10: post_tran_cust.pos_card_data_output_ability
                    PosCardDataOutputAbility = postilionData.PosCardDataOutputAbility,
                    //Position 11: post_tran_cust.pos_terminal_output_ability
                    PosTerminalOutputAbility = postilionData.PosTerminalOutputAbility,
                    //Position 12: post_tran_cust.pos_pin_capture_ability
                    PosPinCaptureAbility = postilionData.PosPinCaptureAbility,
                    //Position 13: post_tran_cust.pos_terminal_operator
                    PosTerminalOperator = postilionData.PosTerminalOperator,
                    //Position 14-15: post_tran_cust.pos_terminal_type
                    PosTerminalType = postilionData.PosTerminalType
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
                    throw new Exception("GetMC4103Data Error. CaseId=" + caseId +
                                        " posDataCode length is corupted. posDataCode: " + error);
                }

                #endregion

                mc4103._123 = posDataCode;            
        }
    }
}
