using SgqSystem.ViewModels;
using System.Collections.Generic;
using System.Dynamic;
using System.Web.Http;


namespace SgqSystem.Controllers.Api.RelatoriosBrasil
{
    [RoutePrefix("api/RelatorioGenerico")]
    public class RelatorioGenericoApiController : ApiController
    {
        [HttpPost]
        [Route("Get")]
        public List<ExpandoObject> GetRelatorio([FromBody] FormularioParaRelatorioViewModel form)
        {

            var lista = new List<PropriedadesGenericas>();

            dynamic lista2 = new List<ExpandoObject>();

            dynamic retorno = new List<ExpandoObject>();

            for (int i = 0; i < 2; i++)
            {
                lista2.add(new PropriedadesGenericasHeader() { title = "Col" + i , mData = "Col" + i});
            }

            for (int i = 0; i < 10; i++)
            {

                lista.Add(new PropriedadesGenericas() { col1 = "teste coluna 1 " + i, col2 = "teste coluna 2 " + i });
            }

            retorno.add(lista);
            retorno.add(lista2);

            return retorno;
        }

    }

    public class PropriedadesGenericas
    {
        public string col1 { get; set; }
        public string col2 { get; set; }
    }

    public class PropriedadesGenericasHeader
    {
        public string title { get; set; }
        public string mData { get; set; }
    }
}
