using System;
using System.Collections.Generic;

namespace ComplaintTool.Accounting.Currency
{
    public class PKOExchangeRatesDaily
    {
        public string TableNumber { get; set; }
        public DateTime PublishDate { get; set; }
        public List<PKOExchange> Positions { get; set; }
    }
}
