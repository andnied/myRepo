using System.ComponentModel;

namespace ComplaintTool.Common.Enum
{
    public enum CaseSuffix
    {
        [Description("CB")]
        Chargeback,
        [Description("RC")]
        RetainedCard,
        [Description("GF")]
        GoodFaithLetter
    }
}
