using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgqSystem.Seed
{
    public class Seed
    {
        public void SetSeedValues()
        {
            DicionarioEstaticoSeed dicionarioSeed = new DicionarioEstaticoSeed();
            dicionarioSeed.SetDicionarioEstatico();

            if (GlobalConfig.LanguageBrasil)
            {
                var resourcePtSeed = new ResourcePtSeed();
                resourcePtSeed.SetResourcePTDictionary();
            }
            else
            {
                var resourceENSeed = new ResourceEnSeed();
                resourceENSeed.SetResourceENDictionary();
            }
        }    
    }
}