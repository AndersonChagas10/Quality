using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class ConfigController : Controller
    {
        public String UpdateAppScripts()
        {
            Dominio.Seed.Seed.SetSeedValues(isEN: GlobalConfig.LanguageEUA, runSetSeed: true);
            return "Atualizado scripts do banco";
        }
        public String UpdateDicionarioEstatico()
        {
            Dominio.Seed.Seed.SetSeedValues(isEN: GlobalConfig.LanguageEUA, runSetSeed: true);
            return "Atualizado dicionario estático";
        }
    }
}