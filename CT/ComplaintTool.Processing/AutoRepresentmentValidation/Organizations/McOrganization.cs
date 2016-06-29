using ComplaintTool.Common.Enum;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.Processing.AutoRepresentmentValidation.Representments;
using Model = ComplaintTool.Processing.AutoRepresentmentValidation.Representments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Processing.AutoRepresentmentValidation.Organizations
{
    public class McOrganization:IOrganization
    {
        private ComplaintUnitOfWork _unitOfWork;

        public McOrganization(ComplaintUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Model.Representment CheckRules(Complaint complaint)
        {
            Model.Representment representment = new PastTimeFrameRepresentment(_unitOfWork);
            if (representment.Calculate(complaint))return representment;

            representment = new McDuplicateChargebackRepresentment(_unitOfWork);
            if (representment.Calculate(complaint))
                return representment;

            representment = new SecureCheckRepresentment(_unitOfWork);
            representment.Calculate(complaint);
            return representment;

        }

        
    }
}
