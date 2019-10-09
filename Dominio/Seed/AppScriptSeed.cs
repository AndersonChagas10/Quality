using Dominio.Seed;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Dominio.Seed
{
    public class AppScriptSeed
    {
        public void SetAppScript()
        {
            var appScripts = new List<AppScript>();

            using (var db = new Dominio.SgqDbDevEntities())
            {
                appScripts = db.AppScript.ToList();
            }

            string AppFilesVersion = DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.AppFiles;

            string path = AppDomain.CurrentDomain.BaseDirectory + "Scripts\\appColeta\\" + AppFilesVersion;
            string searchPattern = "*.*";
            string[] MyFiles = Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories)
                .Where(file => file.ToLower().EndsWith("js") || file.ToLower().EndsWith("css")).ToArray();

            var arquivo = new Dictionary<string, string>();
            foreach (var url in MyFiles)
            {
                var conteudo = System.IO.File.ReadAllText(url).ToString();
                var nomeArquivo = Path.GetFileName(url);

                arquivo.Add(nomeArquivo, conteudo);
            }

            //apaga todos os scrpts do banco
            using (var db = new Dominio.SgqDbDevEntities())
            {
                foreach (var item in appScripts)
                {
                    db.AppScript.Attach(item);
                    db.AppScript.Remove(item);
                    db.SaveChanges();
                }

                //Salvar os scripts atuais
                foreach (var item in arquivo)
                {
                    var newAppScripts = new AppScript();

                    newAppScripts.ArchiveName = item.Key;
                    newAppScripts.Script = item.Value;
                    newAppScripts.Version = AppFilesVersion;

                    db.AppScript.Add(newAppScripts);
                    db.SaveChanges();
                }

            }
        }
    }
}