using AutoMapper;
using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO.Params;
using Newtonsoft.Json.Linq;
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
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.AutoDetectChangesEnabled = false;
        }

        // GET: api/RecravacaoLinhaApi
        /// <summary>
        /// Chamado quando usuário requisita Indicadores existentes para recravação, (param: Company)
        /// Chamado quando usuário requisita linhas de um indicador (param: Company e level1Id)
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="level1Id"></param>
        /// <param name="linhaId"></param>
        /// <returns></returns>
        public HttpResponseMessage Get(int companyId, int level1Id, int linhaId)
        {
            if (level1Id <= 0)
            {
                var level1List = db.ParLevel1.Where(r => r.IsActive && r.IsRecravacao == true).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, new { resposta = "Busca de Indicadores Concluída", model = level1List });
            }
            else
            {
                var queryRecravacaoJson = string.Format("SELECT * FROM RecravacaoJson WHERE Linha_Id = {0} AND ParCompany_Id = {1} AND ParLevel1_Id = {2} AND SalvoParaInserirNovaColeta IS NULL AND ISACTIVE = 1 ORDER BY Id DESC", linhaId, companyId, level1Id);
                var recravacoes = QueryNinja(db, queryRecravacaoJson);

                var query = string.Format(@"SELECT * FROM ParRecravacao_Linhas WHERE ParCompany_Id = {0} 
                        and ParLevel2_Id in (SELECT DISTINCT(parlevel2_Id) FROM PARLEVEL2Level1 where parlevel1_Id = {1} AND isactive = 1)", companyId, level1Id);
                var listLinhasDoLevel1 = QueryNinja(db, query).ToList();

                foreach (var linha in listLinhasDoLevel1)
                {
                    DateTime? ultimaLataRetirada = null;

                    int id = linha.GetValue("Id").Value<int>();
                    int horaVerificacao = 2;

                    int? recravacaoId = recravacoes?.FirstOrDefault()?.GetValue("Id").Value<int>();

                    var recravacaoJson = recravacoes.FirstOrDefault();

                    if (recravacaoId != null)
                    {
                        List<JObject> latas = QueryNinja(db, string.Format("SELECT * FROM RecravacaoLataJson where RecravacaoJson_Id = {0}", recravacaoId)).ToList();

                        horaVerificacao = JObject.Parse(recravacaoJson).GetValue("HoraVerificacao") == null ? 2 :
                            JObject.Parse(recravacaoJson).GetValue("HoraVerificacao").Value<int>();

                        foreach (var lata in latas)
                        {
                            DateTime? dataRetirada = String.IsNullOrEmpty((lata)?.GetValue("HoraDaRetiradaDaLata")?.ToString()) ? null :
                                    ((JObject)lata).GetValue("HoraDaRetiradaDaLata").Value<DateTime?>();

                            if (dataRetirada != null)
                            {
                                ultimaLataRetirada = ultimaLataRetirada == null
                                    ? dataRetirada : (dataRetirada > ultimaLataRetirada ? dataRetirada : ultimaLataRetirada);
                            }
                        }
                    }

                    ultimaLataRetirada = ultimaLataRetirada?.AddHours(-2);

                    linha["UltimaLataRetirada"] = ultimaLataRetirada?.ToString("dd/MM/yyyy HH:mm");
                    linha["HoraVerificacao"] = horaVerificacao;

                }

                return Request.CreateResponse(HttpStatusCode.OK, new { resposta = "Busca de Linhas Concluída", model = listLinhasDoLevel1 });
            }
        }

        // GET: api/RecravacaoLinhaApi
        /// <summary>
        /// Chamado quando usuário requisita uma linha (clica em uma linha) ou requisita nova coleta / atualização de parametrização
        /// , retorna parametrização da linha utilizando vinculos entre level1 , level2 , parLinhas, e ParLevel3.
        /// Fluxo:
        ///     Busca Parametrização de ParLevel3 vinculados a Indicador, Monitoramento, Linha e Lata
        ///     Busca ParLevel3Value Destes ParLevel3 vinculados
        ///     Insere ParLevel3Value no model da recravação LATA
        ///     Model (LATA) é devolvido pela requisição.
        /// </summary>
        /// <param name="Company"></param>
        /// <returns></returns>
        public HttpResponseMessage Post(JObject linha)
        {
            var id = linha["Id"].ToString();
            var parlevel2_Id = linha["ParLevel2_Id"].ToString();
            var parcompany = linha["ParCompany_Id"].ToString();
            var level1Ids = string.Join(",", db.Database.SqlQuery<ParLevel1DTO>("SELECT Id, Name From Parlevel1 WHERE IsRecravacao = 1").ToList().Select(r => r.Id).ToList());
            var queryLinhaPorCompanyELevel2 = string.Format(" SELECT * FROM ParRecravacao_Linhas WHERE ParCompany_Id = {0} AND ParLevel2_Id is not null AND Id  = {1}", parcompany, id);
            var queryTipoLataPorparRecravacao_TypeLata_Id = "SELECT * FROM ParRecravacao_TipoLata WHERE Id = {0} AND IsActive = 1";
            var results = QueryNinja(db, queryLinhaPorCompanyELevel2).ToList();

            var queryRecravacaoJson = string.Format("SELECT * FROM RecravacaoJson WHERE Linha_Id = {0} AND ParCompany_Id = {1} AND ParLevel1_Id IN ({2}) AND SalvoParaInserirNovaColeta IS NULL AND ISACTIVE = 1 ORDER BY Id DESC", id, parcompany, level1Ids);
            var recravacoes = QueryNinja(db, queryRecravacaoJson);

            foreach (var linhaDb in results)
            {
                var listaLevel3 = new List<ParLevel3DTO>();
                string queryVinculoLevel21, queryVinculoLevel32, queryVinculoLevel321;
                List<JObject> hasVinculoLevel21, hasVinculoLevel32, hasVinculoLevel321;
                List<JObject> hasVinculoLevel21TodasUnidade, hasVinculoLevel32TodasUnidade, hasVinculoLevel321TodasUnidade;

                CriaQueryesParaParLevel3ValuesDaLataDaLinha(parlevel2_Id, parcompany, level1Ids, out queryVinculoLevel21, out queryVinculoLevel32, out queryVinculoLevel321, out hasVinculoLevel21, out hasVinculoLevel32, out hasVinculoLevel321);

                InsereParLevel3ValueNaLata(parcompany, listaLevel3, hasVinculoLevel21, hasVinculoLevel32, hasVinculoLevel321);

                CriaQueryesParaParLevel3OuterValuesDasLatasDaLinha(parlevel2_Id, parcompany, level1Ids, out queryVinculoLevel21, out queryVinculoLevel32, out queryVinculoLevel321, out hasVinculoLevel21TodasUnidade, out hasVinculoLevel32TodasUnidade, out hasVinculoLevel321TodasUnidade);

                InsereParLevel3OuterValueNaLata(parcompany, listaLevel3, hasVinculoLevel21TodasUnidade, hasVinculoLevel32TodasUnidade, hasVinculoLevel321TodasUnidade);

                DateTime? ultimaLataRetirada = null;
                
                int horaVerificacao = 2;

                String recravacaoJson = recravacoes
                    .Where(r => r.GetValue("Linha_Id").Value<int>() == Int32.Parse(id))
                    .FirstOrDefault()?.GetValue("ObjectRecravacaoJson").Value<String>();

                //if (recravacaoJson != null)
                //{
                //    JArray latas = JObject.Parse(recravacaoJson).GetValue("latas").Value<JArray>();

                //    horaVerificacao = JObject.Parse(recravacaoJson).GetValue("HoraVerificacao") == null ? 2 :
                //        JObject.Parse(recravacaoJson).GetValue("HoraVerificacao").Value<int>();

                //    foreach (var lata in latas)
                //    {
                //        DateTime? dataRetirada = String.IsNullOrEmpty(((JObject)lata)?.GetValue("HoraDaRetiradaDaLata")?.ToString()) ? null :
                //                ((JObject)lata).GetValue("HoraDaRetiradaDaLata").Value<DateTime?>();

                //        if (dataRetirada != null)
                //        {
                //            ultimaLataRetirada = ultimaLataRetirada == null
                //                ? dataRetirada : (dataRetirada > ultimaLataRetirada ? dataRetirada : ultimaLataRetirada);
                //        }
                //    }
                //}

                ultimaLataRetirada = ultimaLataRetirada?.AddHours(-2);

                linha["UltimaLataRetirada"] = ultimaLataRetirada?.ToString("dd/MM/yyyy HH:mm"); ;
                linha["HoraVerificacao"] = horaVerificacao;
                linhaDb["TipoDeLata"] = QueryNinja(db, string.Format(queryTipoLataPorparRecravacao_TypeLata_Id, int.Parse(linhaDb["ParRecravacao_TypeLata_Id"].ToString()))).FirstOrDefault();
                linhaDb["ListParlevel3"] = JToken.FromObject(listaLevel3, new Newtonsoft.Json.JsonSerializer { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { resposta = "Busca de linhas concluída", model = results });
        }
        
        #region Aux Private

        private void GetParLevel3VinculadosLata(string parcompany, List<ParLevel3DTO> listaLevel3, JObject vinculoLevel32)
        {
            var idLevel3 = int.Parse(vinculoLevel32["ParLevel3_Id"].ToString());
            var level3 = db.ParLevel3.Include("ParLevel3Value").Include("ParLevel3Value.ParMeasurementUnit").Include("ParLevel3Value.ParLevel3BoolFalse").Include("ParLevel3Value.ParLevel3BoolTrue").FirstOrDefault(r => r.Id == idLevel3);

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

        private void InsereParLevel3ValueNaLata(string parcompany, List<ParLevel3DTO> listaLevel3, List<JObject> hasVinculoLevel21, List<JObject> hasVinculoLevel32, List<JObject> hasVinculoLevel321)
        {
            if (hasVinculoLevel21.Count() > 0 && hasVinculoLevel321.Count() > 0 && hasVinculoLevel32.Count() > 0)
                foreach (var vinculoLevel32 in hasVinculoLevel32)
                    GetParLevel3VinculadosLata(parcompany, listaLevel3, vinculoLevel32);
        }

        private void InsereParLevel3OuterValueNaLata(string parcompany, List<ParLevel3DTO> listaLevel3, List<JObject> hasVinculoLevel21TodasUnidade, List<JObject> hasVinculoLevel32TodasUnidade, List<JObject> hasVinculoLevel321TodasUnidade)
        {
            if (hasVinculoLevel21TodasUnidade.Count() > 0 && hasVinculoLevel32TodasUnidade.Count() > 0 && hasVinculoLevel321TodasUnidade.Count() > 0)
                foreach (var vinculoLevel32TodaUnidades in hasVinculoLevel32TodasUnidade)
                    GetParLevel3VinculadosLata(parcompany, listaLevel3, vinculoLevel32TodaUnidades);
        }

        private void CriaQueryesParaParLevel3OuterValuesDasLatasDaLinha(string parlevel2_Id, string parcompany, string level1Ids, out string queryVinculoLevel21, out string queryVinculoLevel32, out string queryVinculoLevel321, out List<JObject> hasVinculoLevel21TodasUnidade, out List<JObject> hasVinculoLevel32TodasUnidade, out List<JObject> hasVinculoLevel321TodasUnidade)
        {
            queryVinculoLevel21 = $@"SELECT * FROM ParLevel2Level1 WHERE ParLevel1_Id in ({level1Ids}) AND ParLevel2_Id = {parlevel2_Id} AND ParCompany_Id IS NULL AND  IsActive = 1";

            queryVinculoLevel32 = $@"SELECT * FROM ParLevel3Level2 WHERE ParLevel2_Id = {parlevel2_Id} AND (ParCompany_Id IS NULL) AND IsActive = 1
                    AND ParLevel3_Id NOT IN  (
                        SELECT DISTINCT(ParLevel3_Id) FROM ParLevel3Level2 WHERE ParLevel2_Id = {parlevel2_Id} AND (ParCompany_Id = {parcompany}) AND IsActive = 1)
                    --AND ParLevel3_Id IN  (
                    --    SELECT DISTINCT(ParLevel3_Id) FROM ParLevel3Value WHERE (ParCompany_Id IN ({parcompany}) OR ParCompany_Id IS NULL)  AND IsActive = 1)";

            queryVinculoLevel321 = $@"SELECT * FROM parlevel3level2Level1 
                    WHERE ParLevel1_Id in ({level1Ids}) 
                    AND ParLevel3Level2_Id IN (
	                        SELECT Id FROM ParLevel3Level2 WHERE ParLevel2_Id = {parlevel2_Id} AND (ParCompany_Id IS NULL) AND IsActive = 1
	                        AND ParLevel3_Id NOT IN  (
	                        SELECT DISTINCT(ParLevel3_Id) FROM ParLevel3Level2 WHERE ParLevel2_Id = {parlevel2_Id} AND (ParCompany_Id = {parcompany}) AND IsActive = 1)
	                    )";

            hasVinculoLevel21TodasUnidade = QueryNinja(db, queryVinculoLevel21);
            hasVinculoLevel32TodasUnidade = QueryNinja(db, queryVinculoLevel32);
            hasVinculoLevel321TodasUnidade = QueryNinja(db, queryVinculoLevel321);
        }

        private void CriaQueryesParaParLevel3ValuesDaLataDaLinha(string parlevel2_Id, string parcompany, string level1Ids, out string queryVinculoLevel21, out string queryVinculoLevel32, out string queryVinculoLevel321, out List<JObject> hasVinculoLevel21, out List<JObject> hasVinculoLevel32, out List<JObject> hasVinculoLevel321)
        {
            queryVinculoLevel21 = $@"SELECT * FROM ParLevel2Level1 WHERE ParLevel1_Id in ({level1Ids}) AND ParLevel2_Id = {parlevel2_Id} AND ParCompany_Id = {parcompany} AND IsActive = 1";
            queryVinculoLevel32 = $@"SELECT * FROM ParLevel3Level2 WHERE ParLevel2_Id = {parlevel2_Id} AND ParCompany_Id = {parcompany} AND IsActive = 1";
            queryVinculoLevel321 = $@"SELECT * FROM parlevel3level2Level1 WHERE ParLevel1_Id in ({level1Ids}) AND ParLevel3Level2_Id IN (select Id from ParLevel3Level2 WHERE ParLevel2_Id = {parlevel2_Id} AND ParCompany_Id = {parcompany} AND IsActive = 1)";
            hasVinculoLevel21 = QueryNinja(db, queryVinculoLevel21);
            hasVinculoLevel32 = QueryNinja(db, queryVinculoLevel32);
            hasVinculoLevel321 = QueryNinja(db, queryVinculoLevel321);
        }

        #endregion
    }
}
