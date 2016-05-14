using EvalServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace EvalServerREST
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new WebServiceHost(typeof(EvalService)))
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
