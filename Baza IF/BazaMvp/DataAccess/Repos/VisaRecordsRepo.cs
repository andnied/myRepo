using BazaMvp.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaMvp.DataAccess.Repos
{
    public class VisaRecordsRepo : RepositoryBase
    {
        public VisaRecordsRepo(DbContext context)
            : base(context)
        { }

        public void AddRecord(InputBase record)
        {
            base.Add((VisaRecord)record);
        }
    }
}
