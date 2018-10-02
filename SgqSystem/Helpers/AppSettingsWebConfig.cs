using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SgqSystem.Helpers
{
    public static class AppSettingsWebConfig
    {
        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}