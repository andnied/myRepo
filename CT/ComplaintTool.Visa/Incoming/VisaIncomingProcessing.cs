using ComplaintTool.Common;
using ComplaintTool.Common.Config;
using ComplaintTool.Common.CTLogger;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.Visa.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common.Utils;

namespace ComplaintTool.Visa.Incoming
{
    public class VisaIncomingProcessing : VisaIncomingBase
    {
        #region Fields

        private static readonly ILogger _logger = LogManager.GetLogger();
        private readonly int _fileId;
        private readonly string _arn;

        #endregion

        #region ModelFields

        private readonly string[] _complaintTCs = { "52", "15", "16", "35", "36", "10", "20", "01", "02", "03" };
        private readonly string[] _feeCollectionTCs = { "10", "20" };
        private readonly string[] _feeCollectionReasonCodes = { "0220", "0350" };
        private readonly string _fileHeaderTc = "90";
        private readonly string _batchEndTc = "91";
        private readonly string _newTransTcr = "00";

        #endregion

        #region IComplaintProcess

        public override string ProcessFilePath
        {
            get { throw new NotImplementedException(); }
        }

        public override string FilePath
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region Constructor

        public VisaIncomingProcessing(int fileId, string arn = null)
        {
            Guard.ThrowIf<ArgumentException>(fileId < 1, "fileId");
            _fileId = fileId;
            _arn = arn;
        }

        #endregion

        #region MainProcess

        public override int Process()
        {
            try
            {
                RegOrgIncomingFile regOrg = null;
                DateTime started;
                DateTime finished;

                using (var unitOfWork = ComplaintUnitOfWork.Create())
                {
                    regOrg = unitOfWork.Repo<RegOrgIncomingFilesRepo>().FindRegOrgIncomingFile(_fileId);

                    if (regOrg == null)
                        throw new Exception(string.Format("Incoming file with id {0} not found.", _fileId));

                    started = DateTime.UtcNow;
                    finished = new DateTime();

                    regOrg.ErrorFlag = true;
                    regOrg.ParsingStarted = started;
                    unitOfWork.Repo<RegOrgIncomingFilesRepo>().Update(regOrg);
                    unitOfWork.Commit();
                }

                using (var unitOfWork = ComplaintUnitOfWork.Create())
                {
                    List<IncomingRecordVisa> incomingRecordVisaList = null;
                    var lines = unitOfWork.Repo<FileRepo>().GetIncomingRegOrgFile(regOrg.FileId);
                    if (lines == null || !(lines.Any()))
                        return 0;

                    try
                    {
                        incomingRecordVisaList = _arn != null ? this.ProcessFile(unitOfWork, lines).Where(m => m.ARN == _arn).ToList() : this.ProcessFile(unitOfWork, lines);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogComplaintEvent(509, "Visa", regOrg.Name);
                        throw new Exception(string.Format(ComplaintConfig.Instance.Notifications[511].MessageText, "Visa", regOrg.Name, ex.ToString()));
                    }

                    if (incomingRecordVisaList.Count > 0)
                    {

                        foreach (var incRec in incomingRecordVisaList)
                        {
                            incRec.FileId = regOrg.FileId;
                            if (regOrg.CreationDateTime.HasValue)
                                incRec.IncomingDate = regOrg.CreationDateTime.Value.ToString("yyyyMMdd");
                            if (!string.IsNullOrWhiteSpace(incRec.ARN))
                            {
                                var binFromArn = ComplaintTool.Common.Utils.Convert.ExtractBinFromARN(incRec.ARN);
                                incRec.BIN = binFromArn;
                                var sourceCountryCode = unitOfWork.Repo<BINListRepo>().GetSourceCountryCodeByBin(binFromArn);
                                incRec.SourceCountryCode = !string.IsNullOrWhiteSpace(sourceCountryCode) ? sourceCountryCode : new string(' ', 3);
                            }
                        }

                        var processingMode = regOrg.ProcessingMode ?? 0;

                        //created "mid list" to be able to pass the list as a parameter (no circual reference issue)
                        var incomingRecordVisaMidList = incomingRecordVisaList.Select(r => this.CopyIncomingRecordVisaToMid(r));

                        if (unitOfWork.Repo<RegOrgIncomingFilesRepo>().InsertOrgIncomingTranVISA(incomingRecordVisaMidList, processingMode))
                        {
                            finished = DateTime.UtcNow;

                            regOrg.ParsingStarted = started;
                            regOrg.ParsingFinished = finished;
                            regOrg.ProcesingStart = finished;
                            regOrg.ProcesingFinished = finished;
                            regOrg.Status = 3;
                            regOrg.ErrorFlag = false;

                            _logger.LogComplaintEvent(103, new object[] { "Visa", regOrg.Name, incomingRecordVisaList.Count });
                        }
                        else
                            throw new Exception(string.Format(ComplaintConfig.Instance.Notifications[507].MessageText, "Visa", regOrg.Name));
                    }
                    else
                    {
                        finished = DateTime.UtcNow;

                        regOrg.ParsingStarted = started;
                        regOrg.ParsingFinished = finished;
                        regOrg.ProcesingStart = finished;
                        regOrg.ProcesingFinished = finished;
                        regOrg.Status = 3;
                        regOrg.ErrorFlag = false;

                        _logger.LogComplaintEvent(506, new object[] { "Visa", regOrg.Name });
                    }

                    unitOfWork.Repo<RegOrgIncomingFilesRepo>().Update(regOrg);
                    unitOfWork.Commit();

                    return 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogComplaintException(ex);
                return -1;
            }
        }

        #endregion

        #region PrivateMethods

        public List<IncomingRecordVisa> ProcessFile(ComplaintUnitOfWork unitOfWork, IEnumerable<string> lines)
        {
            var tempIncomingRecordVisaList = new List<IncomingRecordVisa>();
            var incomingRecordVisaList = new List<IncomingRecordVisa>();
            IncomingRecordVisa record = null;
            var incomingDate = string.Empty;
            var count = 1;

            foreach (var line in lines)
            {
                if (line.Replace('\r', ' ').Trim().Length == 0) continue;
                var tc = line.Substring(0, 2);

                if (tc.Equals(_fileHeaderTc))
                {
                    var tc90 = new TC90(line);
                    incomingDate = ComplaintTool.Common.Utils.Convert.JulianDateToDateTimeForVisa(tc90.ProcessingDate).ToString("yyyy-MM-dd HH:mm:ss:fff");
                }
                else if (_complaintTCs.Contains(tc))
                {
                    var tcr = line.Substring(2, 2);

                    if (tcr.Equals(_newTransTcr))
                    {
                        if (record != null)
                            tempIncomingRecordVisaList.Add(record);
                        record = new IncomingRecordVisa(unitOfWork);
                    }

                    if (record != null)
                        record.AddInformation(line, tc, tcr);
                }
                else
                {
                    if (record != null)
                    {
                        tempIncomingRecordVisaList.Add(record);
                        record = null;
                    }
                }

                if (tc.Equals(_batchEndTc))
                {
                    var tcObject = new TC91(line);
                    var settlementDate = ComplaintTool.Common.Utils.Convert.JulianDateToDateTimeForVisa(tcObject.ProcessingDate).ToString("yyyy-MM-dd HH:mm:ss:fff");

                    foreach (var incomingTran in tempIncomingRecordVisaList)
                    {
                        if (_feeCollectionTCs.Contains(incomingTran.TransactionCode)
                            && !_feeCollectionReasonCodes.Contains(incomingTran.ReasonCode)) continue;
                        incomingTran.SettlementDate = settlementDate;
                        incomingTran.IncomingDate = incomingDate;
                        count++;
                    }
                    incomingRecordVisaList.AddRange(tempIncomingRecordVisaList);
                    tempIncomingRecordVisaList = new List<IncomingRecordVisa>();
                }
            }

            return incomingRecordVisaList;
        }

        private IncomingRecordVisaMid CopyIncomingRecordVisaToMid(IncomingRecordVisa record)
        {
            if (typeof(IncomingRecordVisaMid).GetProperties().Count() != record.GetType().GetProperties().Count())
                throw new Exception("Cannot create a 'mid list' of Incoming Record Visa List. Properties count does not match.");

            var mid = new IncomingRecordVisaMid();
            foreach (var prop in mid.GetType().GetProperties())
            {
                prop.SetValue(mid, record.GetType().GetProperty(prop.Name).GetValue(record), null);
            }

            return mid;
        }

        #endregion
    }
}
