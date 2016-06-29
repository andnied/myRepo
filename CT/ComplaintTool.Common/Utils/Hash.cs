using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ComplaintTool.Common.Utils
{
    public static class Hash
    {

        public static string MaskCardNumber(string pan)
        {
            if (string.IsNullOrEmpty(pan.Trim())) return string.Empty;
            if (pan.ToUpper().Contains("X")) return pan.ToUpper().Replace("-","").Replace(" ","");
            return pan.Substring(0, 6) + "X".PadLeft(6, 'X') + pan.Substring(12, 4);

        }

        public static string RemoveNoNumericCharacters(string str)
        {
            return Regex.Replace(str, "[^0-9]+", "", RegexOptions.Compiled);
        }

        public static string CalculatePanReference(string pan)
        {
            long bigInteger;
            if (!long.TryParse(pan, out bigInteger)) return string.Empty;
            var data = ConvertHexStringToData(PadLeft(bigInteger.ToString("X"), 16, '0'));
            var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(data);
            return "00" + ConvertDataToHexString(hash);
        }



        // dopełnia tekst z lewej strony do uzyskania żądanej długości
        private static string PadLeft(string s, int length, char filler)
        {
            while (s.Length < length)
            {
                s = filler + s;
            }
            return s;
        }

        // konwertuje tekst "303030" na 0x30,0x30,0x30
        private static byte[] ConvertHexStringToData(string hexString)
        {
            var length = hexString.Length;
            var bytes = new byte[length / 2];
            for (var i = 0; i < length; i += 2)
            {
                bytes[i / 2] = System.Convert.ToByte(hexString.Substring(i, 2), 16);                
            }
            return bytes;
        }

        // konweruje tablicę bajtów 0x30,0x30,0x30 na tekst "303030"
        private static string ConvertDataToHexString(byte[] data)
        {
            var sb = new StringBuilder(data.Length * 2);
            var size = data.Length;
            for (var i = 0; i < size; i++)
            {
                sb.AppendFormat("{0:X2}", data[i]);
            }
            return sb.ToString();
        }


    }
}
