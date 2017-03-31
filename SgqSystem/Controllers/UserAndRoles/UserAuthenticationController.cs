using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using Helper;
using SgqSystem.ViewModels;
using System;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Api
{
    [HandleController()]
    public class UserAuthenticationController : BaseController
    {

        private readonly IUserDomain _userDomain;
        private readonly IBaseDomain<UserSgq, UserDTO> _userBaseDomain;

        public UserAuthenticationController(IUserDomain userDomain, IBaseDomain<UserSgq, UserDTO> userBaseDomain)
        {
            _userBaseDomain = userBaseDomain;
            _userDomain = userDomain;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult LogIn()
        {
            HttpCookie currentUserCookie = Request.Cookies["webControlCookie"];
            if (currentUserCookie != null)
                return RedirectToAction("Index", "Home");

            ExpireCookie();
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(UserViewModel user)
        {

            var isAuthorized = _userDomain.AuthenticationLogin(user);

            if (isAuthorized.Retorno.IsNotNull())
            {
                CreateCookie(isAuthorized);
                return RedirectToAction("Index", "Home");
            }
            else
                ModelState.AddModelError("", isAuthorized.Mensagem);

            return View(user);
        }

        private void CreateCookie(GenericReturn<DTO.DTO.UserDTO> isAuthorized)
        {
            CreateCookieFromUserDTO(isAuthorized.Retorno);
        }

        public ActionResult LogOut(UserViewModel user)
        {
            // clear cookies
            ExpireCookie();
            return RedirectToAction("LogIn", "UserAuthentication");
        }

        private void ExpireCookie()
        {
            HttpCookie currentUserCookie = Request.Cookies["webControlCookie"];
            if (currentUserCookie != null)
            {
                Response.Cookies.Remove("webControlCookie");
                currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                currentUserCookie.Value = null;
                Response.SetCookie(currentUserCookie);
            }

        }

        [HttpGet]
        public ActionResult KeepAlive(int id)
        {
            var isAuthorized = _userBaseDomain.GetByIdNoLazyLoad(id);
            CreateCookieFromUserDTO(isAuthorized);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

    }
}