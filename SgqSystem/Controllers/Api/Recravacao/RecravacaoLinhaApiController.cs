using Dominio;
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

        public RecravacaoLinhaApiController()
        {
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
            return Request.CreateResponse(HttpStatusCode.OK, new { resposta = "Recuperados dados das Linhas", model = results });
        }
     
       
    }
}
