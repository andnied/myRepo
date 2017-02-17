using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI.Host.Owin.Basic_Authentication
{
    public class BasicAuthenticationOptions : AuthenticationOptions
    {
        public string Realm { get; private set; }
        public Func<string, string, Task<IEnumerable<Claim>>> ValidationFunction { get; private set; }

        public BasicAuthenticationOptions(string realm, Func<string, string, Task<IEnumerable<Claim>>> validationFunction)
            : base("Basic")
        {
            Realm = realm;
            ValidationFunction = validationFunction;
        }
    }
}