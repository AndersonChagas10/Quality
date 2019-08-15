using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dominio;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace SgqService.Controllers.Api
{
    public class AppScriptsController : BaseApiController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        private SgqServiceBusiness.Api.AppScriptsController business;

        public AppScriptsController()
        {
            business = new SgqServiceBusiness.Api.AppScriptsController();
        }

        // GET: AppScripts
        public string GetByVersion(string version)
        {
            return business.GetByVersion(version);
        }
    }
}
