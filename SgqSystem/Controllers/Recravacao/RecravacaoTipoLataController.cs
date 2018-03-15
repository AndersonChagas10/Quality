using System.Linq;
using System.Web.Mvc;
using Dominio;
using Helper;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Data.SqlClient;

namespace SgqSystem.Controllers.Recravacao
{
    [CustomAuthorize]
    public class RecravacaoTipoLataController : Controller
    {
        private SgqDbDevEntities db;
        public RecravacaoTipoLataController()
        {
            db = new SgqDbDevEntities();
            //repo = new Repo<UserSgq>();
        }

        // GET: RecravacaoTipoLata
        public ActionResult Index()
        {
            var model = db.Database.SqlQuery<ParRecravacao_TipoLata>("SELECT * FROM ParRecravacao_TipoLata").OrderByDescending(r => r.IsActive).ToList();
            return View(model);
        }

        // GET: RecravacaoTipoLata/Create
        public ActionResult Create()
        {
            var model = new ParRecravacao_TipoLataDTO();
            model.ParLataImagensList = new List<ParLataImagens>();
            model.IsActive = true;
            return View(model);
        }

        // GET: RecravacaoTipoLata/Edit/5
        public ActionResult Edit(int id)
        {
            ParRecravacao_TipoLataDTO model = GetTipoLata(id);

            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Edit(ParRecravacao_TipoLataDTO parRecravacao_TipoLata, IEnumerable<HttpPostedFileBase> files)
        {
            return Create(parRecravacao_TipoLata, files);
        }

        // GET: RecravacaoTipoLata/Details/5
        public ActionResult Details(int id)
        {
            ParRecravacao_TipoLata model = GetTipoLata(id);
            return View(model);
        }

        // POST: RecravacaoTipoLata/Create
        [HttpPost]
        public ActionResult Create(ParRecravacao_TipoLataDTO parRecravacao_TipoLata, IEnumerable<HttpPostedFileBase> files)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {

                    Save(parRecravacao_TipoLata);
                    var counter = 0;
                    foreach (var file in files)
                    {
                        if (file != null)
                            if (file.ContentLength > 0)
                            {
                                var fileName = "_recravacao_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + Path.GetFileName(file.FileName);
                                var mapPath = Server.MapPath("~/Imagens/TempData");
                                Directory.CreateDirectory(mapPath);
                                var path = Path.Combine(mapPath, fileName);
                                file.SaveAs(path);
                                var imagem = Image.FromStream(file.InputStream, true, true);

                                var parLataImagens = new ParLataImagens() { AddDate = DateTime.Now };
                                parLataImagens.Imagem = ImageToByteArray(imagem);
                                parLataImagens.ParRecravacao_TipoLata_Id = parRecravacao_TipoLata.Id;
                                parLataImagens.PathFile = path;
                                parLataImagens.FileName = fileName;
                                parLataImagens.PontoIndex = (counter + 1);

                                parRecravacao_TipoLata.ParLataImagensList = new List<ParLataImagens>();
                                parRecravacao_TipoLata.ParLataImagensList.Add(parLataImagens);

                                db.ParLataImagens.Add(parLataImagens);
                                db.SaveChanges();
                            }

                        counter++;
                    }
                }
                else
                    return View(parRecravacao_TipoLata);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View(parRecravacao_TipoLata);
            }
        }

        private void Save(ParRecravacao_TipoLata model)
        {
            if (model.Id > 0)
            {
                var update = string.Format("\n UPDATE [dbo].[ParRecravacao_TipoLata] " +
                    "\n   SET[Name] = N'{0}'" +
                    "\n      ,[Description] = N'{1}'" +
                    "\n      ,[NumberOfPoints] = {2}" +
                    "\n      ,[AlterDate] = {3}" +
                    "\n      ,[IsActive] = {4}" +
                    "\n WHERE Id = {5}"
                    , model.Name
                    , model.Description
                    , model.NumberOfPoints.ToString()
                    , "GETDATE()"
                    , model.IsActive ? "1" : "0"
                    , model.Id
                    );

                db.Database.ExecuteSqlCommand(update);

            }
            else
            {
                var insert = string.Format("\n INSERT INTO[dbo].[ParRecravacao_TipoLata] " +
                    "\n        ([Name]                          " +
                    "\n        ,[Description]                   " +
                    "\n        ,[NumberOfPoints]                " +
                    "\n        ,[AddDate]                       " +
                    "\n        ,[IsActive])                     " +
                    "\n  VALUES                                 " +
                    "\n        (N'{0}'                           " +
                    "\n        ,N'{1}'                    " +
                    "\n        ,{2}                         " +
                    "\n        ,{3}                       " +
                    "\n        ,{4} ) SELECT SCOPE_IDENTITY()"
                    , model.Name
                    , model.Description
                    , model.NumberOfPoints.ToString()
                    , "GETDATE()"
                    , model.IsActive ? "1" : "0");

                model.Id = int.Parse(db.Database.SqlQuery<decimal>(insert).FirstOrDefault().ToString());
            }
        }

        private ParRecravacao_TipoLataDTO GetTipoLata(int id)
        {
            var model = new ParRecravacao_TipoLataDTO();
            if (id > 0)
                model = db.Database.SqlQuery<ParRecravacao_TipoLataDTO>("SELECT * FROM ParRecravacao_TipoLata WHERE Id = " + id).FirstOrDefault();

            model.ParLataImagensList = new List<ParLataImagens>();
            model.ParLataImagensList = db.ParLataImagens.Where(r => r.ParRecravacao_TipoLata_Id == model.Id).OrderBy(r => r.PontoIndex).ToList();
            return model;
        }

        private byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        private Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

    }

    public class ParRecravacao_TipoLataDTO : ParRecravacao_TipoLata
    {
        public List<ParLataImagens> ParLataImagensList { get; set; }
        public List<Image> ImageList { get; set; }
    }

}
