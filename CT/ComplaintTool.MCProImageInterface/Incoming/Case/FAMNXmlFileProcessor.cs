using System;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.MCProImageInterface.Model;

namespace ComplaintTool.MCProImageInterface.Incoming.Case
{
    class FAMNXmlFileProcessor : CaseFilingXmlFileProcessor<Famn>
    {
        public FAMNXmlFileProcessor(ComplaintUnitOfWork unitOfWork, XmlFileInfo xmlFile) 
            : base(unitOfWork, xmlFile)
        {
        }

        protected override bool MatchCase(Famn famn)
        {
            _caseFilingRecord = new CaseFilingRecord();
            _caseFilingRecord.MasterCardCaseID = famn.CaseID;
            _caseFilingRecord.FilingICA = famn.FilingICA;
            _caseFilingRecord.FiledAgainstICA = famn.FiledAgainstICA;
            _caseFilingRecord.CaseType = famn.CaseType;
            _caseFilingRecord.PrimaryAccountNumber = famn.CardholderNumber;
            _caseFilingRecord.CBReferenceNumber = famn.ChargebackReferenceNumber;
            _caseFilingRecord.ResponseDueDate = CaseFilingUtil.ConvertToMComDate(famn.ResponseDueDateTime);
            _caseFilingRecord.SubmittedDate = CaseFilingUtil.ConvertToMComDate(famn.SubmittedDateTime);
            _caseFilingRecord.MemoText = famn.Memo;
            _caseFilingRecord.UnjustEnrichmentChargebackDate = CaseFilingUtil.ConvertToMComDate(famn.UnjustEnrchmtChbkDt);
            _caseFilingRecord.ReasonCode = famn.ChargebackReasonCode;
            _caseFilingRecord.ViolationCode = famn.ViolationCode;
            _caseFilingRecord.ViolationDate = CaseFilingUtil.ConvertToMComDate(famn.ViolationDate);
            _caseFilingRecord.FilingRegion = famn.FilingRegion;
            _caseFilingRecord.FiledAgainstRegion = famn.FilingAgainstRegion;
            _caseFilingRecord.CurrencyCode = famn.DisputeAmountCurrencyCode;
            return true;
        }

        protected override bool ProcessCase(Famn data)
        {
            throw new NotImplementedException();
        }
    }
}
