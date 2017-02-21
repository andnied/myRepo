using CacheCow.Server;
using JsonPatch.Formatting;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;
using WebAPI.BLL.Facades;
using WebAPI.BLL.Services;
using WebAPI.Contracts.BLL;
using WebAPI.Contracts.DAL;
using WebAPI.DAL;
using WebAPI.Mapper;
using WebAPI.Model;

namespace WebAPI.Host.App_Start
{
    public class WebApiConfig
    {
        public static HttpConfiguration Register()
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("default", "api/{controller}/{id}", new { id = RouteParameter.Optional });

            config.Formatters.Add(new JsonPatchFormatter());
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            var container = new UnityContainer();
            container.RegisterInstance(WebApiMapper.GetMapper());
            container.RegisterInstance(new WebAPIContext());
            container.RegisterType<IValuesFcd, ValuesFcd>();
            container.RegisterType<IAccountsFcd, AccountsFcd>();
            container.RegisterType<IValuesService, ValuesService>();
            container.RegisterType<IValuesRepository, ValuesRepository>();

            config.DependencyResolver = new UnityDependencyResolver(container);
            config.MessageHandlers.Add(new CachingHandler(config));

            return config;
        }
    }
}