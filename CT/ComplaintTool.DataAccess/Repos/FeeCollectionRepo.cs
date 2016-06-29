using ComplaintTool.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.DataAccess.Repos
{
    public class FeeCollectionRepo : RepositoryBase
    {
        public FeeCollectionRepo(DbContext context) 
            : base(context)
        {
        }

        public FeeCollectionExtract FindExtractByCaseIdAndFileName(string caseId, string sourceFileName)
        {
            return base.GetDbSet<FeeCollectionExtract>()
                .Include(f => f.FeeCollection)
                .Include(f => f.FeeCollectionPostilionFile)
                .FirstOrDefault(f => 
                    f.CaseId == caseId && 
                    f.Status == 2 && 
                    f.FeeCollectionPostilionFile.FileName == sourceFileName);
        }

        public FeeCollection FindFeeCollectionById(long id)
        {
            return base.GetDbSet<FeeCollection>().FirstOrDefault(f => f.FeeCollectionId == id);
        }

        //public ICollection FindFeeCollectionExtractsForExtract(string orgId, int status)
        //{
        //    return (
        //        from f in GetDbSet<FeeCollectionExtract>()
        //        where f.Status == status && f.FeeCollection.Complaint.OrganizationId == orgId
        //        select f
        //        ).ToList();
        //}

        public int GetFeeCollectionPostilionFilesCount()
        {
            return base.GetDbSet<FeeCollectionPostilionFile>().Count(x => x.IsSend == true);
        }

        public FeeCollectionPostilionFile Add(FeeCollectionPostilionFile repPosFile)
        {
            return base.GetDbSet<FeeCollectionPostilionFile>().Add(repPosFile);
        }
    }
}
