using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OpenForum.Startup))]
namespace OpenForum
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
