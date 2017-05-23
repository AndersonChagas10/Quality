using Dominio;
using DTO.Helpers;
using DTO.ResultSet;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api.RelatoriosBrasil
{
    [RoutePrefix("api/VisaoGeralDaArea")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VisaoGeralDaAreaApiController : ApiController
    {

        private List<VisaoGeralDaAreaResultSet> _mock { get; set; }
        private List<VisaoGeralDaAreaResultSet> _list { get; set; }

        [HttpPost]
        [Route("Grafico1")]
        public List<VisaoGeralDaAreaResultSet> Grafico1([FromBody] FormularioParaRelatorioViewModel form)
        {
            CriaMockG1();
            //return _mock;
            return _list;
        }

        [HttpPost]
        [Route("Grafico2/{regId}")]
        public List<VisaoGeralDaAreaResultSet> Grafico2([FromBody] FormularioParaRelatorioViewModel form, int regId)
        {
            CriaMockG2();
            //return _mock;
            return _list;
        }

        [HttpPost]
        [Route("Grafico3/{regId}")]
        public List<VisaoGeralDaAreaResultSet> Grafico3([FromBody] FormularioParaRelatorioViewModel form, int regId)
        {
            CriaMockG3(form);
            return _mock;
        }

        [HttpPost]
        [Route("Grafico4/{regId}")]
        public List<VisaoGeralDaAreaResultSet> Grafico4([FromBody] FormularioParaRelatorioViewModel form, int regId)
        {
            CriaMockG4();
            return _mock;
        }

        [HttpPost]
        [Route("Grafico5/{regId}")]
        public List<VisaoGeralDaAreaResultSet> Grafico5([FromBody] FormularioParaRelatorioViewModel form, int regId)
        {
            CriaMockG5();
            return _mock;
        }

        /// <summary>
        /// A query para o grafico1 da Visão geral da área SGQ deve retornar:
        ///   
        ///   Coluna            |   Tipagem
        ///                     |
        ///   regId             |   Int
        ///   regName           |   string
        ///   scorecardJbs      |   decimal
        ///   scorecardJbsReg   |   decimal
        ///   
        /// Objeto a ser utilizado: VisaoGeralDaAreaResultSet
        /// </summary>
        private void CriaMockG1()
        {
            //_mock = new List<VisaoGeralDaAreaResultSet>();

            //_mock.Add(new VisaoGeralDaAreaResultSet()
            //{
            //    regId = 1,
            //    regName = "reg1",
            //    scorecardJbs = 80M,
            //    scorecardJbsReg = 26.5M
            //});

            //_mock.Add(new VisaoGeralDaAreaResultSet()
            //{
            //    regId = 2,
            //    regName = "reg2",
            //    scorecardJbs = 80M,
            //    scorecardJbsReg = 66.2M
            //});

            _list = new List<VisaoGeralDaAreaResultSet>();

            var query = "" +

// "\n DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + "'    " +
// "\n DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + "'       " +
"\n IF OBJECT_ID('tempdb.dbo.#SCORE', 'U') IS NOT NULL																																																										" +
"\n   DROP TABLE #SCORE; 																																																																	" +
"\n 																																																																						" +
"\n CREATE TABLE #SCORE (																																																																	" +
"\n 	Cluster int null,																																																																	" +
"\n 	ClusterName Varchar(max) null,																																																														" +
"\n 	Regional int null,																																																																	" +
"\n 	RegionalName Varchar(max) null, 																																																													" +
"\n 	ParCompany_Id int null,																																																																" +
"\n 	ParCompanyName Varchar(max) null, 																																																													" +
"\n 	TipoIndicador int null,																																																																" +
"\n 	TipoIndicadorName Varchar(max) null, 																																																												" +
"\n 	Level1Id int null,																																																																	" +
"\n 	Level1Name Varchar(max) null, 																																																														" +
"\n 	Criterio int null,																																																																	" +
"\n 	CriterioName Varchar(max) null, 																																																													" +
"\n 	Av decimal(30,5) null,																																																																" +
"\n 	Nc decimal(30,5) null,																																																																" +
"\n 	Pontos decimal(30,5) null,																																																															" +
"\n 	PontosIndicador decimal(30,5) null,																																																													" +
"\n 	Meta decimal(30,5) null,																																																															" +
"\n 	Real decimal(30,5) null,																																																															" +
"\n 	PontosAtingidos decimal(30,5) null,																																																													" +
"\n 	Scorecard decimal(30,5) null,																																																														" +
"\n 	TipoScore Varchar(max) null																																																															" +
"\n 	)																																																																					" +
"\n 																																																																						" +
"\n 																																																																						" +
"\n 																																																																						" +
"\n DECLARE @I INT = 1																																																																		" +
"\n 																																																																						" +
"\n WHILE (SELECT @I) < 100																																																																	" +
"\n BEGIN  																																																																					" +
"\n    																																																																						" +
"\n     																																																																					" +
"\n 																																																																						" +
"\n  DECLARE @ParCompany_Id INT = @I                                                                                                                                                                                                                                     					" +
"\n  DECLARE @DATAINICIAL DATETIME = '20160501 00:00'                                                                                                                                                                                                                    					" +
"\n  DECLARE @DATAFINAL   DATETIME = '20170522 23:59'                                                                                                                                                                                                                    					" +
"\n  CREATE TABLE #AMOSTRATIPO4 ( 																																																															" +
"\n  UNIDADE INT NULL, 																																																																		" +
"\n  INDICADOR INT NULL, 																																																																	" +
"\n  AM INT NULL, 																																																																			" +
"\n  DEF_AM INT NULL 																																																																		" +
"\n  ) 																																																																						" +
"\n  INSERT INTO #AMOSTRATIPO4 																																																																" +
"\n  SELECT 																																																																				" +
"\n   UNIDADE, INDICADOR, 																																																																	" +
"\n  COUNT(1) AM 																																																																			" +
"\n  ,SUM(DEF_AM) DEF_AM 																																																																	" +
"\n  FROM 																																																																					" +
"\n  ( 																																																																						" +
"\n      SELECT 																																																																			" +
"\n      cast(C2.CollectionDate as DATE) AS DATA 																																																											" +
"\n      , C.Id AS UNIDADE 																																																																	" +
"\n      , C2.ParLevel1_Id AS INDICADOR 																																																													" +
"\n      , C2.EvaluationNumber AS AV 																																																														" +
"\n      , C2.Sample AS AM 																																																																	" +
"\n      , case when SUM(C2.WeiDefects) = 0 then 0 else 1 end DEF_AM 																																																						" +
"\n      FROM CollectionLevel2 C2 																																																															" +
"\n      INNER JOIN ParLevel1 L1 																																																															" +
"\n      ON L1.Id = C2.ParLevel1_Id 																																																														" +
"\n      INNER JOIN ParCompany C 																																																															" +
"\n      ON C.Id = C2.UnitId 																																																																" +
"\n      where cast(C2.CollectionDate as DATE) BETWEEN @DATAINICIAL AND @DATAFINAL 																																																			" +
"\n      and C2.NotEvaluatedIs = 0 																																																															" +
"\n      and C2.Duplicated = 0 																																																																" +
"\n      and L1.ParConsolidationType_Id = 4 																																																												" +
"\n      group by C.Id, ParLevel1_Id, EvaluationNumber, Sample, cast(CollectionDate as DATE) 																																																" +
"\n  ) TAB 																																																																					" +
"\n  GROUP BY UNIDADE, INDICADOR 																																																															" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n  DECLARE @VOLUMEPCC INT                                                                                                                                                                                                                                              					" +
"\n  DECLARE @DIASABATE INT                                                                                                                                                                                                                                              					" +
"\n  DECLARE @DIASDEVERIFICACAO INT                                                                                                                                                                                                                                      					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n  DECLARE @AVFREQUENCIAVERIFICACAO INT                                                                                                                                                                                                                                					" +
"\n  DECLARE @NCFREQUENCIAVERIFICACAO INT                                                                                                                                                                                                                                					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n  /* INICIO DADOS DA FREQUENCIA ------------------------------------------------------*/                                                                                                                                                                              					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n  DECLARE @CLUSTER INT                                                                                                                                                                                                                                                					" +
"\n  DECLARE @CLUSTERNAME VARCHAR(MAX)                                                                                                                                                                                                                                   					" +
"\n  DECLARE @REGIONAL INT                                                                                                                                                                                                                                               					" +
"\n  DECLARE @REGIONALNAME VARCHAR(MAX)                                                                                                                                                                                                                                  					" +
"\n  DECLARE @PARCOMPANY INT                                                                                                                                                                                                                                             					" +
"\n  DECLARE @PARCOMPANYNAME VARCHAR(MAX)                                                                                                                                                                                                                                					" +
"\n  DECLARE @CRITERIO INT                                                                                                                                                                                                                                               					" +
"\n  DECLARE @CRITERIONAME VARCHAR(MAX)                                                                                                                                                                                                                                  					" +
"\n  DECLARE @PONTOS VARCHAR(MAX)                                                                                                                                                                                                                                        					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n  SELECT                                                                                                                                                                                                                                                              					" +
"\n   @CLUSTER = CL.Id                                                                                                                                                                                                                                                   					" +
"\n  , @CLUSTERNAME = CL.Name                                                                                                                                                                                                                                            					" +
"\n  , @REGIONAL = S.Id                                                                                                                                                                                                                                                  					" +
"\n  , @REGIONALNAME = S.Name                                                                                                                                                                                                                                            					" +
"\n  , @PARCOMPANY = C.Id                                                                                                                                                                                                                                                					" +
"\n  , @PARCOMPANYNAME = C.Name                                                                                                                                                                                                                                          					" +
"\n  , @CRITERIO = L1C.ParCriticalLevel_Id                                                                                                                                                                                                                               					" +
"\n  , @CRITERIONAME = CRL.Name                                                                                                                                                                                                                                          					" +
"\n  , @PONTOS = L1C.Points                                                                                                                                                                                                                                              					" +
"\n  FROM ParCompany C                                                                                                                                                                                                                                                   					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n  LEFT JOIN ParCompanyXStructure CS                                                                                                                                                                                                                                   					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n         ON CS.ParCompany_Id = C.Id                                                                                                                                                                                                                                   					" +
"\n  LEFT JOIN ParStructure S                                                                                                                                                                                                                                            					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n         ON S.Id = CS.ParStructure_Id                                                                                                                                                                                                                                 					" +
"\n  LEFT JOIN ParStructureGroup SG                                                                                                                                                                                                                                      					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n         ON SG.Id = S.ParStructureGroup_Id                                                                                                                                                                                                                            					" +
"\n  LEFT JOIN ParCompanyCluster CCL                                                                                                                                                                                                                                     					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n         ON CCL.ParCompany_Id = C.Id  AND CCL.Active = 1                                                                                                                                                                                                                                 " +
"\n  LEFT JOIN ParCluster CL                                                                                                                                                                                                                                             					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n         ON CL.Id = CCL.ParCluster_Id                                                                                                                                                                                                                                 					" +
"\n  LEFT JOIN ParLevel1XCluster L1C                                                                                                                                                                                                                                     					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n         ON L1C.ParLevel1_Id = 25 AND L1C.ParCluster_Id = Cl.Id   AND L1C.IsActive = 1                                                                                                                                                                                                   " +
"\n  LEFT JOIN ParCriticalLevel CRL                                                                                                                                                                                                                                      					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n         ON L1C.ParCriticalLevel_Id = CRL.Id                                                                                                                                                                                                                          					" +
"\n  WHERE C.Id = @ParCompany_Id                                                                                                                                                                                                                                         					" +
"\n  --AND L1C.ParLevel1_Id = 25                                                                                                                                                                                                                                         					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n  /* FIM DOS DADOS DA FREQUENCIA -----------------------------------------------------*/                                                                                                                                                                              					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n  SELECT TOP 1 @DIASABATE = COUNT(1), @VOLUMEPCC = SUM(Quartos) FROM VolumePcc1b WHERE ParCompany_id = @ParCompany_id AND Data BETWEEN @DATAINICIAL AND @DATAFINAL                                                                                                    					" +
"\n  SELECT @DIASDEVERIFICACAO = COUNT(1) FROM(SELECT CONVERT(DATE, ConsolidationDate) DATA FROM ConsolidationLevel1 CL1 WHERE ParLevel1_Id = 24 AND CONVERT(DATE, ConsolidationDate) BETWEEN @DATAINICIAL AND @DATAFINAL GROUP BY CONVERT(DATE, ConsolidationDate)) VT  					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n  SET @AVFREQUENCIAVERIFICACAO = @DIASABATE                                                                                                                                                                                                                           					" +
"\n  SET @NCFREQUENCIAVERIFICACAO = @DIASDEVERIFICACAO                                                                                                                                                                                                                   					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n  SET @NCFREQUENCIAVERIFICACAO = CASE WHEN @NCFREQUENCIAVERIFICACAO > @AVFREQUENCIAVERIFICACAO THEN @AVFREQUENCIAVERIFICACAO ELSE @NCFREQUENCIAVERIFICACAO END                                                                                                        					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n  --SELECT @AVFREQUENCIAVERIFICACAO, @NCFREQUENCIAVERIFICACAO                                                                                                                                                                                                         					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n  DECLARE @NAPCC INT                                                                                                                                                                                                                                                  					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n  SELECT                                                                                                                                                                                                                                                              					" +
"\n         @NAPCC =                                                                                                                                                                                                                                                    					" +
"\n           COUNT(1)                                                                                                                                                                                                                                                 						" +
"\n           FROM                                                                                                                                                                                                                                                     						" +
"\n      (                                                                                                                                                                                                                                                             						" +
"\n               SELECT                                                                                                                                                                                                                                               						" +
"\n               COUNT(1) AS NA                                                                                                                                                                                                                                       						" +
"\n               FROM CollectionLevel2 C2                                                                                                                                                                                                                             						" +
"\n               LEFT JOIN Result_Level3 C3                                                                                                                                                                                                                           						" +
"\n               ON C3.CollectionLevel2_Id = C2.Id                                                                                                                                                                                                                    						" +
"\n               WHERE convert(date, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL                                                                                                                                                                           						" +
"\n               AND C2.ParLevel1_Id = (SELECT top 1 id FROM Parlevel1 where Hashkey = 1)                                                                                                                                                                             						" +
"\n               AND C2.UnitId = @ParCompany_Id                                                                                                                                                                                                                       						" +
"\n               AND IsNotEvaluate = 1                                                                                                                                                                                                                                						" +
"\n               GROUP BY C2.ID                                                                                                                                                                                                                                       						" +
"\n           ) NA                                                                                                                                                                                                                                                     						" +
"\n           WHERE NA = 2                                                                                                                                                                                                                                             						" +
"\n  																																																																						" +
"\n  INSERT INTO #SCORE																																																																		" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n  SELECT * FROM                                                                                                                                                                                                                                            								" +
"\n  (                                                                                                                                                                                                                                                                   					" +
"\n  SELECT                                                                                                                                                                                                                                                              					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n    Cluster                                                                                                                                                                                                                                                           					" +
"\n   , ClusterName                                                                                                                                                                                                                                                      					" +
"\n   , Regional                                                                                                                                                                                                                                                         					" +
"\n   , RegionalName                                                                                                                                                                                                                                                     					" +
"\n   , ParCompanyId                                                                                                                                                                                                                                                     					" +
"\n   , ParCompanyName                                                                                                                                                                                                                                                   					" +
"\n   , CASE WHEN TipoIndicador = 0 THEN 1 ELSE 2 END TipoIndicador                                                                                                                                                                                                      					" +
"\n   , CASE WHEN TipoIndicador = 0 THEN 'Menor' ELSE 'Maior' END TipoIndicadorName                                                                                                                                                                                      					" +
"\n   , Level1Id                                                                                                                                                                                                                                                         					" +
"\n   , Level1Name                                                                                                                                                                                                                                                       					" +
"\n   , Criterio                                                                                                                                                                                                                                                         					" +
"\n   , CriterioName                                                                                                                                                                                                                                                     					" +
"\n   , ROUND(AV,2) AV                                                                                                                                                                                                                                                               		" +
"\n   , ROUND(CASE WHEN Level1Id = 25 THEN AV - NC ELSE NC END,2) NC /* VERIFICAÇÃO DA TIPIFICAÇÃO */                                                                                                                                                                             			" +
"\n   , ROUND(Pontos,2) Pontos                                                                                                                                                                                                                                                           	" +
"\n   , ROUND(CASE WHEN AV = 0 THEN 0 ELSE Pontos END,2) AS PontosIndicador                                                                                                                                                                                                                 " +
"\n   , ROUND(Meta,2) AS Meta                                                                                                                                                                                                                                                             	" +
"\n   , ROUND(CASE WHEN Level1Id = 25 THEN CASE WHEN AV = 0 THEN 0 ELSE (AV - NC) / AV * 100 END ELSE Real END,2) Real /* VERIFICAÇÃO DA TIPIFICAÇÃO */                                                                                                                            			" +
"\n   , ROUND(PontosAtingidos,2)  PontosAtingidos                                                                                                                                                                                                                                           " +
"\n   , ROUND(Scorecard,2) Scorecard                                                                                                                                                                                                                                                        " +
"\n   , TipoScore                                                                                                                                                                                                                                                        					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n  FROM                                                                                                                                                                                                                                                                					" +
"\n  (                                                                                                                                                                                                                                                                   					" +
"\n  SELECT                                                                                                                                                                                                                                                              					" +
"\n  *,                                                                                                                                                                                                                                                                  					" +
"\n  /* INICIO SCORECARD COMPLETO-------------------------------------*/                                                                                                                                                                                                 					" +
"\n  CASE                                                                                                                                                                                                                                                                					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n      WHEN                                                                                                                                                                                                                                                            					" +
"\n      /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            					" +
"\n      CASE                                                                                                                                                                                                                                                            					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n              CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE REAL / META END                                                                                                                                                                                                          		" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          ELSE                                                                                                                                                                                                                                                        					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n              CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                        " +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          END                                                                                                                                                                                                                                                         					" +
"\n      /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            					" +
"\n      > 1                                                                                                                                                                                                                                                             					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n      THEN 1                                                                                                                                                                                                                                                          					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n      ELSE                                                                                                                                                                                                                                                            					" +
"\n      /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            					" +
"\n      CASE                                                                                                                                                                                                                                                            					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n              CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE REAL / META END                                                                                                                                                                                                          		" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          ELSE                                                                                                                                                                                                                                                        					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n              CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                        " +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          END                                                                                                                                                                                                                                                         					" +
"\n      /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            					" +
"\n  END * 100                                                                                                                                                                                                                                                           					" +
"\n  /* FIM SCORECARD COMPLETO----------------------------------------*/                                                                                                                                                                                                 					" +
"\n  AS SCORECARD                                                                                                                                                                                                                                                        					" +
"\n  ,                                                                                                                                                                                                                                                                   					" +
"\n  /* INICIO SCORECARD COMPLETO-------------------------------------*/                                                                                                                                                                                                 					" +
"\n  CASE                                                                                                                                                                                                                                                                					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n      WHEN                                                                                                                                                                                                                                                            					" +
"\n      /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            					" +
"\n      CASE                                                                                                                                                                                                                                                            					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n              CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE REAL / META END                                                                                                                                                                                                          		" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          ELSE                                                                                                                                                                                                                                                        					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n              CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                        " +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          END                                                                                                                                                                                                                                                         					" +
"\n      /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            					" +
"\n      > 1                                                                                                                                                                                                                                                             					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n      THEN 1                                                                                                                                                                                                                                                          					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n      WHEN                                                                                                                                                                                                                                                            					" +
"\n      /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            					" +
"\n      CASE                                                                                                                                                                                                                                                            					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n              CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE REAL / META END                                                                                                                                                                                                          		" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          ELSE                                                                                                                                                                                                                                                        					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n              CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                        " +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          END                                                                                                                                                                                                                                                         					" +
"\n      /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            					" +
"\n      < 0.7                                                                                                                                                                                                                                                           					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n      THEN 0                                                                                                                                                                                                                                                          					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n      ELSE                                                                                                                                                                                                                                                            					" +
"\n      /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            					" +
"\n      CASE                                                                                                                                                                                                                                                            					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n              CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE REAL / META END                                                                                                                                                                                                          		" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          ELSE                                                                                                                                                                                                                                                        					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n              CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                        " +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          END                                                                                                                                                                                                                                                         					" +
"\n      /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            					" +
"\n  END                                                                                                                                                                                                                                                                 					" +
"\n  /* FIM SCORECARD COMPLETO----------------------------------------*/                                                                                                                                                                                                 					" +
"\n  * /* MULTIPLICAÇÃO */                                                                                                                                                                                                                                               					" +
"\n  PONTOS                                                                                                                                                                                                                                                              					" +
"\n  AS PONTOSATINGIDOS                                                                                                                                                                                                                                                  					" +
"\n  FROM                                                                                                                                                                                                                                                                					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n  (                                                                                                                                                                                                                                                                   					" +
"\n  SELECT                                                                                                                                                                                                                                                              					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n    ISNULL(CL.Id, @CLUSTER) AS Cluster                                                                                                                                                                                                                                					" +
"\n   , ISNULL(CL.Name, @CLUSTERNAME) AS ClusterName                                                                                                                                                                                                                     					" +
"\n   , ISNULL(S.Id, @REGIONAL) AS Regional                                                                                                                                                                                                                              					" +
"\n   , ISNULL(S.Name, @REGIONALNAME) AS RegionalName                                                                                                                                                                                                                    					" +
"\n   , ISNULL(CL1.UnitId, @PARCOMPANY) AS ParCompanyId                                                                                                                                                                                                                  					" +
"\n   , ISNULL(C.Name, @PARCOMPANYNAME) AS ParCompanyName                                                                                                                                                                                                                					" +
"\n   , L1.IsRuleConformity AS TipoIndicador                                                                                                                                                                                                                             					" +
"\n   , L1.Id AS Level1Id                                                                                                                                                                                                                                                					" +
"\n   , L1.Name AS Level1Name                                                                                                                                                                                                                                            					" +
"\n   , ISNULL(CRL.Id, @CRITERIO) AS Criterio                                                                                                                                                                                                                            					" +
"\n   , ISNULL(CRL.Name, @CRITERIONAME) AS CriterioName                                                                                                                                                                                                                  					" +
"\n   , ISNULL((select top 1 Points from ParLevel1XCluster aaa where aaa.ParLevel1_Id = L1.Id AND aaa.ParCluster_Id = CL.Id AND aaa.AddDate <  @DATAFINAL), @PONTOS) AS Pontos                                                                                                              " +
"\n                                                                                                                                                                                                                                                                      					" +
"\n   --ISNULL(CL.Id, @CLUSTER) AS Cluster                                                                                                                                                                                                                               					" +
"\n   --, (CL.Name)AS ClusterName                                                                                                                                                                                                                                        					" +
"\n   --, (S.Id)AS Regional                                                                                                                                                                                                                                              					" +
"\n   --, (S.Name)AS RegionalName                                                                                                                                                                                                                                        					" +
"\n   --, (CL1.UnitId)AS ParCompanyId                                                                                                                                                                                                                                    					" +
"\n   --, (C.Name)AS ParCompanyName                                                                                                                                                                                                                                      					" +
"\n   --, L1.IsRuleConformity AS TipoIndicador                                                                                                                                                                                                                           					" +
"\n   --, L1.Id AS Level1Id                                                                                                                                                                                                                                              					" +
"\n   --, L1.Name AS Level1Name                                                                                                                                                                                                                                          					" +
"\n   --, (CRL.Id)AS Criterio                                                                                                                                                                                                                                            					" +
"\n   --, (CRL.Name)AS CriterioName                                                                                                                                                                                                                                      					" +
"\n   --, (L1C.Points)AS Pontos                                                                                                                                                                                                                                          					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n   , ST.Name AS TipoScore                                                                                                                                                                                                                                             					" +
"\n   ,                                                                                                                                                                                                                                                                  					" +
"\n    /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                              					" +
"\n    CASE                                                                                                                                                                                                                                                              					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n      WHEN L1.Id = 25 THEN @AVFREQUENCIAVERIFICACAO                                                                                                                                                                                                                   					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n      WHEN L1.hashKey = 1 THEN @VOLUMEPCC - @NAPCC                                                                                                                                                                                                                    					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n      WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                                 					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n      WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                                					" +
"\n      WHEN CT.Id IN(4) THEN SUM(A4.AM)																																																													" +
"\n    END                                                                                                                                                                                                                                                               					" +
"\n    /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                              					" +
"\n    AS AV                                                                                                                                                                                                                                                             					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n   ,                                                                                                                                                                                                                                                                  					" +
"\n    /*INICIO NC COMPLETO----------------------------------------------*/                                                                                                                                                                                              					" +
"\n    CASE WHEN L1.IsRuleConformity = 1 THEN                                                                                                                                                                                                                            					" +
"\n        /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                          					" +
"\n        CASE                                                                                                                                                                                                                                                          					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN L1.Id = 25 THEN @AVFREQUENCIAVERIFICACAO                                                                                                                                                                                                               					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN L1.hashKey = 1 THEN @VOLUMEPCC - @NAPCC                                                                                                                                                                                                                					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                             					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                            					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN CT.Id IN(4) THEN SUM(A4.AM)                                                                                                                                                                                                                            					" +
"\n        END                                                                                                                                                                                                                                                           					" +
"\n          /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                        					" +
"\n          -                                                                                                                                                                                                                                                           					" +
"\n        /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                          					" +
"\n        CASE                                                                                                                                                                                                                                                          					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN L1.Id = 25 THEN @NCFREQUENCIAVERIFICACAO                                                                                                                                                                                                               					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiDefects)                                                                                                                                                                                                                					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                              					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN CT.Id IN(4) THEN SUM(A4.DEF_AM)                                                                                                                                                                                                                        					" +
"\n        END                                                                                                                                                                                                                                                           					" +
"\n        /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                          					" +
"\n     ELSE                                                                                                                                                                                                                                                             					" +
"\n        /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                          					" +
"\n        CASE                                                                                                                                                                                                                                                          					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN L1.Id = 25 THEN @NCFREQUENCIAVERIFICACAO                                                                                                                                                                                                               					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiDefects)                                                                                                                                                                                                                					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                              					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN CT.Id IN(4) THEN SUM(A4.DEF_AM)                                                                                                                                                                                                                        					" +
"\n        END                                                                                                                                                                                                                                                           					" +
"\n        /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                          					" +
"\n     END                                                                                                                                                                                                                                                              					" +
"\n     /*FIM NC COMPLETO-------------------------------------------------*/                                                                                                                                                                                             					" +
"\n     AS NC                                                                                                                                                                                                                                                            					" +
"\n   ,                                                                                                                                                                                                                                                                  					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n    CASE                                                                                                                                                                                                                                                              					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n      WHEN                                                                                                                                                                                                                                                            					" +
"\n        /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                          					" +
"\n        CASE                                                                                                                                                                                                                                                          					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN L1.Id = 25 THEN @AVFREQUENCIAVERIFICACAO                                                                                                                                                                                                               					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN L1.hashKey = 1 THEN @VOLUMEPCC - @NAPCC                                                                                                                                                                                                                					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                             					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                            					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN CT.Id IN(4) THEN SUM(A4.AM)                                                                                                                                                                                                                            					" +
"\n        END                                                                                                                                                                                                                                                           					" +
"\n        /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                          					" +
"\n        = 0                                                                                                                                                                                                                                                           					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n      THEN 0                                                                                                                                                                                                                                                          					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n       ELSE                                                                                                                                                                                                                                                           					" +
"\n        /*INICIO NC COMPLETO----------------------------------------------*/                                                                                                                                                                                          					" +
"\n        CASE WHEN L1.IsRuleConformity = 1 THEN                                                                                                                                                                                                                        					" +
"\n            /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                      					" +
"\n            CASE                                                                                                                                                                                                                                                      					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n              WHEN L1.Id = 25 THEN @AVFREQUENCIAVERIFICACAO                                                                                                                                                                                                           					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n              WHEN L1.hashKey = 1 THEN @VOLUMEPCC - @NAPCC                                                                                                                                                                                                            					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n              WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                         					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n              WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                        					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n              WHEN CT.Id IN(4) THEN SUM(A4.AM)                                                                                                                                                                                                                            				" +
"\n            END                                                                                                                                                                                                                                                       					" +
"\n              /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                    					" +
"\n              -  /* SUBTRAÇÃO */                                                                                                                                                                                                                                      					" +
"\n                 /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                 					" +
"\n            CASE                                                                                                                                                                                                                                                      					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n              WHEN L1.Id = 25 THEN @NCFREQUENCIAVERIFICACAO                                                                                                                                                                                                           					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n              WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiDefects)                                                                                                                                                                                                            					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n              WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                          					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n              WHEN CT.Id IN(4) THEN SUM(A4.DEF_AM)                                                                                                                                                                                                                            			" +
"\n            END                                                                                                                                                                                                                                                       					" +
"\n            /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                      					" +
"\n         ELSE                                                                                                                                                                                                                                                         					" +
"\n            /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                      					" +
"\n            CASE                                                                                                                                                                                                                                                      					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n              WHEN L1.Id = 25 THEN @NCFREQUENCIAVERIFICACAO                                                                                                                                                                                                           					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n              WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiDefects)                                                                                                                                                                                                            					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n              WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                          					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n              WHEN CT.Id IN(4) THEN SUM(A4.DEF_AM)                                                                                                                                                                                                                            			" +
"\n            END                                                                                                                                                                                                                                                       					" +
"\n            /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                      					" +
"\n         END                                                                                                                                                                                                                                                          					" +
"\n         /*FIM NC COMPLETO-------------------------------------------------*/                                                                                                                                                                                         					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n         / /*DIVISÃO*/                                                                                                                                                                                                                                                					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n        /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                          					" +
"\n        CASE                                                                                                                                                                                                                                                          					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN L1.Id = 25 THEN @AVFREQUENCIAVERIFICACAO                                                                                                                                                                                                               					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN L1.hashKey = 1 THEN @VOLUMEPCC - @NAPCC                                                                                                                                                                                                                					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                             					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                            					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n          WHEN CT.Id IN(4) THEN SUM(A4.AM)                                                                                                                                                                                                                            					" +
"\n        END                                                                                                                                                                                                                                                           					" +
"\n        /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                          					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n       END * 100                                                                                                                                                                                                                                                      					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n       AS REAL                                                                                                                                                                                                                                                        					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n   ,                                                                                                                                                                                                                                                                  					" +
"\n   CASE                                                                                                                                                                                                                                                               					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n      WHEN(SELECT COUNT(1) FROM ParGoal G WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) AND G.AddDate <= @DATAFINAL) > 0 THEN                                                                                                   					" +
"\n          (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) AND G.AddDate <= @DATAFINAL ORDER BY G.ParCompany_Id DESC, AddDate DESC)                                         					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n      ELSE                                                                                                                                                                                                                                                            					" +
"\n          (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) ORDER BY G.ParCompany_Id DESC, AddDate ASC)                                                                      					" +
"\n   END                                                                                                                                                                                                                                                                					" +
"\n   AS META                                                                                                                                                                                                                                                            					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n  FROM      ParLevel1 L1                                                                                                                                                                                                                                              					" +
"\n  LEFT JOIN ConsolidationLevel1 CL1                                                                                                                                                                                                                                   					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n         ON L1.Id = CL1.ParLevel1_Id                                                                                                                                                                                                                                  					" +
"\n  LEFT JOIN ParScoreType ST                                                                                                                                                                                                                                           					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n         ON ST.Id = L1.ParScoreType_Id                                                                                                                                                                                                                                					" +
"\n  LEFT JOIN ParCompany C                                                                                                                                                                                                                                              					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n         ON C.Id = CL1.UnitId                                                                                                                                                                                                                                         					" +
"\n  LEFT JOIN #AMOSTRATIPO4 A4                                                                                                                                                                                                                                          					" +
"\n          ON A4.UNIDADE = C.Id                                                                                                                                                                                                                                      						" +
"\n          AND A4.INDICADOR = L1.ID                                                                                                                                 																														" +
"\n  LEFT JOIN ParCompanyXStructure CS                                                                                                                                                                                                                                   					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n         ON CS.ParCompany_Id = C.Id                                                                                                                                                                                                                                   					" +
"\n  LEFT JOIN ParStructure S                                                                                                                                                                                                                                            					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n         ON S.Id = CS.ParStructure_Id                                                                                                                                                                                                                                 					" +
"\n  LEFT JOIN ParStructureGroup SG                                                                                                                                                                                                                                      					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n         ON SG.Id = S.ParStructureGroup_Id                                                                                                                                                                                                                            					" +
"\n  LEFT JOIN ParCompanyCluster CCL                                                                                                                                                                                                                                     					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n         ON CCL.ParCompany_Id = C.Id  AND CCL.Active = 1                                                                                                                                                                                                                                 " +
"\n  LEFT JOIN ParCluster CL                                                                                                                                                                                                                                             					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n         ON CL.Id = CCL.ParCluster_Id                                                                                                                                                                                                                                 					" +
"\n  LEFT JOIN ParConsolidationType CT                                                                                                                                                                                                                                   					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n         ON CT.Id = L1.ParConsolidationType_Id                                                                                                                                                                                                                        					" +
"\n  LEFT JOIN ParLevel1XCluster L1C                                                                                                                                                                                                                                     					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n         ON L1C.ParLevel1_Id = L1.Id AND L1C.ParCluster_Id = CL.Id  AND L1C.IsActive = 1                                                                                                                                                                                                 " +
"\n  LEFT JOIN ParCriticalLevel CRL                                                                                                                                                                                                                                      					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n         ON CRL.Id  = (select top 1 ParCriticalLevel_Id from ParLevel1XCluster aaa where aaa.ParLevel1_Id = L1.Id AND aaa.ParCluster_Id = CL.Id AND aaa.AddDate <  @DATAFINAL)                                                                                                           " +
"\n  WHERE(ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL OR L1.Id = 25)                                                                                                                                                                                          					" +
"\n    AND(C.Id = @ParCompany_Id OR(C.Id IS NULL AND L1.Id = 25))                                                                                                                                                                                                       					" +
"\n  GROUP BY                                                                                                                                                                                                                                                            					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n       CL.Id                                                                                                                                                                                                                                                          					" +
"\n      , CL.Name                                                                                                                                                                                                                                                       					" +
"\n      , S.Id                                                                                                                                                                                                                                                          					" +
"\n      , S.Name                                                                                                                                                                                                                                                        					" +
"\n      , CL1.UnitId                                                                                                                                                                                                                                                    					" +
"\n      , C.Name                                                                                                                                                                                                                                                        					" +
"\n      , L1.IsRuleConformity                                                                                                                                                                                                                                           					" +
"\n      , L1.Id                                                                                                                                                                                                                                                         					" +
"\n      , L1.Name                                                                                                                                                                                                                                                       					" +
"\n      , CRL.Id                                                                                                                                                                                                                                                        					" +
"\n      , CRL.Name                                                                                                                                                                                                                                                      					" +
"\n      , ST.Name                                                                                                                                                                                                                                                       					" +
"\n      , CT.Id                                                                                                                                                                                                                                                         					" +
"\n      , L1.HashKey                                                                                                                                                                                                                                                    					" +
"\n      , C.Id                                                                                                                                                                                                                                                          					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n  ) SCORECARD                                                                                                                                                                                                                                                         					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n  ) FIM                                                                                                                                                                                                                                                               					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n  UNION ALL                                                                                                                                                                                                                                                           					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n  /*SCORECARD VAZIO----------------------------------------*/                                                                                                                                                                                                         					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n  SELECT                                                                                                                                                                                                                                                              					" +
"\n  CL.Id AS Cluster                                                                                                                                                                                                                                                    					" +
"\n  , CL.Name AS ClusterName                                                                                                                                                                                                                                            					" +
"\n  , S.Id AS Regional                                                                                                                                                                                                                                                  					" +
"\n  , S.Name AS RegionalName                                                                                                                                                                                                                                            					" +
"\n  , C.Id AS ParCompanyId                                                                                                                                                                                                                                              					" +
"\n  , C.Name AS ParCompanyName                                                                                                                                                                                                                                          					" +
"\n  , CASE WHEN L1.IsRuleConformity = 0 THEN 1 ELSE 2 END AS TipoIndicador                                                                                                                                                                                              					" +
"\n  , CASE WHEN L1.IsRuleConformity = 0 THEN 'Menor' ELSE 'Maior' END AS TipoIndicadorName                                                                                                                                                                              					" +
"\n  , L1.Id AS Level1Id                                                                                                                                                                                                                                                 					" +
"\n  , L1.Name AS Level1Name                                                                                                                                                                                                                                             					" +
"\n  , CRL.Id AS Criterio                                                                                                                                                                                                                                                					" +
"\n  , CRL.Name AS CriterioName                                                                                                                                                                                                                                          					" +
"\n  , 0  AS AV                                                                                                                                                                                                                                                          					" +
"\n  , 0  AS NC                                                                                                                                                                                                                                                          					" +
"\n  , (select top 1 Points from ParLevel1XCluster aaa where aaa.ParLevel1_Id = L1.Id AND aaa.ParCluster_Id = CL.Id AND aaa.AddDate <  @DATAFINAL) AS Pontos                                                                                                                                " +
"\n  , 0 AS PontosIndicador                                                                                                                                                                                                                                              					" +
"\n  , ROUND(CASE                                                                                                                                                                                                                                                              				" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n      WHEN(SELECT COUNT(1) FROM ParGoal G WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) AND G.AddDate <= @DATAFINAL) > 0 THEN                                                                                                   					" +
"\n          (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) AND G.AddDate <= @DATAFINAL ORDER BY G.ParCompany_Id DESC, AddDate DESC)                                         					" +
"\n                                                                                                                                                                                                                                                                      					" +
"\n      ELSE                                                                                                                                                                                                                                                            					" +
"\n          (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) ORDER BY G.ParCompany_Id DESC, AddDate ASC)                                                                      					" +
"\n   END,2)                                                                                                                                                                                                                                                                				" +
"\n   AS META                                                                                                                                                                                                                                                            					" +
"\n  , 0 AS Real                                                                                                                                                                                                                                                         					" +
"\n  , 0  AS PontosAtingidos                                                                                                                                                                                                                                             					" +
"\n  , 0  AS Scorecard                                                                                                                                                                                                                                                   					" +
"\n  , ST.Name AS TipoScore                                                                                                                                                                                                                                              					" +
"\n  FROM ParLevel1 L1                                                                                                                                                                                                                                                   					" +
"\n  LEFT JOIN ParScoreType ST                                                                                                                                                                                                                                           					" +
"\n  ON ST.Id = L1.ParScoreType_Id                                                                                                                                                                                                                                       					" +
"\n  LEFT JOIN ParCompany C                                                                                                                                                                                                                                              					" +
"\n  ON C.Id = @ParCompany_Id                                                                                                                                                                                                                                            					" +
"\n  LEFT JOIN ParCompanyXStructure CS                                                                                                                                                                                                                                   					" +
"\n  ON CS.ParCompany_Id = C.Id                                                                                                                                                                                                                                          					" +
"\n  LEFT JOIN ParStructure S                                                                                                                                                                                                                                            					" +
"\n  ON S.Id = CS.ParStructure_Id                                                                                                                                                                                                                                        					" +
"\n  LEFT JOIN ParStructureGroup SG                                                                                                                                                                                                                                      					" +
"\n  ON SG.Id = S.ParStructureGroup_Id                                                                                                                                                                                                                                   					" +
"\n  LEFT JOIN ParCompanyCluster CCL                                                                                                                                                                                                                                     					" +
"\n  ON CCL.ParCompany_Id = C.Id AND CCL.Active = 1                                                                                                                                                                                                                                        	" +
"\n  LEFT JOIN ParCluster CL                                                                                                                                                                                                                                             					" +
"\n  ON CL.Id = CCL.ParCluster_Id                                                                                                                                                                                                                                        					" +
"\n  LEFT JOIN ParConsolidationType CT                                                                                                                                                                                                                                   					" +
"\n  ON CT.Id = L1.ParConsolidationType_Id                                                                                                                                                                                                                               					" +
"\n  INNER JOIN ParLevel1XCluster L1C                                                                                                                                                                                                                                     					" +
"\n  ON L1C.ParLevel1_Id = L1.Id AND L1C.ParCluster_Id = CL.Id  AND L1C.IsActive = 1                                                                                                                                                                                                        " +
"\n  INNER JOIN ParCriticalLevel CRL                                                                                                                                                                                                                                      					" +
"\n  ON CRL.Id = (select top 1 ParCriticalLevel_Id from ParLevel1XCluster aaa where aaa.ParLevel1_Id = L1.Id AND aaa.ParCluster_Id = CL.Id AND aaa.AddDate <  @DATAFINAL)                                                                                                                   " +
"\n  WHERE C.Id = @ParCompany_Id                                                                                                                                                                                                                                         					" +
"\n  AND L1.Id <> 25                                                                                                                                                                                                                                                     					" +
"\n  AND L1.AddDate <= @DATAFINAL                                                                                                                                                                                                                                        					" +
"\n  AND L1.IsActive <> 0                                                                                                                                                                                                                                                					" +
"\n  AND L1.Id NOT IN(SELECT CCC.ParLevel1_Id FROM ConsolidationLevel1 CCC WHERE CCC.UnitId = @ParCompany_Id                                                                                                                                                             					" +
"\n  AND CCC.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL)                                                                                                                                                                                                      					" +
"\n  AND CL.Id = @CLUSTER                                                                                                                                                                                                                                                                   " +
"\n   ) SC                                                                                                                                                                                                                                                               					" +
"\n   ORDER BY 11, 10                                                                                                                                                                                                                                                    					" +
"\n   DROP TABLE #AMOSTRATIPO4 																																																																" +
"\n 																																																																						" +
"\n   SET @I = @I + 1																																																																		" +
"\n 																																																																						" +
"\n END																																																																						" +

"\n  SELECT " +
"\n  RegionalName regName, " +
"\n  Regional regId, " +
"\n  case when sum(isnull(PontosIndicador, 0)) = 0 then 0 else sum(isnull(PontosAtingidos, 0)) / sum(isnull(PontosIndicador, 0)) * 100 end as scorecardJbs, " +
"\n  case when sum(isnull(PontosIndicador, 0)) = 0 then 0 else sum(isnull(PontosAtingidos, 0)) / sum(isnull(PontosIndicador, 0)) * 100 end as scorecardJbsReg " +
"\n  FROM ParStructure Reg " +
"\n  left join #SCORE S " +
"\n  on S.Regional = Reg.Id " +
"\n  where Regional is not null " +
"\n  and Reg.Active = 1 " +
"\n  GROUP BY RegionalName, Regional ";

            using (var db = new SgqDbDevEntities())
            {
                _list = db.Database.SqlQuery<VisaoGeralDaAreaResultSet>(query).ToList();
            }

            //return _list;

        }

        /// <summary>
        /// A query para o grafico2 da Visão geral da área SGQ deve retornar:
        /// 
        ///    Coluna            |   Tipagem
        ///                      |
        ///    companySigla      |   string
        ///    companyScorecard  |   decimal
        ///    scorecardJbs      |   decimal
        ///    scorecardJbsReg   |   decimal
        /// 
        /// Objeto a ser utilizado: VisaoGeralDaAreaResultSet
        /// </summary>
        private void CriaMockG2()
        {
            //_mock = new List<VisaoGeralDaAreaResultSet>();

            //_mock.Add(new VisaoGeralDaAreaResultSet()
            //{
            //    companySigla = "Com",
            //    companyScorecard = 90M,
            //    scorecardJbs = 80M,
            //    scorecardJbsReg = 66.2M
            //});

            //_mock.Add(new VisaoGeralDaAreaResultSet()
            //{
            //    companySigla = "Pan",
            //    companyScorecard = 70M,
            //    scorecardJbs = 80M,
            //    scorecardJbsReg = 66.2M
            //});

            _list = new List<VisaoGeralDaAreaResultSet>();

            var query = "" +

            "\n  SELECT " +
"\n  RegionalName companySigla, " +
"\n  case when sum(isnull(PontosIndicador, 0)) = 0 then 0 else sum(isnull(PontosAtingidos, 0)) / sum(isnull(PontosIndicador, 0)) * 100 end companyScorecard, " +
"\n  case when sum(isnull(PontosIndicador, 0)) = 0 then 0 else sum(isnull(PontosAtingidos, 0)) / sum(isnull(PontosIndicador, 0)) * 100 end as scorecardJbs, " +
"\n  case when sum(isnull(PontosIndicador, 0)) = 0 then 0 else sum(isnull(PontosAtingidos, 0)) / sum(isnull(PontosIndicador, 0)) * 100 end as scorecardJbsReg " +
"\n  FROM ParStructure Reg " +
"\n  left join #SCORE S " +
"\n  on S.Regional = Reg.Id " +
"\n  where Regional is not null " +
"\n  and Reg.Active = 1 " +
"\n  GROUP BY RegionalName, Regional ";

            using (var db = new SgqDbDevEntities())
            {
                _list = db.Database.SqlQuery<VisaoGeralDaAreaResultSet>(query).ToList();
            }



        }

        /// <summary>
        /// A query para o grafico3 da Visão geral da área SGQ deve retornar:
        /// 
        ///      Coluna         |   Tipagem
        ///                     |
        ///      nc             |   decimal
        ///      procentagemNc  |   decimal
        ///      date           |   datetime
        ///      
        /// Objeto a ser utilizado: VisaoGeralDaAreaResultSet
        /// </summary>
        /// <param name="form"></param>
        private void CriaMockG3(FormularioParaRelatorioViewModel form)
        {
            var primeiroDiaMesAnterior = Guard.PrimeiroDiaMesAnterior(form._dataInicio);
            var proximoDomingo = Guard.GetNextWeekday(form._dataFim, DayOfWeek.Sunday);

            _mock = new List<VisaoGeralDaAreaResultSet>();

            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                nc = 10M,
                procentagemNc = 90M,
                date = proximoDomingo.AddDays(-8)
            });
            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                nc = 50M,
                av = 50M,
                procentagemNc = 40M,
                date = proximoDomingo.AddDays(-9)
            });
            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                nc = 20M,
                av = 150M,
                procentagemNc = 50M,
                date = proximoDomingo.AddDays(-18)
            });
            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                nc = 90M,
                av = 200M,
                procentagemNc = 90M,
                date = proximoDomingo.AddDays(-15)
            });
            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                nc = 120M,
                av = 75M,
                procentagemNc = 20M,
                date = proximoDomingo.AddDays(-22)
            });

            for (DateTime i = primeiroDiaMesAnterior; i < proximoDomingo; i = i.AddDays(1))
            {
                if (_mock.FirstOrDefault(r => r.date == i) == null)
                {
                    _mock.Add(new VisaoGeralDaAreaResultSet()
                    {
                        nc = 0M,
                        av = 0M,
                        procentagemNc = 0M,
                        date = i
                    });

                    //_mock.Add(new VisaoGeralDaAreaResultSet());

                }
            }
            _mock = _mock.OrderBy(r => r.date).ToList();

        }

        /// <summary>
        /// A query para o grafico4 da Visão geral da área SGQ deve retornar:
        /// 
        ///      Coluna         |   Tipagem
        ///                     |
        ///      nc             |   decimal
        ///      procentagemNc  |   decimal
        ///      av             |   decimal
        ///      level1Name     |   string
        ///      level2Name     |   string
        ///      level1Id       |   int
        ///      level2Id       |   int
        ///      
        /// Objeto a ser utilizado: VisaoGeralDaAreaResultSet
        /// </summary>
        private void CriaMockG4()
        {
            _mock = new List<VisaoGeralDaAreaResultSet>();

            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                nc = 10M,
                procentagemNc = 50M,
                av = 20M,
                level1Name = "level1 Name1",
                level2Name = "level2 Name1",
                level1Id = 1,
                level2Id = 1,
            });

            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                nc = 50M,
                procentagemNc = 33.3M,
                av = 120M,
                level1Name = "level1 Name1",
                level2Name = "level2 Name2",
                level1Id = 1,
                level2Id = 2,
            });

        }

        /// <summary>
        /// A query para o grafico5 da Visão geral da área SGQ deve retornar:
        /// 
        ///      Coluna         |   Tipagem
        ///                     |
        ///      nc             |   decimal
        ///      procentagemNc  |   decimal
        ///      av             |   decimal
        ///      level1Name     |   string
        ///      level2Name     |   string
        ///      level3Name     |   string
        ///      level1Id       |   int
        ///      level2Id       |   int
        ///      level3Id       |   int
        /// 
        /// Objeto a ser utilizado: VisaoGeralDaAreaResultSet
        /// </summary>
        private void CriaMockG5()
        {
            _mock = new List<VisaoGeralDaAreaResultSet>();

            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                nc = 10M,
                procentagemNc = 50M,
                av = 20M,
                level1Name = "level1 Name1",
                level2Name = "level2 Name1",
                level3Name = "level3 Name1",
                level1Id = 1,
                level2Id = 1,
                level3Id = 1,
            });

            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                nc = 50M,
                procentagemNc = 33.3M,
                av = 120M,
                level1Name = "level1 Name1",
                level2Name = "level2 Name1",
                level3Name = "level3 Name2",
                level1Id = 1,
                level2Id = 1,
                level3Id = 2,
            });
            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                nc = 102M,
                procentagemNc = 90.3M,
                av = 120M,
                level1Name = "level1 Name1",
                level2Name = "level2 Name1",
                level3Name = "level3 Name3",
                level1Id = 1,
                level2Id = 1,
                level3Id = 3,
            });

        }
    }

}
