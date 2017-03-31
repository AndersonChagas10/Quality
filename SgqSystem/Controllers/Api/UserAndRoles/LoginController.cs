using SgqSystem.Handlres;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data.SqlClient;
using System;

namespace SgqSystem.Controllers.Api.Login
{
    [HandleApi()]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/LoginApi")]
    public class LoginController : ApiController
    {
        [HttpGet]
        [Route("Logado")]
        public string Logado()
        {
            string mensagem = "noDataBase";
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    connection.Open();
                    return "onLine";
                }
            }
            catch (SqlException ex)
            {
               
                return mensagem;
            }
            catch (Exception ex)
            {
                return mensagem;
            }
        }
    }
}
