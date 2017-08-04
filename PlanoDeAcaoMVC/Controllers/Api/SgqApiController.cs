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
    public class SgqApiController : ApiController
    {

        private PlanoAcaoEF.PlanoDeAcaoEntities db;

        public SgqApiController()
        {
            db = new PlanoAcaoEF.PlanoDeAcaoEntities();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.AutoDetectChangesEnabled = false;
        }

        [HttpPost]
        [Route("GetAcoes")]
        public int GetAcoes(JObject filtro)
        {
            var unidadeId = 1;
            var level1Id = 1;
            var level2Id = 1;
            var level3Id = 1;
            var dataInicio = DateTime.Now.AddDays(-3);
            var dataFim = DateTime.Now.AddDays(3);
            return 1;

            //return db.Pa_Acao.Count(r => r.Unidade_Id == unidadeId && r.Level1Id == level1Id && r.Level2Id == level2Id && r.Level3Id == level3Id && r.AddDate <= dataFim && r.AddDate >= dataInicio);
        }


    }
}
