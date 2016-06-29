using System.ComponentModel;

namespace ComplaintTool.Common.Enum
{
    /// <summary>
    /// Violation Code enum
    /// </summary>
    public enum ViolationCodeEnum
    {
        [Description("Witthdraw")]
        Witthdraw,
        [Description("Escalate")]
        Escalate,
        [Description("Rebuttal")]
        Rebuttal
    }
}
