using Dominio;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class ParCargoXDepartmentsController : Controller
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParCargoXDepartments
        public ActionResult Index()
        {
            var parCargoXDepartment = db.ParCargoXDepartment.Include(p => p.ParCargo).Include(p => p.ParDepartment).Where(x => x.IsActive);
            return View(parCargoXDepartment.ToList());
        }

        // GET: ParCargoXDepartments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParCargoXDepartment parCargoXDepartment = db.ParCargoXDepartment.Find(id);
            if (parCargoXDepartment == null)
            {
                return HttpNotFound();
            }
            return View(parCargoXDepartment);
        }

        // GET: ParCargoXDepartments/Create
        public ActionResult Create(int parCargoId)
        {
            ViewBag.ParCargo_Id = parCargoId;
            var listaDeCargosSalvos = db.ParCargoXDepartment.Where(x => x.ParCargo_Id == parCargoId && x.IsActive == true).Select(m => m.ParDepartment_Id).ToList();

            ViewBag.ParDepartment_Id = db.ParDepartment.ToList()
           .Select(x => new KeyValuePair<int, string>(x.Id, x.Id + "- " + x.Name))
           .ToList();

            if (ViewBag.ParDepartment_Id.Count == 0)
            {
                var semDados = new List<KeyValuePair<int, string>>() {
                new KeyValuePair<int, string>(0, ""),

            };
                ViewBag.ParDepartment_Id = semDados;
            }

            //ViewBag.ParDepartment_Id = new SelectList(db.ParDepartment.Where(m => !listaDeCargosSalvos.Contains(m.Id) && m.Active).Select(m => m).ToList(), "Id", "Name");

            return View(new ParCargoXDepartment() { ParCargo_Id = parCargoId });
        }

        // POST: ParCargoXDepartments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ParDepartment_Id,ParCargo_Id,IsActive,AddDate,AlterDate")] ParCargoXDepartment parCargoXDepartment)
        {

            //buscar se ja existe um vinculo entr departamento e cargo, se sim bloquear a criação

            var parCargoXDepartmentExistente = db.ParCargoXDepartment
                                                    .Where(x => x.ParCargo_Id == parCargoXDepartment.ParCargo_Id && x.ParDepartment_Id == parCargoXDepartment.ParDepartment_Id)
                                                    .FirstOrDefault();

            if (parCargoXDepartmentExistente != null)
            {
                ModelState.AddModelError("ParDepartment_Id", "Já exste um vinculo entre Centro de Custo e Cargo selecionados!");
            }
            if (ModelState.IsValid)
            {
                db.ParCargoXDepartment.Add(parCargoXDepartment);
                db.SaveChanges();
                return RedirectToAction("Details", "ParCargo", new { id = parCargoXDepartment.ParCargo_Id });
                //return RedirectToAction("Index");
            }


            ViewBag.ParCargo_Id = new SelectList(db.ParCargo, "Id", "Name", parCargoXDepartment.ParCargo_Id);
            ViewBag.ParDepartment_Id = new SelectList(db.ParDepartment, "Id", "Name", parCargoXDepartment.ParDepartment_Id);
            return View(parCargoXDepartment);
        }

        // GET: ParCargoXDepartments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParCargoXDepartment parCargoXDepartment = db.ParCargoXDepartment.Find(id);
            if (parCargoXDepartment == null)
            {
                return HttpNotFound();
            }

            ViewBag.ParCargo_Id = new SelectList(db.ParCargo, "Id", "Name", parCargoXDepartment.ParCargo_Id);
            ViewBag.ParDepartment_Id = new SelectList(db.ParDepartment, "Id", "Name", parCargoXDepartment.ParDepartment_Id);
            return View(parCargoXDepartment);
        }

        // POST: ParCargoXDepartments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ParDepartment_Id,ParCargo_Id,IsActive,AddDate,AlterDate")] ParCargoXDepartment parCargoXDepartment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parCargoXDepartment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "ParCargo", new { id = parCargoXDepartment.ParCargo_Id });
            }
            ViewBag.ParCargo_Id = new SelectList(db.ParCargo, "Id", "Name", parCargoXDepartment.ParCargo_Id);
            ViewBag.ParDepartment_Id = new SelectList(db.ParDepartment, "Id", "Name", parCargoXDepartment.ParDepartment_Id);
            return View(parCargoXDepartment);
        }

        // GET: ParCargoXDepartments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParCargoXDepartment parCargoXDepartment = db.ParCargoXDepartment.Find(id);
            if (parCargoXDepartment == null)
            {
                return HttpNotFound();
            }
            return View(parCargoXDepartment);
        }

        // POST: ParCargoXDepartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParCargoXDepartment parCargoXDepartment = db.ParCargoXDepartment.Find(id);
            db.ParCargoXDepartment.Remove(parCargoXDepartment);
            db.SaveChanges();
            return RedirectToAction("Details", "ParCargo", new { id = parCargoXDepartment.ParCargo_Id });
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
