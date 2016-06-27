using CarRental.Business.Contracts.Service_Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using CarRental.Business.Entities;
using System.ServiceModel;
using CarRental.Data.Repositories;
using Core.Common.Exceptions;
using System.Security.Permissions;
using CarRental.Business.Contracts.Data_Contracts;
using CarRental.Common;
using Core.Common.Utils;
using CarRental.Data.Contracts;
using CarRental.Business.Common;

namespace CarRental.Business.Services.Services
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class RentalService : ServiceBase, IRentalService
    {
        public RentalService(IDataRepositoryFactory factory, IBusinessEngineFactory factoryBusiness)
            : base(factory, factoryBusiness)
        { }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void AcceptCarReturn(int carId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                var rentalRepository = _factoryRepo.GetRepo<IRentalRepository>();
                var rental = _factoryRepo.GetRepo<IRentalRepository>().GetCurrentRentalByCar(carId);

                Guard.ThrowIf<CarNotRentedException>(rental == null, "Car {0} is not currently rented.", carId.ToString());

                rental.DateReturned = DateTime.Now;

                Rental updatedRentalEntity = rentalRepository.Update(rental);
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void CancelReservation(int reservationId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                IReservationRepository reservationRepository = _factoryRepo.GetRepo<IReservationRepository>();

                Reservation reservation = reservationRepository.Get(reservationId);

                Guard.ThrowIf<NotFoundException>(reservation == null, "No reservation found for id '{0}'.", reservationId.ToString());

                reservationRepository.Remove(reservationId);
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void ExecuteRentalFromReservation(int reservationId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                var accountRepository = _factoryRepo.GetRepo<IAccountRepository>();
                var reservationRepository = _factoryRepo.GetRepo<IReservationRepository>();
                var carRentalEngine = _factoryBusinessEngine.GetEngine<ICarRentalEngine>();

                var reservation = reservationRepository.Get(reservationId);
                var account = accountRepository.Get(reservation.AccountId);

                Guard.ThrowIf<NotFoundException>(reservation == null, "No reservation found for id '{0}'.", reservationId.ToString());
                Guard.ThrowIf<NotFoundException>(account == null, "Account not found for ID {0}", reservation.AccountId.ToString());

                try
                {
                    var rental = carRentalEngine.RentCarToCustomer(account.LoginEmail, reservation.CarId, reservation.RentalDate, reservation.ReturnDate);
                }
                catch (UnableToRentForDateException ex)
                {
                    throw new FaultException<UnableToRentForDateException>(ex, ex.Message);
                }
                catch (CarCurrentlyRentedException ex)
                {
                    throw new FaultException<CarCurrentlyRentedException>(ex, ex.Message);
                }
                catch (NotFoundException ex)
                {
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                reservationRepository.Remove(reservation);
            });
        }

        public IEnumerable<CustomerRentalData> GetCurrentRentals()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var rentalData = new List<CustomerRentalData>();
                var rentalRepository = _factoryRepo.GetRepo<IRentalRepository>();
                var rentalInfoSet = rentalRepository.GetCurrentCustomerRentalInfo();

                foreach (var rentalInfo in rentalInfoSet)
                {
                    rentalData.Add(new CustomerRentalData()
                    {
                        RentalId = rentalInfo.Rental.RentalId,
                        Car = rentalInfo.Car.Color + " " + rentalInfo.Car.Year + " " + rentalInfo.Car.Description,
                        CustomerName = rentalInfo.Customer.FirstName + " " + rentalInfo.Customer.LastName,
                        DateRented = rentalInfo.Rental.DateRented,
                        ExpectedReturn = rentalInfo.Rental.DateDue
                    });
                }

                return rentalData;
            });
        }

        public IEnumerable<CustomerReservationData> GetCurrentReservations()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var reservationRepository = _factoryRepo.GetRepo<IReservationRepository>();
                var reservationData = new List<CustomerReservationData>();
                var reservationInfoSet = reservationRepository.GetCurrentCustomerReservationInfo();

                foreach (var reservationInfo in reservationInfoSet)
                {
                    reservationData.Add(new CustomerReservationData()
                    {
                        ReservationId = reservationInfo.Reservation.ReservationId,
                        Car = reservationInfo.Car.Color + " " + reservationInfo.Car.Year + " " + reservationInfo.Car.Description,
                        CustomerName = reservationInfo.Customer.FirstName + " " + reservationInfo.Customer.LastName,
                        RentalDate = reservationInfo.Reservation.RentalDate,
                        ReturnDate = reservationInfo.Reservation.ReturnDate
                    });
                }

                return reservationData;
            });
        }

        public IEnumerable<CustomerReservationData> GetCustomerReservations(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var reservationData = new List<CustomerReservationData>();
                var accountRepository = _factoryRepo.GetRepo<IAccountRepository>();
                var reservationRepository = _factoryRepo.GetRepo<IReservationRepository>();
                var account = accountRepository.GetByLogin(loginEmail);

                Guard.ThrowIf<NotFoundException>(account == null, "No account found for login '{0}'.", loginEmail);

                var reservationInfoSet = reservationRepository.GetCustomerOpenReservationInfo(account.AccountId);

                foreach (var reservationInfo in reservationInfoSet)
                {
                    reservationData.Add(new CustomerReservationData()
                    {
                        ReservationId = reservationInfo.Reservation.ReservationId,
                        Car = reservationInfo.Car.Color + " " + reservationInfo.Car.Year + " " + reservationInfo.Car.Description,
                        CustomerName = reservationInfo.Customer.FirstName + " " + reservationInfo.Customer.LastName,
                        RentalDate = reservationInfo.Reservation.RentalDate,
                        ReturnDate = reservationInfo.Reservation.ReturnDate
                    });
                }

                return reservationData;
            });
        }

        public IEnumerable<Reservation> GetDeadReservations()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var reservationRepository = _factoryRepo.GetRepo<IReservationRepository>();

                return reservationRepository.GetReservationsByPickupDate(DateTime.Now.AddDays(-1));
            });
        }

        public Rental GetRental(int rentalId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var accountRepository = _factoryRepo.GetRepo<IAccountRepository>();
                var rentalRepository = _factoryRepo.GetRepo<IRentalRepository>();
                var rental = rentalRepository.Get(rentalId);

                Guard.ThrowIf<NotFoundException>(rental == null, "No rental record found for id '{0}'.", rentalId.ToString());

                return rental;
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalUser)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public IEnumerable<Rental> GetRentalHistory(string loginMail)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var account = _factoryRepo.GetRepo<IAccountRepository>().GetByLogin(loginMail);
                
                Guard.ThrowIf<NotFoundException>(account == null, "Account not found for login {0}", loginMail);
                ValidateAuthorization(account);

                return _factoryRepo.GetRepo<RentalRepository>().GetRentalHistoryByAccount(account.AccountId);                
            });
        }

        public Reservation GetReservation(int reservationId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var reservationRepository = _factoryRepo.GetRepo<IReservationRepository>();

                Reservation reservation = reservationRepository.Get(reservationId);

                Guard.ThrowIf<NotFoundException>(reservation == null, "No reservation found for id '{0}'.", reservationId.ToString());

                return reservation;
            });
        }

        public bool IsCarCurrentlyRented(int carId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var carRentalEngine = _factoryBusinessEngine.GetEngine<ICarRentalEngine>();

                return carRentalEngine.IsCarCurrentlyRented(carId);
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Reservation MakeReservation(string loginEmail, int carId, DateTime rentalDate, DateTime returnDate)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var accountRepository = _factoryRepo.GetRepo<IAccountRepository>();
                var reservationRepository = _factoryRepo.GetRepo<IReservationRepository>();

                Account account = accountRepository.GetByLogin(loginEmail);

                Guard.ThrowIf<NotFoundException>(account == null, "No account found for login '{0}'.", loginEmail);

                Reservation reservation = new Reservation()
                {
                    AccountId = account.AccountId,
                    CarId = carId,
                    RentalDate = rentalDate,
                    ReturnDate = returnDate
                };

                Reservation savedEntity = reservationRepository.Add(reservation);
                
                return savedEntity;
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Rental RentCarToCustomer(string loginEmail, int carId, DateTime dateDueBack)
        {
            return RentCarToCustomer(loginEmail, carId, DateTime.Now, dateDueBack);
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Rental RentCarToCustomer(string loginEmail, int carId, DateTime rentalDate, DateTime dateDueBack)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                try
                {
                    return _factoryBusinessEngine.GetEngine<ICarRentalEngine>().RentCarToCustomer(loginEmail, carId, rentalDate, dateDueBack);
                }
                catch (UnableToRentForDateException ex)
                {
                    throw new FaultException<UnableToRentForDateException>(ex, ex.Message);
                }
                catch (CarCurrentlyRentedException ex)
                {
                    throw new FaultException<CarCurrentlyRentedException>(ex, ex.Message);
                }
                catch (NotFoundException ex)
                {
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
            });
        }
    }
}
