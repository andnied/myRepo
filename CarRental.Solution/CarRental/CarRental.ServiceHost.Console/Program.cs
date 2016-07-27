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
            container.RegisterInstance(typeof(UnityContainer), container);
            container.RegisterType<IDataRepositoryFactory, DataRepositoryFactory>();
            container.RegisterType<IBusinessEngineFactory, BusinessEngineFactory>();
            
            using (var host = new System.ServiceModel.ServiceHost(typeof(InventoryService)))
            {
                StartService(host, "InventoryService");

                System.Console.WriteLine("");
                System.Console.WriteLine("Press any key to exit.");
                System.Console.ReadKey();

                host.Close();
            }
        }

        private static void StartService(System.ServiceModel.ServiceHost host, string desc)
        {
            host.Open();
            System.Console.WriteLine(string.Format(@"Service {0} started.", desc));

            foreach (var endpoint in host.Description.Endpoints)
            {
                System.Console.WriteLine(string.Format("Listening on endpoint:"));
                System.Console.WriteLine(string.Format("Address: {0}", endpoint.Address.Uri));
                System.Console.WriteLine(string.Format("Binding: {0}", endpoint.Binding.Name));
                System.Console.WriteLine(string.Format("Contract: {0}", endpoint.Contract.Name));
            }

            System.Console.WriteLine();
        }
    }
}
