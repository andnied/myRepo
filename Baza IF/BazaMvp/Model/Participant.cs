using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaMvp.Model
{
    public class Participant
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual IEnumerable<VisaImportRule> VisaImportRules { get; set; }
    }
}
