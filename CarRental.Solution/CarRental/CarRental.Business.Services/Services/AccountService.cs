using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using System.ServiceModel;
using CarRental.Business.Contracts.Service_Contracts;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using Core.Common.Utils;
using Core.Common.Exceptions;
using System.Security.Permissions;
using CarRental.Common;

namespace CarRental.Business.Services.Services
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class AccountService : ServiceBase, IAccountService
    {
        public AccountService(IDataRepositoryFactory factory, IBusinessEngineFactory factoryBusiness)
            : base(factory, factoryBusiness)
        { }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        public Account GetCustomerAccountInfo(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var account = _factoryRepo.GetRepo<IAccountRepository>().GetByLogin(loginEmail);

                Guard.ThrowIf<NotFoundException>(account == null, "Account with login {0} not found.", loginEmail);

                ValidateAuthorization(account);

                return account;
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        public void UpdateCustomerAccountInfo(Account account)
        {
            ExecuteFaultHandledOperation(() =>
            {
                ValidateAuthorization(account);

                _factoryRepo.GetRepo<IAccountRepository>().Update(account);
            });
        }
    }
}
