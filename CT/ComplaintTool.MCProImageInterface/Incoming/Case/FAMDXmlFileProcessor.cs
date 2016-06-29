using System;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.MCProImageInterface.Model;

namespace ComplaintTool.MCProImageInterface.Incoming.Case
{
    class FAMDXmlFileProcessor : CaseFilingXmlFileProcessor<Famd>
    {
        public FAMDXmlFileProcessor(ComplaintUnitOfWork unitOfWork, XmlFileInfo xmlFile) 
            : base(unitOfWork, xmlFile)
        {
        }

        protected override bool MatchCase(Famd famd)
        {
            _caseFilingRecord = new CaseFilingRecord();
            _caseFilingRecord.MasterCardCaseID = famd.CaseID;
            _caseFilingRecord.FilingICA = famd.FilingICA;
            _caseFilingRecord.FiledAgainstICA = famd.FiledAgainstICA;
            _caseFilingRecord.CaseType = famd.CaseType;
            _caseFilingRecord.PrimaryAccountNumber = famd.CardholderNumber;
            _caseFilingRecord.CBReferenceNumber = famd.ChargebackReferenceNumber;
            _caseFilingRecord.ResponseDueDate = CaseFilingUtil.ConvertToMComDate(famd.ResponseDueDateTime);
            _caseFilingRecord.SubmittedDate = CaseFilingUtil.ConvertToMComDate(famd.SubmittedDateTime);
            _caseFilingRecord.RebuttalDate = CaseFilingUtil.ConvertToMComDate(famd.RebuttedDateTime);
            _caseFilingRecord.RejectedDate = CaseFilingUtil.ConvertToMComDate(famd.RejectedDateTime);
            _caseFilingRecord.AcceptedDate = CaseFilingUtil.ConvertToMComDate(famd.AcceptedDateTime);
            _caseFilingRecord.EscalationDate = CaseFilingUtil.ConvertToMComDate(famd.EscalatedDateTime);
            _caseFilingRecord.ReasonCode = famd.ChargebackReasonCode;
            _caseFilingRecord.ViolationCode = famd.ViolationCode;
            _caseFilingRecord.ViolationDate = CaseFilingUtil.ConvertToMComDate(famd.ViolationDate);
            _caseFilingRecord.WithdrawnDate = CaseFilingUtil.ConvertToMComDate(famd.WithdrawnDateTime);
            _caseFilingRecord.Ruling = famd.Ruling;
            _caseFilingRecord.RulingDate = CaseFilingUtil.ConvertToMComDate(famd.RulingDateTime);
            _caseFilingRecord.CaseFee = CaseFilingUtil.ConvertToMComDecimal(famd.ComplianceCaseFee);
            _caseFilingRecord.FilingFee = CaseFilingUtil.ConvertToMComDecimal(famd.FilingFee);
            _caseFilingRecord.TechFee = CaseFilingUtil.ConvertToMComDecimal(famd.TechFee);
            _caseFilingRecord.HubsiteFee = CaseFilingUtil.ConvertToMComDecimal(famd.HubsiteFee);
            _caseFilingRecord.AdminFee = CaseFilingUtil.ConvertToMComDecimal(famd.AdminFee);
            _caseFilingRecord.AppealFee = CaseFilingUtil.ConvertToMComDecimal(famd.CaseDecisionAppealFee);
            _caseFilingRecord.WithdrawingFee = CaseFilingUtil.ConvertToMComDecimal(famd.CaseWithdrawingFee);
            _caseFilingRecord.RulingAmount = CaseFilingUtil.ConvertToMComDecimal(famd.RulingAmount);
            _caseFilingRecord.RulingCurrencyCode = famd.RulingAmountCurrencyCode;
            _caseFilingRecord.FilingRegion = famd.FilingRegion;
            _caseFilingRecord.FiledAgainstRegion = famd.FilingAgainstRegion;
            _caseFilingRecord.CurrencyCode = famd.DisputeAmountCurrencyCode;
            return true;
        }

        protected override bool ProcessCase(Famd data)
        {
            // TODO Zmiana stage na RULL w przypadku nie pustych wartości w polach rull’ w polu Rulling. Zaczytujemy kody response.
            throw new NotImplementedException();
        }
    }
}
