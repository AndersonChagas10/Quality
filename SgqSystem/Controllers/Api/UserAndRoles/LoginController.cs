using SgqSystem.Handlres;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api.Login
{
    [HandleApi()]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/LoginApi")]
    public class LoginController : ApiController
    {
        [HttpGet]
        //[HttpPost]
        [Route("Logado")]
        public string Logado()
        {
            //string mensagem = "noDataBase";
            //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            //try
            //{
            //    using (SqlConnection connection = new SqlConnection(conexao))
            //    {
            //        connection.Open();
            return "onLine";
            //    }
            //}
            //catch (SqlException ex)
            //{

            //    return mensagem;
            //}
            //catch (Exception ex)
            //{
            //    return mensagem;
            //}
        }

        // Route /status to this controller

        /// <summary>
        /// Metodo CORRETO a ser utilizado para PING:
        ///     $.post("http://localhost/SgqSystem/api/LoginApi")
        ///     $.get("http://localhost/SgqSystem/api/LoginApi")
        /// 
        /// </summary>
        /// <returns></returns>
        //[HttpGet] // accept get
        //[HttpPost] // accept post
        //[Route("Logado")] // route default request to this method.
        //public IHttpActionResult Get()
        //{
        //    return Ok();
        //}

    }
}
