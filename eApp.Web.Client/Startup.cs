using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eApp.Web.Client.Startup))]
namespace eApp.Web.Client
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
