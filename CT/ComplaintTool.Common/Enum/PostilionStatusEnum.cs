using System.ComponentModel;

namespace ComplaintTool.Common.Enum
{
    /// <summary>
    /// Postilion Status Enum
    /// </summary>
    public enum PostilionStatusEnum
    {
        [Description("ApprovedOrCompletedSuccessfully")]
        ApprovedOrCompletedSuccessfully = 0,
        [Description("Error")]
        Error = 6,
        [Description("FormatError")]
        FormatError = 30,
        [Description("RoutingError")]
        RoutingError = 92,
        [Description("DuplicateTransaction")]
        DuplicateTransaction = 94,
        [Description("SystemMalfunction")]
        SystemMalfunction = 96
    }
}
