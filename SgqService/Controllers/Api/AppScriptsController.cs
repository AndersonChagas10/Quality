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

namespace SgqService.Controllers.Api
{
    public class AppScriptsController : BaseApiController
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
                }else
                {
                    scriptDctionary.Add(item.ArchiveName, item.Script);
                }

                scriptsList.Add(scriptDctionary);
            }

            return JsonConvert.SerializeObject(scriptsList);
        }
    }
}
