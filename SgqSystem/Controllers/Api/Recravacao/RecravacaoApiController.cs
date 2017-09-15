using Dominio;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace SgqSystem.Controllers.Api
{
    public class RecravacaoApiController : BaseApiController
    {
        private SgqDbDevEntities db;
        private List<string> errors;
        private string mensagemSucesso;

        public RecravacaoApiController()
        {
            db = new SgqDbDevEntities();
            errors = new List<string>();
            mensagemSucesso = string.Empty;
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
                int IdLinha = int.Parse(dados["Id"]);
                int IdCompany = int.Parse(dados["ParCompany_Id"]);
                //var IdLinha = int.Parse(((JValue)(((JContainer)linha).First.FirstOrDefault())).Value.ToString());
                var existente = db.Database.SqlQuery<int>(string.Format("SELECT Id from RecravacaoJson WHERE Linha_Id = {0} AND ParCompany_Id = {1}", IdLinha, IdCompany)).FirstOrDefault();

                if (existente > 0)
                {
                    var queryUpdate = string.Format("UPDATE RecravacaoJson SET ObjectRecravacaoJson = N'{0}' WHERE Id = {1}", linhaStringFormatada, existente);
                    var queryUpdateAlterDate = string.Format("UPDATE RecravacaoJson SET AlterDate = {0} WHERE Id = {1}", "GETDATE()", existente);
                    db.Database.ExecuteSqlCommand(queryUpdate);
                    db.Database.ExecuteSqlCommand(queryUpdateAlterDate);
                }
                else
                {
                    var queryInsert = string.Format("INSERT INTO RecravacaoJson (UserSgqId, ParCompany_Id, Linha_Id, ObjectRecravacaoJson) \n \t VALUES \n \t" +
                        "\n ({0}, \n {1}, \n {2}, \n N'{3} \n ')", "1", IdCompany, IdLinha, linhaStringFormatada);
                    var Id = int.Parse(db.Database.SqlQuery<decimal>(queryInsert + " SELECT SCOPE_IDENTITY()").FirstOrDefault().ToString());
                }

                //Post Save
                mensagemSucesso = "Registro atualizado";

            }
            catch (Exception e)
            {
                errors.Add("Não foi possível inserir o Usuário, favor entrar em contato com o Suporte do Sistema, descrição do problema: " + e.Message);
            }

            if (errors.Count() > 0)
                return Request.CreateResponse(HttpStatusCode.OK, new { errors });

            return Request.CreateResponse(HttpStatusCode.OK, new { resposta = mensagemSucesso, model = Request.Content.ReadAsStringAsync().Result });

        }

        // GET: api/RecravacaoApi
        public HttpResponseMessage Get(int Company)
        {
            var requestResults = Request.Content.ReadAsStringAsync().Result;
            var paramsFromRequest = ToDynamic(Request.Content.ReadAsStringAsync().Result);
            var query = string.Format("SELECT * FROM RecravacaoJson WHERE ParCompany_Id = {0} ORDER BY Id DESC", Company);
            var results = QueryNinja(db, query);
            return Request.CreateResponse(HttpStatusCode.OK, new { resposta = "Dados Recuperados", model = results });
        }


    }
}
