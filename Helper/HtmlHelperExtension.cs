using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Helper
{
    public static class HtmlHelperExtension
    {
        public static MvcHtmlString LinkVoltar(this HtmlHelper html, string idLink, string textoLink = "Voltar")
        {
            string strLink = String.Format("<a id=\"{0}\" href=\"javascript:history.go(-1);\">{1}</a>", idLink, textoLink);
            return new MvcHtmlString(strLink);
        }


        /// <summary>
        /// Retorna input formatado para decimal no caso de resources multiregião
        /// </summary>
        /// <param name="html"></param>
        /// <param name="name"></param>
        /// <param name="valor"></param>
        /// <param name="idInput"></param>
        /// <param name="classe"></param>
        /// <returns></returns>
        public static MvcHtmlString DecimalFormatado(this HtmlHelper html,
                                                string name,
                                                decimal valor,
                                                string idInput,
                                                string classe
                                                )
        {
            string strLink = html.TextBox(name, valor.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture), htmlAttributes: new { id = idInput, @class = classe, @onkeypress = "return (event.charCode >= 48 && event.charCode <= 57) ||  event.charCode == 46 || event.charCode == 0 " }).ToString();
            return new MvcHtmlString(strLink);
        }

    }
}