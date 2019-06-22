using Dominio.Seed;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace Dominio.Seed
{
    public static class Seed
    {
        public static void SetSeedValues(bool isPT = false, bool runAppScripts = false)
        {
            DicionarioEstaticoSeed dicionarioSeed = new DicionarioEstaticoSeed();
            dicionarioSeed.SetDicionarioEstatico();

            using (var db = new Dominio.SgqDbDevEntities())
            {
                var x = new ExpandoObject() as IDictionary<string, object>;

                foreach (var item in db.DicionarioEstatico.ToList())
                {
                    x.Add(item.Key, item.Value);
                }
                DicionarioEstaticoGlobal.DicionarioEstaticoHelpers = x;
            }

            if (isPT)
            {
                var resourcePtSeed = new ResourcePtSeed();
                resourcePtSeed.SetResourcePTDictionary();

                using (var db = new Dominio.SgqDbDevEntities())
                {
                    var resources = new ExpandoObject() as IDictionary<string, object>;
                    foreach (var item in db.ResourcePT.ToList())
                    {
                        resources.Add(item.Key, item.Value);
                    }
                    Resources.Resource = resources;
                }
            }
            else
            {
                var resourceENSeed = new ResourceEnSeed();
                resourceENSeed.SetResourceENDictionary();

                using (var db = new Dominio.SgqDbDevEntities())
                {
                    var resources = new ExpandoObject() as IDictionary<string, object>;
                    foreach (var item in db.ResourceEN.ToList())
                    {
                        resources.Add(item.Key, item.Value);
                    }
                    Resources.Resource = resources;
                }
            }

            if (runAppScripts)
            {
                //metodo para preencher os scrips com base nos arquivos da pasta, para mante-los atualizados
                AppScriptSeed appScriptSeed = new AppScriptSeed();
                appScriptSeed.SetAppScript();
            }
        }
    }
}