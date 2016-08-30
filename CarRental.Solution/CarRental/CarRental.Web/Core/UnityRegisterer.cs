using CarRental.Data;
using Core.Common.Container;
using Core.Common.Contracts;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRental.Web.Core
{
    public static class UnityRegisterer
    {
        public static void RegisterDependencies()
        {
            var container = DependencyFactory.Container;

            container.RegisterInstance(container);
            container.RegisterType<IDataRepositoryFactory, DataRepositoryFactory>();
        }
    }
}