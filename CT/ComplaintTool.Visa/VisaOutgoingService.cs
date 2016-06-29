using ComplaintTool.Common;
using ComplaintTool.DataAccess;
using ComplaintTool.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using System.IO;
using ComplaintTool.Common.Utils;
using ComplaintTool.Common.Config;
using System.Security.Principal;
using System.Data.Entity.Validation;
using ComplaintTool.Visa.Outgoing;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Visa
{
    public class VisaOutgoingService : IComplaintProcess
    {
        #region Fields

        private readonly IVisaOutgoingService _processor;
        private readonly Guid _bulkProcessKey = Guid.NewGuid();
        private readonly string _baseFolder;
        private string _filePath;
        private static readonly ILogger _logger = LogManager.GetLogger();

        #endregion

        #region IComplaintProcess

        public string OrganizationId
        {
            get
            {
                return Common.Enum.Organization.VISA.ToString();
            }
        }

        public string ProcessName
        {
            get
            {
                return Globals.VisaOutgoingInterfaceProcessName;
            }
        }

        public string ProcessFilePath
        {
            get
            {
                return _filePath;
            }
        }

        public string FilePath
        {
            get
            {
                return _filePath;
            }
        }

        #endregion

        #region Constructors

        public VisaOutgoingService(string folderPath, string processType)
        {
            Guard.ThrowIf<ArgumentNullException>(folderPath.IsEmpty(), "processType");
            Guard.ThrowIf<ArgumentNullException>(folderPath.IsEmpty(), "folderPath");

            if (processType == "export")
                _processor = new DocumentExporter();
            else if (processType == "extract")
                _processor = new DocumentExtracter();
            else
                throw new Exception("Invalid process type.");

            _baseFolder = folderPath;
            _filePath = Path.Combine(folderPath, _bulkProcessKey.ToString());
        }

        #endregion

        #region MainProcess

        public void Process()
        {
            try
            {
                var documentCollection = _processor.GetRecords(this.OrganizationId);

                if (documentCollection.Count == 0)
                    return;

                foreach (var item in documentCollection)
                {
                    using (var unitOfWork = ComplaintUnitOfWork.Create())
                    {
                        try
                        {
                            _processor.ProcessRecord(item, unitOfWork, _baseFolder);
                        }
                        catch (Exception ex)
                        {
                            _processor.Notify(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogComplaintException(ex);
            }  
        }

        #endregion
    }
}
