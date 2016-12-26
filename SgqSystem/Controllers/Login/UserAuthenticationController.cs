using Dominio.Interfaces.Services;
using DTO.Helpers;
using Helper;
using SgqSystem.ViewModels;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Api
{
    [HandleController()]
    public class UserAuthenticationController : BaseController
    {

        private readonly IUserDomain _userDomain;

        public UserAuthenticationController(IUserDomain userDomain)
        {
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
            //create a cookie
            HttpCookie myCookie = new HttpCookie("webControlCookie");

            //Add key-values in the cookie
            myCookie.Values.Add("userId", isAuthorized.Retorno.Id.ToString());
            myCookie.Values.Add("userName", isAuthorized.Retorno.Name);
            if (isAuthorized.Retorno.Role != null)
                myCookie.Values.Add("roles", isAuthorized.Retorno.Role.Replace(';', ',').ToString());//"admin, teste, operacional, 3666,344, 43434,...."
            else
                myCookie.Values.Add("roles", "");

            if (isAuthorized.Retorno.ParCompanyXUserSgq != null)
                if (isAuthorized.Retorno.ParCompanyXUserSgq.Any(r => r.Role != null))
                    myCookie.Values.Add("rolesCompany", string.Join(",", isAuthorized.Retorno.ParCompanyXUserSgq.Select(n => n.Role).Distinct().ToArray()));
            else
                myCookie.Values.Add("rolesCompany", "");

            //set cookie expiry date-time. Made it to last for next 12 hours.
            myCookie.Expires = DateTime.Now.AddMinutes(30);

            //Most important, write the cookie to client.
            Response.Cookies.Add(myCookie);
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

    }
}