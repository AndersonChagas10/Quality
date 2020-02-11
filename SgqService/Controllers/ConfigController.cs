using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqService.Controllers
{
    public class ConfigController : Controller
    {

        [HttpGet]
        public String UpdateAppScripts()
        {
            return new SgqServiceBusiness.Api.ConfigController().UpdateAppScripts();
        }

        [HttpGet]
        public String UpdateDicionarioEstatico()
        {
            return new SgqServiceBusiness.Api.ConfigController().UpdateDicionarioEstatico();
        }

        [HttpGet]
        public dynamic GetAppVersionIsUpdated(string version)
        {
            return new SgqServiceBusiness.Api.ConfigController().GetAppVersionIsUpdated(version);
        }
    }
}