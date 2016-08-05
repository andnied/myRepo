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
using Core.Common.Container;
using CarRental.Business.Services.Services;
using System.Timers;
using System.Transactions;
using System.Security.Principal;

namespace CarRental.ServiceHost.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var principal = new GenericPrincipal(new GenericIdentity("Admin"), new string[] { "CarRentalAdmin" });

            System.Console.WriteLine("Starting up services...");
            System.Console.WriteLine("");

            var container = DependencyFactory.Container;
            container.RegisterInstance(typeof(UnityContainer), container);
            container.RegisterType<IDataRepositoryFactory, DataRepositoryFactory>();
            container.RegisterType<IBusinessEngineFactory, BusinessEngineFactory>();

            var hostInventoryService = new System.ServiceModel.ServiceHost(typeof(InventoryService));
            var hostRentalService = new System.ServiceModel.ServiceHost(typeof(RentalService));
            //var hostInventoryService = new System.ServiceModel.ServiceHost(typeof(InventoryService));
            
            StartService(hostInventoryService, "InventoryService");
            //StartService(hostRentalService, "RentalService");

            var timer = new Timer(10000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            System.Console.WriteLine("");
            System.Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();

            timer.Stop();

            hostInventoryService.Close();
            hostRentalService.Close();
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

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var rentalService = new RentalService();
            var deadReservations = rentalService.GetDeadReservations();

            foreach (var res in deadReservations)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        rentalService.CancelReservation(res.ReservationId);
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(string.Format("Problem with canceling dead reservation: {0}", ex.Message));
                    }
                }
            }
        }
    }
}
