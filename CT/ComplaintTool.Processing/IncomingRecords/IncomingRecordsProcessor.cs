using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.Models;
using ComplaintTool.Visa.Incoming;
using eService.MCParser.Parsers;

namespace ComplaintTool.Processing.IncomingRecords
{
    public class IncomingRecordsProcessor
    {
        public string FileName { get; set; }
        public string ResultDirectoryPath { get; set; }

        private readonly ComplaintUnitOfWork _unitOfWork;

        public IncomingRecordsProcessor(string fileName, string resultPath)
        {
            FileName = fileName;
            ResultDirectoryPath = resultPath;
            _unitOfWork = ComplaintUnitOfWork.Create(false);
        }

        public string Process()
        {
            var regOrgIncomingFile = _unitOfWork.Repo<RegOrgIncomingFilesRepo>().GetRegOrgIncomingFile(FileName);
            if (regOrgIncomingFile == null)
                throw new Exception(string.Format("Cannot find file {0} in database", FileName));

            string resultFilePath;
            switch (regOrgIncomingFile.OrganizationId)
            {
                case "VISA":
                    resultFilePath = VerifyVisaFile(regOrgIncomingFile.FileId);
                    break;
                case "MC":
                    resultFilePath = VerifyMcFile(regOrgIncomingFile);
                    break;
                default:
                    throw new Exception(string.Format("Unrecognized organization: {0}", regOrgIncomingFile.OrganizationId));
            }

            if (!File.Exists(resultFilePath))
                throw new Exception(string.Format("Cannot find result file in path '{0}'", resultFilePath));

            return Path.GetFileName(resultFilePath);
        }

        private string VerifyVisaFile(int fileId)
        {
            var lines = _unitOfWork.Repo<FileRepo>().GetIncomingRegOrgFile(fileId);
            var visaIncomingProcessor = new VisaIncomingProcessing(0);
            var records = visaIncomingProcessor.ProcessFile(null, lines);
            var arnList = new Dictionary<string,int>();
            foreach (var record in records)
            {
                if (arnList.ContainsKey(record.ARN)) continue;
                var count = records.Count(x => x.ARN.Equals(record.ARN));
                arnList.Add(record.ARN, count);
            }
            return SaveResultFile(arnList);
        }

        private string VerifyMcFile(RegOrgIncomingFile regOrgIncomingFile)
        {
            if (regOrgIncomingFile.Name.EndsWith(".ipm"))
                throw new Exception("IPM files are not supported");

            var filePath = Path.Combine(ResultDirectoryPath, regOrgIncomingFile.Name);

            _unitOfWork.Repo<FileRepo>().GetIncomingRegOrgStream(regOrgIncomingFile.FileId, filePath);

            var parser = new Blk2ipmAsc();
            var partiallyParsedBulk = parser.ParseToModel(filePath, 0);
            var parsedIncoming = partiallyParsedBulk.Select(m => MasterCard.Incoming.MasterCardFileProcessing.SetProperties(m, _unitOfWork)).ToList();
            var arnList = new Dictionary<string, int>();
            foreach (var record in parsedIncoming)
            {
                if (arnList.ContainsKey(record.ARN)) continue;
                var count = parsedIncoming.Count(x => x.ARN.Equals(record.ARN));
                arnList.Add(record.ARN, count);
            }
            return SaveResultFile(arnList);
        }

        private string SaveResultFile(Dictionary<string, int> arnList)
        {
            var resultFilePath = Path.Combine(ResultDirectoryPath, string.Format("{0}_ARNList.csv", FileName));
            var streamWriter = new StreamWriter(resultFilePath, true);
            foreach (var arn in arnList) 
                streamWriter.WriteLine("{0};{1}", arn.Key, arn.Value);
            streamWriter.Close();
            return resultFilePath;
        }
    }
}
