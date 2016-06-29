using System;
using System.Globalization;
using ComplaintTool.Common.Utils;
using ComplaintTool.Models;
using ComplaintTool.MCProImageInterface.Model;

namespace ComplaintTool.MCProImageInterface.Outgoing.Case
{
    public class CaseFilingRecordToFICUGenerator : CaseFilingRecordToXmlFileGenerator
    {
        public CaseFilingRecordToFICUGenerator(CaseFilingRecord record, ComplaintStage lastStage, string tempFolderPath)
            : base(record, lastStage, tempFolderPath)
        {
        }

        public override string FileType
        {
            get
            {
                return OutgoingFileType.FICU;
            }
        }

        public override string Generate()
        {
            var ficu = new Ficu();
            // wymagane
            ficu.CaseID  = _record.CaseId;
            // wymagane
            ficu.FilingICA  = _record.FilingICA;
            // opcjonalne
            ficu.FiledAgainstICA  = _record.CBReferenceNumber;
            // wymagane
            ficu.CaseType  = this.GetCaseType();
            // opcjonalne
            ficu.CardholderNumber  = _record.PrimaryAccountNumber;
            // opcjonalne
            //ficu.VirtualAccountNumber  = null; 
            // opcjonalne
            ficu.ARN = _complaint.ARN;
            //brak pola w specyfikacji excel
            //ficu.ChargebackReferenceNumber = null; 
            //brak pola w specyfikacji excel
            //ficu.MessageReasonCode  = null; 
            // opcjonalne
            //ficu.MembersFileNumber  = null; 
            // opcjonalne
            //ficu.MerchantName  = null; 
            // opcjonalne
            //ficu.DisputeAmount  = null; 
            // opcjonalne
            //ficu.DisputeAmountCurrencyCode  = null; 
            // opcjonalne
            //ficu.SenderSettlementAmount  = null; 
            // opcjonalne
            //ficu.SenderSettlementAmountCurrencyCode  = null;
            // opcjonalne
            //ficu.ResponseDueDateTime  = null; 
            // wymagane TODO brak informacji ???????????????????
            //ficu.ResponseCode  = null; 
            // opcjonalne
            //ficu.FileAs  = null; 
            // opcjonalne
            //ficu.CaseOwner  = null;
            // wymagane TODO skąd pobierana wartość??? chyba CaseFilingRecord - brak pola w DB
            //ficu.CaseState  = null; 
            // opcjonalne TODO skąd pobierana wartość??? chyba CaseFilingRecord - brak pola w DB
            //ficu.FilingResponseCode  = null; 
            // wymagane
            ficu.Memo  = _record.MemoText;
            // opcjonalne
            //ficu.ImageLength  = null; 
            // opcjonalne
            //ficu.SubmittedDateTime  = null; 
            // opcjonalne
            //ficu.SubmittedBy  = null; 
            // opcjonalne
            //ficu.TotalPagesNumber  = null; 
            // opcjonalne
            //ficu.RebuttedBy  = null; 
            // opcjonalne
            //ficu.RebuttedDateTime  = null; 
            // opcjonalne
            //ficu.RejectedBy  = null; 
            // opcjonalne
            //ficu.RejectedDateTime  = null; 
            // opcjonalne
            //ficu.WithdrawnBy  = null; 
            // opcjonalne
            //ficu.WithdrawnDateTime  = null; 

            return XmlUtil.SerializeToFile<Ficu>(ficu, this.ProcessFilePath).ToImageProEncoding();
        }

        private string GetCaseType()
        {
            return "?";
        }
    }
}
