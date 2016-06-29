using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Models
{
    public class CaseItem
    {
        public string CaseId { get; set; }
        public long StageId { get; set; }
        public string StageCode { get; set; }
    }
}
