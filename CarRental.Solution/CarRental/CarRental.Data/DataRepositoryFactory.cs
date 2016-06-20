using Core.Common.Contracts;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data
{
    public class DataRepositoryFactory : IDataRepositoryFactory
    {
        private readonly IUnityContainer _container;
        //private IDictionary<string, IDataRepository> _repos = new Dictionary<string, IDataRepository>();

        public DataRepositoryFactory(IUnityContainer container)
        {
            _container = container;
        }

        public T GetRepo<T>() where T : IDataRepository
        {
            //var typeName = typeof(T).FullName;

            //if (_repos.ContainsKey(typeName))
            //    return (T)_repos[typeName];

            //var repo = (T)Activator.CreateInstance(typeof(T));
            //_repos.Add(typeName, repo);
            //return repo;

            return _container.Resolve<T>();
        }
    }
}
