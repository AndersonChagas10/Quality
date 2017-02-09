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
    /// ViewBag.UnidadeUsuario:
    /// @Html.DropDownListFor(model => model.ParCompany_Id, new SelectList(ViewBag.UnidadeUsuario, "Id", "Name"), Resources.Resource.select + "...", new { @class = "form-control" })
    /// </summary>
    public class FilterUnit : ActionFilterAttribute
    {
        public bool filtraUnidadeDoUsuario { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //filterContext.HttpContext.Trace.Write("(Logging Filter)Action Executing: " +
            //    filterContext.ActionDescriptor.ActionName);

            using (var db = new SgqDbDevEntities())
            {

                db.Configuration.LazyLoadingEnabled = false;

                HttpCookie cookie = filterContext.HttpContext.Request.Cookies.Get("webControlCookie");

                if (cookie != null)
                {
                    var rolesCompany = "";
                    var userId = 0;

                    if (!string.IsNullOrEmpty(cookie.Values["userId"]))
                        int.TryParse(cookie.Values["userId"].ToString(), out userId);

                    UserSgq userLogado = db.UserSgq.FirstOrDefault(r => r.Id == userId);

                    filterContext.Controller.ViewBag.UserSgq = db.UserSgq.ToList();

                    if (userId > 0)
                    {

                        if (filtraUnidadeDoUsuario)
                        {
                            filterContext.Controller.ViewBag.UnidadeUsuario = Mapper.Map<IEnumerable<ParCompanyDTO>>(db.ParCompany.Where(r => r.Id == userLogado.ParCompany_Id));
                        }
                        else if (!string.IsNullOrEmpty(cookie.Values["rolesCompany"])) /*Se user possuir mais de uma unidade*/
                        {
                            rolesCompany = cookie.Values["rolesCompany"].ToString();

                            #region Query Unidades

                            var _companyXUserSgq = db.ParCompanyXUserSgq.Where(r => r.UserSgq_Id == userId).Select(r => r.ParCompany).ToList().OrderBy(r => r.Name).GroupBy(r => r.Id).Select(group => group.First()).ToList();

                            filterContext.Controller.ViewBag.UnidadeUsuario = Mapper.Map<IEnumerable<ParCompanyDTO>>(_companyXUserSgq);

                            #endregion
                        }
                        else /*Se não possui mais de uma undiade*/
                        {
                            var unidades = db.ParCompany.ToList();

                            if (unidades == null || unidades.Count() > 0)
                                unidades = db.ParCompany.Where(r => r.Id == userLogado.ParCompany_Id).ToList();

                            filterContext.Controller.ViewBag.UnidadeUsuario = Mapper.Map<IEnumerable<ParCompanyDTO>>(unidades);
                        }

                    }
                }
                //return retorno;
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