using Application.Interface;
using DTO.DTO;
using DTO.Helpers;
using SgqSystem.Secirity;
using SgqSystem.ViewModels;
using System.Web.Mvc;

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
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserViewModel user)
        {
            return View();
        }

    }
}