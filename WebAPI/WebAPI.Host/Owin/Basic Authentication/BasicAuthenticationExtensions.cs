using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace WebAPI.Host.Owin.Basic_Authentication
{
    public static class BasicAuthenticationExtensions
    {
        public static IAppBuilder UseBasicAuthentication(this IAppBuilder app, string realm, Func<string, string, Task<IEnumerable<Claim>>> validationFunction)
        {
            var options = new BasicAuthenticationOptions(realm, validationFunction);

            return app.Use<BasicAuthenticationMiddleware>(options);
        }
    }
}