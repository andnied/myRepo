using System.Xml.Serialization;

namespace ComplaintTool.MCProImageInterface.Model
{
    [XmlType]
    public class CurrencyExponents
    {
        [XmlElement(Order = 1, ElementName = "s1")]
        public string S1CurrencyCode { get; set; }

        [XmlElement(Order = 2, ElementName = "s2")]
        public string S2CurrencyExponent { get; set; }

        //next doesn't meet the documetation...

        [XmlElement(Order = 3, ElementName = "s3")]
        public string S3CurrencyCode { get; set; }

        [XmlElement(Order = 4, ElementName = "s4")]
        public string S4CurrencyExponent { get; set; }

        [XmlElement(Order = 5, ElementName = "s5")]
        public string S5CurrencyCode { get; set; }

        [XmlElement(Order = 6, ElementName = "s6")]
        public string S6CurrencyExponent { get; set; }

        [XmlElement(Order = 7, ElementName = "s7")]
        public string S7CurrencyCode { get; set; }

        [XmlElement(Order = 8, ElementName = "s8")]
        public string S8CurrencyExponent { get; set; }

        [XmlElement(Order = 9, ElementName = "s9")]
        public string S9CurrencyCode { get; set; }

        [XmlElement(Order = 10, ElementName = "s10")]
        public string S10CurrencyExponent { get; set; }

        [XmlElement(Order = 11, ElementName = "s11")]
        public string S11CurrencyCode { get; set; }

        [XmlElement(Order = 12, ElementName = "s12")]
        public string S12CurrencyExponent { get; set; }

        [XmlElement(Order = 13, ElementName = "s13")]
        public string S13CurrencyCode { get; set; }

        [XmlElement(Order = 14, ElementName = "s14")]
        public string S14CurrencyExponent { get; set; }

        [XmlElement(Order = 15, ElementName = "s15")]
        public string S15CurrencyCode { get; set; }

        [XmlElement(Order = 16, ElementName = "s16")]
        public string S16CurrencyExponent { get; set; }

        [XmlElement(Order = 17, ElementName = "s17")]
        public string S17CurrencyCode { get; set; }

        [XmlElement(Order = 18, ElementName = "s18")]
        public string S18CurrencyExponent { get; set; }

        [XmlElement(Order = 19, ElementName = "s19")]
        public string S19CurrencyCode { get; set; }

        [XmlElement(Order = 20, ElementName = "s20")]
        public string S20CurrencyExponent { get; set; }

        [XmlElement(Order = 21, ElementName = "s21")]
        public string S21CurrencyCode { get; set; }

        [XmlElement(Order = 22, ElementName = "s22")]
        public string S22CurrencyExponent { get; set; }

        [XmlElement(Order = 23, ElementName = "s23")]
        public string S23CurrencyCode { get; set; }

        [XmlElement(Order = 24, ElementName = "s24")]
        public string S24CurrencyExponent { get; set; }

        [XmlElement(Order = 25, ElementName = "s25")]
        public string S25CurrencyCode { get; set; }

        [XmlElement(Order = 26, ElementName = "s26")]
        public string S26CurrencyExponent { get; set; }

        [XmlElement(Order = 27, ElementName = "s27")]
        public string S27CurrencyCode { get; set; }

        [XmlElement(Order = 28, ElementName = "s28")]
        public string S28CurrencyExponent { get; set; }

        [XmlElement(Order = 29, ElementName = "s29")]
        public string S29CurrencyCode { get; set; }

        [XmlElement(Order = 30, ElementName = "s30")]
        public string S30CurrencyExponent { get; set; }

        public string GetExponent(string currencyCode)
        {
            if (!string.IsNullOrEmpty(S1CurrencyCode) && S1CurrencyCode.Equals(currencyCode))
            {
                return S2CurrencyExponent;
            }
            if (!string.IsNullOrEmpty(S3CurrencyCode) && S3CurrencyCode.Equals(currencyCode))
            {
                return S4CurrencyExponent;
            }
            if (!string.IsNullOrEmpty(S5CurrencyCode) && S5CurrencyCode.Equals(currencyCode))
            {
                return S6CurrencyExponent;
            }
            if (!string.IsNullOrEmpty(S7CurrencyCode) && S7CurrencyCode.Equals(currencyCode))
            {
                return S8CurrencyExponent;
            }
            if (!string.IsNullOrEmpty(S9CurrencyCode) && S9CurrencyCode.Equals(currencyCode))
            {
                return S10CurrencyExponent;
            }
            if (!string.IsNullOrEmpty(S11CurrencyCode) && S11CurrencyCode.Equals(currencyCode))
            {
                return S12CurrencyExponent;
            }
            if (!string.IsNullOrEmpty(S13CurrencyCode) && S13CurrencyCode.Equals(currencyCode))
            {
                return S14CurrencyExponent;
            }
            if (!string.IsNullOrEmpty(S15CurrencyCode) && S15CurrencyCode.Equals(currencyCode))
            {
                return S16CurrencyExponent;
            }
            if (!string.IsNullOrEmpty(S17CurrencyCode) && S17CurrencyCode.Equals(currencyCode))
            {
                return S18CurrencyExponent;
            }
            if (!string.IsNullOrEmpty(S19CurrencyCode) && S19CurrencyCode.Equals(currencyCode))
            {
                return S20CurrencyExponent;
            }
            if (!string.IsNullOrEmpty(S21CurrencyCode) && S21CurrencyCode.Equals(currencyCode))
            {
                return S22CurrencyExponent;
            }
            if (!string.IsNullOrEmpty(S23CurrencyCode) && S23CurrencyCode.Equals(currencyCode))
            {
                return S24CurrencyExponent;
            }
            if (!string.IsNullOrEmpty(S25CurrencyCode) && S25CurrencyCode.Equals(currencyCode))
            {
                return S26CurrencyExponent;
            }
            if (!string.IsNullOrEmpty(S27CurrencyCode) && S27CurrencyCode.Equals(currencyCode))
            {
                return S28CurrencyExponent;
            }
            if (!string.IsNullOrEmpty(S29CurrencyCode) && S29CurrencyCode.Equals(currencyCode))
            {
                return S30CurrencyExponent;
            }
            return null;
        }

        public override string ToString()
        {
            return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}{23}{24}{25}{26}{27}{28}{29}",
                S1CurrencyCode,
                S2CurrencyExponent,
                S3CurrencyCode,
                S4CurrencyExponent,
                S5CurrencyCode,
                S6CurrencyExponent,
                S7CurrencyCode,
                S8CurrencyExponent,
                S9CurrencyCode,
                S10CurrencyExponent,
                S11CurrencyCode,
                S12CurrencyExponent,
                S13CurrencyCode,
                S14CurrencyExponent,
                S15CurrencyCode,
                S16CurrencyExponent,
                S17CurrencyCode,
                S18CurrencyExponent,
                S19CurrencyCode,
                S20CurrencyExponent,
                S21CurrencyCode,
                S22CurrencyExponent,
                S23CurrencyCode,
                S24CurrencyExponent,
                S25CurrencyCode,
                S26CurrencyExponent,
                S27CurrencyCode,
                S28CurrencyExponent,
                S29CurrencyCode,
                S30CurrencyExponent);
        }
    }
}
