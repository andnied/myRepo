using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common;
using ComplaintTool.Processing._3DSecure.SecurityProviders;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Processing._3DSecure
{
    public class Secure3DProcessor
    {
        private const string CorrectValueObject = "Y";
        private Common.Enum.Organization _orgranization;
        private ISecurityDataProvider _i3dSecurity;
        private readonly ILogger Logger = LogManager.GetLogger();

        public Secure3DProcessor(Common.Enum.Organization orgranization)
        {
            _orgranization=orgranization;
        }

        public void UpdateSecureAndComplaintRecord(string caseId)
        {
            using(var unitOfWork=ComplaintUnitOfWork.Create())
            {
                var complaint = unitOfWork.Repo<ComplaintRepo>().FindByCaseId(caseId);
                var lastComplaintRecord = unitOfWork.Repo<ComplaintRepo>().FindLastComplaintRecord(caseId);
                ValidateParams(complaint, lastComplaintRecord, caseId);
                _i3dSecurity = new PostilionSecurityDataProvider(unitOfWork.Repo<PostilionRepo>());
                _i3dSecurity.LoadData(complaint);
                complaint.Is3DSecure = _i3dSecurity.Secure3d;
                unitOfWork.Repo<ComplaintRepo>().Update(complaint);
                lastComplaintRecord = _i3dSecurity.Fill(lastComplaintRecord);
                unitOfWork.Repo<ComplaintRepo>().Update(lastComplaintRecord);
                unitOfWork.Commit();
                Logger.LogComplaintEvent(151, _orgranization.ToString());
            }
        }

        public void ValidateAndUpdate3dSecures()
        {
            using(var unitOfWork=ComplaintUnitOfWork.Create())
            {
                var complaints = unitOfWork.Repo<ComplaintRepo>().FindComplaintsWithout3dSecure(_orgranization.ToString());
                var secureProvider = GetSecureProvider(unitOfWork);
                foreach (var complaint in complaints)
                {            
                    var is3DSecure=secureProvider.Get3dSecure(complaint,CorrectValueObject);
                    if (!is3DSecure.HasValue)
                        continue;

                    complaint.Is3DSecure = is3DSecure;
                    unitOfWork.Repo<ComplaintRepo>().Update(complaint);
                }
                unitOfWork.Commit();
                Logger.LogComplaintEvent(151,_orgranization.ToString());
            } 
        }

        private void ValidateParams(Complaint complaint,ComplaintRecord complaintRecord, string caseId)
        {
            if (complaint == null)
                throw new Exception(string.Format("Cannot find case for CaseId = {0}", caseId));

            if (!complaint.OrganizationId.Equals(_orgranization.ToString()))
                throw new Exception(string.Format("Case {0} is not MasterCard", caseId));

            if (complaintRecord == null)
                throw new Exception(string.Format("Can not find Complaint Record for CaseId: {0}",caseId));
        }

        private I3dSecureProvider GetSecureProvider(ComplaintUnitOfWork unitOfWork)
        {
            switch (_orgranization)
            {
                case Common.Enum.Organization.VISA:
                    return new Visa3dSecureProvider(unitOfWork);
                case Common.Enum.Organization.MC:
                    return new Mc3dSecureProvider(unitOfWork);
                default:
                    throw new Exception(string.Format("Could not find implementation for ogranisation {0}", _orgranization.ToString()));
            }
        }
    }
}
