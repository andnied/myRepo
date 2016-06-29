using ComplaintTool.Common;
using ComplaintTool.Common.Utils;
using ComplaintTool.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.Models;
using System.Security.Principal;
using System.Data.Entity.Validation;
using ComplaintTool.Postilion.Outgoing;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Postilion
{
    public class PostilionOutgoingService : IComplaintProcess
    {
        #region Fields

        private static readonly ILogger _logger = LogManager.GetLogger();
        private readonly string _folderPath;
        private readonly string _filePath;
        private readonly ComplaintTool.Common.Enum.Organization _org;
        private readonly ComplaintTool.Common.Enum.PostilionServiceEnum _type;
        private readonly string _organization;
        private readonly Guid _bulkProcessKey = Guid.NewGuid();
        private ExtracterAbstract _extracter;

        #endregion
        
        #region IComplaintProcess

        public string OrganizationId
        {
            get { return _organization; }
        }

        public string ProcessName
        {
            get { return Globals.PostilionOutgoingService; }
        }

        public string ProcessFilePath
        {
            get { return _filePath; }
        }

        public string FilePath
        {
            get { return _filePath; }
        }

        #endregion

        #region Constructor

        public PostilionOutgoingService(string folderPath, ComplaintTool.Common.Enum.Organization org, ComplaintTool.Common.Enum.PostilionServiceEnum type)
        {
            Guard.ThrowIf<ArgumentNullException>(folderPath.IsEmpty(), "folderPath");
            _folderPath = folderPath;
            _filePath = Path.Combine(folderPath, _bulkProcessKey.ToString());
            _org = org;
            _type = type;
            _organization = org.ToString();
        }

        #endregion

        #region MainProcess

        public void CreateExtract()
        {
            try
            {
                using (var unitOfWork = ComplaintUnitOfWork.Create())
                {
                    _extracter = ExtracterAbstract.GetExtracter(_type, _org, unitOfWork);
                    Guard.ThrowIf<Exception>(_extracter == null, "Extract type not recognized.");

                    var colForExtract = _extracter.GetExtract();

                    if (colForExtract.Count == 0)
                        return;

                    var createdFile = _extracter.WriteExtractToFile(colForExtract, _folderPath);
                    var postilionFile = _extracter.CreatePostilionFile(createdFile);
                    var fileName = _extracter.UpdateExtracts(colForExtract, postilionFile);

                    unitOfWork.Commit();

                    _extracter.Notify(_logger, fileName);
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
