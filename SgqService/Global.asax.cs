using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DTO;
using SgqServiceBusiness.Mappers;

namespace SgqService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutoMapperConfig.RegisterMappings();
            GlobalConfig.VerifyConfig("DefaultConnection");

            Dominio.Seed.Seed.SetDicionario();

            Dominio.Seed.Seed.SetSeedValues(isEN: GlobalConfig.LanguageEUA, runSetSeed: bool.Parse(DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.RunScriptsService ?? "false"));
        }
    }
}
