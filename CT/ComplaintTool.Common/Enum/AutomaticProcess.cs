using System.ComponentModel;

namespace ComplaintTool.Common.Enum
{
    public enum AutomaticProcess
    {
        [Description("Representment")]
        Representment,
        [Description("CaseAutoClosed")]
        CaseAutoClosed,
        [Description("CLFAutoReject")]
        CLFAutoReject
    }
}
