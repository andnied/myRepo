using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using Core.Common.Extensions;
using CarRental.Data.Contracts.DTOs;

namespace CarRental.Data.Repositories
{
    public class RentalRepository : DataRepositoryBase<Rental>, IRentalRepository
    {
        protected RentalRepository(CarRentalContext context)
            : base(context)
        { }

        protected override Rental AddEntity(CarRentalContext entityContext, Rental entity)
        {
            return entityContext.RentalSet.Add(entity);
        }

        protected override Rental UpdateEntity(CarRentalContext entityContext, Rental entity)
        {
            return (from e in entityContext.RentalSet
                    where e.RentalId == entity.RentalId
                    select e).FirstOrDefault();
        }

        protected override IEnumerable<Rental> GetEntities(CarRentalContext entityContext)
        {
            return from e in entityContext.RentalSet
                   select e;
        }

        protected override Rental GetEntity(CarRentalContext entityContext, int id)
        {
            var query = (from e in entityContext.RentalSet
                         where e.RentalId == id
                         select e);

            var results = query.FirstOrDefault();

            return results;
        }

        public IEnumerable<Rental> GetRentalHistoryByCar(int carId)
        {
            var query = from e in _context.RentalSet
                        where e.CarId == carId
                        select e;

            return query.ToList();
        }

        public Rental GetCurrentRentalByCar(int carId)
        {
            var query = from e in _context.RentalSet
                        where e.CarId == carId && e.DateReturned == null
                        select e;

            return query.FirstOrDefault();
        }

        public IEnumerable<Rental> GetCurrentlyRentedCars()
        {
            var query = from e in _context.RentalSet
                        where e.DateReturned == null
                        select e;

            return query.ToList();
        }

        public IEnumerable<Rental> GetRentalHistoryByAccount(int accountId)
        {
            var query = from e in _context.RentalSet
                        where e.AccountId == accountId
                        select e;

            return query.ToList();
        }

        public IEnumerable<CustomerRentalInfo> GetCurrentCustomerRentalInfo()
        {
            var query = from r in _context.RentalSet
                        where r.DateReturned == null
                        join a in _context.AccountSet on r.AccountId equals a.AccountId
                        join c in _context.CarSet on r.CarId equals c.CarId
                        select new CustomerRentalInfo()
                        {
                            Customer = a,
                            Car = c,
                            Rental = r
                        };

            return query.ToList();
        }
    }
}
