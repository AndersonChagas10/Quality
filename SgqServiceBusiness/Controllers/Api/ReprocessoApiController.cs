using ADOFactory;
using Dominio;
using ServiceModel;
using SgqServiceBusiness.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace SgqServiceBusiness.Api
{
    public class ReprocessoApiController
    {
        string PROC_ParReprocessoEntradaOP;
        string PROC_ParReprocessoCertificadosSaidaOP;
        string PROC_ParReprocessoSaidaOP;
        string PROC_ParReprocessoHeaderOP;
        string BuildEm;
        public ReprocessoApiController(string PROC_ParReprocessoEntradaOP, string BuildEm
            ,string PROC_ParReprocessoCertificadosSaidaOP, string PROC_ParReprocessoSaidaOP
            ,string PROC_ParReprocessoHeaderOP)
        {
            this.PROC_ParReprocessoEntradaOP = PROC_ParReprocessoEntradaOP;
            this.PROC_ParReprocessoCertificadosSaidaOP = PROC_ParReprocessoCertificadosSaidaOP;
            this.PROC_ParReprocessoSaidaOP = PROC_ParReprocessoSaidaOP;
            this.PROC_ParReprocessoHeaderOP = PROC_ParReprocessoHeaderOP;
            this.BuildEm = BuildEm;
        }

        public RetrocessoReturn Get(int ParCompany_Id)
        {
            try
            {
                SgqDbDevEntities sgqDbDevEntities = new SgqDbDevEntities();

                var parCompany = sgqDbDevEntities.ParCompany.FirstOrDefault(r => r.Id == ParCompany_Id);

                Factory factorySgq = new Factory("DefaultConnection");

                var userSQL = "UserGQualidade";
                var passSQL = "grJsoluco3s";

                if (BuildEm == "DesenvolvimentoDeployServidorGrtParaTeste")
                {
                    userSQL = "sa";
                    //passSQL = "betsy1";
                    passSQL = "1qazmko0";
                }

                using (var db = new SgqDbDevEntities())
                {
                    var log = new LogJson();
                    log.Device_Id = "Web";
                    log.callback = "Reprocesso";
                    log.AddDate = DateTime.Now;
                    log.log =
                        "{ \"User\" : \"" + userSQL + "\", " +
                        " \"Password\" : \"" + passSQL + "\" " +
                        " \"IPServer\" : \"" + parCompany.IPServer + "\" " +
                        " \"DBServer\" : \"" + parCompany.DBServer + "\"} ";


                    log.result =
                        "{ \"User\" : \"" + userSQL + "\", " +
                        " \"Password\" : \"" + passSQL + "\" " +
                        " \"IPServer\" : \"" + parCompany.IPServer + "\" " +
                        " \"DBServer\" : \"" + parCompany.DBServer + "\"} ";

                    db.LogJson.Add(log);

                    db.LogJson.Add(log);
                    db.SaveChanges();
                }


                Factory factoryParReprocessoHeaderOP = new Factory(parCompany.IPServer, parCompany.DBServer, passSQL, userSQL);
                Factory factoryParReprocessoCertificadosSaidaOP = new Factory(parCompany.IPServer, parCompany.DBServer, passSQL, userSQL);
                Factory factoryParReprocessoEntradaOP = new Factory(parCompany.IPServer, parCompany.DBServer, passSQL, userSQL);
                Factory factoryParReprocessoSaidaOP = new Factory(parCompany.IPServer, parCompany.DBServer, passSQL, userSQL);

                if (parCompany != null)
                {
                    return new RetrocessoReturn
                    {
                        parReprocessoHeaderOPs = factoryParReprocessoHeaderOP.SearchQuery<ParReprocessoHeaderOP>("EXEC " + PROC_ParReprocessoHeaderOP + " " + parCompany.CompanyNumber),
                        parReprocessoCertificadosSaidaOP = factoryParReprocessoCertificadosSaidaOP.SearchQuery<ParReprocessoCertificadosSaidaOP>("EXEC " + PROC_ParReprocessoCertificadosSaidaOP + " " + parCompany.CompanyNumber),
                        parReprocessoSaidaOPs = factoryParReprocessoSaidaOP.SearchQuery<ParReprocessoSaidaOP>("EXEC " + PROC_ParReprocessoSaidaOP + " " + parCompany.CompanyNumber).Select(r =>
                        {
                            r.produto = factorySgq.SearchQuery<Produto>("SELECT * FROM Produto WHERE nCdProduto = " + r.nCdProduto.ToString()).FirstOrDefault();
                            if (r.produto != null)
                            {
                                r.produto.cNmProduto = r.produto.cNmProduto.Replace("\"", "");
                            }
                            return r;
                        }).ToList(),
                        parReprocessoEntradaOPs =
                        factoryParReprocessoEntradaOP.SearchQuery<ParReprocessoEntradaOP>("EXEC " + PROC_ParReprocessoEntradaOP + " " + parCompany.CompanyNumber).Select(r =>
                        {
                            r.produto = factorySgq.SearchQuery<Produto>("SELECT * FROM Produto WHERE nCdProduto = " + r.nCdProduto).FirstOrDefault();
                            if (r.produto != null)
                            {
                                r.produto.cNmProduto = r.produto.cNmProduto.Replace("\"", "");
                            }
                            return r;
                        }).ToList(),
                        headerFieldsEntrada = factorySgq.SearchQuery<Header>("SELECT 'cb'+ CAST(id AS VARCHAR(400)) AS Id FROM ParHeaderField WHERE Description like 'ReprocessoEntrada%'"),
                        headerFieldsSaida = factorySgq.SearchQuery<Header>("SELECT 'cb'+ CAST(id AS VARCHAR(400)) AS Id FROM ParHeaderField WHERE Description like 'ReprocessoSaida%'")
                    };

                }


            }
            catch (Exception e)
            {

            }

            return null;


        }

        public IEnumerable<dynamic> GetCollectionLevel2Reprocesso(int ParCompany_Id, string dtIni, string dtFim, int headerEntrada, int headerSaida)
        {
            Factory factory = new Factory("DefaultConnection");

            var query = @"SELECT R3.Value
                        FROM CollectionLevel2 C2
                        LEFT JOIN CollectionLevel2XParHeaderField R3
                        ON R3.CollectionLevel2_Id = C2.Id
                        LEFT JOIN CollectionLevel2Object C2O
                        ON C2O.CollectionLevel2_Id = C2.Id
                        WHERE C2.UnitId = " + ParCompany_Id.ToString() + @"
                        AND ParHeaderField_Id in (" + headerEntrada.ToString() + " ," + headerSaida.ToString() + @")
                        AND C2.CollectionDate BETWEEN '" + dtIni.ToString() + " 00:00' AND '" + dtFim.ToString() + "  23:59:59' GROUP BY R3.Value";


            var retorno = factory.QueryNinjaADO(query);

            return retorno; // retorno;
        }

        public string GetUnidadeMedida(int level3_id)
        {
            Factory factory = new Factory("DefaultConnection");

            var query = @"select max(p3u.name) unidade from parlevel3 p3
                            left join parlevel3value p3v
                            on p3v.parlevel3_id = p3.id
                            left join parMeasurementUnit p3u
                            on p3u.id = p3v.parMeasurementUnit_id
                            where p3.id = " + level3_id.ToString();

            string valor = "";

            valor = factory.SearchQuery<string>(query).FirstOrDefault();

            return valor;
        }

        public IEnumerable<dynamic> GetReportReprocesso(int cl2_Id, int ParCompany_Id, string dtIni, string dtFim, int cabecalho_idEntrada, int level2_idEntrada, int cabecalho_idSaida, int level2_idSaida)
        {
            Factory factory = new Factory("DefaultConnection");
            SgqDbDevEntities sgqDbDevEntities = new SgqDbDevEntities();

            sgqDbDevEntities.Configuration.LazyLoadingEnabled = false;

            var query = @"SELECT  C2.*, R3.*, (

                            select max(p3u.name) unidade from parlevel3 p3
                            left join parlevel3value p3v
                            on p3v.parlevel3_id = p3.id
                            left join parMeasurementUnit p3u
                            on p3u.id = p3v.parMeasurementUnit_id
                            where p3.id = R3.parLevel3_id
                        ) unidadeMedida
                        FROM CollectionLevel2 C2 with(nolock)
                        LEFT JOIN Result_Level3 R3 with(nolock)
                        ON R3.CollectionLevel2_Id = C2.Id
                        /*
                        LEFT JOIN CollectionLevel2XParHeaderField HF with(nolock)
                        ON HF.CollectionLevel2_Id = C2.Id
                        LEFT JOIN CollectionLevel2Object C2O with(nolock)
                        ON C2O.CollectionLevel2_Id = C2.Id
                        */
                        WHERE C2.ID = (

                            SELECT  MAX(C2.id) ID
                            FROM CollectionLevel2 C2 with(nolock)
                            LEFT JOIN Result_Level3 R3 with(nolock)
                            ON R3.CollectionLevel2_Id = C2.Id
                            LEFT JOIN CollectionLevel2XParHeaderField HF with(nolock)
                            ON HF.CollectionLevel2_Id = C2.Id
                            LEFT JOIN CollectionLevel2Object C2O with(nolock)
                            ON C2O.CollectionLevel2_Id = C2.Id
                            WHERE C2.UnitId = " + ParCompany_Id.ToString() + @"
                            AND ParLevel2_Id = " + level2_idEntrada.ToString() + @"
                            AND ParHeaderField_Id = " + cabecalho_idEntrada.ToString() + @"
                            AND C2.CollectionDate BETWEEN '" + dtIni.ToString() + " 00:00' AND '" + dtFim.ToString() + @"  23:59:59'
                            AND HF.Value = '" + cl2_Id.ToString() + @"' 
                            -- GROUP BY C2.id, c2.collectiondate, C2.PARLEVEL2_ID
                            

                        )


                        union all

                        SELECT C2.*, R3.*, (

                            select max(p3u.name) unidade from parlevel3 p3
                            left join parlevel3value p3v
                            on p3v.parlevel3_id = p3.id
                            left join parMeasurementUnit p3u
                            on p3u.id = p3v.parMeasurementUnit_id
                            where p3.id = R3.parLevel3_id
                        ) unidadeMedida
                        FROM CollectionLevel2 C2 with(nolock)
                        LEFT JOIN Result_Level3 R3 with(nolock)
                        ON R3.CollectionLevel2_Id = C2.Id
                        /*
                        LEFT JOIN CollectionLevel2XParHeaderField HF with(nolock)
                        ON HF.CollectionLevel2_Id = C2.Id
                        LEFT JOIN CollectionLevel2Object C2O with(nolock)
                        ON C2O.CollectionLevel2_Id = C2.Id
                        */
                        WHERE C2.ID = (

                            SELECT  MAX(C2.id) ID
                            FROM CollectionLevel2 C2 with(nolock)
                            LEFT JOIN Result_Level3 R3 with(nolock)
                            ON R3.CollectionLevel2_Id = C2.Id
                            LEFT JOIN CollectionLevel2XParHeaderField HF with(nolock)
                            ON HF.CollectionLevel2_Id = C2.Id
                            LEFT JOIN CollectionLevel2Object C2O with(nolock)
                            ON C2O.CollectionLevel2_Id = C2.Id
                            WHERE C2.UnitId = " + ParCompany_Id.ToString() + @"
                            AND ParLevel2_Id = " + level2_idSaida.ToString() + @"
                            AND ParHeaderField_Id = " + cabecalho_idSaida.ToString() + @"
                            AND C2.CollectionDate BETWEEN '" + dtIni.ToString() + " 00:00' AND '" + dtFim.ToString() + @"  23:59:59'
                            AND HF.Value = '" + cl2_Id.ToString() + @"' 
                            -- GROUP BY C2.id, c2.collectiondate, C2.PARLEVEL2_ID
                           

                        )";

            var retorno = factory.QueryNinjaADO(query);

            return retorno; // retorno;

        }

        public IEnumerable<dynamic> GetReportReprocessoHeader(int cl2_Id, int unitId, string dtIni, string dtFim, int cabecalho_idEntrada, int level2_idEntrada, int cabecalho_idSaida, int level2_idSaida)
        {
            var ParCompany_Id = unitId;

            Factory factory = new Factory("DefaultConnection");
            SgqDbDevEntities sgqDbDevEntities = new SgqDbDevEntities();

            sgqDbDevEntities.Configuration.LazyLoadingEnabled = false;

            var query = @"SELECT C2.*, HF.*, C2O.*, ISNULL(PMV.Name,HF.Value) as ValueMultiple
                        FROM CollectionLevel2 C2
                        LEFT JOIN CollectionLevel2XParHeaderField HF
                        ON HF.CollectionLevel2_Id = C2.Id
                        LEFT JOIN CollectionLevel2Object C2O
                        ON C2O.CollectionLevel2_Id = C2.Id
                        LEFT JOIN ParMultipleValues PMV
                        ON cast(PMV.Id as varchar) = HF.Value and HF.ParFieldType_Id = 1
                        WHERE 
                            C2.ID IN (
                            SELECT C2.ID
                            FROM CollectionLevel2 C2
                            LEFT JOIN CollectionLevel2XParHeaderField HF
                            ON HF.CollectionLevel2_Id = C2.Id
                            LEFT JOIN CollectionLevel2Object C2O
                            ON C2O.CollectionLevel2_Id = C2.Id
                            LEFT JOIN ParMultipleValues PMV
                            ON cast(PMV.Id as varchar) = HF.Value and HF.ParFieldType_Id = 1
                            WHERE C2.UnitId = " + ParCompany_Id.ToString() + @"
                            AND HF.ParHeaderField_Id in (" + cabecalho_idEntrada.ToString() + " ," + cabecalho_idSaida.ToString() + @")
                            AND C2.CollectionDate BETWEEN '" + dtIni.ToString() + " 00:00' AND '" + dtFim.ToString() + @"  23:59:59'
                            AND HF.Value = '" + cl2_Id.ToString() + "')";

            var retorno = factory.QueryNinjaADO(query);

            return retorno; // retorno;
        }

        public IEnumerable<dynamic> GetMonitor(int user)
        {
            Factory factory = new Factory("DefaultConnection");
            SgqDbDevEntities sgqDbDevEntities = new SgqDbDevEntities();

            sgqDbDevEntities.Configuration.LazyLoadingEnabled = false;

            var query = @"SELECT top 1 *
                        FROM UserSgq
                        WHERE Id = " + user.ToString();

            var retorno = factory.QueryNinjaADO(query);

            return retorno; // retorno;
        }
    }
}
