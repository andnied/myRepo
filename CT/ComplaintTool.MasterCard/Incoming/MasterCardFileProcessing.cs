using ComplaintTool.Common;
using ComplaintTool.Common.Config;
using ComplaintTool.Common.CTLogger;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using eService.MCParser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using eService.MCParser.Parsers;

namespace ComplaintTool.MasterCard.Incoming
{
    public class MasterCardFileProcessing : IComplaintProcess// : MasterCardIncomingBase
    {
        #region Fields

        private static readonly ILogger _logger = LogManager.GetLogger();
        private readonly int _fileId;
        private readonly string _filePath;
        private readonly string _arn;
        private DateTime _started;
        private DateTime _finished;

        #endregion

        #region IComplaintProcess

        public string OrganizationId
        {
            get
            {
                return Common.Enum.Organization.MC.ToString();
            }
        }

        public string ProcessName
        {
            get
            {
                return Globals.MCIncomingInterfaceProcessName;
            }
        }

        public string ProcessFilePath
        {
            get
            {
                return _filePath;
            }
        }

        public string FilePath
        {
            get
            {
                return _filePath;
            }
        }

        #endregion

        #region Constructor

        public MasterCardFileProcessing(int fileId, string processingPath, string arn = null)
        {
            _fileId = fileId;

            if (!(Directory.Exists(processingPath)))
                Directory.CreateDirectory(processingPath);

            _filePath = Path.Combine(processingPath, Guid.NewGuid().ToString());
            _arn = arn;
        }

        #endregion

        #region MainProcess

        public int Process()
        {
            RegOrgIncomingFile regOrg;

            try
            {
                using (var unitOfWork = ComplaintUnitOfWork.Create())
                {
                    regOrg = unitOfWork.Repo<RegOrgIncomingFilesRepo>().FindRegOrgIncomingFile(_fileId);

                    if (regOrg == null)
                        throw new Exception(string.Format("Incoming file with id {0} not found.", _fileId));

                    regOrg.ParsingStarted = this._started = DateTime.UtcNow;
                    regOrg.ErrorFlag = true;

                    unitOfWork.Repo<RegOrgIncomingFilesRepo>().Update(regOrg);
                    unitOfWork.Commit();
                }

                using (var unitOfWork = ComplaintUnitOfWork.Create())
                {
                    unitOfWork.Repo<FileRepo>().GetIncomingRegOrgStream(regOrg.FileId, this.ProcessFilePath);
                    var partiallyParsedIncoming = this.ParseIncoming(regOrg.FileId, _arn);
                    var parsedIncoming = partiallyParsedIncoming.Select(m => SetProperties(m, unitOfWork)).ToList();
                    var result = unitOfWork.Repo<RegOrgIncomingFilesRepo>().InsertOrgIncomingTranMASTERCARD(parsedIncoming, regOrg.ProcessingMode ?? 0);

                    if (result == "1")
                    {
                        regOrg.ParsingStarted = this._started;
                        regOrg.ParsingFinished = this._finished = DateTime.UtcNow;
                        regOrg.ProcesingStart = this._finished;
                        regOrg.Status = 3;
                        regOrg.ErrorFlag = false;

                        unitOfWork.Repo<RegOrgIncomingFilesRepo>().Update(regOrg);
                        unitOfWork.Commit();

                        _logger.LogComplaintEvent(103, new object[] { "MasterCard", regOrg.Name, parsedIncoming.Count() });
                    }
                    else
                        throw new Exception(string.Format(ComplaintConfig.Instance.Notifications[518].MessageText,
                            new object[] { "MasterCard", regOrg.Name, result }));
                }

                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogComplaintException(ex);
                return -1;
            }
            finally
            {
                if (File.Exists(ProcessFilePath))
                    File.Delete(ProcessFilePath);
            }
        }

        #endregion

        #region PrivateMethods

        public static BlkModel SetProperties(BlkModel model, ComplaintUnitOfWork unitOfWork)
        {
            var field0105 = model.IncomingDate;
            if (field0105 != null)
            {
                model.BusinessDate = ComplaintTool.Common.Utils.Convert.JulianDateToDateTimeForVisa(field0105.Substring(3, 6));
                model.IncomingDate = field0105.Substring(3, 6);
            }

            model.StageDate = DateTime.UtcNow.ToString("yyyyMMdd");

            if (model.PAN != null)
                model.IssuerCountryCode = unitOfWork.Repo<ViewsRepo>().GetCountryAlpha(model.PAN);

            if (model.ARN != null)
            {
                var postilionRepo = unitOfWork.Repo<PostilionRepo>();
                model.SourceCountryCode = unitOfWork.Repo<BINListRepo>().GetSourceCountryCodeByBin(model.Bin);
                var postilionData = postilionRepo.GetPostilionData(model.ARN, model.PAN.Substring(0, 6), model.PAN.Substring(12, 4));

                if (postilionData != null)
                {
                    postilionData = SetNullProps(postilionData);

                    model.PostTranId = postilionData.PostID.ToString();
                    var sign = "N";
                    var posTerminalType = postilionData.PosTerminalType;
                    var tranType = postilionData.TranType;
                    var msgType = postilionData.MessageType;

                    if (posTerminalType.Equals("90")
							|| posTerminalType.Equals("91")
							|| posTerminalType.Equals("92")
							|| posTerminalType.Equals("93")
							|| posTerminalType.Equals("94")
							|| posTerminalType.Equals("95")
                            || posTerminalType.Equals("96"))
                    {
                        sign = "Y";
                    }

                    model.PosTerminalType = posTerminalType;
                    model.ECommerce = model.MPILogFlag = sign;
                    model.MessageType = msgType;

                    if (((tranType.Equals("00") || tranType.Equals("09")) && (!msgType
                            .Equals("0400") || !msgType.Equals("0420")))
                            || (tranType.Equals("20") && (msgType
                                    .Equals("0400") || msgType.Equals("0420"))))
                        model.TransactionAmountSign = "-";
                    if (((tranType.Equals("00") || tranType.Equals("09")) && (msgType
                            .Equals("0400") || msgType.Equals("0420")))
                            || (tranType.Equals("20") && (!msgType
                                    .Equals("0400") || !msgType.Equals("0420"))))
                        model.TransactionAmountSign = "+";

                    model.CVCFlag = postilionData.CardVerificationResult;
                    model.CardAcceptorIDCode = postilionData.CardAcceptorIDCode;
                    model.TransactionCurrencyCode = postilionData.CurrencyCode;
                    model.TransactionAmountExponent = postilionData.NrDecimals.ToString();
                    model.POSEntryMode = postilionData.POSEntryMode;
                    model.PosCardDataInputMode = postilionData.PosCardDataInputMode;
                    model.PosCardPresent = postilionData.PosCardPresent;
                    model.PosCardholderPresent = postilionData.PosCardholderPresent;
                    model.PosCardholderAuthMethod = postilionData.POSCardholderAuthMethod;
                    model.ExpiryDate = postilionData.ExpiryDate;
                    model.SettleAmountImpact = postilionData.SettleAmountImpact.ToString();
                    model.TranAmountReq = postilionData.TranAmountReq.ToString();
                    model.PrevPostTranId = postilionData.PrevPostTranId.ToString();
                    model.PosCardholderAuthEntity = postilionData.PosCardholderAuthEntity;
                    model.PosCardDataOutputAbility = postilionData.PosCardDataOutputAbility;
                    model.SystemTraceAuditNr = postilionData.SystemTraceAuditNr;
                    model.DatetimeTranLocal = postilionData.DatetimeTranLocal.ToString();
                    model.MerchantType = postilionData.MerchantType;
                    model.CardSeqNr = postilionData.CardSeqNr;
                    model.PosConditionCode = postilionData.PosConditionCode;
                    model.RetrievalReferenceNr = postilionData.RetrievalReferenceNr;
                    model.AuthIDRsp = postilionData.AuthIDRsp;
                    model.RspCodeRsp = postilionData.RspCodeRsp;
                    model.ServiceRestrictionCode = postilionData.ServiceRestrictionCode;
                    model.TerminalID = postilionData.TerminalID;
                    model.CardAcceptorNameLoc = postilionData.CardAcceptorNameLoc;
                    model.PosCardDataInputAbility = postilionData.PosCardDataInputAbility;
                    model.PosCardholderAuthAbility = postilionData.PosCardholderAuthAbility;
                    model.PosCardCaptureAbility = postilionData.PosCardCaptureAbility;
                    model.PosOperatingEnvironment = postilionData.PosOperatingEnvironment;
                    model.PosTerminalOutputAbility = postilionData.PosTerminalOutputAbility;
                    model.PosPinCaptureAbility = postilionData.PosPinCaptureAbility;
                    model.PosTerminalOperator = postilionData.PosTerminalOperator;
                    model.UcafData = postilionData.UcafData;
                    model.TranAmountRsp = postilionData.TranAmountRsp.ToString();
                    model.TranType = postilionData.TranType;
                    model.PrevMessageType = postilionData.PrevMessageType;
                    model.PrevUcafData = postilionData.PrevUcafData;
                    model.StructuredDataReq = postilionData.StructuredDataReq;
                    model.ParticipantId = postilionData.ParticipantId;
                    model.DatetimeRsp = Convert.ToDateTime(postilionData.DatetimeRsp);

                    if (model.StructuredDataReq != null)
                    {
                        model.Narritive = postilionData.GetStructuredDataValue("eS:GiccMerchantName") == " " ? null : postilionData.GetStructuredDataValue("eS:GiccMerchantName");
                        model.CVVFlag = postilionData.GetStructuredDataValue("eS:GiccCVV2P") == " " ? null : postilionData.GetStructuredDataValue("eS:GiccCVV2P");
                        model.GiccMCC = postilionData.GetStructuredDataValue("eS:GiccMCC") == " " ? null : postilionData.GetStructuredDataValue("eS:GiccMCC");
                        model.GiccDomesticMCC = postilionData.GetStructuredDataValue("eS:GiccDomesticMCC") == " " ? null : postilionData.GetStructuredDataValue("eS:GiccDomesticMCC");
                        model.GiccRevDate = postilionData.GetStructuredDataValue("eS:GiccRevDate") == " " ? null : postilionData.GetStructuredDataValue("eS:GiccRevDate");
                    }

                    var setAmtImpact = postilionData.SettleAmountImpact.GetValueOrDefault().ToString();
                    setAmtImpact = setAmtImpact != null && setAmtImpact.Contains("-") ? setAmtImpact.Substring(1) : setAmtImpact;

                    if (model.MessageType == "0400" || model.MessageType == "0420")
                    {
                        if (!string.IsNullOrWhiteSpace(model.GiccRevDate))
                        {
                            var day = model.GiccRevDate.Substring(2, 2);
                            var month = model.GiccRevDate.Substring(0, 2);
                            var year = PostilionData.getYY(month);
                            var hour = model.GiccRevDate.Substring(4, 2);
                            var minutes = model.GiccRevDate.Substring(6, 2);
                            var seconds = model.GiccRevDate.Substring(8, 2);
                            var tranDatetime = "20" + year + "-" + month + "-" + day + " " + hour + ":" + minutes + ":" + seconds + ":000";
                            model.TransactionDateTimeLocal = Convert.ToDateTime(tranDatetime).ToString("yyyy-MM-dd HH:mm:ss:fff");
                        }

                        if (!string.IsNullOrWhiteSpace(setAmtImpact) && !setAmtImpact.Equals("0"))
                            model.TransactionAmount = setAmtImpact;
                    }
                    else
                    {
                        model.TransactionDateTimeLocal = postilionData.DatetimeTranLocal.HasValue ? postilionData.DatetimeTranLocal.Value.ToString() : null;
                        model.TransactionAmount = postilionData.TranAmountReq.HasValue ? postilionData.TranAmountReq.Value.ToString() : null;
                    }

                    if ((model.TransactionAmount == null || model.TransactionAmount == "0") && model.PrevPostTranId != null)
                        model.TransactionAmount = unitOfWork.Repo<ViewsRepo>().GetPreviousTransactionAmount(long.Parse(model.PrevPostTranId));
                }
            }
            else
            {
                model.MPILogFlag = "N";
                model.ECommerce = "N";
            }

            if (model.TransactionAmountExponent != null)
            {
                var amtLen = model.TransactionAmount.Length;
                var exp = int.Parse(model.TransactionAmountExponent);

                if (amtLen < exp)
                    model.TransactionAmount = model.TransactionAmount.PadLeft(exp - amtLen + model.TransactionAmount.Length, '0');
            }

            return model;
        }

        private static PostilionData SetNullProps(PostilionData data)
        {
            foreach (var prop in data.GetType().GetProperties())
                if (prop.PropertyType == typeof(string) && prop.GetValue(data).ToString() == "")
                    prop.SetValue(data, null);

            return data;
        }

        private List<BlkModel> ParseIncoming(int fileId, string arn = null)
        {
            var parser = new Blk2ipmAsc();
            var res = parser.ParseToModel(ProcessFilePath, fileId).Where(m => m.IsComplaint);
            return (arn == null ? res : res.Where(m => m.ARN == arn)).ToList();
        }

        #endregion
    }
}
