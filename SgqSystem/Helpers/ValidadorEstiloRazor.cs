using System.Web.Mvc;

namespace SgqSystem.Helpers
{
    public static class ValidadorEstiloRazor
    {
        public static MvcHtmlString ErrorManual(this HtmlHelper html, string idLink, string textoError = "")
        {
            string strLink = string.Format("<span id = " + idLink + " style='Display:none' class='text - danger field - validation - error' data-valmsg-for='' data-valmsg-replace='true'><span for='' class=''>" + textoError + "</span></span>", idLink, textoError);
            return new MvcHtmlString(strLink);
        }
    }
}