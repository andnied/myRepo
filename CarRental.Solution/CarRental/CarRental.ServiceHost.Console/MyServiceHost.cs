using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.ServiceHost.Console
{
    public class MyServiceHost : System.ServiceModel.ServiceHost
    {
        public MyServiceHost(IDataRepositoryFactory repoFactory, IBusinessEngineFactory beFactory, Type serviceType, Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            if (repoFactory == null || beFactory == null)
                throw new ArgumentNullException();

            foreach (var cd in ImplementedContracts.Values)
            {
                cd.Behaviors.Add(new MyInstanceProvider(repoFactory, beFactory));
            }
        }
    }
}
