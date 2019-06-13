using DTO;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace SgqSystem.Seed
{
    public static class Seed
    {
        public static void SetSeedValues()
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
                DicionarioEstaticoHelper.DicionarioEstaticoHelpers = x;
            }

           

            if (GlobalConfig.LanguageBrasil)
            {
                var resourcePtSeed = new ResourcePtSeed();
                resourcePtSeed.SetResourcePTDictionary();

                using (var db = new Dominio.SgqDbDevEntities())
                {
                    var x = new ExpandoObject() as IDictionary<string, object>;
                    foreach (var item in db.ResourcePT.ToList())
                    {
                        x.Add(item.Key, item.Value);
                        // Resources.Resource.Add( item.Key, item.Value);
                        //Resources.Resource.GetType().InvokeMember(item.Key,
                        //    BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                        //    Type.DefaultBinder, Resources.Resource, item.Value);
                    }
                    Resources.Resource = x;
                }
            }
            else
            {
                var resourceENSeed = new ResourceEnSeed();
                resourceENSeed.SetResourceENDictionary();

                using (var db = new Dominio.SgqDbDevEntities())
                {
                    var x = new ExpandoObject() as IDictionary<string, object>;
                    foreach (var item in db.ResourceEN.ToList())
                    {
                        x.Add(item.Key, item.Value);
                        // Resources.Resource.Add( item.Key, item.Value);
                        //Resources.Resource.GetType().InvokeMember(item.Key,
                        //    BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                        //    Type.DefaultBinder, Resources.Resource, item.Value);
                    }
                    Resources.Resource = x;
                }
            }
        }    
    }
}