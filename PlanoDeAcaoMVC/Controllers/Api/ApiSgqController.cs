using ADOFactory;
using Newtonsoft.Json.Linq;
using PlanoDeAcaoMVC.SgqIntegracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/Sgq")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ApiSgqController : ApiController
    {

        private PlanoAcaoEF.PlanoDeAcaoEntities db;

        public ApiSgqController()
        {
            db = new PlanoAcaoEF.PlanoDeAcaoEntities();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.AutoDetectChangesEnabled = false;
        }

        [HttpPost]
        [Route("GetAcoes")]
        public Dictionary<string, int> GetAcoes(JObject filtro)
        {
            dynamic filtroDyn = filtro;
            var dataInicio = DateTime.Now.AddDays(-3);
            var dataFim = DateTime.Now.AddDays(3);
            var retorno = new Dictionary<string, int>();

            using (var dbSgq = new ConexaoSgq().db)
            {
                string unidade = filtroDyn.unitName;
                string level1 = filtroDyn.level1Name;

                FiltraPorLevel3(filtroDyn, dataInicio, dataFim, retorno, dbSgq, unidade, level1);

            }

            return retorno;
        }

        private void FiltraPorLevel3(dynamic filtroDyn, DateTime dataInicio, DateTime dataFim, Dictionary<string, int> retorno, Factory dbSgq, string unidade, string level1)
        {
            string level2 = filtroDyn.level2Name;
            List<string> level3Name = filtroDyn.level3names.ToObject<List<string>>();

            dynamic unidadeDyn = dbSgq.QueryNinjaADO("SELECT Id FROM PARCOMPANY WHERE Initials = '" + unidade + "'").FirstOrDefault();
            int unidadeId = unidadeDyn.Id;
            var registros = 0;
            foreach (var l3Name in level3Name)
            {
                registros = db.Pa_Acao.Count(r => r.Unidade_Id == unidadeId && r.Level1Name == level1 && r.Level2Name == level2 && r.Level3Name == l3Name && r.AddDate <= dataFim && r.AddDate >= dataInicio);
                retorno.Add(l3Name, registros);
            }
        }
    }
}
