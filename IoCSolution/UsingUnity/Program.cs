using Data;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingUnity
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var container = new UnityContainer())
            {
                container.RegisterType<ICard, MasterCard>(new ContainerControlledLifetimeManager(), new InjectionProperty("CarHolderName", "Endrju"));
                var test = container.Resolve<Shopper>(/*new DependencyOverride(typeof(ICard), typeof(Visa))*/);
                test.Charge();
            }

            Console.Read();
        }
    }
}
