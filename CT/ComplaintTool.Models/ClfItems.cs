using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Models
{
    public class ClfItems
    {
        public CLFReportItem IncomingItem { get; set; }
        public CLFReportItem OutgoingItem { get; set; }
        public StageDefinition StageDefinition { get; set; }
    }
}
