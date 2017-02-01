using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class RoleController : Controller
    {

        public RoleController() { 
}
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }
    }
}