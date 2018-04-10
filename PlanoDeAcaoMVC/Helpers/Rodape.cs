using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Reflection;

namespace PlanoDeAcaoMVC.Helpers
{
    public static class Rodape
    {
        public static IHtmlString AssemblyVersion(this HtmlHelper helper)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var versionTime = File.GetCreationTime(Assembly.GetExecutingAssembly().Location);
            return MvcHtmlString.Create(version + "<ll id='versionTime' style='display: none;'>" + versionTime + "</ll>");
        }
    }
}