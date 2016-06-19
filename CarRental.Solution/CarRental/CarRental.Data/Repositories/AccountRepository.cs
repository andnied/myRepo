using CarRental.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.Repositories
{
    public class AccountRepository : DataRepositoryBase<Account>
    {
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
            context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            return entity;   
        }
    }
}
