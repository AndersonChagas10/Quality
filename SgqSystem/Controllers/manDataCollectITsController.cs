using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Dominio;
using DTO.DTO.Params;
using Dominio.Interfaces.Services;
using DTO.Helpers;
using SgqSystem.Secirity;
using System;

namespace SgqSystem.Controllers
{
    [CustomAuthorize(Roles = "somentemanutencao-sgq")]
    public class manDataCollectITsController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        private IBaseDomain<ParCompany, ParCompanyDTO> _baseDomainParCompany;
        private IBaseDomain<ParFrequency, ParFrequencyDTO> _baseDomainParFrequency;
        private IBaseDomain<ParMeasurementUnit, ParMeasurementUnitDTO> _baseDomainParMeasurementUnit;

        public manDataCollectITsController(IBaseDomain<ParCompany, ParCompanyDTO> baseDomainParCompany,
                    IBaseDomain<ParFrequency, ParFrequencyDTO> baseDomainParFrequency,
                    IBaseDomain<ParMeasurementUnit, ParMeasurementUnitDTO> baseDomainParMeasurementUnit)
        {
            _baseDomainParCompany = baseDomainParCompany;
            _baseDomainParFrequency = baseDomainParFrequency;
            _baseDomainParMeasurementUnit = baseDomainParMeasurementUnit;

            ViewBag.listaParCompany = _baseDomainParCompany.GetAll();
            ViewBag.listaParFrequency = _baseDomainParFrequency.GetAll();
            ViewBag.listaParMeasurementUnit = _baseDomainParMeasurementUnit.GetAll();


        }

        // GET: manDataCollectITs
        public ActionResult Index()
        {
            ViewBag.listaParCompany = FiltraUnidades();
            return View(db.manDataCollectIT.ToList());
        }

        // GET: manDataCollectITs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            manDataCollectIT manDataCollectIT = db.manDataCollectIT.Find(id);
            if (manDataCollectIT == null)
            {
                return HttpNotFound();
            }
            return View(manDataCollectIT);
        }

        // GET: manDataCollectITs/Create
        public ActionResult Create()
        {
            ViewBag.listaParCompany = FiltraUnidades();
            return View(new manDataCollectIT() {amountData=0});
        }

        // POST: manDataCollectITs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AddDate,AlterDate,referenceDatetime,userSGQ_id,parCompany_id,parFrequency_id,shift,dataType,amountData,ParMeasurementUnit_Id,IsActive")] manDataCollectIT manDataCollectIT)
        {
            manDataCollectIT.IsActive = true;
            manDataCollectIT.userSGQ_id = Guard.GetUsuarioLogado_Id(HttpContext);

            if (ModelState.IsValid)
            {
                manDataCollectIT.AddDate = DateTime.Now;
                db.manDataCollectIT.Add(manDataCollectIT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(manDataCollectIT);
        }

        // GET: manDataCollectITs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            manDataCollectIT manDataCollectIT = db.manDataCollectIT.Find(id);
            if (manDataCollectIT == null)
            {
                return HttpNotFound();
            }
            return View(manDataCollectIT);
        }

        // POST: manDataCollectITs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AddDate,AlterDate,referenceDatetime,userSGQ_id,parCompany_id,parFrequency_id,shift,dataType,amountData,ParMeasurementUnit_Id,IsActive")] manDataCollectIT manDataCollectIT)
        {
            if (ModelState.IsValid)
            {
                manDataCollectIT.AlterDate = DateTime.Now;
                db.Entry(manDataCollectIT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(manDataCollectIT);
        }

        // GET: manDataCollectITs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            manDataCollectIT manDataCollectIT = db.manDataCollectIT.Find(id);
            if (manDataCollectIT == null)
            {
                return HttpNotFound();
            }
            return View(manDataCollectIT);
        }

        // POST: manDataCollectITs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            manDataCollectIT manDataCollectIT = db.manDataCollectIT.Find(id);
            db.manDataCollectIT.Remove(manDataCollectIT);
            db.SaveChanges();
            return RedirectToAction("Index");
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
