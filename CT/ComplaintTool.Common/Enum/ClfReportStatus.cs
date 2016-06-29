using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Common.Enum
{
    public enum ClfReportStatus
    {
        [Description("New Report")]
        New=0,

        [Description("Report Processed")]
        Processed=3
    }
}
