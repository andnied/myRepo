using ContactsBook.Interface;
using ContactsBook.SqlRepository;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace ContactsBook
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            container.RegisterType<IContactsRepository, ContactsSqlRepository>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}