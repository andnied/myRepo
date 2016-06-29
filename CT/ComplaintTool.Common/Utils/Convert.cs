using System;
using ComplaintTool.Common.Enum;

namespace ComplaintTool.Common.Utils
{
    public static class Convert
    {
        public static DateTime JulianDateToDateTimeForVisa(string julianDate)
        {
            var yy = 2000 + int.Parse(julianDate.Substring(0, 2));
            var ddd = int.Parse(julianDate.Substring(2));
            return new DateTime(yy, 1, 1).AddDays(ddd);
        }

        public static string ExtractBinFromARN(string arn)
        {
            return arn.Substring(1, 6);
        }

        public static string ReplacePolishChars(string t)
        {
            return t.Replace("ą", "a")
                .Replace("Ą", "A")
                .Replace("ę", "e")
                .Replace("Ę", "E")
                .Replace("ś", "s")
                .Replace("Ś", "S")
                .Replace("ż", "z")
                .Replace("Ż", "Z")
                .Replace("ć", "c")
                .Replace("Ć", "C")
                .Replace("ź", "z")
                .Replace("Ź", "Z")
                .Replace("ń", "n")
                .Replace("Ń", "N")
                .Replace("ó", "o")
                .Replace("Ó", "O")
                .Replace("ł", "l")
                .Replace("Ł", "L");
        }

        public static DateTime? CountExpirationDate(DateTime? incomingDate, string organizationId, string stage)
        {
            if (!incomingDate.HasValue) return null;

            if (organizationId == Organization.VISA.ToString())
            {
                if (stage == ChbRecordStage._1CB.ToString().TrimStart('_'))
                    return incomingDate.Value.AddDays(45);
                if (stage == ChbRecordStage._RR.ToString().TrimStart('_'))
                    return incomingDate.Value.AddDays(30);
                if (stage == ChbRecordStage._2PR.ToString().TrimStart('_'))
                    return incomingDate.Value.AddDays(5);
                if (stage == ChbRecordStage._2CB.ToString().TrimStart('_'))
                    return null;
                if (stage == ChbRecordStage._COLL.ToString().TrimStart('_'))
                    return incomingDate.Value.AddDays(30);
                if (stage == ChbRecordStage._PAIN.ToString().TrimStart('_')
                    || stage == ChbRecordStage._PCIN.ToString().TrimStart('_'))
                    return incomingDate.Value.AddDays(30);
                if (stage == ChbRecordStage._ARB.ToString().TrimStart('_')
                    || stage == ChbRecordStage._COM.ToString().TrimStart('_'))
                    return incomingDate.Value.AddDays(14);
                if (stage == ChbRecordStage._PAOU.ToString().TrimStart('_')
                    || stage == ChbRecordStage._PCOU.ToString().TrimStart('_'))
                    return null;
            }

            if (organizationId == Organization.MC.ToString())
            {
                if (stage == ChbRecordStage._1CB.ToString().TrimStart('_'))
                    return incomingDate.Value.AddDays(45);
                if (stage == ChbRecordStage._RR.ToString().TrimStart('_'))
                    return incomingDate.Value.AddDays(30);
                if (stage == ChbRecordStage._2PR.ToString().TrimStart('_'))
                    return incomingDate.Value.AddDays(8);
                if (stage == ChbRecordStage._2CB.ToString().TrimStart('_'))
                    return incomingDate.Value.AddDays(45);
                if (stage == ChbRecordStage._COLL.ToString().TrimStart('_'))
                    return incomingDate.Value.AddDays(30);
                if (stage == ChbRecordStage._PAIN.ToString().TrimStart('_')
                    || stage == ChbRecordStage._PCIN.ToString().TrimStart('_'))
                    return incomingDate.Value.AddDays(30);
                if (stage == ChbRecordStage._ARB.ToString().TrimStart('_')
                    || stage == ChbRecordStage._COM.ToString().TrimStart('_'))
                    return incomingDate.Value.AddDays(14);
                if (stage == ChbRecordStage._PAOU.ToString().TrimStart('_')
                    || stage == ChbRecordStage._PCOU.ToString().TrimStart('_'))
                    return null;
            }
            return null;
        }
    }
}
