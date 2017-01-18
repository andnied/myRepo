using System.Collections.Generic;
using WebAPI.Common.Structures;
using WebAPI.Model.SearchParams;

namespace WebAPI.Contracts.DAL
{
    public interface IRepositoryBase
    {
    }

    public interface IRepositoryBase<T> 
        where T : class, new()
    {
        ApiCollection<T> Get(BaseSearchParams searchParams);
        T Get(int id);
        T Add(T entity);
        T Update(int id, T entity);
        void Delete(int id);
    }
}
