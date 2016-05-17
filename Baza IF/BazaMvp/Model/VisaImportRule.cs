using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaMvp.Model
{
    public class VisaImportRule
    {
        public int Id { get; set; }
        public string ReportingFor { get; set; }

        public virtual Participant Participant { get; set; }
    }
}
