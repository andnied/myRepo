using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;
using CarRental.Business.Contracts;

namespace CarRental.ServiceHost.Tests
{
    [TestClass]
    public class ServiceAccessTests
    {
        [TestMethod]
        public void InventoryServicePingTest()
        {
            using (var channelFactory = new ChannelFactory<IInventoryService>(""))
            {
                var proxy = channelFactory.CreateChannel();
                
                (proxy as ICommunicationObject).Open();
                channelFactory.Close();
            }
        }

        [TestMethod]
        public void RentalServicePingTest()
        {
            using (var factory = new ChannelFactory<IRentalService>(""))
            {
                var proxy = factory.CreateChannel();

                (proxy as ICommunicationObject).Open();
                factory.Close();
            }
        }

        //[TestMethod]
        //public void MyTestMethod()
        //{
        //    using (var factory = new ChannelFactory<IInventoryService>(""))
        //    {
        //        var proxy = factory.CreateChannel();
        //        var car = proxy.GetCar(0);
        //    }
        //}
    }
}
