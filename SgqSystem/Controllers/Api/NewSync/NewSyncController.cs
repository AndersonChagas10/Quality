
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

        [HttpGet]
        [Route("GetConsolidationLevel1")]
        public ConsolidacaoReturn getConsolidationLevel1(int unidade, string data)
        {
            using (var db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                ConsolidacaoReturn consolidacao = new ConsolidacaoReturn();
                consolidacao.consolidationLevel1 = db.ConsolidationLevel1.Where(r => r.UnitId == unidade).ToList();
                //consolidacao.consolidationLevel2 = db.ConsolidationLevel2.Where(r => r.UnitId == unidade).ToList();
                string queryConsolidationL2 = " select cdl2.* from consolidationlevel2 cdl2 where cast(cdl2.ConsolidationDate as date) = '"+data+"' and cdl2.UnitId = "+unidade;
                consolidacao.consolidationLevel2 = db.Database.SqlQuery<ConsolidationLevel2>(queryConsolidationL2).ToList();
                string queryCollection = "select * from collectionlevel2 cl2 inner join consolidationlevel2 cdl2 on cdl2.id = cl2.ConsolidationLevel2_Id "+
                                         "\n where cast(cl2.CollectionDate as date) = '"+data+"' and cdl2.UnitId = "+unidade;
                consolidacao.collectionLevel2 = db.Database.SqlQuery<CollectionLevel2>(queryCollection).ToList();
                string query = "select rl3.* from result_level3 rl3 inner join collectionlevel2 cl2 on cl2.id = rl3.collectionlevel2_id "+
                                            "\n inner join consolidationlevel2 cdl2 on cdl2.id = cl2.ConsolidationLevel2_Id "+
                                            "\n where cast(cl2.CollectionDate as date) = '"+ data + "' and cdl2.UnitId =" + unidade;
                consolidacao.result_Level3 = db.Database.SqlQuery<Result_Level3>(query).ToList();
                return consolidacao;  
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
    public class ConsolidacaoReturn
    {
        public List<ConsolidationLevel1> consolidationLevel1 { get; set; }
        public List<ConsolidationLevel2> consolidationLevel2 { get; set; }
        public List<CollectionLevel2> collectionLevel2 { get; set; }
        public List<Result_Level3> result_Level3 { get; set; }
    }
}