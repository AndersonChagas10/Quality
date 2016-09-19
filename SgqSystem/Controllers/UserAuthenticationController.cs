using Application.Interface;
using DTO.Helpers;
using SgqSystem.Secirity;
using SgqSystem.ViewModels;
using System.Web.Mvc;
using System.Web.Security;

namespace SgqSystem.Controllers.Api
{
    public class UserAuthenticationController : Controller
    {

        private readonly IUserApp _userApp;

        public UserAuthenticationController(IUserApp userApp)
        {
            _userApp = userApp;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult LogIn()
        {
            if(!(string.IsNullOrEmpty(SessionPersister.Username)))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]  
        public ActionResult LogIn(UserViewModel user)
        {
            var isAuthorized = _userApp.AuthenticationLogin(user);
            if (isAuthorized.Retorno.IsNotNull())
            {
                SessionPersister.Username = isAuthorized.Retorno.Name;
                return RedirectToAction("Index", "Home");
            }
            else
                ModelState.AddModelError("", isAuthorized.Mensagem);

            return View(user);
        }

        public ActionResult LogOut(UserViewModel user)
        {
            SessionPersister.LogOut();
            return RedirectToAction("LogIn", "UserAuthentication");
        }

        //[HttpGet]
        //public ActionResult Register()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Register(UserViewModel user)
        //{
        //    return View();
        //}

        //public void RenewCurrentUser()
        //{
        //    System.Web.HttpCookie authCookie =
        //        System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
        //    if (authCookie != null)
        //    {
        //        FormsAuthenticationTicket authTicket = null;
        //        authTicket = FormsAuthentication.Decrypt(authCookie.Value);

        //        if (authTicket != null && !authTicket.Expired)
        //        {
        //            FormsAuthenticationTicket newAuthTicket = authTicket;

        //            if (FormsAuthentication.SlidingExpiration)
        //            {
        //                newAuthTicket = FormsAuthentication.RenewTicketIfOld(authTicket);
        //            }
        //            string userData = newAuthTicket.UserData;
        //            string[] roles = userData.Split(',');

        //            System.Web.HttpContext.Current.User =
        //                new System.Security.Principal.GenericPrincipal(new FormsIdentity(newAuthTicket), roles);
        //        }
        //    }
        //}
    }
}