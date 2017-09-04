using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Params
{

    public class EmailConfigEUAController : Controller
    {
        private SgqDbDevEntities db; 
        public EmailConfigEUAController()
        {
            db = new SgqDbDevEntities();
            ViewBag.ParCompany = db.ParCompany.ToList();
            ViewBag.UserSgq = db.UserSgq.ToList();
            ViewBag.ParFrequency = db.ParFrequency.ToList();
            ViewBag.RoleUserSgq = db.RoleUserSgq.ToList();
        }
        // GET: EmailConfigEUA
        public ActionResult Index()
        {
            return View();
        }
    }

    public class EmailConfig
    {
        public int Frequency_Id { get; set; }
        public int AlertLevel_Id { get; set; }
        public List<int> RoleUserSgq_Id { get; set; }
        public List<int> UserSgq_Id { get; set; }
        public bool isCoorporated { get; set; }
    }
}