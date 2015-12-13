using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Chart.Startup))]
namespace Chart
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
