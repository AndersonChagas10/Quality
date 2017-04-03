using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Dominio;
using SgqSystem.Secirity;
using Helper;

namespace SgqSystem.Controllers
{
    [HandleController()]
    [CustomAuthorize]
    public class ParClustersController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParClusters
        public ActionResult Index()
        {
            var parCluster = db.ParCluster.Include(p => p.ParClusterGroup);
            return View(parCluster.ToList());
        }

        // GET: ParClusters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParCluster parCluster = db.ParCluster.Find(id);
            if (parCluster == null)
            {
                return HttpNotFound();
            }
            return View(parCluster);
        }

        // GET: ParClusters/Create
        public ActionResult Create()
        {
            ViewBag.ParClusterGroup_Id = new SelectList(db.ParClusterGroup, "Id", "Name");
            return View();
        }

        // POST: ParClusters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ParClusterGroup_Id,Name,Description,ParClusterParent_Id,AddDate,AlterDate,IsActive")] ParCluster parCluster)
        {
            if (ModelState.IsValid)
            {
                parCluster.IsActive = true;
                db.ParCluster.Add(parCluster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParClusterGroup_Id = new SelectList(db.ParClusterGroup, "Id", "Name", parCluster.ParClusterGroup_Id);
            return View(parCluster);
        }

        // GET: ParClusters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParCluster parCluster = db.ParCluster.Find(id);
            if (parCluster == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParClusterGroup_Id = new SelectList(db.ParClusterGroup, "Id", "Name", parCluster.ParClusterGroup_Id);
            return View(parCluster);
        }

        // POST: ParClusters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ParClusterGroup_Id,Name,Description,ParClusterParent_Id,AddDate,AlterDate,IsActive")] ParCluster parCluster)
        {
            if (ModelState.IsValid)
            {
                parCluster.IsActive = true;
                db.Entry(parCluster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParClusterGroup_Id = new SelectList(db.ParClusterGroup, "Id", "Name", parCluster.ParClusterGroup_Id);
            return View(parCluster);
        }

        // GET: ParClusters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParCluster parCluster = db.ParCluster.Find(id);
            if (parCluster == null)
            {
                return HttpNotFound();
            }
            return View(parCluster);
        }

        // POST: ParClusters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParCluster parCluster = db.ParCluster.Find(id);
            db.ParCluster.Remove(parCluster);
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
