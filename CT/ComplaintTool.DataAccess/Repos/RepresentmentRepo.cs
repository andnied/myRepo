using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ComplaintTool.Models;
using System.Collections;

namespace ComplaintTool.DataAccess.Repos
{
    public class RepresentmentRepo : RepositoryBase
    {
        public RepresentmentRepo(DbContext context)
            : base(context)
        {
        }

        public IEnumerable<Representment> FindRepresentmentsForExtract(string orgId)
        {
            var representments = GetDbSet<Representment>().Include(x => x.RepresentmentDocuments);
            return (
                from r in representments
                where r.Status == 2 
                where r.Complaint.OrganizationId == orgId
                where r.RepresentmentDocuments.Any(x => x.Status == 0)
                select r
                ).ToList();
        }

        public IEnumerable<Representment> FindRepresentmentsForExtract(string orgId, int status)
        {
            return (
                from r in GetDbSet<Representment>().Include(r => r.RepresentmentDocuments).Include(r => r.Complaint)
                where r.Status == status && r.Complaint.OrganizationId == orgId
                select r
                ).ToList();
        }

        public ICollection FindRepresentmentExtractsForExtract(string orgId, int status)
        {
            return (
                from r in GetDbSet<RepresentmentExtract>()
                where r.Status == status && r.Representment.Complaint.OrganizationId == orgId
                select r
                ).ToList();
        }

        public Representment FindRepresentmentById(long id)
        {
            return GetDbSet<Representment>().FirstOrDefault(r => r.RepresentmentId == id);
        }

        public RepresentmentExtract FindRepresentmentExtract(Representment representment)
        {
            return GetDbSet<RepresentmentExtract>().SingleOrDefault(x => x.CaseId == representment.CaseId && x.RepresentmentId == representment.RepresentmentId);
        }

        public RepresentmentExtract FindRepresentmentExtractByCaseIdAndFileName(string caseId, string fileName)
        {
            return GetDbSet<RepresentmentExtract>()
                .Include(r => r.Representment)
                .Include(r => r.RepresentmentPostilionFile)
                .FirstOrDefault(x => 
                    x.CaseId == caseId && 
                    x.Status == 2 && 
                    x.RepresentmentPostilionFile.FileName == fileName);
        }

        public void Update(Representment representment)
        {
            GetDbSet<Representment>().Attach(representment);
            SetEntityState(representment, EntityState.Modified);
            Commit();
        }

        public void Update(RepresentmentDocument doc)
        {
            GetDbSet<RepresentmentDocument>().Attach(doc);
            SetEntityState(doc, EntityState.Modified);
            Commit();
        }

        //public void Update(RepresentmentExtract representmentExtract)
        //{
        //    GetDbSet<RepresentmentExtract>().Attach(representmentExtract);
        //    SetEntityState(representmentExtract, EntityState.Modified);
        //    Commit();
        //}

        public void Add(RepresentmentExtractDocument extrDoc)
        {
            base.GetDbSet<RepresentmentExtractDocument>().Add(extrDoc);
        }

        public RepresentmentPostilionFile Add(RepresentmentPostilionFile repPosFile)
        {
            return base.GetDbSet<RepresentmentPostilionFile>().Add(repPosFile);
        }

        public void Add(RepresentmentExtract repExtr)
        {
            base.GetDbSet<RepresentmentExtract>().Add(repExtr);
        }

        public Representment Add(Representment representment)
        {
           return GetDbSet<Representment>().Add(representment);
        }

        public int GetRepresentmentPostilionFilesCount()
        {
            return base.GetDbSet<RepresentmentPostilionFile>().Count(x => x.IsSend == true);
        }

        public void UpdateToExtracted(Representment representment)
        {
            representment.Status = 3;
            Update<Representment>(representment);
        }

        public RepresentmentCondition FindAutomaticRepresentmentCondition(string organizationId, string stageCode, string reasonCode, bool? is3dSecure = null, int dayStep = 0)
        {
            var representmentConditions = GetDbSet<RepresentmentCondition>().Where(x => x.OrganizationId == organizationId && x.IsAutomatic && x.SourceStageCode == stageCode && x.SourceReasonCode == reasonCode && x.DayStep == dayStep);
            if (is3dSecure.HasValue)
                return representmentConditions.FirstOrDefault(x => x.Is3DSecure == is3dSecure.Value);

            return representmentConditions.FirstOrDefault();
        }

    }
}
