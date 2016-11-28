using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Model.SearchParams;

namespace WebAPI.Contracts.DAL
{
    public interface IRepositoryBase
    {
    }

    public interface IRepositoryBase<T> where T : class, new()
    {
        IEnumerable<T> Get(BaseSearchParams searchParams);
        T Get(int id);
        T Add(T entity);
        T Update(int id, T entity);
        void Delete(int id);
    }
}
