using ComplaintTool.DataAccess;
using ComplaintTool.Processing.AutoRepresentmentValidation.Representments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Models;
using Model = ComplaintTool.Processing.AutoRepresentmentValidation.Representments;

namespace ComplaintTool.Processing.AutoRepresentmentValidation.Organizations
{
    public class VisaOrganization:IOrganization
    {
        private ComplaintUnitOfWork _unitOfWork;
        public VisaOrganization(ComplaintUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Model.Representment CheckRules(Complaint complaint)
        {
            Model.Representment representment = new PastTimeFrameRepresentment(_unitOfWork);
            if (representment.Calculate(complaint))
                return representment;

            representment = new VisaTransactionNotRecognizedRepresentment(_unitOfWork);
            if (representment.Calculate(complaint))
                return representment;

            representment = new VisaTransactionNotRecognizedSecureRepresentment(_unitOfWork);
            representment.Calculate(complaint);

            return representment;
        }
    }
}
