using System;
using System.Globalization;
using ComplaintTool.Common.Utils;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.MCProImageInterface.Model;

namespace ComplaintTool.MCProImageInterface.Outgoing.Case
{
    public class CaseFilingRecordToFICNGenerator : CaseFilingRecordToXmlFileGenerator
    {
        public CaseFilingRecordToFICNGenerator(CaseFilingRecord record, ComplaintStage lastStage, string tempFolderPath)
            : base(record, lastStage, tempFolderPath)
        {
        }

        public override string FileType
        {
            get
            {
                return OutgoingFileType.FICN;
            }
        }

        public override string Generate()
        {
            var complaintValue = _unitOfWork.Repo<ComplaintRepo>().GetComplaintValueByStageId(_lastStage.StageId);
            var merchantName = _unitOfWork.Repo<CbdRepo>().GetMerchantNameByMID(_complaint.MID);

            var ficn = new Ficn();
            ficn.CaseType = this.GetCaseType(); //PA dla etapu PAOU i P dla etapu PCOU (ostatni etap z ComplaintStage)
            ficn.CardholderNumber = _record.PrimaryAccountNumber;
            ficn.ChargebackReasonCode = _record.ReasonCode;
            ficn.DisputeAmount = complaintValue.BookingAmount.GetValueOrDefault().ToString(); //po StageID z ComplaintValue.BookingAmount;
            ficn.DisputeAmountCurrencyCode = complaintValue.BookingCurrencyCode; //po StageID z ComplaintValue.BookingCurrency 
            ficn.ARN = _record.Complaint.ARN;
            ficn.ChargebackReferenceNumber = _record.CBReferenceNumber;
            ficn.FilingICA = _record.FilingICA;
            ficn.FiledAgainstICA = _record.FiledAgainstICA;
            ficn.MerchantName = merchantName; // z CBD po Complaint.MID pole KLN_NAZWA
            //ficn.SubmittedBy = _record.InsertUser; - nie uzupełniamy
            ficn.SubmittedDateTime = _record.SubmittedDate.GetValueOrDefault().ToString("yyMMdd", CultureInfo.CurrentUICulture.DateTimeFormat);
            ficn.Memo = _record.MemoText; //TODO: Do wyjaśnienia czy może nie z ComplaintRecord
            ficn.MembersFileNum = _complaint.CaseId;
            //ficn.UnjustEnrchmtChbkDt = // TODO brak pola
            //ficn.UnjustEnrchmtCrDt = // TODO brak pola
            //ficn.VirtualAccountNum = // Nie wrzucamy.
            ficn.ViolationDate = _record.ViolationDate.GetValueOrDefault().ToString("yyMMdd", CultureInfo.CurrentUICulture.DateTimeFormat); //(konwersja)
            ficn.ViolationCode = _record.ViolationCode;
            //ficn.RespDueDttm = // TODO brak pola  
            ficn.FileAs = "A";
            //ficn.UnjustEnrchmtChbkDt = _record.ChargebackDate
            //ficn.UnjustEnrchmtCrDt = _record.ChargebackDate

            return XmlUtil.SerializeToFile<Ficn>(ficn, this.ProcessFilePath).ToImageProEncoding();
        }

        private string GetCaseType()
        {
            switch(_lastStage.StageCode)
            {
                case "PAOU":
                    return "PA";
                case "PCOU":
                    return "P";
                default:
                    throw ComplaintCaseFilingExtractException.InvalidStageCode(_lastStage.StageCode);

            }
        }
    }
}
