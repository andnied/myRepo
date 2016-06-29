using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ComplaintTool.Common;
using ComplaintTool.Common.Config;
using ComplaintTool.Common.Enum;
using ComplaintTool.Common.Extensions;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.MCProImageInterface.Model;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.MCProImageInterface
{
    public class OutgoingPackageService : IComplaintProcess
    {
        public static readonly ILogger Logger = LogManager.GetLogger();
        private readonly DateTime _processStart = DateTime.Now;
        private readonly Guid _processKey;
        private readonly string _extractPath;  

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
                return Globals.MCProImageInterfaceProcessName;
            }
        }

        public string ProcessFilePath
        {
            get
            {
                return _extractPath;
            }
        }

        public string FilePath
        {
            get
            {
                return _extractPath;
            }
        }

        #endregion

        #region Constructors

        public OutgoingPackageService(string extractPath)
            : this(Guid.NewGuid(), extractPath)
        {
        }

        public OutgoingPackageService(Guid processKey, string extractPath)
        {
            Guard.ThrowIf<ArgumentNullException>(processKey.IsEmpty(), "processKey");
            Guard.ThrowIf<ArgumentNullException>(extractPath.IsEmpty(), "extractPath");
            _processKey = processKey;
            _extractPath = extractPath;
        } 

        #endregion

        #region NewBulkExtract

        public string NewBulkExtract(string endpointName)
        {
            Guard.ThrowIf<ArgumentNullException>(endpointName.IsEmpty(), "endpointName");

            var bulkFileName = string.Empty;
            var tempFolder = string.Empty;
            var zipStreamId = Guid.NewGuid();

            var mcEndpointDefinitions = ComplaintConfig.Instance
                    .EndPointDefinitions.Values
                    .Where(x => x.OrganizationId == this.OrganizationId && x.EndPointName == endpointName)
                    .ToList();

            using (var unitOfWork = ComplaintUnitOfWork.Create())
            {
                var extractItems = new List<OutgoingPackageItemInfo>();
                foreach (var endpoint in mcEndpointDefinitions)
                {
                    tempFolder = GetTempFolder(endpoint.EndPointDirectoryName);
                    var packageItems = unitOfWork.Repo<OutgoingPackageRepo>().FindAllByEndpoint(endpoint.BIN, this.OrganizationId);
                    if (!packageItems.Any()) continue;

                    var itemsToExtract = new List<OutgoingPackageItem>();
                    foreach (var item in packageItems)
                    {
                        unitOfWork.Repo<OutgoingPackageRepo>().ProcessOutgoingPackageItem(item, tempFolder, _processKey);
                        itemsToExtract.Add(item);
                    }
                    var newItem = new OutgoingPackageItemInfo
                    {
                        DirectoryPath = tempFolder,
                        DirectoryName = endpoint.EndPointDirectoryName,
                        EndpointName = endpoint.EndPointName,
                        Items = itemsToExtract,
                        ProcessKey = _processKey
                    };
                    extractItems.Add(newItem);
                }

                if (extractItems.Any())
                {
                    if (extractItems.Count != 1)
                        throw ComplaintBulkException.DuplicateEndpointFile(endpointName);

                    var itemToExtract = extractItems.First();
                    tempFolder = GetTempFolder(itemToExtract.DirectoryName);
                    bulkFileName = GetBulkFileName(itemToExtract.EndpointName);
                    var zipFileList = GetZipFileList(tempFolder);
                    var zipPath = SaveZipFile(bulkFileName, zipFileList);

                    unitOfWork.Repo<OutgoingPackageRepo>().ProcessOutgoingPackage(_processKey,
                        zipStreamId,
                        zipPath,
                        OrganizationId,
                        bulkFileName,
                        itemToExtract.Items);
                    unitOfWork.Repo<ProcessRepo>().AddProcessKey(_processKey, bulkFileName, ProcessingStatus.Administration);

                    Logger.LogComplaintEvent(134, bulkFileName, string.Join(", ", zipFileList.Select(x => Path.GetFileName(x))));
                    //Logger.LogComplaintEvent(143, bulkFileName);
                }

                unitOfWork.Commit();
                return bulkFileName;
            }
        } 

        #endregion

        #region GetBulkExtract

        public string GetBulkExtract()
        {
            using (var unitOfWork = ComplaintUnitOfWork.Create())
            {
                var bulk = unitOfWork.Repo<OutgoingPackageRepo>().FindBulkByProcessKey(_processKey);
                if (bulk == null)
                    throw ComplaintBulkException.BulkNotFound(_processKey);

                var items = unitOfWork.Repo<OutgoingPackageRepo>().FindAllBulkItemsByProcessKey(_processKey);
                if (!items.Any())
                    throw ComplaintBulkException.BulkItemsNotFound(_processKey);

                string bulkFileName = bulk.PackageFileName;
                var extractionFolder = GetBulkExtractionFolder(bulkFileName);
                if (!Directory.Exists(extractionFolder))
                    Directory.CreateDirectory(extractionFolder);

                foreach (var item in items)
                {
                    var fileStream = unitOfWork.Repo<FileRepo>().GetOutgoingFile(item.stream_id);
                    var itemFilePath = Path.Combine(extractionFolder, item.FileName);
                    if (fileStream != null)
                    {
                        if (File.Exists(itemFilePath))
                            File.Delete(itemFilePath);
                        File.WriteAllBytes(itemFilePath, fileStream);
                    }

                    if (!File.Exists(itemFilePath))
                        throw ComplaintBulkException.BulkItemCorrupted(item.FileName);
                }

                SaveZipFile(bulkFileName, GetZipFileList(extractionFolder));
                unitOfWork.Commit();
                return bulkFileName;
            }
        } 

        #endregion

        #region Helpers

        private string GetTempFolder(string directoryName)
        {
            string temp = FileUtil.GetFolderTemp(_extractPath);
            string tempFolder = directoryName != null
                ? string.Format("{0}{1}_{2:yyyyMMdd}", temp, directoryName, _processStart)
                : temp;
            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);
            return tempFolder;
        }

        private string GetBulkFileName(string endpointName)
        {
            return string.Format("{0}_{1:yyyyMMdd}_{2:hhmmss}.zip", endpointName, _processStart, DateTime.Now);
        }

        private string GetBulkExtractionFolder(string bulkFileName)
        {
            return Path.Combine(FileUtil.GetFolderTemp(_extractPath), bulkFileName.Substring(0, bulkFileName.Length - 4));
        }

        private IEnumerable<string> GetZipFileList(string folderPath)
        {
            return Directory.GetFiles(folderPath, "*.xml")
                .Union(Directory.GetFiles(folderPath, "*.XML"))
                .Union(Directory.GetFiles(folderPath, "*.tif"))
                .Union(Directory.GetFiles(folderPath, "*.TIF"));
        }

        private string SaveZipFile(string bulkFileName, IEnumerable<string> zipFileList)
        {
            var zipPath = Path.Combine(_extractPath, bulkFileName);
            Extract.CreateZip(zipPath, zipFileList.ToArray());
            foreach (var file in zipFileList)
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
            return zipPath;
        } 

        #endregion
    }
}
