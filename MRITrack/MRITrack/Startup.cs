using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MRITrack.Startup))]
namespace MRITrack
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
