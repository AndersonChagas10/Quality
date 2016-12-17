using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Dominio;

namespace SgqSystem.Controllers
{
    public class CepRecortesController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: CepRecortes
        public ActionResult Index()
        {
            var cepRecortes = db.VolumeCepRecortes.Include(c => c.ParCompany).Include(c => c.ParLevel1);
            return View(cepRecortes.ToList());
        }

        // GET: CepRecortes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolumeCepRecortes cepRecortes = db.VolumeCepRecortes.Find(id);
            if (cepRecortes == null)
            {
                return HttpNotFound();
            }
            return View(cepRecortes);
        }

        // GET: CepRecortes/Create
        public ActionResult Create()
        {
            ViewBag.ParCompany_id = new SelectList(db.ParCompany.OrderBy(c => c.Name), "Id", "Name");
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1.Where(c => c.Id == 23), "Id", "Name");
            return View();
        }

        // POST: CepRecortes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Indicador,Unidade,Data,Departamento,HorasTrabalhadasPorDia,QtdadeMediaKgRecProdDia,QtdadeMediaKgRecProdHora,NBR,TotalKgAvaliaHoraProd,QtadeTrabEsteiraRecortes,TotalAvaliaColaborEsteirHoraProd,TamanhoAmostra,TotalAmostraAvaliaColabEsteiraHoraProd,Avaliacoes,Amostras,AddDate,AlterDate,ParCompany_id,ParLevel1_id")] VolumeCepRecortes cepRecortes)
        {
            if (ModelState.IsValid)
            {
                db.VolumeCepRecortes.Add(cepRecortes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParCompany_id = new SelectList(db.ParCompany.OrderBy(c => c.Name), "Id", "Name", cepRecortes.ParCompany_id);
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1, "Id", "Name", cepRecortes.ParLevel1_id);
            return View(cepRecortes);
        }

        // GET: CepRecortes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolumeCepRecortes cepRecortes = db.VolumeCepRecortes.Find(id);
            if (cepRecortes == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParCompany_id = new SelectList(db.ParCompany.OrderBy(c => c.Name), "Id", "Name", cepRecortes.ParCompany_id);
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1.Where(c => c.Id == 23), "Id", "Name", cepRecortes.ParLevel1_id);
            return View(cepRecortes);
        }

        // POST: CepRecortes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Indicador,Unidade,Data,Departamento,HorasTrabalhadasPorDia,QtdadeMediaKgRecProdDia,QtdadeMediaKgRecProdHora,NBR,TotalKgAvaliaHoraProd,QtadeTrabEsteiraRecortes,TotalAvaliaColaborEsteirHoraProd,TamanhoAmostra,TotalAmostraAvaliaColabEsteiraHoraProd,Avaliacoes,Amostras,AddDate,AlterDate,ParCompany_id,ParLevel1_id")] VolumeCepRecortes cepRecortes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cepRecortes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParCompany_id = new SelectList(db.ParCompany.OrderBy(c => c.Name), "Id", "Name", cepRecortes.ParCompany_id);
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1, "Id", "Name", cepRecortes.ParLevel1_id);
            return View(cepRecortes);
        }

        // GET: CepRecortes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolumeCepRecortes cepRecortes = db.VolumeCepRecortes.Find(id);
            if (cepRecortes == null)
            {
                return HttpNotFound();
            }
            return View(cepRecortes);
        }

        // POST: CepRecortes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VolumeCepRecortes cepRecortes = db.VolumeCepRecortes.Find(id);
            db.VolumeCepRecortes.Remove(cepRecortes);
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
