using System;
using ComplaintTool.Common.Config;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;

namespace ComplaintTool.Postilion.Incoming.Model
{
    class Representment : PostilionIncomingAbstract
    {
        public Representment(ComplaintUnitOfWork unitOfWork) 
            : base(unitOfWork)
        { }

        public override void ProcessMessage(string sourceFileNameWithoutExtension, string tempCaseId, string postilionStatus, string sourceFileName)
        {
            var representmentExtract = _unitOfWork.Repo<RepresentmentRepo>().FindRepresentmentExtractByCaseIdAndFileName(tempCaseId, sourceFileName);
            if (representmentExtract != null)
            {
                representmentExtract.PostilionStatusMessage = sourceFileNameWithoutExtension;
                representmentExtract.PostilionStatus = postilionStatus;
                representmentExtract.Status = 3;
                _unitOfWork.Repo<RepresentmentRepo>().Update(representmentExtract);

                AddNote(tempCaseId, postilionStatus, "Representment");
                if (AddToCRBReportItem(representmentExtract))
                {
                    _unitOfWork.Repo<ComplaintRepo>().AddComplaintNote(representmentExtract.CaseId,
                                string.Format("Representment case:{0} add to CRB Report.",
                                    representmentExtract.CaseId));
                }

                if ("00".Equals(postilionStatus))
                    AddFinancialBalance(representmentExtract.Representment.StageId);

                _unitOfWork.Commit();
            }
            else
            {
                throw new Exception(string.Format(ComplaintConfig.Instance.Notifications[542].MessageText, tempCaseId));
            }
        }

        public override bool AddToCRBReportItem(object representmentExtract)
        {
            var re = (RepresentmentExtract)representmentExtract;
            var complaint = _unitOfWork.Repo<ComplaintRepo>().FindByCaseId(re.CaseId);
            if (complaint == null) return false;

            var binList = _unitOfWork.Repo<BINListRepo>().FindBINListByBin(complaint.BIN, true);
            if (binList == null || !(binList.CRBReport ?? false)) return false;

            var representment = re.Representment;

            _unitOfWork.Repo<CRBReportRepo>().Add(new CRBReportItem
            {
                CaseId = complaint.CaseId,
                StageId = representment.StageId,
                ValueId = representment.ValueId,
                RecordsId = representment.RecordsId,
                CBReportStatus = 0
            });

            return true;
        }
    }
}
