using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.Business.Entities;
using Core.Common.Contracts;
using CarRental.Data.Contracts.DTOs;

namespace CarRental.Data.Contracts
{
    public interface IRentalRepository : IDataRepository<Rental>
    {
        IEnumerable<Rental> GetRentalHistoryByCar(int carId);
        Rental GetCurrentRentalByCar(int carId);
        IEnumerable<Rental> GetCurrentlyRentedCars();
        IEnumerable<Rental> GetRentalHistoryByAccount(int accountId);
        IEnumerable<CustomerRentalInfo> GetCurrentCustomerRentalInfo();
    }
}
