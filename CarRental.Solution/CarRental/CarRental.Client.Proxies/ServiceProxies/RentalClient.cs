using CarRental.Client.Contracts.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Client.Contracts.DataContracts;
using CarRental.Client.Entities;
using Core.Common.ServiceModel;

namespace CarRental.Client.Proxies.ServiceProxies
{
    public class RentalClient : UserClientBase<IRentalService>, IRentalService
    {
        public void AcceptCarReturn(int carId)
        {
            Channel.AcceptCarReturn(carId);
        }

        public void CancelReservation(int reservationId)
        {
            Channel.CancelReservation(reservationId);
        }

        public void ExecuteRentalFromReservation(int reservationId)
        {
            Channel.ExecuteRentalFromReservation(reservationId);
        }

        public IEnumerable<CustomerRentalData> GetCurrentRentals()
        {
            return Channel.GetCurrentRentals();
        }

        public IEnumerable<CustomerReservationData> GetCurrentReservations()
        {
            return Channel.GetCurrentReservations();
        }

        public IEnumerable<CustomerReservationData> GetCustomerReservations(string loginEmail)
        {
            return Channel.GetCustomerReservations(loginEmail);
        }

        public IEnumerable<Reservation> GetDeadReservations()
        {
            return Channel.GetDeadReservations();
        }

        public Rental GetRental(int rentalId)
        {
            return Channel.GetRental(rentalId);
        }

        public IEnumerable<Rental> GetRentalHistory(string loginEmail)
        {
            return Channel.GetRentalHistory(loginEmail);
        }

        public Reservation GetReservation(int reservationId)
        {
            return Channel.GetReservation(reservationId);
        }

        public bool IsCarCurrentlyRented(int carId)
        {
            return Channel.IsCarCurrentlyRented(carId);
        }

        public Reservation MakeReservation(string loginEmail, int carId, DateTime rentalDate, DateTime returnDate)
        {
            return Channel.MakeReservation(loginEmail, carId, rentalDate, returnDate);
        }

        public Rental RentCarToCustomer(string loginEmail, int carId, DateTime dateDueBack)
        {
            return Channel.RentCarToCustomer(loginEmail, carId, dateDueBack);
        }

        public Rental RentCarToCustomer(string loginEmail, int carId, DateTime rentalDate, DateTime dateDueBack)
        {
            return Channel.RentCarToCustomer(loginEmail, carId, rentalDate, dateDueBack);
        }
    }
}
