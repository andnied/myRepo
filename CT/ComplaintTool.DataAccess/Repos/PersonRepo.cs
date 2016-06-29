using System.Data.Entity;
using System.Linq;
using ComplaintTool.Models;

namespace ComplaintTool.DataAccess.Repos
{
    public class PersonRepo : RepositoryBase
    {
        public PersonRepo(DbContext context) : base(context)
        {
        }

        public Person GetPerson(long personId)
        {
            return GetDbSet<Person>().FirstOrDefault(x => x.PersonId == personId);
        }
    }
}
