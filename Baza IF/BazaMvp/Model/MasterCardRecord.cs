using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaMvp.Model
{
    [Serializable]
    public class MasterCardRecord : InputBase
    {
        public string Product { get; set; }
        public string Region { get; set; }
        public string SubRegion { get; set; }
        public string Ica { get; set; }
        public int Cycle { get; set; }
        public string TransFunc { get; set; }
        public string ProcessingCode { get; set; }
        public string Ird { get; set; }
        public int Count { get; set; }
        public decimal ReconAmount { get; set; }
        public decimal TransFee { get; set; }
        public string CurrencyCode { get; set; }

        //public override int FileType { get { return 1; } }
    }
}
