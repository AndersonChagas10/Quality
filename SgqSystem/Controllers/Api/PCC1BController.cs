using Dominio;
using Dominio.ADO;
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

            
                using (var db = new FactoryADO(company.IPServer, company.DBServer, pass, userName))
                {
                    var query = "EXEC FBED_GRTTipificacao '" + receive.Data + "', " + company.CompanyNumber.ToString() + ", " + receive.sequencialAtual.ToString();
                    var resultQuery = db.SearchQuery<ResultadosSequencialBanda>(query).ToList();
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
        public ResultTotalNC TotalNC(_Receive receive, int parLevel2IdDianteiro, int parLevel2Id2Traseiro)
        {
            ParLevel1 parLevel1 = new ParLevel1();
            using (var db = new SgqDbDevEntities())
            {
                parLevel1 = db.ParLevel1.FirstOrDefault(r => r.Id == 3);
            }
            var _result = new ResultTotalNC();

            var query = "\n SELECT * from (SELECT ISNULL (SUM(CONVERT(Decimal,DefectsResult)), 0) as ncDianteiro                                            " +
                        "\n FROM CollectionLevel2                                                                                                           " +
                        "\n WHERE parlevel1_Id = " + parLevel1.Id + "                                                                                       " +
                        "\n and ParLevel2_Id = " + parLevel2IdDianteiro + " --Dianteiro                                                                     " +
                        "\n and UnitId = " + receive.Unit +
                        "\n and CollectionDate Between ('" + receive.Data.ToString() + " 00:00:00.0000000') and ('" + receive.Data + " 23:59:59.0000000')   " +
                        "\n ) j, (                                                                                                                          " +
                        "\n select ISNULL (SUM(CONVERT(Decimal,DefectsResult)), 0) as ncTraseiro                                                            " +
                        "\n from CollectionLevel2                                                                                                           " +
                        "\n WHERE parlevel1_Id = " + parLevel1.Id + "                                                                                       " +
                        "\n and ParLevel2_Id = " + parLevel2Id2Traseiro + " --Traseiro                                                                      " +
                        "\n and UnitId = " + receive.Unit +
                        "\n and CollectionDate Between ('" + receive.Data + " 00:00:00.0000000') and ('" + receive.Data + " 23:59:59.0000000')              " +
                        "\n ) m";

            using (var db = new SgqDbDevEntities())
            {
                _result = db.Database.SqlQuery<ResultTotalNC>(query).FirstOrDefault();
            }

            return _result;
        }

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
