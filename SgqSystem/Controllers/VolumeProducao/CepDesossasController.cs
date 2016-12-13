using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Dominio;

namespace SgqSystem.Controllers
{
    public class CepDesossasController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();
        
        // GET: CepDesossas
        public ActionResult Index()
        {
            var cepDesossa = db.VolumeCepDesossa.Include(c => c.ParCompany).Include(c => c.ParLevel1);
            return View(cepDesossa.ToList());
        }

        // GET: CepDesossas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolumeCepDesossa cepDesossa = db.VolumeCepDesossa.Find(id);
            if (cepDesossa == null)
            {
                return HttpNotFound();
            }
            return View(cepDesossa);
        }

        // GET: CepDesossas/Create
        public ActionResult Create()
        {
            ViewBag.ParCompany_id = new SelectList(db.ParCompany, "Id", "Name");
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1, "Id", "Name");
            return View();
        }

        // POST: CepDesossas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Indicador,Unidade,Data,Departamento,HorasTrabalhadasPorDia,AmostraPorDia,QtdadeFamiliaProduto,Avaliacoes,Amostras,AddDate,AlterDate,ParCompany_id,ParLevel1_id")] VolumeCepDesossa cepDesossa)
        {
            if (ModelState.IsValid)
            {
                db.VolumeCepDesossa.Add(cepDesossa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParCompany_id = new SelectList(db.ParCompany, "Id", "Name", cepDesossa.ParCompany_id);
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1, "Id", "Name", cepDesossa.ParLevel1_id);
            return View(cepDesossa);
        }

        // GET: CepDesossas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolumeCepDesossa cepDesossa = db.VolumeCepDesossa.Find(id);
            if (cepDesossa == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParCompany_id = new SelectList(db.ParCompany, "Id", "Name", cepDesossa.ParCompany_id);
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1, "Id", "Name", cepDesossa.ParLevel1_id);
            return View(cepDesossa);
        }

        // POST: CepDesossas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Indicador,Unidade,Data,Departamento,HorasTrabalhadasPorDia,AmostraPorDia,QtdadeFamiliaProduto,Avaliacoes,Amostras,AddDate,AlterDate,ParCompany_id,ParLevel1_id")] VolumeCepDesossa cepDesossa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cepDesossa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParCompany_id = new SelectList(db.ParCompany, "Id", "Name", cepDesossa.ParCompany_id);
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1, "Id", "Name", cepDesossa.ParLevel1_id);
            return View(cepDesossa);
        }

        // GET: CepDesossas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolumeCepDesossa cepDesossa = db.VolumeCepDesossa.Find(id);
            if (cepDesossa == null)
            {
                return HttpNotFound();
            }
            return View(cepDesossa);
        }

        // POST: CepDesossas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VolumeCepDesossa cepDesossa = db.VolumeCepDesossa.Find(id);
            db.VolumeCepDesossa.Remove(cepDesossa);
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
