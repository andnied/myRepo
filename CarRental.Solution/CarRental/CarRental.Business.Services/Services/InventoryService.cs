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
using System.Security.Permissions;
using CarRental.Common;

namespace CarRental.Business.Services
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public class InventoryService : ServiceBase, IInventoryService
    {
        public InventoryService(IDataRepositoryFactory factory)
            : base(factory)
        { }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalUser)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public IEnumerable<Car> GetAllRentedCars()
        {
            return ExecuteFaultHandledOperations(() =>
            {
                var cars = _factory.GetRepo<ICarRepository>().GetRentedCars();

                cars.ToList().ForEach(c => c.CurrentlyRented = true);

                return cars;
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalUser)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public Car GetCar(int id)
        {
            return ExecuteFaultHandledOperations(() =>
            {
                var car = _factory.GetRepo<ICarRepository>().Get(id);

                if (car != null)
                    return car;

                var ex = new NotFoundException(string.Format("Car with id = {0} not found.", id));
                throw new FaultException<NotFoundException>(ex, ex.Message);
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [OperationBehavior(TransactionScopeRequired = true)]
        public Car UpdateCar(Car car)
        {
            return ExecuteFaultHandledOperations(() =>
            {
                if (car.CarId == 0)
                    return _factory.GetRepo<ICarRepository>().Add(car);
                else
                    return _factory.GetRepo<ICarRepository>().Update(car);
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [OperationBehavior(TransactionScopeRequired = true)]
        public void DeleteCar(int id)
        {
            ExecuteFaultHandledOperations(() =>
            {
                _factory.GetRepo<ICarRepository>().Remove(id);
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalUser)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public IEnumerable<Car> GetAvailableCars(DateTime pickupDate, DateTime returnDate)
        {
            return ExecuteFaultHandledOperations(() =>
            {
                return _factory.GetRepo<ICarRepository>().GetAvailableCars(pickupDate, returnDate);
            });
        }
    }
}
