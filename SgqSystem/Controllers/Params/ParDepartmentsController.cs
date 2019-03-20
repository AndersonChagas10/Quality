using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Dominio;
using Helper;

namespace SgqSystem.Controllers
{
    [CustomAuthorize]
    public class ParDepartmentsController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParDepartments
        public ActionResult Index()
        {
            var listaFilhos = db.ParDepartment.ToList();
            listaFilhos.Add(new ParDepartment() { Id = -1, Name = "Selecione" });
            ViewBag.Parent_Id = new SelectList(listaFilhos, "Id", "Name", -1);

            return View(db.ParDepartment.ToList());
        }

        // GET: ParDepartments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParDepartment parDepartment = db.ParDepartment.Find(id);
            if (parDepartment == null)
            {
                return HttpNotFound();
            }
            return View(parDepartment);
        }

        // GET: ParDepartments/Create
        public ActionResult Create()
        {
            MontaLista(new ParDepartment());
            return View();
        }

        // POST: ParDepartments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,AddDate,AlterDate,Active,Parent_Id")] ParDepartment parDepartment)
        {
            MontaHash(parDepartment);
            if (parDepartment.Parent_Id <= 0)
                parDepartment.Parent_Id = null;

            DepartamentoDuplicado(parDepartment);
            if (ModelState.IsValid)
            {
                db.ParDepartment.Add(parDepartment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            MontaLista(parDepartment);
            return View(parDepartment);
        }

        // GET: ParDepartments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParDepartment parDepartment = db.ParDepartment.Find(id);
            if (parDepartment == null)
            {
                return HttpNotFound();
            }
            MontaLista(parDepartment);
            return View(parDepartment);
        }

        // POST: ParDepartments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,AddDate,AlterDate,Active,Parent_Id")] ParDepartment parDepartment)
        {
            MontaHash(parDepartment);
            DepartamentoDuplicado(parDepartment);
            if (ModelState.IsValid)
            {
                db.Entry(parDepartment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            MontaLista(parDepartment);
            return View(parDepartment);
        }

        // GET: ParDepartments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParDepartment parDepartment = db.ParDepartment.Find(id);
            if (parDepartment == null)
            {
                return HttpNotFound();
            }
            return View(parDepartment);
        }

        // POST: ParDepartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParDepartment parDepartment = db.ParDepartment.Find(id);
            db.ParDepartment.Remove(parDepartment);
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

        private void MontaHash(ParDepartment parDepartment)
        {
            ParDepartment pai = new ParDepartment();
            if (parDepartment.Parent_Id > 0)
                pai = db.ParDepartment.AsNoTracking().Where(x => x.Id == parDepartment.Parent_Id).FirstOrDefault();
            else
                pai = null;

            if (pai != null)
            {
                if (pai.Hash != null)
                {
                    parDepartment.Hash = pai.Hash + "|" + pai.Id;
                }
                else
                {
                    parDepartment.Hash = pai.Id.ToString();
                }
            }
            else
            {
                parDepartment.Hash = null;
            }
        }

        private void MontaLista(ParDepartment parDepartment)
        {
            ViewBag.TemFilhos = db.ParDepartment.Any(x => x.Parent_Id == parDepartment.Id && x.Active);

            var listaFilhos = db.ParDepartment.Where(x => x.Active).ToList();
            listaFilhos.Insert(0, new ParDepartment() { Id = 0, Name = "Selecione" });
            listaFilhos.Remove(parDepartment);
            ViewBag.Parent_Id = new SelectList(listaFilhos, "Id", "Name", parDepartment.Parent_Id);
        }

        private void DepartamentoDuplicado(ParDepartment parDepartment)
        {
            if(db.ParDepartment.Any(x=>x.Name == parDepartment.Name && x.Id != parDepartment.Id))
            {
                ModelState.AddModelError("", Resources.Resource.duplicated_department);
            }
        }
    }
}
