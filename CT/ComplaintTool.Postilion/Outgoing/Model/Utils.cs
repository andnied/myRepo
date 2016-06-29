using System;
using System.Globalization;
using System.Text.RegularExpressions;
using ComplaintTool.Common.Enum;

namespace ComplaintTool.Postilion.Outgoing.Model
{
    public static class Utils
    {
        public static string RegexReplace(string text)
        {
            return Regex.Replace(text, "[^a-zA-Z0-9 -<>?/=_]", "");
        }

        public static string GetFeeCollectionControlNr(Organization organization, long feeCollectionId)
        {
            var controlNr = DateTime.Now.ToString("yyyyMMdd"); //50420400001
            var count = feeCollectionId;
            switch (organization)
            {
                case Organization.MC:
                    controlNr += "4";
                    controlNr += count.ToString(CultureInfo.InvariantCulture).PadLeft(9, '0');
                    return controlNr;
                case Organization.VISA:
                    controlNr += "1";
                    controlNr += count.ToString(CultureInfo.InvariantCulture).PadLeft(8, '0');
                    return controlNr;
                default:
                    return controlNr;
            }
        }
    }
}
