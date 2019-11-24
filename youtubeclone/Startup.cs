using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(youtubeclone.Startup))]
namespace youtubeclone
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
