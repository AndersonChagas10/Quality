using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Helpers
{
    public static class Teste
    {
        public static MvcHtmlString LinkVoltar(this HtmlHelper html, string idLink, string textoLink = "Voltar")
        {
            string strLink = string.Format("<a id=\"{0}\" href=\"javascript:history.go(-1);\">{1}</a>", idLink, textoLink);
            return new MvcHtmlString(strLink);
        }
        
        public static IHtmlString AssemblyAppVersion(this HtmlHelper helper)
        {
            var versaoApp = ConfigurationManager.AppSettings["appVersion"];
            var versionTime = File.GetCreationTime(Assembly.GetExecutingAssembly().Location);
            return MvcHtmlString.Create(versaoApp + "<ll id='versionTime' style='display: none;'>" + versionTime + "</ll>");
        }

        public static IHtmlString AssemblyVersion(this HtmlHelper helper)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var versionTime = File.GetCreationTime(Assembly.GetExecutingAssembly().Location);
            return MvcHtmlString.Create(version + "<ll id='versionTime' style='display: none;'>" + versionTime +"</ll>");
        }
    }
}