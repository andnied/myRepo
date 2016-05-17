using BazaMvp.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaMvp.DataAccess.Repos
{
    public class McRecordsRepo : RepositoryBase
    {
        public McRecordsRepo(DbContext context)
            : base(context)
        { }

        public void AddRecord(InputBase record)
        {
            base.Add((MasterCardRecord)record);
        }

        public IEnumerable<MasterCardRecord> GetAllMcFiles()
        {
            return base.GetDbSet<MasterCardRecord>().ToList();
        }
    }
}
