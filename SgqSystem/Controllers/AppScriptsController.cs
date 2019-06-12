using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dominio;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace SgqSystem.Controllers
{
    public class AppScriptsController : Controller
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: AppScripts
        public string GetByVersion(string version)
        {
            //myString = myString.Replace(System.Environment.NewLine, "replacement text")
            var scripts = db.AppScript.Where(x => x.Version == version).ToList();

            var scriptsList = new List<Dictionary<string, string>>();

            foreach (var item in scripts)
            {
                var scriptDctionary = new Dictionary<string, string>();

                if (System.Configuration.ConfigurationManager.AppSettings["Producao"] == "SIM")
                {
                    #region ReduzScript (Minify)
                    var blockComments = @"/\*(.*?)\*/";
                    var lineComments = @"//(.*?)\r?\n";
                    var strings = @"""((\\[^\n]|[^""\n])*)""";
                    var verbatimStrings = @"@(""[^""]*"")+";

                    item.Script = Regex.Replace(item.Script,
                        blockComments + "|" + lineComments + "|" + strings + "|" + verbatimStrings,
                        me =>
                        {
                            if (me.Value.StartsWith("/*") || me.Value.StartsWith("//"))
                                return me.Value.StartsWith("//") ? Environment.NewLine : "";
                            // Keep the literal strings
                            return me.Value;
                        },
                        RegexOptions.Singleline);

                    var x = item.Script.Length;
                    RegexOptions options = RegexOptions.None;
                    Regex regex = new Regex("[ ]{2,}", options);
                    item.Script = regex.Replace(item.Script, " ");

                    scriptDctionary.Add(item.ArchiveName
                        , item.Script.Replace(System.Environment.NewLine, "").Replace("\\\"", "\'"));
                    #endregion
                }
                else
                {
                    scriptDctionary.Add(item.ArchiveName, item.Script);
                }

                scriptsList.Add(scriptDctionary);
            }

            return JsonConvert.SerializeObject(scriptsList);
        }

        // GET: AppScripts
        public ActionResult Index()
        {
            return View(db.AppScript.ToList());
        }

        // GET: AppScripts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppScript appScript = db.AppScript.Find(id);
            if (appScript == null)
            {
                return HttpNotFound();
            }
            return View(appScript);
        }

        // GET: AppScripts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AppScripts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Version,ArchiveName,Script")] AppScript appScript)
        {
            ValidaCampos(appScript);
            if (ModelState.IsValid)
            {
                db.AppScript.Add(appScript);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(appScript);
        }

        private void ValidaCampos(AppScript appScript)
        {
            if (appScript.Version == null)
                ModelState.AddModelError("Version", Resources.Resource.required_field + " " + "Versão");

            if (appScript.Script == null)
                ModelState.AddModelError("Script", Resources.Resource.required_field + " " + "Script");

            if (appScript.ArchiveName == null)
                ModelState.AddModelError("ArchiveName", Resources.Resource.required_field + " " + "Nome do Arquivo");
        }

        // GET: AppScripts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppScript appScript = db.AppScript.Find(id);
            if (appScript == null)
            {
                return HttpNotFound();
            }
            return View(appScript);
        }

        // POST: AppScripts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Version,ArchiveName,Script")] AppScript appScript)
        {
            ValidaCampos(appScript);
            if (ModelState.IsValid)
            {
                db.Entry(appScript).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appScript);
        }

        // GET: AppScripts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppScript appScript = db.AppScript.Find(id);
            if (appScript == null)
            {
                return HttpNotFound();
            }
            return View(appScript);
        }

        // POST: AppScripts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AppScript appScript = db.AppScript.Find(id);
            db.AppScript.Remove(appScript);
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
