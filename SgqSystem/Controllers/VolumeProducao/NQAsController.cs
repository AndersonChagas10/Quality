using Dominio;
using Helper;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    [CustomAuthorize]
    public class NQAsController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: NQAs
        public ActionResult Index()
        {
            return View(db.NQA.ToList());
        }

        // GET: NQAs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NQA nQA = db.NQA.Find(id);
            if (nQA == null)
            {
                return HttpNotFound();
            }
            return View(nQA);
        }

        // GET: NQAs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NQAs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NivelGeralInspecao,TamanhoLoteMin,TamanhoLoteMax,Amostra,UsuarioInsercao,DataInsercao,UsuarioAlteracao,DataAlteracao")] NQA nQA)
        {
            if (ModelState.IsValid)
            {
                db.NQA.Add(nQA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nQA);
        }

        // GET: NQAs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NQA nQA = db.NQA.Find(id);
            if (nQA == null)
            {
                return HttpNotFound();
            }
            return View(nQA);
        }

        // POST: NQAs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NivelGeralInspecao,TamanhoLoteMin,TamanhoLoteMax,Amostra,UsuarioInsercao,DataInsercao,UsuarioAlteracao,DataAlteracao")] NQA nQA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nQA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nQA);
        }

        // GET: NQAs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NQA nQA = db.NQA.Find(id);
            if (nQA == null)
            {
                return HttpNotFound();
            }
            return View(nQA);
        }

        // POST: NQAs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NQA nQA = db.NQA.Find(id);
            db.NQA.Remove(nQA);
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
