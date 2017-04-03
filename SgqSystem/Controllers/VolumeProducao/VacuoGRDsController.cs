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
    public class VacuoGRDsController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        private void GetNumeroDeFamiliasPorUnidadeDoUsuarioVacuoGRD(VolumeVacuoGRD model, int hashKey)
        {
            hashKey = 3;
            var naoCorporativas = CommonData.GetNumeroDeFamiliasPorUnidadeDoUsuario(HttpContext, hashKey);
            var corporativos = CommonData.GetNumeroDeFamiliasCorporativo(HttpContext, hashKey);
            model.QtdadeFamiliaProduto = corporativos + naoCorporativas;
            model.ParLevel1_id = db.ParLevel1.AsNoTracking().FirstOrDefault(r => r.hashKey == hashKey).Id;
        }

        // GET: VacuoGRDs
        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult Index()
        {
            var userId = Guard.GetUsuarioLogado_Id(HttpContext);
            var userLogado = db.UserSgq.Where(r => r.Id == userId);
            var vacuoGRD = db.VolumeVacuoGRD.Where(VCD => userLogado.FirstOrDefault().ParCompanyXUserSgq.Any(c => c.ParCompany_Id == VCD.ParCompany_id) || VCD.ParCompany_id == userLogado.FirstOrDefault().ParCompany_Id).Include(c => c.ParCompany).Include(c => c.ParLevel1);
            //var vacuoGRD = db.VolumeVacuoGRD.Include(v => v.ParCompany).Include(v => v.ParLevel1).OrderByDescending(v => v.Data);

            //Date filter
            if (!string.IsNullOrEmpty(Request.QueryString["startDate"]) && !string.IsNullOrEmpty(Request.QueryString["endDate"]))
            {
                //Date filter
                System.DateTime startDate = Guard.ParseDateToSqlV2(Request.QueryString["startDate"]);
                System.DateTime endDate = Guard.ParseDateToSqlV2(Request.QueryString["endDate"]);

                vacuoGRD = vacuoGRD.Where(VCD => VCD.Data >= startDate && VCD.Data <= endDate);
            }
            else
            {
                System.DateTime startDate = System.DateTime.Now.AddDays(-2);
                System.DateTime endDate = System.DateTime.Now;
                vacuoGRD = vacuoGRD.Where(VCD => VCD.Data >= startDate && VCD.Data <= endDate);
            }

            //Company filter
            if (!string.IsNullOrEmpty(Request.QueryString["ParCompany_id"]))
            {
                int id = System.Convert.ToInt32(Request.QueryString["ParCompany_id"]);
                vacuoGRD = vacuoGRD.Where(VCD => VCD.ParCompany_id == id);
            }

            vacuoGRD = vacuoGRD.OrderByDescending(c => c.Data);

            if (vacuoGRD.Count() > 0)
                return View(vacuoGRD.ToList());
            else
                return View(new System.Collections.Generic.List<VolumeVacuoGRD>());
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
            GetNumeroDeFamiliasPorUnidadeDoUsuarioVacuoGRD(model, 3);
            return View(model);
        }

        // POST: VacuoGRDs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Indicador,Unidade,Data,Departamento,HorasTrabalhadasPorDia,AmostraPorDia,QtdadeFamiliaProduto,Avaliacoes,Amostras,AddDate,AlterDate,ParCompany_id,ParLevel1_id")] VolumeVacuoGRD vacuoGRD)
        {
            GetNumeroDeFamiliasPorUnidadeDoUsuarioVacuoGRD(vacuoGRD, 3);
            ValidaVacuoGRD(vacuoGRD);

            if (ModelState.IsValid)
            {
                //Verifica se já existe uma coleta no mesmo dia
                if (db.VolumeVacuoGRD.Where(r => r.Data == vacuoGRD.Data && r.ParCompany_id == vacuoGRD.ParCompany_id).ToList().Count() == 0)
                {

                    db.VolumeVacuoGRD.Add(vacuoGRD);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ReturnError();
                    //return View(vacuoGRD);
                }
            }

            ViewBag.ParCompany_id = new SelectList(db.ParCompany.OrderBy(c => c.Name), "Id", "Name", vacuoGRD.ParCompany_id);
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1, "Id", "Name", vacuoGRD.ParLevel1_id);
            return View(vacuoGRD);
        }

        private void ValidaVacuoGRD(VolumeVacuoGRD vacuoGRD)
        {
            if (vacuoGRD.Data == null)
                ModelState.AddModelError("Data", Guard.MesangemModelError("Data", false));

            if (vacuoGRD.ParCompany_id == null)
                ModelState.AddModelError("ParCompany_id", Guard.MesangemModelError("Unidade", false));

            if (vacuoGRD.HorasTrabalhadasPorDia == null)
                ModelState.AddModelError("HorasTrabalhadasPorDia", Guard.MesangemModelError("Horas trabalhadas por dia", false));
            else
            if (vacuoGRD.HorasTrabalhadasPorDia.Value <= 0)
                ModelState.AddModelError("HorasTrabalhadasPorDia", "O campo \"Horas trabalhadas por dia\" precisa ter valor maior que 0.");

            if (vacuoGRD.QtdadeFamiliaProduto == null)
                ModelState.AddModelError("QtdadeFamiliaProduto", Guard.MesangemModelError("Número de famílias cadastradas", false));

            /*if (vacuoGRD.Avaliacoes == null)
                ModelState.AddModelError("Avaliacoes", Guard.MesangemModelError("Avaliacoes", false));

            if (vacuoGRD.Amostras == null)
                ModelState.AddModelError("Amostras", Guard.MesangemModelError("Amostras por Avaliação", false));*/
        }

        private void ReturnError()
        {
            ModelState.AddModelError("Data", "Já existe um registro nesta data para esta unidade!");
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
            GetNumeroDeFamiliasPorUnidadeDoUsuarioVacuoGRD(vacuoGRD, 3);
            return View(vacuoGRD);
        }

        // POST: VacuoGRDs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Indicador,Unidade,Data,Departamento,HorasTrabalhadasPorDia,AmostraPorDia,QtdadeFamiliaProduto,Avaliacoes,Amostras,AddDate,AlterDate,ParCompany_id,ParLevel1_id")] VolumeVacuoGRD vacuoGRD)
        {
            GetNumeroDeFamiliasPorUnidadeDoUsuarioVacuoGRD(vacuoGRD, 3);
            ValidaVacuoGRD(vacuoGRD);
            if (ModelState.IsValid)
            {
                if (db.VolumeVacuoGRD.Where(r => r.Data == vacuoGRD.Data && r.ParCompany_id == vacuoGRD.ParCompany_id).ToList().Count() == 0)
                {
                    db.Entry(vacuoGRD).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    //Se for a edição da mesma data e parCompany
                    if (db.VolumeVacuoGRD.Where(r => r.Data == vacuoGRD.Data &&
                                                       r.ParCompany_id == vacuoGRD.ParCompany_id &&
                                                       r.Id == vacuoGRD.Id).ToList().Count() == 1)
                    {
                        using (var db2 = new SgqDbDevEntities())
                        {
                            db2.Entry(vacuoGRD).State = EntityState.Modified;
                            db2.SaveChanges();
                            return RedirectToAction("Index");
                        }

                    }
                    else
                    {
                        ReturnError();
                        //return View(vacuoGRD);
                    }
                }
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


    }
}
