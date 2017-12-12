using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [RoutePrefix("api/BuildPermission")]
    public class BuildPermissionApiController : ApiController
    {   
        public class PermissionValues
        {
            public String User { get; set; }
            public String Password { get; set; }
            public DateTime Date { get; set; }
        }

        [HttpPost]
        [Route("Permission")]
        public String Permission(PermissionValues PermissionValues)
        {
            if (PermissionValues.User.Equals("teste") && PermissionValues.Password.Equals("123"))
            {
                return Guard.EncryptStringAES(PermissionValues.Date.ToString());
            }
            return null;
        }
    }
}
