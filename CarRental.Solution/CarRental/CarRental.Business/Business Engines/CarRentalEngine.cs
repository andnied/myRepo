using CarRental.Business.Common;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Business.Business_Engines
{
    public class CarRentalEngine : ICarRentalEngine
    {
        private readonly IDataRepositoryFactory _factory;

        public CarRentalEngine(IDataRepositoryFactory factory)
        {
            _factory = factory;
        }

        public bool IsCarCurrentlyRented(int carId, int accountId)
        {
            var currentRental = _factory.GetRepo<IRentalRepository>().GetCurrentRentalByCar(carId);
            if (currentRental != null && currentRental.AccountId == accountId)
                return true;

            return false;
        }

        public bool IsCarCurrentlyRented(int carId)
        {
            var currentRental = _factory.GetRepo<IRentalRepository>().GetCurrentRentalByCar(carId);
            if (currentRental != null)
                return true;

            return false;
        }

        public Rental RentCarToCustomer(string loginEmail, int carId, DateTime rentalDate, DateTime dateDueBack)
        {
            if (rentalDate > DateTime.Now)
                throw new UnableToRentForDateException(string.Format("Cannot rent for date {0} yet.", rentalDate.ToShortDateString()));

            var accountRepository = _factory.GetRepo<IAccountRepository>();
            var rentalRepository = _factory.GetRepo<IRentalRepository>();

            bool carIsRented = IsCarCurrentlyRented(carId);
            if (carIsRented)
                throw new CarCurrentlyRentedException(string.Format("Car {0} is already rented.", carId));

            var account = accountRepository.GetByLogin(loginEmail);
            if (account == null)
                throw new NotFoundException(string.Format("No account found for login '{0}'.", loginEmail));

            Rental rental = new Rental()
            {
                AccountId = account.AccountId,
                CarId = carId,
                DateRented = rentalDate,
                DateDue = dateDueBack
            };

            Rental savedEntity = rentalRepository.Add(rental);

            return savedEntity;
        }

        //public bool IsCarAvailableForRental(int carId, DateTime pickupDate, DateTime returnDate,
        //                                    IEnumerable<Rental> rentedCars, IEnumerable<Reservation> reservedCars)
        //{
        //    bool available = true;

        //    Reservation reservation = reservedCars.Where(item => item.CarId == carId).FirstOrDefault();
        //    if (reservation != null && (
        //        (pickupDate >= reservation.RentalDate && pickupDate <= reservation.ReturnDate) ||
        //        (returnDate >= reservation.RentalDate && returnDate <= reservation.ReturnDate)))
        //    {
        //        available = false;
        //    }

        //    if (available)
        //    {
        //        Rental rental = rentedCars.Where(item => item.CarId == carId).FirstOrDefault();
        //        if (rental != null && (pickupDate <= rental.DateDue))
        //            available = false;
        //    }

        //    return available;
        //}
    }
}
