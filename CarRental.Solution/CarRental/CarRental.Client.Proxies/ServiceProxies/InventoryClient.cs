using CarRental.Client.Contracts.ServiceContracts;
using CarRental.Client.Entities;
using Core.Common.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Client.Proxies.ServiceProxies
{
    public class InventoryClient : UserClientBase<IInventoryService>, IInventoryService
    {
        public void DeleteCar(int id)
        {
            Channel.DeleteCar(id);
        }

        public IEnumerable<Car> GetAllRentedCars()
        {
            return Channel.GetAllRentedCars();
        }

        public IEnumerable<Car> GetAvailableCars(DateTime pickupDate, DateTime returnDate)
        {
            return Channel.GetAvailableCars(pickupDate, returnDate);
        }

        public Car GetCar(int id)
        {
            return GetCar(id);
        }

        public Car UpdateCar(Car car)
        {
            return UpdateCar(car);
        }
    }
}
