﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Microsoft.Practices.Unity;
using WebAPI.Contracts.DAL;
using WebAPI.Contracts.BLL;
using WebAPI.BLL.Facades;
using Unity.WebApi;
using JsonPatch.Formatting;
using System.Net.Http.Headers;
using WebAPI.Mocks;
using WebAPI.DAL;
using WebAPI.BLL.Services;
using WebAPI.Mapper;
using CacheCow.Server;
using WebAPI.Model;

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

            config.Formatters.Add(new JsonPatchFormatter());
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            var container = new UnityContainer();
            //container.RegisterInstance(ValuesMock.GetValueRepositoryMock());
            container.RegisterInstance(WebApiMapper.GetMapper());
            container.RegisterInstance(new WebAPIContext());
            container.RegisterType<IValuesFcd, ValuesFcd>();
            container.RegisterType<IValuesService, ValuesService>();
            container.RegisterType<IValuesRepository, ValuesRepository>();

            config.DependencyResolver = new UnityDependencyResolver(container);

            config.MessageHandlers.Add(new CachingHandler(config));
        }
    }
}
