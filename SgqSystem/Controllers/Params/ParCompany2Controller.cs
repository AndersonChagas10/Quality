using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Conformity.Application.Util;
using Conformity.Domain.Core.Entities.Parametrizacao;

namespace SgqSystem.Controllers
{
    public class ParCompany2Controller : BaseAuthenticatedController
    {

        private Conformity.Application.Core.Parametrizacao.ParCompanyService _serviceCompany;

        public ParCompany2Controller(ApplicationConfig applicationConfig
            , Conformity.Application.Core.Parametrizacao.ParCompanyService serviceCompany)
            : base (applicationConfig)
        {
            _serviceCompany = serviceCompany;
        }

        // GET: ParCompany2
        public ActionResult Index()
        {
            var lista = View(_serviceCompany.GetAll());
            return lista;
        }

        // GET: ParCompany2/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParCompany parCompany = _serviceCompany.GetById(id.Value);
            if (parCompany == null)
            {
                return HttpNotFound();
            }

            return View(parCompany);
        }

        // GET: ParCompany2/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParCompany2/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id, Name, Description, IsActive, Initials, SIF, CompanyNumber, IpServer, DBServer, IntegrationId, ParCompany_Id, Identification")] ParCompany parCompany)
        {
            ValidModelState(parCompany);
            if (ModelState.IsValid)
            {
                _serviceCompany.Add(parCompany);
                return RedirectToAction("Index");
            }

            return View(parCompany);
        }

        // GET: ParCompany2/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParCompany parCompany = _serviceCompany.GetById(id.Value);
            if (parCompany == null)
            {
                return HttpNotFound();
            }
            return View(parCompany);
        }

        // POST: ParCompany2/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, Name, Description, IsActive, Initials, SIF, CompanyNumber, IpServer, DBServer, IntegrationId, ParCompany_Id, Identification")] Conformity.Domain.Core.Entities.Parametrizacao.ParCompany parCompany)
        {
            //ValidModelState(parCompany);
            if (ModelState.IsValid)
            {
                //parCompany.AlterDate = DateTime.Now;
                _serviceCompany.Update(parCompany);
                return RedirectToAction("Index");
            }
            return View(parCompany);
        }

        private void ValidModelState(ParCompany parCompany)
        {
            if (!string.IsNullOrEmpty(parCompany.Identification) && parCompany.Identification.Length > 50)
                ModelState.AddModelError("Identification", "O campo deve conter apenas 50 caracteres!");

            //if (totalDeVinculos > 0 && parCompany.IsActive == false) 
            //    ModelState.AddModelError("IsActive", Resources.Resource.module_link_indicator);

            //if (string.IsNullOrEmpty(parCompany.Name))
            //    ModelState.AddModelError("Name", Resources.Resource.required_field + " " + Resources.Resource.name);
        }
    }
}
