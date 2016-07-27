using CarRental.Business.Services;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.ServiceHost.Console
{
    public class MyInstanceProvider : IInstanceProvider, IContractBehavior
    {
        private readonly IDataRepositoryFactory _repoFactory;
        private readonly IBusinessEngineFactory _beFactory;

        public MyInstanceProvider(IDataRepositoryFactory repoFactory, IBusinessEngineFactory beFactory)
        {
            if (repoFactory == null || beFactory == null)
                throw new ArgumentNullException();

            _repoFactory = repoFactory;
            _beFactory = beFactory;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return new InventoryService(_repoFactory, _beFactory);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return GetInstance(instanceContext);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            var disposable = instance as IDisposable;

            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {

        }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            dispatchRuntime.InstanceProvider = this;
        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
            
        }
    }
}
