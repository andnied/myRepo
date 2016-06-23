using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using Core.Common.Extensions;
using CarRental.Data.Contracts.DTOs;

namespace CarRental.Data
{
    public class ReservationRepository : DataRepositoryBase<Reservation>, IReservationRepository
    {
        protected override Reservation AddEntity(CarRentalContext entityContext, Reservation entity)
        {
            return entityContext.ReservationSet.Add(entity);
        }

        protected override Reservation UpdateEntity(CarRentalContext entityContext, Reservation entity)
        {
            return (from e in entityContext.ReservationSet
                    where e.ReservationId == entity.ReservationId
                    select e).FirstOrDefault();
        }

        protected override IEnumerable<Reservation> GetEntities(CarRentalContext entityContext)
        {
            return from e in entityContext.ReservationSet
                   select e;
        }

        protected override Reservation GetEntity(CarRentalContext entityContext, int id)
        {
            var query = (from e in entityContext.ReservationSet
                         where e.ReservationId == id
                         select e);

            var results = query.FirstOrDefault();

            return results;
        }

        public IEnumerable<CustomerReservationInfo> GetCurrentCustomerReservationInfo()
        {
            var query = from r in _context.ReservationSet
                        join a in _context.AccountSet on r.AccountId equals a.AccountId
                        join c in _context.CarSet on r.CarId equals c.CarId
                        select new CustomerReservationInfo()
                        {
                            Customer = a,
                            Car = c,
                            Reservation = r
                        };

            return query.ToList();
        }

        public IEnumerable<Reservation> GetReservationsByPickupDate(DateTime pickupDate)
        {
            var query = from r in _context.ReservationSet
                        where r.RentalDate < pickupDate
                        select r;

            return query.ToList();
        }

        public IEnumerable<CustomerReservationInfo> GetCustomerOpenReservationInfo(int accountId)
        {
            var query = from r in _context.ReservationSet
                        join a in _context.AccountSet on r.AccountId equals a.AccountId
                        join c in _context.CarSet on r.CarId equals c.CarId
                        where r.AccountId == accountId
                        select new CustomerReservationInfo()
                        {
                            Customer = a,
                            Car = c,
                            Reservation = r
                        };

            return query.ToList();
        }
    }
}
