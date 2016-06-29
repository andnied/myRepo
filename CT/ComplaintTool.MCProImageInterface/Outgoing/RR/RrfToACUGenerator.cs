using System;
using System.IO;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.MCProImageInterface.Model;

namespace ComplaintTool.MCProImageInterface.Outgoing.RR
{
    public class RrfToACUGenerator : DocumentGenerator
    {
        private readonly Complaint _complaint;

        public RrfToACUGenerator(ComplaintUnitOfWork unitOfWork, string tempFolderPath, string tifFilePath, Complaint complaint)
            : base(unitOfWork, tempFolderPath, tifFilePath)
        {
            Guard.ThrowIf<ArgumentNullException>(complaint == null, "complaint");
            _complaint = complaint;
        }

        public override string FileType { get { return OutgoingFileType.ACU; } }

        public override string Generate()
        {
            var incomingTran = _unitOfWork.Repo<IncomingTranRepo>().FindTranMASTERCARDByCaseId(_complaint.CaseId);
            if (incomingTran == null) return null;

            var lastStage = _unitOfWork.Repo<ComplaintRepo>().FindLastStageByCaseId(incomingTran.CaseId);
            var lastRecord = _unitOfWork.Repo<ComplaintRepo>().FindRecordByStageId(lastStage.StageId);
            
            string pan, panExtention;
            _unitOfWork.Repo<ComplaintRepo>().GetPanByCaseId(incomingTran.CaseId, out pan, out panExtention);

            var memo = lastRecord != null ?
                (string.IsNullOrEmpty(lastRecord.MemoText) ? lastRecord.MemoText : string.Empty) : string.Empty;
            var arc = lastRecord != null ?
                (string.IsNullOrEmpty(lastRecord.ResponseCode) ? lastRecord.ResponseCode : string.Empty) : string.Empty;

            var acu = new Acu
            {
                Mti = incomingTran.MTI,
                Pan = pan,
                FunctionCode = incomingTran.FunctionCode,
                ARD = new AcquirerReferenceData()
                {
                    S1MixedUse = incomingTran.ARN.Substring(0, 1),
                    S2AcquirersBIN = incomingTran.ARN.Substring(1, 6),
                    S3JulianProcessingDate = incomingTran.ARN.Substring(7, 4),
                    S4AcquirersSequenceNumber = incomingTran.ARN.Substring(11, 11),
                    S5CheckDigitNumeric = incomingTran.ARN.Substring(22, 1)
                },
                AdditionalData = new AdditionalData()
                {
                    MasterComSenderMemo = memo,
                    MasterComFulfillmentDocumentCode = arc,
                    MasterComImageMetadata = new MasterComImageMetadata()
                    {
                        S1MasterComTIFFImageDataLengthHex = (new FileInfo(TifFilePath)).Length.ToString("X8"),
                        S2MasterComTIFFImageDataOffsetHex = "00000000",
                        S3MasterComTIFFImageCRC32 = CRC32Util.GenerateHashForTif(TifFilePath),
                        S4MasterComTIFFImageFilename = Path.GetFileNameWithoutExtension(ProcessFilePath) + ".tif"
                    }
                }

            };

            return XmlUtil.SerializeToFile(acu, ProcessFilePath).ToImageProEncoding();
        }
    }
}
