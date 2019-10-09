using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgqSystem.ViewModels
{
    public class RotinaIntegracaoViewModel
    {
        public int Id { get; set; }
        public string Parametro { get; set; }
        public string Name { get; set; }
        public string Retornos { get; set; }
        public bool IsOffline { get; set; }
        public List<JObject> Resultado { get; set; }
    }
}