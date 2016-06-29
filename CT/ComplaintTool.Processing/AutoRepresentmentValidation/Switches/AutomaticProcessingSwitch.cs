using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.Processing.AutoRepresentmentValidation.Validating;
using System;
using System.Collections.Generic;
using System.Linq;
using Model = ComplaintTool.Processing.AutoRepresentmentValidation.Representments;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Processing.AutoRepresentmentValidation.Switches
{
    public class AutomaticProcessingSwitch:IProcessingSwitch
    {
        private ComplaintUnitOfWork _unitOfWork;
        private readonly Model.Representment _representment;
        private readonly ILogger _logger = LogManager.GetLogger();
        private const string Note = "Autorepresentment process: {0}";

        public AutomaticProcessingSwitch(Model.Representment representment)
        {
            _representment = representment;
        }

        public void Switch(string caseId)
        {
            _unitOfWork = ComplaintUnitOfWork.Create();
            try
            {
                using (_unitOfWork)
                {
                    var complaint = _unitOfWork.Repo<ComplaintRepo>().FindByCaseId(caseId);
                    if (complaint == null)
                        throw new Exception(string.Format("Can not find complaint for caseId {0}", caseId));

                    var complaintRecord = _unitOfWork.Repo<ComplaintRepo>().FindLastComplaintRecord(complaint.CaseId);

                    if (complaintRecord == null)
                        throw new Exception(string.Format("Can not find Complaint record for case Id:{0}", complaint.CaseId));

                    var updatedRecord = !complaintRecord.InsertMode.HasValue ? _unitOfWork.Repo<AutoRepresentmentRepo>().UpdateComplaintRecord(complaintRecord) : complaintRecord;

                    if (!Validate(updatedRecord, complaintRecord))
                        return;

                    var completer = new AutomaticSwitchCompleter(_unitOfWork, _representment);
                    completer.Complete(complaint, updatedRecord);
                    var noteText = string.Format(Note, _representment.MemberMessageText != null ? _representment.MemberMessageText.MessageText : string.Empty);
                    _unitOfWork.Repo<ComplaintRepo>().AddComplaintNote(caseId, noteText);
                    _unitOfWork.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogComplaintException(ex);
                SwitchToManual(caseId);
            }
        }

        private bool Validate(ComplaintRecord updatedRecord, ComplaintRecord complaintRecord)
        {
            if (complaintRecord.InsertMode.HasValue) return true;
            var validating = new AutoRepresentmentValidating(_unitOfWork);
            var notValidatingItems = validating.ValidateRepresentmentData(updatedRecord);
            LogErrors(notValidatingItems);
            return !notValidatingItems.Any();
        }

        private void LogErrors(IEnumerable<ValidatingItem> notValidatingItems)
        {
            var logMsg = string.Empty;
            foreach (var item in notValidatingItems)
            {
                logMsg = logMsg + string.Format("ComparedItem -> {0} : {1}. RecordItem -> {2} : {3}.\n",
                    item.ComparedItem.Name, item.ComparedItem.Value, item.RecordItem.Name, item.RecordItem.Value);
            }
            if (!string.IsNullOrEmpty(logMsg))
                _logger.LogComplaintException(new Exception(logMsg));
        }

        private static void SwitchToManual(string caseId)
        {
            var processSwitch = new ManualProcessingSwitch();
            processSwitch.Switch(caseId);
        }
    }
}
