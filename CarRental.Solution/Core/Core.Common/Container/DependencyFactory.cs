using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Container
{
    public static class DependencyFactory
    {
        private static readonly IUnityContainer _container;

        public static IUnityContainer Container
        {
            get
            {
                return _container;
            }
        }

        static DependencyFactory()
        {
            _container = new UnityContainer();
        }

        public static T Resolve<T>()
        {
            T ret = default(T);

            if (_container.IsRegistered(typeof(T)))
                ret = _container.Resolve<T>();

            return ret;
        }
    }
}
