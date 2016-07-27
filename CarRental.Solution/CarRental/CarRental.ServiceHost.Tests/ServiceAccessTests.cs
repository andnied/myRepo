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
        public void TestInventoryService()
        {
            var channelFactory = new ChannelFactory<IInventoryService>("");
            var proxy = channelFactory.CreateChannel();

            (proxy as ICommunicationObject).Open();
            channelFactory.Close();
        }
    }
}
