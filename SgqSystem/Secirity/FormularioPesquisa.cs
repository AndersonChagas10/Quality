using AutoMapper;
using Dominio;
using DTO.DTO.Params;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using DTO.DTO;

namespace SgqSystem.Secirity
{
    /// <summary>
    /// Disponibiliza as ViewsBags: 
    /// 
    /// ViewBag.UnidadeUsuario 
    /// ViewBag.UserSgq
    /// ViewBag.Level01
    /// ViewBag.Level02
    /// ViewBag.Level03
    /// ViewBag.Shift
    /// ViewBag.Period 
    ///                        
    /// </summary>
    public class FormularioPesquisa : ActionFilterAttribute
    {
        //public IEnumerable<ParCompanyDTO> _ParCompanyDTO { get; set; }
        public bool filtraUnidadePorUsuario { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //filterContext.HttpContext.Trace.Write("(Logging Filter)Action Executing: " +
            //    filterContext.ActionDescriptor.ActionName);

            IEnumerable<ParCompanyDTO> unidades = new List<ParCompanyDTO>();
            List<ParCompany> _companyXUserSgq;
            List<UserSgq> _Users;
            List<ParLevel1DTO> _Level1;
            List<ParLevel2DTO> _Level2;
            List<ParLevel3DTO> _Level3;
            List<ShiftDTO> _Shift;
            List<PeriodDTO> _Period;

            HttpCookie cookie = filterContext.HttpContext.Request.Cookies.Get("webControlCookie");
            if (cookie != null)
            {
                var rolesCompany = "";
                var userId = 0;
                if (!string.IsNullOrEmpty(cookie.Values["userId"]))
                    int.TryParse(cookie.Values["userId"].ToString(), out userId);

                unidades = GetData(filterContext, unidades, out _Users, out _Level1, out _Level2, out _Level3, out _Shift, out _Period);
                if (userId > 0)
                    if (!string.IsNullOrEmpty(cookie.Values["rolesCompany"]))
                    {
                        rolesCompany = cookie.Values["rolesCompany"].ToString();

                        if (filtraUnidadePorUsuario)/*Se filtra uNidades por Usuario*/
                        {
                            using (var db = new SgqDbDevEntities())
                            {
                                #region Query Unidades

                                _companyXUserSgq = db.ParCompanyXUserSgq.Where(r => r.UserSgq_Id == userId).Select(r => r.ParCompany).ToList().OrderBy(r => r.Name).GroupBy(r => r.Id).Select(group => group.First()).ToList();

                                unidades = Mapper.Map<IEnumerable<ParCompanyDTO>>(_companyXUserSgq);

                                #endregion
                            }
                        }
                    }
            }

            //return retorno;

            base.OnActionExecuting(filterContext);
        }

        private IEnumerable<ParCompanyDTO> GetData(ActionExecutingContext filterContext, IEnumerable<ParCompanyDTO> unidades, out List<UserSgq> _Users, out List<ParLevel1DTO> _Level1, out List<ParLevel2DTO> _Level2, out List<ParLevel3DTO> _Level3, out List<ShiftDTO> _Shift, out List<PeriodDTO> _Period)
        {
            using (var db2 = new SgqDbDevEntities())
            {
                db2.Configuration.LazyLoadingEnabled = false;
                if (!filtraUnidadePorUsuario)/*Se não filtra uNidades por Usuario*/
                {
                    unidades = Mapper.Map<IEnumerable<ParCompanyDTO>>(db2.ParCompany.ToList());
                }
                _Users = db2.UserSgq.ToList();
                _Level1 = Mapper.Map<List<ParLevel1DTO>>(db2.ParLevel1.ToList());
                _Level2 = Mapper.Map<List<ParLevel2DTO>>(db2.ParLevel2.ToList());
                _Level3 = Mapper.Map<List<ParLevel3DTO>>(db2.ParLevel3.ToList());
                _Period = Mapper.Map<List<PeriodDTO>>(db2.Period.ToList());
                _Shift = Mapper.Map<List<ShiftDTO>>(db2.Shift.ToList());
            }

            filterContext.Controller.ViewBag.UnidadeUsuario = unidades;
            filterContext.Controller.ViewBag.UserSgq = _Users;
            filterContext.Controller.ViewBag.Level01 = _Level1;
            filterContext.Controller.ViewBag.Level02 = _Level2;
            filterContext.Controller.ViewBag.Level03 = _Level3;
            filterContext.Controller.ViewBag.Shift = _Shift;
            filterContext.Controller.ViewBag.Period = _Period;
            return unidades;
        }

        //public override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    //if (filterContext.Exception != null)
        //    //    filterContext.HttpContext.Trace.Write("(Logging Filter)Exception thrown");

        //    base.OnActionExecuted(filterContext);
        //}
    }
}