using System;
using System.Globalization;
using ComplaintTool.Common.Extensions;

namespace ComplaintTool.MCProImageInterface.Incoming.Case
{
    public static class CaseFilingUtil
    {
        private static readonly DateTimeFormatInfo _format = CultureInfo.CurrentUICulture.DateTimeFormat;

        public static DateTime? ConvertToMComDate(string dateTimeValue)
        {
            return !dateTimeValue.IsEmpty() ? DateTime.ParseExact(dateTimeValue, "yyMMdd", _format) : (DateTime?)null;
        }

        public static decimal? ConvertToMComDecimal(string decimalValue)
        {
            return !decimalValue.IsEmpty() ? decimal.Parse(decimalValue) : (decimal?)null;
        }
    }
}
