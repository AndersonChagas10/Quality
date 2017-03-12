using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO;
using Helper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    [CustomAuthorize]
    public class RoleController : BaseController
    {

        private SgqDbDevEntities db = new SgqDbDevEntities();

        private IBaseDomain<RoleType, RoleTypeDTO> _baseDomainRoleType;
        private IBaseDomain<ScreenComponent, ScreenComponentDTO> _baseDomainScreenComponent;
        private IBaseDomain<RoleSGQ, RoleSGQDTO> _baseDomainRoleSGQ;
        private IBaseDomain<RoleJBS, RoleJBSDTO> _baseDomainRoleJBS;

        public RoleController(IBaseDomain<RoleType, RoleTypeDTO> baseDomainRoleType,
            IBaseDomain<ScreenComponent, ScreenComponentDTO> baseDomainScreenComponent,
            IBaseDomain<RoleSGQ, RoleSGQDTO> baseDomainRoleSGQ,
            IBaseDomain<RoleJBS, RoleJBSDTO> baseDomainRoleJBS
            )
        {
            _baseDomainRoleType = baseDomainRoleType;
            _baseDomainScreenComponent = baseDomainScreenComponent;
            _baseDomainRoleSGQ = baseDomainRoleSGQ;
            _baseDomainRoleJBS = baseDomainRoleJBS;

            ViewBag.listaRoleType = _baseDomainRoleType.GetAll();
            ViewBag.listaScreenComponent = _baseDomainScreenComponent.GetAll();
            ViewBag.listaRoleSGQ = _baseDomainRoleSGQ.GetAll();
            ViewBag.listaRoleJBS = _baseDomainRoleJBS.GetAll();
        }
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }

        public String UpdateScreenComponentList()
        {
            ViewBag.listaScreenComponent = _baseDomainScreenComponent.GetAll();

            var lines = "";
            foreach (var component in ViewBag.listaScreenComponent)
            {
                lines +=
                "<tr>" +
                    "<td>" + component.HashKey + "</td>" +
                    "<td>" + component.Component + "</td>" +
                    "<td>";

                if (component.RoleType != null)
                    lines += component.RoleType.Type;

                lines += "</td>" +
                    "<td>" +
                        "<button type='button' class='btn btn-primary btn-sm' " +
                        "onclick='indexScreenComponent.openComponent(" + component.Id + ")'>" +
                        Resources.Resource.edit + "</button>" +
                    "</td>" +
                "</tr>";
            }

            var body = "<tbody>" + lines + "</tbody>";

            return body;

        }


        public String UpdateRoleJBSList()
        {
            ViewBag.listaRoleJBS = _baseDomainRoleJBS.GetAll();

            var lines = "";
            foreach (var component in ViewBag.listaRoleJBS)
            {
                lines +=
                "<tr>" +
                    "<td id='RoleSGQRow_@component.Id' class='hide'>" +
                        component.Id +
                    "</td>" +
                    "<td> " +
                        component.ScreenComponent_Id +
                    "</td>" +
                    "<td> " +
                        component.Role +
                    "</td>" +
                    "<td> " +
                        "<button onclick='indexRoleJBS.openRole(" + component.Id + ")' class='btn btn-primary btn-sm'>" + Resources.Resource.edit + "</button>" +
                    "</td>" +
                "</tr>";
            }

            var body = "<tbody>" + lines + "</tbody>";

            return body;

        }

        public String UpdateRoleSGQList()
        {
            ViewBag.listaRoleType = _baseDomainRoleType.GetAll();
            ViewBag.listaScreenComponent = _baseDomainScreenComponent.GetAll();
            ViewBag.listaRoleSGQ = _baseDomainRoleSGQ.GetAll();

            var lines = "";
            foreach (var component in ViewBag.listaRoleSGQ)
            {
                lines +=
                "<tr>" +
                    "<td id='RoleSGQRow_" + component.Id + "' class='hide'>" +
                        component.Id +
                    "</td>" +
                    "<td> " +
                        component.ScreenComponent_Id +
                    "</td>" +
                    "<td> " +
                        component.Role +
                    "</td>" +
                    "<td> " +
                        "<button onclick='indexRoleSGQ.openRole(" + component.Id + ")' class='btn btn-primary btn-sm'>" + Resources.Resource.edit + "</button>" +
                    "</td>" +
                "</tr>";
            }

            var body = "<tbody>" + lines + "</tbody>";

            return body;

        }


        public ActionResult LeftControlRole()
        {
            ViewBag.Title = Resources.Resource.left_menu_control;

            var model = db.LeftControlRole.ToList();

            return View(model);
        }

        public ActionResult EditLeftControlRole(int? id)
        {
            ViewBag.Title = Resources.Resource.edit;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = db.LeftControlRole.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string SaveLeftControlRole(LeftControlRole model)
        {
            try
            {
                if (model.Id == 0)
                {
                    db.Entry(model).State = EntityState.Added;
                    db.LeftControlRole.Add(model);
                }
                else
                {
                    var leftControlRole = db.LeftControlRole.Where(x => x.Id == model.Id).FirstOrDefault();

                    db.Entry(leftControlRole).State = EntityState.Modified;

                    leftControlRole.Controller = model.Controller;
                    leftControlRole.Action = model.Action;
                    leftControlRole.Role = model.Role;
                }

                db.SaveChanges();
                return Resources.Resource.saved_successfully;

            }
            catch (Exception)
            {
                return Resources.Resource.try_again_contact_support;
            }

        }

        [HttpPost]
        public bool VerificarRoleLink(string controller, string action)
        {
            try
            {
                var leftControlRole = db.LeftControlRole.Where(x => x.Controller.Equals(controller) && x.Action.Equals(action)).FirstOrDefault();

                if (leftControlRole != null)
                {
                    var rolesLink = new List<string>();

                    if (leftControlRole.Role != null)
                    {
                        rolesLink = leftControlRole.Role.Trim().Replace(" ", "").Split(',').ToList();
                        rolesLink = LimparListaVazia(rolesLink);
                    }

                    if (rolesLink.Count > 0)
                    {
                        //Comparar com role usuario
                        HttpCookie cookie = HttpContext.Request.Cookies.Get("webControlCookie");
                        if (cookie != null)
                        {
                            int userId = 0;
                            if (!string.IsNullOrEmpty(cookie.Values["userId"]))
                            {
                                int.TryParse(cookie.Values["userId"].ToString(), out userId);

                                #region ROLES UserSgq

                                var userSgq = db.UserSgq.Where(x => x.Id == userId).FirstOrDefault();
                                var rolesUserSgq = new List<string>();
                                if (userSgq.Role != null)
                                {
                                    rolesUserSgq = userSgq.Role.Trim().Replace(" ", "").Replace(",", ";").Split(';').ToList();
                                    rolesUserSgq = LimparListaVazia(rolesUserSgq);
                                }

                                #endregion

                                #region ROLES ParCompanyXUserSgq

                                var parCompany = db.ParCompanyXUserSgq.Where(x => x.UserSgq_Id == userId).ToList();
                                var rolesParCompanyXUserSgq = new List<string>();
                                rolesParCompanyXUserSgq = parCompany.Select(x => x.Role).ToList();
                                rolesParCompanyXUserSgq = LimparListaVazia(rolesParCompanyXUserSgq);

                                #endregion

                                rolesUserSgq.AddRange(rolesParCompanyXUserSgq);


                                if (rolesLink.Count > 0)
                                {
                                    foreach (var i in rolesLink)
                                    {
                                        if (rolesUserSgq.Contains(i))
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }

                            return false;
                        }
                        else
                        {
                            return false;
                        }

                    }

                    return true;
                }
                else
                {
                    //Inserir Controller se não tiver na lista
                    leftControlRole = new Dominio.LeftControlRole();
                    leftControlRole.Id = 0;
                    leftControlRole.Action = action;
                    leftControlRole.Controller = controller;
                    leftControlRole.Role = null;
                    db.Entry(leftControlRole).State = EntityState.Added;
                    db.LeftControlRole.Add(leftControlRole);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        private List<string> LimparListaVazia(List<string> value)
        {
            for (var i = 0; i < value.Count;)
            {
                if (value[i].Trim().Equals(""))
                {
                    value.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
            return value;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}