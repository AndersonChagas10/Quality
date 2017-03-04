using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/Relatorios")]
    public class ApiRelatoriosController : ApiController
    {
        //api/Relatorios/GetGrafico1
        [HttpPost]
        [Route("GetGrafico1")]
        public void GetGrafico1([FromBody] filtros filtro)
        {

        }


        [HttpPost]
        [Route("GetGrafico2")]
        public void GetGrafico2([FromBody] filtros filtro)
        {

        }

        [HttpPost]
        [Route("GetGrafico3")]
        public void GetGrafico3([FromBody] filtros filtro)
        {

        }

    }

    /// <summary>
    /// Json : 
    /// { 
    ///     diretoria: 1,
    ///     gerencia: 2,
    ///     dataInicio: '20/01/2017'
    ///     dataFim: '20/01/2017'
    /// }
    /// </summary>
    public class filtros
    {
        public int diretoria { get; set; }
        public int gerencia { get; set; }
        public string dataInicio { get; set; }
        public string dataFim { get; set; }
    }

}
