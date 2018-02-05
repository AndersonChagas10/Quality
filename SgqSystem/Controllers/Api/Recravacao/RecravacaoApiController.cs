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
            var requestResults = Request.Content.ReadAsStringAsync().Result;
            var paramsFromRequest = ToDynamic(Request.Content.ReadAsStringAsync().Result);
            var query = string.Format("SELECT TOP 1* FROM RecravacaoJson WHERE ParCompany_Id = {0} AND ParLevel1_Id = {1} AND SalvoParaInserirNovaColeta IS NULL AND Linha_Id = {2} AND ISACTIVE = 1 ORDER BY Id DESC", companyId, level1Id, linhaId);
            var results = QueryNinja(db, query);
            var produtos = db.Database.SqlQuery<ReprocessoApiController.Produto>("SELECT * FROM Produto").ToList();
            var sugestoes = db.Database.SqlQuery<DTO.DTO.RecravacaoSugestaoDTO>("SELECT * FROM RecravacaoSugestao").ToList(); 
            return Request.CreateResponse(HttpStatusCode.OK, 
                new { resposta = "Dados Recuperados", model = results, produtos = produtos, sugestoes = sugestoes });
        }



        // POST: api/RecravacaoApi
        public HttpResponseMessage Post()
        {
            ////Teste de erros não controlados
            //throw new Exception("teste", new Exception("INNER", new Exception("Inner 2")));
            var model = string.Empty;

            try
            {
                ////Teste de erros controlados
                //throw new Exception("teste", new Exception("INNER", new Exception("Inner 2")));
                model = Request.Content.ReadAsStringAsync().Result;
                System.Web.Script.Serialization.JavaScriptSerializer json_serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                dynamic dados = json_serializer.DeserializeObject(model);
                var linha = ToDynamic(model);
                var linhaStringFormatada = ToJson(linha);
                int idLinha = int.Parse(dados["Id"]);
                int idCompany = int.Parse(dados["ParCompany_Id"]);
                int parLevel1_Id = int.Parse(dados["ParLevel1_Id"]);
                bool salvoParaInserirNovaColeta = false;
                bool isValidated = false;
                var existente = db.RecravacaoJson.FirstOrDefault(r => r.ParCompany_Id == idCompany && r.Linha_Id == idLinha && !isValidated && r.SalvoParaInserirNovaColeta == null)?.Id;
                
                if(IsPropertyExist(dados, "isValidated"))
                    isValidated = dados["isValidated"];

                if (IsPropertyExist(dados, "SalvoParaInserirColeta"))
                    salvoParaInserirNovaColeta = dados["SalvoParaInserirColeta"];

                if (existente.GetValueOrDefault() > 0 && salvoParaInserirNovaColeta == false)
                    Update(linhaStringFormatada, existente);
                else
                {
                    if (existente.GetValueOrDefault() > 0  && salvoParaInserirNovaColeta == true)
                        UpdateRecravacaoJsonParaNovaColeta(linhaStringFormatada, existente.GetValueOrDefault());
                    else
                        Save(linhaStringFormatada, idLinha, idCompany, parLevel1_Id);
                }

                if (isValidated)
                    UpdateRecravacaoJsonFinalizaColetaValidada(linhaStringFormatada, existente.GetValueOrDefault());
                //Post Save
                mensagemSucesso = "Registro atualizado";

                SaveLatas(existente, dados["latas"]);

            }
            catch (Exception e)
            {
                errors.Add("Não foi possível Salvas os Dados: " + e.Message);
            }

            if (errors.Count() > 0)
                return Request.CreateResponse(HttpStatusCode.OK, new { errors });

            return Request.CreateResponse(HttpStatusCode.OK, new { resposta = mensagemSucesso, model = Request.Content.ReadAsStringAsync().Result });

        }
        
        private void UpdateRecravacaoJsonFinalizaColetaValidada(string linhaStringFormatada, int? existente)
        {
            var updateRecravacaoJsonFinalizaColetaValidada = db.RecravacaoJson.FirstOrDefault(r => r.Id == existente);
            updateRecravacaoJsonFinalizaColetaValidada.ObjectRecravacaoJson = linhaStringFormatada;
            updateRecravacaoJsonFinalizaColetaValidada.AlterDate = DateTime.Now;
            updateRecravacaoJsonFinalizaColetaValidada.isValidated = true;
            repo.Save(updateRecravacaoJsonFinalizaColetaValidada);
        }

        private void UpdateRecravacaoJsonParaNovaColeta(string linhaStringFormatada, int? existente)
        {
            var updateRecravacaoJsonNovaColeta = db.RecravacaoJson.FirstOrDefault(r => r.Id == existente);
            updateRecravacaoJsonNovaColeta.ObjectRecravacaoJson = linhaStringFormatada;
            updateRecravacaoJsonNovaColeta.AlterDate = DateTime.Now;
            updateRecravacaoJsonNovaColeta.SalvoParaInserirNovaColeta = existente;
            repo.Save(updateRecravacaoJsonNovaColeta);
        }

        private void Save(string linhaStringFormatada, int idLinha, int idCompany, int parLevel1_Id)
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
            repo.Save(newRecravacaoColeta);
        }

        private void Update(string linhaStringFormatada, int? existente)
        {
            var updateRecravacaoJson = db.RecravacaoJson.FirstOrDefault(r => r.Id == existente);
            updateRecravacaoJson.ObjectRecravacaoJson = linhaStringFormatada;
            updateRecravacaoJson.AlterDate = DateTime.Now;
            repo.Save(updateRecravacaoJson);
        }

        private void SaveLatas(int? RecravacaoJson_Id, dynamic latas)
        {

            Factory factory = new Factory("DbContextSgqEUA");
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
            Factory factory = new Factory("SgqDbDevEntities");
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
