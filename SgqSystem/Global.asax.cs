﻿using Helper;
using Microsoft.ApplicationInsights.Extensibility;
using SgqSystem.Handlres;
using SgqSystem.Mappers;
using System;
using System.Diagnostics;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Hangfire;

namespace SgqSystem
{
    public class WebApiApplication : System.Web.HttpApplication
    {

        private BackgroundJobServer _backgroundJobServer;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfig.RegisterMappings();
            DisableApplicationInsightsOnDebug();
            Hangfire.GlobalConfiguration.Configuration
           .UseSqlServerStorage("DbContextSgqEUA");

            #if DEBUG
            TelemetryConfiguration.Active.DisableTelemetry = true;
            #endif


            _backgroundJobServer = new BackgroundJobServer();

        }

        protected void Application_End(object sender, EventArgs e)
        {
            _backgroundJobServer.Dispose();
        }

        /// <summary>
        /// Disables the application insights locally.
        /// </summary>
        [Conditional("DEBUG")]
        private static void DisableApplicationInsightsOnDebug()
        {
            //TelemetryConfiguration.Active.DisableTelemetry = true;
        }

    }

}
