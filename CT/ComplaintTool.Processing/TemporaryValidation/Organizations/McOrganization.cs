using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Processing.TemporaryValidation.Organizations
{
    public class McOrganization : IOrganization
    {
        private ComplaintUnitOfWork _unitOfWork;

        public McOrganization(ComplaintUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<string> GetFilesNames(int processingStatus,int destintationStatus)
        {
            return _unitOfWork.Repo<IncomingTranRepo>().FindFileNameMcAndSetTempStatus(processingStatus, destintationStatus);
        }
    }
}
