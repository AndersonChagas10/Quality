using SgqSystem.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [RoutePrefix("api/hf")]
    public class HangFireServicesController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Reconsolidacao")]
        public void Reconsolidacao()
        {
            SimpleAsynchronous.ResendProcessJson();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SendMail")]
        public void SendMail()
        {
            SimpleAsynchronous.SendMailFromDeviationSgqApp();
        }
    }
}
