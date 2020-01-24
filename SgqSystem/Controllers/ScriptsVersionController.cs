using Dominio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace SgqSystem.Controllers
{
    public class ScriptsVersionController : BaseController
    {

        public SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ScriptsVersion
        public ActionResult Index(string version)
        {
            List<string> versions;
            var listChangeLog = GetChangeLogByVersion(version, out versions);
            ViewBag.Versions = versions.OrderBy(x=>x).ToList();
            return View(listChangeLog);
        }

        public List<ChangeLog> GetChangeLogByVersion(string version, out List<string> versions)
        {
            versions = new List<string>();
            List<ChangeLog> changeLogs = new List<ChangeLog>();
            var path = Path.Combine(@AppDomain.CurrentDomain.BaseDirectory, "ChangeLog", "ChangeLog.xml");
            foreach (XElement card in XElement.Load(path).Elements("Card"))
            {
                if (version != null && card.Element("Version").Value.Contains(version))
                {
                    changeLogs.Add(new ChangeLog
                    {
                        CardNumber = card.Element("CardNumber").Value,
                        Version = card.Element("Version").Value,
                        Description = card.Element("Description").Value,
                        Script = card.Element("Script").Value
                    });
                }

                if(!versions.Any(x=>x == card.Element("Version").Value))
                {
                    versions.Add(card.Element("Version").Value);
                }

            }

            return changeLogs;
        }
    }
    public class ChangeLog
    {
        public string CardNumber { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string Script { get; set; }
    }
}