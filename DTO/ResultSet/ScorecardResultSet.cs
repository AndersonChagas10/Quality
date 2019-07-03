﻿using System;

public class ScorecardResultSet
{
    public int? Cluster { get; set; }
    public string ClusterName { get; set; }

    public int? Regional { get; set; }
    public string RegionalName { get; set; }

    public int? ParCompanyId { get; set; }
    public string ParCompanyName { get; set; }

    public int? TipoIndicador { get; set; }
    public string TipoIndicadorName { get; set; }

    public int? Level1Id { get; set; }
    public string Level1Name { get; set; }

    public int? Criterio { get; set; }
    public string CriterioName { get; set; }

    public decimal? AV { get; set; }
    public decimal? NC { get; set; }

    public decimal? Pontos { get; set; }
    public decimal? PontosIndicador { get; set; }
    public decimal? Meta { get; set; }
    public decimal? Real { get; set; }
    public decimal? PontosAtingidos { get; set; }
    public decimal? PontosAtingidosIndicador { get; set; }
    public decimal? Scorecard { get; set; }

    public string TipoScore { get; set; }

    public string getSQLScorecard(DateTime dtInicio, DateTime dtFim, int unidadeId, int tipo, int clusterSelected_Id, int GroupLevel1, int moduloId, int shift) //Se tipo 0, tras pontos , se 1, tras tudo
    {

        /*
            SCORECARD REFORMULADO 2017
            2017-03-14
            AUTOR: GABRIEL CRUZ NUNES

            ETAPAS DO SCORE CARD:

            1. BÁSICO:

            AV:		SE INDICADOR = PCC1B			ENTÃO	VOLUMEPCC - N/A PCC
            SE TIPODECONSOLIDAÇÃO IN (1,2)	ENTÃO	SUM(WeiEvaluation)
            SE TIPODECONSOLIDAÇÃO IN (3)	ENTÃO	SUM(EvaluatedResult)

            FREQUENCIA DA TIPIFICACAO: AV É SE TEVE ABATE
                           NC É SE TEVE VERIFICACAO

            NC:		SE TIPODECONSOLIDAÇÃO IN (1,2)	ENTÃO	SUM(WeiDefects)
            SE TIPODECONSOLIDAÇÃO IN (3)	ENTÃO	SUM(WeiEvaluation)

            META:	SE EXITIR UMA META NO PERÍODOS SELECIONADO, ENTÃO:
            SELECT TOP 1 ISNULL(G.PercentValue,0) FROM ParGoal G WHERE G.ParLevel1_id = L1.id AND (G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) AND G.AddDate <= @MAXDATE ORDER BY G.ParCompany DESC, AddDate DESC
            SE NÃO, PEGUE A MENOR META DO INDICAODR DESTA EMPRESA OU DO CORPORATIVO
            -----------

            2. ACERTOS DE REGRAS

            NC:		SE TIPOINDICADOR = 'MAIOR'		ENTÃO	AV - NC

            -----------

            3. CALCULO DO REALIZADO

            REAL:	SE AV = 0	ENTÃO 0
            SE NC = 0	ENTÃO 100
            SENÃO		NC / AV

            -----------

            4. CALCULO DO SCORECARD

            SCORECARD:	SE TIPOINDICADOR = 'MAIOR'	ENTÃO	META / REAL
            SENÃO						REAL / META

            -----------

            5. ACERTO NO SCORECARD

            SCORECARD:	SE SCORECARD > 100	ENTÃO 100

            -----------

            6. CÁLCULO DOS PONTOS ATINGIDOS

            PONTOSATINGIDOS:	SCORECARD * PONTOS

            -----------

            7. SOMA TOTAL

            PONTOSDISPUTADOS:	SE	AV > 0	ENTÃO SUM(PONTOS)

            PONTOSATINGIDOS:	SUM(PONTOSATINGIDOS)		

            SCORECARD_MENSAL: PONTOSATINGIDOS / PONTOSDISPUTADOS

            */

        string listaUnidades = "";

        switch (GroupLevel1)
        {
            case 1:
                listaUnidades = "1,2,3,4,5";
                break;
            case 2:
                listaUnidades = "6,7,8,9,10";
                break;
            case 3:
                listaUnidades = "11,12,13,14,15";
                break;
        }

        string listaUnidades2 = " where Level1Id in (" + listaUnidades + ") ";
        string listaUnidades3 = " and L1.Id in (" + listaUnidades + ") ";

        if (listaUnidades == "")
        {
            listaUnidades2 = "";
            listaUnidades3 = "";
        }

        string selectTipo = "SELECT * FROM ";
        string orderby = "ORDER BY 11, 10";

        string where = " WHERE 1=1 ";

        string Wshift = "";

        if (clusterSelected_Id > 0)
        {
            where += "\n AND cluster = " + clusterSelected_Id;
        }

        if (shift > 0)
        {
            Wshift += $@" AND CL1.Shift = {shift} ";
        }

        where += "\n AND Meta Is Not Null";


        if (tipo == 0)
        {
            selectTipo = "SELECT SUM(PontosIndicador) as PontosAtingidosIndicador, SUM(PontosAtingidos) as PontosAtingidos FROM ";
            orderby = "";
        }

        string sql = "";

        sql = "" +

           "\n DECLARE @ParCompany_Id INT = " + unidadeId + "                                                                                                                                                                                                                                     " +
           "\n DECLARE @DATAINICIAL DATETIME = '" + dtInicio.ToString("yyyyMMdd") + " 00:00'                                                                                                                                                                                                                    " +
           "\n DECLARE @DATAFINAL   DATETIME = '" + dtFim.ToString("yyyyMMdd") + "  23:59:59'                                                                                                                                                                                                                    " +
           "\n DECLARE @ParModule_Id INT = " + moduloId + // + unidadeId + "  
           "\n DECLARE @Shift_Id INT = " + shift + // + Turno + "  

        // Alteração
        "\n CREATE TABLE #AMOSTRATIPO4 ( " +

                "\n UNIDADE INT NULL, " +
                "\n INDICADOR INT NULL, " +
                "\n AM INT NULL, " +
                "\n DEF_AM INT NULL " +
                "\n ) " +


                "\n INSERT INTO #AMOSTRATIPO4 " +
                /*
                "\n SELECT " +
                "\n  UNIDADE, INDICADOR, " +
                "\n FROM " +
                "\n ( " +
                */
                "\n     SELECT " +
                //"\n     cast(C2.CollectionDate as DATE) AS DATA " +
                "\n     C.Id AS UNIDADE " +
                "\n     , C2.ParLevel1_Id AS INDICADOR " +
                "\n , COUNT(DISTINCT CONCAT(c2.Period, '-', c2.shift, '-', C2.EvaluationNumber, '-', C2.Sample, '-', cast(cast(C2.CollectionDate as date) as varchar))) AM  " +
                "\n , SUM(IIF(C2.WeiDefects = 0, 0, 1)) DEF_AM " +
                //"\n     , C2.EvaluationNumber AS AV " +
                // "\n     , C2.Sample AS AM " +
                //"\n     , case when SUM(C2.WeiDefects) = 0 then 0 else 1 end DEF_AM " +
                "\n     FROM CollectionLevel2 C2 (nolock) " +
                "\n     INNER JOIN ParLevel1 L1 (nolock)  " +
                "\n     ON L1.Id = C2.ParLevel1_Id AND ISNULL(L1.ShowScorecard, 1) = 1" +

                "\n     INNER JOIN ParCompany C (nolock)  " +
                "\n     ON C.Id = C2.UnitId " +
                "\n     INNER JOIN ParLevel1XModule P1M with (nolock) on p1m.parlevel1_id = L1.id  and p1m.parmodule_id = @ParModule_Id and p1m.isActive = 1 and p1m.EffectiveDateStart <= @DATAINICIAL and (P1M.ParCluster_Id is null OR P1M.ParCluster_Id in (select ParCluster_Id from ParCompanyCluster where ParCompany_Id = C.Id and Active = 1))" +
                "\n     where C2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL " +
                "\n     AND C2.UnitId = @ParCompany_Id and C2.NotEvaluatedIs = 0 " +
                "\n     and C2.Duplicated = 0 " +
                "\n     and L1.ParConsolidationType_Id = 4 " +
                "\n     group by C.Id, C2.ParLevel1_Id" +
           /*
           "\n ) TAB " +
           "\n GROUP BY UNIDADE, INDICADOR " +
           */



           "\n                                                                                                                                                                                                                                                                     " +
           "\n DECLARE @VOLUMEPCC INT                                                                                                                                                                                                                                              " +
           "\n DECLARE @DIASABATE INT                                                                                                                                                                                                                                              " +
           "\n DECLARE @DIASDEVERIFICACAO INT                                                                                                                                                                                                                                      " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n DECLARE @AVFREQUENCIAVERIFICACAO INT                                                                                                                                                                                                                                " +
           "\n DECLARE @NCFREQUENCIAVERIFICACAO INT                                                                                                                                                                                                                                " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n /* INICIO DADOS DA FREQUENCIA ------------------------------------------------------*/                                                                                                                                                                              " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n DECLARE @CLUSTER INT                                                                                                                                                                                                                                                " +
           "\n DECLARE @CLUSTERNAME VARCHAR(153)                                                                                                                                                                                                                                   " +
           "\n DECLARE @REGIONAL INT                                                                                                                                                                                                                                               " +
           "\n DECLARE @REGIONALNAME VARCHAR(153)                                                                                                                                                                                                                                  " +
           "\n DECLARE @PARCOMPANY INT                                                                                                                                                                                                                                             " +
           "\n DECLARE @PARCOMPANYNAME VARCHAR(153)                                                                                                                                                                                                                                " +
           "\n DECLARE @CRITERIO INT                                                                                                                                                                                                                                               " +
           "\n DECLARE @CRITERIONAME VARCHAR(153)                                                                                                                                                                                                                                  " +
           "\n DECLARE @PONTOS VARCHAR(153)                                                                                                                                                                                                                                        " +
           "\n                                                                                                                                                                                                                                                                     " +

           @"SELECT                                                                                                                                                                                                                                                              
              @CLUSTER = CL.Id                                                                                                                                                                                                                                                   
             , @CLUSTERNAME = 
             (
             SELECT TOP 1 (select name from parcluster where id = L1Ca.ParCluster_Id) FROM ParLevel1XCluster L1Ca WITH(NOLOCK) 
                     WHERE @CLUSTER = L1Ca.ParCluster_ID 
                         AND 25 = L1Ca.ParLevel1_Id 
                         AND L1Ca.IsActive = 1 
                         AND L1Ca.EffectiveDate <= @DATAFINAL 
                     ORDER BY L1Ca.EffectiveDate  desc
             )                                                                                                                                                                                                                                           
             , @REGIONAL = S.Id                                                                                                                                                                                                                                                  
             , @REGIONALNAME = S.Name                                                                                                                                                                                                                                            
             , @PARCOMPANY = C.Id                                                                                                                                                                                                                                                
             , @PARCOMPANYNAME = C.Name                                                                                                                                                                                                                                          
             , @CRITERIO = 
             (
             SELECT TOP 1 L1Ca.ParCriticalLevel_Id FROM ParLevel1XCluster L1Ca WITH(NOLOCK) 
                     WHERE @CLUSTER = L1Ca.ParCluster_ID 
                         AND 25 = L1Ca.ParLevel1_Id 
                         AND L1Ca.IsActive = 1 
                         AND L1Ca.EffectiveDate <= @DATAFINAL 
                     ORDER BY L1Ca.EffectiveDate  desc
             )                                                                                                                                                                                                                               
             , @CRITERIONAME = 
             (
             SELECT top 1 (select name from parcriticallevel where id = L1Ca.ParCriticalLevel_Id) FROM ParLevel1XCluster L1Ca WITH(NOLOCK) 
                     WHERE @CLUSTER = L1Ca.ParCluster_ID 
                         AND 25 = L1Ca.ParLevel1_Id 
                         AND L1Ca.IsActive = 1 
                         AND L1Ca.EffectiveDate <= @DATAFINAL 
                     ORDER BY L1Ca.EffectiveDate  desc
             )                                                                                                                                                                                                                                        
             , @PONTOS = 
             (
             SELECT TOP 1 L1Ca.Points FROM ParLevel1XCluster L1Ca WITH(NOLOCK) 
                     WHERE @CLUSTER = L1Ca.ParCluster_ID 
                         AND 25 = L1Ca.ParLevel1_Id 
                         AND L1Ca.IsActive = 1 
                         AND L1Ca.EffectiveDate <= @DATAFINAL 
                     ORDER BY L1Ca.EffectiveDate  desc
             )                                                                                                                                                                                                                                              
             FROM ParCompany C  (nolock) " +





           //"\n SELECT                                                                                                                                                                                                                                                              " +
           //"\n  @CLUSTER = CL.Id                                                                                                                                                                                                                                                   " +
           //"\n , @CLUSTERNAME = CL.Name                                                                                                                                                                                                                                            " +
           //"\n , @REGIONAL = S.Id                                                                                                                                                                                                                                                  " +
           //"\n , @REGIONALNAME = S.Name                                                                                                                                                                                                                                            " +
           //"\n , @PARCOMPANY = C.Id                                                                                                                                                                                                                                                " +
           //"\n , @PARCOMPANYNAME = C.Name                                                                                                                                                                                                                                          " +
           //"\n , @CRITERIO = L1C.ParCriticalLevel_Id                                                                                                                                                                                                                               " +
           //"\n , @CRITERIONAME = CRL.Name                                                                                                                                                                                                                                          " +
           //"\n , @PONTOS = L1C.Points                                                                                                                                                                                                                                              " +
           //"\n FROM ParCompany C  (nolock)                                                                                                                                                                                                                                                   " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n INNER JOIN ParCompanyXStructure CS  (nolock)                                                                                                                                                                                                                                   " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON CS.ParCompany_Id = C.Id AND CS.Active = 1                                                                                                                                                                                                                                  " +
           "\n LEFT JOIN ParStructure S   (nolock)                                                                                                                                                                                                                                           " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON S.Id = CS.ParStructure_Id                                                                                                                                                                                                                                 " +
           "\n LEFT JOIN ParStructureGroup SG  (nolock)                                                                                                                                                                                                                                      " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON SG.Id = S.ParStructureGroup_Id                                                                                                                                                                                                                            " +
           "\n LEFT JOIN ParCompanyCluster CCL   (nolock)                                                                                                                                                                                                                                    " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON CCL.ParCompany_Id = C.Id  AND CCL.Active = 1                                                                                                                                                                                                                                 " +
           "\n LEFT JOIN ParCluster CL   (nolock)                                                                                                                                                                                                                                            " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON CL.Id = CCL.ParCluster_Id                                                                                                                                                                                                                                 " +
           "\n LEFT JOIN ParLevel1XCluster L1C  (nolock)                                                                                                                                                                                                                                     " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON L1C.ParLevel1_Id = 25 AND L1C.ParCluster_Id = Cl.Id   AND L1C.IsActive = 1                                                                                                                                                                                                    " +
           "\n LEFT JOIN ParCriticalLevel CRL   (nolock)                                                                                                                                                                                                                                     " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON L1C.ParCriticalLevel_Id = CRL.Id                                                                                                                                                                                                                          " +
           "\n WHERE C.Id = @ParCompany_Id                                                                                                                                                                                                                                         " +
           "\n --AND L1C.ParLevel1_Id = 25                                                                                                                                                                                                                                         " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n /* FIM DOS DADOS DA FREQUENCIA -----------------------------------------------------*/                                                                                                                                                                              " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n SELECT TOP 1 @DIASABATE = COUNT(1), @VOLUMEPCC = SUM(Quartos) FROM VolumePcc1b  (nolock) WHERE ParCompany_id = @ParCompany_id AND Data BETWEEN @DATAINICIAL AND @DATAFINAL                                                                                                    " +
            $@"
            --Pega Volume do Perido
            SELECT Data,ParCompany_id,Shift_Id,Quartos
            INTO #VOLUME
            FROM VolumePcc1b WITH(nolock)
            WHERE 1 = 1
            AND ParCompany_id = @ParCompany_Id
            AND Data BETWEEN @DATAINICIAL AND @DATAFINAL

            --Pega pcc1B Coletado e verifica quais turnos foram coletados no periodo
            SELECT

                [Shift]

                INTO #TurnoPcc1B
	            FROM ConsolidationLevel1 WITH(NOLOCK)
            WHERE 1 = 1
            AND ParLevel1_Id = 3
            AND UnitId = @ParCompany_Id
            AND ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL


            DECLARE @SHIFT_1 INT = (SELECT top 1 [Shift] FROM #TurnoPcc1B WHERE [Shift] = 1)
            DECLARE @SHIFT_2 INT = (SELECT top 1 [Shift] FROM #TurnoPcc1B WHERE [Shift] = 2)

	            IF @Shift_Id = 0
                -- FILTREI TODOS

                    BEGIN
                    IF @SHIFT_1 is not null and @SHIFT_2 is not null

                    BEGIN
                    -- COLETEI TURNO 1 E PARA O TURNO 2

                            SELECT TOP 1

                                    @DIASABATE = COUNT(DISTINCT DATA), 
						            @VOLUMEPCC = SUM(Quartos)

                             FROM #VOLUME WITH (nolock) 
				            WHERE 1 = 1

                            AND ParCompany_id = @ParCompany_id

                            AND Data BETWEEN @DATAINICIAL AND @DATAFINAL

                            AND Shift_Id IS NULL


                            IF @DIASABATE = 0 OR @DIASABATE IS NULL

                            --QUANDO NÃO TEM VOLUME CADASTRADO PARA 'TODOS'

                            BEGIN
                                SELECT TOP 1

                                        @DIASABATE = COUNT(DISTINCT DATA), 
							            @VOLUMEPCC = SUM(Quartos)

                                    FROM #VOLUME WITH (nolock) 
					            WHERE 1 = 1

                                AND ParCompany_id = @ParCompany_id

                                AND Data BETWEEN @DATAINICIAL AND @DATAFINAL

                                AND Shift_Id IN(1, 2)

                            END

                        END


                    --COLETA TURNO 1

                        IF @SHIFT_1 is not null and @SHIFT_2 is null

                        BEGIN

                            SELECT TOP 1

                                    @DIASABATE = COUNT(DISTINCT DATA), 
						            @VOLUMEPCC = SUM(Quartos)

                             FROM #VOLUME WITH (nolock) 
				            WHERE 1 = 1

                            AND ParCompany_id = @ParCompany_id

                            AND Data BETWEEN @DATAINICIAL AND @DATAFINAL

                            AND Shift_Id = 1


                        END
                    -- COLETA TURNO 2

                        IF @SHIFT_1 is null and @SHIFT_2 is not null

                        BEGIN

                            SELECT TOP 1

                                    @DIASABATE = COUNT(DISTINCT DATA), 
						            @VOLUMEPCC = SUM(Quartos)

                             FROM #VOLUME WITH (nolock) 
				            WHERE 1 = 1

                            AND ParCompany_id = @ParCompany_id

                            AND Data BETWEEN @DATAINICIAL AND @DATAFINAL

                            AND Shift_Id = 2

                        END

                    END


                IF @Shift_Id = 1

                    BEGIN
                    -- FILTREI TURNO 1

                            SELECT TOP 1

                                    @DIASABATE = COUNT(DISTINCT DATA), 
						            @VOLUMEPCC = SUM(Quartos)

                             FROM #VOLUME WITH (nolock) 
				            WHERE 1 = 1

                            AND ParCompany_id = @ParCompany_id

                            AND Data BETWEEN @DATAINICIAL AND @DATAFINAL

                            AND Shift_Id = 1


                            IF @DIASABATE = 0 OR @DIASABATE IS NULL

                            --QUANDO NÃO TEM VOLUME CADASTRADO PARA TURNO 1

                            BEGIN
                                SELECT TOP 1

                                        @DIASABATE = COUNT(DISTINCT DATA), 
							            @VOLUMEPCC = SUM(Quartos)

                                    FROM #VOLUME WITH (nolock) 
					            WHERE 1 = 1

                                AND ParCompany_id = @ParCompany_id

                                AND Data BETWEEN @DATAINICIAL AND @DATAFINAL

                                AND Shift_Id IS NULL

                            END
                    END

                IF @Shift_Id = 2

                    BEGIN
                    -- FILTREI TURNO 2

                            SELECT TOP 1

                                    @DIASABATE = COUNT(DISTINCT DATA), 
						            @VOLUMEPCC = SUM(Quartos)

                             FROM #VOLUME WITH (nolock) 
				            WHERE 1 = 1

                            AND ParCompany_id = @ParCompany_id

                            AND Data BETWEEN @DATAINICIAL AND @DATAFINAL

                            AND Shift_Id = 2


                            IF @DIASABATE = 0 OR @DIASABATE IS NULL

                            BEGIN
                            -- QUANDO NÃO TEM VOLUME CADASTRADO PARA TURNO 1

                                SELECT TOP 1

                                        @DIASABATE = COUNT(DISTINCT DATA), 
							            @VOLUMEPCC = SUM(Quartos)

                                    FROM #VOLUME WITH (nolock) 
					            WHERE 1 = 1

                                AND ParCompany_id = @ParCompany_id

                                AND Data BETWEEN @DATAINICIAL AND @DATAFINAL

                                AND Shift_Id IS NULL

                            END
                    END
            " +

           "\n SELECT @DIASDEVERIFICACAO = COUNT(1) FROM(SELECT CONVERT(DATE, ConsolidationDate) DATA FROM ConsolidationLevel1 CL1 (nolock)  WHERE ParLevel1_Id = 24 AND CONVERT(DATE, ConsolidationDate) BETWEEN @DATAINICIAL AND @DATAFINAL AND CL1.UnitId = @ParCompany_Id GROUP BY CONVERT(DATE, ConsolidationDate)) VT  " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n SET @AVFREQUENCIAVERIFICACAO = @DIASABATE                                                                                                                                                                                                                           " +
           "\n SET @NCFREQUENCIAVERIFICACAO = @DIASDEVERIFICACAO                                                                                                                                                                                                                   " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n SET @NCFREQUENCIAVERIFICACAO = CASE WHEN @NCFREQUENCIAVERIFICACAO > @AVFREQUENCIAVERIFICACAO THEN @AVFREQUENCIAVERIFICACAO ELSE @NCFREQUENCIAVERIFICACAO END                                                                                                        " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n --SELECT @AVFREQUENCIAVERIFICACAO, @NCFREQUENCIAVERIFICACAO                                                                                                                                                                                                         " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n DECLARE @NAPCC INT                                                                                                                                                                                                                                                  " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n SELECT                                                                                                                                                                                                                                                              " +
           "\n        @NAPCC =                                                                                                                                                                                                                                                    " +
           "\n          COUNT(1)                                                                                                                                                                                                                                                 " +
           "\n          FROM                                                                                                                                                                                                                                                     " +
           "\n     (                                                                                                                                                                                                                                                             " +
           "\n              SELECT                                                                                                                                                                                                                                               " +
           "\n              COUNT(1) AS NA                                                                                                                                                                                                                                       " +
           "\n              FROM CollectionLevel2 C2   (nolock)                                                                                                                                                                                                                            " +
           "\n              LEFT JOIN Result_Level3 C3   (nolock)                                                                                                                                                                                                                          " +
           "\n              ON C3.CollectionLevel2_Id = C2.Id                                                                                                                                                                                                                    " +
           "\n              WHERE convert(date, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL                                                                                                                                                                           " +
           "\n              AND C2.ParLevel1_Id = (SELECT top 1 p1.id FROM Parlevel1 p1 with (nolock) INNER JOIN ParLevel1XModule P1M with (nolock) on p1m.parlevel1_id = p1.id  WHERE Hashkey = 1 AND ISNULL(ShowScorecard, 1) = 1 and p1m.parmodule_id = @ParModule_Id  and p1m.IsActive = 1 and p1m.EffectiveDateStart <= @DATAINICIAL and (P1M.ParCluster_Id is null OR P1M.ParCluster_Id in (select ParCluster_Id from ParCompanyCluster where ParCompany_Id = @ParCompany_Id and Active = 1)))                                                                                                                                                                              " +
           "\n              AND C2.UnitId = @ParCompany_Id                                                                                                                                                                                                                       " +
           "\n              AND IsNotEvaluate = 1                                                                                                                                                                                                                                " +
           "\n              GROUP BY C2.ID                                                                                                                                                                                                                                       " +
           "\n          ) NA                                                                                                                                                                                                                                                     " +
           "\n          WHERE NA = 2                                                                                                                                                                                                                                             " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n " + selectTipo + "                                                                                                                                                                                                                                           " +
           "\n (                                                                                                                                                                                                                                                                   " +
           "\n SELECT                                                                                                                                                                                                                                                              " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n   Cluster                                                                                                                                                                                                                                                           " +
           "\n  , ClusterName                                                                                                                                                                                                                                                      " +
           "\n  , Regional                                                                                                                                                                                                                                                         " +
           "\n  , RegionalName                                                                                                                                                                                                                                                     " +
           "\n  , ParCompanyId                                                                                                                                                                                                                                                     " +
           "\n  , ParCompanyName                                                                                                                                                                                                                                                   " +
           "\n  , CASE WHEN TipoIndicador = 0 THEN 1 ELSE 2 END TipoIndicador                                                                                                                                                                                                      " +
           "\n  , CASE WHEN TipoIndicador = 0 THEN 'MENOR' ELSE 'MAIOR' END TipoIndicadorName                                                                                                                                                                                      " +
           "\n  , Level1Id                                                                                                                                                                                                                                                         " +
           "\n  , Level1Name                                                                                                                                                                                                                                                       " +
           "\n  , Criterio                                                                                                                                                                                                                                                         " +
           "\n  , CriterioName                                                                                                                                                                                                                                                     " +
           "\n  , ROUND(AV,2) AV                                                                                                                                                                                                                                                               " +
           "\n  , ROUND(CASE WHEN Level1Id = 25 THEN AV - NC ELSE NC END,2) NC /* VERIFICAÇÃO DA TIPIFICAÇÃO */                                                                                                                                                                             " +
           "\n  , ROUND(Pontos,2) Pontos                                                                                                                                                                                                                                                           " +
           "\n  , ROUND(CASE WHEN AV = 0 THEN 0 ELSE Pontos END,2) AS PontosIndicador                                                                                                                                                                                                                                        " +
           "\n  , ROUND(Meta,2) AS Meta                                                                                                                                                                                                                                                             " +
           "\n  , ROUND(CASE WHEN Level1Id = 25 THEN CASE WHEN AV = 0 THEN 0 ELSE (AV - NC) / AV * 100 END WHEN Level1Id = 43 THEN case when NC = 0 then 0 when (Meta / NC) > 1 then 1 else Meta / NC end * 100 ELSE Real END,2) Real /* VERIFICAÇÃO DA TIPIFICAÇÃO */                                                                                                                            " +
           "\n  , ISNULL(ROUND(CASE WHEN Level1Id = 43 AND AV > 0 AND NC = 0 THEN Pontos ELSE PontosAtingidos END,2),0)  PontosAtingidos                                                                                                                                                                                                                                               " +
           "\n  , ISNULL(ROUND(CASE WHEN Level1Id = 43 AND AV > 0 AND NC = 0 THEN 100 ELSE Scorecard END,2),0)  Scorecard                                                                                                                                                                                                                                                        " +
           "\n  , TipoScore                                                                                                                                                                                                                                                        " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n FROM                                                                                                                                                                                                                                                                " +
           "\n (                                                                                                                                                                                                                                                                   " +
           "\n SELECT                                                                                                                                                                                                                                                              " +
           "\n *,                                                                                                                                                                                                                                                                  " +
           "\n /* INICIO SCORECARD COMPLETO-------------------------------------*/                                                                                                                                                                                                 " +
           "\n CASE                                                                                                                                                                                                                                                                " +
           "\n     WHEN Level1Id = 43 THEN case when NC = 0 then 0 when (Meta / NC) > 1 then 1 else Meta / NC end                                                                                                                                                                                                                                                                " +
           "\n     WHEN                                                                                                                                                                                                                                                            " +
           "\n     /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            " +
           "\n     CASE                                                                                                                                                                                                                                                            " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             CASE WHEN NC = 0 AND AV > 0 THEN (CASE WHEN Level1Id = 25 then 100 else 0 end) ELSE (CASE WHEN Level1Id = 25 then (100 - real) else real end) / meta END                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         ELSE                                                                                                                                                                                                                                                        " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         END                                                                                                                                                                                                                                                         " +
           "\n     /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            " +
           "\n     > 1                                                                                                                                                                                                                                                             " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     THEN 1                                                                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     ELSE                                                                                                                                                                                                                                                            " +
           "\n     /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            " +
           "\n     CASE                                                                                                                                                                                                                                                            " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             CASE WHEN NC = 0 AND AV > 0 THEN (CASE WHEN Level1Id = 25 then 100 else 0 end) ELSE (CASE WHEN Level1Id = 25 then (100 - real) else real end) / meta END                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         ELSE                                                                                                                                                                                                                                                        " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         END                                                                                                                                                                                                                                                         " +
           "\n     /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            " +
           "\n END * 100                                                                                                                                                                                                                                                           " +
           "\n /* FIM SCORECARD COMPLETO----------------------------------------*/                                                                                                                                                                                                 " +
           "\n AS SCORECARD                                                                                                                                                                                                                                                        " +
           "\n ,                                                                                                                                                                                                                                                                   " +
           "\n /* INICIO SCORECARD COMPLETO-------------------------------------*/                                                                                                                                                                                                 " +
           "\n CASE WHEN AV = 0 THEN 0 ELSE CASE                                                                                                                                                                                                                                                                " +
           "\n     WHEN Level1Id = 43 THEN case when NC = 0 then 0 when (Meta / NC) > 1 then 1 when (Meta / NC) < 0.7 then 0 else Meta / NC end                                                                                                                                                                                                                                                                 " +
           "\n     WHEN                                                                                                                                                                                                                                                            " +
           "\n     /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            " +
           "\n     CASE                                                                                                                                                                                                                                                            " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             CASE WHEN NC = 0 AND AV > 0 THEN (CASE WHEN Level1Id = 25 then 100 else 0 end) ELSE (CASE WHEN Level1Id = 25 then (100 - real) else real end) / meta END                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         ELSE                                                                                                                                                                                                                                                        " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         END                                                                                                                                                                                                                                                         " +
           "\n     /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            " +
           "\n     > 1                                                                                                                                                                                                                                                             " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     THEN 1                                                                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     WHEN                                                                                                                                                                                                                                                            " +
           "\n     /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            " +
           "\n     CASE                                                                                                                                                                                                                                                            " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             CASE WHEN NC = 0 AND AV > 0 THEN (CASE WHEN Level1Id = 25 then 100 else 0 end) ELSE (CASE WHEN Level1Id = 25 then (100 - real) else real end) / meta END                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         ELSE                                                                                                                                                                                                                                                        " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         END                                                                                                                                                                                                                                                         " +
           "\n     /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            " +
           "\n     < 0.7                                                                                                                                                                                                                                                           " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     THEN 0                                                                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     ELSE                                                                                                                                                                                                                                                            " +
           "\n     /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            " +
           "\n     CASE                                                                                                                                                                                                                                                            " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             CASE WHEN NC = 0 AND AV > 0 THEN (CASE WHEN Level1Id = 25 then 100 else 0 end) ELSE (CASE WHEN Level1Id = 25 then (100 - real) else real end) / meta END                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         ELSE                                                                                                                                                                                                                                                        " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         END                                                                                                                                                                                                                                                         " +
           "\n     /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            " +
           "\n END                                                                                                                                                                                                                                                                 " +
           "\n /* FIM SCORECARD COMPLETO----------------------------------------*/                                                                                                                                                                                                 " +
           "\n * /* MULTIPLICAÇÃO */                                                                                                                                                                                                                                               " +
           "\n PONTOS                                                                                                                                                                                                                                                              " +
           "\n END AS PONTOSATINGIDOS                                                                                                                                                                                                                                                  " +
           "\n FROM                                                                                                                                                                                                                                                                " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n (                                                                                                                                                                                                                                                                   " +
           "\n SELECT                                                                                                                                                                                                                                                              " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n   ISNULL(CL.Id, @CLUSTER) AS Cluster                                                                                                                                                                                                                                " +
           //"\n  , ISNULL(CL.Name, @CLUSTERNAME) AS ClusterName                                                                                                                                                                                                                     " +

           "\n , ISNULL((                                                                                                           " +
           "\n SELECT TOP 1(select name from ParCluster where id = L1Ca.ParCluster_Id) FROM ParLevel1XCluster L1Ca WITH(NOLOCK)     " +
           "\n WHERE @CLUSTER = L1Ca.ParCluster_ID                                                                                  " +
           "\n     AND L1.Id = L1Ca.ParLevel1_Id AND @CLUSTER = L1Ca.ParCluster_ID                                                  " +
           "\n     AND L1Ca.IsActive = 1                                                                                            " +
           "\n     AND L1Ca.EffectiveDate <= @DATAFINAL                                                                             " +
           "\n     ORDER BY L1Ca.EffectiveDate  desc                                                                                " +
           "\n )                                                                                                                    " +
           "\n , @CLUSTERNAME)  AS ClusterName                                                                                      " +




           "\n  , ISNULL(S.Id, @REGIONAL) AS Regional                                                                                                                                                                                                                              " +
           "\n  , ISNULL(S.Name, @REGIONALNAME) AS RegionalName                                                                                                                                                                                                                    " +
           "\n  , ISNULL(CL1.UnitId, @PARCOMPANY) AS ParCompanyId                                                                                                                                                                                                                  " +
           "\n  , ISNULL(C.Name, @PARCOMPANYNAME) AS ParCompanyName                                                                                                                                                                                                                " +
           "\n  , L1.IsRuleConformity AS TipoIndicador                                                                                                                                                                                                                             " +
           "\n  , L1.Id AS Level1Id                                                                                                                                                                                                                                                " +
           "\n  , L1.Name AS Level1Name                                                                                                                                                                                                                                            " +
           "\n  , ISNULL( " +

           "( " +

           "\n         SELECT TOP 1 L1Ca.ParCriticalLevel_Id FROM ParLevel1XCluster L1Ca WITH(NOLOCK) " +

           "\n         WHERE @CLUSTER = L1Ca.ParCluster_ID " +

           "\n             AND L1.Id = L1Ca.ParLevel1_Id AND @CLUSTER = L1Ca.ParCluster_ID " +

           "\n             AND L1Ca.IsActive = 1 " +

           "\n             AND L1Ca.EffectiveDate <= @DATAFINAL " +

           "\n         ORDER BY L1Ca.EffectiveDate  desc " +
            "\n	)" +

           "\n , @CRITERIO) AS Criterio                                                                                                                                                                                                                            " +
           "\n  , ISNULL( " +

           "( " +

           "\n         SELECT TOP 1 (select top 1 name from ParCriticalLevel where id = L1Ca.ParCriticalLevel_Id) FROM ParLevel1XCluster L1Ca WITH(NOLOCK) " +

           "\n         WHERE @CLUSTER = L1Ca.ParCluster_ID " +

           "\n             AND L1.Id = L1Ca.ParLevel1_Id AND @CLUSTER = L1Ca.ParCluster_ID " +

           "\n             AND L1Ca.IsActive = 1 " +

           "\n             AND L1Ca.EffectiveDate <= @DATAFINAL " +

           "\n         ORDER BY L1Ca.EffectiveDate  desc " +
            "\n	)" +

           "\n , @CRITERIONAME) AS CriterioName                                                                                                                                                                                                                  " +
           "\n  , ISNULL(" +

           "( " +

               "\n         SELECT TOP 1 L1Ca.Points FROM ParLevel1XCluster L1Ca WITH(NOLOCK) " +

               "\n         WHERE @CLUSTER = L1Ca.ParCluster_ID " +

               "\n             AND L1.Id = L1Ca.ParLevel1_Id AND @CLUSTER = L1Ca.ParCluster_ID " +

               "\n             AND L1Ca.IsActive = 1 " +

               "\n             AND L1Ca.EffectiveDate <= @DATAFINAL " +

               "\n         ORDER BY L1Ca.EffectiveDate desc  " +
                "\n	)" +

           "\n, @PONTOS) AS Pontos                                                                                                                                                                                                                            " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n  --ISNULL(CL.Id, @CLUSTER) AS Cluster                                                                                                                                                                                                                               " +
           "\n  --, (CL.Name)AS ClusterName                                                                                                                                                                                                                                        " +
           "\n  --, (S.Id)AS Regional                                                                                                                                                                                                                                              " +
           "\n  --, (S.Name)AS RegionalName                                                                                                                                                                                                                                        " +
           "\n  --, (CL1.UnitId)AS ParCompanyId                                                                                                                                                                                                                                    " +
           "\n  --, (C.Name)AS ParCompanyName                                                                                                                                                                                                                                      " +
           "\n  --, L1.IsRuleConformity AS TipoIndicador                                                                                                                                                                                                                           " +
           "\n  --, L1.Id AS Level1Id                                                                                                                                                                                                                                              " +
           "\n  --, L1.Name AS Level1Name                                                                                                                                                                                                                                          " +
           "\n  --, (CRL.Id)AS Criterio                                                                                                                                                                                                                                            " +
           "\n  --, (CRL.Name)AS CriterioName                                                                                                                                                                                                                                      " +
           "\n  --, (L1C.Points)AS Pontos                                                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n  , ST.Name AS TipoScore                                                                                                                                                                                                                                             " +
           "\n  ,                                                                                                                                                                                                                                                                  " +
           "\n   /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                              " +
           "\n   CASE                                                                                                                                                                                                                                                              " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     WHEN L1.Id = 25 THEN @AVFREQUENCIAVERIFICACAO                                                                                                                                                                                                                   " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     WHEN L1.hashKey = 1 THEN @VOLUMEPCC - @NAPCC                                                                                                                                                                                                                    " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     WHEN CT.Id IN(1, 2, 5) THEN ISNULL(SUM(CL1.WeiEvaluation),0)                                                                                                                                                                                                                 " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                                " +
           "\n     WHEN CT.Id IN(4) THEN AVG(A4.AM)" +
           "\n   END                                                                                                                                                                                                                                                               " +
           "\n   /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                              " +
           "\n   AS AV                                                                                                                                                                                                                                                             " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n  ,                                                                                                                                                                                                                                                                  " +
           "\n   /*INICIO NC COMPLETO----------------------------------------------*/                                                                                                                                                                                              " +
           "\n   CASE WHEN L1.IsRuleConformity = 1 THEN                                                                                                                                                                                                                            " +
           "\n       /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                          " +
           "\n       CASE                                                                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN L1.Id = 25 THEN @AVFREQUENCIAVERIFICACAO                                                                                                                                                                                                               " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN L1.hashKey = 1 THEN @VOLUMEPCC - @NAPCC                                                                                                                                                                                                                " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN CT.Id IN(1, 2, 5) THEN ISNULL(SUM(CL1.WeiEvaluation),0)                                                                                                                                                                                                             " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                            " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN CT.Id IN(4) THEN AVG(A4.AM)                                                                                                                                                                                                                            " +
           "\n       END                                                                                                                                                                                                                                                           " +
           "\n         /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                        " +
           "\n         -                                                                                                                                                                                                                                                           " +
           "\n       /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                          " +
           "\n       CASE                                                                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN L1.Id = 25 THEN @NCFREQUENCIAVERIFICACAO                                                                                                                                                                                                               " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN CT.Id IN(1, 2) THEN ISNULL(SUM(CL1.WeiDefects),0)                                                                                                                                                                                                                " +
           "\n         WHEN CT.Id IN(5) THEN CASE WHEN ISNULL(SUM(CL1.WeiDefects),0) > ISNULL(SUM(CL1.WeiEvaluation),0) THEN ISNULL(SUM(CL1.WeiEvaluation),0) ELSE ISNULL(SUM(CL1.WeiDefects),0) END                                                                                                                                                                                                                                                            " +
           "\n         WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                              " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN CT.Id IN(4) THEN AVG(A4.DEF_AM)                                                                                                                                                                                                                        " +

           "\n       END                                                                                                                                                                                                                                                           " +
           "\n       /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                          " +
           "\n    ELSE                                                                                                                                                                                                                                                             " +
           "\n       /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                          " +
           "\n       CASE                                                                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN L1.Id = 25 THEN @NCFREQUENCIAVERIFICACAO                                                                                                                                                                                                               " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN CT.Id IN(1, 2) THEN ISNULL(SUM(CL1.WeiDefects),0)                                                                                                                                                                                                                " +
           "\n         WHEN CT.Id IN(5) THEN CASE WHEN ISNULL(SUM(CL1.WeiDefects),0) > ISNULL(SUM(CL1.WeiEvaluation),0) THEN ISNULL(SUM(CL1.WeiEvaluation),0) ELSE ISNULL(SUM(CL1.WeiDefects),0) END                                                                                                                                                                                                                                                            " +
           "\n         WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                              " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN CT.Id IN(4) THEN AVG(A4.DEF_AM)                                                                                                                                                                                                                        " +

           "\n       END                                                                                                                                                                                                                                                           " +
           "\n       /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                          " +
           "\n    END                                                                                                                                                                                                                                                              " +
           "\n    /*FIM NC COMPLETO-------------------------------------------------*/                                                                                                                                                                                             " +
           "\n    AS NC                                                                                                                                                                                                                                                            " +
           "\n  ,                                                                                                                                                                                                                                                                  " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n   CASE                                                                                                                                                                                                                                                              " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     WHEN                                                                                                                                                                                                                                                            " +
           "\n       /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                          " +
           "\n       CASE                                                                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN L1.Id = 25 THEN @AVFREQUENCIAVERIFICACAO                                                                                                                                                                                                               " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN L1.hashKey = 1 THEN @VOLUMEPCC - @NAPCC                                                                                                                                                                                                                " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN CT.Id IN(1, 2, 5) THEN ISNULL(SUM(CL1.WeiEvaluation),0)                                                                                                                                                                                                             " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                            " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN CT.Id IN(4) THEN AVG(A4.AM)                                                                                                                                                                                                                            " +

           "\n       END                                                                                                                                                                                                                                                           " +
           "\n       /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                          " +
           "\n       = 0                                                                                                                                                                                                                                                           " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     THEN 0                                                                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n      ELSE                                                                                                                                                                                                                                                           " +
           "\n       /*INICIO NC COMPLETO----------------------------------------------*/                                                                                                                                                                                          " +
           "\n       CASE WHEN L1.IsRuleConformity = 1 THEN                                                                                                                                                                                                                        " +
           "\n           /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                      " +
           "\n           CASE                                                                                                                                                                                                                                                      " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             WHEN L1.Id = 25 THEN @AVFREQUENCIAVERIFICACAO                                                                                                                                                                                                           " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             WHEN L1.hashKey = 1 THEN @VOLUMEPCC - @NAPCC                                                                                                                                                                                                            " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             WHEN CT.Id IN(1, 2, 5) THEN ISNULL(SUM(CL1.WeiEvaluation),0)                                                                                                                                                                                                         " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                        " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             WHEN CT.Id IN(4) THEN AVG(A4.AM)                                                                                                                                                                                                                            " +

           "\n           END                                                                                                                                                                                                                                                       " +
           "\n             /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                    " +
           "\n             -  /* SUBTRAÇÃO */                                                                                                                                                                                                                                      " +
           "\n                /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                 " +
           "\n           CASE                                                                                                                                                                                                                                                      " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             WHEN L1.Id = 25 THEN @NCFREQUENCIAVERIFICACAO                                                                                                                                                                                                           " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             WHEN CT.Id IN(1, 2) THEN ISNULL(SUM(CL1.WeiDefects),0)                                                                                                                                                                                                            " +
           "\n             WHEN CT.Id IN(5) THEN CASE WHEN ISNULL(SUM(CL1.WeiDefects),0) > ISNULL(SUM(CL1.WeiEvaluation),0) THEN ISNULL(SUM(CL1.WeiEvaluation),0) ELSE ISNULL(SUM(CL1.WeiDefects),0) END                                                                                                                                                                                                                                                        " +
           "\n             WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             WHEN CT.Id IN(4) THEN AVG(A4.DEF_AM)                                                                                                                                                                                                                            " +
           "\n           END                                                                                                                                                                                                                                                       " +
           "\n           /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                      " +
           "\n        ELSE                                                                                                                                                                                                                                                         " +
           "\n           /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                      " +
           "\n           CASE                                                                                                                                                                                                                                                      " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             WHEN L1.Id = 25 THEN @NCFREQUENCIAVERIFICACAO                                                                                                                                                                                                           " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             WHEN CT.Id IN(1, 2) THEN ISNULL(SUM(CL1.WeiDefects),0)                                                                                                                                                                                                            " +
           "\n             WHEN CT.Id IN(5) THEN CASE WHEN ISNULL(SUM(CL1.WeiDefects),0) > ISNULL(SUM(CL1.WeiEvaluation),0) THEN ISNULL(SUM(CL1.WeiEvaluation),0) ELSE ISNULL(SUM(CL1.WeiDefects),0) END                                                                                                                                                                                                                                                        " +
           "\n             WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             WHEN CT.Id IN(4) THEN AVG(A4.DEF_AM)                                                                                                                                                                                                                            " +

           "\n           END                                                                                                                                                                                                                                                       " +
           "\n           /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                      " +
           "\n        END                                                                                                                                                                                                                                                          " +
           "\n        /*FIM NC COMPLETO-------------------------------------------------*/                                                                                                                                                                                         " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        / /*DIVISÃO*/                                                                                                                                                                                                                                                " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n       /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                          " +
           "\n       CASE                                                                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN L1.Id = 25 THEN @AVFREQUENCIAVERIFICACAO                                                                                                                                                                                                               " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN L1.hashKey = 1 THEN @VOLUMEPCC - @NAPCC                                                                                                                                                                                                                " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN CT.Id IN(1, 2, 5) THEN ISNULL(SUM(CL1.WeiEvaluation),0)                                                                                                                                                                                                             " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                            " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN CT.Id IN(4) THEN AVG(A4.AM)                                                                                                                                                                                                                            " +

           "\n       END                                                                                                                                                                                                                                                           " +
           "\n       /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n      END * 100                                                                                                                                                                                                                                                      " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n      AS REAL                                                                                                                                                                                                                                                        " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n  ,                                                                                                                                                                                                                                                                  " +
           "\n  CASE                                                                                                                                                                                                                                                               " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     WHEN(SELECT COUNT(1) FROM ParGoal G WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) AND G.EffectiveDate <= @DATAFINAL AND G.IsActive = 1 ) > 0 THEN                                                                                                   " +
           "\n         (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G (nolock)  WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) AND G.EffectiveDate <= @DATAFINAL AND G.IsActive = 1  ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)                                         " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     ELSE                                                                                                                                                                                                                                                            " +
           "\n         (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G (nolock)  WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) AND G.EffectiveDate <= @DATAFINAL AND G.IsActive = 1  ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)                                                                      " +
           "\n  END                                                                                                                                                                                                                                                                " +
           "\n  AS META                                                                                                                                                                                                                                                            " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n FROM     (SELECT p1.* FROM ParLevel1 p1 with (nolock) INNER JOIN ParLevel1XModule P1M with (nolock) on p1m.parlevel1_id = p1.id  WHERE ISNULL(p1.ShowScorecard, 1) = 1 and p1m.parmodule_id = @ParModule_Id  and p1m.IsActive = 1 and p1m.EffectiveDateStart <= @DATAINICIAL  and (P1M.ParCluster_Id is null OR P1M.ParCluster_Id in (select ParCluster_Id from ParCompanyCluster where ParCompany_Id = @ParCompany_Id and Active = 1))) L1                                                                                                                                                                                                                                             " +
           "\n LEFT JOIN ConsolidationLevel1 CL1   (nolock)                                                                                                                                                                                                                                  " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON L1.Id = CL1.ParLevel1_Id                                                                                                                                                                                                                                  " +
           "\n  LEFT JOIN ConsolidationLevel1XCluster CL1C " +
           "\n  ON CL1C.ConsolidationLevel1_Id = CL1.Id    " +
           "\n LEFT JOIN ParScoreType ST  (nolock)                                                                                                                                                                                                                                           " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON ST.Id = L1.ParScoreType_Id                                                                                                                                                                                                                                " +
           "\n LEFT JOIN ParCompany C    (nolock)                                                                                                                                                                                                                                            " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON C.Id = CL1.UnitId                                                                                                                                                                                                                                         " +
           "\n LEFT JOIN #AMOSTRATIPO4 A4    (nolock)                                                                                                                                                                                                                                        " +
           "\n         ON A4.UNIDADE = C.Id                                                                                                                                                                                                                                      " +
           "\n         AND A4.INDICADOR = L1.ID                                                                                                                                 " +
           "\n INNER JOIN ParCompanyXStructure CS   (nolock)                                                                                                                                                                                                                                  " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON CS.ParCompany_Id = C.Id AND CS.Active = 1                                                                                                                                                                                                                                  " +
           "\n LEFT JOIN ParStructure S     (nolock)                                                                                                                                                                                                                                         " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON S.Id = CS.ParStructure_Id                                                                                                                                                                                                                                 " +
           "\n LEFT JOIN ParStructureGroup SG     (nolock)                                                                                                                                                                                                                                   " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON SG.Id = S.ParStructureGroup_Id                                                                                                                                                                                                                            " +
           "\n LEFT JOIN ParCompanyCluster CCL   (nolock)                                                                                                                                                                                                                                    " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON CCL.ParCompany_Id = C.Id  AND CCL.Active = 1     and CASE WHEN CL1C.ParCluster_Id IS NULL THEN @CLUSTER ELSE CL1C.ParCluster_Id END = ccl.parcluster_id                                                                                                                                                                                                                             " +
           "\n LEFT JOIN ParCluster CL       (nolock)                                                                                                                                                                                                                                        " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON CL.Id = CCL.ParCluster_Id                                                                                                                                                                                                                                 " +
           "\n LEFT JOIN ParConsolidationType CT  (nolock)                                                                                                                                                                                                                                   " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON CT.Id = L1.ParConsolidationType_Id                                                                                                                                                                                                                        " +
           "\n --LEFT JOIN ParLevel1XCluster L1C  (nolock)                                                                                                                                                                                                                                     " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n --       ON L1C.ParLevel1_Id = L1.Id AND L1C.ParCluster_Id = CL.Id  AND L1C.IsActive = 1                                                                                                                                                                                                  " +
           "\n LEFT JOIN ParCriticalLevel CRL   (nolock)                                                                                                                                                                                                                                     " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON CRL.Id  = (select top 1 ParCriticalLevel_Id from ParLevel1XCluster aaa (nolock)  where aaa.ParLevel1_Id = L1.Id AND aaa.ParCluster_Id = CL.Id AND aaa.EffectiveDate <  @DATAFINAL)                                                                                                                                                                                                                       " +
           "\n WHERE(ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL OR L1.Id = 25)                                                                                                                                                                                          " +
           $@"   AND(C.Id = @ParCompany_Id OR(C.Id IS NULL AND L1.Id = 25 AND @CLUSTER in (SELECT DISTINCT ParCluster_Id FROM ParLevel1xCluster where IsActive = 1 AND parlevel1_id = 25 AND EffectiveDate < @DATAINICIAL)))                                                                                                                                                                                                       
           { Wshift }
            GROUP BY                                                                                                                                                                                                                                                            " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n      CL.Id                                                                                                                                                                                                                                                          " +
           "\n     , CL.Name                                                                                                                                                                                                                                                       " +
           "\n     , S.Id                                                                                                                                                                                                                                                          " +
           "\n     , S.Name                                                                                                                                                                                                                                                        " +
           "\n     , CL1.UnitId                                                                                                                                                                                                                                                    " +
           "\n     , C.Name                                                                                                                                                                                                                                                        " +
           "\n     , L1.IsRuleConformity                                                                                                                                                                                                                                           " +
           "\n     , L1.Id                                                                                                                                                                                                                                                         " +
           "\n     , L1.Name                                                                                                                                                                                                                                                       " +
           "\n     , CRL.Id                                                                                                                                                                                                                                                        " +
           "\n     , CRL.Name                                                                                                                                                                                                                                                      " +
           //"\n     , L1C.Points                                                                                                                                                                                                                                                    " +
           "\n     , ST.Name                                                                                                                                                                                                                                                       " +
           "\n     , CT.Id                                                                                                                                                                                                                                                         " +
           "\n     , L1.HashKey                                                                                                                                                                                                                                                    " +
           "\n     , C.Id                                                                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                    " +
           "\n ) SCORECARD  " +
           listaUnidades2 +
           "\n ) FIM                                                                                                                                                                                                                                                               " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n UNION ALL                                                                                                                                                                                                                                                           " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n /*SCORECARD VAZIO----------------------------------------*/                                                                                                                                                                                                         " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n SELECT                                                                                                                                                                                                                                                              " +
           "\n CL.Id AS Cluster                                                                                                                                                                                                                                                    " +
           "\n , CL.Name AS ClusterName                                                                                                                                                                                                                                            " +
           "\n , S.Id AS Regional                                                                                                                                                                                                                                                  " +
           "\n , S.Name AS RegionalName                                                                                                                                                                                                                                            " +
           "\n , C.Id AS ParCompanyId                                                                                                                                                                                                                                              " +
           "\n , C.Name AS ParCompanyName                                                                                                                                                                                                                                          " +
           "\n , CASE WHEN L1.IsRuleConformity = 0 THEN 1 ELSE 2 END AS TipoIndicador                                                                                                                                                                                              " +
           "\n , CASE WHEN L1.IsRuleConformity = 0 THEN 'MENOR' ELSE 'MAIOR' END AS TipoIndicadorName                                                                                               " +
           "\n , L1.Id AS Level1Id                                                                                                                                                                                                                                                 " +
           "\n  , L1.Name AS Level1Name                                                                                                                                                                                                                                            " +
           "\n  , ISNULL( " +

           "( " +

           "\n         SELECT TOP 1 L1Ca.ParCriticalLevel_Id FROM ParLevel1XCluster L1Ca WITH(NOLOCK) " +

           "\n         WHERE @CLUSTER = L1Ca.ParCluster_ID " +

           "\n             AND L1.Id = L1Ca.ParLevel1_Id " +

           "\n             AND L1Ca.IsActive = 1 " +

           "\n             AND L1Ca.EffectiveDate <= @DATAFINAL " +

           "\n         ORDER BY L1Ca.EffectiveDate  desc " +
            "\n	)" +

           "\n , @CRITERIO) AS Criterio                                                                                                                                                                                                                            " +
           "\n  , ISNULL( " +

           "( " +

           "\n         SELECT TOP 1 (select top 1 name from ParCriticalLevel where id = L1Ca.ParCriticalLevel_Id) FROM ParLevel1XCluster L1Ca WITH(NOLOCK) " +

           "\n         WHERE @CLUSTER = L1Ca.ParCluster_ID " +

           "\n             AND L1.Id = L1Ca.ParLevel1_Id " +

           "\n             AND L1Ca.IsActive = 1 " +

           "\n             AND L1Ca.EffectiveDate <= @DATAFINAL " +

           "\n         ORDER BY L1Ca.EffectiveDate  desc " +
            "\n	)" +

           "\n , @CRITERIONAME) AS CriterioName                                                                                                                                                                                                                  " +

           "\n ,0 as av " +
           "\n ,0 as nc " +

           "\n  , ISNULL(" +

           "( " +

               "\n         SELECT TOP 1 L1Ca.Points FROM ParLevel1XCluster L1Ca WITH(NOLOCK) " +

               "\n         WHERE @CLUSTER = L1Ca.ParCluster_ID " +

               "\n             AND L1.Id = L1Ca.ParLevel1_Id " +

               "\n             AND L1Ca.IsActive = 1 " +

               "\n             AND L1Ca.EffectiveDate <= @DATAFINAL " +

               "\n         ORDER BY L1Ca.EffectiveDate desc  " +
                "\n	)" +

           "\n, @PONTOS) AS Pontos " +
           "\n , 0 AS PontosIndicador                                                                                                                                                                                                                                              " +
           "\n , ROUND(CASE                                                                                                                                                                                                                                                              " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     WHEN(SELECT COUNT(1) FROM ParGoal G WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) AND G.EffectiveDate <= @DATAFINAL AND G.IsActive = 1) > 0 THEN                                                                                                   " +
           "\n         (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G (nolock)  WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) AND G.EffectiveDate <= @DATAFINAL AND G.IsActive = 1 ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)                                         " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     ELSE                                                                                                                                                                                                                                                            " +
           "\n         (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G (nolock)  WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) AND G.EffectiveDate <= @DATAFINAL AND G.IsActive = 1 ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)                                                                      " +
           "\n  END,2)                                                                                                                                                                                                                                                                " +
           "\n  AS META                                                                                                                                                                                                                                                            " +
           "\n , 0 AS Real                                                                                                                                                                                                                                                         " +
           "\n , 0  AS PontosAtingidos                                                                                                                                                                                                                                             " +
           "\n , 0  AS Scorecard                                                                                                                                                                                                                                                   " +
           "\n , ST.Name AS TipoScore                                                                                                                                                                                                                                              " +
           "\n FROM (SELECT p1.* FROM ParLevel1 p1 with (nolock) INNER JOIN ParLevel1XModule P1M with (nolock) on p1m.parlevel1_id = p1.id  WHERE ISNULL(ShowScorecard, 1) = 1 and p1m.parmodule_id = @ParModule_Id and p1m.IsActive = 1 and p1m.EffectiveDateStart <= @DATAINICIAL and (P1M.ParCluster_Id is null OR P1M.ParCluster_Id in (select ParCluster_Id from ParCompanyCluster where ParCompany_Id = @ParCompany_Id and Active = 1))) L1                                                                                                                                                                                                                                                " +
           "\n LEFT JOIN ParScoreType ST    (nolock)                                                                                                                                                                                                                                         " +
           "\n ON ST.Id = L1.ParScoreType_Id                                                                                                                                                                                                                                       " +
           "\n LEFT JOIN ParCompany C    (nolock)                                                                                                                                                                                                                                            " +
           "\n ON C.Id = @ParCompany_Id                                                                                                                                                                                                                                            " +
           "\n INNER JOIN ParCompanyXStructure CS (nolock)                                                                                                                                                                                                                                    " +
           "\n ON CS.ParCompany_Id = C.Id AND CS.Active = 1                                                                                                                                                                                                                                           " +
           "\n LEFT JOIN ParStructure S    (nolock)                                                                                                                                                                                                                                          " +
           "\n ON S.Id = CS.ParStructure_Id                                                                                                                                                                                                                                      " +
           "\n LEFT JOIN ParStructureGroup SG   (nolock)                                                                                                                                                                                                                                     " +
           "\n ON SG.Id = S.ParStructureGroup_Id                                                                                                                                                                                                                                   " +
           "\n LEFT JOIN ParCompanyCluster CCL    (nolock)                                                                                                                                                                                                                                   " +
           "\n ON CCL.ParCompany_Id = C.Id AND CCL.Active = 1                                                                                                                                                                                                                                        " +
           "\n LEFT JOIN ParCluster CL  (nolock)                                                                                                                                                                                                                                             " +
           "\n ON CL.Id = CCL.ParCluster_Id                                                                                                                                                                                                                                        " +
           "\n LEFT JOIN ParConsolidationType CT   (nolock)                                                                                                                                                                                                                                  " +
           "\n ON CT.Id = L1.ParConsolidationType_Id                                                                                                                                                                                                                               " +
           "\n --INNER JOIN ParLevel1XCluster L1C    (nolock)                                                                                                                                                                                                                                   " +
           "\n --ON L1C.ParLevel1_Id = L1.Id AND L1C.ParCluster_Id = CL.Id  AND L1C.IsActive = 1                                                                                                                                                                                                         " +
           "\n INNER JOIN ParCriticalLevel CRL    (nolock)                                                                                                                                                                                                                                    " +
           "\n ON CRL.Id = (select top 1 ParCriticalLevel_Id from ParLevel1XCluster aaa (nolock)  where aaa.ParLevel1_Id = L1.Id AND aaa.ParCluster_Id = CL.Id AND aaa.EffectiveDate <  @DATAFINAL)                                                                                                                                                                                                                                " +
           "\n WHERE C.Id = @ParCompany_Id                                                                                                                                                                                                                                         " +
           "\n AND L1.Id <> 25                                                                                                                                                                                                                                                     " +
           "\n -- AND L1.AddDate <= @DATAFINAL                                                                                                                                                                                                                                        " +
           "\n AND L1.IsActive <> 0                                                                                                                                                                                                                                                " +
           "\n AND L1.Id NOT IN(SELECT CCC.ParLevel1_Id FROM ConsolidationLevel1 CCC (nolock)  WHERE CCC.UnitId = @ParCompany_Id                                                                                                                                                             " +
           "\n AND CCC.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL)                                                                                                                                                                                                      " +
            //"\n AND L1C.ParCluster_Id = @CLUSTER                                                                                                                                                                                                                                                                    " +

            " AND ( " +

           "\n         SELECT TOP 1 ParCluster_Id FROM ParLevel1XCluster L1Ca WITH(NOLOCK) " +

           "\n         WHERE @CLUSTER = L1Ca.ParCluster_ID " +

           "\n             AND L1.Id = L1Ca.ParLevel1_Id " +

           "\n             AND L1Ca.IsActive = 1 " +

           "\n             AND L1Ca.EffectiveDate <= @DATAFINAL " +

           "\n         ORDER BY L1Ca.EffectiveDate  desc " +
            "\n	) = @CLUSTER" +
           listaUnidades3 +
           "\n  ) SC                                                                                                                                                                                                                                                               " +
           "\n  " + where +
           "\n  " + orderby + "                                                                                                                                                                                                                                                    " +
           "\n  DROP TABLE #AMOSTRATIPO4 ";

        return sql;
    }



    public string SelectScorecardCompleto(DateTime dtInicio, DateTime dtFim, int unidadeId, int tipo, int clusterSelected_Id, int GroupLevel1, int moduloId, int shift) //Se 0, tras pontos , se 1, tras tudo
    {
        string sql;

        sql = getSQLScorecard(dtInicio, dtFim, unidadeId, tipo, clusterSelected_Id, GroupLevel1, moduloId, shift); //Se 0, tras pontos , se 1, tras tudo


        return sql;

    }

}

