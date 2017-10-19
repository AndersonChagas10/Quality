using AutoMapper;
using Dominio;
using DTO.DTO.Params;
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
            //var lista = new List<ParStructure>();

            //if (Unidades.Count > 0)
            //{
            //    var Unidade = "";

            //    foreach (var item in Unidades)
            //    {
            //        Unidade += item.Id.ToString() + ",";
            //    }

            //    Unidade.Remove(Unidade.Length - 1);

            //    var sql = @"SELECT
            //            	DISTINCT(PS.Id) as Id, PS.Name as Name
            //            FROM ParStructure PS
            //            INNER JOIN ParCompanyXStructure CXS
            //            	ON PS.Id = CXS.ParCompany_Id
            //            	and PS.ParStructureGroup_Id = 2
            //            where CXS.ParCompany_Id in " + Unidades + @"";

            //    lista = db.Database.SqlQuery<ParStructureDTO>(sql).ToList();
            //}


            var retorno = Mapper.Map<List<ParStructureDTO>>(db.ParStructure.Where(r => r.ParStructureGroup_Id == 2 && r.ParStructureParent_Id == 1).ToList());

            return retorno;
        }
    }
}
