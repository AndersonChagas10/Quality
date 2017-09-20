using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace SgqSystem.Controllers.Api
{
    public class RecravacaoLinhaApiController : BaseApiController
    {
        private SgqDbDevEntities db;
        private List<string> errors;
        private string mensagemSucesso;
        private IParamsDomain _paramDomain;

        public RecravacaoLinhaApiController(IParamsDomain paramDomain)
        {
            _paramDomain = paramDomain;
            db = new SgqDbDevEntities();
            errors = new List<string>();
            mensagemSucesso = string.Empty;
        }

        // GET: api/RecravacaoLinhaApi
        public HttpResponseMessage Get(int Company)
        {
            var paramsFromRequest = ToDynamic(Request.Content.ReadAsStringAsync().Result);
            var query = string.Format("SELECT * FROM ParRecravacao_Linhas WHERE ParCompany_Id = {0}", Company);
            var results = QueryNinja(db, query).ToList();

            var queryTipoLata = "SELECT * FROM ParRecravacao_TipoLata WHERE Id = {0} AND IsActive = 1";
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.AutoDetectChangesEnabled = false;

            foreach (var i in results)
            {
                int idLata = int.Parse(i["Id"].ToString());
                var tipoDeLAta = QueryNinja(db, string.Format(queryTipoLata, 1)).FirstOrDefault();
                i["TipoDeLata"] = QueryNinja(db, string.Format(queryTipoLata, 1)).FirstOrDefault();


                var queryVinculoLevel21 = "select * from ParLevel2Level1 WHERE ParLevel1_Id = 48 AND ParLevel2_Id = 354 AND ParCompany_Id = 14 AND IsActive = 1";
                var queryVinculoLevel32 = "select * from ParLevel3Level2 where ParLevel2_Id = 354 AND ParCompany_Id = 14 and IsActive = 1";
                var queryVinculoLevel321 = "select * from parlevel3level2Level1 where ParLevel1_Id = 48 and ParLevel3Level2_Id IN(select Id from ParLevel3Level2 where ParLevel2_Id = 354 AND ParCompany_Id = 14 and IsActive = 1)";

                var hasVinculoLevel21 = QueryNinja(db, queryVinculoLevel21);
                var hasVinculoLevel32 = QueryNinja(db, queryVinculoLevel32);
                var hasVinculoLevel321 = QueryNinja(db, queryVinculoLevel321);

                var listaLevel3 = new List<ParLevel3>();
                if (hasVinculoLevel21.Count() > 0 && hasVinculoLevel321.Count() > 0 && hasVinculoLevel32.Count() > 0)
                {
                    foreach (var vinculoLevel32 in hasVinculoLevel32)
                    {
                        var idLevel3 = int.Parse(vinculoLevel32["ParLevel3_Id"].ToString());
                        var level3 = db.ParLevel3.Include("ParLevel3Value").FirstOrDefault(r => r.Id == idLevel3);
                        listaLevel3.Add(level3);
                    }
                }
              
                i["ListParlevel3"] = Newtonsoft.Json.Linq.JToken.FromObject(listaLevel3, new Newtonsoft.Json.JsonSerializer { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
            }
            return Request.CreateResponse(HttpStatusCode.OK, new { resposta = "Recuperados dados das Linhas", model = results });
        }
     
       
    }
}
