using Dominio;
using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace PlanoDeAcaoMVC.Controllers
{

    public class BaseController : Controller
    {
        private string _userSgqRoles;

        public BaseController()
        {

        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {

            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");

            try
            {

                System.Resources.ResourceManager resourceManager = ResourcesPA.Resource.ResourceManager;

                ViewBag.Resources = resourceManager.GetResourceSet(
                    Thread.CurrentThread.CurrentUICulture, true, false).Cast<DictionaryEntry>();

            }
            catch (Exception ex)
            {
            }

            base.Initialize(requestContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.IsAdmin = false;

            var cookie = System.Web.HttpContext.Current.Request.Cookies["webControlCookie"];

            if (cookie != null)
            {

                var userId = 0;

                if (!string.IsNullOrEmpty(cookie.Values["userId"]))
                {
                    int.TryParse(cookie.Values["userId"].ToString(), out userId);
                }

                var userlogado = new UserSgq();

                using (var db = new SgqDbDevEntities())
                {
                    userlogado = db.UserSgq.Find(userId);
                }

                if (!string.IsNullOrEmpty(userlogado.Role))
                {
                    _userSgqRoles = userlogado.Role.ToString();
                    filterContext.Controller.ViewBag.IsAdmin = VerificarRole("Admin");
                }

            }

            base.OnActionExecuting(filterContext);
        }

        protected bool VerificarRole(string role)
        {
            return _userSgqRoles.ToLowerInvariant().Contains(role.ToLowerInvariant());
        }
    }
}