using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eRecruitment.Sita.Web.Startup))]
namespace eRecruitment.Sita.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
