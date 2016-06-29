using ComplaintTool.Common;
using ComplaintTool.Common.Config;
using ComplaintTool.Common.CTLogger;
using ComplaintTool.Common.Enum;
using ComplaintTool.Common.Extensions;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.EVOGermany.Incoming
{
    class EVOGermanyClf : IEVOGermanyIncoming
    {
        private static readonly ILogger _logger = LogManager.GetLogger();
        private readonly string[] _clfExt = ComplaintConfig.Instance.Parameters["CLFExt"].ParameterValue.Split('|');

        public bool Validate(string filePath)
        {
            if (!(_clfExt.Contains(Path.GetExtension(filePath))))
            {
                _logger.LogComplaintEvent(523, new object[] { "EVO CLF", Path.GetFileName(filePath) });
                return false;
            }

            return true;
        }

        public int ProcessFile(string filePath)
        {
            try
            {
                var fileName = Path.GetFileName(filePath);

                using (var unitOfWork = ComplaintUnitOfWork.Create())
                {
                    if (unitOfWork.Repo<CLFRepo>().CLFReportExists(filePath, false, 3))
                    {
                        _logger.LogComplaintEvent(500, "EVO CLF", fileName);
                        return -1;
                    }

                    var guid = unitOfWork.Repo<FileRepo>().AddIncomingFile(filePath);

                    _logger.LogComplaintEvent(100, "EVO CLF", fileName);

                    var clfReport = CreateCLF(filePath, guid, unitOfWork);

                    if (clfReport == null)
                        return -1;

                    var clfReportItemList = this.ReadStream(File.ReadAllBytes(filePath), clfReport, unitOfWork);

                    if (clfReportItemList.All(c => c.Date != null))
                    {
                        try
                        {
                            foreach (var item in clfReportItemList)
                            {
                                this.AddClfReportItem(item, unitOfWork);
                            }

                            clfReport.ProcesingFinished = DateTime.UtcNow;
                            clfReport.Status = 3;
                            clfReport.ErrorFlag = false;

                            unitOfWork.Commit();

                            _logger.LogComplaintEvent(110, "EVO CLF", clfReport.FileName);

                            return 0;
                        }
                        catch
                        {
                            throw new Exception(string.Format(ComplaintConfig.Instance.Notifications[507].MessageText, "EVO CLF", clfReport.FileName));
                        }
                    }
                    else
                        throw new Exception(string.Format(ComplaintConfig.Instance.Notifications[515].MessageText, "EVO CLF", clfReport.FileName));
                }
            }
            catch (Exception ex)
            {
                _logger.LogComplaintEvent(503, new object[] { "EVO CLF", Path.GetFileName(filePath), Path.GetDirectoryName(filePath) });
                _logger.LogComplaintException(ex);

                return -1;
            }
        }

        public void NotifyMainException(Exception ex)
        {
            _logger.LogComplaintEvent(534, "EVO CLF", ex.ToString());
        }

        #region PrivateMethods

        private CLFReport CreateCLF(string filePath, Guid guid, ComplaintUnitOfWork unitOfWork)
        {
            var clfReport = unitOfWork.Repo<CLFRepo>().Add(new CLFReport()
            {
                FileName = filePath,
                Incomong = true,
                Outgoing = false,
                Status = 0,
                ErrorFlag = true,
                stream_id = guid,
                ProcesingStart = DateTime.UtcNow
            });

            return clfReport;
        }

        private List<CLFReportItem> ReadStream(byte[] stream, CLFReport clfReport, ComplaintUnitOfWork unitOfWork)
        {
            var clfReportItemList = new List<CLFReportItem>();

            using (var memoryStream = new MemoryStream())
            {
                memoryStream.Write(stream, 0, stream.Length);
                memoryStream.Position = 0;

                using (var streamReader = new StreamReader(memoryStream))
                {
                    try
                    {
                        string line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            var clfReportItem = this.GetCLFReportItem(line);

                            if (unitOfWork.Repo<ComplaintRepo>().ComplaintExists(clfReportItem.CaseId))
                            {
                                clfReportItem.CLFReport = clfReport;
                                clfReportItem.SourceIncoming = FileSource.EVOG.ToString();
                                clfReportItemList.Add(clfReportItem);
                            }
                            else
                            {
                                throw new Exception(string.Format(ComplaintConfig.Instance.Notifications[542].MessageText, clfReportItem.CaseId));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogComplaintEvent(515, "EVO CLF", clfReport.FileName);
                        throw new Exception(string.Format(ComplaintConfig.Instance.Notifications[511].MessageText, "EVO CLF", clfReport.FileName, ex.ToString()));
                    }
                }
            }

            return clfReportItemList;
        }

        private CLFReportItem GetCLFReportItem(string recordLine)
        {
            var clfReportItem = new CLFReportItem();
            var record = recordLine.Split('|');

            if (!string.IsNullOrWhiteSpace(record[0]))
                clfReportItem.Date = DateTime.Parse(record[0], new CultureInfo("de-DE"));
            clfReportItem.ActivityType = record[1];
            clfReportItem.InfoAcquirer = record[2];
            clfReportItem.ResponseCode = record[3];
            clfReportItem.ResponseInformation = record[4];
            clfReportItem.CommentsProcessor = record[5];
            if (!string.IsNullOrWhiteSpace(record[6]))
                clfReportItem.GoodDeliveryDate = DateTime.Parse(record[6], new CultureInfo("de-DE"));
            clfReportItem.PlaceHolder = record[7];
            clfReportItem.Brand = record[8];
            clfReportItem.Stage = record[9];
            clfReportItem.RC = record[10];
            clfReportItem.CaseId = record[11];
            clfReportItem.ChargebackAmount = record[12];
            clfReportItem.MID = record[13];
            clfReportItem.PAN = record[14];
            clfReportItem.ARN = record[15];
            clfReportItem.PlaceHolder2 = record[16];
            if (!string.IsNullOrWhiteSpace(record[17]))
                clfReportItem.RefundDate = DateTime.Parse(record[17], new CultureInfo("de-DE"));
            clfReportItem.RefundAmount = record[18];
            clfReportItem.RefundCurrency = record[19];
            clfReportItem.ItemId = record[20];
            clfReportItem.CLFReportStatus = 0;
            clfReportItem.CLFExpirationDate = DateTime.UtcNow.AddBusinessDays(5);

            return clfReportItem;
        }

        private void AddClfReportItem(CLFReportItem clfReportItem, ComplaintUnitOfWork unitOfWork)
        {
            var complaint = unitOfWork.Repo<ComplaintRepo>().FindByCaseId(clfReportItem.CaseId);

            if (complaint != null)
            {
                var updateDateTime = DateTime.Now;

                if (complaint.Close.HasValue)
                {
                    complaint.Close = false;
                    complaint.CloseDate = updateDateTime;
                    var noteMsg = "Case reopened by CLF file";

                    unitOfWork.Repo<ComplaintRepo>().AddComplaintNote(complaint.CaseId, noteMsg);
                }
                else
                {
                    complaint.Close = false;
                    complaint.CloseDate = updateDateTime;
                }

                var complaintStage = unitOfWork.Repo<ComplaintRepo>().FindLastStageByCaseId(complaint.CaseId);

                if (complaintStage != null)
                {
                    complaintStage.Status = (int)ComplaintViewStatus.ClfReceived;
                    complaintStage.StatusDate = updateDateTime;
                }
            }

            unitOfWork.Repo<CLFRepo>().Add(clfReportItem);
        }

        #endregion
    }
}
