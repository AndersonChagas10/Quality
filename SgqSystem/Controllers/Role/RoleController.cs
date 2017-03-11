using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO;
using Helper;
using System;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    [CustomAuthorize]
    public class RoleController : BaseController
    {

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
    }
}