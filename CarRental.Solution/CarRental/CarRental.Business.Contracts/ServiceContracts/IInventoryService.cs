using CarRental.Business.Entities;
using Core.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Business.Contracts.ServiceContracts
{
    [ServiceContract]
    public interface IInventoryService
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
