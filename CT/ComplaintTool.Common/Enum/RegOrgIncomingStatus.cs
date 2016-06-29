using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Common.Enum
{
    public enum RegOrgIncomingStatus
    {
        [Description("New")]
        NotProcessed = 2,
        [Description("Processed")]
        Processed = 3,
    }
}
