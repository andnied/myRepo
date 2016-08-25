using CarRental.Client.Contracts.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Client.Entities;
using System.Threading;
using Core.Common.ServiceModel;

namespace CarRental.Client.Proxies.ServiceProxies
{
    public class AccountClient : UserClientBase<IAccountService>, IAccountService
    {
        public Account GetCustomerAccountInfo(string loginEmail)
        {
            return Channel.GetCustomerAccountInfo(loginEmail);
        }

        public void UpdateCustomerAccountInfo(Account account)
        {
            Channel.UpdateCustomerAccountInfo(account);
        }
    }
}
