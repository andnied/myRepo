using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

namespace WebAPI.Host.Owin
{
    public class ApiMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> _next;

        public ApiMiddleware(Func<IDictionary<string, object>, Task> next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> env)
        {
            var owinContext = new OwinContext(env);

            // TODO: authentication logic
            owinContext.Request.User = new GenericPrincipal(new GenericIdentity("TestUser"), new string[] { "admin" });

            await _next(env);
        }
    }
}