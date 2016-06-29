using System;
using System.Xml.Serialization;

namespace ComplaintTool.MCProImageInterface.Model
{
    [XmlType]
    public class CurrencyCodesAmountsOriginal
    {
        [XmlElement(Order = 1, ElementName = "s1")]
        public string S1CurrencyCodeOriginalTransactionAmount { get; set; }

        [XmlElement(Order = 2, ElementName = "s2")]
        public string S2CurrencyCodeOriginalReconciliationAmount { get; set; }

        public override string ToString()
        {
            return String.Format("{0}{1}", S1CurrencyCodeOriginalTransactionAmount, S2CurrencyCodeOriginalReconciliationAmount);
        }
    }
}
