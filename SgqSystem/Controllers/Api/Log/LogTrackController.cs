using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api.Log
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/LogTrack")]
    public class LogTrackController : ApiController
    {
        // GET: LogTrack
        [Route("Get/{table_name}/{id}")]
        public IEnumerable<object> Get([FromUri] string table_name, int id)
        {
            return LogSystem.LogTrackBusiness.GetLogTrack(table_name, id);
        }

        // GET: LogTrack
        [Route("GetHeaderFieldByResult_Level3/{id}")]
        public IEnumerable<object> Get([FromUri] int id)
        {
            var table_name = "CollectionLevel2XParHeaderField";
            var ids = new List<int>();
            using (var db = new Dominio.SgqDbDevEntities())
            {
                var collectionLevel2_Id = db.Result_Level3
                    .Where(x => x.Id == id)
                    .ToList()
                    .Select(x => x.CollectionLevel2_Id)
                    .FirstOrDefault();

                ids = db.CollectionLevel2XParHeaderField
                    .Where(x => x.CollectionLevel2_Id == collectionLevel2_Id)
                    .Select(x => x.Id)
                    .ToList();
            }

            return LogSystem.LogTrackBusiness.GetLogTrack(table_name, ids);

        }
    }
}