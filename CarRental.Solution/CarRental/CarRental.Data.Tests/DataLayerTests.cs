using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarRental.Data.Contracts;
using Microsoft.Practices.Unity;
using CarRental.Data.Tests.TestClasses;
using Moq;
using CarRental.Business.Entities;
using System.Collections.Generic;
using Core.Common.Contracts;

namespace CarRental.Data.Tests
{
    [TestClass]
    public class DataLayerTests
    {
        private RepositoryTestClass _carRepoTestClass;

        [TestInitialize]
        public void Initialize()
        {
            var cars = new List<Car>
            {
                new Car { Color = "test1" },
                new Car { Color = "test2" }
            };

            var mock = new Mock<IDataRepositoryFactory>();
            mock.Setup(f => f.GetRepo<ICarRepository>().Get()).Returns(cars);

            var container = new UnityContainer();
            container.RegisterInstance(typeof(IUnityContainer), container);
            container.RegisterInstance(typeof(IDataRepositoryFactory), mock.Object);

            _carRepoTestClass = container.Resolve<RepositoryTestClass>();
        }

        [TestMethod]
        public void MyTestMethod()
        {
            var test = _carRepoTestClass.GetCars();

            Assert.AreEqual(test.Count(), 2);
        }
    }
}
