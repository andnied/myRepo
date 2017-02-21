using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using WebAPI.Host.App_Start;
using WebAPI.Host.Owin;

namespace WebAPI.Host
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(8),
                Provider = new AuthorizationServerProvider()
            };

            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            app.UseWebApi(WebApiConfig.Register());
        }
    }
}