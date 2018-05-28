using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace SgqSystem.Helpers
{
    public static class Util
    {

        public static List<string> PreencherMensagem(ModelStateDictionary modelState)
        {
            var lista = new List<string>();
            foreach (var erro in modelState.Values)
            {
                lista.Add(erro.Errors.FirstOrDefault()?.ErrorMessage);

            }
            return lista;

        }
    }
}