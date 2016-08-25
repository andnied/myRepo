using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarRental.Client.Proxies.ServiceProxies;

namespace CarRental.Client.Proxies.Tests
{
    [TestClass]
    public class ServiceAccessTests
    {
        [TestMethod]
        public void test_inventory_client_connection()
        {
            var proxy = new InventoryClient();
            
            proxy.Open();
        }
    }
}
