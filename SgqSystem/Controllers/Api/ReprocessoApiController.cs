using ADOFactory;
using Dominio;
using SgqSystem.Handlres;
using SgqSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("api/Reprocesso")]
    public class ReprocessoApiController : ApiController
    {
        public class RetrocessoReturn
        {
           public List<ParReprocessoHeaderOP> parReprocessoHeaderOPs { get; set; }
           public List<ParReprocessoCertificadosSaidaOP> parReprocessoCertificadosSaidaOP { get; set; }
           public List<ParReprocessoSaidaOP> parReprocessoSaidaOPs { get; set; }
           public List<ParReprocessoEntradaOP> parReprocessoEntradaOPs { get; set; }
           public List<Header> headerFieldsEntrada { get; set; }
           public List<Header> headerFieldsSaida { get; set; }
        }

        public class ParReprocessoHeaderOP
        {
            public decimal nCdOrdemProducao { get; set; }
            public decimal nCdEmpresa { get; set; }
            public DateTime dLancamento { get; set; }
            public decimal nCdUsuario { get; set; }
            public String cCdRastreabilidade { get; set; }
            public String cValidaHabilitacaoEntrada { get; set; }
            public decimal nCdHabilitacao { get; set; }
            public String cNmHabilitacao { get; set; }
            public String cSgHabilitacao { get; set; }
        }

        public class ParReprocessoCertificadosSaidaOP
        {
            public decimal nCdOrdemProducao { get; set; }
            public decimal nCdCertificacao { get; set; }
            public String cNmCertificacao { get; set; }
            public String cSgCertificacao { get; set; }
            public decimal nCdEmpresa { get; set; }
        }

        public class ParReprocessoSaidaOP
        {
            public decimal nCdOrdemProducao { get; set; }
            public int iItem { get; set; }
            public decimal nCdProduto { get; set; }
            public int iQtdePrevista { get; set; }
            public String cQtdeTipo { get; set; }
            public decimal nCdLocalEstoque { get; set; }
            public String cNmLocalEstoque { get; set; }
            public DateTime dProducao { get; set; }
            public DateTime dValidade { get; set; }
            public int iTotalPeca { get; set; }
            public int iTotalVolume { get; set; }
            public decimal nTotalPeso { get; set; }
            public Produto produto { get; set; }
            public decimal nCdEmpresa { get; set; }

        }

        public class ParReprocessoEntradaOP
        {
            public decimal nCdOrdemProducao { get; set; }
            public decimal nCdProduto { get; set; }
            public DateTime dProducao { get; set; }
            public DateTime dEmbalagem { get; set; }
            public DateTime dValidade { get; set; }
            public decimal nCdLocalEstoque { get; set; }
            public String cNmLocalEstoque { get; set; }
            public String cCdOrgaoRegulador { get; set; }
            public String cCdRastreabilidade { get; set; }
            public int iVolume { get; set; }
            public decimal nPesoLiquido { get; set; }
            public Produto produto { get; set; }
            public decimal nCdEmpresa { get; set; }
        }

        public class Produto
        {
            public decimal nCdProduto { get; set; }
            public String cNmProduto { get; set; }
            public String cDescricaoDetalhada { get; set; }
        }

        public class Header
        {
            public String Id { get; set; }
        }

        [Route("Get/{ParCompany_Id}")]
        [HttpGet]
        public RetrocessoReturn Get(int ParCompany_Id)
        {
            

            SgqDbDevEntities sgqDbDevEntities = new SgqDbDevEntities();            

            var parCompany = sgqDbDevEntities.ParCompany.FirstOrDefault(r => r.Id == ParCompany_Id);

            Factory factorySgq = new Factory("DbContextSgqEUA");

            var userSQL = "UserGQualidade";
            var passSQL = "grJsoluco3s";

            if(AppSettingsWebConfig.GetValue("BuildEm") == "DesenvolvimentoDeployServidorGrtParaTeste")
            {
                userSQL = "sa";
                passSQL = "betsy1";
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
                    parReprocessoHeaderOPs = factoryParReprocessoHeaderOP.SearchQuery<ParReprocessoHeaderOP>("EXEC " + AppSettingsWebConfig.GetValue("PROC_ParReprocessoHeaderOP") + " " + parCompany.CompanyNumber),
                    parReprocessoCertificadosSaidaOP = factoryParReprocessoCertificadosSaidaOP.SearchQuery<ParReprocessoCertificadosSaidaOP>("EXEC " + AppSettingsWebConfig.GetValue("PROC_ParReprocessoCertificadosSaidaOP") + " " + parCompany.CompanyNumber),
                    parReprocessoSaidaOPs = factoryParReprocessoSaidaOP.SearchQuery<ParReprocessoSaidaOP>("EXEC " + AppSettingsWebConfig.GetValue("PROC_ParReprocessoSaidaOP") + " " + parCompany.CompanyNumber).Select(r =>
                    {
                        r.produto = factorySgq.SearchQuery<Produto>("SELECT * FROM Produto WHERE nCdProduto = " + r.nCdProduto.ToString()).FirstOrDefault();
                        if (r.produto != null)
                        {
                            r.produto.cNmProduto = r.produto.cNmProduto.Replace("\"", "");
                        }
                        return r;
                    }).ToList(),
                    parReprocessoEntradaOPs =
                    factoryParReprocessoEntradaOP.SearchQuery<ParReprocessoEntradaOP>("EXEC " + AppSettingsWebConfig.GetValue("PROC_ParReprocessoEntradaOP") + " " + parCompany.CompanyNumber).Select(r =>
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

            return null;

        }

        [Route("GetCollectionLevel2Reprocesso/{ParCompany_Id}/{dtIni}/{dtFim}/{headerEntrada}/{headerSaida}")]
        [HttpGet]
        public IEnumerable<dynamic> GetCollectionLevel2Reprocesso(int ParCompany_Id, string dtIni, string dtFim, int headerEntrada, int headerSaida)
        {
            Factory factory = new Factory("DbContextSgqEUA");
            //SgqDbDevEntities sgqDbDevEntities = new SgqDbDevEntities(false);

            //sgqDbDevEntities.Configuration.LazyLoadingEnabled = false;
            //    public IEnumerable<CollectionLevel2> GetCollectionLevel2Reprocesso(int ParCompany_Id, DateTime dtIni, DateTime dtFim)
            //{
            //    Factory factory = new Factory("DbContextSgqEUA");
            //    SgqDbDevEntities sgqDbDevEntities = new SgqDbDevEntities(false);

            //    sgqDbDevEntities.Configuration.LazyLoadingEnabled = false;

            //var ID_parLevel1 = sgqDbDevEntities.ParLevel1.FirstOrDefault(r => r.hashKey == 6).Id;

            var query = @"SELECT R3.Value
                        FROM CollectionLevel2 C2
                        LEFT JOIN CollectionLevel2XParHeaderField R3
                        ON R3.CollectionLevel2_Id = C2.Id
                        LEFT JOIN CollectionLevel2Object C2O
                        ON C2O.CollectionLevel2_Id = C2.Id
                        WHERE C2.UnitId = " + ParCompany_Id.ToString() + @"
                        AND ParHeaderField_Id in (" + headerEntrada.ToString() + " ," + headerSaida.ToString() + @")
                        AND C2.CollectionDate BETWEEN '" + dtIni.ToString() + " 00:00' AND '" + dtFim.ToString() + " 23:59' GROUP BY R3.Value";


            var retorno = factory.QueryNinjaADO(query);

            return retorno; // retorno;


           //var retorno = sgqDbDevEntities.CollectionLevel2

           //     .Where(
           //     r => r.UnitId == ParCompany_Id 
           //     && r.CollectionDate >= dtIni 
           //     && r.CollectionDate <= dtFim 
           //     //&& r.ParLevel1_Id == ID_parLevel1
           //     ).ToList();

           // retorno = retorno
           //     .Select(r => 
           //     {
           //         r.CollectionLevel21 = null;
           //         r.CollectionLevel22 = null;
           //         return r;
           //     })
           //     .ToList();

           // return retorno;

        }

        [Route("GetUnidadeMedida/{level3_id}")]
        [HttpGet]
        public string GetUnidadeMedida(int level3_id)
        {

            Factory factory = new Factory("DbContextSgqEUA");
            SgqDbDevEntities sgqDbDevEntities = new SgqDbDevEntities(false);

            sgqDbDevEntities.Configuration.LazyLoadingEnabled = false;

            var query = @"select max(p3u.name) unidade from parlevel3 p3
                            left join parlevel3value p3v
                            on p3v.parlevel3_id = p3.id
                            left join parMeasurementUnit p3u
                            on p3u.id = p3v.parMeasurementUnit_id
                            where p3.id = " + level3_id.ToString();

            string valor = "";

            valor = sgqDbDevEntities.Database.SqlQuery<string>(query).FirstOrDefault();

            return valor;
        }

        [Route("GetReportReprocesso/{cl2_Id}/{ParCompany_Id}/{dtIni}/{dtFim}/{cabecalho_idEntrada}/{level2_idEntrada}/{cabecalho_idSaida}/{level2_idSaida}")]
        [HttpGet]
        public IEnumerable<dynamic> GetReportReprocesso(int cl2_Id, int ParCompany_Id, string dtIni, string dtFim, int cabecalho_idEntrada, int level2_idEntrada, int cabecalho_idSaida, int level2_idSaida)
        {
            Factory factory = new Factory("DbContextSgqEUA");
            SgqDbDevEntities sgqDbDevEntities = new SgqDbDevEntities(false);

            sgqDbDevEntities.Configuration.LazyLoadingEnabled = false;



            //var retorno = sgqDbDevEntities.CollectionLevel2

            //    .Where(r => r.Id == CollectionLevel2_Id).ToList();

            //var query = @"SELECT *
            //              FROM CollectionLevel2 C2
            //              LEFT JOIN Result_Level3 R3
            //              ON R3.CollectionLevel2_Id = C2.Id
            //              LEFT JOIN CollectionLevel2Object C2O
            //              ON C2O.CollectionLevel2_Id = C2.Id
            //              WHERE C2.Id = " + cl2_Id.ToString();

            //var query = @"SELECT *
            //            FROM CollectionLevel2 C2
            //            LEFT JOIN Result_Level3 R3
            //            ON R3.CollectionLevel2_Id = C2.Id
            //            LEFT JOIN CollectionLevel2XParHeaderField HF
            //            ON R3.CollectionLevel2_Id = C2.Id
            //            LEFT JOIN CollectionLevel2Object C2O
            //            ON C2O.CollectionLevel2_Id = C2.Id
            //            WHERE C2.UnitId = " + ParCompany_Id.ToString() + @"
            //            AND ParHeaderField_Id in (41,47)
            //            AND C2.CollectionDate BETWEEN '" + dtIni.ToString() + "' AND '" + dtFim.ToString() + @"'
            //            AND HF.Value = '" + cl2_Id.ToString() + "'";

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
                            AND C2.CollectionDate BETWEEN '" + dtIni.ToString() + " 00:00' AND '" + dtFim.ToString() + @" 23:59'
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
                            AND C2.CollectionDate BETWEEN '" + dtIni.ToString() + " 00:00' AND '" + dtFim.ToString() + @" 23:59'
                            AND HF.Value = '" + cl2_Id.ToString() + @"' 
                            -- GROUP BY C2.id, c2.collectiondate, C2.PARLEVEL2_ID
                           

                        )";



            var retorno = factory.QueryNinjaADO(query);


            //retorno = retorno
            //    .Select(r =>
            //    {
            //        r.CollectionLevel21 = null;
            //        r.CollectionLevel22 = null;
            //        return r;
            //    })
            //    .ToList();

            return retorno; // retorno;

        }


        [Route("GetReportReprocessoHeader/{cl2_Id}/{unitId}/{dtIni}/{dtFim}/{cabecalho_idEntrada}/{level2_idEntrada}/{cabecalho_idSaida}/{level2_idSaida}")]
        [HttpGet]
        public IEnumerable<dynamic> GetReportReprocessoHeader(int cl2_Id, int unitId, string dtIni, string dtFim, int cabecalho_idEntrada, int level2_idEntrada, int cabecalho_idSaida, int level2_idSaida)
        {

            var ParCompany_Id = unitId;

            Factory factory = new Factory("DbContextSgqEUA");
            SgqDbDevEntities sgqDbDevEntities = new SgqDbDevEntities(false);

            sgqDbDevEntities.Configuration.LazyLoadingEnabled = false;



            //var retorno = sgqDbDevEntities.CollectionLevel2

            //    .Where(r => r.Id == CollectionLevel2_Id).ToList();

            //var query = @"SELECT *
            //            FROM CollectionLevel2 C2
            //            LEFT JOIN CollectionLevel2XParHeaderField R3
            //            ON R3.CollectionLevel2_Id = C2.Id
            //            LEFT JOIN CollectionLevel2Object C2O
            //            ON C2O.CollectionLevel2_Id = C2.Id
            //            WHERE C2.Id = " + cl2_Id.ToString();

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
                            AND HF.ParHeaderField_Id in ("+ cabecalho_idEntrada.ToString() + " ," + cabecalho_idSaida.ToString() + @")
                            AND C2.CollectionDate BETWEEN '" + dtIni.ToString() + " 00:00' AND '" + dtFim.ToString() + @" 23:59'
                            AND HF.Value = '" + cl2_Id.ToString() + "')";

            var retorno = factory.QueryNinjaADO(query);

            return retorno; // retorno;

        }

        [Route("GetMonitor/{user}")]
        [HttpGet]
        public IEnumerable<dynamic> GetMonitor(int user)
        {
            Factory factory = new Factory("DbContextSgqEUA");
            SgqDbDevEntities sgqDbDevEntities = new SgqDbDevEntities(false);

            sgqDbDevEntities.Configuration.LazyLoadingEnabled = false;

            var query = @"SELECT top 1 *
                        FROM UserSgq
                        WHERE Id = " + user.ToString();


            var retorno = factory.QueryNinjaADO(query);

            return retorno; // retorno;

        }
    }
}
