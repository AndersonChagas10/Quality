using Dominio;
using Dominio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class RoleController : BaseController
    {

        //private IBaseDomain<RoleType, RoleTypeDTO> _baseDomainRoleType;

       // public RoleController(IBaseDomain<RoleType, RoleTypeDTO> baseDomainRoleType)
        public RoleController()
        {
            //_baseDomainRoleType = baseDomainRoleType;

            //ViewBag.listaRoleType = _baseDomainRoleType.GetAll();
        }
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }
        
    }
}