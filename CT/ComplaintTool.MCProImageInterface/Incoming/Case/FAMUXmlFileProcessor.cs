using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.MCProImageInterface.Model;

namespace ComplaintTool.MCProImageInterface.Incoming.Case
{
    class FAMUXmlFileProcessor : CaseFilingXmlFileProcessor<Famu>
    {
        public FAMUXmlFileProcessor(ComplaintUnitOfWork unitOfWork, XmlFileInfo xmlFile) 
            : base(unitOfWork, xmlFile)
        {
        }

        protected override bool MatchCase(Famu famu)
        {
            _caseFilingRecord = new CaseFilingRecord();
            _caseFilingRecord.MasterCardCaseID = famu.CaseID;
            _caseFilingRecord.FilingICA = famu.FilingICA;
            _caseFilingRecord.FiledAgainstICA = famu.FilingAgainstICA;
            _caseFilingRecord.CaseType = famu.CaseType;
            _caseFilingRecord.PrimaryAccountNumber = famu.CardholderNumber;
            _caseFilingRecord.CBReferenceNumber = famu.ChargebackReferenceNumber;
            _caseFilingRecord.ResponseDueDate = CaseFilingUtil.ConvertToMComDate(famu.ResponseDueDateTime);
            _caseFilingRecord.SubmittedDate = CaseFilingUtil.ConvertToMComDate(famu.SubmittedDateTime);
            _caseFilingRecord.RebuttalDate = CaseFilingUtil.ConvertToMComDate(famu.RebuttedDateTime);
            _caseFilingRecord.RejectedDate = CaseFilingUtil.ConvertToMComDate(famu.RejectedDateTime);
            _caseFilingRecord.AcceptedDate = CaseFilingUtil.ConvertToMComDate(famu.AcceptedDateTime);
            _caseFilingRecord.EscalationDate = CaseFilingUtil.ConvertToMComDate(famu.EscalatedDateTime);
            _caseFilingRecord.MemoText = famu.Memo;
            _caseFilingRecord.ReasonCode = famu.ChargebackReasonCode;
            _caseFilingRecord.ViolationCode = famu.ViolationCode;
            _caseFilingRecord.ViolationDate = CaseFilingUtil.ConvertToMComDate(famu.ViolationDate);
            _caseFilingRecord.WithdrawnDate = CaseFilingUtil.ConvertToMComDate(famu.WithdrawnDateTime);
            _caseFilingRecord.FilingRegion = famu.FilingRegion;
            _caseFilingRecord.FiledAgainstRegion = famu.FilingAgainstRegion;
            _caseFilingRecord.CurrencyCode = famu.DisputeAmountCurrencyCode;
            return true;
        }

        protected override bool ProcessCase(Famu data)
        {
            throw new NotImplementedException();
        }
    }
}
