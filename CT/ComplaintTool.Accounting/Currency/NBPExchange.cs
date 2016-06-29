using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComplaintTool.Accounting.Currency
{
    public class NBPExchange
    {
        public string Name { get; set; }
        public int Conversion { get; set; }
        public string CurrencyCode { get; set; }
        public decimal AverageExchangeRate { get; set; }
    }
}
