using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarRental.Business.Entities;
using Moq;
using Core.Common.Contracts;
using CarRental.Data.Contracts;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using CarRental.Business.Common;
using CarRental.Business.Business_Engines;

namespace CarRental.Business.Tests
{
    [TestClass]
    public class CarRentalEngineTests
    {
        private ICarRentalEngine _engine;

        [TestInitialize]
        public void Initialize()
        {
            var rental = new Rental { CarId = 1 };
            var mockRepo = new Mock<IRentalRepository>();
            mockRepo.Setup(r => r.GetCurrentRentalByCar(1)).Returns(rental);

            var mockRepoFactory = new Mock<IDataRepositoryFactory>();
            mockRepoFactory.Setup(f => f.GetRepo<IRentalRepository>()).Returns(mockRepo.Object);

            var container = new UnityContainer();
            container.RegisterInstance(mockRepo.Object);
            container.RegisterInstance(container);
            container.RegisterInstance(mockRepoFactory.Object);

            _engine = container.Resolve<CarRentalEngine>();
        }

        [TestMethod]
        public void IsCarCurrentlyRentedFor1ReturnsTrue()
        {
            Assert.IsTrue(_engine.IsCarCurrentlyRented(1));
        }

        [TestMethod]
        public void IsCarCurrentlyRentedFor2ReturnsFalse()
        {
            Assert.IsFalse(_engine.IsCarCurrentlyRented(2));
        }
    }
}
