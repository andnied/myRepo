using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;

namespace SecurityMiddleware.Pipeline
{
    public class TestMiddleware
    {
        private const string Token = "123abc";
        private readonly Func<IDictionary<string, object>, Task> _next;

        public TestMiddleware(Func<IDictionary<string, object>, Task> next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> env)
        {
            var owinContext = new OwinContext(env);

            owinContext.Request.User = GetPrincipalBy(Token);

            Helper.Write("Middleware", owinContext.Request.User);

            await _next(env);
        }

        private GenericPrincipal GetPrincipalBy(string token)
        {
            return new GenericPrincipal(new GenericIdentity("TestUser"), new string[] { "admin" });
        }
    }
}