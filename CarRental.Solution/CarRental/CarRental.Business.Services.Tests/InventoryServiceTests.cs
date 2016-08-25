using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarRental.Business.Contracts.ServiceContracts;
using CarRental.Business.Entities;
using System.Collections.Generic;
using Moq;
using CarRental.Data.Contracts;
using Core.Common.Contracts;
using Microsoft.Practices.Unity;
using CarRental.Business.Services;
using System.Linq;
using System.ServiceModel;
using Core.Common.Exceptions;
using System.Security.Principal;
using System.Threading;

namespace CarRental.Business.Services.Tests
{
    [TestClass]
    public class InventoryServiceTests
    {
        private IInventoryService _inventoryService;
        private IList<Car> _carList = new List<Car>();

        [TestInitialize]
        public void Initialize()
        {
            var principal = new GenericPrincipal(new GenericIdentity("TestIdentity"), new string[] { "AppAdmins" });
            Thread.CurrentPrincipal = principal;

            var repoMock = new Mock<ICarRepository>();
            repoMock.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(i => _carList.FirstOrDefault(c => c.CarId == i));
            repoMock.Setup(r => r.Add(It.IsAny<Car>())).Callback<Car>(car => _carList.Add(car));
            repoMock.Setup(r => r.Update(It.IsAny<Car>())).Callback<Car>(car => _carList[_carList.IndexOf(_carList.FirstOrDefault(c => c.CarId == car.CarId))] = car);

            var factoryMock = new Mock<IDataRepositoryFactory>();
            factoryMock.Setup(f => f.GetRepo<ICarRepository>()).Returns(repoMock.Object);

            var container = new UnityContainer();
            container.RegisterInstance(factoryMock.Object);
            container.RegisterInstance(container);
            container.RegisterType<IBusinessEngineFactory>(new InjectionFactory(c => null));

            _inventoryService = container.Resolve<InventoryService>();
        }

        [TestMethod]
        public void Update_car_add_new()
        {
            var newCar = new Car { CarId = 0, Description = "Ferrari" };
            Car car = null;

            try
            {
                car = _inventoryService.GetCar(0);
            }
            catch (FaultException)
            {  }

            Assert.IsNull(car);

            _inventoryService.UpdateCar(newCar);

            Assert.IsNotNull(_inventoryService.GetCar(0));
        }

        [TestMethod]
        public void update_car_updates_car()
        {
            _carList.Add(new Car { CarId = 1, Description = "Lanos" });
            _inventoryService.GetCar(1);

            var newCar = new Car { CarId = 1, Description = "Nubira" };

            _inventoryService.UpdateCar(newCar);

            var car = _inventoryService.GetCar(1);

            Assert.AreEqual(car.Description, "Nubira");
        }
    }
}
