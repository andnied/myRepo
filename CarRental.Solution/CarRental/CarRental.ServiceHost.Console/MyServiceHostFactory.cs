using CarRental.Business;
using CarRental.Data;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace CarRental.ServiceHost.Console
{
    public class MyServiceHostFactory : ServiceHostFactory
    {
        private readonly IDataRepositoryFactory _repoFactory;
        private readonly IBusinessEngineFactory _beFactory;

        public MyServiceHostFactory()
        {
            _repoFactory = new DataRepositoryFactory(null);
            _beFactory = new BusinessEngineFactory(null);
        }

        protected override System.ServiceModel.ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return new MyServiceHost(_repoFactory, _beFactory, serviceType, baseAddresses);
        }
    }
}
