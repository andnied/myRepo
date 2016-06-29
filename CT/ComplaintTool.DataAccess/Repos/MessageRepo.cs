using ComplaintTool.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.DataAccess.Repos
{
    public class MessageRepo:RepositoryBase
    {
        public MessageRepo(DbContext dbContext) : base(dbContext) { }

        public MemberMessageText FindMemberMessageTextById(int messageId)
        {
            return GetDbSet<MemberMessageText>().FirstOrDefault(x => x.MessageId == messageId);
        }
    }
}
