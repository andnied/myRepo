using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using Core.Common.Extensions;

namespace CarRental.Data
{
    public class CarRepository : DataRepositoryBase<Car>, ICarRepository
    {
        public CarRepository()
            : base()
        {

        }

        public CarRepository(DbContext context)
            : base(context)
        {

        }

        protected override Car AddEntity(CarRentalContext entityContext, Car entity)
        {
            return entityContext.CarSet.Add(entity);
        }

        protected override Car UpdateEntity(CarRentalContext entityContext, Car entity)
        {
            return (from e in entityContext.CarSet
                    where e.CarId == entity.CarId
                    select e).FirstOrDefault();
        }

        protected override IEnumerable<Car> GetEntities(CarRentalContext entityContext)
        {
            return from e in entityContext.CarSet
                   select e;
        }

        protected override Car GetEntity(CarRentalContext entityContext, int id)
        {
            var query = (from e in entityContext.CarSet
                         where e.CarId == id
                         select e);

            var results = query.FirstOrDefault();

            return results;
        }

        public IEnumerable<Car> GetRentedCars()
        {
            using (var context = (CarRentalContext)_injectedContext ?? new CarRentalContext())
            {
                //var cars = context.RentalSet.Include(r => r.Car).Where(r => r.DateReturned == null).Select(r => r.Car.FirstOrDefault());
                var cars = context.CarSet.Where(c => c.Rentals.Any(r => r.DateReturned == null));
                return cars.ToList();
            }
        }
    }
}
