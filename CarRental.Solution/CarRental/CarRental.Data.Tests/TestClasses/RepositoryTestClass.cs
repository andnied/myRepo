using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.Tests.TestClasses
{
    public class RepositoryTestClass
    {
        private readonly IDataRepositoryFactory _factory;

        public RepositoryTestClass(IDataRepositoryFactory factory)
        {
            _factory = factory;
        }

        public IEnumerable<Car> GetCars()
        {
            return _factory.GetRepo<ICarRepository>().Get();
        }
    }
}
