using CarRental.Business.Contracts.ServiceContracts;
using CarRental.Client.Entities;
using CarRental.Client.Proxies.ServiceProxies;
using Core.Common.Exceptions;
using Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Functional.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var proxy = new InventoryClient();
            var car = proxy.UpdateCar(new Car
            {
                Description = "Fajny"
            });
        }
    }
}
