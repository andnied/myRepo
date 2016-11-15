using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Microsoft.Practices.Unity;
using WebAPI.Contracts.DAL;
using WebAPI.Common.Mocks;
using WebAPI.Contracts.BLL;
using WebAPI.BLL.Facades;
using Unity.WebApi;

namespace WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var container = new UnityContainer();
            container.RegisterInstance(ValuesMock.GetValueRepositoryMock());
            container.RegisterType<IValuesFcd, ValuesFcd>();

            config.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
