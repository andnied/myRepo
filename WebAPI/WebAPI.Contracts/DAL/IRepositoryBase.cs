using System.Collections.Generic;
using WebAPI.Common.Structures;
using WebAPI.Model.SearchParams;

namespace WebAPI.Contracts.DAL
{
    public interface IRepositoryBase
    {
    }

    public interface IRepositoryBase<T, T2> 
        where T : class, new()
        where T2 : class
    {
        T2 Get(BaseSearchParams searchParams);
        T Get(int id);
        T Add(T entity);
        T Update(int id, T entity);
        void Delete(int id);
    }
}
