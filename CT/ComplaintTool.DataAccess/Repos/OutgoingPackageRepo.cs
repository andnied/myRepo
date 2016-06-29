using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using ComplaintTool.Common.Properties;
using ComplaintTool.Models;

namespace ComplaintTool.DataAccess.Repos
{
    public class OutgoingPackageRepo : RepositoryBase
    {
        public OutgoingPackageRepo(DbContext context) 
            : base(context)
        {
        }

        public OutgoingPackage FindBulkByProcessKey(Guid processKey)
        {
            return GetDbSet<OutgoingPackage>().FirstOrDefault(x => x.ProcessKey == processKey);
        }

        public OutgoingPackage FindByFileName(string fileName)
        {
            return GetDbSet<OutgoingPackage>().FirstOrDefault(x => x.PackageFileName == fileName);
        }

        public IEnumerable<OutgoingPackageItem> FindAllByEndpoint(string endpointBin, string orgId)
        {
            return GetDbSet<OutgoingPackageItem>()
                .Where(x => x.BIN == endpointBin && x.OrganizationId == orgId && x.Status == 0).ToList();
        }

        public IEnumerable<OutgoingPackageItem> FindAllBulkItemsByProcessKey(Guid processKey)
        {
            return GetDbSet<OutgoingPackageItem>().Where(x => x.ProcessKey == processKey).ToList();
        }

        public void ProcessOutgoingPackageItem(OutgoingPackageItem item, string tempFolder, Guid processKey)
        {
            var packageItem = GetDbSet<OutgoingPackageItem>().FirstOrDefault(x => x.ItemId == item.ItemId);
            if (packageItem != null)
                packageItem.Status = 1;

            var fileStream = new ObjectParameter("file_Stream", typeof(byte[]));
            // proc. skład.
            GetContext<ComplaintEntities>().GetOutgoingFile(item.stream_id, fileStream);

            var itemFilePath = string.Format(@"{0}\{1}", tempFolder, item.FileName);
            if (fileStream.Value != DBNull.Value)
            {
                if (File.Exists(itemFilePath))
                    File.Delete(itemFilePath);
                File.WriteAllBytes(itemFilePath, (byte[])fileStream.Value);
            }

            if (!File.Exists(itemFilePath))
                throw new Exception(string.Format(Resources._5005, item.FileName));

            if (packageItem != null)
            {
                packageItem.Status = 2;
                packageItem.ProcessKey = processKey;
            }
            Commit();
        }

        public OutgoingPackage ProcessOutgoingPackage(Guid processKey, Guid zipStreamId, string zipPath, string orgId, string bulkFileName, List<OutgoingPackageItem> items)
        {
            var currentDate = GetCurrentDateTime();
            var fileStream = File.ReadAllBytes(zipPath);
            var outgoingName = DateTime.UtcNow.ToString("yyyyMMddhhmmssfff") + "_" + Path.GetFileName(zipPath);
            GetContext<ComplaintEntities>().AddOutgoingFile(zipStreamId, fileStream, outgoingName);

            var zipInfo = new FileInfo(zipPath);
            var newOutgoingPackage = new OutgoingPackage
            {
                OrganizationId = orgId,
                PackageFileName = bulkFileName,
                stream_id = zipStreamId,
                CreateDate = zipInfo.CreationTimeUtc,
                Status = 2,
                ProcessKey = processKey,
                InsertDate = currentDate,
                InsertUser = GetCurrentUser()
            };
            GetDbSet<OutgoingPackage>().Add(newOutgoingPackage);
            var packageId = newOutgoingPackage.OutgoingPackageId;

            foreach (var item in items)
            {
                var outgoingPackageItem = GetDbSet<OutgoingPackageItem>().FirstOrDefault(x => x.ItemId == item.ItemId);
                if (outgoingPackageItem != null)
                    outgoingPackageItem.OutgoingPackageId = packageId;

                var outgoingFile = GetDbSet<CaseFilingOutgoingFile>().FirstOrDefault(x => x.stream_id == item.stream_id);
                if (outgoingFile == null) continue;
                outgoingFile.ProcessKey = processKey;
                var msg = string.Format(Resources._1001, outgoingFile.FileName, bulkFileName);
                var newComplaintNote = new ComplaintNote
                {
                    CaseId = outgoingFile.CaseId,
                    Note = msg,
                    InsertDate = currentDate,
                    InsertUser = GetCurrentUser()
                };
                GetDbSet<ComplaintNote>().Add(newComplaintNote);
            }
            Commit();
            return newOutgoingPackage;
        }
    }
}
