using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Processing.TemporaryValidation.Organizations
{
    public class VisaOrganization : IOrganization
    {
        private ComplaintUnitOfWork _unitOfWork;
        public VisaOrganization(ComplaintUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<string> GetFilesNames(int processingStatus,int destinationStatus)
        {
            return _unitOfWork.Repo<IncomingTranRepo>().FindFileNameVisaAndSetTempStatus(processingStatus, destinationStatus);
        }
    }
}
