using System;
using System.IO;
using System.Linq;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.MCProImageInterface.Model;

namespace ComplaintTool.MCProImageInterface.Outgoing.Chargeback
{
    public class RepresentmentToSCUGenerator : DocumentGenerator
    {
        private readonly Representment _representment;

        public override string FileType
        {
            get { return OutgoingFileType.SCU; }
        }

        public RepresentmentToSCUGenerator(ComplaintUnitOfWork unitOfWork, string tempFolderPath, string tifFilePath, Representment rep)
            : base(unitOfWork, tempFolderPath, tifFilePath)
        {
            Guard.ThrowIf<ArgumentNullException>(rep == null, "representment");
            _representment = rep;
        }

        public override string Generate()
        {
            var incomingTran = _unitOfWork.Repo<IncomingTranRepo>().FindTranMASTERCARDByCaseId(_representment.CaseId);
            if (incomingTran == null) return null;

            // sprawdza czy status Postiliona dla danej reprezentacji ma wartość '00' 
            var repExtract = _unitOfWork.Repo<RepresentmentRepo>().FindRepresentmentExtract(_representment);
            if (repExtract == null || repExtract.PostilionStatus != "00")
                throw ComplaintCaseFilingExtractException.InvalidRepresentmentPostilionStatus(repExtract);

            //var lastStage = _unitOfWork.Repo<ComplaintRepo>().FindLastStageByCaseId(incomingTran.CaseId);
            //var lastRecord = _unitOfWork.Repo<ComplaintRepo>().FindRecordByStageId(lastStage.StageId);

            string pan, panExtention;
            _unitOfWork.Repo<ComplaintRepo>().GetPanByCaseId(incomingTran.CaseId, out pan, out panExtention);

            //var memo = lastRecord != null ?
            //    (string.IsNullOrEmpty(lastRecord.MemoText) ? lastRecord.MemoText : string.Empty) : string.Empty;
            //var arc = lastRecord != null ?
            //    (string.IsNullOrEmpty(lastRecord.ResponseCode) ? lastRecord.ResponseCode : string.Empty) : string.Empty;

            
            var postilionRepo = _unitOfWork.Repo<PostilionRepo>();
            var data = postilionRepo.GetPostilionData(incomingTran);

            if (data == null)
            {
                foreach (var repDoc in _representment.RepresentmentDocuments.ToList())
                    repDoc.Status = 0;

                Logger.LogComplaintEvent(539, incomingTran.ARN);
                FileUtil.CleanFolder(TempFolderPath);
                return null;
            }

            var scu = new Scu()
            {
                Mti = "1240",//incomingTran.MTI,
                Pan = pan,
                PoS = new PointOfServiceDataCode()
                {
                    S1TerminalDataCardDataInputCapability = data.PosCardDataInputAbility,
                    S2TerminalDataCardholderAuthenticationCapability =
                        data.PosCardholderAuthAbility,
                    S3TerminalDataCardCaptureCapability = data.PosCardCaptureAbility,
                    S4TerminalOperatingEnvironment = data.PosOperatingEnvironment,
                    S5CardholderPresentData = data.PosCardholderPresent,
                    S6CardPresentData = data.PosCardPresent,
                    S7CardDataInputMode = data.PosCardDataInputMode,
                    S8CardholderAuthenticationMethod = data.POSCardholderAuthMethod,
                    S9CardholderAuthenticationEntity = data.PosCardholderAuthEntity,
                    S10CardDataOutputCapability = data.PosCardDataOutputAbility,
                    S11TerminalDataOutputCapability = data.PosTerminalOutputAbility,
                    S12PINCaptureCapability = data.PosPinCaptureAbility
                },
                FunctionCode = "205",//incomingTran.FunctionCode,
                ARD = new AcquirerReferenceData()
                {
                    S1MixedUse = incomingTran.ARN.Substring(0, 1),
                    S2AcquirersBIN = incomingTran.ARN.Substring(1, 6),
                    S3JulianProcessingDate = incomingTran.ARN.Substring(7, 4),
                    S4AcquirersSequenceNumber = incomingTran.ARN.Substring(11, 11),
                    S5CheckDigitNumeric = incomingTran.ARN.Substring(22, 1)
                },
                CardIssuerReferenceData = incomingTran.KKOCbReference,
                AdditionalData = new AdditionalData()
                {
                    MasterComSenderMemo = "Case " + incomingTran.CaseId,
                    MasterComImageMetadata = new MasterComImageMetadata()
                    {
                        S1MasterComTIFFImageDataLengthHex = (new FileInfo(TifFilePath)).Length.ToString("X8"),
                        S2MasterComTIFFImageDataOffsetHex = "00000000",
                        S3MasterComTIFFImageCRC32 = CRC32Util.GenerateHashForTif(TifFilePath),
                        S4MasterComTIFFImageFilename = Path.GetFileNameWithoutExtension(ProcessFilePath) + ".tif"
                    }
                }
            };

            return XmlUtil.SerializeToFile(scu, ProcessFilePath).ToImageProEncoding();
        }
    }
}
