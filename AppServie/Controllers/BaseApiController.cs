using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace AppServie.Api.Controllers
{
    public class CredenciaisSgq
    {
        public string Username { get; set; }
        public string Senha { get; set; }
    }

    public class BaseApiController : ApiController
    {
        public string token;

        // GET: BaseAPI
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            // Do some stuff
            base.Initialize(controllerContext);

            try
            {
                token = Request.Headers.GetValues("token").FirstOrDefault().ToString();
            }
            catch
            {

            }

            string language = "";
            try
            {
                language = Request.Headers.GetValues("lang").FirstOrDefault();
            }
            catch
            {

            }

            if (string.IsNullOrEmpty(language))
            {
                language = "pt-BR";
            }

            Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
        }

        protected void VerifyIfIsAuthorized()
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    throw new UnauthorizedAccessException("Acesso negado!");
                }
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException("Acesso negado!");
            }
        }

    }
}