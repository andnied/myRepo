using System;
using System.Diagnostics;
using System.IO;
using ComplaintTool.Common;
using ComplaintTool.Common.Extensions;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.MCProImageInterface.Outgoing.Case
{
    public abstract class CaseFilingRecordToXmlFileGenerator : IComplaintProcess
    {
        public static readonly ILogger Logger = LogManager.GetLogger();

        private readonly string _filePath;
        private readonly long _counter;
        protected readonly ComplaintUnitOfWork _unitOfWork = ComplaintUnitOfWork.Get();
        protected readonly CaseFilingRecord _record;
        protected readonly Complaint _complaint;
        protected readonly ComplaintStage _lastStage;
        public abstract string FileType { get; }
        public long Counter { get { return _counter; } }

        public CaseFilingRecordToXmlFileGenerator(CaseFilingRecord record, ComplaintStage lastStage, string tempFolderPath)
        {
            Guard.ThrowIf<ArgumentNullException>(record == null, "caseFilingRecord");
            Guard.ThrowIf<ArgumentNullException>(record.Complaint == null, "caseFilingRecord.Complaint");
            Guard.ThrowIf<ArgumentNullException>(lastStage == null, "lastComplaintStage");
            Guard.ThrowIf<ArgumentNullException>(tempFolderPath.IsEmpty(), "filePath");
            _record = record;
            _complaint = record.Complaint;
            _lastStage = lastStage;
            _filePath = Path.Combine(tempFolderPath, 
                string.Format("{0}.xml", 
                _unitOfWork.Repo<NameListRepo>().AddNewIdentifier(this.OrganizationId, this.FileType, out _counter)));
        }

        #region IComplaintProcess
        public string OrganizationId
        {
            get
            {
                return ComplaintTool.Common.Enum.Organization.MC.ToString();
            }
        }

        public string ProcessName
        {
            get
            {
                return Globals.MCProImageInterfaceProcessName;
            }
        }

        public string FilePath
        {
            get
            {
                return _filePath;
            }
        }

        public string ProcessFilePath
        {
            get
            {
                return _filePath;
            }
        }
        #endregion

        public abstract string Generate();

        public static CaseFilingRecordToXmlFileGenerator Create(string exportCode, CaseFilingRecord record, ComplaintStage lastStage, string tempFolderPath)
        {
            switch(exportCode)
            {
                case OutgoingFileType.FICN:
                    return new CaseFilingRecordToFICNGenerator(record, lastStage, tempFolderPath);
                case OutgoingFileType.FICU:
                    return new CaseFilingRecordToFICUGenerator(record, lastStage, tempFolderPath);
                case OutgoingFileType.FACU:
                    return new CaseFilingRecordToFACUGenerator(record, lastStage, tempFolderPath);
                default:
                    throw ComplaintCaseFilingExtractException.InvalidExportCode(exportCode);
            }
        }
    }
}
