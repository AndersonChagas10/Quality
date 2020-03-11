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
            var listVersionName = versions.OrderBy(x => x).ToList();

            ViewBag.Versions = geVersions(listVersionName);
            return View(listChangeLog);
        }

        private List<ChangeLog> GetChangeLogByVersion(string version, out List<string> versions)
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

                if (!versions.Any(x => x == card.Element("Version").Value))
                {
                    versions.Add(card.Element("Version").Value);
                }

            }

            return changeLogs;
        }

        private List<Version> geVersions(List<string> versions)
        {

            //List<ChangeLog> changeLogs = new List<ChangeLog>();
            List<Version> Versoes = new List<Version>();
            var path = Path.Combine(@AppDomain.CurrentDomain.BaseDirectory, "ChangeLog", "ChangeLog.xml");

            var listMigrationHistory = db.MigrationHistory.ToList();

            foreach (var versao in versions)
            {
                var hasInMigrationHistory = true;

                foreach (XElement card in XElement.Load(path).Elements("Card"))
                {                   
                    if (card.Element("Version").Value.Contains(versao) && 
                        card.Element("Script").Value != null && 
                        card.Element("Script").Value != "")
                    {
                        hasInMigrationHistory = hasAnyNoInsert(card.Element("CardNumber").Value, listMigrationHistory);

                        if (!hasInMigrationHistory)
                            break;
                    }
                }

                Versoes.Add(new Version
                {
                    Name = versao,
                    HasInMigrationHistory = hasInMigrationHistory
                });
            }

            return Versoes;
        }

        //Se algum card não estiver no banco, retornar na lista de versões HasInMigrationHistory = false
        private bool hasAnyNoInsert(string CardNumber, List<MigrationHistory> listMigrationHistory)
        {
            var hasAnyNoInsert = false;

            if (listMigrationHistory.Any(x => x.Name == CardNumber))
            {
                hasAnyNoInsert = true;
            }

            return hasAnyNoInsert;
        }

    }

    public class ChangeLog
    {
        public string CardNumber { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string Script { get; set; }
    }

    public class Version
    {
        public string Name { get; set; }

        public bool HasInMigrationHistory { get; set; }

        public string Fa_Icon
        {
            get
            {
                if (this.HasInMigrationHistory)
                    return "fa fa-check-square-o";
                else
                    return "fa fa-square-o";
            }
        }
    }
}