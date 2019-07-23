using Dominio;
using Helper;
using PagedList;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    [CustomAuthorize]
    public class ParDepartmentsController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParDepartments
        public ActionResult Index(int? page, string filtro = "")
        {
            var listaFilhos = db.ParDepartment.ToList();
            listaFilhos.Add(new ParDepartment() { Id = -1, Name = "Selecione" });
            ViewBag.Parent_Id = new SelectList(listaFilhos, "Id", "Name", -1);

            var listaUnidades = db.ParCompany.ToList();
            listaUnidades.Add(new ParCompany() { Id = -1, Name = "Selecione" });
            ViewBag.ParCompany_Id = new SelectList(listaUnidades, "Id", "Name", -1);
            
            List<ParDepartment> departamentos = new List<ParDepartment>();

            departamentos = db.ParDepartment.OrderBy(x => x.Id).ToList();

            if(filtro != "")
            {
                departamentos = db.ParDepartment.Where(x => x.Name.Contains(filtro)).OrderBy(x => x.Id).ToList();
                ViewBag.filtro = filtro;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);


            return View(departamentos.ToPagedList(pageNumber, pageSize));
        }

        // GET: ParDepartments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParDepartment parDepartment = db.ParDepartment.Find(id);
            if (parDepartment == null)
            {
                return HttpNotFound();
            }
            return View(parDepartment);
        }

        // GET: ParDepartments/Create
        public ActionResult Create()
        {
            MontaLista(new ParDepartment());
            return View();
        }

        // POST: ParDepartments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,AddDate,AlterDate,Active,Parent_Id,ParCompany_Id,ParDepartmentGroup_Id")] ParDepartment parDepartment)
        {
            ValidaVinculos(parDepartment);

            MontaHash(parDepartment);
            if (parDepartment.Parent_Id <= 0)
                parDepartment.Parent_Id = null;

            if (parDepartment.ParCompany_Id == 0)
                parDepartment.ParCompany_Id = null;

            if (parDepartment.ParDepartmentGroup_Id == 0)
                parDepartment.ParDepartmentGroup_Id = null;

            DepartamentoDuplicado(parDepartment);
            if (ModelState.IsValid)
            {
                db.ParDepartment.Add(parDepartment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            MontaLista(parDepartment);
            return View(parDepartment);
        }

        private void ValidaVinculos(ParDepartment parDepartment)
        {
            if (parDepartment.ParCompany_Id > 0 && parDepartment.Parent_Id > 0)
            {
                ModelState.AddModelError("ParCompany_Id", "Empresas vinculadas a um pai não podem ter vinculo com Unidades!");
            }
        }

        // GET: ParDepartments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParDepartment parDepartment = db.ParDepartment.Find(id);
            if (parDepartment == null)
            {
                return HttpNotFound();
            }

            MontaLista(parDepartment);
            return View(parDepartment);
        }

        // POST: ParDepartments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,AddDate,AlterDate,Active,Parent_Id,ParCompany_Id,ParDepartmentGroup_Id")] ParDepartment parDepartment)
        {
            MontaHash(parDepartment);
            DepartamentoDuplicado(parDepartment);

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                var parCompanyDepartmentOld = db.ParDepartment.AsNoTracking().Where(x => x.Id == parDepartment.Id).Select(x => x.ParCompany_Id).FirstOrDefault();

                if (parDepartment.ParCompany_Id == 0)
                    parDepartment.ParCompany_Id = null;

                if (parDepartment.ParDepartmentGroup_Id == 0)
                    parDepartment.ParDepartmentGroup_Id = null;

                if (parDepartment.ParCompany_Id != parCompanyDepartmentOld && parDepartment.Parent_Id == null || (parDepartment.ParDepartmentGroup_Id != null))
                    AlteraParCompanyFilhos(parDepartment);

                if (ModelState.IsValid)
                {
                    db.Entry(parDepartment).State = EntityState.Modified;
                    
                    if (parDepartment.Parent_Id > 0)
                        db.Entry(parDepartment).Property(x => x.ParCompany_Id).IsModified = false;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            MontaLista(parDepartment);
            return View(parDepartment);
        }

        private void AlteraParCompanyFilhos(ParDepartment parDepartment)
        {
            string idPai = parDepartment.Id.ToString() + '|';
            using (SgqDbDevEntities dbEntities = new SgqDbDevEntities())
            {
                var filhos = dbEntities.ParDepartment.Where(x => x.Active
                    && (x.Parent_Id == parDepartment.Id || x.Hash.StartsWith(idPai))).ToList();

                foreach (var item in filhos)
                {
                    item.ParCompany_Id = parDepartment.ParCompany_Id;
                    item.ParDepartmentGroup_Id = parDepartment.ParDepartmentGroup_Id;
                    dbEntities.SaveChanges();
                }
            }
        }

        //private void VincularDepartamentoAoGrupoDeDepartamentos(ParDepartment parDepartment)
        //{
        //    string idPai = parDepartment.Id.ToString() + '|';
        //    using (SgqDbDevEntities dbEntities = new SgqDbDevEntities())
        //    {
        //        var filhos = dbEntities.ParDepartment.Where(x => x.Active
        //            && (x.Parent_Id == parDepartment.Id || x.Hash.StartsWith(idPai))).ToList();

        //        foreach (var item in filhos)
        //        {
        //            item.ParDepartmentGroup_Id = parDepartment.ParDepartmentGroup_Id;
        //            dbEntities.SaveChanges();
        //        }
        //    }
        //}

        // GET: ParDepartments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParDepartment parDepartment = db.ParDepartment.Find(id);
            if (parDepartment == null)
            {
                return HttpNotFound();
            }
            return View(parDepartment);
        }

        // POST: ParDepartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParDepartment parDepartment = db.ParDepartment.Find(id);
            db.ParDepartment.Remove(parDepartment);
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

        private void MontaHash(ParDepartment parDepartment)
        {
            ParDepartment pai = new ParDepartment();
            if (parDepartment.Parent_Id > 0)
                pai = db.ParDepartment.AsNoTracking().Where(x => x.Id == parDepartment.Parent_Id).FirstOrDefault();
            else
                pai = null;

            if (pai != null)
            {
                parDepartment.ParCompany_Id = pai.ParCompany_Id;

                if (pai.Hash != null)
                {
                    parDepartment.Hash = pai.Hash + "|" + pai.Id;
                }
                else
                {
                    parDepartment.Hash = pai.Id.ToString();
                }
            }
            else
            {
                parDepartment.Hash = null;
            }
        }

        private void MontaLista(ParDepartment parDepartment)
        {
            ViewBag.TemFilhos = db.ParDepartment.Any(x => x.Parent_Id == parDepartment.Id && x.Active);

            var listaFilhos = db.ParDepartment.Where(x => x.Active).ToList();
            listaFilhos.Insert(0, new ParDepartment() { Id = 0, Name = "Selecione" });
            listaFilhos.Remove(parDepartment);
            ViewBag.Parent_Id = new SelectList(listaFilhos, "Id", "Name", parDepartment.Parent_Id);

            var listaUnidades = db.ParCompany.Where(x => x.IsActive).ToList();
            listaUnidades.Insert(0, new ParCompany() { Id = 0, Name = "Selecione" });
            ViewBag.ParCompany_Id = new SelectList(listaUnidades, "Id", "Name", parDepartment.ParCompany_Id);

            var listaGrupoDepartamentos = db.ParDepartmentGroup.Where(x => x.IsActive).ToList();
            listaGrupoDepartamentos.Insert(0, new ParDepartmentGroup() { Id = 0, Name = "Selecione" });
            ViewBag.ParDepartmentGroup_Id = new SelectList(listaGrupoDepartamentos, "Id", "Name", parDepartment.ParDepartmentGroup_Id);

            ViewBag.Parents = db.ParDepartment.Where(x => x.Id == parDepartment.Parent_Id).ToList()
             .Select(x => new KeyValuePair<int, string>(x.Id, x.Id + "- " + x.Name))
             .ToList();

            if (ViewBag.Parents.Count == 0)
            {
                var semDados = new List<KeyValuePair<int, string>>() {
                new KeyValuePair<int, string>(0, ""),

            };
                ViewBag.Parents = semDados;
            }

        }

        private void DepartamentoDuplicado(ParDepartment parDepartment)
        {
            if (db.ParDepartment.Any(x => x.Name == parDepartment.Name && x.Id != parDepartment.Id))
            {
                ModelState.AddModelError("", Resources.Resource.duplicated_department);
            }
        }
    }
}
