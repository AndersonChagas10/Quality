using System;
using System.Web.Mvc;

namespace Helper
{
    public static class HtmlHelperExtension
    {
        public static MvcHtmlString LinkVoltar(this HtmlHelper html, string idLink, string textoLink = "Voltar")
        {
            string strLink = String.Format("<a id=\"{0}\" href=\"javascript:history.go(-1);\">{1}</a>", idLink, textoLink);
            return new MvcHtmlString(strLink);
        }
    }
}