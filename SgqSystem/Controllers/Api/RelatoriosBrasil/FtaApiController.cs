using Dominio;
using DTO;
using DTO.Helpers;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.RelatoriosBrasil
{
    [RoutePrefix("api/Fta")]
    public class FtaApiController : ApiController
    {



        /// <summary>
        /// Pa_Acao/NewFTA?
        ///     MetaFTA=30
        ///     &PercentualNCFTA=40
        ///     &ReincidenciaDesvioFTA=60
        ///     &Level1Id=1
        ///     &Supervisor_Id=3
        ///     &Unidade_Id=1
        ///     &Departamento_Id=1
        /// &_DataInicioFTA=27%2F03%2F2017
        /// &_DataFimFTA=29%2F03%2F2017
        /// </summary>

        [HttpPost]
        [Route("GetUrl")]
        public string GetUrl([FromBody] DataCarrierFormulario form)
        {

            //auditorId
            //auditorName
            var url = "http://192.168.25.200/PlanoAcao/Pa_Acao//NewFTA?";

            using (var db = new SgqDbDevEntities())
            {

                url += "MetaFTA=" + form.MetaFTA.ToString();
                url += "&PercentualNCFTA=" + form.PercentualNCFTA.ToString();
                url += "&ReincidenciaDesvioFTA=" + form.ReincidenciaDesvioFTA.ToString();

                if (form.unitId > 0)
                {
                    url += "&Unidade_Id=" + db.ParCompany.FirstOrDefault(r => r.Name.Equals(form.unitId)).Id.ToString();
                }
                else
                {
                    url += "&Unidade_Id=0";
                }

                url += "&Level1Id=" + db.ParLevel1.FirstOrDefault(r => r.Name.Equals(form.level1Name)).Id.ToString();
                
                url += "&Supervisor_Id=" + form.auditorId.ToString();
                url += "&Departamento_Id=" + 1.ToString();
                
                url += "&_DataInicioFTA" + form._dataInicio.ToString("dd/MM/yyyy").Replace("/", "%2F");
                url += "&_DataFimFTA" + form._dataFim.ToString("dd/MM/yyyy").Replace("/", "%2F");

            }

            return url;
        }

    }
}
