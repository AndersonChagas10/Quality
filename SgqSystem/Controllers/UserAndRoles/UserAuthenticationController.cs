using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using Helper;
using SgqSystem.ViewModels;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Api
{
    public class UserAuthenticationController : BaseController
    {

        private readonly IUserDomain _userDomain;
        private readonly IBaseDomain<UserSgq, UserDTO> _userBaseDomain;
        private IBaseDomain<UserSgq, UserSgqDTO> _userSgqDomain;

        public UserAuthenticationController(IUserDomain userDomain, IBaseDomain<UserSgq, UserDTO> userBaseDomain, IBaseDomain<UserSgq, UserSgqDTO> userSgqDomain
            /*,IBaseDomain<EmailContent, EmailContentDTO> emailContent*/)
        {
            _userBaseDomain = userBaseDomain;
            _userDomain = userDomain;
            _userSgqDomain = userSgqDomain;
            // _emailContent = emailContent;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult LogIn()
        {
            #region Redireciona se for a configuração inicial do sistema*/
            SgqDbDevEntities db = new SgqDbDevEntities();
            if (!db.UserSgq.Any())
            {
                return RedirectToAction("Index", "FirstConfig");
            }
            #endregion

            HttpCookie currentUserCookie = Request.Cookies["webControlCookie"];
            if (currentUserCookie != null)
                return RedirectToAction("Index", "Home");

            ExpireCookie();
            return View(new UserViewModel() { IsWeb = true });
        }

        [HttpPost]
        [CustomAuthorize(isLogin: true)]
        public ActionResult LogIn(UserViewModel user)
        {

            var isAuthorized = _userDomain.AuthenticationLogin(user);

            if (isAuthorized.Retorno.IsNotNull())
            {
                CreateCookie(isAuthorized);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", ""/*isAuthorized.Mensagem*/);
                user.ErrorList = isAuthorized.MensagemExcecao + isAuthorized.StackTrace;
            }

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

        [HttpPost]
        public void LogOutCookie()
        {
            // clear cookies
            ExpireCookie();
        }



        [HttpGet]
        public ActionResult KeepAlive(int id)
        {
            var isAuthorized = _userBaseDomain.GetByIdNoLazyLoad(id);
            CreateCookieFromUserDTO(isAuthorized);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public ActionResult PasswordRecovery()
        {
            return View("PasswordRecovery");
        }

        [HttpGet]
        public JsonResult BuscaEmail(string nome)
        {
            var dado = _userDomain.GetByName2(nome).Retorno;

            return Json(dado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool enviaEmail(int idUser)
        {
            UserSgqDTO user = _userSgqDomain.GetById(idUser);
            string hash = Guard.DecryptStringAES(user.Password);
            EmailContent email = new EmailContent();
            email.To = user.Email;
            email.Subject = "Recuperação de Senha Sgq";
            email.Body = "Seu usuário: " + user.Name + "\n " + "sua Senha: " + hash;
            email.AddDate = DateTime.Now;
            email.Project = "SGQApp";
            email.IsBodyHtml = false;
            using (var db = new SgqDbDevEntities())
            {
                db.EmailContent.Add(email);
                var ret = db.SaveChanges();
                if (ret > 0)
                    return true;
                else
                    return false;
            }
        }
    }
}