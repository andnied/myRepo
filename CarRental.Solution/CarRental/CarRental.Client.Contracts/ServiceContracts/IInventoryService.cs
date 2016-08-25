using CarRental.Client.Entities;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Client.Contracts.ServiceContracts
{
    [ServiceContract]
    public interface IInventoryService : IServiceContract
    {
        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        Car GetCar(int id);

        [OperationContract]
        IEnumerable<Car> GetAllRentedCars();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        Car UpdateCar(Car car);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void DeleteCar(int id);

        [OperationContract]
        IEnumerable<Car> GetAvailableCars(DateTime pickupDate, DateTime returnDate);
    }
}
