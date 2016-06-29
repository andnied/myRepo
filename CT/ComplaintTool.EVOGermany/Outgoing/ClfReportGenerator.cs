using ComplaintTool.Common;
using ComplaintTool.Common.Config;
using ComplaintTool.Common.CTLogger;
using ComplaintTool.Common.Enum;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.EVOGermany.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.EVOGermany.Outgoing
{
    public class ClfReportGenerator
    {
        private string _baseFolder;
        private ComplaintUnitOfWork _unitOfWork = null;
        private ILogger Logger = LogManager.GetLogger();

        public DateTime StartProccessTime { get; private set; }
        public DateTime EndProccessTime { get; private set; }
        public CLFReport Report { get; set; }
        IRecordParser _recordParser;
        
        public ClfReportGenerator(CLFReport report,string baseFolder)
        {
            Report = report;
            _baseFolder=baseFolder;
            _recordParser = new CsvRecordParser();
            _recordParser.ExportedColumns = GetPropertyToExport();
        
        }

        public void Generate()
        {
            using (var _unitOfWork = ComplaintUnitOfWork.Create())
            {
                var clfReportItems = _unitOfWork.Repo<CLFRepo>().FindItemsForReport(Report.CLFReportId);
                var reportItemsToResponse = _unitOfWork.Repo<CLFRepo>().FindItemsToResponse(clfReportItems).ToList();
                if (clfReportItems.Count() != reportItemsToResponse.Count())
                    return;
                
                string filePath = SaveFile(reportItemsToResponse);

                var streamId = _unitOfWork.Repo<FileRepo>().AddOutgoingFile(filePath);
                var reportOutgoing = AddClfReportOutgoing(filePath, streamId);
                UpdateReportAndReportItems(reportItemsToResponse, reportOutgoing);
                _unitOfWork.Commit();
                Logger.LogComplaintEvent(115, Path.GetFileName(filePath));
            }
        }

        private CLFReport AddClfReportOutgoing(string filePath, Guid streamId)
        {
            var clfReportOutgoing = new CLFReport
            {
                FileName = filePath,
                stream_id = streamId,
                Incomong = false,
                Outgoing = true,
                Status = (int)ClfFileStatus.Complete,
                ProcesingStart = StartProccessTime,
                ProcesingFinished = EndProccessTime,
                ErrorFlag = false
            };

            _unitOfWork.Repo<CLFRepo>().Add(clfReportOutgoing);
            return clfReportOutgoing;
        }

        private void UpdateReportAndReportItems(List<CLFReportItem> reportItemsToResponse,CLFReport clfReport)
        {
            reportItemsToResponse.ForEach(x => {
                x.CLFReport = clfReport;
            });
            var reportToUpdate = _unitOfWork.Repo<CLFRepo>().FindReport(Report.CLFReportId);
            reportToUpdate.Status = (int)ClfFileStatus.Complete;
            reportToUpdate.ProcesingFinished = EndProccessTime;
        }

        private string SaveFile(IEnumerable<CLFReportItem> clfReportItemsToResponse)
        {
            StartProccessTime = DateTime.Now;
            string filePath = GetFilePath(_baseFolder);
            string tempFilePath = filePath+".tmp";

            if (File.Exists(tempFilePath))
                throw new Exception(String.Format(ComplaintConfig.Instance.Notifications[533].MessageText, Path.GetFileName(filePath), _baseFolder));

            using (var textWriter = new StreamWriter(tempFilePath))
            {
                foreach (var reportItemToResponse in clfReportItemsToResponse)
                {
                    string line = _recordParser.GetLine<CLFReportItem>(reportItemToResponse);
                    textWriter.WriteLine(line);
                }
            }

            if (File.Exists(filePath))
                File.Delete(filePath);

            File.Copy(tempFilePath, filePath);

            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);

            EndProccessTime = DateTime.Now;
            return filePath;
        }

        private string GetFilePath(string folder)
        {
            string name = Path.GetFileNameWithoutExtension(Report.FileName);
            string extension = Path.GetExtension(Report.FileName);
            string fileName = string.Format("{0}_REPONSE{1}", name, extension);
            return Path.Combine(folder, fileName);
        }

        private static LinkedList<string> GetPropertiesNames<T>(params Expression<Func<T, object>>[] lambdaArray)
        {
            LinkedList<string> propNames = new LinkedList<string>();
            foreach (var lambda in lambdaArray)
            {
                var member = lambda.Body as MemberExpression;
                PropertyInfo propInfo;
                if (member != null)
                {
                    propInfo = (PropertyInfo)member.Member;
                }
                else
                {
                    var op = ((UnaryExpression)lambda.Body).Operand;
                    propInfo = (PropertyInfo)((MemberExpression)op).Member;
                }
                propNames.AddLast(propInfo.Name);
            }

            return propNames;
        }

        private LinkedList<string> GetPropertyToExport()
        {
            return new LinkedList<string>(
                GetPropertiesNames<CLFReportItem>(x => x.Date,
                x => x.ActivityType,
                x => x.InfoAcquirer,
                x => x.ResponseCode,
                x => x.ResponseInformation,
                x => x.CommentsProcessor,
                x => x.GoodDeliveryDate,
                x => x.PlaceHolder,
                x => x.Brand,
                x => x.Stage,
                x => x.RC,
                x => x.CaseId,
                x => x.ChargebackAmount,
                x => x.MID,
                x => x.PAN,
                x => x.ARN,
                x => x.PlaceHolder2,
                x => x.RefundDate,
                x => x.RefundAmount,
                x => x.RefundCurrency,
                x => x.ItemId
                )
            );
        }
    }
}
