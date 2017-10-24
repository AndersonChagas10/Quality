using AutoMapper;
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
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.AutoDetectChangesEnabled = false;

            var parlevel1_ids = db.Database.SqlQuery<ParLevel1DTO>("SELECT Id, Name From Parlevel1 where IsRecravacao = 1").ToList();
            var parlevel2 = db.Database.SqlQuery<ParLevel2DTO>(@"select id, name from parlevel2 where id in (select DISTINCT(Parlevel2_Id) from parlevel3level2 where id in ( select parlevel3level2_id from parlevel3level2level1 where parlevel1_id in ( select id from parlevel1 where isrecravacao = 1)))").ToList();
            var parcompany = Company;
            var paramsFromRequest = ToDynamic(Request.Content.ReadAsStringAsync().Result);
            var query = string.Format(" SELECT * FROM ParRecravacao_Linhas WHERE ParCompany_Id = {0} and ParLevel2_Id is not null", Company);
            var results = QueryNinja(db, query).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, new { resposta = "Busca de linhas concluída", model = results });
        }

        // GET: api/RecravacaoLinhaApi
        public HttpResponseMessage Post(Newtonsoft.Json.Linq.JObject Company)
        {
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.AutoDetectChangesEnabled = false;
            var id = Company["Id"].ToString();
            var parlevel1_ids = db.Database.SqlQuery<ParLevel1DTO>("SELECT Id, Name From Parlevel1 WHERE IsRecravacao = 1").ToList();
            var parlevel2_Id = Company["ParLevel2_Id"].ToString();
            var parcompany = Company["ParCompany_Id"].ToString();
            var query = string.Format(" SELECT * FROM ParRecravacao_Linhas WHERE ParCompany_Id = {0} AND ParLevel2_Id is not null AND Id  = {1}", parcompany, id);
            var results = QueryNinja(db, query).ToList();
            var queryTipoLata = "SELECT * FROM ParRecravacao_TipoLata WHERE Id = {0} AND IsActive = 1";
            var level1Ids = string.Join(",", parlevel1_ids.Select(r => r.Id).ToList());

            foreach (var i in results)
            {
                int idLata = int.Parse(i["Id"].ToString());
                int parRecravacao_TypeLata_Id = int.Parse(i["ParRecravacao_TypeLata_Id"].ToString());
                var tipoDeLAta = QueryNinja(db, string.Format(queryTipoLata, parRecravacao_TypeLata_Id)).FirstOrDefault();
                i["TipoDeLata"] = tipoDeLAta;//QueryNinja(db, string.Format(queryTipoLata, parRecravacao_TypeLata_Id)).FirstOrDefault();
                var queryVinculoLevel21 = string.Format("SELECT * FROM ParLevel2Level1 WHERE ParLevel1_Id in ({0}) AND ParLevel2_Id = {1} AND ParCompany_Id = {2} AND IsActive = 1", level1Ids, parlevel2_Id, parcompany);
                var queryVinculoLevel32 = string.Format("SELECT * FROM ParLevel3Level2 WHERE ParLevel2_Id = {0} AND ParCompany_Id = {1} AND IsActive = 1", parlevel2_Id, parcompany);
                var queryVinculoLevel321 = string.Format("SELECT * FROM parlevel3level2Level1 WHERE ParLevel1_Id in ({0}) AND ParLevel3Level2_Id IN (select Id from ParLevel3Level2 WHERE ParLevel2_Id = {1} AND ParCompany_Id = {2} AND IsActive = 1)", level1Ids, parlevel2_Id, parcompany);
                var hasVinculoLevel21 = QueryNinja(db, queryVinculoLevel21);
                var hasVinculoLevel32 = QueryNinja(db, queryVinculoLevel32);
                var hasVinculoLevel321 = QueryNinja(db, queryVinculoLevel321);
                var listaLevel3 = new List<ParLevel3DTO>();

                if (hasVinculoLevel21.Count() > 0 && hasVinculoLevel321.Count() > 0 && hasVinculoLevel32.Count() > 0)
                    foreach (var vinculoLevel32 in hasVinculoLevel32)
                        AdicionaParLevel3ALata(parcompany, listaLevel3, vinculoLevel32);

                queryVinculoLevel21 = string.Format("SELECT * FROM ParLevel2Level1 WHERE ParLevel1_Id in ({0}) AND ParLevel2_Id = {1} AND ParCompany_Id IS NULL AND          IsActive = 1", level1Ids, parlevel2_Id, parcompany);
                queryVinculoLevel32 = string.Format(@"
	                SELECT * FROM ParLevel3Level2 WHERE ParLevel2_Id = {0} AND (ParCompany_Id IS NULL) AND IsActive = 1
                    AND ParLevel3_Id NOT IN  (
                        SELECT DISTINCT(ParLevel3_Id) FROM ParLevel3Level2 WHERE ParLevel2_Id = {0} AND (ParCompany_Id = {1}) AND IsActive = 1)
                    --AND ParLevel3_Id IN  (
                    --    SELECT DISTINCT(ParLevel3_Id) FROM ParLevel3Value WHERE (ParCompany_Id IN ({1}) OR ParCompany_Id IS NULL)  AND IsActive = 1)
                    ", parlevel2_Id, parcompany);
                queryVinculoLevel321 = string.Format(@"
                    SELECT * FROM parlevel3level2Level1 
                    WHERE ParLevel1_Id in ({0}) 
                    AND ParLevel3Level2_Id IN (
	                    SELECT Id FROM ParLevel3Level2 WHERE ParLevel2_Id = {1} AND (ParCompany_Id IS NULL) AND IsActive = 1
	                    AND ParLevel3_Id NOT IN  (
	                    SELECT DISTINCT(ParLevel3_Id) FROM ParLevel3Level2 WHERE ParLevel2_Id = {1} AND (ParCompany_Id = {2}) AND IsActive = 1)
	                    )
                    ", level1Ids, parlevel2_Id, parcompany);

                var hasVinculoLevel21TodasUnidade = QueryNinja(db, queryVinculoLevel21);
                var hasVinculoLevel32TodasUnidade = QueryNinja(db, queryVinculoLevel32);
                var hasVinculoLevel321TodasUnidade = QueryNinja(db, queryVinculoLevel321);

                if (hasVinculoLevel21TodasUnidade.Count() > 0 && hasVinculoLevel32TodasUnidade.Count() > 0 && hasVinculoLevel321TodasUnidade.Count() > 0)
                    foreach (var vinculoLevel32TodaUnidades in hasVinculoLevel32TodasUnidade)
                        AdicionaParLevel3ALata(parcompany, listaLevel3, vinculoLevel32TodaUnidades);

                i["ListParlevel3"] = Newtonsoft.Json.Linq.JToken.FromObject(listaLevel3, new Newtonsoft.Json.JsonSerializer { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { resposta = "Busca de linhas concluída", model = results });
        }

        private void AdicionaParLevel3ALata(string parcompany, List<ParLevel3DTO> listaLevel3, Newtonsoft.Json.Linq.JObject vinculoLevel32)
        {
            var idLevel3 = int.Parse(vinculoLevel32["ParLevel3_Id"].ToString());
            var level3 = db.ParLevel3.Include("ParLevel3Value").Include("ParLevel3Value.ParLevel3BoolFalse").Include("ParLevel3Value.ParLevel3BoolTrue").FirstOrDefault(r => r.Id == idLevel3);

            while (level3.ParLevel3Value.Any(r => !r.IsActive))
                level3.ParLevel3Value.Remove(level3.ParLevel3Value.FirstOrDefault(r => !r.IsActive));

            var level3Dto = Mapper.Map<ParLevel3DTO>(level3);
            var pointLess = db.Database.SqlQuery<bool>(string.Format("SELECT IsPointLess FROM ParLevel3 WHERE Id = {0}", level3Dto.Id)).FirstOrDefault();

            var AllowNA = db.Database.SqlQuery<bool>(string.Format("SELECT AllowNA FROM ParLevel3 WHERE Id = {0}", level3.Id)).FirstOrDefault();
            level3Dto.AllowNA = AllowNA;

            level3Dto.IsPointLess = pointLess;
            var valueCampoCalcOutro = db.Database.SqlQuery<ParLevel3Value_OuterListDTO>(string.Format(@"SELECT * FROM ParLevel3Value_Outer WHERE Parlevel3_Id = {0} AND IsActive = 1 AND (OuterEmpresa_Id = {1} OR OuterEmpresa_Id = -1)", level3.Id, parcompany)).ToList();

            level3Dto.ParLevel3Value_OuterList = valueCampoCalcOutro;
            level3Dto.ParLevel3Value_OuterListGrouped = valueCampoCalcOutro.GroupBy(r => r.ParCompany_Id);

            foreach (var bin in level3.ParLevel3Value)
            {
                if (bin.ParLevel3BoolTrue_Id > 0 && bin.ParLevel3BoolTrue != null)
                    level3Dto.ParLevel3Value.FirstOrDefault(r => r.Id == bin.Id).ParLevel3BoolTrue = Mapper.Map<ParLevel3BoolTrueDTO>(bin.ParLevel3BoolTrue);
                if (bin.ParLevel3BoolFalse_Id > 0 && bin.ParLevel3BoolFalse != null)
                    level3Dto.ParLevel3Value.FirstOrDefault(r => r.Id == bin.Id).ParLevel3BoolFalse = Mapper.Map<ParLevel3BoolFalseDTO>(bin.ParLevel3BoolFalse);
            }
            listaLevel3.Add(level3Dto);
        }
    }
}
