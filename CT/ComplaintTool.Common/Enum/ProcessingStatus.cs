using System.ComponentModel;

namespace ComplaintTool.Common.Enum
{
    public enum ProcessingStatus
    {
        [Description("New")]
        New = 0,
        [Description("ManualProcess")]
        ManualProcess = 1,
        [Description("AutomaticRepresentment")]
        AutomaticRepresentment = 2,
        [Description("Administration")]
        Administration = 3,
        [Description("Automatic")]
        Automatic = 4
    }
}
