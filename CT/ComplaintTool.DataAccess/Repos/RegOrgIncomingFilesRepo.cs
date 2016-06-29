using ComplaintTool.Models;
using eService.MCParser.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ComplaintTool.DataAccess.Repos
{
    public class RegOrgIncomingFilesRepo : RepositoryBase
    {
        public RegOrgIncomingFilesRepo(DbContext context)
            : base(context)
        {
        }

        public bool RegOrgExists(string incomingId)
        {
            return GetDbSet<RegOrgIncomingFile>().Any(r => r.IncomingFileID.Equals(incomingId));
        }

        public RegOrgIncomingFile AddRegOrg(RegOrgIncomingFile regOrg)
        {
            return GetDbSet<RegOrgIncomingFile>().Add(regOrg);
        }

        public void Update(RegOrgIncomingFile regOrg)
        {
            GetDbSet<RegOrgIncomingFile>().Attach(regOrg);
            SetEntityState(regOrg, EntityState.Modified);
        }

        public bool InsertOrgIncomingTranVISA(IEnumerable<IncomingRecordVisaMid> incomingRecordVisaList, int processingMode)
        {
            return incomingRecordVisaList.All(record => base.InsertOrgIncomingTranVISA(record, processingMode) == "1");
        }

        public string InsertOrgIncomingTranMASTERCARD(IEnumerable<BlkModel> incomingMcList, int processingMode)
        {
            foreach (var record in incomingMcList)
            {
                var result = base.InsertOrgIncomingTranMASTERCARD(record, processingMode);
                if (result != "1")
                    return result;
            }

            return "1";
        }

        public RegOrgIncomingFile FindRegOrgIncomingFile(int fileId)
        {
            return GetDbSet<RegOrgIncomingFile>().FirstOrDefault(x => x.FileId == fileId);
        }

        public IEnumerable<RegOrgIncomingFile> FindRegOrgsForProcessing(string orgId)
        {
            var result = GetDbSet<RegOrgIncomingFile>()
                .Where(r => 
                    r.ErrorFlag == false && r.Status == 2 &&
                    r.OrganizationId == orgId && r.stream_id.HasValue)
                    .ToList();

            if (orgId == "MC")
                return result.Where(r => r.Name.EndsWith(".001"));

            return result;
        }

        public IEnumerable<RegOrgIncomingFile> FindRegOrgsByFileName(string fileName)
        {
            return GetDbSet<RegOrgIncomingFile>().Where(r => r.Name == fileName).ToList();
        }

        //public void ResumeIncomingFile(string fileName,int processingMode)
        //{
        //    var incomingFile = GetDbSet<RegOrgIncomingFile>().FirstOrDefault(x => x.Name.Equals(fileName));
        //    if (incomingFile == null) throw new Exception(string.Format("Cannot find file {0} in Complaint database.", fileName));
        //    incomingFile.ErrorFlag = false;
        //    incomingFile.Status = (int)RegOrgIncomingStatus.NotProcessed;
        //    incomingFile.ProcessingMode = processingMode;
        //    Update(incomingFile);
        //}

        public RegOrgIncomingFile GetRegOrgIncomingFile(string fileName)
        {
            return GetDbSet<RegOrgIncomingFile>().FirstOrDefault(x => x.Name.Equals(fileName));
        }
    }
}
