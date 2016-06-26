using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using Core.Common.Utils;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.Tests
{
    [TestClass]
    public class RepositoryTests
    {
        private ICarRepository _carRepo;

        [TestInitialize]
        public void Initialize()
        {
            var rentals = new List<Rental>
            {
                new Rental { DateReturned = null },
                new Rental { DateReturned = DateTime.Now },
                new Rental { DateReturned = DateTime.UtcNow },
                new Rental { DateReturned = DateTime.Now }
            };

            var cars = new List<Car>
            {
                new Car { CarId = 1, Description = "Porsche", Rentals = rentals.Take(2).ToList() },
                new Car { CarId = 2, Description = "Rented", Rentals = rentals.Skip(2).Take(2).ToList() }
            };

            var mockCars = new Mock<DbSet<Car>>();
            mockCars.As<IQueryable<Car>>().Setup(m => m.Provider).Returns(cars.AsQueryable().Provider);
            mockCars.As<IQueryable<Car>>().Setup(m => m.Expression).Returns(cars.AsQueryable().Expression);
            mockCars.As<IQueryable<Car>>().Setup(m => m.ElementType).Returns(cars.AsQueryable().ElementType);
            mockCars.As<IQueryable<Car>>().Setup(m => m.GetEnumerator()).Returns(cars.GetEnumerator());

            var mockRentals = new Mock<DbSet<Rental>>();
            mockRentals.As<IQueryable<Rental>>().Setup(m => m.Provider).Returns(rentals.AsQueryable().Provider);
            mockRentals.As<IQueryable<Rental>>().Setup(m => m.Expression).Returns(rentals.AsQueryable().Expression);
            mockRentals.As<IQueryable<Rental>>().Setup(m => m.ElementType).Returns(rentals.AsQueryable().ElementType);
            mockRentals.As<IQueryable<Rental>>().Setup(m => m.GetEnumerator()).Returns(rentals.GetEnumerator());

            var mockContext = new Mock<CarRentalContext>();
            mockContext.Setup(c => c.CarSet).Returns(mockCars.Object);
            mockContext.Setup(c => c.RentalSet).Returns(mockRentals.Object);

            var container = new UnityContainer();
            container.RegisterInstance(typeof(CarRentalContext), mockContext.Object);
            container.RegisterInstance(typeof(UnityContainer), container);
            container.RegisterType<ICarRepository, CarRepository>();
            container.RegisterType<IDataRepositoryFactory, DataRepositoryFactory>();

            var factory = container.Resolve<IDataRepositoryFactory>();
            _carRepo = factory.GetRepo<ICarRepository>();
        }

        [TestMethod]
        public void MyTestMethod()
        {
            var test = _carRepo.GetRentedCars();

            Assert.AreEqual(test.Count(), 1);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<NotFoundException>))]
        public void MyTestMethod2()
        {
            var test = _carRepo.Get(3);

            Guard.ThrowIf<NotFoundException>(test == null, "Car with id = {0} not found.", "3");
        }
    }
}
