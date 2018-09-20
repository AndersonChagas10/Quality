using Dominio;
using DTO;
using DTO.DTO;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Helper;
using System.Collections;
using AutoMapper;
using DTO.DTO.Params;
using System.Collections.Generic;

namespace SgqSystem.Controllers
{
    [HandleController]
    public class BaseController : Controller
    {

        public static int VariavelEstatica = 1;

        public BaseController()
        {

            ViewBag.UrlDataCollect = GlobalConfig.urlAppColleta;

            using (var db = new SgqDbDevEntities())
            {

                ViewBag.Clusters = Mapper.Map<IEnumerable<ParClusterDTO>>(db.ParCluster.Where(r => r.IsActive == true));

                ViewBag.Modulos = Mapper.Map<IEnumerable<ParClusterGroupDTO>>(db.ParClusterGroup.Where(r => r.IsActive == true));

                //ViewBag.ItensMenu = Mapper.Map<IEnumerable<ItemMenuDTO>>(db.ItemMenu.Where(r => r.IsActive == true && r.ItemMenu_Id != null));
            }

            var listaURLPA = GetWebConfigList("URL_PA");

            ViewBag.urlRootPa = listaURLPA[0];
            ViewBag.urlRootPaFTA = listaURLPA[1];
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {

            HttpCookie languageCookie = System.Web.HttpContext.Current.Request.Cookies["Language"];

            if (languageCookie != null)
            {
                if (languageCookie.Value == "en")
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("");
                }
                else
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(languageCookie.Value);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCookie.Value);
                }
            }
            else
            {
                if (GlobalConfig.LanguageBrasil)
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
                }
                else if (GlobalConfig.LanguageEUA)
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("");
                }
            }

            try
            {

                System.Resources.ResourceManager resourceManager = Resources.Resource.ResourceManager;

                ViewBag.Resources = resourceManager.GetResourceSet(
                    Thread.CurrentThread.CurrentUICulture, true, false).Cast<DictionaryEntry>();

            }
            catch (Exception)
            {
            }

            SetItensMenu();

            base.Initialize(requestContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var webControlCookie = System.Web.HttpContext.Current.Request.Cookies["webControlCookie"];

            using (var db = new SgqDbDevEntities())
            {
                var currentRoute = this.ControllerContext.RouteData.Values.Values.ToList();
                var controller = currentRoute[0].ToString();
                var action = currentRoute[1].ToString();

                if (db.ItemMenu.Any(r => r.IsActive == true && r.Url.Contains(controller) && r.Url.Contains(action)))

                    if (webControlCookie != null && webControlCookie.Values["userId"] != null)
                    {
                        var itensMenu = (IEnumerable<ItemMenuDTO>)ViewBag.ItensMenu;
                        ViewBag.itemMenu = itensMenu.FirstOrDefault(i => i.Url != null && i.Url.ToUpperInvariant().Contains((controller + "/" + action).ToUpperInvariant()));
                        if (ViewBag.itemMenu == null)
                            throw new Exception("Acesso Negado!");
                    }
            }

            base.OnActionExecuting(filterContext);
        }

        protected override void EndExecute(IAsyncResult asyncResult)
        {
            base.EndExecute(asyncResult);
        }

        public void CreateCookieFromUserDTO(UserDTO isAuthorized)
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get("webControlCookie");

            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddHours(48);
                HttpContext.Response.Cookies.Set(cookie);
                Response.Cookies.Add(cookie);
            }
            else
            {
                //create a cookie
                HttpCookie myCookie = new HttpCookie("webControlCookie");

                //Add key-values in the cookie
                myCookie.Values.Add("userId", isAuthorized.Id.ToString());
                myCookie.Values.Add("userName", isAuthorized.Name);
                myCookie.Values.Add("CompanyId", isAuthorized.ParCompany_Id.GetValueOrDefault().ToString());

                using (var db = new SgqDbDevEntities())
                {
                    var linkedComapnyIds = db.ParCompanyXUserSgq.Where(c => c.UserSgq_Id == isAuthorized.Id).Select(c => c.ParCompany_Id).ToList();
                    myCookie.Values.Add("LinkedCompanyIds", System.Web.Helpers.Json.Encode(linkedComapnyIds));
                }

                if (isAuthorized.AlterDate != null)
                {
                    myCookie.Values.Add("alterDate", isAuthorized.AlterDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
                }
                else
                {
                    myCookie.Values.Add("alterDate", "");
                }

                myCookie.Values.Add("addDate", isAuthorized.AddDate.ToString("dd/MM/yyyy"));

                if (isAuthorized.Role != null)
                    myCookie.Values.Add("roles", isAuthorized.Role.Replace(';', ',').ToString());//"admin, teste, operacional, 3666,344, 43434,...."
                else
                    myCookie.Values.Add("roles", "");

                if (isAuthorized.ParCompanyXUserSgq != null)
                    if (isAuthorized.ParCompanyXUserSgq.Any(r => r.Role != null))
                        myCookie.Values.Add("rolesCompany", string.Join(",", isAuthorized.ParCompanyXUserSgq.Select(n => n.Role).Distinct().ToArray()));
                    else
                        myCookie.Values.Add("rolesCompany", string.Join(",", isAuthorized.ParCompanyXUserSgq.Select(n => n.ParCompany_Id).Distinct().ToArray()));

                //set cookie expiry date-time. Made it to last for next 12 hours.
                myCookie.Expires = DateTime.Now.AddHours(48);

                //Most important, write the cookie to client.
                Response.Cookies.Add(myCookie);

                //SetItensMenu(isAuthorized);

            }
        }

        protected void ExpireCookie()
        {
            HttpCookie currentUserCookie = Request.Cookies["webControlCookie"];

            if (currentUserCookie != null)
            {
                Response.Cookies.Remove("webControlCookie");
                Response.Cookies.Remove("Language");

                currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                currentUserCookie.Value = null;
                Response.SetCookie(currentUserCookie);
            }

        }

        public static string[] GetWebConfigList(string key)
        {
            var list = GetWebConfigSettings(key).Split(';');
            //Dictionary<string, string> dict = new Dictionary<string, string>();
            //foreach (var o in list)
            //{
            //    if (o.Length >= 3)
            //    {
            //        var obj = o.Split('>');
            //        dict.Add(obj[0], obj[1]);
            //    }
            //}
            return list;
        }

        public static string GetWebConfigSettings(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }

        protected void SetItensMenu()
        {
            var webControlCookie = System.Web.HttpContext.Current.Request.Cookies["webControlCookie"];

            if (webControlCookie != null)
            {
                var UserId = webControlCookie.Values["userId"];
                ViewBag.UserRoles = webControlCookie.Values["roles"];

                if (UserId != null && UserId != "" && int.Parse(UserId) > 0)
                {
                    using (var db = new SgqDbDevEntities())
                    {
                        var rolesNames = db.UserSgq.Find(int.Parse(UserId)).Role.Split(',');

                        var rolesIDs = db.RoleUserSgq.Where(r => rolesNames.Contains(r.Name) && r.IsActive == true).Select(r => r.Id).ToList();

                        var ItensDeMenuUsuarioIds = db.RoleUserSgqXItemMenu.Where(r => rolesIDs.Contains(r.RoleUserSgq_Id) && r.IsActive == true).Select(r => r.ItemMenu_Id).Distinct().ToList();

                        ViewBag.ItensMenu = Mapper.Map<IEnumerable<ItemMenuDTO>>(db.ItemMenu.Where(r => r.IsActive == true && r.ItemMenu_Id != null && ItensDeMenuUsuarioIds.Contains(r.Id)));
                    }
                }
            }
        }
    }

}