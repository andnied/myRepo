using Core.Common.Utils;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRental.Web.Core
{
    public class UnityDependencyResolver : IDependencyResolver, IDisposable
    {
        private readonly IUnityContainer _container;

        public UnityDependencyResolver(IUnityContainer container)
        {
            Guard.ThrowIf<ArgumentNullException>(container == null, "container");

            _container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}