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
        
        public static System.Web.Mvc.MvcHtmlString InformationPopover(this System.Web.Mvc.HtmlHelper html, string text)
        {
            string response = $@"<i class='fa fa-info-circle fa-lg text-primary popovers' data-container='body' data-trigger='hover' data-placement='bottom' data-content='{text}' aria-hidden='true' data-original-title='' title='' data-toggle='popover'></i>";
            return new System.Web.Mvc.MvcHtmlString(response);
        }
    }
}