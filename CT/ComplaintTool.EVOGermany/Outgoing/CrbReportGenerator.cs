using ComplaintTool.Common;
using ComplaintTool.Common.CTLogger;
using ComplaintTool.Common.Enum;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.EVOGermany.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.EVOGermany.Outgoing
{
    public class CrbReportGenerator
    {
        private string _baseFolder;
        private ILogger Logger = LogManager.GetLogger();
        private DateTime _uniwersalEndTime;
        private DateTime _uniwersalStartTime;
        private ComplaintUnitOfWork _unitOfWork;

        public CrbReportGenerator(string baseFolder)
        {
            _baseFolder = baseFolder;
        }

        public void Generate()
        {
            _uniwersalStartTime = DateTime.Now.ToUniversalTime();
            
            using (_unitOfWork = ComplaintUnitOfWork.Create())
            {
                var reportItems = _unitOfWork.Repo<CRBRepo>().GetReportItemsByStatus((int?)CrbReportStatus.New);
                if (reportItems.Count() == 0)
                    return;
                var chbReport = new ChbReport(_unitOfWork);

                var records = chbReport.GetChbRecordList(reportItems);
                var header = GetHeader(records.Count, _uniwersalStartTime);
                var filePath=SaveReport(header, records);
                var streamId = _unitOfWork.Repo<FileRepo>().AddOutgoingFile(filePath);
                var crbReport=AddCrbReportToDb(filePath, streamId, header);
                UpdateCrbReportItem(crbReport, reportItems, CrbReportStatus.Send);
                _unitOfWork.Commit();
                Logger.LogComplaintEvent(113, filePath); 
            }
        }

        private CHBHeader GetHeader(int recordCount,DateTime startTime)
        {

            var chbHeader = new CHBHeader();
            chbHeader.NumberOfRecords = recordCount;
            chbHeader.NumberInCurrentYear = _unitOfWork.Repo<CRBRepo>().GetNextNumberInYear(startTime.Year);
            chbHeader.ImplementValue();

            return chbHeader;
        }

        private string SaveReport(CHBHeader chbHeader,IEnumerable<ChbRecord> records)
        {
            var fileName = Path.Combine(_baseFolder, @"ESERV_CBK-" + "YYMMDD-HHMMSS.tmp");  
            if (File.Exists(fileName)) File.Delete(fileName);

            using (TextWriter textWriter = new StreamWriter(fileName, true, Encoding.GetEncoding(28605)))
            {
                textWriter.WriteLine(chbHeader.GetHeaderLine());
                foreach (var record in records)
                {
                    textWriter.WriteLine(record.GetRecordLine());
                }
            }
            _uniwersalEndTime = DateTime.Now.ToUniversalTime();

            var crbReportFileName = String.Format(@"{0}\ESERV_CBK-{1}-{2}", _baseFolder, _uniwersalEndTime.ToString("yyMMdd"), _uniwersalEndTime.ToString("HHmmss"));

            if (File.Exists(crbReportFileName)) File.Delete(crbReportFileName);
            File.Copy(fileName, crbReportFileName);
            File.Delete(fileName);

            //Logger.Info("File named {0} successfully saved", crbReportFileName);

            return crbReportFileName;
        }

        private CRBReport AddCrbReportToDb(string filePath, Guid streamId,CHBHeader header)
        {
            var crbReport = new CRBReport
            {
                FileName = filePath,
                stream_id = streamId,
                CurrentYear = _uniwersalStartTime.Year,
                NumberInCurrentYear = header.NumberInCurrentYear,
                NumberOfDetailRecords = header.NumberOfRecords,
                ProcesingStart = _uniwersalStartTime,
                ProcesingFinished = _uniwersalEndTime
            };

            _unitOfWork.Repo<CRBRepo>().AddCRBReport(crbReport);
            return crbReport;
        }

        private void UpdateCrbReportItem(CRBReport report, IEnumerable<CRBReportItem> crbReportItems, CrbReportStatus reportStatus)
        {
            foreach (var reportItem in crbReportItems)
            {
                reportItem.CBReportStatus = (int)reportStatus;
                reportItem.CRBReport = report;
                _unitOfWork.Repo<CRBRepo>().Update(reportItem);
            }
        }
    }
}
