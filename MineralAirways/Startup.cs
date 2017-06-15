using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MineralAirways.Startup))]
namespace MineralAirways
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
