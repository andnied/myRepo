using System.ComponentModel;

namespace ComplaintTool.Common.Enum
{
    public enum InternalStageResult
    {
        [Description("Success")]
        Success,
        [Description("FinancialBalanceNotAdded")]
        FinancialBalanceNotAdded,
        [Description("CaseNotClosed")]
        CaseNotClosed,
    }
}
