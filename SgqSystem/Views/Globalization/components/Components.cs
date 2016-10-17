using System;
using System.Web.Mvc;

namespace SgqSystem.Helpers
{
    public static class Components
    {
        public static MvcHtmlString LinkVoltar(this HtmlHelper html, string idLink, string textoLink = "Voltar")
        {
            string strLink = string.Format("<a id=\"{0}\" href=\"javascript:history.go(-1);\">{1}</a>", idLink, textoLink);
            return new MvcHtmlString(strLink);
        }

        public static MvcHtmlString ParLevel1HTMLComponent()
        {
            String finalComponent = "";
            return new MvcHtmlString(finalComponent);
        }
    }
}