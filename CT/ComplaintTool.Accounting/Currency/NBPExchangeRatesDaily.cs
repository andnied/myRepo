using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComplaintTool.Accounting.Currency
{
    public class NBPExchangeRatesDaily
    {
        public string FileName { get; set; }
        public DateTime PublishDate { get; set; }
        public List<NBPExchange> Rates { get; set; }
    }
}
