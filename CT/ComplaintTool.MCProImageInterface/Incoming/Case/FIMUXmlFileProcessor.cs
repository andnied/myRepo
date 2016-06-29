using System;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.MCProImageInterface.Model;

namespace ComplaintTool.MCProImageInterface.Incoming.Case
{
    class FIMUXmlFileProcessor : CaseFilingXmlFileProcessor<Fimu>
    {
        public FIMUXmlFileProcessor(ComplaintUnitOfWork unitOfWork, XmlFileInfo xmlFile) 
            : base(unitOfWork, xmlFile)
        {
        }

        protected override bool MatchCase(Fimu fimu)
        {
            _caseFilingRecord = new CaseFilingRecord();
            _caseFilingRecord.MasterCardCaseID = fimu.CaseID;
            _caseFilingRecord.FilingICA = fimu.FilingICA;
            _caseFilingRecord.FiledAgainstICA = fimu.FiledAgainstICA;
            _caseFilingRecord.CaseType = fimu.CaseType;
            _caseFilingRecord.PrimaryAccountNumber = fimu.CardholderNumber;
            _caseFilingRecord.CBReferenceNumber = fimu.ChargebackReferenceNumber;
            _caseFilingRecord.ResponseDueDate = CaseFilingUtil.ConvertToMComDate(fimu.ResponseDueDateTime);
            _caseFilingRecord.SubmittedDate = CaseFilingUtil.ConvertToMComDate(fimu.SubmittedDateTime);
            _caseFilingRecord.RebuttalDate = CaseFilingUtil.ConvertToMComDate(fimu.RebuttedDateTime);
            _caseFilingRecord.RejectedDate = CaseFilingUtil.ConvertToMComDate(fimu.RejectedDateTime);
            _caseFilingRecord.AcceptedDate = CaseFilingUtil.ConvertToMComDate(fimu.AcceptedDateTime);
            _caseFilingRecord.EscalationDate = CaseFilingUtil.ConvertToMComDate(fimu.EscalatedDateTime);
            _caseFilingRecord.MemoText = fimu.Memo;
            _caseFilingRecord.ReasonCode = fimu.ChargebackReasonCode;
            _caseFilingRecord.ViolationCode = fimu.ViolationCode;
            _caseFilingRecord.ViolationDate = CaseFilingUtil.ConvertToMComDate(fimu.ViolationDate);
            _caseFilingRecord.WithdrawnDate = CaseFilingUtil.ConvertToMComDate(fimu.WithdrawnDateTime);
            _caseFilingRecord.FilingRegion = fimu.FilingRegion;
            _caseFilingRecord.FiledAgainstRegion = fimu.FilingAgainstRegion;
            _caseFilingRecord.CurrencyCode = fimu.DisputeAmountCurrencyCode;
            return true;
        }

        protected override bool ProcessCase(Fimu data)
        {
            throw new NotImplementedException();
        }
    }
}
