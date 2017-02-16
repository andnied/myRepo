using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebAPI.API.App_Start
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
            
            await _next(env);
        }
    }
}