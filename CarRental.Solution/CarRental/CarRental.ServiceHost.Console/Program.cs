using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using CarRental.Business.Services;
using Microsoft.Practices.Unity;
using Core.Common.Contracts;
using CarRental.Data;
using CarRental.Business;

namespace CarRental.ServiceHost.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Starting up services...");
            System.Console.WriteLine("");

            var container = new UnityContainer();
            container.RegisterType<IDataRepositoryFactory, DataRepositoryFactory>();
            container.RegisterType<IBusinessEngineFactory, BusinessEngineFactory>();

            using (var host = new System.ServiceModel.ServiceHost(container.Resolve<InventoryService>()))
            {
                host.Open();
            }

            System.Console.WriteLine("");
            System.Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
        }

        private static void Register()
        {

        }
    }
}
