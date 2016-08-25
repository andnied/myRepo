using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using CarRental.Client.Contracts.ServiceContracts;
using CarRental.Client.Proxies.ServiceProxies;
using Core.Common.Contracts;

namespace CarRental.Client.Proxies.Tests
{
    [TestClass]
    public class ProxyObtainmentTests
    {
        private IServiceFactory _factory;

        [TestInitialize]
        public void Initialize()
        {
            var container = new UnityContainer();
            container.RegisterInstance(container);
            container.RegisterType<IInventoryService, InventoryClient>();
            container.RegisterType<IRentalService, RentalClient>();
            container.RegisterType<IServiceFactory, ServiceFactory>();

            _factory = container.Resolve<IServiceFactory>();
        }

        [TestMethod]
        public void obtain_proxy_from_service_factory()
        {
            var proxy = _factory.GetService<IInventoryService>();

            Assert.IsTrue(proxy is InventoryClient);
        }
    }
}
