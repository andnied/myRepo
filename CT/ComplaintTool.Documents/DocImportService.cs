using ComplaintTool.Common;
using ComplaintTool.Common.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common.Extensions;
using ComplaintTool.Documents.DocParsers;
using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Repos;
using System.Globalization;
using ComplaintTool.Documents.OrganizationServices;
using ComplaintTool.Common.Enum;
using ComplaintTool.Models;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Documents
{
    public abstract class DocImportService:IComplaintProcess
    {
        private string _filePath;
        private IDocParser _docParser;
        private static readonly ILogger Logger = LogManager.GetLogger();
        private List<ParserKey> _parserKeys;

        public abstract string OrganizationId {get;}
        public abstract string ProcessName {get;}
        public abstract FileSource FileSource{ get; }

        public string ProcessFilePath { get; private set; }

        public string FilePath { get; private set; }

        public void LoadKeysForParam(string paramName)
        {
            using (var unitOfWork = ComplaintUnitOfWork.Create())
            {
                _parserKeys = unitOfWork.Repo<ParserKeyRepo>().GetKeys(paramName, OrganizationId).ToList();
            }
        }

        public void Process(string filePath)
        {
            FilePath = filePath;
            ProcessFilePath = Path.Combine(Path.GetDirectoryName(this.FilePath), Globals.TempFolderName, Path.GetFileName(this.FilePath));

            if (!ProcessUtil.MoveFileForProcessing(this))
                throw new Exception("Cannot copy file to temporary directory");

            var docParser = GetParser(ProcessFilePath, OrganizationId);
            string arn = docParser.GetParamValue(_parserKeys);

            if (string.IsNullOrEmpty(arn))
                throw new Exception("Cannot find ARN in document");

            SaveDoc(arn);
            File.Delete(ProcessFilePath);
            Logger.LogComplaintEvent(101, ProcessName, Path.GetFileName(ProcessFilePath));
        }

        private void SaveDoc(string arn)
        {
            string caseId = null;
            Guid streamId;

            using (var unitOfWork = ComplaintUnitOfWork.Create())
            {
                var complaint=unitOfWork.Repo<ComplaintRepo>().FindByARN(arn);
                caseId =complaint!=null? complaint.CaseId:null;
                if (caseId == null)
                    throw new Exception(string.Format("CaseId {0} not exists in Complaint Repo",caseId));

                streamId=unitOfWork.Repo<FileRepo>().AddDocumentFile(ProcessFilePath);
                unitOfWork.Repo<FileRepo>().AddDocumentToComplaint(Path.GetFileName(ProcessFilePath), caseId, streamId, FileSource);
                unitOfWork.Commit();
            }
        }

        public static DocImportService GetService(string organizationId)
        {
            if (organizationId.Equals(Common.Enum.Organization.VISA.ToString()))
                return new VisaDocImportService();

            if (organizationId.Equals(Common.Enum.Organization.MC.ToString()))
                return new McDocImportService();

            return null;
        }

        private IDocParser GetParser(string filePath,string organizationId)
        {
            return new PdfDocParser(filePath,OrganizationId);
        }
    }
}
