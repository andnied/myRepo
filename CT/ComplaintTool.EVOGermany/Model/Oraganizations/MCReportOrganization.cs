using ComplaintTool.Common.Config;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.EVOGermany.Model.Oraganizations
{
    public class MCReportOrganization:IReportOrganization
    {
        ComplaintUnitOfWork _unitOfWork;

        public MCReportOrganization(ComplaintUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ChbRecord FillRecord(ChbRecord chbRecord,long? incomingId)
        {
            var incoming = _unitOfWork.Repo<IncomingTranRepo>().FindTranMASTERCARDById(incomingId);
            if (incoming == null)
                return chbRecord;

            chbRecord.CvvFlag = incoming.CVVFlag;
            chbRecord.CvcFlag = ComplaintDictionaires.GetMappingValue("CVCFlag", incoming.CVCFlag);
            chbRecord.MemberMessageText = incoming.MemberMessageText;
            chbRecord.KkocbReference = incoming.KKOCbReference;
            chbRecord.TransactionId = string.Empty;
            chbRecord.Rrid = string.Empty;
            chbRecord.ReturnReasonCode1 = string.Empty;
            chbRecord.ReturnReasonCode2 = string.Empty;
            chbRecord.ReturnReasonCode3 = string.Empty;
            chbRecord.ReturnReasonCode4 = string.Empty;
            chbRecord.ReturnReasonCode5 = string.Empty;

            return chbRecord;
        }
    }
}
