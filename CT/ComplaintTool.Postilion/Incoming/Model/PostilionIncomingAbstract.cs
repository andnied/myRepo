using ComplaintTool.Common.Enum;
using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Repos;

namespace ComplaintTool.Postilion.Incoming.Model
{
    public abstract class PostilionIncomingAbstract
    {
        protected readonly ComplaintUnitOfWork _unitOfWork;

        #region Constructor

        public static PostilionIncomingAbstract GetInstance(ReplyMode mode, ComplaintUnitOfWork unitOfWork)
        {
            if (mode == ReplyMode.Representment)
                return new Representment(unitOfWork);
            else if (mode == ReplyMode.FeeCollection)
                return new FeeCollection(unitOfWork);

            return null;
        }

        protected PostilionIncomingAbstract(ComplaintUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? ComplaintUnitOfWork.Create();
        }

        #endregion

        public abstract void ProcessMessage(string sourceFileNameWithoutExtension, string tempCaseId, string postilionStatus, string sourceFileName);

        public abstract bool AddToCRBReportItem(object extract);
        
        protected void AddNote(string caseId, string postilionStatus, string source)
        {
            string note;

            switch (postilionStatus)
            {
                //- ‘00’ – Approved or completed successfully
                case "00":
                    note = string.Format("Case {0} Postilion response: {1} status: 00 – Approved or completed successfully", caseId, source);
                    _unitOfWork.Repo<ComplaintRepo>().AddComplaintNote(caseId, note);
                    break;
                //- ‘06’ – Error
                case "06":
                    note = string.Format("Case {0} Postilion response: {1} status: 06 – Error", caseId, source);
                    _unitOfWork.Repo<ComplaintRepo>().AddComplaintNote(caseId, note);
                    break;
                //- ‘30’ – Format Error
                case "30":
                    note = string.Format("Case {0} Postilion response: {1} status: 30 – Format Error", caseId, source);
                    _unitOfWork.Repo<ComplaintRepo>().AddComplaintNote(caseId, note);
                    break;
                //- ‘92’ – Routing error
                case "92":
                    note = string.Format("Case {0} Postilion response: {1} status: 92 – Routing error", caseId, source);
                    _unitOfWork.Repo<ComplaintRepo>().AddComplaintNote(caseId, note);
                    break;
                //- ‘94’ – Duplicate transaction
                case "94":
                    note = string.Format("Case {0} Postilion response: {1} status: 94 – Duplicate transaction", caseId, source);
                    _unitOfWork.Repo<ComplaintRepo>().AddComplaintNote(caseId, note);
                    break;
                //- ‘96’ – System malfunction
                case "96":
                    note = string.Format("Case {0} Postilion response: {1} status: 96 – System malfunction", caseId, source);
                    _unitOfWork.Repo<ComplaintRepo>().AddComplaintNote(caseId, note);
                    break;

                default:
                    note = string.Format("Case {0} Postilion response: {1} status: {2} - unknown", caseId, source, postilionStatus);
                    _unitOfWork.Repo<ComplaintRepo>().AddComplaintNote(caseId, note);
                    break;
            }
        }

        protected void AddFinancialBalance(long stageId)
        {
            _unitOfWork.Repo<FinancialBalanceRepo>().AddFinancialBalance(stageId, null, null, null);
        }
    }
}
