using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Common.Enum
{
    public enum CrbReportStatus
    {
        [Description("New report")]
        New = 0,

        [Description("Processed report")]
        Send = 1,

        [Description("Manual modyfing report")]
        ManualModyfing = 10
    }
}
