using Dominio;
using SgqSystem.Handlres;
using System.Collections.Generic;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.App
{
    [RoutePrefix("api/AppParams")]
    public class AppParamsApiController : ApiController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        public AppParamsApiController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }

        //$.post('http://192.168.25.200/SgqBr/api/AppParams/GetContadoresX',{ }, function(r)
        //{
        //    console.log(r);
        //});
        //[HttpPost]
        //[HandleApi()]
        //[Route("GetContadoresX")]
        //public List<DefeitosPorAmostra> GetContadoresX([FromBody]int level1ID)
        //{
        //    return new ContadoresXX().GetContadoresXX(db, level1ID);

        //}

    }

   

}