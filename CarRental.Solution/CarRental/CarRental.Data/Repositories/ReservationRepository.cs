using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using Core.Common.Extensions;
using CarRental.Data.Contracts.DTOs;

namespace CarRental.Data.Repositories
{
    public class ReservationRepository : DataRepositoryBase<Reservation>, IReservationRepository
    {
        public ReservationRepository(CarRentalContext context)
            : base(context)
        { }

        protected override Reservation AddEntity(Reservation entity)
        {
            return _context.ReservationSet.Add(entity);
        }

        protected override Reservation UpdateEntity(Reservation entity)
        {
            return (from e in _context.ReservationSet
                    where e.ReservationId == entity.ReservationId
                    select e).FirstOrDefault();
        }

        protected override IEnumerable<Reservation> GetEntities()
        {
            return (from e in _context.ReservationSet
                   select e).ToList();
        }

        protected override Reservation GetEntity(int id)
        {
            return _context.ReservationSet.FirstOrDefault(r => r.ReservationId == id);
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
