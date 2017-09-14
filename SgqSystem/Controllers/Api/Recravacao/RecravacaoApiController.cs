using Dominio;
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

                //Teste de erros controlados
                //throw new Exception("teste", new Exception("INNER", new Exception("Inner 2")));

                model = Request.Content.ReadAsStringAsync().Result;
                var teste = ToDynamic(model);
                var teste2 = ToJson(teste);
                //var objJsonString = ToJson(value);
                var query = string.Format("INSERT INTO RecravacaoJson (UserSgqId, ParCompany_Id, ObjectRecravacaoJson) \n \t VALUES \n \t" +
                    "\n ({0}, \n {1}, \n N'{2} \n ')", "1", "14", teste2);

                var Id = int.Parse(db.Database.SqlQuery<decimal>(query).FirstOrDefault().ToString());

                //Post Save
                mensagemSucesso = "Registro atualizado";

            }
            catch (Exception e)
            {
                errors.Add("Não foi possível inserir o Usuário, favor entrar em contato com o Suporte do Sistema, descrição do problema: " + e.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { resposta = mensagemSucesso, model = Request.Content.ReadAsStringAsync().Result });

        }

        // GET: api/RecravacaoApi
        public HttpResponseMessage Get()
        {
            var paramsFromRequest = ToDynamic(Request.Content.ReadAsStringAsync().Result);
            var query = "SELECT * FROM RecravacaoJson ORDER BY Id DESC";
            var results = QueryNinja(db, query).FirstOrDefault();
            return Request.CreateResponse(HttpStatusCode.OK, new { resposta = mensagemSucesso, model = results });
        }
     
       
    }
}
