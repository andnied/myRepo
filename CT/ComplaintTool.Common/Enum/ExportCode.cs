using System.ComponentModel;

namespace ComplaintTool.Common.Enum
{
    /// <summary>
    /// Case Filing export codes enum
    /// </summary>
    public enum ExportCode
    {
        [Description("FICN")]
        FICN,
        [Description("FIMFA")]
        FIMFA,
        [Description("FICU")]
        FICU,
        [Description("FAMN")]
        FAMN,
        [Description("FACU")]
        FACU,
        [Description("FAMU")]
        FAMU
    }
}
