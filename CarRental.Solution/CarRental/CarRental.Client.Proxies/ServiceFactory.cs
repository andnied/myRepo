using Core.Common.Contracts;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Client.Proxies
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly IUnityContainer _container;

        public ServiceFactory(IUnityContainer container)
        {
            _container = container;
        }

        public T GetService<T>() where T : IServiceContract
        {
            return _container.Resolve<T>();
        }
    }
}
