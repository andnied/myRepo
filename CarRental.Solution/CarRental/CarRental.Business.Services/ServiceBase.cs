using CarRental.Business.Entities;
using CarRental.Common;
using CarRental.Data.Contracts.Repository_Interfaces;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarRental.Business.Services
{
    public abstract class ServiceBase
    {
        protected readonly string _loginName;
        protected readonly Account _account;
        protected readonly IDataRepositoryFactory _factory;

        protected ServiceBase(IDataRepositoryFactory factory)
        {
            var context = OperationContext.Current;
            if (context != null)
            {
                _loginName = context.IncomingMessageHeaders.GetHeader<string>("String", "System");
                _loginName = _loginName.IndexOf(@"\") > 1 ? string.Empty : _loginName;
            }

            if (!string.IsNullOrWhiteSpace(_loginName))
            {
                _account = _factory.GetRepo<IAccountRepository>().GetByLogin(_loginName);
                
                Guard.ThrowIf<NotFoundException>(_account == null, "Cannot find account for login name {0} to use for security trimming.", _loginName);
            }

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

        protected void ValidateAuthorization(IAccountOwnedEntity entity)
        {
            if (Thread.CurrentPrincipal.IsInRole(Security.CarRentalAdminRole))
                return;

            if (_account == null)
                return;

            if (_loginName == string.Empty || entity.OwnerAccountId == _account.AccountId)
                return;

            Guard.ThrowIf<AuthorizationValidationException>(true, "Attempt to access a secure record.");
        }
    }
}
