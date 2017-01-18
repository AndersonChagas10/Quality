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
    public class VacuoGRDsController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: VacuoGRDs
        public ActionResult Index()
        {
            var vacuoGRD = db.VolumeVacuoGRD.Include(v => v.ParCompany).Include(v => v.ParLevel1);
            return View(vacuoGRD.ToList());
        }

        // GET: VacuoGRDs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolumeVacuoGRD vacuoGRD = db.VolumeVacuoGRD.Find(id);
            if (vacuoGRD == null)
            {
                return HttpNotFound();
            }
            return View(vacuoGRD);
        }

        // GET: VacuoGRDs/Create
        public ActionResult Create()
        {
            ViewBag.ParCompany_id = new SelectList(db.ParCompany.OrderBy(c => c.Name), "Id", "Name");
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1.Where(c => c.Id == 22), "Id", "Name");
            var model = new VolumeVacuoGRD();
            GetNumeroDeFamilias(model);
            return View(model);
        }

        // POST: VacuoGRDs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Indicador,Unidade,Data,Departamento,HorasTrabalhadasPorDia,AmostraPorDia,QtdadeFamiliaProduto,Avaliacoes,Amostras,AddDate,AlterDate,ParCompany_id,ParLevel1_id")] VolumeVacuoGRD vacuoGRD)
        {
            GetNumeroDeFamilias(vacuoGRD);
            ValidaVacuoGRD(vacuoGRD);

            if (ModelState.IsValid)
            {
                db.VolumeVacuoGRD.Add(vacuoGRD);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParCompany_id = new SelectList(db.ParCompany.OrderBy(c => c.Name), "Id", "Name", vacuoGRD.ParCompany_id);
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1, "Id", "Name", vacuoGRD.ParLevel1_id);
            return View(vacuoGRD);
        }

        private void ValidaVacuoGRD(VolumeVacuoGRD vacuoGRD)
        {
            if (vacuoGRD.Data == null)
                ModelState.AddModelError("Data", Guard.MesangemModelError("Data", false));

            if (vacuoGRD.HorasTrabalhadasPorDia == null)
                ModelState.AddModelError("HorasTrabalhadasPorDia", Guard.MesangemModelError("Horas trabalhadas por dia", false));

            if (vacuoGRD.QtdadeFamiliaProduto == null)
                ModelState.AddModelError("QtdadeFamiliaProduto", Guard.MesangemModelError("Número de famílias cadastradas", false));
        }

        // GET: VacuoGRDs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolumeVacuoGRD vacuoGRD = db.VolumeVacuoGRD.Find(id);
            if (vacuoGRD == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParCompany_id = new SelectList(db.ParCompany.OrderBy(c => c.Name), "Id", "Name", vacuoGRD.ParCompany_id);
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1.Where(c => c.Id == 22), "Id", "Name", vacuoGRD.ParLevel1_id);
            GetNumeroDeFamilias(vacuoGRD);
            return View(vacuoGRD);
        }

        // POST: VacuoGRDs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Indicador,Unidade,Data,Departamento,HorasTrabalhadasPorDia,AmostraPorDia,QtdadeFamiliaProduto,Avaliacoes,Amostras,AddDate,AlterDate,ParCompany_id,ParLevel1_id")] VolumeVacuoGRD vacuoGRD)
        {
            GetNumeroDeFamilias(vacuoGRD);
            ValidaVacuoGRD(vacuoGRD);
            if (ModelState.IsValid)
            {
                db.Entry(vacuoGRD).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParCompany_id = new SelectList(db.ParCompany.OrderBy(c => c.Name), "Id", "Name", vacuoGRD.ParCompany_id);
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1, "Id", "Name", vacuoGRD.ParLevel1_id);
            return View(vacuoGRD);
        }

        // GET: VacuoGRDs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolumeVacuoGRD vacuoGRD = db.VolumeVacuoGRD.Find(id);
            if (vacuoGRD == null)
            {
                return HttpNotFound();
            }
            return View(vacuoGRD);
        }

        // POST: VacuoGRDs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VolumeVacuoGRD vacuoGRD = db.VolumeVacuoGRD.Find(id);
            db.VolumeVacuoGRD.Remove(vacuoGRD);
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

        private void GetNumeroDeFamilias(VolumeVacuoGRD model)
        {
            model.QtdadeFamiliaProduto = db.ParLevel1.AsNoTracking().FirstOrDefault(r => r.hashKey == 3).Level2Number;
            model.ParLevel1_id = db.ParLevel1.AsNoTracking().FirstOrDefault(r => r.hashKey == 3).Id;
        }
    }
}
