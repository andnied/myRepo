using BazaMvp.DataAccess.Repos;
using BazaMvp.Model;
using BazaMvp.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BazaIF.Logic
{
    public class MasterCardParser : ParserBase, IParser
    {
        private static readonly MyLogger Logger = new MyLogger();

        protected override string ValueRegex { get { return @"^.{1}(?<transFunc>.{12}).{1}(?<procCode>.{15}).{1}(?<ird>\w{2}.{1}).{1}(?<count>.{7}\d{1}).{1}(?<reconAmount>.{21}\d{2}.{3}).{5}(?<currency>.{3}).{1}(?<transFee>.{18}\d{2}.{3}).{32}$"; } }

        public MasterCardParser(string filePath)
            : base(filePath)
        { }

        public IEnumerable<string[]> GetMessages()
        {
            return base.GetAllMessages("1IP727010-AA", "MASTERCARD WORLDWIDE");
        }

        public string[] RetrieveHeader(string[] msg)
        {
            return msg.Take(6).ToArray();
        }

        public InputBase InitializeParent(string[] header)
        {
            try
            {
                var regexLine2 = new Regex(@"^.{19}(?<product>\w{3}).{41}(?<cycle>\w{3}).{67}$");
                var regexLine3 = new Regex(@"^.{25}(?<region>.{17}).{20}(?<date>\d{4}-\d{2}-\d{2}).{61}$");
                var regexLine4 = new Regex(@"^.{22}(?<subRegion>\d{6}).{105}$");
                var regexLine5 = new Regex(@"^.{11}(?<fid>.{28}).{94}$");
                var regexLine6 = new Regex(@"^.{12}(?<ica>\d{11}).{110}$");

                var line2 = regexLine2.Match(header[1]);
                var line3 = regexLine3.Match(header[2]);
                var line4 = regexLine4.Match(header[3]);
                var line5 = regexLine5.Match(header[4]);
                var line6 = regexLine6.Match(header[5]);

                if (!(line2.Success && line3.Success && line4.Success && line6.Success))
                    return null;

                return new MasterCardRecord
                {
                    Product = line2.Groups["product"].Value,
                    Cycle = int.Parse(line2.Groups["cycle"].Value),
                    Region = line3.Groups["region"].Value.Trim(),
                    BusinessDate = DateTime.ParseExact(line3.Groups["date"].Value, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    SubRegion = line4.Groups["subRegion"].Value,
                    Fid = line5.Groups["fid"].Value,
                    Ica = int.Parse(line6.Groups["ica"].Value).ToString()
                };
            }
            catch (Exception ex)
            {
                throw new IFException("Unable to retrieve header values.", ex);
            }
        }

        public IEnumerable<InputBase> GenerateRecords(InputBase parentRecord, string[] msg)
        {
            var records = new List<MasterCardRecord>();

            if (parentRecord != null)
            {
                for (int i = 10; i < msg.Length; i++)
                {
                    var record = AssignValues(((MasterCardRecord)parentRecord).Clone(), msg[i]);

                    if (record != null)
                    {
                        if (record.TransFunc.Length == 0 && records.Count > 0)
                            record.TransFunc = records.Last().TransFunc;

                        records.Add(record);
                    }
                }
            }

            return records;
        }

        public void AddRecords(IEnumerable<InputBase> records)
        {
            base.AddRecords(records);
            records.ToList().ForEach(r => _unitOfWork.Repo<McRecordsRepo>().AddRecord(r));
            _unitOfWork.Commit();
        }

        private MasterCardRecord AssignValues(MasterCardRecord record, string line)
        {
            try
            {
                var regexValues = new Regex(ValueRegex);
                var match = regexValues.Match(line);
                if (!(match.Success))
                    return null;

                record.TransFunc = match.Groups["transFunc"].Value.Trim();
                record.ProcessingCode = match.Groups["procCode"].Value.Trim();
                record.CurrencyCode = match.Groups["currency"].Value.Trim();
                record.Ird = match.Groups["ird"].Value.Trim();
                record.Count = int.Parse(match.Groups["count"].Value.Trim());
                record.ReconAmount = base.GetDecimalValue(match.Groups["reconAmount"].Value) * (-1);
                record.TransFee = base.GetDecimalValue(match.Groups["transFee"].Value);

                return record;
            }
            catch (Exception ex)
            {
                throw new IFException("Unable to retrieve assign values.", ex);
            }
        }
    }
}
