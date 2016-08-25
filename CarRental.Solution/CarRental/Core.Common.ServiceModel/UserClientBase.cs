using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Common.ServiceModel
{
    public abstract class UserClientBase<T> : ClientBase<T> where T : class
    {
        protected UserClientBase()
        {
            var userName = Thread.CurrentPrincipal.Identity.Name;
            var header = new MessageHeader<string>(userName);
            var contextScope = new OperationContextScope(InnerChannel);
            OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("String", "System"));
        }
    }
}
