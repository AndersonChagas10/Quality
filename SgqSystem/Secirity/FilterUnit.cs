using AutoMapper;
using Dominio;
using DTO.DTO.Params;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;

namespace SgqSystem.Secirity
{
    /// <summary>
    /// Disponibliza a ViewBag:
    /// 
    /// ViewBag.UnidadeUsuario
    /// 
    /// </summary>
    public class FilterUnit : ActionFilterAttribute
    {
        //public IEnumerable<ParCompanyDTO> _ParCompanyDTO { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //filterContext.HttpContext.Trace.Write("(Logging Filter)Action Executing: " +
            //    filterContext.ActionDescriptor.ActionName);

            IEnumerable<ParCompanyDTO> retorno = new List<ParCompanyDTO>();
            HttpCookie cookie = filterContext.HttpContext.Request.Cookies.Get("webControlCookie");
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

                        filterContext.Controller.ViewBag.UnidadeUsuario = retorno;

                    }
            }

            //return retorno;

            base.OnActionExecuting(filterContext);
        }

        //public override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //  if (filterContext.Exception != null)
        //      filterContext.HttpContext.Trace.Write("(Logging Filter)Exception thrown");
        //  base.OnActionExecuted(filterContext);
        //}
    }
}