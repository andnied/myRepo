using System;
using System.IO;
using ComplaintTool.Common;
using ComplaintTool.Models;

namespace ComplaintTool.MCProImageInterface
{
    [Serializable]
    public class ComplaintCaseFilingMatchException : ComplaintInvalidOperationException
    {
        public ComplaintCaseFilingMatchException() { }
        public ComplaintCaseFilingMatchException(string message) : base(message) { }
        public ComplaintCaseFilingMatchException(string message, Exception inner) : base(message, inner) { }
        protected ComplaintCaseFilingMatchException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        internal static ComplaintCaseFilingMatchException EmptyARN(string fileType)
        {
            return new ComplaintCaseFilingMatchException(string.Format("ARN in incoming file {0} is empty!", fileType));
        }

        internal static ComplaintCaseFilingMatchException CaseNotFound(string arn, string fileType)
        {
            return new ComplaintCaseFilingMatchException(string.Format("Invalid incoming file {0}. Case with ARN {1} not found!", fileType, arn));
        }

        internal static ComplaintCaseFilingMatchException StageNotFound(string caseId, string fileType)
        {
            return new ComplaintCaseFilingMatchException(string.Format("CaseFiling stage for CaseId: {0} and file {1} not found!", caseId, fileType));
        }

        internal static Exception MasterCardCaseIdNotExists(string caseId)
        {
            return new ComplaintCaseFilingMatchException(string.Format("MasterCardCaseId: {0} not exist in database!", caseId));
        }
    }

    [Serializable]
    public class ComplaintCaseFilingExtractException : ComplaintInvalidOperationException
    {
        public ComplaintCaseFilingExtractException() { }
        public ComplaintCaseFilingExtractException(string message) : base(message) { }
        public ComplaintCaseFilingExtractException(string message, Exception inner) : base(message, inner) { }
        protected ComplaintCaseFilingExtractException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        internal static ComplaintCaseFilingExtractException CaseFilingRecordNotFound(string caseId, long stageId)
        {
            return new ComplaintCaseFilingExtractException(string.Format("Case Filing Record for CaseId: {0} and StageId: {1} not found!", caseId, stageId));
        }

        internal static ComplaintCaseFilingExtractException StageNotFound(string caseId)
        {
            return new ComplaintCaseFilingExtractException(string.Format("CaseFiling stage for CaseId: {0} not found!", caseId));
        }

        internal static ComplaintCaseFilingExtractException InvalidExportCode(string exportCode)
        {
            return new ComplaintCaseFilingExtractException(string.Format("Export code: {0} is invalid!", exportCode));
        }

        internal static ComplaintCaseFilingExtractException InvalidStageCode(string stageCode)
        {
            return new ComplaintCaseFilingExtractException(string.Format("Stage code: {0} is invalid!", stageCode));
        }

        internal static ComplaintCaseFilingExtractException XmlNotGenerated(string exportCode, string caseId)
        {
            return new ComplaintCaseFilingExtractException(string.Format("Xml {0} not generated for case: {1}!", exportCode, caseId));
        }

        internal static ComplaintCaseFilingExtractException TiffNotGenerated(string tiffPath, long representmentId)
        {
            return new ComplaintCaseFilingExtractException(string.Format("Tiff {0} not generated for representment: {1}!", Path.GetFileName(tiffPath), representmentId));
        }

        internal static ComplaintCaseFilingExtractException InvalidRepresentmentPostilionStatus(RepresentmentExtract repExtract)
        {
            return new ComplaintCaseFilingExtractException(string.Format("RepresentmentExtract: {0} has invalid postilion status: {1}", repExtract.RepresentmentExtractId, repExtract.PostilionStatus));
        }
    }

    [Serializable]
    public class ComplaintCaseFilingIncomingException : ComplaintInvalidOperationException
    {
        public ComplaintCaseFilingIncomingException() { }
        public ComplaintCaseFilingIncomingException(string message) : base(message) { }
        public ComplaintCaseFilingIncomingException(string message, Exception inner) : base(message, inner) { }
        protected ComplaintCaseFilingIncomingException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        internal static ComplaintCaseFilingIncomingException UnsupportedIncomingFile(string filePath, Exception inner)
        {
            string message = null;
            try
            {
                if (ImageProXmlUtil.IsXml(filePath))
                    message = string.Format("Incoming file {0} is not supported by ComplaintTool!", Path.GetFileName(filePath));
                else
                    message = string.Format("Incoming file {0} is not valid xml file!", Path.GetFileName(filePath));
            }
            catch 
            {
                message = "Incoming file is invalid!";
            }
            return new ComplaintCaseFilingIncomingException(message, inner);
        }

        internal static ComplaintCaseFilingIncomingException UnsupportedIncomingFile(string filePath)
        {
            return new ComplaintCaseFilingIncomingException(string.Format("Incoming file {0} is not supported by ComplaintTool!", Path.GetFileName(filePath)));
        }

        internal static ComplaintCaseFilingIncomingException UnsupportedAuditFile(string filePath)
        {
            return new ComplaintCaseFilingIncomingException(string.Format("Incoming file {0} is not valid audit file!", Path.GetFileName(filePath)));
        }
    }

    [Serializable]
    public class ComplaintCaseFilingUpdateException : ComplaintInvalidOperationException
    {
        public ComplaintCaseFilingUpdateException() { }
        public ComplaintCaseFilingUpdateException(string message) : base(message) { }
        public ComplaintCaseFilingUpdateException(string message, Exception inner) : base(message, inner) { }
        protected ComplaintCaseFilingUpdateException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        internal static ComplaintCaseFilingUpdateException InvalidMissingDateValues(string fileType)
        {
            return new ComplaintCaseFilingUpdateException(string.Format("CaseFiling {0} file doesn't have any required date value!", fileType));
        }

        internal static ComplaintCaseFilingUpdateException InvalidTooManyDateValues(string fileType)
        {
            return new ComplaintCaseFilingUpdateException(string.Format("CaseFiling {0} file have too many required date value!", fileType));
        }
    }
    [Serializable]
    public class ComplaintBulkException : ComplaintInvalidOperationException
    {
        public ComplaintBulkException() { }
        public ComplaintBulkException(string message) : base(message) { }
        public ComplaintBulkException(string message, Exception inner) : base(message, inner) { }
        protected ComplaintBulkException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        internal static ComplaintBulkException DuplicateEndpointFile(string endpointName)
        {
            return new ComplaintBulkException(string.Format("There is more than one file for endpoint {0}.", endpointName));
        }

        internal static ComplaintBulkException BulkNotFound(Guid processKey)
        {
            return new ComplaintBulkException(string.Format("Cannot find bulk file with ProcessKey {0}", processKey));
        }

        internal static ComplaintBulkException BulkItemsNotFound(Guid processKey)
        {
            return new ComplaintBulkException(string.Format("Cannot find records in table [File].OutgoingPackageItems with ProcessKey {0}", processKey));
        }

        internal static Exception BulkItemCorrupted(string fileItem)
        {
            return new ComplaintBulkException(string.Format("Item {0} of bulk file is corrupted - cannot create file by stream_id.", fileItem));
        }
    }
}
