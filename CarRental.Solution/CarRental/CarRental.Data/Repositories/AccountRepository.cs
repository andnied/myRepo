using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.Repositories
{
    public class AccountRepository : DataRepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(CarRentalContext context)
            : base(context)
        { }

        protected override Account AddEntity(Account entity)
        {
            return _context.AccountSet.Add(entity);
        }

        protected override IEnumerable<Account> GetEntities()
        {
            return _context.AccountSet.ToList();
        }

        protected override Account GetEntity(int id)
        {
            return _context.AccountSet.FirstOrDefault(a => a.AccountId == id);
        }

        protected override Account UpdateEntity(Account entity)
        {
            return (from a in _context.AccountSet
                    where a.AccountId == entity.AccountId
                    select a).FirstOrDefault();
        }

        public Account GetByLogin(string login)
        {
            return _context.AccountSet.FirstOrDefault(a => a.LoginEmail == login);
        }
    }
}
