using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ComplaintTool.Models;
using ComplaintTool.Common.Enum;
using System.Security.Principal;

namespace ComplaintTool.DataAccess.Repos
{
    public class ComplaintRepo : RepositoryBase
    {
        public ComplaintRepo(DbContext context) 
            : base(context)
        {
        }

        public ComplaintValue AddComplaintValue(ComplaintValue complantValue)
        {
            return GetDbSet<ComplaintValue>().Add(complantValue);
        }

        public ComplaintAutomaticEvent AddComplaintAutomaticEvent(ComplaintAutomaticEvent complaintAutomaticEvent)
        {
            return GetDbSet<ComplaintAutomaticEvent>().Add(complaintAutomaticEvent);
        }

        public ComplaintAutomaticEvent AddComplaintAutomaticEventsWithLastFK(Complaint complaint)
        {
            var automaticKey = Guid.NewGuid();
                var value=complaint.ComplaintValues.OrderByDescending(x=>x.ValueId).FirstOrDefault();
                var stage=complaint.ComplaintValues.OrderByDescending(x=>x.StageId).FirstOrDefault();
                var record=complaint.ComplaintRecords.OrderByDescending(x=>x.RecordId).FirstOrDefault();
                var automaticEvent = new ComplaintAutomaticEvent()
                {
                    CaseId = complaint.CaseId,
                    AutomaticKey = automaticKey,
                    AutomaticProcess = AutomaticProcess.CaseAutoClosed.ToString(),
                    ValueId = value != null ? value.ValueId : 0,
                    RecordsId = record != null ? record.RecordId : 0,
                    StageId = stage != null ? (stage.StageId ?? 0) : 0,
                    InsertDate = DateTime.UtcNow,
                    InsertUser = GetInsertUserName()
                };
                AddComplaintAutomaticEvent(automaticEvent);

            return automaticEvent;
        }

        private static string GetInsertUserName()
        {
            var identity = WindowsIdentity.GetCurrent();
            var currentUser = identity != null ? identity.Name : "ComplaintServices";
            return currentUser;
        }

        public Complaint FindByCaseIdAndARN(string caseId, string arn)
        {
            return GetDbSet<Complaint>().FirstOrDefault(x => x.CaseId == caseId && x.ARN == arn);
        }

        public Complaint FindByCaseId(string caseId)
        {
            return GetDbSet<Complaint>().FirstOrDefault(x => x.CaseId == caseId);
        }

        public IEnumerable<Complaint> FindWithoutPostilionData()
        {
            return GetDbSet<Complaint>().Where(x => x.PostilionData.HasValue&&!x.PostilionData.Value).ToList();
        }

        public Complaint FindByARN(string arn)
        {
            return GetDbSet<Complaint>().FirstOrDefault(x => x.ARN == arn);
        }

        public IEnumerable<Complaint> FindNotClose(int processingStatus)
        {
            return GetDbSet<Complaint>().Where(x => x.ProcessingStatus.HasValue && x.ProcessingStatus.Value == processingStatus && x.Close.HasValue && !x.Close.Value).ToList();
        }

        public IEnumerable<Complaint> FindComplaintsWithout3dSecure(string organizationId)
        {
            return GetDbSet<Complaint>().Where(x => x.OrganizationId == organizationId && !x.Is3DSecure.HasValue).ToList();
        }
      

        public ComplaintStage FindLastStageByCaseId(string caseId)
        {
            return GetDbSet<ComplaintStage>()
                .OrderByDescending(x => x.StageId)
                .FirstOrDefault(x => x.CaseId == caseId);
        }

        public ComplaintStage FindLastStageForCaseFiling(string caseId)
        {
            var paou = ComplaintTool.Common.Config.ComplaintConfig.Instance.StageDefinitions[6];
            var pcou = ComplaintTool.Common.Config.ComplaintConfig.Instance.StageDefinitions[12];

            return GetDbSet<ComplaintStage>()
                .OrderByDescending(x => x.StageId)
                .FirstOrDefault(
                    x => x.CaseId == caseId && 
                        (x.StageCode == paou.StageCode || x.StageCode == pcou.StageCode));
        }

        public ComplaintStage GetStageById(long? stageId)
        {
            return GetDbSet<ComplaintStage>().FirstOrDefault(x => x.StageId == stageId);
        }

        public void UnsuspendCase(string caseId)
        {
            var currentDate = GetCurrentDateTime();
            var suspend = GetDbSet<ComplaintSuspended>()
                .OrderByDescending(x => x.SuspendedId)
                .FirstOrDefault(x => x.CaseId == caseId && x.SuspendedDay == 8
                && x.SuspendedEndDate > currentDate);

            if (suspend != null)
            {
                var newUnsuspend = new ComplaintSuspended
                {
                    CaseId = caseId,
                    SuspendedDate = suspend.SuspendedDate,
                    SuspendedDay = (currentDate.Date - suspend.SuspendedDate.Date).Days,
                    SuspendedEndDate = currentDate,
                    InsertDate = currentDate,
                    InsertUser = GetCurrentUser()
                };
                GetDbSet<ComplaintSuspended>().Add(newUnsuspend);
            }
            Commit();
        }

        public void AddComplaintSuspend(ComplaintSuspended complaintSuspended)
        {
            GetDbSet<ComplaintSuspended>().Add(complaintSuspended);
        }

        public bool AddComplaintNote(string caseId, string note)
        {
            var newComplaintNote = new ComplaintNote
            {
                CaseId = caseId,
                Note = note,
                InsertDate = GetCurrentDateTime(),
                InsertUser = GetCurrentUser()
            };
            GetDbSet<ComplaintNote>().Add(newComplaintNote);
            Commit();
            return true;
        }

        public void AddComplaintNote(string caseId, int msgNumber,params object[] msgParams)
        {
            var notificationDefinition = ComplaintTool.Common.Config.ComplaintConfig.Instance.Notifications[msgNumber];
            var message = msgParams.Any()? string.Format(notificationDefinition.MessageText, msgParams): notificationDefinition.MessageText;
            AddComplaintNote(caseId, message);
        }

        public ComplaintRecord FindRecordByIssuerReference(string issuerRefData)
        {
            return GetDbSet<ComplaintRecord>().FirstOrDefault(x => x.KKOCbReference == issuerRefData);
        }

        public ComplaintRecord FindRecordByStageId(long stageId)
        {
            return GetDbSet<ComplaintRecord>().FirstOrDefault(x => x.StageId == stageId);
        }

        public ComplaintRecord FindRecordByRecordId(long? recordId)
        {
            return GetDbSet<ComplaintRecord>().FirstOrDefault(x => x.RecordId == recordId);
        }

        public ComplaintRecord FindLastComplaintRecord(string caseId)
        {
            return GetDbSet<ComplaintRecord>().Where(x => x.CaseId == caseId).OrderByDescending(x => x.RecordId).FirstOrDefault();
        }

        public IEnumerable<Complaint> FindComplaintsWithDocumentExport(string orgId)
        {
            var complaints = GetDbSet<Complaint>();
            var documentExports = GetDbSet<DocumentExport>();
            return (from c in complaints
                    join dex in documentExports
                    on c.CaseId equals dex.CaseId
                    where dex.Status == 0 && dex.OrganizationId == orgId
                    select c).ToList();
        }

        public IEnumerable<Complaint> FindComplaintsWithoutSuspendNotesCf(string noteMsg,string cfFileType)
        {
            int manualStatus=(int)ComplaintTool.Common.Enum.ProcessingStatus.ManualProcess;
            IQueryable<Complaint> complaints=GetDbSet<Complaint>().Where(x=>x.ProcessingStatus==manualStatus&&x.Close.HasValue&&!x.Close.Value);
            IQueryable<ComplaintSuspended> complaintSuspended = GetDbSet<ComplaintSuspended>().Where(x=>x.SuspendedDay==8&&x.SuspendedEndDate<DateTimeOffset.Now);
            IQueryable<ComplaintNote> complaintNotes = GetDbSet<ComplaintNote>().Where(x=>x.Note==noteMsg);
            IQueryable<CaseFilingIncomingFile> caseFilings = GetDbSet<CaseFilingIncomingFile>().Where(x=>x.FileType==cfFileType);
            complaints = complaints.Where(x => !complaintSuspended.Select(y => y.CaseId).Contains(x.CaseId));
            complaints = complaints.Where(x => !complaintNotes.Select(y => y.CaseId).Contains(x.CaseId));
            complaints = complaints.Where(x => !caseFilings.Select(y => y.CaseId).Contains(x.CaseId));
            return complaints.ToList();
        }

        public List<CaseItem> GetCaseItemsList()
        {
            var bins = GetDbSet<BINList>().Where(x => x.DocumentProcessStatus == 1 && x.ProductionStatus == true).Select(x => x.BIN);
            var stages = GetDbSet<StageDefinition>().Where(x => x.DocumentProcess == 1 && x.IsActive == true).Select(x => x.StageCode);
            var casesId = GetDbSet<Complaint>().Where(x => bins.Contains(x.BIN)).Select(x => x.CaseId);
            var complaintStages = FindYesterdayStagesWithoutDocuments(bins, stages, casesId);
            return complaintStages.ToList();
        }

        private IQueryable<CaseItem> FindYesterdayStagesWithoutDocuments(IQueryable<string> bins, IQueryable<string> stageCodes, IQueryable<string> caseIds)
        {
            DateTimeOffset yesterdayDate = DateTime.Now.AddDays(-1);
            var compaintStages = GetDbSet<ComplaintStage>().Where(x => bins.Contains(x.Complaint.BIN) && stageCodes.Contains(x.StageCode) && caseIds.Contains(x.CaseId));
            var yesterdayStages = compaintStages.Where(x => x.StageDate != null && x.StageDate.Value.Year == yesterdayDate.Year && x.StageDate.Value.Month == yesterdayDate.Month && x.StageDate.Value.Day == yesterdayDate.Day);
            var caseItemsWithoutDocument = yesterdayStages.Select(x => new { x.CaseId, x.StageId, x.StageCode }).Except(GetDbSet<ComplaintStageDocument>().Select(x => new { x.CaseId, x.StageId, x.StageCode }));
            IQueryable<CaseItem> result = caseItemsWithoutDocument.Select(x => new CaseItem() { CaseId = x.CaseId, StageCode = x.StageCode, StageId = x.StageId });
            return result;
        }

        public ComplaintValue GetComplaintValueByStageId(long stageId)
        {
            return GetDbSet<ComplaintValue>().FirstOrDefault(x => x.StageId == stageId);
        }

        public ComplaintValue FindComplaintValueByValueId(long? valueId)
        {
            return GetDbSet<ComplaintValue>().FirstOrDefault(x => x.ValueId == valueId);
        }

        public ComplaintValue FindLastComplaintValue(string caseId)
        {
            return GetDbSet<ComplaintValue>().Where(x => x.CaseId == caseId).OrderByDescending(x => x.ValueId).FirstOrDefault();
        }

        public IEnumerable<ComplaintValue> GetValuesForStage(IEnumerable<ComplaintStage> stages)
        {
            var result = from complaintStage in stages
                         join complaintValue in GetDbSet<ComplaintValue>() on complaintStage.StageId equals complaintValue.StageId into sg
                         from subset in sg.DefaultIfEmpty()
                         select subset;

            return result.ToList();
        }

        public bool ComplaintDocumentExists(string caseId)
        {
            return GetDbSet<ComplaintDocument>().Any(c => c.CaseId == caseId);
        }

        public bool ComplaintExists(string caseId)
        {
            return GetDbSet<Complaint>().Any(c => c.CaseId == caseId);
        }
    }
}
