using System;
using System.Data.Entity;
using System.Linq;
using ComplaintTool.Models;
using Organization = ComplaintTool.Common.Enum.Organization;

namespace ComplaintTool.DataAccess.Repos
{
    public class FinancialBalanceRepo : RepositoryBase
    {
        public FinancialBalanceRepo(DbContext context)
            : base(context)
        {
        }

        public bool AddFinancialBalance(long stageId, long? internalStageId, decimal? internalAmount,
            string internalAmountCurrencyCode)
        {
            var complaintStage = GetDbSet<ComplaintStage>().FirstOrDefault(x => x.StageId == stageId);
            if (complaintStage == null)
                throw new Exception(string.Format("Cannot find ComplaintStage with StageID = {0}", stageId));

            var complaint = GetDbSet<Complaint>().FirstOrDefault(x => x.CaseId.Equals(complaintStage.CaseId));
            if (complaint == null)
                throw new Exception(string.Format("Cannot find Complaint with CaseID = {0}", complaintStage.CaseId));

            if (internalStageId.HasValue)
            {
                if (!internalAmount.HasValue)
                    throw new Exception("InternalAmount parameter cannot be null for internal stage");
                if (string.IsNullOrWhiteSpace(internalAmountCurrencyCode))
                    throw new Exception("InternalAmountCurrencyCode parameter cannot be null for internal stage");

                var complaintStageInternal =
                    GetDbSet<ComplaintStageInternal>().FirstOrDefault(x => x.StageInternalId == internalStageId);
                if (complaintStageInternal == null)
                    throw new Exception(
                        string.Format("Cannot find ComplaintStageInternal with StageInternalID = {0}",
                            internalStageId.Value));

                var internalStageDefinition =
                        GetDbSet<InternalStageDefinition>().FirstOrDefault(
                            x => x.InternalStageId == complaintStageInternal.InternalStageId);
                if (internalStageDefinition == null)
                    throw new Exception(
                        string.Format("Cannot find InternalStageDefinition with InternalStageID = {0}",
                            complaintStageInternal.InternalStageId));

                var organization = (Organization)Enum.Parse(typeof(Organization), complaint.OrganizationId);

                var internalEuroAmount = CalculateExchangeSettlement(internalAmount.Value,
                    internalAmountCurrencyCode, "978", organization);

                var newComplaintFinancialBalance = new ComplaintFinacialBalance
                    {
                        CaseId = complaint.CaseId,
                        StageId = stageId,
                        InternalStageId = internalStageId,
                        MID = complaint.MID,
                        DebitAmount = internalStageDefinition.Debit ? internalAmount : null,
                        CreditAmount = internalStageDefinition.Credit ? internalAmount : null,
                        CurrencyCodeAmount = internalAmountCurrencyCode,
                        EuroCreditAmount = internalStageDefinition.Credit ? internalEuroAmount : null,
                        EuroDebitAmount = internalStageDefinition.Debit ? internalEuroAmount : null,
                        ExchangeDate = DateTime.Now,
                        IsPartial = complaintStageInternal.IsPartial,
                        InsertDate = DateTimeOffset.Now,
                        InsertUser = GetCurrentUser()
                    };
                GetDbSet<ComplaintFinacialBalance>().Add(newComplaintFinancialBalance);
            }
            else
            {
                if (!complaintStage.DefinitionStageId.HasValue)
                    throw new Exception(
                        string.Format("ComplaintStage record with StageId = {0} doesn't have a StageDefinitionId",
                            stageId));

                var stageDefinition = GetDbSet<StageDefinition>().FirstOrDefault(x => x.StageId == complaintStage.DefinitionStageId.Value);
                if (stageDefinition == null)
                    throw new Exception(string.Format("Cannot find StageDefinition with StageDefinitionID = {0}",
                        complaintStage.DefinitionStageId.Value));

                var complaintValue = GetDbSet<ComplaintValue>().Where(x => x.StageId == stageId).OrderByDescending(x => x.ValueId)
                   .FirstOrDefault();
                if (complaintValue == null)
                    throw new Exception(string.Format("Cannot find ComplaintValue with StageId = {0}", stageId));

                var newComplaintFinancialBalance = new ComplaintFinacialBalance
                {
                    CaseId = complaintStage.CaseId,
                    StageId = stageId,
                    InternalStageId = null,
                    MID = complaint.MID,
                    DebitAmount = stageDefinition.Debit ? complaintValue.StageAmount : null,
                    CreditAmount = stageDefinition.Credit ? complaintValue.StageAmount : null,
                    CurrencyCodeAmount = complaintValue.StageCurrencyCode,
                    EuroCreditAmount = stageDefinition.Credit ? complaintValue.EuroBookingAmount : null,
                    EuroDebitAmount = stageDefinition.Debit ? complaintValue.EuroBookingAmount : null,
                    ExchangeDate = complaintStage.StageDate.HasValue ? complaintStage.StageDate.Value.DateTime : (DateTime?)null,
                    IsPartial = complaintValue.IsPartial,
                    InsertDate = DateTimeOffset.Now,
                    InsertUser = GetCurrentUser()
                };
                GetDbSet<ComplaintFinacialBalance>().Add(newComplaintFinancialBalance);
            }
            return true;
        }
    }
}
