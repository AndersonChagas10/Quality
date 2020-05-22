using AutoMapper;
using Dominio;
using DTO.DTO.Params;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [RoutePrefix("api/CompanyForm")]
    public class CompanyFormApiController : ApiController
    {

        private SgqDbDevEntities db = new SgqDbDevEntities();


        [HttpPost]
        [Route("GetRegionaisUsuario")]
        public List<ParStructureDTO> GetRegionaisUsuario()
        {
            int parStructureGroup1 = Convert.ToInt32(DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.ParStructureGroup1);
            int parStructureGroup2 = Convert.ToInt32(DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.ParStructureGroup2);
           var retorno = Mapper.Map<List<ParStructureDTO>>(
                db.ParStructure
                .Where(r => r.ParStructureGroup_Id == parStructureGroup2
                && r.ParStructureParent_Id == parStructureGroup1).ToList());

            return retorno;
        }

        [HttpGet]
        [Route("GetListClusterVinculadoClusterGroup")]
        public List<ParClusterDTO> GetListClusterVinculadoClusterGroup(int ClusterGroupId)
        {
            var retorno = Mapper.Map<List<ParClusterDTO>>(db.ParCluster.Where(r => r.ParClusterGroup_Id == ClusterGroupId).ToList());

            return retorno;
        }
    }
}
