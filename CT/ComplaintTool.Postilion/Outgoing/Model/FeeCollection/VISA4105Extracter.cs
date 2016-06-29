using System;
using System.Globalization;
using System.Linq;
using ComplaintTool.Common.Config;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.DataAccess.Utils;
using ComplaintTool.Postilion.Outgoing.Model.FeeCollection.Visa;
using Organization = ComplaintTool.Common.Enum.Organization;

namespace ComplaintTool.Postilion.Outgoing.Model.FeeCollection
{
    public class VISA4105Extracter : Organization4105Extracter
    {
        public VISA4105Extracter(ComplaintUnitOfWork unitOfWork, Models.FeeCollection feeCollection) : base(unitOfWork, feeCollection)
        {
        }

        protected override I4105 Set4105(Complaint complaint, ComplaintValue complaintValue, ComplaintStage complaintStage,
            ComplaintRecord complaintRecord)
        {
            if (FeeCollection == null)
                throw new Exception("FeeCollection.SetVISA4105 Error. FeeCollection is null");
            if (!complaintValue.BookingAmount.HasValue)
                throw new Exception(string.Format("FeeCollection.SetVISA4105 Error. CaseId = {0} ComplaintValues.BookingAmount is null. ValueId = {1}",
                    FeeCollection.CaseId, FeeCollection.ValueId));

            var participantId = complaint.ParticipantId ?? UnitOfWork.Repo<BINListRepo>().GetParticipantId(complaint.BIN);
            if (string.IsNullOrWhiteSpace(participantId))
                throw new Exception(string.Format("FeeCollection.SetVISA4105 Error. CaseId = {0} PARTICIPANT_ID is null. BIN = {1}",
                    complaint.CaseId, complaint.BIN));

            var helper = new Helper();
            var currencyCodes = ComplaintDictionaires.Instance.GetCurrencyCode();

            string pan;
            string panExtention;
            UnitOfWork.Repo<ComplaintRepo>().GetPanByCaseId(complaint.CaseId, out pan, out panExtention);

            var bookingCurrencyCode = short.Parse(complaintValue.BookingCurrencyCode);
            var currencyCode = currencyCodes[bookingCurrencyCode];

            var memberMessageText = Utils.RegexReplace(complaintStage.MemberMessageText);
            memberMessageText = memberMessageText.Length > 70
                ? memberMessageText.Substring(0, 70)
                : memberMessageText;

            var feeCollectionControlNr = Utils.GetFeeCollectionControlNr(Organization.VISA,
                FeeCollection.FeeCollectionId);

            var adjustmentComponent = new AdjustmentComponent
            {
                FeeCollectionControlNr = feeCollectionControlNr,
                MessageText = memberMessageText,
                DestinationInstitutionId = complaintRecord.DestinationInstitutionId,
                AcquiringInstIdCode = complaint.BIN,
                ReceivingInstIdCode = complaint.PANMask.Substring(0, 6),
                MsgId = complaint.PostTranId.HasValue ? complaint.PostTranId.Value.ToString(CultureInfo.InvariantCulture) : null,
                MsgTypeId = FeeCollection.IsReversal.HasValue && FeeCollection.IsReversal.Value ? "15" : complaintRecord.MsgTypeId,
                ExtendedFields = string.Format("PARTICIPANT_ID:{0};", participantId)
            };

            var mockData = new MockData()
            {
                CaseID = complaint.CaseId,
                AdjustmentComponent = adjustmentComponent
            };

            int exponent;
            var visa4105 = new VISA4105
            {
                _2 = pan,
                _7 = FeeCollection.InsertDate.ToString(),
                _12 = FeeCollection.InsertDate.ToString(),
                _4 = helper.AmountToString(currencyCode, complaintValue.BookingAmount.Value, 12, out exponent),
                _49 = complaintValue.BookingCurrencyCode,
                _56 = complaintStage.ReasonCode,
                _22 = complaintRecord.POSEntryMode,
                _25 = complaintRecord.PosConditionCode,
                _37 = complaintRecord.RetrievalReferenceNr,
                _39 = complaintRecord.RspCodeRsp,
                _127_022 = mockData,
                _127_033 = FeeCollection.IsReversal.HasValue && FeeCollection.IsReversal.Value ? "4112"
                    : complaintRecord.MsgTypeId == "5" ? "4105" : "4106"
            };
            return visa4105;
        }

        protected override I4105 Set4105(GoodFaithLetter gfl, GoodFaithLetterValue gflValue, GoodFaithLetterStage gflStage,
            GoodFaithLetterRecord gflRecord)
        {
            if (FeeCollection == null)
                throw new Exception("FeeCollection.SetVISA4105 Error. FeeCollection is null");
            if (!gflValue.FeeAmount.HasValue)
                throw new Exception(string.Format("FeeCollection.SetVISA4105 Error. CaseId = {0} GoodFaithLetterValue.FeeAmount is null. ValueId = {1}",
                    FeeCollection.CaseId, FeeCollection.ValueId));

            if (!gflRecord.FeeTypeId.HasValue)
                throw new Exception(string.Format(
                    "FeeCollection.SetVISA4105 Error. FeeCollectionId = {0} CaseId = {1} Cannot find FeeTypeId value in GoodFaithLetterRecord with RecordId = {2}",
                    FeeCollection.FeeCollectionId, FeeCollection.CaseId, gflRecord.RecordId));

            var helper = new Helper();
            var currencyCodes = ComplaintDictionaires.Instance.GetCurrencyCode();
            var feeAmountSum = gflValue.FeeAmount.Value;

            string pan;
            string panExtention;
            UnitOfWork.Repo<ComplaintRepo>().GetPanByCaseId(gfl.CaseId, out pan, out panExtention);

            var currencyCode = currencyCodes.Values.FirstOrDefault(x => x.Alphabetical == gflValue.FeeCurrencyCode);
            if (currencyCode == null) throw new Exception("Cannot find currency code with Alphabetical = " + gflValue.FeeCurrencyCode);

            var memberMessageText = string.Format("Accept GFL {0} {1} {2}", gflValue.FeeAmount, gflValue.FeeCurrencyCode, gfl.PANNameCardHolder);
            memberMessageText = Common.Utils.Convert.ReplacePolishChars(memberMessageText);

            var feeCollectionControlNr = Utils.GetFeeCollectionControlNr(Organization.VISA, FeeCollection.FeeCollectionId);

            MockData mockData;
            var visa4105 = LoadVisaFeeConfiguration(gfl.BIN, gfl.CountryCode, gflRecord.FeeTypeId.Value, out mockData);

            mockData.AdjustmentComponent.FeeCollectionControlNr = feeCollectionControlNr;
            mockData.AdjustmentComponent.MessageText = memberMessageText;
            mockData.AdjustmentComponent.DestinationInstitutionId = gflRecord.DestinationInstitutionId;
            //mockData.AdjustmentComponent.AcquiringInstIdCode = gfl.BIN;
            mockData.AdjustmentComponent.ReceivingInstIdCode = gfl.PANMask.Substring(0, 6);
            mockData.CaseID = gfl.CaseId;

            int exponent;
            visa4105._2 = pan;
            visa4105._7 = FeeCollection.InsertDate.ToString();
            visa4105._12 = FeeCollection.InsertDate.ToString();
            visa4105._4 = helper.AmountToString(currencyCode, feeAmountSum, 12, out exponent);
            visa4105._49 = currencyCode.Numeric.ToString();
            visa4105._127_022 = mockData;

            return visa4105;
        }

        protected override I4105 Set4105(RecoveryCard recoveryCard, RecoveryCardValue recoveryCardValue, RecoveryCardStage recoveryCardStage,
            RecoveryCardRecord recoveryCardRecord)
        {
            if (FeeCollection == null)
                throw new Exception("FeeCollection.SetVISA4105 Error. FeeCollection is null");
            if (!recoveryCardValue.FeeAmount.HasValue)
                throw new Exception(string.Format("FeeCollection.SetVISA4105 Error. CaseId = {0} RecoveryCardValue.FeeAmount is null. ValueId = {1}",
                    FeeCollection.CaseId, FeeCollection.ValueId));

            if (!recoveryCardRecord.FeeTypeId.HasValue)
                throw new Exception(string.Format(
                    "FeeCollection.SetVISA4105 Error. FeeCollectionId = {0} CaseId = {1} Cannot find FeeTypeId value in RecoveryCardRecord with RecordId = {2}",
                    FeeCollection.FeeCollectionId, FeeCollection.CaseId, recoveryCardRecord.RecordId));

            var helper = new Helper();
            var currencyCodes = ComplaintDictionaires.Instance.GetCurrencyCode();

            var feeAmountSum = recoveryCardValue.FeeAmount.Value + (recoveryCardValue.PrizeAmount ?? 0);
            
            Person person = null;
            var recoveryCardList = UnitOfWork.Repo<RecoveryCardRepo>().GetRecoveryCardList(recoveryCard.CaseId);
            if (recoveryCardList != null)
                person = UnitOfWork.Repo<PersonRepo>().GetPerson(recoveryCardList.PersonId);

            string pan;
            string panExtention;
            UnitOfWork.Repo<ComplaintRepo>().GetPanByCaseId(recoveryCard.CaseId, out pan, out panExtention);

            var bookingCurrencyCode = short.Parse(recoveryCardValue.FeeCurrencyCode);
            var currencyCode = currencyCodes[bookingCurrencyCode];

            var memberMessageText = string.Format("RECOVERED CARD;{0} {1} {2} {3} ", feeAmountSum, recoveryCardValue.FeeCurrencyCode,
                person != null ? person.FirstName : string.Empty, person != null ? person.LastName : string.Empty);
            memberMessageText = Common.Utils.Convert.ReplacePolishChars(memberMessageText);

            var feeCollectionControlNr = Utils.GetFeeCollectionControlNr(Organization.VISA, FeeCollection.FeeCollectionId);

            MockData mockData;
            var visa4105 = LoadVisaFeeConfiguration(recoveryCard.BIN, recoveryCard.CountryCode, recoveryCardRecord.FeeTypeId.Value, out mockData);

            mockData.AdjustmentComponent.FeeCollectionControlNr = feeCollectionControlNr;
            mockData.AdjustmentComponent.MessageText = memberMessageText;
            mockData.AdjustmentComponent.DestinationInstitutionId = recoveryCardRecord.DestinationInstitutionId;
            mockData.AdjustmentComponent.ReceivingInstIdCode = recoveryCard.PANMask.Substring(0, 6);
            mockData.CaseID = recoveryCard.CaseId;

            int exponent;
            visa4105._2 = pan;
            visa4105._7 = FeeCollection.InsertDate.ToString();
            visa4105._12 = FeeCollection.InsertDate.ToString();
            visa4105._4 = helper.AmountToString(currencyCode, feeAmountSum, 12, out exponent);
            visa4105._49 = recoveryCardValue.FeeCurrencyCode;
            visa4105._127_022 = mockData;

            return visa4105;
        }
    }
}