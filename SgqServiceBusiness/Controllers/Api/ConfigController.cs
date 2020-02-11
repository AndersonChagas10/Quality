using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using Dominio;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace SgqServiceBusiness.Api
{
    public class ConfigController
    {
        public String UpdateAppScripts()
        {
            Dominio.Seed.Seed.SetSeedValues(isEN: DTO.GlobalConfig.LanguageEUA, runSetSeed: true);
            return "Atualizado scripts do banco";
        }

        public String UpdateDicionarioEstatico()
        {
            Dominio.Seed.Seed.SetSeedValues(isEN: DTO.GlobalConfig.LanguageEUA, runSetSeed: true);
            Dominio.Seed.Seed.SetDicionario();
            return "Atualizado dicionario estático";
        }

        // GET: AppScripts
        public dynamic GetAppVersionIsUpdated(string versionNumber)
        {
            string appVersionNumber = DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.appVersion;

            if (!string.IsNullOrWhiteSpace(versionNumber) && !versionNumber.Contains(appVersionNumber))
                return new { updated = false, versionNumber = appVersionNumber };
            return new { updated = true, versionNumber = appVersionNumber };
        }
    }
}
