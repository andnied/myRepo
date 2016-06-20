using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.Business.Entities;
using Core.Common.Contracts;
using CarRental.Data.Contracts.DTOs;

namespace CarRental.Data.Contracts
{
    public interface IReservationRepository : IDataRepository<Reservation>
    {
        IEnumerable<Reservation> GetReservationsByPickupDate(DateTime pickupDate);
        IEnumerable<CustomerReservationInfo> GetCurrentCustomerReservationInfo();
        IEnumerable<CustomerReservationInfo> GetCustomerOpenReservationInfo(int accountId);
    }
}
