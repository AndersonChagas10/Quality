using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Dominio;
using System.Data.Entity.Infrastructure;
using System;
using SgqSystem.Secirity;

namespace SgqSystem.Controllers
{
    [CustomAuthorize]
    public class CepRecortesController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();
        private IObjectContextAdapter ctx;

        public int getNQA(string nivel, string tamanhoLote)
        {

            int amostra = 0;

            int loteInt = Int32.Parse(tamanhoLote);
            var L2EQuery = from nqa in db.NQA
                           where nqa.NivelGeralInspecao.ToString() == nivel
                           where loteInt >= nqa.TamanhoLoteMin
                           where loteInt <= nqa.TamanhoLoteMax
                           select nqa;

            var result = L2EQuery.FirstOrDefault();

            if (result != null)
            {
                amostra = result.Amostra;
            }

            return amostra ;
        }

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

            if (cepRecortes.Data == null)
                ModelState.AddModelError("Data", "O campo Data precisa ser preenchido.");

            if (cepRecortes.ParCompany_id == null || cepRecortes.ParCompany_id <= 0)
                ModelState.AddModelError("ParCompany_id", "É necessário selecionar uma Empresa.");

            if (cepRecortes.ParLevel1_id == null || cepRecortes.ParLevel1_id <= 0)
                ModelState.AddModelError("ParLevel1_id", "É necessário selecionar um Indicador.");

            if (cepRecortes.HorasTrabalhadasPorDia == null)
                ModelState.AddModelError("HorasTrabalhadasPorDia", "O campo \"Horas trabalhadas por dia\" precisa ser preenchido.");

            if (cepRecortes.QtdadeMediaKgRecProdDia == null)
                ModelState.AddModelError("QtdadeMediaKgRecProdDia", "O campo \"Quantidade Média em KG de Recortes Produzidos Diáriamente\" precisa ser preenchido.");

            if (cepRecortes.NBR == null || cepRecortes.NBR <= 0)
                ModelState.AddModelError("NBR", "É necessário selecionar um NBR - Nível Geral de Inspeção Escolhido.");

            if (cepRecortes.QtadeTrabEsteiraRecortes == null)
                ModelState.AddModelError("QtadeTrabEsteiraRecortes", "O campo \"Quantidade de Colaboradores Ou Esteiras de Recortes\" precisa ser preenchido.");

            if (cepRecortes.TotalAvaliaColaborEsteirHoraProd == null)
                ModelState.AddModelError("TotalAvaliaColaborEsteirHoraProd", "O campo \"Total em KG Para Avaliação Por Colaborador/Esteira, Por Hora de Produção\" precisa ser preenchido.");

            if (cepRecortes.TamanhoAmostra == null)
                ModelState.AddModelError("TamanhoAmostra", "O campo \"Tamanho de Cada Amostra\" precisa ser preenchido.");

            if (ModelState.IsValid)
            {
                cepRecortes.AddDate = DateTime.Now;
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
