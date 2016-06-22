using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Business.Services
{
    public abstract class ServiceBase
    {
        protected readonly IDataRepositoryFactory _factory;

        protected ServiceBase(IDataRepositoryFactory factory)
        {
            _factory = factory;
        }

        protected T ExecuteFaultHandledOperations<T>(Func<T> callback)
        {
            try
            {
                return callback.Invoke();
            }
            catch (FaultException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        protected void ExecuteFaultHandledOperations(Action callback)
        {
            try
            {
                callback.Invoke();
            }
            catch (FaultException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }
    }
}
