using ComplaintTool.Common;
using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Repos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common.Enum;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.EVOGermany.Incoming
{
    class EVOGermanyPdf : IEVOGermanyIncoming
    {
        private static readonly ILogger _logger = LogManager.GetLogger();
        private readonly string[] _pdfExt = new string[] { ".pdf", ".PDF" };

        public bool Validate(string filePath)
        {
            if (!(_pdfExt.Contains(Path.GetExtension(filePath))))
            {
                _logger.LogComplaintEvent(523, new object[] { "EVO PDF", Path.GetFileName(filePath) });
                return false;
            }

            return true;
        }

        public int ProcessFile(string filePath)
        {
            try
            {
                var fileName = Path.GetFileName(filePath);
                var name = Path.GetFileNameWithoutExtension(filePath);

                using (var unitOfWork = ComplaintUnitOfWork.Create())
                {
                    var guid = unitOfWork.Repo<FileRepo>().AddDocumentFile(filePath);

                    _logger.LogComplaintEvent(100, "EVO PDF", fileName);

                    var fileNameElements = name.Split('_');
                    var caseId = fileNameElements[6].ToUpper().Trim();

                    if (unitOfWork.Repo<ComplaintRepo>().ComplaintDocumentExists(caseId))
                    {
                        var regFileName = unitOfWork.Repo<FileRepo>().AddDocumentToComplaint(fileName, caseId, guid, FileSource.EVOG);
                        unitOfWork.Commit();

                        _logger.LogComplaintEvent(101, "EVO PDF", regFileName);

                        return 0;
                    }
                    else
                    {
                        _logger.LogComplaintEvent(527, "EVO PDF", fileName);

                        return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogComplaintEvent(503, new object[] { "EVO PDF", Path.GetFileName(filePath), Path.GetDirectoryName(filePath) });
                _logger.LogComplaintException(ex);

                return -1;
            }            
        }

        public void NotifyMainException(Exception ex)
        {
            _logger.LogComplaintEvent(519, "EVOGermany", ex.ToString());
        }
    }
}
