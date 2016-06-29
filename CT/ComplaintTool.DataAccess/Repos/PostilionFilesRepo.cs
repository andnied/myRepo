using ComplaintTool.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.DataAccess.Repos
{
    public class PostilionFilesRepo : RepositoryBase
    {
        public PostilionFilesRepo(DbContext context)
            : base(context)
        {
        }

        public IEnumerable<ResponsePostilionFile> FindResponsePostilionFiles(string fileName)
        {
            return base.GetDbSet<ResponsePostilionFile>().Where(x => x.FileName == fileName && x.ErrorFlag == false && x.Status >= 2);
        }

        public IEnumerable<ResponsePostilionFile> FindResponseFilesForProcessing()
        {
            return base.GetDbSet<ResponsePostilionFile>().Where(
                x =>
                    x.ErrorFlag == false && 
                    x.Status == 0 && 
                    x.IsReceived == true && 
                    x.stream_id.HasValue)
                    .ToList();
        }

        public ResponsePostilionFile FindResponsePostilionFileById(long id)
        {
            return base.GetDbSet<ResponsePostilionFile>().FirstOrDefault(r => r.ResponsePostilionFileId == id);
        }

        public void Add(ResponsePostilionFile respPostFile)
        {
            base.GetDbSet<ResponsePostilionFile>().Add(respPostFile);
        }
    }
}
