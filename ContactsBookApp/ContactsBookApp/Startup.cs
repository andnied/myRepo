using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ContactsBookApp.Startup))]
namespace ContactsBookApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
