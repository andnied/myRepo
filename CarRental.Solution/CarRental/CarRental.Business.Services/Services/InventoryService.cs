using CarRental.Business.Contracts;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using Core.Common.Exceptions;
using System.ServiceModel;

namespace CarRental.Business.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IDataRepositoryFactory _factory;

        public InventoryService(IDataRepositoryFactory factory)
        {
            _factory = factory;
        }

        public IEnumerable<Car> GetAllCars()
        {
            //var cars = _factory.GetRepo<ICarRepository>().Get();
            return null;
        }

        public Car GetCar(int id)
        {
            try
            {
                var car = _factory.GetRepo<ICarRepository>().Get(id);

                if (car != null)
                    return car;

                var ex = new NotFoundException(string.Format("Car with id = {0} not found.", id));
                throw new FaultException<NotFoundException>(ex, ex.Message);
            }
            catch (FaultException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }
    }
}
