using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using Core.Common.Extensions;

namespace CarRental.Data.Repositories
{
    public class CarRepository : DataRepositoryBase<Car>, ICarRepository
    {
        public CarRepository(CarRentalContext context)
            : base(context)
        { }

        protected override Car AddEntity(Car entity)
        {
            return _context.CarSet.Add(entity);
        }

        protected override Car UpdateEntity(Car entity)
        {
            return (from e in _context.CarSet
                    where e.CarId == entity.CarId
                    select e).FirstOrDefault();
        }

        protected override IEnumerable<Car> GetEntities()
        {
            return from e in _context.CarSet
                   select e;
        }

        protected override Car GetEntity(int id)
        {
            var query = (from e in _context.CarSet
                         where e.CarId == id
                         select e);

            var results = query.FirstOrDefault();

            return results;
        }

        public IEnumerable<Car> GetRentedCars()
        {
            var cars = _context.CarSet
                .Where(c => c.Rentals.Any(r => r.DateReturned == null));
            return cars.ToList();
        }
        
        public IEnumerable<Car> GetAvailableCars(DateTime pickupDate, DateTime returnDate)
        {
            var cars = _context.CarSet
                .Where(c => 
                    c.Rentals.Any(r => r.DateDue < pickupDate) && 
                    c.Reservations.Any(r => 
                        (r.RentalDate > pickupDate || r.RentalDate < pickupDate) &&
                        (r.ReturnDate > returnDate || r.ReturnDate < returnDate)));
            return cars.ToList();
        }
    }
}
