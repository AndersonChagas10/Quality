using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Dominio;
using DTO.Helpers;
using SgqSystem.Secirity;
using Helper;
using System.Collections.Generic;

namespace SgqSystem.Controllers.Params
{
    [FilterUnit]
    [CustomAuthorize]
    public class EquipamentosController : BaseController
    {
   
        private SGQ_GlobalEntities db = new SGQ_GlobalEntities();
        private SgqDbDevEntities db2 = new SgqDbDevEntities();

        // GET: Equipamentos
        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult Index()
        {
            var userId = Guard.GetUsuarioLogado_Id(HttpContext);

            //var company = 
            var user = db2.UserSgq.FirstOrDefault(r => r.Id == userId); //db2.Database.ExecuteSqlCommand("SELECT ParCompany_Id FROM UserSgq where id = 56");
            var company = db2.ParCompany.FirstOrDefault(r => r.Id == user.ParCompany_Id);
            var parCompanyXUserSgq = db2.ParCompanyXUserSgq.Where(r => r.UserSgq_Id == user.Id).ToList();
            var equipamentos = db.Equipamentos.ToList();

            var companys = db2.ParCompany.ToList();
            var equipRetorno = new List<Equipamentos>();

            foreach (var i in equipamentos)
            {

                i.ParCompanyName = companys.FirstOrDefault(r => r.Id == i.ParCompany_Id).Name;


                if(parCompanyXUserSgq.Any(r => r.ParCompany_Id == i.ParCompany_Id))
                    equipRetorno.Add(i);

            }

            //var company = db2.Database.ExecuteSqlCommand("SELECT ParCompany_Id FROM UserSgq where id = 56");
            //db2.
            //Company filter
            if (!string.IsNullOrEmpty(Request.QueryString["ParCompanyName"]))
            {

                int id = System.Convert.ToInt32(Request.QueryString["ParCompany_Id"]);
                //equipamentos = equipamentos.Where(eqp => eqp.ParCompany_Id == id);
            }
            
            //var result = db.Database.ExecuteSqlCommand("Select * ");


            if (equipRetorno.Count() > 0)
                return View(equipRetorno.ToList());
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
        public ActionResult Create([Bind(Include = "ParCompany_Id,Nome,Tipo")] Equipamentos equipamentos)
        {
           
            ValidaEquipamentos(equipamentos);
            if (ModelState.IsValid)
            {
                equipamentos.DataInsercao = System.DateTime.Now;
                equipamentos.UsuarioInsercao = Guard.GetUsuarioLogado_Id(HttpContext);
                equipamentos.Unidade = 1;
                db.Equipamentos.Add(equipamentos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(equipamentos);
        }

        private void ValidaEquipamentos(Equipamentos equipamentos)
        {
            if (equipamentos.ParCompany_Id == null)
                ModelState.AddModelError("ParCompany_Id", Guard.MesangemModelError("Unidade", true));

            if (equipamentos.Nome == null)
                ModelState.AddModelError("Nome", Guard.MesangemModelError("Nome", true));

            if (equipamentos.Tipo == null)
                ModelState.AddModelError("Tipo", Guard.MesangemModelError("Tipo", true));

            //if (equipamentos.Subtipo == null)
            //{
            //    equipamentos.Subtipo = "";
            //    ModelState.AddModelError("Subtipo", Guard.MesangemModelError("Subtipo", true));
            //}
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
        public ActionResult Edit([Bind(Include = "Id,ParCompany_Id,Nome,Tipo")] Equipamentos equipamentos)
        {
            ValidaEquipamentos(equipamentos);

            if (ModelState.IsValid)
            {
                equipamentos.DataInsercao = System.DateTime.Now;
                equipamentos.UsuarioAlteracao = Guard.GetUsuarioLogado_Id(HttpContext);
                equipamentos.Unidade = 1;
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
