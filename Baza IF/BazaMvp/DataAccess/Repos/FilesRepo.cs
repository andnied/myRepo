using BazaMvp.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaMvp.DataAccess.Repos
{
    public class FilesRepo : RepositoryBase
    {
        public FilesRepo(DbContext context)
            : base(context)
        { }

        public InputFile AddFile(InputFile file)
        {
            return GetDbSet<InputFile>().Add(file);
        }

        public void DeleteFile(long id)
        {
            var record = GetDbSet<InputFile>().Include(f => f.Records).FirstOrDefault(f => f.Id == id);
            GetDbSet<InputFile>().Remove(record);
        }

        public IEnumerable<InputFile> GetAllFiles()
        {
            return base.GetDbSet<InputFile>()
                .Include(f => f.Records)
                .ToList();
        }

        public bool FidExists(string fid)
        {
            return GetDbSet<InputFile>()
                .Include(f => f.Records)
                .Any(f => f.Records.Any(r => r.Fid == fid));
        }
    }
}
