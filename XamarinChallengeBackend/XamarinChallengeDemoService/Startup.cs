using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(XamarinChallengeDemoService.Startup))]

namespace XamarinChallengeDemoService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}