﻿using AutoMapper;
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
    /// @Html.DropDownList("ParCompany_id", new SelectList(ViewBag.UnidadeUsuario, "Id", "Name"), Resources.Resource.select + "...", new { @class = "form-control" })                         
    /// /// </summary>
    /// 

    public class IdName
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class FormularioPesquisa : ActionFilterAttribute
    {
        //public IEnumerable<ParCompanyDTO> _ParCompanyDTO { get; set; }
        public bool filtraUnidadePorUsuario { get; set; }
        public bool filtraUnidadeDoUsuario { get; set; }
        public bool parLevel1e2 { get; set; }
        public bool parLevel3 { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            using (var db = new SgqDbDevEntities())
            {

                db.Configuration.LazyLoadingEnabled = false;

                List<IdName> GroupLevel1 = new List<IdName>();

                

                for(var i = 0; i < 5; i++)
                {
                    IdName obj = new IdName();

                    obj.Id = i;
                    switch (i)
                    {
                        case 0:
                            obj.Name = "01 - 05";
                            GroupLevel1.Add(obj);
                            break;

                        case 1:
                            obj.Name = "06 - 10";
                            GroupLevel1.Add(obj);
                            break;

                        case 2:
                            obj.Name = "10 - 15";
                            GroupLevel1.Add(obj);
                            break;

                        case 3:
                            obj.Name = "16 - 20";
                            GroupLevel1.Add(obj);
                            break;

                        case 4:
                            obj.Name = "21 - 25";
                            GroupLevel1.Add(obj);
                            break;

                        default:
                            obj.Name = "26 - 30";
                            GroupLevel1.Add(obj);
                            break;

                    }

                    
                }

                filterContext.Controller.ViewBag.GroupLevel1 = new List<IdName>();
                filterContext.Controller.ViewBag.GroupLevel1 = GroupLevel1;

                filterContext.Controller.ViewBag.Level1 = new List<ParLevel1DTO>();
                filterContext.Controller.ViewBag.Level2 = new List<ParLevel2DTO>();
                filterContext.Controller.ViewBag.Level3 = new List<ParLevel3DTO>();

                if (parLevel1e2)
                {
                    filterContext.Controller.ViewBag.Level1 = Mapper.Map<List<ParLevel1DTO>>(db.ParLevel1.ToList());
                    filterContext.Controller.ViewBag.Level2 = Mapper.Map<List<ParLevel2DTO>>(db.ParLevel2.ToList());
                }

                if (parLevel3)
                    filterContext.Controller.ViewBag.Level3 = Mapper.Map<List<ParLevel3DTO>>(db.ParLevel3.ToList());

                HttpCookie cookie = filterContext.HttpContext.Request.Cookies.Get("webControlCookie");

                if (cookie != null)
                {
                    var rolesCompany = "";
                    var userId = 0;

                    if (!string.IsNullOrEmpty(cookie.Values["userId"]))
                        int.TryParse(cookie.Values["userId"].ToString(), out userId);

                    UserSgq userLogado = db.UserSgq.FirstOrDefault(r => r.Id == userId);

                    filterContext.Controller.ViewBag.UserSgq = db.UserSgq.OrderBy(x => x.Name).ToList();
                    filterContext.Controller.ViewBag.Level01 = Mapper.Map<List<ParLevel1DTO>>(db.ParLevel1.ToList());
                    filterContext.Controller.ViewBag.Level02 = Mapper.Map<List<ParLevel2DTO>>(db.ParLevel2.ToList());
                    filterContext.Controller.ViewBag.Level03 = Mapper.Map<List<ParLevel3DTO>>(db.ParLevel3.ToList());
                    filterContext.Controller.ViewBag.Period = Mapper.Map<List<PeriodDTO>>(db.Period.ToList());
                    filterContext.Controller.ViewBag.Shift = Mapper.Map<List<ShiftDTO>>(db.Shift.ToList());
                    filterContext.Controller.ViewBag.Department = Mapper.Map<List<ParDepartmentDTO>>(db.ParDepartment.ToList());
                    filterContext.Controller.ViewBag.Modulos = Mapper.Map<List<ParModuleDTO>>(db.ParModule.ToList()).Where(r => r.IsActive == true);

                    if (!filtraUnidadePorUsuario)/*Se não filtra uNidades por Usuario*/
                    {
                        var unidades = db.ParCompany.ToList();

                        if (unidades == null || unidades.Count() > 0)
                            unidades = db.ParCompany.Where(r => r.Id == userLogado.ParCompany_Id).ToList();

                        filterContext.Controller.ViewBag.UnidadeUsuario = Mapper.Map<IEnumerable<ParCompanyDTO>>(unidades);
                    }
                    else if (filtraUnidadePorUsuario)/*Se filtra uNidades por Usuario*/
                    {
                        if (userId > 0) {
                            var user = db.UserSgq.Find(userId);
                            if (user != null && user.ShowAllUnits != true)
                            {
                                if (!string.IsNullOrEmpty(cookie.Values["rolesCompany"])) /*Se user possuir mais de uma unidade*/
                                {
                                    rolesCompany = cookie.Values["rolesCompany"].ToString();

                                    #region Query Unidades

                                    var _companyXUserSgq = db.ParCompanyXUserSgq.Where(r => r.UserSgq_Id == userId).Select(r => r.ParCompany).ToList().OrderBy(r => r.Name).GroupBy(r => r.Id).Select(group => group.First()).ToList();

                                    filterContext.Controller.ViewBag.UnidadeUsuario = Mapper.Map<IEnumerable<ParCompanyDTO>>(_companyXUserSgq.Where(u => u.IsActive == true));

                                    #endregion
                                }
                                else /*Se não possui mais de uma undiade*/
                                {
                                    var unidades = db.ParCompany.ToList();

                                    if (unidades == null || unidades.Count() > 0)
                                        unidades = db.ParCompany.Where(r => r.Id == userLogado.ParCompany_Id).ToList();

                                    filterContext.Controller.ViewBag.UnidadeUsuario = Mapper.Map<IEnumerable<ParCompanyDTO>>(unidades);
                                }
                            }else
                            {
                                var _companyXUserSgq = db.ParCompany.Where(p=>p.IsActive == true).ToList();

                                filterContext.Controller.ViewBag.UnidadeUsuario = Mapper.Map<IEnumerable<ParCompanyDTO>>(_companyXUserSgq);
                            }
                        }
                    }

                    if (filtraUnidadeDoUsuario)
                    {
                        if (filterContext.Controller.ViewBag.UnidadeUsuario != null)
                            filterContext.Controller.ViewBag.UnidadeUsuario = Mapper.Map<IEnumerable<ParCompanyDTO>>(db.ParCompany.Where(r => r.Id == userLogado.ParCompany_Id));
                    }
                }

                //return retorno;
            }
            base.OnActionExecuting(filterContext);
        }



        //public override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    //if (filterContext.Exception != null)
        //    //    filterContext.HttpContext.Trace.Write("(Logging Filter)Exception thrown");

        //    base.OnActionExecuted(filterContext);
        //}
    }
}