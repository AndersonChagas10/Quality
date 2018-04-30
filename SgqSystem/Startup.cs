using Microsoft.Owin;
using Owin;
using System.Web;
using SgqSystem.Mail;
using DTO;

[assembly: OwinStartup(typeof(SgqSystem.Startup))]

namespace SgqSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
