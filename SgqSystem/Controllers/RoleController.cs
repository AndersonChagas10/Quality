using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class RoleController : BaseController
    {

        private IBaseDomain<RoleType, RoleTypeDTO> _baseDomainRoleType;
        private IBaseDomain<ScreenComponent, ScreenComponentDTO> _baseDomainScreenComponent;

        public RoleController(IBaseDomain<RoleType, RoleTypeDTO> baseDomainRoleType,
            IBaseDomain<ScreenComponent, ScreenComponentDTO> baseDomainScreenComponent)
        {
            _baseDomainRoleType = baseDomainRoleType;
            _baseDomainScreenComponent = baseDomainScreenComponent;

            ViewBag.listaRoleType = _baseDomainRoleType.GetAll();
            ViewBag.listaScreenComponent = _baseDomainScreenComponent.GetAll();
        }
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }
        
    }
}