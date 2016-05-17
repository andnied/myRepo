using BazaMvp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaMvp.Model
{
    [Serializable]
    public class VisaRecord : InputBase
    {
        public string ReportingFor { get; set; }
        [VisaRecordAttribute(Position = 4)]
        public string TransactionType { get; set; }
        [VisaRecordAttribute(Position = 7)]
        public string TransactionCode { get; set; }
        [VisaRecordAttribute(Position = 10)]
        public string Region { get; set; }
        [VisaRecordAttribute(Position = 13)]
        public string TerminalCard { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal InterChangeAmount { get; set; }
        public decimal ReimbursementFee { get; set; }
        public string Currency1 { get; set; }
        public string Currency2 { get; set; }

        //public override int FileType { get { return 2; } }
    }
}
