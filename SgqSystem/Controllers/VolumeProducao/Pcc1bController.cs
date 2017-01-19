using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Dominio;
using DTO.Helpers;
using SgqSystem.Secirity;

namespace SgqSystem.Controllers
{
    [CustomAuthorize]
    public class Pcc1bController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: Pcc1b
        public ActionResult Index()
        {
            var pcc1b = db.VolumePcc1b.Include(p => p.ParCompany).Include(p => p.ParLevel1).OrderByDescending(p => p.Data);
            return View(pcc1b.ToList());
        }

        // GET: Pcc1b/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolumePcc1b pcc1b = db.VolumePcc1b.Find(id);
            if (pcc1b == null)
            {
                return HttpNotFound();
            }
            return View(pcc1b);
        }

        // GET: Pcc1b/Create
        public ActionResult Create()
        {
            ViewBag.ParCompany_id = new SelectList(db.ParCompany.OrderBy(c => c.Name), "Id", "Name");
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1.Where(c => c.Id == 3), "Id", "Name");
            return View();
        }

        // POST: Pcc1b/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Indicador,Unidade,Data,Departamento,VolumeAnimais,Quartos,Meta,ToleranciaDia,Nivel11,Nivel12,Nivel13,Avaliacoes,Amostras,AddDate,AlterDate,ParCompany_id,ParLevel1_id")] VolumePcc1b pcc1b)
        {
            ValidaPcc1B(pcc1b);

            if (ModelState.IsValid)
            {
                db.VolumePcc1b.Add(pcc1b);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParCompany_id = new SelectList(db.ParCompany.OrderBy(c => c.Name), "Id", "Name", pcc1b.ParCompany_id);
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1, "Id", "Name", pcc1b.ParLevel1_id);
            return View(pcc1b);
        }

        // GET: Pcc1b/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolumePcc1b pcc1b = db.VolumePcc1b.Find(id);
            if (pcc1b == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParCompany_id = new SelectList(db.ParCompany.OrderBy(c => c.Name), "Id", "Name", pcc1b.ParCompany_id);
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1.Where(c => c.Id == 3), "Id", "Name", pcc1b.ParLevel1_id);
            return View(pcc1b);
        }

        // POST: Pcc1b/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Indicador,Unidade,Data,Departamento,VolumeAnimais,Quartos,Meta,ToleranciaDia,Nivel11,Nivel12,Nivel13,Avaliacoes,Amostras,AddDate,AlterDate,ParCompany_id,ParLevel1_id")] VolumePcc1b pcc1b)
        {
            ValidaPcc1B(pcc1b);

            if (ModelState.IsValid)
            {
                db.Entry(pcc1b).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParCompany_id = new SelectList(db.ParCompany.OrderBy(c => c.Name), "Id", "Name", pcc1b.ParCompany_id);
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1, "Id", "Name", pcc1b.ParLevel1_id);
            return View(pcc1b);
        }

        private void ValidaPcc1B(VolumePcc1b pcc1b)
        {
            if (pcc1b.Data == null)
                ModelState.AddModelError("Data", Guard.MesangemModelError("Data", false));

            if (pcc1b.ParCompany_id == null)
                ModelState.AddModelError("ParCompany_id", Guard.MesangemModelError("Unidade", false));

            if (pcc1b.ParLevel1_id == null)
                ModelState.AddModelError("ParLevel1_id", Guard.MesangemModelError("Indicador", false));

            if (pcc1b.VolumeAnimais == null)
                ModelState.AddModelError("VolumeAnimais", Guard.MesangemModelError("Número de animais", false));
        }

        // GET: Pcc1b/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolumePcc1b pcc1b = db.VolumePcc1b.Find(id);
            if (pcc1b == null)
            {
                return HttpNotFound();
            }
            return View(pcc1b);
        }

        // POST: Pcc1b/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VolumePcc1b pcc1b = db.VolumePcc1b.Find(id);
            db.VolumePcc1b.Remove(pcc1b);
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
