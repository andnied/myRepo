using BazaMvp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BazaIF.Logic
{
    public class ParserFactory
    {
        private static readonly Regex RegexVisa = new Regex(@"^.{13}VSS-.{116}$");
        private static readonly Regex RegexMC = new Regex(@"^.{55}MASTERCARD.{68}$");

        public static ParserBase GetParser(string filePath)
        {
            ParserBase result = null;
            string line = null;

            using (var stream = new StreamReader(filePath))
            {
                line = stream.ReadLine();
            }

            if (RegexVisa.Match(line).Success)
                result = new VisaParser(filePath);
            else if (RegexMC.Match(line).Success)
                result = new MasterCardParser(filePath);
            else
                throw new IFException("File not recognized.");

            return result;
        }
    }
}
