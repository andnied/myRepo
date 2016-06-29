using System;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.MCProImageInterface.Model;

namespace ComplaintTool.MCProImageInterface.Incoming.Case
{
    class FIMDXmlFileProcessor : CaseFilingXmlFileProcessor<Fimd>
    {
        public FIMDXmlFileProcessor(ComplaintUnitOfWork unitOfWork, XmlFileInfo xmlFile) 
            : base(unitOfWork, xmlFile)
        {
        }

        protected override bool MatchCase(Fimd fimd)
        {
            _caseFilingRecord = new CaseFilingRecord();
            _caseFilingRecord.MasterCardCaseID = fimd.CaseID;
            _caseFilingRecord.FilingICA = fimd.FilingICA;
            _caseFilingRecord.FiledAgainstICA = fimd.FiledAgainstICA;
            _caseFilingRecord.CaseType = fimd.CaseType;
            _caseFilingRecord.PrimaryAccountNumber = fimd.CardholderNumber;
            _caseFilingRecord.CBReferenceNumber = fimd.ChargebackReferenceNumber;
            _caseFilingRecord.ResponseDueDate = CaseFilingUtil.ConvertToMComDate(fimd.ResponseDueDateTime);
            _caseFilingRecord.SubmittedDate = CaseFilingUtil.ConvertToMComDate(fimd.SubmittedDateTime);
            _caseFilingRecord.RebuttalDate = CaseFilingUtil.ConvertToMComDate(fimd.RebuttedDateTime);
            _caseFilingRecord.RejectedDate = CaseFilingUtil.ConvertToMComDate(fimd.RejectedDateTime);
            _caseFilingRecord.AcceptedDate = CaseFilingUtil.ConvertToMComDate(fimd.AcceptedDateTime);
            _caseFilingRecord.EscalationDate = CaseFilingUtil.ConvertToMComDate(fimd.EscalatedDateTime);
            _caseFilingRecord.ReasonCode = fimd.ChargebackReasonCode;
            _caseFilingRecord.ViolationCode = fimd.ViolationCode;
            _caseFilingRecord.ViolationDate = CaseFilingUtil.ConvertToMComDate(fimd.ViolationDateTime);
            _caseFilingRecord.WithdrawnDate = CaseFilingUtil.ConvertToMComDate(fimd.WithdrawnDateTime);
            _caseFilingRecord.Ruling = fimd.Ruling;
            _caseFilingRecord.RulingDate = CaseFilingUtil.ConvertToMComDate(fimd.RulingDateTime);
            _caseFilingRecord.FilingFee = CaseFilingUtil.ConvertToMComDecimal(fimd.FilingFee);
            _caseFilingRecord.TechFee = CaseFilingUtil.ConvertToMComDecimal(fimd.TechViolationFee);
            _caseFilingRecord.HubsiteFee = CaseFilingUtil.ConvertToMComDecimal(fimd.HubsiteFee);
            _caseFilingRecord.AdminFee = CaseFilingUtil.ConvertToMComDecimal(fimd.AdminFee);
            _caseFilingRecord.AppealFee = CaseFilingUtil.ConvertToMComDecimal(fimd.CaseDecisionAppealFee);
            _caseFilingRecord.WithdrawingFee = CaseFilingUtil.ConvertToMComDecimal(fimd.CaseWithdrawFee);
            _caseFilingRecord.RulingAmount = CaseFilingUtil.ConvertToMComDecimal(fimd.RulingAmount);
            _caseFilingRecord.RulingCurrencyCode = fimd.RulingAmountCurrencyCode;
            _caseFilingRecord.FilingRegion = fimd.FilingRegion;
            _caseFilingRecord.FiledAgainstRegion = fimd.FilingAgainstRegion;
            _caseFilingRecord.CurrencyCode = fimd.DisputeAmountCurrencyCode;
            return true;
        }

        protected override bool ProcessCase(Fimd data)
        {
            throw new NotImplementedException();
        }
    }
}
