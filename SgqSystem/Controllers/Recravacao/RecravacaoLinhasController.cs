using ADOFactory;
using Dominio;
using Helper;
using System.Collections.Generic;
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
            using (Factory factory = new Factory("DefaultConnection"))
            {
                db = new SgqDbDevEntities();
                ViewBag.ParCompany = db.ParCompany.Where(r => r.IsActive == true).ToList();
                ViewBag.TipoLata = factory.SearchQuery<ParRecravacao_TipoLata>("SELECT Id, Name FROM ParRecravacao_TipoLata where IsActive = 1");
                ViewBag.ParLevel2 = factory.SearchQuery<DTO.DTO.Params.ParLevel2DTO>(@"select id, name from parlevel2 where id in (select DISTINCT(Parlevel2_Id) from parlevel3level2 where id in ( select parlevel3level2_id from parlevel3level2level1 where parlevel1_id in ( select id from parlevel1 where isrecravacao = 1)))");

                //repo = new Repo<UserSgq>();
            }
        }

        // GET: RecravacaoTipoLata
        public ActionResult Index()
        {
            using (Factory factory = new Factory("DefaultConnection"))
            {
                var model = factory.SearchQuery<ParRecravacao_Linhas>("SELECT * FROM ParRecravacao_Linhas ORDER BY IsActive")
                .OrderByDescending(r => r.IsActive == false)
                .Select(r =>
                {
                    r.ParCompany = factory.SearchQuery<ParCompany>(string.Format("SELECT * FROM ParCompany where Id = {0}", r.ParCompany_Id)).FirstOrDefault();
                    r.TipoLata = factory.SearchQuery<ParRecravacao_TipoLata>(string.Format("SELECT * FROM ParRecravacao_TipoLata where Id = {0}", r.ParRecravacao_TypeLata_Id)).FirstOrDefault();
                    return r;
                })
                .ToList();
                return View(model);
            }
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

        // GET: RecravacaoTipoLata/Details/5
        public ActionResult Details(int id)
        {
            ParRecravacao_Linhas model = GetTipoLinhas(id);
            return View(model);
        }

        #region PostBack

        [HttpPost]
        public ActionResult Edit(ParRecravacao_Linhas collection)
        {
            return Create(collection);
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

        #endregion

        #region Aux

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
                    "\n       ,[ParLevel2_Id] = {8}" +
                    "\n       ,[IsActive] = {6}" +
                    "\n  WHERE Id = {7}"
                    , model.Name
                    , model.ParCompany_Id.ToString()
                    , model.ParRecravacao_TypeLata_Id.ToString()
                    , model.NumberOfHeads.ToString()
                    , model.Description
                    , "GETDATE()"
                    , model.IsActive ? "1" : "0"
                    , model.Id.ToString()
                    , model.ParLevel2_Id);

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
                          "\n       ,[ParLevel2_Id] " +
                          "\n       ,[IsActive]) " +
                          "\n   VALUES " +
                          "\n       (N'{0}' " +
                          "\n       ,{1} " +
                          "\n       ,{2} " +
                          "\n       ,{3} " +
                          "\n       ,'{4}' " +
                          "\n       ,{5} " +
                          "\n       ,{7} " +
                          "\n       ,{6}) SELECT SCOPE_IDENTITY()"
                          , model.Name
                          , model.ParCompany_Id.ToString()
                          , model.ParRecravacao_TypeLata_Id.ToString()
                          , model.NumberOfHeads.ToString()
                          , model.Description
                          , "GETDATE()"
                          , model.IsActive ? "1" : "0"
                          , model.ParLevel2_Id
                          );

                model.Id = int.Parse(db.Database.SqlQuery<decimal>(insert).FirstOrDefault().ToString());
            }
        }

        private ParRecravacao_Linhas GetTipoLinhas(int id)
        {
            using (Factory factory = new Factory("DefaultConnection"))
            {
                var model = new ParRecravacao_Linhas();
                if (id > 0)
                    model = factory.SearchQuery<ParRecravacao_Linhas>("SELECT * FROM ParRecravacao_Linhas WHERE Id = " + id).FirstOrDefault();
                return model;
            }
        }

        private List<ParRecravacao_Linhas> ListaLinhas()
        {
            using (Factory factory = new Factory("DefaultConnection"))
            {
                var model = factory.SearchQuery<ParRecravacao_Linhas>("SELECT * FROM ParRecravacao_Linhas ORDER BY IsActive").OrderByDescending(r => r.IsActive == false).ToList();
                foreach (var linha in model)
                {
                    if (linha.ParCompany_Id > 0)
                        linha.ParCompany = new ParCompany { Name = db.ParCompany.FirstOrDefault(r => r.Id == linha.ParCompany_Id).Name };
                    if (linha.ParRecravacao_TypeLata_Id > 0)
                        linha.TipoLata = new ParRecravacao_TipoLata { Name = factory.SearchQuery<ParRecravacao_TipoLata>($@"SELECT Name FROM ParRecravacao_TipoLata where id = {linha.ParRecravacao_TypeLata_Id}").FirstOrDefault().Name };
                }

                return model;
            }
        }

        #endregion
    }
}
