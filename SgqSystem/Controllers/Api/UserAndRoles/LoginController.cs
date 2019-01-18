﻿using SgqSystem.Handlres;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api.Login
{
    [HandleApi(saveLog: false)]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/LoginApi")]
    public class LoginController : ApiController
    {
        [HttpGet]
        [Route("Logado/{dataApp?}")]
        public string Logado(DateTime? dataApp = null)
        {
            if (dataApp != null)
            {
                var dataServer = DateTime.Now;

                if (dataApp < dataServer.AddHours(-30))
                {
                    return "dataInvalida";
                }
            }

            return "onLine";
        }

        // Route /status to this controller

        /// <summary>
        /// Metodo CORRETO a ser utilizado para PING:
        ///     $.post("http://localhost/SgqSystem/api/LoginApi")
        ///     $.get("http://localhost/SgqSystem/api/LoginApi")
        /// 
        /// </summary>
        /// <returns></returns>
        //[HttpGet] // accept get
        //[HttpPost] // accept post
        //[Route("Logado")] // route default request to this method.
        //public IHttpActionResult Get()
        //{
        //    return Ok();
        //}

    }
}
