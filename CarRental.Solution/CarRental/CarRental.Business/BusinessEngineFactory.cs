using Core.Common.Contracts;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Business
{
    public class BusinessEngineFactory : IBusinessEngineFactory
    {
        private readonly IUnityContainer _container;

        public BusinessEngineFactory(IUnityContainer container)
        {
            _container = container;
        }

        public T GetEngine<T>() where T : IBusinessEngine
        {
            return _container.Resolve<T>();
        }
    }
}
