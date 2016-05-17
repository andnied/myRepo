using BazaMvp.DataAccess.Repos;
using BazaMvp.Model;
using BazaMvp.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BazaIF.Logic
{
    public class VisaParser : ParserBase, IParser
    {
        private static readonly MyLogger Logger = new MyLogger();
        private readonly IEnumerable<VisaImportRule> _rules;
        private readonly IEnumerable<string> _currencyCodes;

        protected override string ValueRegex { get { return @"^(?<transactions>.{46})(?<count>.{15})(?<interchangeAmount>.{25})(?<reimbursementFeeCredits>.{23})(?<reimbursementFeeDebits>.{22}).{2}$"; } }

        public VisaParser(string filePath)
            : base(filePath)
        {
            _rules = _unitOfWork.Repo<HelperRepo>().GetVisaImportRules();
            _currencyCodes = _unitOfWork.Repo<HelperRepo>().GetCurrencyCodes();
        }

        public IEnumerable<string[]> GetMessages()
        {
            return base.GetAllMessages("REPORT ID:  VSS-130", "END OF VSS-130 REPORT");
        }

        public string[] RetrieveHeader(string[] msg)
        {
            return msg.Take(5).ToArray();
        }

        public BazaMvp.Model.InputBase InitializeParent(string[] header)
        {
            try
            {
                var regexReportingFor = new Regex(@"^.{21}(?<value>.{10}).{1}(?<curr2>.{17}).{75}(?<date>.{7}).{2}$");
                var regexCurr1 = new Regex(@"^.{23}(?<curr1>.{3}).{107}$");

                var reportingForMatch = regexReportingFor.Match(header[1]);
                var reportingFor = reportingForMatch.Groups["value"].Value;
                var dateStr = reportingForMatch.Groups["date"].Value;
                var curr2 = _currencyCodes.FirstOrDefault(c => reportingForMatch.Groups["curr2"].Value.Replace("POL", "PLN").Contains(c));
                var curr1 = regexCurr1.Match(header[4]).Groups["curr1"].Value;

                if (!(_rules.Any(r => r.ReportingFor.Equals(reportingFor))))
                    return null;

                return new VisaRecord
                {
                    ReportingFor = reportingFor,
                    Currency1 = curr1,
                    Currency2 = curr2,
                    BusinessDate = DateTime.ParseExact(dateStr, "ddMMMyy", CultureInfo.InvariantCulture),
                    Fid = reportingFor + "/" + DateTime.ParseExact(dateStr, "ddMMMyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "/" + Path.GetFileNameWithoutExtension(_filePath)
                };
            }
            catch (Exception ex)
            {
                throw new IFException("Unable to retrieve header values.", ex);
            }
        }

        public IEnumerable<BazaMvp.Model.InputBase> GenerateRecords(BazaMvp.Model.InputBase parentRecord, string[] msg)
        {
            try
            {
                var records = new List<VisaRecord>();

                if (parentRecord == null)
                    return records;

                for (int i = 8; i < msg.Length; i++)
                {
                    if (ShouldBeSkipped(i))
                        continue;

                    var record = AssignValues(parentRecord, msg[i]);

                    if (record != null)
                        records.Add(record);
                }

                return records;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return null;
            }
        }

        public new void AddRecords(IEnumerable<BazaMvp.Model.InputBase> records)
        {
            base.AddRecords(records);
            records.ToList().ForEach(r => _unitOfWork.Repo<VisaRecordsRepo>().AddRecord(r));
            _unitOfWork.Commit();
        }

        private VisaRecord AssignValues(InputBase record, string line)
        {
            var regex = new Regex(@"^(?<transactions>.{46})(?<count>.{15})(?<interchangeAmount>.{25})(?<reimbursementFeeCredits>.{23})(?<reimbursementFeeDebits>.{22}).{2}$");
            var match = regex.Match(line);

            if (match.Success)
            {
                var transactions = match.Groups["transactions"].Value.Trim();

                if (transactions.Contains("TOTAL") || transactions.Contains("NET  "))
                    return null;

                if (match.Groups.Cast<Group>().Skip(2).All(g => g.Value.Trim().Length == 0))
                {
                    int pos = match.Groups["transactions"].Value.IndexOf(transactions);
                    var prop = typeof(VisaRecord).GetProperties()
                        .FirstOrDefault(p => p.GetCustomAttributes(typeof(VisaRecordAttribute), false)
                            .Any(a => ((VisaRecordAttribute)a).Position == pos));

                    if (prop != null)
                        prop.SetValue(record, transactions);

                    return null;
                }
                else
                {
                    var recordToBeAdded = ((VisaRecord)record).Clone();

                    if (RecordAccepted(recordToBeAdded))
                    {
                        var count = match.Groups["count"].Value.Trim().Replace(",", "").Replace('.', ',');
                        var reimbursementFeeCredits = match.Groups["reimbursementFeeCredits"].Value.Trim().Replace(",", "").Replace('.', ',');
                        var reimbursementFeeDebits = match.Groups["reimbursementFeeDebits"].Value.Trim().Replace(",", "").Replace('.', ',');

                        try
                        {
                            recordToBeAdded.Count = int.Parse(count);
                            recordToBeAdded.InterChangeAmount = base.GetDecimalValue(match.Groups["interchangeAmount"].Value.Trim());
                            recordToBeAdded.ReimbursementFee = reimbursementFeeCredits.Length == 0 ? (decimal.Parse(reimbursementFeeDebits) * -1) : decimal.Parse(reimbursementFeeCredits);
                            recordToBeAdded.Name = transactions;

                            return recordToBeAdded;
                        }
                        catch (FormatException ex)
                        {
                            throw new IFException("Error while parsing record values. Format exception.", ex);
                        }
                        catch (Exception ex)
                        {
                            throw new IFException("Error while parsing record values.", ex);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
        }

        private bool ShouldBeSkipped(int i)
        {
            if (i < 50)
                return false;
            i++;
            int value = (i % 50) * (i / 50) / (i / 50);

            return value > 0 && value < 9;
        }

        private bool RecordAccepted(VisaRecord record)
        {
            if (record.TransactionType == "PURCHASE" && record.TransactionCode == "ORIGINAL SALE" ||
                record.TransactionType == "PURCHASE" && record.TransactionCode == "ORIGINAL SALE          RVRSL" ||
                record.TransactionType == "MERCHANDISE CREDIT" && record.TransactionCode == "ORIGINAL" ||
                record.TransactionType == "MERCHANDISE CREDIT" && record.TransactionCode == "ORIGINAL          RVRSL")
                return true;

            return false;
        }
    }
}
