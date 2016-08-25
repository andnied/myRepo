using CarRental.Business.Contracts.ServiceContracts;
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
using Core.Common.Utils;
using Core.Common.Container;

namespace CarRental.Business.Services
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public class InventoryService : ServiceBase, IInventoryService
    {
        public InventoryService()
            : base(DependencyFactory.Resolve<IDataRepositoryFactory>(), DependencyFactory.Resolve<IBusinessEngineFactory>())
        { }

        public InventoryService(IDataRepositoryFactory factory, IBusinessEngineFactory factoryBusiness)
            : base(factory, factoryBusiness)
        { }

        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public IEnumerable<Car> GetAllRentedCars()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var cars = _factoryRepo.GetRepo<ICarRepository>().GetRentedCars();

                cars.ToList().ForEach(c => c.CurrentlyRented = true);

                return cars;
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public Car GetCar(int id)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var car = _factoryRepo.GetRepo<ICarRepository>().Get(id);

                Guard.ThrowIf<NotFoundException>(car == null, "Car with id = {0} not found.", id.ToString());

                return car;
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [OperationBehavior(TransactionScopeRequired = true)]
        public Car UpdateCar(Car car)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                if (car.CarId == 0)
                    return _factoryRepo.GetRepo<ICarRepository>().Add(car);
                else
                    return _factoryRepo.GetRepo<ICarRepository>().Update(car);
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [OperationBehavior(TransactionScopeRequired = true)]
        public void DeleteCar(int id)
        {
            ExecuteFaultHandledOperation(() =>
            {
                _factoryRepo.GetRepo<ICarRepository>().Remove(id);
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public IEnumerable<Car> GetAvailableCars(DateTime pickupDate, DateTime returnDate)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                return _factoryRepo.GetRepo<ICarRepository>().GetAvailableCars(pickupDate, returnDate);
            });
        }
    }
}
