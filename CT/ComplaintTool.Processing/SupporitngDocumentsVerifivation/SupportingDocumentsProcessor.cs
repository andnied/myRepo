using ComplaintTool.Common.CTLogger;
using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Repos;

namespace ComplaintTool.Processing.SupporitngDocumentsVerifivation
{
    public class SupportingDocumentsProcessor
    {
        private readonly ILogger _logger = LogManager.GetLogger();
        private const string NoteMsg = "Supporting document was not received in 8 days timeframe - case was unsuspended.";
        private const string CfFileType = "RMW";

        public void Process()
        {
            using(var unitOfWork=ComplaintUnitOfWork.Create())
            {
                var complaints = unitOfWork.Repo<ComplaintRepo>().FindComplaintsWithoutSuspendNotesCf(NoteMsg,CfFileType);
                var cases = string.Empty;
                foreach(var complaint in complaints)
                {
                    cases = cases + complaint.CaseId + ", ";
                    unitOfWork.Repo<ComplaintRepo>().AddComplaintNote(complaint.CaseId, NoteMsg);
                }
                unitOfWork.Commit();

                if (string.IsNullOrEmpty(cases)) return;

                cases = cases.Substring(0, cases.Length - 2);
                _logger.LogComplaintEvent(558, cases);
            }
        }
    }
}
