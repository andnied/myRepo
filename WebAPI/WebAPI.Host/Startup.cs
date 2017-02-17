using CacheCow.Server;
using JsonPatch.Formatting;
using Microsoft.Practices.Unity;
using Owin;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Unity.WebApi;
using WebAPI.BLL.Facades;
using WebAPI.BLL.Services;
using WebAPI.Contracts.BLL;
using WebAPI.Contracts.DAL;
using WebAPI.DAL;
using WebAPI.Host.App_Start;
using WebAPI.Host.Owin;
using WebAPI.Host.Owin.Basic_Authentication;
using WebAPI.Mapper;
using WebAPI.Model;

namespace WebAPI.Host
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseBasicAuthentication("webapitestapp", ValidateUsers);
            app.UseWebApi(WebApiConfig.Register());
        }

        private Task<IEnumerable<Claim>> ValidateUsers(string id, string secret)
        {
            if (id == secret)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, id),
                    new Claim(ClaimTypes.Role, "foo")
                };

                return Task.FromResult<IEnumerable<Claim>>(claims);
            }

            return Task.FromResult<IEnumerable<Claim>>(null);
        }
    }
}