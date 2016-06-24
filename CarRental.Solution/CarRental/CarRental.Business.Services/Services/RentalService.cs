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
using CarRental.Common;
using System.Security.Permissions;

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

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalUser)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public IEnumerable<Rental> GetRentalHistory(string loginMail)
        {
            return ExecuteFaultHandledOperations(() =>
            {
                var account = _factory.GetRepo<AccountRepository>().GetByLogin(loginMail);

                if (account != null)
                {
                    ValidateAuthorization(account);
                    return _factory.GetRepo<RentalRepository>().GetRentalHistoryByAccount(account.AccountId);
                }

                var ex = new NotFoundException(string.Format("Account not found for login {0}", loginMail));
                throw new FaultException<NotFoundException>(ex, ex.Message);
            });
        }
    }
}
