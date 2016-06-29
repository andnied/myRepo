using System;
using ComplaintTool.Common.Config;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;

namespace ComplaintTool.Postilion.Incoming.Model
{
    class FeeCollection : PostilionIncomingAbstract
    {
        public FeeCollection(ComplaintUnitOfWork unitOfWork) 
            : base(unitOfWork)
        { }

        public override void ProcessMessage(string sourceFileNameWithoutExtension, string tempCaseId, string postilionStatus, string sourceFileName)
        {
            var feeColExtract = _unitOfWork.Repo<FeeCollectionRepo>().FindExtractByCaseIdAndFileName(tempCaseId, sourceFileName);
            if (feeColExtract != null)
            {
                feeColExtract.PostilionStatusMessage = sourceFileNameWithoutExtension;
                feeColExtract.PostilionStatus = postilionStatus;
                feeColExtract.Status = 3;
                _unitOfWork.Repo<FeeCollectionRepo>().Update(feeColExtract);

                AddNote(tempCaseId, postilionStatus, "FeeCollection");
                if (AddToCRBReportItem(feeColExtract))
                {
                    _unitOfWork.Repo<ComplaintRepo>().AddComplaintNote(feeColExtract.CaseId,
                            string.Format("FeeCollection case:{0} add to CRB Report.",
                                feeColExtract.CaseId));
                }

                if ("00".Equals(postilionStatus))
                    AddFinancialBalance(feeColExtract.FeeCollection.StageId);

                GflAndRecoveryVerification(feeColExtract);

                _unitOfWork.Commit();
            }
            else
            {
                throw new Exception(string.Format(ComplaintConfig.Instance.Notifications[542].MessageText, tempCaseId));
            }
        }

        public override bool AddToCRBReportItem(object feeColExtract)
        {
            var fce = (FeeCollectionExtract)feeColExtract;
            var complaint = _unitOfWork.Repo<ComplaintRepo>().FindByCaseId(fce.CaseId);
            if (complaint == null) return false;

            var binList = _unitOfWork.Repo<BINListRepo>().FindBINListByBin(complaint.BIN, true);
            if (binList == null || !(binList.CRBReport ?? false)) return false;

            var feeCollection = fce.FeeCollection;

            _unitOfWork.Repo<CRBReportRepo>().Add(new CRBReportItem()
            {
                CaseId = complaint.CaseId,
                StageId = feeCollection.StageId,
                ValueId = feeCollection.ValueId,
                RecordsId = feeCollection.RecordsId,
                CBReportStatus = 0
            });

            return true;
        }

        public void GflAndRecoveryVerification(FeeCollectionExtract feeCollectionExtract)
        {
            var isRecovery = _unitOfWork.Repo<RecoveryCardRepo>().IsRecoveryCard(feeCollectionExtract.CaseId);
            var isGfl = _unitOfWork.Repo<GoodFaithLetterRepo>().IsGfl(feeCollectionExtract.CaseId);

            if (!isRecovery && !isGfl) return;

            var internalStageDefinition = _unitOfWork.Repo<StageRepo>().FindInternalStageDefinition(
                feeCollectionExtract.PostilionStatus.Equals("00") ? "EXTRIMP" : "INCMPL");

            if (isRecovery && internalStageDefinition.IsRecoveryCard)
            {
                var newStage = new RecoveryCardStage
                {
                    CaseId = feeCollectionExtract.CaseId,
                    InternalStageCode = internalStageDefinition.InternalStageCode,
                    StageDate = DateTimeOffset.Now,
                };
                _unitOfWork.Repo<RecoveryCardRepo>().AddRecoveryCardStage(newStage);
            }

            if (isGfl && internalStageDefinition.IsGoodFaithLetter)
            {
                var newStage = new GoodFaithLetterStage
                {
                    CaseId = feeCollectionExtract.CaseId,
                    InternalStageCode = internalStageDefinition.InternalStageCode,
                    StageDate = DateTimeOffset.Now,
                };
                _unitOfWork.Repo<GoodFaithLetterRepo>().AddGoodFaithLetterStage(newStage);
            }
        }
    }
}
