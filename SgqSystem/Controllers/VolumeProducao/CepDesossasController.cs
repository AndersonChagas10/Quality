using Dominio;
using DTO.Helpers;
using Helper;
using SgqSystem.Helpers;
using SgqSystem.Secirity;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    [FilterUnit]
    [CustomAuthorize]
    public class CepDesossasController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: CepDesossas
        public ActionResult Index()
        {
            var userId = Guard.GetUsuarioLogado_Id(HttpContext);
            var userLogado = db.UserSgq.Where(r => r.Id == userId);
            var cepDesossa = db.VolumeCepDesossa.Where(VCD => userLogado.FirstOrDefault().ParCompanyXUserSgq.Any(c => c.ParCompany_Id == VCD.ParCompany_id) || VCD.ParCompany_id == userLogado.FirstOrDefault().ParCompany_Id).Include(c => c.ParCompany).Include(c => c.ParLevel1).OrderByDescending(c => c.Data);
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
            ViewBag.ParCompany_id = new SelectList(db.ParCompany.OrderBy(c => c.Name), "Id", "Name");
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1.Where(c => c.Id == 2), "Id", "Name");

            var model = new VolumeCepDesossa();
            GetNumeroDeFamiliasPorUnidadeDoUsuarioDesossa(model);

            return View(model);
        }

        private void GetNumeroDeFamiliasPorUnidadeDoUsuarioDesossa(VolumeCepDesossa model)
        {
            var naoCorporativas = CommonData.GetNumeroDeFamiliasPorUnidadeDoUsuario(HttpContext, 2);
            var corporativos = CommonData.GetNumeroDeFamiliasCorporativo(HttpContext, 2);
            model.QtdadeFamiliaProduto = corporativos + naoCorporativas;
            model.ParLevel1_id = db.ParLevel1.AsNoTracking().FirstOrDefault(r => r.hashKey == 2).Id;
        }

        // POST: CepDesossas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Indicador,Unidade,Data,Departamento,HorasTrabalhadasPorDia,AmostraPorDia,QtdadeFamiliaProduto,Avaliacoes,Amostras,AddDate,AlterDate,ParCompany_id,ParLevel1_id")] VolumeCepDesossa cepDesossa)
        {
            GetNumeroDeFamiliasPorUnidadeDoUsuarioDesossa(cepDesossa);
            ValidaCepDesossa(cepDesossa);
            if (ModelState.IsValid)
            {
                db.VolumeCepDesossa.Add(cepDesossa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParCompany_id = new SelectList(db.ParCompany.OrderBy(c => c.Name), "Id", "Name", cepDesossa.ParCompany_id);
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1, "Id", "Name", cepDesossa.ParLevel1_id);
            return View(cepDesossa);
        }

        private void ValidaCepDesossa(VolumeCepDesossa cepDesossa)
        {
            if (cepDesossa.Data == null)
                ModelState.AddModelError("Data", Guard.MesangemModelError("Data", true));

            if (cepDesossa.ParCompany_id == null)
                ModelState.AddModelError("ParCompany_id", "O campo \"Unidade\" precisa ser selecionado.");

            if (cepDesossa.ParLevel1_id == null)
                ModelState.AddModelError("ParLevel1_id", "O campo \"Indicador\" precisa ser selecionado.");

            if (cepDesossa.HorasTrabalhadasPorDia == null)
                ModelState.AddModelError("HorasTrabalhadasPorDia", "O campo \"Horas trabalhadas por dia\" precisa ser preenchido.");

            if (cepDesossa.AmostraPorDia == null)
                ModelState.AddModelError("AmostraPorDia", "O campo \"Amostra por dia\" precisa ser selecionado.");

            if (cepDesossa.QtdadeFamiliaProduto == null)
                ModelState.AddModelError("QtdadeFamiliaProduto", "O campo \"Número de famílias cadastradas\" precisa ser preenchido.");
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
            ViewBag.ParCompany_id = new SelectList(db.ParCompany.OrderBy(c => c.Name), "Id", "Name", cepDesossa.ParCompany_id);
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1.Where(c => c.Id == 2), "Id", "Name", cepDesossa.ParLevel1_id);
            GetNumeroDeFamiliasPorUnidadeDoUsuarioDesossa(cepDesossa);

            return View(cepDesossa);
        }

        // POST: CepDesossas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Indicador,Unidade,Data,Departamento,HorasTrabalhadasPorDia,AmostraPorDia,QtdadeFamiliaProduto,Avaliacoes,Amostras,AddDate,AlterDate,ParCompany_id,ParLevel1_id")] VolumeCepDesossa cepDesossa)
        {
            GetNumeroDeFamiliasPorUnidadeDoUsuarioDesossa(cepDesossa);
            ValidaCepDesossa(cepDesossa);

            if (ModelState.IsValid)
            {
                db.Entry(cepDesossa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParCompany_id = new SelectList(db.ParCompany.AsNoTracking().OrderBy(c => c.Name), "Id", "Name", cepDesossa.ParCompany_id);
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
