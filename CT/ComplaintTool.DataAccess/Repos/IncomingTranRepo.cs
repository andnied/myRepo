using System.Data.Entity;
using System.Linq;
using ComplaintTool.Models;
using System.Collections.Generic;

namespace ComplaintTool.DataAccess.Repos
{
    public class IncomingTranRepo : RepositoryBase
    {
        public IncomingTranRepo(DbContext context)
            : base(context)
        {

        }

        public IncomingTranMASTERCARD FindTranMASTERCARDByCaseId(string caseId)
        {
            return GetDbSet<IncomingTranMASTERCARD>().OrderByDescending(x => x.IncomingId).FirstOrDefault(x => x.CaseId == caseId);
        }

        public IncomingTranMASTERCARD FindTranMASTERCARDById(long? incomingId)
        {
            return GetDbSet<IncomingTranMASTERCARD>().FirstOrDefault(x => x.IncomingId == incomingId);
        }

        public IncomingTranVISA FindTranVISAById(long? incomingId)
        {
            return GetDbSet<IncomingTranVISA>().FirstOrDefault(x => x.IncomingId == incomingId);
        }

        public IncomingTranVISA FindTranVISAByCaseId(string caseId)
        {
            return GetDbSet<IncomingTranVISA>().FirstOrDefault(x => x.CaseId == caseId);
        }

        public List<string> FindFileNameVisaAndSetTempStatus(int sourceStatus, int descinationStatus)
        {
            var organizationId = Common.Enum.Organization.VISA.ToString();
            var result = (from temp in GetDbSet<Temporary>().Where(x => x.OrganizationId == organizationId)
                          join incoming in GetDbSet<IncomingTranVISA>() on temp.IncomingId equals incoming.IncomingId
                          join file in GetDbSet<RegOrgIncomingFile>() on incoming.FileId equals file.FileId
                          where temp.ProcessingStatus == sourceStatus
                          select new { Temporary = temp, FileName = file.Name }).ToList();

            result.ForEach(x =>
            {
                x.Temporary.ProcessingStatus = descinationStatus;
                Update(x.Temporary);
            });

            return result.Select(x => x.FileName).ToList();
        }

        public List<string> FindFileNameMcAndSetTempStatus(int sourceStatus, int descinationStatus)
        {
            var organizationId = Common.Enum.Organization.MC.ToString();
            var result = (from temp in GetDbSet<Temporary>().Where(x => x.OrganizationId == organizationId)
                          join incoming in GetDbSet<IncomingTranMASTERCARD>() on temp.IncomingId equals incoming.IncomingId
                          join file in GetDbSet<RegOrgIncomingFile>() on incoming.FileId equals file.FileId
                          where temp.ProcessingStatus == sourceStatus
                          select new { Temporary = temp, FileName = file.Name }).ToList();

            result.ForEach(x =>
            {
                x.Temporary.ProcessingStatus = descinationStatus;
                Update(x.Temporary);
            });

            return result.Select(x => x.FileName).ToList();
        }

        public IncomingTranMASTERCARD FindIncomingMcForImageProProcess(string arn, string kkoCbReference, string mti,
            string functionCode)
        {
            if (!string.IsNullOrEmpty(mti) && !string.IsNullOrEmpty(functionCode))
                return
                    GetDbSet<IncomingTranMASTERCARD>()
                        .FirstOrDefault(
                            x =>
                                x.ARN.Equals(arn) && x.KKOCbReference.Equals(kkoCbReference) && x.MTI.Equals(mti) &&
                                x.FunctionCode.Equals(functionCode));
            return
                    GetDbSet<IncomingTranMASTERCARD>()
                        .FirstOrDefault(
                            x =>
                                x.ARN.Equals(arn) && x.KKOCbReference.Equals(kkoCbReference));
        }
    }
}
