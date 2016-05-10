using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(USDutyGear.Startup))]
namespace USDutyGear
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
