using System.ComponentModel;

namespace ComplaintTool.Common.Enum
{
    /// <summary>
    /// Complaint view status enum
    /// </summary>
    public enum ComplaintViewStatus
    {
        [Description("Open")]
        Open,
        [Description("Closed")]
        Closed,
        [Description("ClfReceived")]
        ClfReceived,
        [Description("ClfApproved")]
        ClfApproved,
        [Description("Hold")]
        Hold
    }
}
