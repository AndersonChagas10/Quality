using Dominio.Seed;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Dominio.Seed
{
    public static class Seed
    {
        public static void SetSeedValues(bool isEN = false, bool runSetSeed = false)
        {

            if (!isEN)
            {
                var resourcePtSeed = new ResourcePtSeed();
                if (runSetSeed)
                    resourcePtSeed.SetResourcePTDictionary();
                //new Task(()=>resourcePtSeed.SetResourcePTDictionary());

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
                if (runSetSeed)
                    resourceENSeed.SetResourceENDictionary();
                //new Task(() => resourceENSeed.SetResourceENDictionary());

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

            if (runSetSeed)
            {
                //metodo para preencher os scrips com base nos arquivos da pasta, para mante-los atualizados
                AppScriptSeed appScriptSeed = new AppScriptSeed();
                appScriptSeed.SetAppScript();
                //new Task(() => appScriptSeed.SetAppScript());
            }
        }

        public static void SetDicionario()
        {
            DicionarioEstaticoSeed dicionarioSeed = new DicionarioEstaticoSeed();

                #if !DEBUG
                 dicionarioSeed.SetDicionarioEstatico();
                #endif

            using (var db = new Dominio.SgqDbDevEntities())
            {
                var x = new ExpandoObject() as IDictionary<string, object>;

                foreach (var item in db.DicionarioEstatico.ToList())
                {
                    x.Add(item.Key, item.Value);
                }

                DicionarioEstaticoGlobal.DicionarioEstaticoHelpers = x;
            }
        }
    }
}