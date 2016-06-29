using System.ComponentModel;

namespace ComplaintTool.Common.Enum
{
    public enum FileSource
    {
        [Description("MC")]
        MC,
        [Description("VISA")]
        VISA,
        [Description("EVOG")]
        EVOG,
        [Description("ESRV")]
        ESRV,
        [Description("MERC")]
        MERC
    }
}
