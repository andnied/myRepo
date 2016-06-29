using System;
using System.Globalization;
using System.Text.RegularExpressions;
using ComplaintTool.Common.Config;
using ComplaintTool.Models;

namespace ComplaintTool.DataAccess.Utils
{
    public class Helper
    {
        public string AmountToString(decimal amount, int length)
        {
            var numberFormatInfo = CultureInfo.CurrentCulture.NumberFormat;
            return amount.ToString(CultureInfo.InvariantCulture).Replace(numberFormatInfo.CurrencyDecimalSeparator, "").PadLeft(length, '0');
        }

        public string AmountToString(CurrencyCode currencyCode, decimal amount, int length,
            out int amountExponent)
        {
            if (currencyCode == null)
                throw new Exception("Currency not found in DB");

            amountExponent = currencyCode.DecimalPrecision;
            var res = string.Format(GetFormat(amountExponent, length), amount);

            if (res.Length > length + 1)
            {
                throw new Exception("AmountToString => Value cannot violate the pattern restriction (length)");
            }

            if (amountExponent > 0) res = res.Remove(length - amountExponent, 1);

            return res;
        }

        public static decimal ConvertStringAmountToDecimal(string amount, string exponent)
        {
            if (string.IsNullOrWhiteSpace(amount) || string.IsNullOrWhiteSpace(exponent)) return new decimal(0);

            var numberFormatInfo = CultureInfo.CurrentCulture.NumberFormat;
            var i = int.Parse(exponent);
            var overall = amount.Substring(0, amount.Length - i);
            var fraction = amount.Substring(amount.Length - i, i);
            var amountDecimal = double.Parse(overall + numberFormatInfo.CurrencyDecimalSeparator + fraction);
            return new decimal(amountDecimal);
        }

        private static string GetFormat(int exponent, int length)
        {
            if (exponent == 0)
            {
                return "{0:" + new string('0', length) + "}";
            }
            if (exponent <= 5)
            {
                return "{0:" + new string('0', length - exponent) + "." + new string('0', exponent) + "}";
            }
            throw new InvalidOperationException("Exponent invalid");
        }

        public string RegexReplace(string text)
        {
            return Regex.Replace(text, "[^a-zA-Z0-9 -<>?/=_]", "");
        }

        public static DateTime JulianDateToDateTime(string julianDate, DateTime transactionDate)
        {
            var yy =
                int.Parse(transactionDate.Year.ToString(CultureInfo.InvariantCulture).Substring(0, 3) +
                          julianDate.Substring(0, 1));
            var ddd = int.Parse(julianDate.Substring(1, 3));
            return new DateTime(yy, 1, 1).AddDays(ddd - 1);
        }
    }
}
