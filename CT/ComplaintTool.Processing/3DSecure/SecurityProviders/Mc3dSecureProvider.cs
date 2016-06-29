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
    public class Mc3dSecureProvider:I3dSecureProvider
    {
        private ComplaintUnitOfWork _unitOfWork;
        private const string UcafDataObjectName = "3DSecureUcafData";

        public Mc3dSecureProvider(ComplaintUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool? Get3dSecure(Complaint complaint,string correctValueObject)
        {
            var complaintRecord=_unitOfWork.Repo<ComplaintRepo>().FindLastComplaintRecord(complaint.CaseId);
            if (complaintRecord == null)
                return null;

            return Get3DSecure(complaintRecord.PrevUcafData, complaintRecord.PrevMessageType, correctValueObject);
        }

        public static bool Get3DSecure(string prevUcafData, string prevMessageType, string correctValueObject)
        {
            if (string.IsNullOrEmpty(prevUcafData) || string.IsNullOrEmpty(prevMessageType))
                return false;

            if (!prevMessageType.Equals("0100"))
                return false;

            var prevUcafFirstSubfield = prevUcafData.Substring(0, 1);
            var ucafDataMapValue = ComplaintDictionaires.GetMappingValue(UcafDataObjectName, prevUcafFirstSubfield);

            if (!correctValueObject.Equals(ucafDataMapValue))
                return false;

            return true;
        }
    }
}
