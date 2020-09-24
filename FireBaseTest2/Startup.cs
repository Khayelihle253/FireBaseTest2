using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FireBaseTest2.Startup))]
namespace FireBaseTest2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
