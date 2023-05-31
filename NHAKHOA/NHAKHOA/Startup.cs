using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NHAKHOA.Startup))]
namespace NHAKHOA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
