
using AutoMapper;
using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO.Params;
using Newtonsoft.Json.Linq;
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
    public class NewSyncController : BaseApiController
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
        public ParamsVinculos getParams()
        {
            using (var db = new SgqDbDevEntities())
            {
                string query = "SELECT distinct(PL1.Id) as codigo, PL1.* "+
                               "FROM ParLevel3Level2Level1 Pl31 "+
                               "LEFT JOIN ParLevel3Level2 PL32 on Pl31.ParLevel3Level2_Id = Pl32.Id "+
                               "LEFT JOIN ParLevel1 PL1 on Pl31.ParLevel1_Id = PL1.Id "+
                               "LEFT JOIN ParLevel2 PL2 on PL32.ParLevel2_Id = PL2.Id "+
                               "LEFT JOIN ParLevel3 PL3 on PL32.ParLevel3_Id = PL3.Id " +
                               "where pl1.isActive = 1";
                ParamsVinculos obj = new ParamsVinculos();
                obj.listaDelevel1 = new List<ParamsVinculosL1>();
                var listaL1 = db.Database.SqlQuery<ParLevel1>(query).ToList();
                foreach (var item in listaL1)
                {
                    ParamsVinculosL1 paramsVL1 = new ParamsVinculosL1();
                    paramsVL1.level1 = item;
                    string queryL2 = "SELECT distinct(PL2.Id) as codigo, PL2.* " +
                               "FROM ParLevel3Level2Level1 Pl31 " +
                               "LEFT JOIN ParLevel3Level2 PL32 on Pl31.ParLevel3Level2_Id = Pl32.Id " +
                               "LEFT JOIN ParLevel1 PL1 on Pl31.ParLevel1_Id = PL1.Id " +
                               "LEFT JOIN ParLevel2 PL2 on PL32.ParLevel2_Id = PL2.Id " +
                               "LEFT JOIN ParLevel3 PL3 on PL32.ParLevel3_Id = PL3.Id " +
                               "where pl1.isActive = 1 and pl2.IsActive = 1 and pl1.id ="+item.Id;
                    var listaL2 = db.Database.SqlQuery<ParLevel2>(queryL2).ToList();
                    paramsVL1.listaDelevel2 = new List<ParamsVinculosL2>();
                    foreach (var l2 in listaL2)
                    {
                        ParamsVinculosL2 paramsVL2 = new ParamsVinculosL2();
                        paramsVL2.level2 = l2;
                        string queryL3 = "SELECT distinct(PL3.Id) as codigo, PL3.* " +
                               "FROM ParLevel3Level2Level1 Pl31 " +
                               "LEFT JOIN ParLevel3Level2 PL32 on Pl31.ParLevel3Level2_Id = Pl32.Id " +
                               "LEFT JOIN ParLevel1 PL1 on Pl31.ParLevel1_Id = PL1.Id " +
                               "LEFT JOIN ParLevel2 PL2 on PL32.ParLevel2_Id = PL2.Id " +
                               "LEFT JOIN ParLevel3 PL3 on PL32.ParLevel3_Id = PL3.Id " +
                               "where pl1.isActive = 1 and pl2.IsActive = 1 and pl3.IsActive =1 and pl1.id = "+item.Id+" and pl2.id = " + l2.Id;
                        paramsVL2.listaDelevel3 = db.Database.SqlQuery<ParLevel3>(queryL3).ToList();
                        paramsVL1.listaDelevel2.Add(paramsVL2);
                    }
                    obj.listaDelevel1.Add(paramsVL1);
                }
                return obj;
            }
        }
    }

    public class ParamsVinculos
    {
        public List<ParamsVinculosL1> listaDelevel1 { get; set; }
    }
    public class ParamsVinculosL1
    {
        public ParLevel1 level1 { get; set; }
        public List<ParamsVinculosL2> listaDelevel2 { get; set; }
    }
    public class ParamsVinculosL2
    {
        public ParLevel2 level2 { get; set; }
        public List<ParLevel3> listaDelevel3 { get; set; }
    }
}