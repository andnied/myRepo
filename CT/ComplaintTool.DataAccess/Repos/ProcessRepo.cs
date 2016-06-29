using System;
using System.Data.Entity;
using ComplaintTool.Common.Enum;
using ComplaintTool.Models;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace ComplaintTool.DataAccess.Repos
{
    public class ProcessRepo : RepositoryBase
    {
        public ProcessRepo(DbContext context) 
            : base(context)
        {
        }

        public void AddProcessKey(Guid processKeyGuid, string fileName, ProcessingStatus status, string description = "")
        {
            var currDate = GetCurrentDateTime();
            var processKey = new ProcessKey
            {
                ProcessKey1 = processKeyGuid,
                ProcessDate = currDate,
                Source = status.ToString(),
                FileName = fileName,
                Description = description,
                InsertDate = currDate,
                InsertUser = GetCurrentUser()
            };
            GetDbSet<ProcessKey>().Add(processKey);
            Commit();
        }

        public List<ProcessKey> GetProcessingKeys(int number)
        {
            return GetDbSet<ProcessKey>().OrderByDescending(x => x.ProcessDate).Take(number).ToList();
        }
    }
}
