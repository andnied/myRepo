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

namespace CarRental.Business.Services.Services
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class RentalService : ServiceBase, IRentalService
    {
        public RentalService(IDataRepositoryFactory factory)
            : base(factory)
        { }

        public void AcceptCarReturn(int carId)
        {
            throw new NotImplementedException();
        }

        public void CancelReservation(int reservationId)
        {
            throw new NotImplementedException();
        }

        public void ExecuteRentalFromReservation(int reservationId)
        {
            throw new NotImplementedException();
        }

        public CustomerRentalData[] GetCurrentRentals()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CustomerReservationData> GetCurrentReservations()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CustomerReservationData> GetCustomerReservations(string loginEmail)
        {
            throw new NotImplementedException();
        }

        public Reservation[] GetDeadReservations()
        {
            throw new NotImplementedException();
        }

        public Rental GetRental(int rentalId)
        {
            throw new NotImplementedException();
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalUser)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public IEnumerable<Rental> GetRentalHistory(string loginMail)
        {
            return ExecuteFaultHandledOperations(() =>
            {
                var account = _factory.GetRepo<AccountRepository>().GetByLogin(loginMail);
                
                Guard.ThrowIf<NotFoundException>(account == null, "Account not found for login {0}", loginMail);                
                ValidateAuthorization(account);

                return _factory.GetRepo<RentalRepository>().GetRentalHistoryByAccount(account.AccountId);                
            });
        }

        public Reservation GetReservation(int reservationId)
        {
            throw new NotImplementedException();
        }

        public bool IsCarCurrentlyRented(int carId)
        {
            throw new NotImplementedException();
        }

        public Reservation MakeReservation(string loginEmail, int carId, DateTime rentalDate, DateTime returnDate)
        {
            throw new NotImplementedException();
        }

        public Rental RentCarToCustomer(string loginEmail, int carId, DateTime dateDueBack)
        {
            throw new NotImplementedException();
        }

        public Rental RentCarToCustomer(string loginEmail, int carId, DateTime rentalDate, DateTime dateDueBack)
        {
            throw new NotImplementedException();
        }
    }
}
