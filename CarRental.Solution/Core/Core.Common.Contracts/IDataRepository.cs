using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Contracts
{
    public interface IDataRepository
    {
    }

    public interface IDataRepository<T> : IDataRepository, IDisposable
        where T : class, IIdentifiableEntity, new()
    {
        IEnumerable<T> Get();
        T Get(int id);
        T Add(T entity);
        T Update(T entity);
        void Remove(T entity);
        void Remove(int id);
    }
}
