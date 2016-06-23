using CarRental.Business.Entities;
using CarRental.Data.Contracts.Repository_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.Repositories
{
    public class AccountRepository : DataRepositoryBase<Account>, IAccountRepository
    {
        protected AccountRepository(CarRentalContext context)
            : base(context)
        { }

        protected override Account AddEntity(CarRentalContext context, Account entity)
        {
            return context.AccountSet.Add(entity);
        }

        protected override IEnumerable<Account> GetEntities(CarRentalContext context)
        {
            return context.AccountSet.ToList();
        }

        protected override Account GetEntity(CarRentalContext context, int id)
        {
            return context.AccountSet.FirstOrDefault(a => a.AccountId == id);
        }

        protected override Account UpdateEntity(CarRentalContext context, Account entity)
        {
            return GetEntity(context, entity.AccountId);  
        }

        public Account GetByLogin(string login)
        {
            return _context.AccountSet.FirstOrDefault(a => a.LoginEmail == login);
        }
    }
}
