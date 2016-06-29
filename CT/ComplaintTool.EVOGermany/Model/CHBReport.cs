using System;
using System.Collections.Generic;
using System.Globalization;
using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.EVOGermany.Model.Oraganizations;
using ComplaintTool.Common.Config;
using ComplaintTool.Models;
using ComplaintTool.Common.CTLogger;
using ComplaintTool.DataAccess.Utils;

namespace ComplaintTool.EVOGermany.Model
{
    public class ChbReport
    {
        private static readonly ILogger Logger = LogManager.GetLogger();
        private Helper _helper;
        private IReadOnlyDictionary<short, CurrencyCode> _currencyCodeList;
        private ComplaintUnitOfWork _unitOfWork = null;

        public ChbReport(ComplaintUnitOfWork unitOfWork)
        {
            _helper = new Helper();
            _currencyCodeList = ComplaintDictionaires.Instance.CurrencyCodes;
            _unitOfWork = unitOfWork;
        }

        public List<ChbRecord> GetChbRecordList(IEnumerable<CRBReportItem> crbReportItems)
        {
            var chbRecordList = new List<ChbRecord>();

            foreach (var crbItem in crbReportItems)
            {
                try
                {
                    var chbRecord = GetRecord(crbItem);
                    if (chbRecord != null)
                    {
                        LogInfo(chbRecord);
                        chbRecordList.Add(chbRecord);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogComplaintException(ex);
                }
            }

            return chbRecordList;
        }

        private ChbRecord GetRecord(CRBReportItem crbItem)
        {
            var complaint = _unitOfWork.Repo<ComplaintRepo>().FindByCaseId(crbItem.CaseId);
            var complaintRecord = crbItem.RecordsId.HasValue ? _unitOfWork.Repo<ComplaintRepo>().FindRecordByRecordId(crbItem.RecordsId) : null;
            var chbRecord = new ChbRecord() { ItemId = crbItem.ItemId };

            if (!complaint.PostTranId.HasValue)
                return null;

            chbRecord.Brand = complaint.BrandName;
            chbRecord.Mid = complaint.MID;
            chbRecord.CaseId = complaint.CaseId;
            chbRecord.Pan = GetPanNumber(complaint);
            chbRecord.ARN = complaint.ARN;
            chbRecord.ReasonCode = !string.IsNullOrEmpty(complaint.ReasonCode) ? complaint.ReasonCode.ToString(CultureInfo.InvariantCulture) : string.Empty;
            chbRecord.AuthorisationCode = complaint.ACode;
            chbRecord.MpiLogFlag = !string.IsNullOrWhiteSpace(complaint.ECommerce) ? complaint.ECommerce : "N";
            chbRecord.SettlementDate = complaint.SettlementDate.Value.ToString("yyyy-MM-dd");
            if (!String.IsNullOrEmpty(complaint.TransactionCurrencyCode))
            chbRecord = FillTransaction(complaint, chbRecord);

            var complaintValue = _unitOfWork.Repo<ComplaintRepo>().FindComplaintValueByValueId(crbItem.ValueId);
            if (complaintValue != null)
                chbRecord = FillStages(complaintValue, chbRecord);

            IReportOrganization reportOrganization = GetReportOrganization(complaint);
            if (complaintRecord != null)
            {
                chbRecord.Narritive = complaintRecord.Narritive;
                if(complaintRecord.IncomingId.HasValue)
                chbRecord = reportOrganization.FillRecord(chbRecord, complaintRecord.IncomingId);
            }

            var complaintStage = _unitOfWork.Repo<ComplaintRepo>().GetStageById(crbItem.StageId);
            if (complaintStage != null)
                chbRecord = FillComplaintStages(complaintStage, complaint, chbRecord);

            return chbRecord;
        }

        private void LogInfo(ChbRecord chbRecord)
        {
            string resultInfo=chbRecord.GetNullProperties();
            if (resultInfo != null)
            {
                string info = string.Format("Cannot find values for mandatory fields: {0}", resultInfo);
                //Logger.Info(info);
            }
        }

        private string GetPanNumber(Complaint complaint)
        {
            string pan, panExtention;
            _unitOfWork.Repo<ComplaintRepo>().GetPanByCaseId(complaint.CaseId, out pan, out panExtention);
            return pan + (panExtention ?? string.Empty);

            //if (!string.IsNullOrEmpty(complaint.PANExtention))
            //{
            //    if (complaint.PANExtention == "000")
            //        return complaint.PAN;
            //    else
            //        return complaint.PAN + complaint.PANExtention;
            //}
            //else
            //{
            //    return complaint.PAN;
            //}
        }

        private ChbRecord FillTransaction(Complaint complaint, ChbRecord chbRecord)
        {
            CurrencyCode transactionCurrencyCode;
            short transactionNumericCurrencyCode;
            int transactionAmountExponent = 0;
            string transactionamount = string.Empty;
            chbRecord.TransactionAmountSign = complaint.TransactionAmountSign;
            transactionNumericCurrencyCode = short.Parse(complaint.TransactionCurrencyCode);
            transactionCurrencyCode = _currencyCodeList[transactionNumericCurrencyCode];
            if (complaint.TransactionAmount != null)
                transactionamount = _helper.AmountToString(transactionCurrencyCode, complaint.TransactionAmount.Value, 12, out transactionAmountExponent);
            else
                transactionamount = string.Empty;
            chbRecord.TransactionAmount = transactionamount;
            chbRecord.TransactionAmountExponent = transactionAmountExponent.ToString(CultureInfo.InvariantCulture);
            chbRecord.TransactionCurrencyCode = transactionCurrencyCode.Alphabetical.ToUpper();
            chbRecord.TransactionDate = complaint.TransactionDate.HasValue ? complaint.TransactionDate.ToString() : null;

            return chbRecord;
        }

        private ChbRecord FillStages(ComplaintValue complaintValue, ChbRecord chbRecord)
        {
            chbRecord.PartialFlag = complaintValue.IsPartial.HasValue ? complaintValue.IsPartial.Value ? "Y" : "N" : "N";

            if (!string.IsNullOrWhiteSpace(complaintValue.StageCurrencyCode))
            {
                CurrencyCode stageCurrencyCode;
                short stageNumericCurrencyCode;
                int stageAmountExponent = 0;
                string stageAmount = string.Empty;
                stageNumericCurrencyCode = short.Parse(complaintValue.StageCurrencyCode);

                stageCurrencyCode = _currencyCodeList[stageNumericCurrencyCode];
                if (complaintValue.StageAmount != null)
                    stageAmount = _helper.AmountToString(stageCurrencyCode,complaintValue.StageAmount.Value, 12, out stageAmountExponent);
                else
                    stageAmount = string.Empty;
                chbRecord.StageAmountSign = complaintValue.StageAmountSign;
                chbRecord.StageAmount = stageAmount;
                chbRecord.StageAmountExponent = stageAmountExponent.ToString(CultureInfo.InvariantCulture);
                chbRecord.StageCurrencyCode = stageCurrencyCode.Alphabetical.ToUpper();
            }

            if (!string.IsNullOrWhiteSpace(complaintValue.BookingCurrencyCode)) 
            {
                CurrencyCode bookingCurrencyCode;
                short bookingNumericCurrencyCode;
                int bookingAmountExponent = 0;
                string bookingAmount = string.Empty;
                bookingNumericCurrencyCode = short.Parse(complaintValue.BookingCurrencyCode);
                bookingCurrencyCode = _currencyCodeList[bookingNumericCurrencyCode];
                if (complaintValue.BookingAmount != null)
                    bookingAmount = _helper.AmountToString(bookingCurrencyCode,complaintValue.BookingAmount.Value, 12, out bookingAmountExponent);
                else
                    bookingAmount = string.Empty;
                chbRecord.BookingAmountSign = complaintValue.BookingAmountSign;
                chbRecord.BookingAmount = bookingAmount;
                chbRecord.BookingAmountExponent = bookingAmountExponent.ToString(CultureInfo.InvariantCulture);
                chbRecord.BookingCurrencyCode = bookingCurrencyCode.Alphabetical.ToUpper();
            }

            if (complaintValue.EuroBookingAmount.HasValue)
            {
                CurrencyCode euroBookingCurrencyCode;
                short euroBookingNumericCurrencyCode = 978;
                int euroBookingAmountExponent = 0;
                string euroBookingAmount = string.Empty;
                euroBookingCurrencyCode = _currencyCodeList[euroBookingNumericCurrencyCode];
                if (complaintValue.EuroBookingAmount != null)
                    euroBookingAmount = _helper.AmountToString(euroBookingCurrencyCode, complaintValue.EuroBookingAmount.Value, 12, out euroBookingAmountExponent);
                else
                    euroBookingAmount = string.Empty;
                chbRecord.EuroBookingAmountSign =  complaintValue.EuroBookingAmountSign;
                chbRecord.EuroBookingAmount = euroBookingAmount;
                chbRecord.EuroBookingAmountExponent = euroBookingAmountExponent.ToString(CultureInfo.InvariantCulture);
            }

            return chbRecord;
        }

        private ChbRecord FillComplaintStages(ComplaintStage complaintStage, Complaint complaint, ChbRecord chbRecord)
        {
            if (!string.IsNullOrEmpty(complaintStage.StageCode))
                chbRecord.Stage = complaintStage.StageCode.ToString(CultureInfo.InvariantCulture);

            if (complaintStage.StageDate.HasValue)
                chbRecord.StageDate = complaintStage.StageDate.Value.ToString();
            else
                chbRecord.StageDate = complaint.StatusDate.HasValue ? complaint.StatusDate.Value.ToString() : null;

            return chbRecord;
        }

        private IReportOrganization GetReportOrganization(Complaint complaint)
        {
            if(ComplaintTool.Common.Enum.Organization.MC.ToString().Equals(complaint.OrganizationId))
                return new MCReportOrganization(_unitOfWork);

            if (ComplaintTool.Common.Enum.Organization.VISA.ToString().Equals(complaint.OrganizationId))
                return new VisaReportOrganization(_unitOfWork);

            throw new Exception(string.Format("Unknown report organization {0} for complaint CaseId: {1}",complaint.OrganizationId,complaint.CaseId));
            
        }
    }

}
