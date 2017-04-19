using ADOFactory;
using Dominio;
using DTO.Helpers;
using Helper;
using SgqSystem.ViewModels;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    /// <summary>
    /// Controller para aprendizagem do sistema.
    /// </summary>
    public class ExampleController : BaseController
    {

        public void Desfaz()
        {
            using (var db = new SgqDbDevEntities())
            {
                using (var ts = db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
                {

                    db.Configuration.AutoDetectChangesEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;

                    var users = db.UserSgq.ToList();

                    foreach (var i in users)
                    {

                        var old = i.Password;
                        var nova = Guard.DecryptStringAES(old);
                        var salvar = nova;
                        i.Password = salvar;

                        db.UserSgq.Attach(i);
                        var entry = db.Entry(i);
                        entry.State = System.Data.Entity.EntityState.Modified;
                        entry.Property(e => e.Password).IsModified = true;
                    }

                    db.SaveChanges();
                    ts.Commit();

                }
            }
        }

        public void UpdatePassAES()
        {
            using (var db = new SgqDbDevEntities())
            {
                using (var ts = db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
                {

                    db.Configuration.AutoDetectChangesEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;

                    var users = db.UserSgq.ToList();

                    foreach (var i in users)
                    {

                        var old = i.Password;
                        var nova = Guard.Descriptografar3DES(old);
                        var salvar = Guard.EncryptStringAES(nova);
                        i.Password = salvar;

                        db.UserSgq.Attach(i);
                        var entry = db.Entry(i);
                        entry.State = System.Data.Entity.EntityState.Modified;
                        entry.Property(e => e.Password).IsModified = true;
                    }

                    db.SaveChanges();
                    ts.Commit();

                }
            }
        }

        public ActionResult Index()
        {
            throw new System.Exception("teste");
            using (var db = new Factory(@"SERVERGRT\MSSQLSERVER2014", "SgqDbDev", "1qazmko0", "sa"))
            {
               var results = db.SearchQuery<UserSgq>("Select * from UserSgq");
            }

            ContextExampleViewModel pvm = new ContextExampleViewModel();
            return View(pvm);
        }

        public ActionResult TesteSelect2()
        {
            //using (var db = new FactoryADO(@"SERVERGRT\MSSQLSERVER2014", "SgqDbDev", "1qazmko0", "sa"))
            //{
            //    var results = db.SearchQuery<UserSgq>("Select * from UserSgq");
            //}

            return View();
        }

        [HttpGet]
        public ActionResult TiposDeInputSgq()
        {
            return View(new ContextExampleViewModel() { IntegerProp = 2, DecimalProp = 3.983M });
        }

        [HttpPost]
        public ActionResult TiposDeInputSgq(ContextExampleViewModel model)
        {
            return View(model);
        }

       
    }

    

}