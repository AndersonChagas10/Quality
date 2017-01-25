using Dominio;
using SgqSystem.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/ApontamentosDiarios")]
    public class ApontamentosDiariosApiController : ApiController
    {

        private List<ApontamentosDiariosResultSet> _mock { get; set; }
        private List<ApontamentosDiariosResultSet> _list { get; set; }

        [HttpPost]
        [Route("GetApontamentosDiarios")]
        public List<ApontamentosDiariosResultSet> GetApontamentosDiarios([FromBody] FormularioParaRelatorioViewModel form)
        {
            var query = new ApontamentosDiariosResultSet().Select(form);
            using (var db = new SgqDbDevEntities())
            {
                _list = db.Database.SqlQuery<ApontamentosDiariosResultSet>(query).ToList();
            }

            return _list;
        }
     
    }
}
