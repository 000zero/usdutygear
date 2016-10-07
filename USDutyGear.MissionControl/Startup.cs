using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(USDutyGear.MissionControl.Startup))]
namespace USDutyGear.MissionControl
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
