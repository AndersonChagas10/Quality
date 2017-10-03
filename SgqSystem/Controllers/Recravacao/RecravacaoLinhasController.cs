using Dominio;
using Helper;
using System.Linq;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Recravacao
{
    [CustomAuthorize]
    public class RecravacaoLinhasController : Controller
    {
        private SgqDbDevEntities db;

        public RecravacaoLinhasController()
        {
            db = new SgqDbDevEntities();
            ViewBag.ParCompany = db.ParCompany.Where(r => r.IsActive == true).ToList();
            ViewBag.TipoLata = db.Database.SqlQuery<ParRecravacao_TipoLata>("SELECT Id, Name FROM ParRecravacao_TipoLata where IsActive = 1");
            //repo = new Repo<UserSgq>();
        }

        // GET: RecravacaoTipoLata
        public ActionResult Index()
        {
            var model = db.Database.SqlQuery<ParRecravacao_Linhas>("SELECT * FROM ParRecravacao_Linhas ORDER BY IsActive").OrderByDescending(r=>r.IsActive == false).ToList();
            return View(model);
        }

        // GET: RecravacaoTipoLata/Create
        public ActionResult Create()
        {
            var model = new ParRecravacao_Linhas();
            model.IsActive = true;
            return View(model);
        }

        // GET: RecravacaoTipoLata/Edit/5
        public ActionResult Edit(int id)
        {
            ParRecravacao_Linhas model = GetTipoLinhas(id);
            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Edit(ParRecravacao_Linhas collection)
        {
            return Create(collection);
        }

        // GET: RecravacaoTipoLata/Details/5
        public ActionResult Details(int id)
        {
            ParRecravacao_Linhas model = GetTipoLinhas(id);
            return View(model);
        }

        // POST: RecravacaoTipoLata/Create
        [HttpPost]
        public ActionResult Create(ParRecravacao_Linhas collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                    Save(collection);
                else
                    return View();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private void Save(ParRecravacao_Linhas model)
        {
            if (model.Id > 0)
            {
                var update = string.Format("\n UPDATE [dbo].[ParRecravacao_Linhas] " +
                    "\n   SET[Name] = '{0}'" +
                    "\n       ,[ParCompany_Id] = {1}" +
                    "\n       ,[ParRecravacao_TypeLata_Id] = {2}" +
                    "\n       ,[NumberOfHeads] = {3}" +
                    "\n       ,[Description] = '{4}'" +
                    "\n       ,[AlterDate] = {5}" +
                    "\n       ,[IsActive] = {6}" +
                    "\n  WHERE Id = {7}"
                    , model.Name
                    , model.ParCompany_Id.ToString()
                    , model.ParRecravacao_TypeLata_Id.ToString()
                    , model.NumberOfHeads.ToString()
                    , model.Description
                    , "GETDATE()"
                    , model.IsActive ? "1" : "0"
                    , model.Id.ToString());

                db.Database.ExecuteSqlCommand(update);

            }
            else
            {
                var insert = string.Format("\n INSERT INTO [dbo].[ParRecravacao_Linhas] " +
                          "\n       ([Name] " +
                          "\n       ,[ParCompany_Id] " +
                          "\n       ,[ParRecravacao_TypeLata_Id] " +
                          "\n       ,[NumberOfHeads] " +
                          "\n       ,[Description] " +
                          "\n       ,[AddDate] " +
                          "\n       ,[IsActive]) " +
                          "\n   VALUES " +
                          "\n       (N'{0}' " +
                          "\n       ,{1} " +
                          "\n       ,{2} " +
                          "\n       ,{3} " +
                          "\n       ,'{4}' " +
                          "\n       ,{5} " +
                          "\n       ,{6}) SELECT SCOPE_IDENTITY()"
                          , model.Name
                          , model.ParCompany_Id.ToString()
                          , model.ParRecravacao_TypeLata_Id.ToString()
                          , model.NumberOfHeads.ToString()
                          , model.Description
                          , "GETDATE()" 
                          , model.IsActive ? "1" : "0"); 

                model.Id = int.Parse(db.Database.SqlQuery<decimal>(insert).FirstOrDefault().ToString());
            }
        }

        private ParRecravacao_Linhas GetTipoLinhas(int id)
        {
            var model = new ParRecravacao_Linhas();
            if (id > 0)
                model = db.Database.SqlQuery<ParRecravacao_Linhas>("SELECT * FROM ParRecravacao_Linhas WHERE Id = " + id).FirstOrDefault();
            return model;
        }
    }
}
