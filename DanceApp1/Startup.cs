using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DanceApp1.Startup))]
namespace DanceApp1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
