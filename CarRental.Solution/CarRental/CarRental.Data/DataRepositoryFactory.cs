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

        public DataRepositoryFactory(IUnityContainer container)
        {
            _container = container;
        }

        public T GetRepo<T>() where T : IDataRepository
        {
            return _container.Resolve<T>();
        }
    }
}
