using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Principal;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;

namespace ComplaintTool.Postilion.Outgoing.Model.FeeCollection
{
    public abstract class Organization4105Extracter
    {
        protected ComplaintUnitOfWork UnitOfWork;
        protected Models.FeeCollection FeeCollection;

        protected Organization4105Extracter(ComplaintUnitOfWork unitOfWork, Models.FeeCollection feeCollection)
        {
            UnitOfWork = unitOfWork;
            FeeCollection = feeCollection;
        }

        public bool ExtractFeeCollection()
        {
            try
            {
                if (FeeCollection == null)
                    throw new Exception("ExtractFeeCollection Error. FeeCollection is null");
                var complaint = UnitOfWork.Repo<ComplaintRepo>().FindByCaseId(FeeCollection.CaseId);
                var recoveryCard = UnitOfWork.Repo<RecoveryCardRepo>().GetRecoveryCard(FeeCollection.CaseId);
                var gfl = UnitOfWork.Repo<GoodFaithLetterRepo>().GetGflByCaseId(FeeCollection.CaseId);

                if (complaint != null)
                    Set4105Extract(complaint);
                else if (recoveryCard != null)
                    Set4105Extract(recoveryCard);
                else if (gfl != null)
                    Set4105Extract(gfl);
                else
                    throw new Exception(string.Format("ExtractFeeCollection Error. Cannot find case with CaseID = {0}",
                        FeeCollection.CaseId));
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        protected void Set4105Extract(Complaint complaint)
        {
            var complaintValue = UnitOfWork.Repo<ComplaintRepo>().FindComplaintValueByValueId(FeeCollection.ValueId);
            var complaintStage = UnitOfWork.Repo<ComplaintRepo>().GetStageById(FeeCollection.StageId);
            var complaintRecord = UnitOfWork.Repo<ComplaintRepo>().FindRecordByRecordId(FeeCollection.RecordsId);

            if (complaintValue == null)
                throw new Exception(string.Format("Set4105Extract Error. CaseId = {0} ComplaintValues is null. ValueId = {1}",
                    FeeCollection.CaseId, FeeCollection.ValueId));
            if (complaintStage == null)
                throw new Exception(string.Format("Set4105Extract Error. CaseId = {0} ComplaintStage is null. StageId = {1}",
                    FeeCollection.CaseId, FeeCollection.StageId));
            if (complaintRecord == null)
                throw new Exception(string.Format("Set4105Extract Error. CaseId = {0} ComplaintRecord is null. RecordId = {1}",
                    FeeCollection.CaseId, FeeCollection.RecordsId));

            var i4105 = Set4105(complaint, complaintValue, complaintStage, complaintRecord);
            AddFeeCollectionExtract(i4105);
        }

        protected void Set4105Extract(GoodFaithLetter gfl)
        {
            var gflValue = UnitOfWork.Repo<GoodFaithLetterRepo>().GetGoodFaithLetterValue(FeeCollection.ValueId);
            var gflStage = UnitOfWork.Repo<GoodFaithLetterRepo>().GetGoodFaithLetterStage(FeeCollection.StageId);
            var gflRecord = UnitOfWork.Repo<GoodFaithLetterRepo>().GetGoodFaithLetterRecord(FeeCollection.RecordsId);

            if (gflValue == null)
                throw new Exception(string.Format("Set4105Extract Error. CaseId = {0} GoodFaithLetterValue is null. ValueId = {1}",
                    FeeCollection.CaseId, FeeCollection.ValueId));
            if (gflStage == null)
                throw new Exception(string.Format("Set4105Extract Error. CaseId = {0} GoodFaithLetterStage is null. StageId = {1}",
                    FeeCollection.CaseId, FeeCollection.StageId));
            if (gflRecord == null)
                throw new Exception(string.Format("Set4105Extract Error. CaseId = {0} GoodFaithLetterRecord is null. RecordId = {1}",
                    FeeCollection.CaseId, FeeCollection.RecordsId));

            var i4105 = Set4105(gfl, gflValue, gflStage, gflRecord);
            AddFeeCollectionExtract(i4105);
        }

        protected void Set4105Extract(RecoveryCard recoveryCard)
        {
            var recoveryCardValue = UnitOfWork.Repo<RecoveryCardRepo>().GetRecoveryCardValue(FeeCollection.ValueId);
            var recoveryCardStage = UnitOfWork.Repo<RecoveryCardRepo>().GetRecoveryCardStage(FeeCollection.StageId);
            var recoveryCardRecord = UnitOfWork.Repo<RecoveryCardRepo>().GetRecoveryCardRecord(FeeCollection.RecordsId);

            if (recoveryCardValue == null)
                throw new Exception(string.Format("Set4105Extract Error Error. CaseId = {0} RecoveryCardValue is null. ValueId = {1}",
                    FeeCollection.CaseId, FeeCollection.ValueId));
            if (recoveryCardStage == null)
                throw new Exception(string.Format("Set4105Extract Error Error. CaseId = {0} RecoveryCardStage is null. StageId = {1}",
                    FeeCollection.CaseId, FeeCollection.StageId));
            if (recoveryCardRecord == null)
                throw new Exception(string.Format("Set4105Extract Error Error. CaseId = {0} RecoveryCardRecord is null. RecordId = {1}",
                    FeeCollection.CaseId, FeeCollection.RecordsId));

            var i4105 = Set4105(recoveryCard, recoveryCardValue, recoveryCardStage, recoveryCardRecord);
            AddFeeCollectionExtract(i4105);
        }

        public MC4105 LoadMasterCardFeeConfiguration(string countryCode, int feeTypeId,
            out MasterCard.MockData mockData)
        {
            var feeConfiguration = UnitOfWork.Repo<FeeConfigurationRepo>()
                .GetFeeConfiguration(Common.Enum.Organization.MC, countryCode, feeTypeId);

            var attr003 = feeConfiguration.FirstOrDefault(x => x.Attribute.Equals("003"));
            var attr022 = feeConfiguration.FirstOrDefault(x => x.Attribute.Equals("022"));
            var attr025 = feeConfiguration.FirstOrDefault(x => x.Attribute.Equals("025"));
            var attr032 = feeConfiguration.FirstOrDefault(x => x.Attribute.Equals("032"));
            var attr041 = feeConfiguration.FirstOrDefault(x => x.Attribute.Equals("041"));
            var attr042 = feeConfiguration.FirstOrDefault(x => x.Attribute.Equals("042"));
            var attr043 = feeConfiguration.FirstOrDefault(x => x.Attribute.Equals("043"));
            var attr049 = feeConfiguration.FirstOrDefault(x => x.Attribute.Equals("049"));
            var attr056 = feeConfiguration.FirstOrDefault(x => x.Attribute.Equals("056"));
            var attr100 = feeConfiguration.FirstOrDefault(x => x.Attribute.Equals("100"));
            var attr123 = feeConfiguration.FirstOrDefault(x => x.Attribute.Equals("123"));
            var attr127033 = feeConfiguration.FirstOrDefault(x => x.Attribute.Equals("127.033"));
            var acquiringInstIdCode =
                feeConfiguration.FirstOrDefault(x => x.AttributeName.Equals("AcquiringInstIdCode"));
            var role =
                feeConfiguration.FirstOrDefault(x => x.AttributeName.Equals("Role"));
            var msgTypeId =
                feeConfiguration.FirstOrDefault(x => x.AttributeName.Equals("MsgTypeId"));
            var correction =
                feeConfiguration.FirstOrDefault(x => x.AttributeName.Equals("Correction"));
            var extendedFields =
                feeConfiguration.FirstOrDefault(x => x.AttributeName.Equals("ExtendedFields"));
            var fulfillmentDocumentCode =
                feeConfiguration.FirstOrDefault(x => x.AttributeName.Equals("FulfillmentDocumentCode"));
            var masterCardFunctionCode =
                feeConfiguration.FirstOrDefault(x => x.AttributeName.Equals("MasterCardFunctionCode"));

            var adjustmentComponent = new MasterCard.AdjustmentComponent
            {
                AcquiringInstIdCode = acquiringInstIdCode != null ? acquiringInstIdCode.AttributeValue : @"16035",
                Role = role != null ? role.AttributeValue : @"1",
                MsgTypeId = msgTypeId != null ? msgTypeId.AttributeValue : string.Empty,
                Correction = correction != null ? correction.AttributeValue : @"0",
                ExtendedFields = extendedFields != null ? extendedFields.AttributeValue : string.Empty,
                FulfillmentDocumentCode = fulfillmentDocumentCode != null ? fulfillmentDocumentCode.AttributeValue : string.Empty,
                MasterCardFunctionCode = masterCardFunctionCode != null ? masterCardFunctionCode.AttributeValue : string.Empty
            };

            mockData = new MasterCard.MockData
            {
                AdjustmentComponent = adjustmentComponent
            };

            var mc4105 = new MC4105
            {
                _3 = attr003 != null ? attr003.AttributeValue : @"910000",
                _22 = attr022 != null ? attr022.AttributeValue.PadLeft(3, '0') : new string('0', 3),
                _25 = attr025 != null ? attr025.AttributeValue.PadLeft(2, '0') : new string('0', 2),
                _32 = attr032 != null ? attr032.AttributeValue : @"16035",
                _41 = attr041 != null ? attr041.AttributeValue : new string(' ', 8),
                _42 = attr042 != null ? attr042.AttributeValue : new string(' ', 15),
                _43 = attr043 != null ? attr043.AttributeValue : new string(' ', 40),
                _49 = attr049 != null ? attr049.AttributeValue.PadLeft(3, '0') : new string(' ', 3),
                _56 = attr056 != null ? attr056.AttributeValue.PadLeft(4, '0') : new string(' ', 4),
                _100 = attr100 != null ? attr100.AttributeValue : @"1",
                _123 = attr123 != null ? attr123.AttributeValue : @"000010000000200",
                _127_033 = attr127033 != null ? attr127033.AttributeValue : @"4105",
                _127_022 = mockData
            };

            return mc4105;
        }

        public VISA4105 LoadVisaFeeConfiguration(string bin, string countryCode, int feeTypeId,
            out Visa.MockData mockData)
        {
            var feeConfiguration = UnitOfWork.Repo<FeeConfigurationRepo>()
                .GetFeeConfiguration(Common.Enum.Organization.VISA, countryCode, feeTypeId);

            var attr003 = feeConfiguration.FirstOrDefault(x => x.Attribute.Equals("003"));
            var attr022 = feeConfiguration.FirstOrDefault(x => x.Attribute.Equals("022"));
            var attr025 = feeConfiguration.FirstOrDefault(x => x.Attribute.Equals("025"));
            var attr032 = feeConfiguration.FirstOrDefault(x => x.Attribute.Equals("032"));
            var attr041 = feeConfiguration.FirstOrDefault(x => x.Attribute.Equals("041"));
            var attr042 = feeConfiguration.FirstOrDefault(x => x.Attribute.Equals("042"));
            var attr043 = feeConfiguration.FirstOrDefault(x => x.Attribute.Equals("043"));
            var attr049 = feeConfiguration.FirstOrDefault(x => x.Attribute.Equals("049"));
            var attr056 = feeConfiguration.FirstOrDefault(x => x.Attribute.Equals("056"));
            var attr100 = feeConfiguration.FirstOrDefault(x => x.Attribute.Equals("100"));
            var attr123 = feeConfiguration.FirstOrDefault(x => x.Attribute.Equals("123"));
            var attr127033 = feeConfiguration.FirstOrDefault(x => x.Attribute.Equals("127.033"));
            var acquiringInstIdCode =
                feeConfiguration.FirstOrDefault(x => x.AttributeName.Equals("AcquiringInstIdCode"));
            var acquiringInstCountryCode =
                feeConfiguration.FirstOrDefault(x => x.AttributeName.Equals("AcquiringInstCountryCode"));
            var receivingInstCountryCode =
                feeConfiguration.FirstOrDefault(x => x.AttributeName.Equals("ReceivingInstCountryCode"));
            var role =
                feeConfiguration.FirstOrDefault(x => x.AttributeName.Equals("Role"));
            var msgTypeId =
                feeConfiguration.FirstOrDefault(x => x.AttributeName.Equals("MsgTypeId"));
            var correction =
                feeConfiguration.FirstOrDefault(x => x.AttributeName.Equals("Correction"));
            var extendedFields =
                feeConfiguration.FirstOrDefault(x => x.AttributeName.Equals("ExtendedFields"));

            var adjustmentComponent = new Visa.AdjustmentComponent
            {
                AcquiringInstIdCode = acquiringInstIdCode != null ? acquiringInstIdCode.AttributeValue : @"16035",
                AcquiringInstCountryCode = acquiringInstCountryCode != null ? acquiringInstCountryCode.AttributeValue : new string(' ', 3),
                ReceivingInstCountryCode = receivingInstCountryCode != null ? receivingInstCountryCode.AttributeValue : new string(' ', 3),
                Role = role != null ? role.AttributeValue : @"1",
                MsgTypeId = msgTypeId != null ? msgTypeId.AttributeValue : string.Empty,
                Correction = correction != null ? correction.AttributeValue : @"0",
                ExtendedFields = extendedFields != null ? extendedFields.AttributeValue : string.Empty,
            };

            mockData = new Visa.MockData
            {
                AdjustmentComponent = adjustmentComponent
            };

            var visa4105 = new VISA4105
            {
                _3 = attr003 != null ? attr003.AttributeValue : @"910000",
                _22 = attr022 != null ? attr022.AttributeValue.PadLeft(3, '0') : new string('0', 3),
                _25 = attr025 != null ? attr025.AttributeValue.PadLeft(2, '0') : new string('0', 2),
                _32 = attr032 != null ? attr032.AttributeValue : bin,
                _41 = attr041 != null ? attr041.AttributeValue : new string(' ', 8),
                _42 = attr042 != null ? attr042.AttributeValue : new string(' ', 15),
                _43 = attr043 != null ? attr043.AttributeValue : new string(' ', 40),
                _49 = attr049 != null ? attr049.AttributeValue.PadLeft(3, '0') : new string(' ', 3),
                _56 = attr056 != null ? attr056.AttributeValue.PadLeft(4, '0') : new string(' ', 4),
                _100 = attr100 != null ? attr100.AttributeValue : @"2",
                _123 = attr123 != null ? attr123.AttributeValue : @"000010000000200",
                _127_033 = attr127033 != null ? attr127033.AttributeValue : @"4105",
                _127_022 = mockData
            };

            return visa4105;
        }

        private void AddFeeCollectionExtract(I4105 i4105)
        {
            var lineWhiteBase64String = i4105.GetRecordWhiteBase64String();
            var line = i4105.GetRecord();
            var windowsIdentity = WindowsIdentity.GetCurrent();
            var feeCollectionExtract = new FeeCollectionExtract
            {
                CaseId = FeeCollection.CaseId,
                FeeCollectionId = FeeCollection.FeeCollectionId,
                PostilionExtractClearString = line,
                PostilionExtractWithBase64String = lineWhiteBase64String,
                ErrorFlag = false,
                Status = 0,
                InsertDate = DateTime.Now,
                InsertUser = windowsIdentity != null ? windowsIdentity.Name : "WindowsIdentity error."
            };
            UnitOfWork.Repo<FeeCollectionRepo>().AddFeeCollectionExtract(feeCollectionExtract);
            FeeCollection.Status = 2;
        }

        protected abstract I4105 Set4105(Complaint complaint, ComplaintValue complaintValue, ComplaintStage complaintStage, 
            ComplaintRecord complaintRecord);

        protected abstract I4105 Set4105(GoodFaithLetter gfl, GoodFaithLetterValue gflValue,
            GoodFaithLetterStage gflStage,
            GoodFaithLetterRecord gflRecord);

        protected abstract I4105 Set4105(RecoveryCard recoveryCard, RecoveryCardValue recoveryCardValue,
            RecoveryCardStage recoveryCardStage, RecoveryCardRecord recoveryCardRecord);
    }
}
