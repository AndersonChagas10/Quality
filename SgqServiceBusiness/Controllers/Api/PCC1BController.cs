using ADOFactory;
using Dominio;
using DTO;
using ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SgqServiceBusiness.Api
{
    public class PCC1BController
    {
        public _PCC1B Next(_Receive receive)
        {
            ParCompany company;
            var retorno = new _PCC1B();
            retorno.Side = 1;

            ResultadosSequencialBanda _result = new ResultadosSequencialBanda();

            var userName = "UserGQualidade";
            var pass = "grJsoluco3s";

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

                    resultQuery = db.SearchQuery<ResultadosSequencialBanda>(query).ToList();

                    if (resultQuery != null && resultQuery.Count() > 0)
                    {
                        retorno.Sequential = resultQuery.FirstOrDefault().iSequencial;
                    }
                    else
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

        public IEnumerable<CollectionLevel2PCC1B> TotalNC(_Receive receive, int parLevel2IdDianteiro, int parLevel2Id2Traseiro, int shift)
        {
            ParLevel1 parLevel1 = new ParLevel1();

            string novoparLevel2IdDianteiro = parLevel2IdDianteiro.ToString().Replace(SyncServiceApiController.quebraProcesso, "-");
            string clusterDianteiro = novoparLevel2IdDianteiro.Split('-')[0];
            parLevel2IdDianteiro = Convert.ToInt32(novoparLevel2IdDianteiro.Split('-')[1]);

            string novoparLevel2Id2Traseiro = parLevel2Id2Traseiro.ToString().Replace(SyncServiceApiController.quebraProcesso, "-");
            string clusterTraseiro = novoparLevel2Id2Traseiro.Split('-')[1];
            parLevel2Id2Traseiro = Convert.ToInt32(novoparLevel2Id2Traseiro.Split('-')[1]);

            using (var db = new SgqDbDevEntities())
            {
                parLevel1 = db.ParLevel1.FirstOrDefault(r => r.Id == 3);
            }

            var query =
                "SELECT FORMAT(CollectionDate, 'MMddyyyy') as CollectionDate, cast(" + clusterDianteiro + " as varchar) + cast(" + SyncServiceApiController.quebraProcesso + " as varchar) + CAST(ParLevel1_Id AS VARCHAR) ParLevel1_Id , cast(" + clusterDianteiro + " as varchar) + cast(" + SyncServiceApiController.quebraProcesso + " as varchar) + CAST(ParLevel2_Id AS VARCHAR) ParLevel2_Id, UnitId,  " +
                "Sequential, Side, DefectsResult, [Key]  FROM CollectionLevel2                                     " +
                "WHERE parlevel1_Id = " + parLevel1.Id + "                                                          " +
                "and shift in (" + shift + ")                    " +
                "and ParLevel2_Id in (" + parLevel2IdDianteiro + ", " + parLevel2Id2Traseiro + ")                    " +
                "and UnitId = " + receive.Unit + "                                                                     " +
                "and CollectionDate Between('" + receive.Data + " 00:00:00.0000000') and('" + receive.Data + " 23:59:59')   ";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                return factory.SearchQuery<CollectionLevel2PCC1B>(query).ToList();
            }

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
