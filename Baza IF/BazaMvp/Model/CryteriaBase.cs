using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaMvp.Model
{
    [Table("Cryteria")]
    public abstract class CryteriaBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string Description { get; set; }
        public decimal Fee { get; set; }
        public decimal IFNumber { get; set; }
        public decimal IFBase { get; set; }
        public decimal DeviationReg1 { get; set; }
        public decimal DeviationReg2 { get; set; }
        public decimal DeviationReg3 { get; set; }
        public decimal DeviationIF { get; set; }
        public decimal DeviationIFMin { get; set; }
        public bool IsActive { get; set; }
    }
}
