using ComplaintTool.Common.Config;
using ComplaintTool.Models;
using ComplaintTool.Shell.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using ComplaintTool.Shell.Documents;
using System.IO;
using System.Globalization;
// ReSharper disable ImplicitlyCapturedClosure

namespace ComplaintTool.Shell.Tests.Documents_Test
{
    [TestClass]
    public class MerchantTest
    {
        private const string ExportDocumentsFolder = @"C:\ComplaintHelpers\Documents";
        private const string ExportMerchantFolder = @"C:\ComplaintHelpers\Merchants";
        private const string Doch = "DOCH";

        private readonly List<ComplaintStage> _beforeTestSetStages = new List<ComplaintStage>();
        private readonly List<BINList> _beforeTestSetBin = new List<BINList>();

        [TestMethod]
        public void ExportTest()
        {
            var startTime=DateTime.Now;
            var correctComplaintStagesDocs=PrepareTestSet();
            var cmd = new GetDocumentExport() { DocumentFolder = ExportDocumentsFolder, MerchantFolder = ExportMerchantFolder };
            cmd.Process();
            try
            {
                if (!Validate(correctComplaintStagesDocs, startTime))
                    throw new Exception();
            }
            catch
            {
                RevertChanges(correctComplaintStagesDocs);
                Assert.Fail();
            }
        }

        private List<Tuple<ComplaintStageDocument, byte[]>> PrepareTestSet()
        {
            var tuples=new List<Tuple<ComplaintStageDocument,byte[]>>();
            using(var entities=new ComplaintEntities(ComplaintConfig.Instance.GetEntityConnectionString()))
            {
                entities.Database.BeginTransaction();
                var complaintStagesDocuments = GetComplaintStageDocuments(entities, 5);
                var storedProcedure=new StoredProcedure(entities);
                foreach(var csDoc in complaintStagesDocuments)
                {
                    string fileName;
                    byte [] bytes;
                    storedProcedure.GetDocumentProc(csDoc.DocumentId,out fileName,out bytes);
                    tuples.Add(new Tuple<ComplaintStageDocument, byte[]>(Clone(csDoc), bytes));
                }
                UpdateBinStates(entities, complaintStagesDocuments);
                UpdateForYesteradayDate(entities,complaintStagesDocuments);
                entities.Set<ComplaintStageDocument>().RemoveRange(complaintStagesDocuments);
                entities.Database.CurrentTransaction.Commit();
                entities.SaveChanges();
            }
            return tuples;
        }

        private static List<ComplaintStageDocument> GetComplaintStageDocuments(DbContext entities,int amount)
        {
            var bins = entities.Set<BINList>().Where(x => x.ProductionStatus == true).Select(x => x.BIN);
            var stages = entities.Set<StageDefinition>().Where(x => x.DocumentProcess == 1 && x.IsActive).Select(x => x.StageCode);
            var casesId = entities.Set<Complaint>().Where(x => bins.Contains(x.BIN)).Select(x => x.CaseId);
            var compaintStages = entities.Set<ComplaintStage>().Where(x => bins.Contains(x.Complaint.BIN) && stages.Contains(x.StageCode) && casesId.Contains(x.CaseId));
            var complaintStagesDocuments = entities.Set<ComplaintStageDocument>().Include(x => x.Complaint).Where(x => compaintStages.Select(y => y.CaseId).Contains(x.CaseId) && compaintStages.Select(y => y.StageCode).Contains(x.StageCode) && compaintStages.Select(y => y.StageId).Contains(x.StageId)).Distinct().Take(amount).ToList();

            return complaintStagesDocuments;
        }

        private static ComplaintStageDocument Clone(ComplaintStageDocument csDoc)
        {
            return new ComplaintStageDocument
            {
                CaseId = csDoc.CaseId,
                DocumentId = csDoc.DocumentId,
                InsertDate = csDoc.InsertDate,
                InsertUser = csDoc.InsertUser,
                StageCode = csDoc.StageCode,
                StageId = csDoc.StageId
            };
        }

        private void UpdateBinStates(ComplaintEntities entities, IEnumerable<ComplaintStageDocument> complaintStageDocuments)
        {
            var bins = complaintStageDocuments.Select(x => x.Complaint.BIN);
            var binList=entities.BINLists.Where(x => bins.Contains(x.BIN));
            binList.Distinct().ToList().ForEach(x => 
            {
                _beforeTestSetBin.Add(x.Clone());
                x.DocumentProcessStatus = 1;
                Update(x, entities);
            });
        }

        private void UpdateForYesteradayDate(DbContext entities, IEnumerable<ComplaintStageDocument> complaintStageDocuments)
        {
            DateTimeOffset yesterdayDate = DateTime.Now.AddDays(-1);
            var complaintStages = (from compStDoc in complaintStageDocuments
                       join compSt in entities.Set<ComplaintStage>() on new {compStDoc.StageId, compStDoc.StageCode, compStDoc.CaseId } equals new {compSt.StageId, compSt.StageCode, compSt.CaseId }
                       select compSt).ToList();

            complaintStages.Distinct().ToList().ForEach(x => {
                _beforeTestSetStages.Add(x.Clone());
                x.StageDate = yesterdayDate;
                Update(x,entities);
            });
        }

        private static bool Validate(IEnumerable<Tuple<ComplaintStageDocument, byte[]>> correctComplaintStageDocs,DateTime dateStart)
        {
            using (var entities = new ComplaintEntities(ComplaintConfig.Instance.GetEntityConnectionString()))
            {
                foreach (var tuple in correctComplaintStageDocs)
                {
                    var complaintStageDoc = tuple.Item1;
                    var bytes = tuple.Item2;
                    var filePath = GetPath(complaintStageDoc.StageCode,complaintStageDoc.CaseId);
                    if (!entities.ComplaintStageDocuments.Any(x => x.StageId == complaintStageDoc.StageId && x.StageCode == complaintStageDoc.StageCode && x.CaseId == complaintStageDoc.CaseId&&x.InsertDate>dateStart&&x.InsertDate<DateTime.Now))
                        return false;
                    if (!entities.ComplaintDocuments.Any(x => x.StageId == complaintStageDoc.StageId && x.CaseId == complaintStageDoc.CaseId&&x.ExportDate>dateStart&&x.ExportDate<DateTime.Now))
                        return false;
                    if (!new PdfContentComparer(filePath).Compare(bytes,true))
                        return false;
                }
            }

            return true;
        }

        private void RevertChanges(List<Tuple<ComplaintStageDocument,byte[]>> complaintStagesDoc)
        {
            using(var entities=new ComplaintEntities(ComplaintConfig.Instance.GetEntityConnectionString()))
            {
                _beforeTestSetStages.Distinct().ToList().ForEach(x => Update(x,entities));
                _beforeTestSetBin.Distinct().ToList().ForEach(x => Update(x,entities));
                complaintStagesDoc.ForEach(x => entities.Set<ComplaintStageDocument>().Add(x.Item1));
                entities.SaveChanges();
            }
        }

        private static void Update<T>(T entity,DbContext entities) where T :class
        {
            entities.Set<T>().Attach(entity);
            entities.Entry(entity).State = EntityState.Modified; 
        }

        private static string GetPath(string stageCode,string caseId)
        {
            var folder = ExportMerchantFolder + @"\"
                                               +
                                               DateTime.Now.Year.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0')
                                               +
                                               DateTime.Now.Month.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0')
                                               + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
            var stageName = ComplaintDictionaires.GetMapping(Doch, stageCode);
            var fileName = string.Format(@"{0}_{1}.{2}", stageName, caseId, "pdf");
            var filePath = Path.Combine(folder, fileName);
            return filePath;
        }
    }
}
