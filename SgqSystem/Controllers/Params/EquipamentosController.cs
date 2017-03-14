using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Dominio;
using DTO.Helpers;
using SgqSystem.Secirity;
using SgqSystem.Helpers;
using Helper;

namespace SgqSystem.Controllers.Params
{
    [FilterUnit]
    [CustomAuthorize]
    public class EquipamentosController : Controller
    {
   
        private SGQ_GlobalEntities db = new SGQ_GlobalEntities();
        private SgqDbDevEntities db2 = new SgqDbDevEntities();

        // GET: Equipamentos
        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult Index()
        {
            var userId = Guard.GetUsuarioLogado_Id(HttpContext);

            var equipamentos = db.Equipamentos.Where(eqp => true);

            //Company filter
            if (!string.IsNullOrEmpty(Request.QueryString["ParCompanyName"]))
            {
                string parCompanyName = Request.QueryString["ParCompanyName"];
                equipamentos = equipamentos.Where(eqp => eqp.ParCompanyName == parCompanyName);
            }

            if (equipamentos.Count() > 0)
                return View(equipamentos.ToList());
            else
                return View(new System.Collections.Generic.List<Equipamentos>());
        }

        // GET: Equipamentos/Details/5
        public ActionResult Details(int? id)
        {
            /*if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolumeCepDesossa cepDesossa = db.VolumeCepDesossa.Find(id);
            if (cepDesossa == null)
            {
                return HttpNotFound();
            }
            return View(cepDesossa);*/

            return View();
        }

        // GET: Equipamentos/Create
        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Equipamentos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ParCompanyName,Nome,Tipo,Subtipo")] Equipamentos equipamentos)
        {
            ValidaCepDesossa(equipamentos);
            if (ModelState.IsValid)
            {
                equipamentos.DataInsercao = System.DateTime.Now;
                equipamentos.UsuarioInsercao = Guard.GetUsuarioLogado_Id(HttpContext);
                db.Equipamentos.Add(equipamentos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(equipamentos);
        }

        private void ValidaCepDesossa(Equipamentos equipamentos)
        {
            if (equipamentos.ParCompanyName == null)
                ModelState.AddModelError("ParCompanyName", Guard.MesangemModelError("Unidade", true));

            if (equipamentos.Nome == null)
                ModelState.AddModelError("Nome", Guard.MesangemModelError("Nome", true));

            if (equipamentos.Tipo == null)
                ModelState.AddModelError("Tipo", Guard.MesangemModelError("Tipo", true));

            if (equipamentos.Subtipo == null)
                ModelState.AddModelError("Subtipo", Guard.MesangemModelError("Subtipo", true));
        }

        // GET: Equipamentos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipamentos equipamentos = db.Equipamentos.Find(id);
            if (equipamentos == null)
            {
                return HttpNotFound();
            }

            return View(equipamentos);
        }

        // POST: Equipamentos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ParCompanyName,Nome,Tipo,Subtipo")] Equipamentos equipamentos)
        {
            ValidaCepDesossa(equipamentos);

            if (ModelState.IsValid)
            {
                equipamentos.DataInsercao = System.DateTime.Now;
                equipamentos.UsuarioAlteracao = Guard.GetUsuarioLogado_Id(HttpContext);
                db.Entry(equipamentos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(equipamentos);
        }

        // GET: Equipamentos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipamentos equipamentos = db.Equipamentos.Find(id);
            if (equipamentos == null)
            {
                return HttpNotFound();
            }
            return View(equipamentos);
        }

        // POST: Equipamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Equipamentos equipamentos = db.Equipamentos.Find(id);
            db.Equipamentos.Remove(equipamentos);
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
