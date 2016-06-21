using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
            var rentalsRented = new List<Rental>
            {
                new Rental { DateReturned = null },
                new Rental { DateReturned = DateTime.Now }
            };

            var rentalsNotRented = new List<Rental>
            {
                new Rental { DateReturned = DateTime.UtcNow },
                new Rental { DateReturned = DateTime.Now }
            };

            var cars = new List<Car>
            {
                new Car { CarId = 1, Description = "Porsche", Rentals = rentalsRented },
                new Car { CarId = 2, Description = "Rented", Rentals = rentalsNotRented }
            };

            var mockCars = new Mock<DbSet<Car>>();
            mockCars.As<IQueryable<Car>>().Setup(m => m.Provider).Returns(cars.AsQueryable().Provider);
            mockCars.As<IQueryable<Car>>().Setup(m => m.Expression).Returns(cars.AsQueryable().Expression);
            mockCars.As<IQueryable<Car>>().Setup(m => m.ElementType).Returns(cars.AsQueryable().ElementType);
            mockCars.As<IQueryable<Car>>().Setup(m => m.GetEnumerator()).Returns(cars.GetEnumerator());

            var mockContext = new Mock<CarRentalContext>();
            mockContext.Setup(c => c.CarSet).Returns(mockCars.Object);

            _carRepo = new CarRepository(mockContext.Object);
        }

        [TestMethod]
        public void MyTestMethod()
        {
            var test = _carRepo.GetRentedCars();

            Assert.AreEqual(test.Count(), 1);
        }
    }
}
