using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;

namespace WebAPI.Host.Owin.Basic_Authentication
{
    public class BasicAuthenticationMiddleware : AuthenticationMiddleware<BasicAuthenticationOptions>
    {
        private readonly BasicAuthenticationOptions _options;

        public BasicAuthenticationMiddleware(OwinMiddleware next, BasicAuthenticationOptions options) 
            : base(next, options)
        {
            _options = options;
        }

        protected override AuthenticationHandler<BasicAuthenticationOptions> CreateHandler()
        {
            return new BasicAuthenticationHandler(_options);
        }
    }
}