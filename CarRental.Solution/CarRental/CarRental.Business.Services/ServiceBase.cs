using CarRental.Business.Entities;
using CarRental.Common;
using CarRental.Data.Contracts;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using Core.Common.Utils;
using System;
using System.ServiceModel;
using System.Threading;

namespace CarRental.Business.Services
{
    public abstract class ServiceBase
    {
        protected readonly string _loginName;
        protected readonly Account _account;
        protected readonly IDataRepositoryFactory _factoryRepo;
        protected readonly IBusinessEngineFactory _factoryBusinessEngine;

        protected ServiceBase(IDataRepositoryFactory factoryRepo, IBusinessEngineFactory factoryBusiness)
        {
            _factoryRepo = factoryRepo;
            _factoryBusinessEngine = factoryBusiness;
            var context = OperationContext.Current;

            if (context != null)
            {
                _loginName = context.IncomingMessageHeaders.GetHeader<string>("String", "System");
                _loginName = _loginName.IndexOf(@"\") > 1 ? string.Empty : _loginName;
            }

            if (!string.IsNullOrWhiteSpace(_loginName))
            {
                _account = _factoryRepo.GetRepo<IAccountRepository>().GetByLogin(_loginName);
                
                Guard.ThrowFaultExceptionIf<NotFoundException>(_account == null, "Cannot find account for login name {0} to use for security trimming.", _loginName);
            }
        }

        protected T ExecuteFaultHandledOperation<T>(Func<T> callback)
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

        protected void ExecuteFaultHandledOperation(Action callback)
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

        protected void ValidateAuthorization(IAccountOwnedEntity entity)
        {
            if (Thread.CurrentPrincipal.IsInRole(Security.CarRentalAdminRole))
                return;

            if (_account == null)
                return;

            if (_loginName == string.Empty || entity.OwnerAccountId == _account.AccountId)
                return;

            Guard.ThrowFaultExceptionIf<AuthorizationValidationException>(true, "Attempt to access a secure record.");
        }
    }
}
