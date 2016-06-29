using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Common.Enum
{
    public enum DocumentExportStatus
    {
        [Description("Not Exported")]
        NotExported=1,
        [Description("Export Success")]
        Exported=2,
        [Description("Export Failed")]
        FailedExported=3
    }
}
