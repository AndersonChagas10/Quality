using ADOFactory;
using Dominio;
using DTO;
using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{

    public partial class _PCC1B
    {
        public int Sequential { get; set; }
        public int Side { get; set; }
        public string serverSide { get; set; }
    };

    public partial class _Receive
    {
        public int sequencialAtual { get; set; }
        public String Data { get; set; }
        public int Unit { get; set; }
        public int ParLevel2 { get; set; }
        public int HashKey { get; set; }
    }

    [HandleApi()]
    [RoutePrefix("api/PCC1B")]
    public class PCC1BController : ApiController
    {
        private _PCC1B pcc1b { get; set; }


        [HttpPost]
        [Route("testeContext")]
        public bool testeContext()
        {
            return RequestContext.IsLocal;
        }

        [HttpPost]
        [Route("Next")]
        public _PCC1B Next(_Receive receive)
        {

            string connectionString = "SgqDbDevEntities";

            ParCompany company;
            var retorno = new _PCC1B();
            retorno.Side = 1;

            ResultadosSequencialBanda _result = new ResultadosSequencialBanda();

            var userName = "UserGQualidade";
            var pass = "grJsoluco3s";

            //if (GlobalConfig.Brasil)
            //{
            //    userName = "sa";
            //    pass = "1qazmko0";
            //}

            if (receive.sequencialAtual == 0)
            {
                retorno.Sequential = 1;
                return retorno;
            }

            using (var db = new SgqDbDevEntities())
            {
                company = db.ParCompany.FirstOrDefault(r => r.Id == receive.Unit);
            }

            try
            {
                if (GlobalConfig.MockOn)
                    throw new Exception();
            
                using (var db = new Factory(company.IPServer, company.DBServer, pass, userName))
                {
                    var query = "EXEC FBED_GRTTipificacao '" + receive.Data + "', " + company.CompanyNumber.ToString() + ", " + receive.sequencialAtual.ToString();

                    var resultQuery = new List<ResultadosSequencialBanda>();
                    using (Factory factory = new Factory("DefaultConnection"))
                    {
                        resultQuery = factory.SearchQuery<ResultadosSequencialBanda>(query).ToList();
                    }

                    if (resultQuery != null && resultQuery.Count() > 0)
                    {
                        retorno.Sequential = resultQuery.FirstOrDefault().iSequencial;
                    }else
                    {
                        retorno.Sequential = receive.sequencialAtual + 1;
                    }

                    retorno.serverSide = company.IPServer + company.DBServer + pass + userName;
                    return retorno;
                }

            }
            catch (Exception)
            {
                retorno.Sequential = receive.sequencialAtual + 1;
                retorno.serverSide = company.IPServer + company.DBServer + pass + userName;
                return retorno;
                
            }

        }

        [HttpPost]
        [Route("TotalNC/{parLevel2IdDianteiro}/{parLevel2Id2Traseiro}")]
        public IEnumerable<CollectionLevel2PCC1B> TotalNC(_Receive receive, int parLevel2IdDianteiro, int parLevel2Id2Traseiro)
        {
            ParLevel1 parLevel1 = new ParLevel1();
            using (var db = new SgqDbDevEntities())
            {
                parLevel1 = db.ParLevel1.FirstOrDefault(r => r.Id == 3);
            }
                                                                                                                    
            var query =
                "SELECT FORMAT(CollectionDate, 'MMddyyyy') as CollectionDate, ParLevel1_Id, ParLevel2_Id, UnitId,  " +
                "Sequential, Side, DefectsResult, [Key]  FROM CollectionLevel2                                     " +
                "WHERE parlevel1_Id = "+ parLevel1.Id + "                                                          " +
                "and ParLevel2_Id in ("+ parLevel2IdDianteiro + ", "+ parLevel2Id2Traseiro + ")                    " +   
                "and UnitId = "+receive.Unit+"                                                                     " +
                "and CollectionDate Between('"+ receive.Data+ " 00:00:00.0000000') and('" + receive.Data +         " 23:59:59.0000000')   ";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                return factory.SearchQuery<CollectionLevel2PCC1B>(query).ToList();
            }

        }

    }
    public class CollectionLevel2PCC1B
    {
        public string CollectionDate { get; set; }
        public int ParLevel1_Id { get; set; }
        public int ParLevel2_Id { get; set; }
        public int UnitId { get; set; }
        public int Sequential { get; set; }
        public int Side { get; set; }
        public int DefectsResult { get; set; }
        public string Key { get; set; }
    }

    public class ResultadosSequencialBanda
    {
        public int iSequencial { get; set; }
    }

    public class ResultTotalNC
    {
        public decimal ncDianteiro { get; set; }
        public decimal ncTraseiro { get; set; }


        public int _ncDianteiro
        {
            get
            {
                return Convert.ToInt32(ncDianteiro);
            }
        }
        public int _ncTraseiro
        {
            get
            {
                return Convert.ToInt32(ncTraseiro);
            }
        }

        public decimal totalNc
        {
            get
            {
                return ncDianteiro + ncTraseiro;
            }
        }

        public int _totalNc
        {
            get
            {
                return _ncDianteiro + _ncTraseiro;
            }
        }
    }

}
