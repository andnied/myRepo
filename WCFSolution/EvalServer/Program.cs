using EvalServiceLibrary;
using System;
using System.ServiceModel;

namespace EvalServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(EvalService)))
            {
                try
                {
                    host.Open();
                    Console.WriteLine("Server started...");
                    foreach (var ep in host.Description.Endpoints)
                    {
                        Console.WriteLine(ep.Address);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    host.Abort();
                }
                
                Console.ReadKey();
            }
        }
    }
}
