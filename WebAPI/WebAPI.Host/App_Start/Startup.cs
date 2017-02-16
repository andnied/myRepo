using CacheCow.Server;
using JsonPatch.Formatting;
using Microsoft.Practices.Unity;
using Owin;
using System.Web.Http;
using Unity.WebApi;
using WebAPI.BLL.Facades;
using WebAPI.BLL.Services;
using WebAPI.Contracts.BLL;
using WebAPI.Contracts.DAL;
using WebAPI.DAL;
using WebAPI.Host.Owin;
using WebAPI.Mapper;
using WebAPI.Model;

namespace WebAPI.Host
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
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
            container.RegisterType<IValuesService, ValuesService>();
            container.RegisterType<IValuesRepository, ValuesRepository>();

            config.DependencyResolver = new UnityDependencyResolver(container);
            config.MessageHandlers.Add(new CachingHandler(config));

            app.Use(typeof(ApiMiddleware));
            app.UseWebApi(config);
        }
    }
}