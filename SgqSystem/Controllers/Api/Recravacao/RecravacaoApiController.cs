using ADOFactory;
using Dominio;
using Newtonsoft.Json.Linq;
using SgqSystem.Controllers.Api.Recravacao;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace SgqSystem.Controllers.Api
{

    public class RecravacaoApiController : BaseApiController
    {
        private SgqDbDevEntities db;
        private RepoRecravacao<RecravacaoJson> repo;
        private List<string> errors;
        private string mensagemSucesso;

        public RecravacaoApiController()
        {
            db = new SgqDbDevEntities();
            errors = new List<string>();
            repo = new RepoRecravacao<RecravacaoJson>();
            mensagemSucesso = string.Empty;
        }

        // GET: api/RecravacaoApi
        public HttpResponseMessage Get(int companyId, int level1Id, int linhaId)
        {
            using (Factory factory = new Factory("DefaultConnection"))
            {
                var requestResults = Request.Content.ReadAsStringAsync().Result;
                var paramsFromRequest = ToDynamic(Request.Content.ReadAsStringAsync().Result);
                var query = string.Format("SELECT TOP 1* FROM RecravacaoJson WHERE ParCompany_Id = {0} AND ParLevel1_Id = {1} AND SalvoParaInserirNovaColeta IS NULL AND Linha_Id = {2} AND ISACTIVE = 1 ORDER BY Id DESC", companyId, level1Id, linhaId);
                var results = QueryNinja(db, query);
                var latasId = results.Count() > 0 ? QueryNinja(db, string.Format("SELECT Id from RecravacaoLataJson where RecravacaoJson_Id = {0}", results[0].GetValue("Id").ToString())) : null;
                var produtos = factory.SearchQuery<ReprocessoApiController.Produto>("SELECT * FROM Produto").ToList();
                var sugestoes = factory.SearchQuery<DTO.DTO.RecravacaoSugestaoDTO>("SELECT * FROM RecravacaoSugestao").ToList();
                return Request.CreateResponse(HttpStatusCode.OK,
                    new { resposta = "Dados Recuperados", model = results, produtos = produtos, sugestoes = sugestoes, latasId = latasId });
            }
        }

        public HttpResponseMessage Get(int recravacaoLataJsonId)
        {
            var query = string.Format(@"SELECT * FROM RecravacaoLataJson WHERE Id = {0}", recravacaoLataJsonId);
            var listRecravacaoJson = QueryNinja(db, query).FirstOrDefault();

            return Request.CreateResponse(HttpStatusCode.OK, new { resposta = "Busca", model = listRecravacaoJson });
        }

        // POST: api/RecravacaoApi
        public HttpResponseMessage Post(dynamic data)
        {
            ////Teste de erros não controlados
            //throw new Exception("teste", new Exception("INNER", new Exception("Inner 2")));
            var model = string.Empty;

            try
            {
                ////Teste de erros controlados
                //throw new Exception("teste", new Exception("INNER", new Exception("Inner 2")));
                //model = Request.Content.ReadAsStringAsync().Result;
                //System.Web.Script.Serialization.JavaScriptSerializer json_serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                //dynamic dados = json_serializer.DeserializeObject(model);
                var linha = data["linha"];
                var latas = data["latas"];

                var linhaStringFormatada = ToJson(linha);
                int idLinha = int.Parse(linha["Id"].ToString());
                int idCompany = int.Parse(linha["ParCompany_Id"].ToString());
                int parLevel1_Id = int.Parse(linha["ParLevel1_Id"].ToString());
                bool salvoParaInserirNovaColeta = false;
                bool isValidated = false;
                var existente = db.RecravacaoJson.FirstOrDefault(r => r.ParCompany_Id == idCompany && r.Linha_Id == idLinha && !isValidated && r.SalvoParaInserirNovaColeta == null)?.Id;

                int RecravacaoJsonId = 0;
                
                if(IsPropertyExist(linha, "isValidated"))
                    isValidated = linha["isValidated"];

                if (IsPropertyExist(linha, "SalvoParaInserirColeta"))
                    salvoParaInserirNovaColeta = linha["SalvoParaInserirColeta"];

                if (existente.GetValueOrDefault() > 0 && salvoParaInserirNovaColeta == false)
                    RecravacaoJsonId = Update(linhaStringFormatada, existente);
                else
                {
                    if (existente.GetValueOrDefault() > 0  && salvoParaInserirNovaColeta == true)
                        RecravacaoJsonId = UpdateRecravacaoJsonParaNovaColeta(linhaStringFormatada, existente.GetValueOrDefault());
                    else
                        RecravacaoJsonId = Save(linhaStringFormatada, idLinha, idCompany, parLevel1_Id);
                }

                if (isValidated)
                    RecravacaoJsonId = UpdateRecravacaoJsonFinalizaColetaValidada(linhaStringFormatada, existente.GetValueOrDefault());
                //Post Save
                mensagemSucesso = "Registro atualizado";

                SaveLatas(RecravacaoJsonId, latas);

            }
            catch (Exception e)
            {
                errors.Add("Não foi possível Salvas os Dados: " + e.Message);
            }

            if (errors.Count() > 0)
                return Request.CreateResponse(HttpStatusCode.OK, new { errors });

            //return Request.CreateResponse(HttpStatusCode.OK, new { resposta = mensagemSucesso, model = Request.Content.ReadAsStringAsync().Result });
            return Request.CreateResponse(HttpStatusCode.OK, new { resposta = mensagemSucesso });

        }
        
        private int UpdateRecravacaoJsonFinalizaColetaValidada(string linhaStringFormatada, int? existente)
        {
            var updateRecravacaoJsonFinalizaColetaValidada = db.RecravacaoJson.FirstOrDefault(r => r.Id == existente);
            updateRecravacaoJsonFinalizaColetaValidada.ObjectRecravacaoJson = linhaStringFormatada;
            updateRecravacaoJsonFinalizaColetaValidada.AlterDate = DateTime.Now;
            updateRecravacaoJsonFinalizaColetaValidada.isValidated = true;
            return repo.Save(updateRecravacaoJsonFinalizaColetaValidada).Id;
        }

        private int UpdateRecravacaoJsonParaNovaColeta(string linhaStringFormatada, int? existente)
        {
            var updateRecravacaoJsonNovaColeta = db.RecravacaoJson.FirstOrDefault(r => r.Id == existente);
            updateRecravacaoJsonNovaColeta.ObjectRecravacaoJson = linhaStringFormatada;
            updateRecravacaoJsonNovaColeta.AlterDate = DateTime.Now;
            updateRecravacaoJsonNovaColeta.SalvoParaInserirNovaColeta = existente;
            return repo.Save(updateRecravacaoJsonNovaColeta).Id;
        }

        private int Save(string linhaStringFormatada, int idLinha, int idCompany, int parLevel1_Id)
        {
            var newRecravacaoColeta = new RecravacaoJson()
            {
                AddDate = DateTime.Now,
                IsActive = true,
                UserSgqId = 1,
                ParCompany_Id = idCompany,
                Linha_Id = idLinha,
                ParLevel1_Id = parLevel1_Id,
                ObjectRecravacaoJson = linhaStringFormatada
            };
            return repo.Save(newRecravacaoColeta).Id;
        }

        private int Update(string linhaStringFormatada, int? existente)
        {
            var updateRecravacaoJson = db.RecravacaoJson.FirstOrDefault(r => r.Id == existente);
            updateRecravacaoJson.ObjectRecravacaoJson = linhaStringFormatada;
            updateRecravacaoJson.AlterDate = DateTime.Now;
            return repo.Save(updateRecravacaoJson).Id;
        }

        private void SaveLatas(int? RecravacaoJson_Id, dynamic latas)
        {

            Factory factory = new Factory("DefaultConnection");
            factory.ExecuteSql(string.Format("DELETE FROM RecravacaoLataJson WHERE RecravacaoJson_Id = {0};", RecravacaoJson_Id));
            foreach (dynamic lata in latas)
            {
                var lataStringFormatada = ToJson(lata);
                factory.ExecuteSql(
                    string.Format("INSERT INTO RecravacaoLataJson (RecravacaoJson_Id, AddDate, ObjectRecravacaoJson) VALUES ({0}, GETDATE(), '{1}')", RecravacaoJson_Id, lataStringFormatada));
            }
        }

        private void UpdateRecravacaoLataJson(int RecravacaoJson_Id, string LataJson)
        {
            Factory factory = new Factory("DefaultConnection");
            factory.ExecuteSql(
                string.Format("UPDATE RecravacaoLataJson SET AlterDate = GETDATE(), ObjectRecravacaoJson = '{1}' WHERE RecravacaoJson_Id = {0}", RecravacaoJson_Id, LataJson));
        }

        private bool IsPropertyExist(dynamic obj, string name)
        {
            try
            {
                return ((IDictionary<string, object>)obj).ContainsKey(name);
            }
            catch (Exception)
            {
                return false;
            }
            //if (obj is ExpandoObject)
            //    return ((IDictionary<string, object>)obj).ContainsKey(name);

            //return obj.GetType().GetProperty(name) != null;
        }

    }

    
}
