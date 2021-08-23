using System.Collections.Generic;
using System.Text;

namespace Conformity.Domain.Core.Entities.PlanoDeAcao.Email
{
    public class HtmlDaEvidencia
    {
        public string MontarHtmlDaEvidencia(IEnumerable<string> lista)
        {
            StringBuilder stringBuilder = new StringBuilder("");

            foreach (var item in lista)
            {
                stringBuilder.Append("<img src='data:image/png;base64,");
                stringBuilder.Append(item);
                stringBuilder.Append("' data-img style='width:30%; height:30%;'/>");
            }
            return stringBuilder.ToString();
        }
    }
}
