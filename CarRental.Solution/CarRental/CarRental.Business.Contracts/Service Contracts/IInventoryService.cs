using CarRental.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Business.Contracts
{
    [ServiceContract]
    public interface IInventoryService
    {
        [OperationContract]
        Car GetCar(int id);

        [OperationContract]
        IEnumerable<Car> GetAllCars();
    }
}
