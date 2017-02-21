using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using WebAPI.BLL.Services;
using WebAPI.Host.App_Start;
using WebAPI.Host.Owin;

namespace WebAPI.Host
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = WebApiConfig.Register();
            var options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(8),
                Provider = new AuthorizationServerProvider((ApplicationUserManager)config.DependencyResolver.GetService(typeof(ApplicationUserManager)))
            };

            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());            
            app.UseWebApi(config);
        }
    }
}