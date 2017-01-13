using AutoMapper;
using Dominio;
using DTO;
using DTO.DTO.Params;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{

    public class BaseController : Controller
    {
        public BaseController()
        {
            //GlobalConfig.linkDataCollect = "http://192.168.25.200/AppColeta/";
            GlobalConfig.linkDataCollect = "http://mtzsvmqsc/AppColeta/";
            ViewBag.UrlDataCollect = GlobalConfig.linkDataCollect;
            //UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
            //ViewBag.UrlScorecard = u.Action("Scorecard", "RelatoriosSgq");
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            HttpCookie languageCookie = System.Web.HttpContext.Current.Request.Cookies["Language"];
            if (languageCookie != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(languageCookie.Value);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCookie.Value);
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
            }


            base.Initialize(requestContext);
        }

        #region Auxiliares
        public IEnumerable<ParCompanyDTO> FiltraUnidades()
        {
            IEnumerable<ParCompanyDTO> retorno = new List<ParCompanyDTO>();
            HttpCookie cookie = HttpContext.Request.Cookies.Get("webControlCookie");
            if (cookie != null)
            {
                var rolesCompany = "";
                var userId = 0;
                if (!string.IsNullOrEmpty(cookie.Values["userId"]))
                    int.TryParse(cookie.Values["userId"].ToString(), out userId);

                if (userId > 0)
                    if (!string.IsNullOrEmpty(cookie.Values["rolesCompany"]))
                    {
                        rolesCompany = cookie.Values["rolesCompany"].ToString();
                        List<ParCompany> _companyXUserSgq;
                        using (var db = new SgqDbDevEntities())
                        {
                            _companyXUserSgq = db.ParCompanyXUserSgq.Where(r => r.UserSgq_Id == userId).Select(r => r.ParCompany).ToList().OrderBy(r => r.Name).GroupBy(r => r.Id).Select(group => group.First()).ToList();
                            retorno = Mapper.Map<IEnumerable<ParCompanyDTO>>(_companyXUserSgq);
                        }


                        ViewBag.UnidadeUsuario = retorno;
                    }
            }

            return retorno;
        }

        #endregion
    }

}