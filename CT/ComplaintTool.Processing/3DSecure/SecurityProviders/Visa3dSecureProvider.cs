using ComplaintTool.Common.Config;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Processing._3DSecure.SecurityProviders
{
    public class Visa3dSecureProvider:I3dSecureProvider
    {
        private ComplaintUnitOfWork _unitOfWork;
        private const string EciValueObjectName = "3DSecureECIValue";

        public Visa3dSecureProvider(ComplaintUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool? Get3dSecure(Complaint complaint, string correctValueObject)
        {
            var incoming=_unitOfWork.Repo<IncomingTranRepo>().FindTranVISAByCaseId(complaint.CaseId);

            if (incoming == null)
                return null;

            if (incoming.MOTOECI == null)
                return false;

            var eciMapValue = ComplaintDictionaires.GetMappingValue(EciValueObjectName, incoming.MOTOECI);
            if (!correctValueObject.Equals(eciMapValue))
                return false;

            return true;
        }
    }
}
