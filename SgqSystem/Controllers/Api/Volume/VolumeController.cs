using System.Web.Http;
using System.Web.Http.Cors;
using Dominio;
using System.Linq;

namespace SgqSystem.Controllers.Api.Volume
{
    [RoutePrefix("api/Volume")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VolumeController : ApiController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        [HttpPost]
        [Route("GetVolumeDesossa/{idUnidade}")]
        public int GetVolumeDesossa(int idUnidade)
        {
            var query = "" +
                "\n select isNull(sum(fam.familias),0) as familias from                                                               " +
                "\n (                                                                                                       " +
                "\n select                                                                                                  " +
                "\n top 1 count(1) familias, Max(InitDate) data                                                             " +
                "\n from ParLevel2ControlCompany      p                                                                      " +
                "\n where parcompany_id = " + idUnidade + "        and p.isactive = 1                                                        " +
                "\n and ParLevel1_Id = (Select id from parlevel1 where hashkey = 2) and InitDate < getdate()                " +
                "\n group by InitDate                                                                                       " +
                "\n order by 2 desc                                                                                         " +
                "\n                                                                                                         " +
                "\n union all                                                                                               " +
                "\n                                                                                                         " +
                "\n select                                                                                                  " +
                "\n top 1 count(1) familias, Max(InitDate) data                                                             " +
                "\n from ParLevel2ControlCompany     p                                                                       " +
                "\n where parcompany_id is null and ParLevel1_Id = (Select id from parlevel1 where hashkey = 2)             " +
                "\n and InitDate < getdate()      and p.isactive   = 1                                                                         " +
                "\n group by InitDate                                                                                       " +
                "\n order by 2 desc                                                                                         " +
                "\n                                                                                                         " +
                "\n ) fam                                                                                                   ";
            var result = db.Database.SqlQuery<int>(query).FirstOrDefault();
            return result;
        }

        [HttpPost]
        [Route("GetVolumeGRD/{idUnidade}")]
        public int GetVolumeGRD(int idUnidade)
        {
            var query = "" +
                "\n select isNull(sum(fam.familias), 0) as familias from                                                               " +
                "\n (                                                                                                       " +
                "\n select                                                                                                  " +
                "\n top 1 count(1) familias, Max(InitDate) data                                                             " +
                "\n from ParLevel2ControlCompany    p                                                                        " +
                "\n where parcompany_id = " + idUnidade + "       and p.isactive       = 1                                                     " +
                "\n and ParLevel1_Id = (Select id from parlevel1 where hashkey = 3) and InitDate < getdate()                " +
                "\n group by InitDate                                                                                       " +
                "\n order by 2 desc                                                                                         " +
                "\n                                                                                                         " +
                "\n union all                                                                                               " +
                "\n                                                                                                         " +
                "\n select                                                                                                  " +
                "\n top 1 count(1) familias, Max(InitDate) data                                                             " +
                "\n from ParLevel2ControlCompany    p                                                                        " +
                "\n where parcompany_id is null and ParLevel1_Id = (Select id from parlevel1 where hashkey = 3)             " +
                "\n and InitDate < getdate()     and p.isactive                = 1                                                             " +
                "\n group by InitDate                                                                                       " +
                "\n order by 2 desc                                                                                         " +
                "\n                                                                                                         " +
                "\n ) fam                                                                                                   ";
            var result = db.Database.SqlQuery<int>(query).FirstOrDefault();
            return result;
        }

        
    }
}
