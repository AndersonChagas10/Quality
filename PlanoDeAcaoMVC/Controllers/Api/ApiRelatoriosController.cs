using ADOFactory;
using PlanoAcaoCore;
using System.Collections.Generic;
using System.Web.Http;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/Relatorios")]
    public class ApiRelatoriosController : ApiController
    {
      

        //api/Relatorios/GetGrafico1
        [HttpPost]
        [Route("GetGrafico1")]
        public List<Pa_RelatorioGrafico> GetGrafico1([FromBody] filtros filtro)
        {

            string sql = "" +
            "\n SELECT                                                                                  " +
            "\n count(1) as quantidade,                                                                 " +
            "\n S.Name AS status,                                                                       " +
            "\n i.Name AS filtro                                                                        " +
            "\n from pa_acao A                                                                          " +
            "\n INNER JOIN PA_STATUS S                                                                  " +
            "\n ON S.ID = A.Status                                                                      " +
            "\n INNER JOIN Pa_IndicadorSgqAcao I                                                        " +
            "\n ON I.ID = A.Pa_IndicadorSgqAcao_Id                                                      " +
            "\n where A.QuandoInicio between '" + filtro.dataInicio + "' and '" + filtro.dataFim + "'   " +
            "\n group by i.Name, S.Name                                                                 " +
            "";

            var retorno = Pa_BaseObject.ListarGenerico<Pa_RelatorioGrafico>(sql);

            return retorno;

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

    public class Pa_RelatorioGrafico
    {
        public int quantidade { get; set; }
        public string status { get; set; }
        public string filtro { get; set; }
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
