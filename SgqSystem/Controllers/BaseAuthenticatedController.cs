using Dominio;
using DTO;
using DTO.DTO;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Helper;
using System.Collections;
using AutoMapper;
using DTO.DTO.Params;
using System.Collections.Generic;
using Conformity.Application.Util;

namespace SgqSystem.Controllers
{
    [HandleController]
    public class BaseAuthenticatedController : BaseController
    {

        private ApplicationConfig _applicationConfig;

        public BaseAuthenticatedController(ApplicationConfig applicationConfig) : base()
        {
            _applicationConfig = applicationConfig;
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            var webControlCookie = System.Web.HttpContext.Current.Request.Cookies["webControlCookie"];

            if (webControlCookie != null)
            {
                var UserId = webControlCookie.Values["userId"];
                if (UserId != null && UserId != "" && int.Parse(UserId) > 0)
                {
                    _applicationConfig.Authenticated_Id = Convert.ToInt32(UserId);
                }
            }
            base.Initialize(requestContext);
        }
    }

}