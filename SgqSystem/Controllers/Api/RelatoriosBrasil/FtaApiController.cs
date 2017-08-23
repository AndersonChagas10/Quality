using Dominio;
using DTO;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.RelatoriosBrasil
{
    [RoutePrefix("api/Fta")]
    public class FtaApiController : ApiController
    {
        private SgqDbDevEntities db;

        public FtaApiController()
        {
            db = new SgqDbDevEntities();
        }

        [HttpPost]
        [Route("GetUnitId")]
        public JObject GetUnitId(JObject json)
        {
            try
            {
                dynamic form = json;
                string name = form.unitName;
                bool isByname = false;

                if (form.byName != null)
                {
                    isByname = form.byName;
                }
                
                if (isByname)
                    form.unitId = db.ParCompany.FirstOrDefault(r => r.Name.Equals(name)).Id;
                else
                    form.unitId = db.ParCompany.FirstOrDefault(r => r.Initials.Equals(name)).Id;

                return form;
            }
            catch (System.Exception ex)
            {
                throw;
            }

        }

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
        public JObject GetUrl([FromBody] DataCarrierFormulario form)
        {

            //auditorId
            //auditorName
            dynamic retorno = new JObject();
            retorno.url = "http://mtzsvmqsc/PlanoDeAcao/Pa_Acao//NewFTA?";
            //retorno.url = "http://192.168.25.200/PlanoAcao/Pa_Acao//NewFTA?";

            retorno.MetaFTA += "MetaFTA=" + form.MetaFTA.ToString();
            retorno.PercentualNCFTA += "&PercentualNCFTA=" + form.PercentualNCFTA.ToString();
            retorno.ReincidenciaDesvioFTA += "&ReincidenciaDesvioFTA=" + form.ReincidenciaDesvioFTA.ToString();
            retorno.Level1Id += "&Level1Id=" + db.ParLevel1.FirstOrDefault(r => r.Name.Equals(form.level1Name)).Id.ToString();
            retorno.Level2Id += "&Level2Id=" + db.ParLevel2.FirstOrDefault(r => r.Name.Equals(form.level2Name)).Id.ToString();
            retorno.Level3Id += "&Level3Id=" + db.ParLevel3.FirstOrDefault(r => r.Name.Equals(form.level3Name)).Id.ToString();
            #region Mock de reparo para correção emergencial.
            //retorno.Supervisor_Id += "&Supervisor_Id=3";
            //retorno.Unidade_Id += "&Unidade_Id=1";
            #endregion

            if (form.unitId > 0)
            {
                retorno.Unidade_Id += "&Unidade_Id=" + db.ParCompany.FirstOrDefault(r => r.Name.Equals(form.unitId)).Id.ToString();
            }
            else
            {
                retorno.Unidade_Id += "&Unidade_Id=0";
            }

            retorno.Supervisor_Id += "&Supervisor_Id=" + form.auditorId.ToString();
            retorno.Departamento_Id += "&Departamento_Id=" + 1.ToString();

            retorno._DataInicioFTA += "&_DataInicioFTA" + form._dataInicio.ToString("dd/MM/yyyy").Replace("/", "%2F");
            retorno._DataFimFTA += "&_DataFimFTA" + form._dataFim.ToString("dd/MM/yyyy").Replace("/", "%2F");

            return retorno;
        }


    }
}
