using Core.Common.Contracts;
using Core.Common.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data
{
    public abstract class DataRepositoryBase<T> : DataRepositoryBase<T, CarRentalContext>
        where T : class, IIdentifiableEntity, new()
    {
        protected DbContext _injectedContext;

        public DataRepositoryBase()
        {

        }

        public DataRepositoryBase(DbContext context)
        {
            _injectedContext = context;
        }
    }
}
