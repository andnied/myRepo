using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using ComplaintTool.Models;

namespace ComplaintTool.DataAccess.Repos
{
    public class NameListRepo : RepositoryBase
    {
        public NameListRepo(DbContext context)
            : base(context)
        {

        }

        public void AddNewIdInNameList(string filePath, string orgId, CaseFilingIncomingFile incomingFile)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            if (fileNameWithoutExtension != null)
            {
                var count = Convert.ToInt64(fileNameWithoutExtension.Substring(3, 10));
                var newNameList = new NameList
                {
                    FileName = Path.GetFileNameWithoutExtension(incomingFile.FileName),
                    OrganizationId = orgId,
                    Counter = count,
                    InsertUser = GetCurrentUser(),
                    InsertDate = GetCurrentDateTime()
                };
                GetDbSet<NameList>().Add(newNameList);
            }
            Commit();
        }

        public bool CheckFileId(string filePath)
        {
            var name = Path.GetFileNameWithoutExtension(filePath) ?? "0000000000";
            long counter = -1;
            try
            {
                counter = Convert.ToInt64(name.Substring(3, 10));
            }
            catch { }
            return GetDbSet<NameList>().FirstOrDefault(x => x.Counter == counter) != null;
        }

        public long GetNextCounter()
        {
            // TODO przydałoby się lock-a założyć ale na etapie refactoringu na razie zostawiam tak jak było...
            long counter = 1;
            var isExist = GetDbSet<NameList>().Any();
            if (!isExist) return counter;
            var tmpCounter = GetDbSet<NameList>().Max(x => x.Counter);
            tmpCounter++;
            counter = tmpCounter;
            return counter;
        }

        public string GetNewIdentifier(string prefix)
        {
            return GetNewIdentifier(prefix, GetNextCounter());
        }

        public string GetNewIdentifier(string prefix, long counter)
        {
            return prefix + counter.ToString().PadLeft(10, '0');
        }

        public string AddNewIdentifier(string orgId, string prefix, out long counter)
        {
            counter = GetNextCounter();
            var fileName = GetNewIdentifier(prefix, counter);

            var newNameList = new NameList
            {
                FileName = fileName,
                OrganizationId = orgId,
                Counter = counter,
                InsertUser = GetCurrentUser(),
                InsertDate = GetCurrentDateTime()
            };
            GetDbSet<NameList>().Add(newNameList);
            Commit();
            return fileName;
        }
    }
}
