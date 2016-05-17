using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManualContainer
{
    class Program
    {
        static void Main(string[] args)
        {
            Resolver.RegisterDependency<ICard, Visa>();
            Resolver.RegisterDependency<Shopper, Shopper>();
            var shopper = Resolver.Resolve<Shopper>();
            shopper.Charge();
            Console.Read();
        }
    }
}
