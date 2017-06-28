using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO.Params;
using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.NewSync
{
    [HandleApi()]
    [RoutePrefix("api/NewSync")]
    public class NewSyncController : ApiController
    {
        private IParamsDomain _paramdDomain;
        private IBaseDomain<ParLevel1, ParLevel1DTO> _basedomain;

        public NewSyncController(IParamsDomain paramsDomain, IBaseDomain<ParLevel1, ParLevel1DTO> basedomain)
        {
            _paramdDomain = paramsDomain;
            _basedomain = basedomain;
        }

        //[HttpGet]
        //[Route("getParams")]
        //public ParamsVinculos getParams()
        //{
        //    var retorno = new ParamsVinculos();
        //    #region level1
        //    string query = "select pl1.id as level1,pl2.id as level2, pl3.id as level3 from ParLevel3Level2Level1 Pl3L2L1 LEFT JOIN ParLevel1 Pl1 on Pl3L2L1.ParLevel1_Id = Pl1.Id "+
        //                   "LEFT JOIN ParLevel3Level2 Pl3L2 on Pl3L2L1.ParLevel3Level2_Id = Pl3L2.Id LEFT JOIN ParLevel2 Pl2 on Pl3L2.ParLevel2_Id = Pl2.Id "+
        //                   "LEFT JOIN ParLevel3 Pl3 on Pl3L2.ParLevel3_Id = Pl3.Id where pl1.IsActive = 1 and pl2.IsActive = 1 and pl3.IsActive = 1";
        //    using (var db = new SgqDbDevEntities())
        //    {
        //        listaL1 = db.Database.SqlQuery<ParLevel1>(query).ToList();

        //        query = "select distinct(pl2.id) as codigol2, pl2.* from ParLevel3Level2Level1 Pl3L2L1 LEFT JOIN ParLevel1 Pl1 on Pl3L2L1.ParLevel1_Id = Pl1.Id " +
        //                "LEFT JOIN ParLevel3Level2 Pl3L2 on Pl3L2L1.ParLevel3Level2_Id = Pl3L2.Id LEFT JOIN ParLevel2 Pl2 on Pl3L2.ParLevel2_Id = Pl2.Id " +
        //                "LEFT JOIN ParLevel3 Pl3 on Pl3L2.ParLevel3_Id = Pl3.Id where pl1.IsActive = 1 and pl2.IsActive = 1";
        //        listaL2 = db.Database.SqlQuery<ParLevel2>(query).ToList();

        //        query = "select distinct(pl3.id) as codigol3, pl3.* from ParLevel3Level2Level1 Pl3L2L1 LEFT JOIN ParLevel1 Pl1 on Pl3L2L1.ParLevel1_Id = Pl1.Id " +
        //                    "LEFT JOIN ParLevel3Level2 Pl3L2 on Pl3L2L1.ParLevel3Level2_Id = Pl3L2.Id LEFT JOIN ParLevel2 Pl2 on Pl3L2.ParLevel2_Id = Pl2.Id " +
        //                    "LEFT JOIN ParLevel3 Pl3 on Pl3L2.ParLevel3_Id = Pl3.Id where pl1.IsActive = 1 and pl2.IsActive = 1 and pl3.IsActive = 1 ";
        //    }
        //    #endregion
        //    var count = 0;
        //    #region level2

        //    retorno.listaDeLevel1 = new List<Level1Params>();
        //    foreach (var item in listaL1)
        //    {
        //        var it = new Level1Params();
        //        it.level1 = item;

        //        query = "select distinct(pl2.id) as codigol2, pl2.* from ParLevel3Level2Level1 Pl3L2L1 LEFT JOIN ParLevel1 Pl1 on Pl3L2L1.ParLevel1_Id = Pl1.Id " +
        //                "LEFT JOIN ParLevel3Level2 Pl3L2 on Pl3L2L1.ParLevel3Level2_Id = Pl3L2.Id LEFT JOIN ParLevel2 Pl2 on Pl3L2.ParLevel2_Id = Pl2.Id " +
        //                "LEFT JOIN ParLevel3 Pl3 on Pl3L2.ParLevel3_Id = Pl3.Id where pl1.IsActive = 1 and pl2.IsActive = 1 and pl1.Id = "+item.Id;

        //        using (var db = new SgqDbDevEntities())
        //        {
        //            listaL2 = db.Database.SqlQuery<ParLevel2>(query).ToList();

        //        }
        //        it.listaDeLevel2 = new List<Level2Params>();
        //        foreach (var i in listaL2)
        //        {
        //            var itL2 = new Level2Params();
        //            itL2.level2 = i;
        //            it.listaDeLevel2.Add(itL2);
        //            query = "select distinct(pl3.id) as codigol3, pl3.* from ParLevel3Level2Level1 Pl3L2L1 LEFT JOIN ParLevel1 Pl1 on Pl3L2L1.ParLevel1_Id = Pl1.Id " +
        //                    "LEFT JOIN ParLevel3Level2 Pl3L2 on Pl3L2L1.ParLevel3Level2_Id = Pl3L2.Id LEFT JOIN ParLevel2 Pl2 on Pl3L2.ParLevel2_Id = Pl2.Id " +
        //                    "LEFT JOIN ParLevel3 Pl3 on Pl3L2.ParLevel3_Id = Pl3.Id where pl1.IsActive = 1 and pl2.IsActive = 1 and pl3.IsActive = 1 and pl1.id = " + item.Id
        //                    + " and pl2.Id = " + i.Id;
        //            using (var db = new SgqDbDevEntities())
        //            {

        //            }
        //                foreach (var tarefa in )
        //        }
        //        retorno.listaDeLevel1.Add(it);

        //        count++;
        //    }
        //    #endregion


        //    return retorno;
        //}

        [HttpGet]
        [Route("getParams")]
        public IEnumerable<ParLevel1DTO> getParams()
        {
            return _paramdDomain.GetAllLevel1().Where(r => r.IsActive == true && r.listParLevel3Level2Level1Dto != null);

        }
    }

    public class ParamsVinculos
    {
        public int level1 { get; set; }
        public int level2 { get; set; }
        public int level3 { get; set; }
    }
    public class Level1Params
    {
        public ParLevel1 level1 { get; set; }
        public List<Level2Params> listaDeLevel2 { get; set; }
    }
    public class Level2Params
    {
        public ParLevel2 level2 { get; set; }
        public List<ParLevel3> listaDeLevel3 { get; set; }
    }
}
