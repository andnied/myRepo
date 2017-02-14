using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Common.Structures;
using WebAPI.Common.SearchParams;

namespace WebAPI.Contracts.DAL
{
    public interface IRepositoryBase
    {
    }

    public interface IRepositoryBase<T> 
        where T : class, new()
    {
        Task<ApiCollection> Get(BaseSearchParams searchParams);
        Task<T> Get(int id);
        Task<T> Add(T entity);
        Task<T> Update(int id, T entity);
        Task Delete(int id);
    }
}
