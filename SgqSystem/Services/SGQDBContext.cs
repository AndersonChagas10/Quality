﻿using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System;
using System.Linq;
using Dominio;
using System.Threading;
using System.Collections;
using SgqSystem.Services;
using DTO;
using ADOFactory;

namespace SGQDBContext
{
    public partial class ParLevel1
    {

        public string Id { get; set; }
        public int hashKey { get; set; }
        public string Name { get; set; }
        public int ParCriticalLevel_Id { get; set; }
        public string ParCriticalLevel_Name { get; set; }
        public bool HasSaveLevel2 { get; set; }
        public bool HasNoApplicableLevel2 { get; set; }
        public int ParConsolidationType_Id { get; set; }
        public int ParFrequency_Id { get; set; }
        public bool HasAlert { get; set; }
        public bool IsSpecific { get; set; }
        public bool haveRealTimeConsolidation { get; set; }
        public int RealTimeConsolitationUpdate { get; set; }
        public bool IsLimitedEvaluetionNumber { get; set; }
        public bool IsPartialSave { get; set; }
        public int tipoAlerta { get; set; }
        public decimal valorAlerta { get; set; }
        public bool HasCompleteEvaluation { get; set; }
        public bool IsReaudit { get; set; }
        public bool EditLevel2 { get; set; }
        public bool IsSpecificNumberEvaluetion { get; set; }
        public bool IsFixedEvaluetionNumber { get; set; }

        public bool HasGroupLevel2 { get; set; }
        public bool HasTakePhoto { get; set; }
        public bool ShowScorecard { get; set; }

        public int ParCluster_Id { get; set; }
        public int ParLevel1_Id { get; set; }

        public bool? IsRecravacao { get; set; }

        public ParLevel1()
        {

        }

        private SqlConnection db { get; set; }

        public ParLevel1(SqlConnection _db)
        {
            db = _db;
        }

        public ParLevel1 getById(int Id)
        {
            try
            {

                string sql = "SELECT * FROM ParLevel1 (nolock)  WHERE Id='" + Id + "'";

                ParLevel1 parLevel1List = null;
                using (Factory factory = new Factory("DefaultConnection"))
                {
                    parLevel1List = factory.SearchQuery<ParLevel1>(sql).ToList().FirstOrDefault();
                }

                return parLevel1List;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<ParLevel1> getByFamilia(DateTime dateCollection, int ParLevel1_Id = 0)
        {
            try
            {

                string sql = "SELECT * FROM ParLevel1 WHERE Id                                              " +
                             " IN (                                                                         " +
                             " SELECT ParLevel1_Id FROM ParLevel2ControlCompany                             " +
                             "WHERE                                                                         " +
                             "CAST(InitDate AS DATE) <= '" + dateCollection.ToString("yyyy-MM-dd") + "'     " +
                             " AND IsActive = 1                                                             ";
                if (ParLevel1_Id > 0)
                {
                    sql += " AND ParLevel1_Id = " + ParLevel1_Id;
                }
                sql += " GROUP BY ParLevel1_Id, InitDate                                                    " +
                        "HAVING MAX(InitDate) = InitDate)                                                   ";

                var parLevel1List = new List<ParLevel1>();

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    parLevel1List = factory.SearchQuery<ParLevel1>(sql).ToList();
                }

                return parLevel1List;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<ParLevel1> getParLevel1ParCriticalLevelList(int ParCompany_Id, string Level1ListId, DateTime dateCollection)
        {
            string whereIsChildren = " AND IsChildren = 0 ";

            string sql = @"SELECT * FROM (
                        -- SELECT CAST(C.Id AS VARCHAR) + '" + SyncServices.quebraProcesso + @"' + CAST(P1.Id AS VARCHAR)  AS Id, P1.Id as ParLevel1_Id , P1.Name + ' - ' + C.Name as Name, P1.HasTakePhoto, 
                        
                        SELECT CAST(C.Id AS VARCHAR) + '" + SyncServices.quebraProcesso + @"' + CAST(P1.Id AS VARCHAR)  AS Id, P1.Id as ParLevel1_Id , P1.Name, P1.HasTakePhoto, 

                        -- (select top 1 id from parCriticalLevel where id = (select top 1 parCriticalLevel_id from parlevel1XCluster where EffectiveDate <= CAST('" + dateCollection.ToString("yyyy-MM-dd") + @"' AS DATE)  and parlevel1_id = P1.id AND isactive = 1 and ParCluster_id = (select top 1 parCluster_id from ParCompanyCluster where ParCompany_id = '" + ParCompany_Id + @"') ORDER BY EffectiveDate Desc)) AS ParCriticalLevel_Id, 
                        -- (select top 1 name from parCriticalLevel where id = (select top 1 parCriticalLevel_id from parlevel1XCluster where EffectiveDate <= CAST('" + dateCollection.ToString("yyyy-MM-dd") + @"' AS DATE)  and parlevel1_id = P1.id AND isactive = 1 and ParCluster_id = (select top 1 parCluster_id from ParCompanyCluster where ParCompany_id = '" + ParCompany_Id + @"') ORDER BY EffectiveDate Desc)) AS ParCriticalLevel_Name,
                        
                        (select top 1 id from parCriticalLevel   where id = (select top 1 parCriticalLevel_id from parlevel1XCluster where EffectiveDate <= CAST('" + dateCollection.ToString("yyyy-MM-dd") + @"' AS DATE)  and parlevel1_id = P1.id AND isactive = 1 and ParCluster_id = PC.ParCluster_Id ORDER BY EffectiveDate Desc)) AS ParCriticalLevel_Id, 
                        (select top 1 name from parCriticalLevel where id = (select top 1 parCriticalLevel_id from parlevel1XCluster where EffectiveDate <= CAST('" + dateCollection.ToString("yyyy-MM-dd") + @"' AS DATE)  and parlevel1_id = P1.id AND isactive = 1 and ParCluster_id = PC.ParCluster_Id ORDER BY EffectiveDate Desc)) AS ParCriticalLevel_Name,

                        P1.HasSaveLevel2 AS HasSaveLevel2, P1.ParConsolidationType_Id AS ParConsolidationType_Id, P1.ParFrequency_Id AS ParFrequency_Id,     
                        P1.HasNoApplicableLevel2 AS HasNoApplicableLevel2, P1.HasAlert, P1.IsSpecific, P1.hashKey, P1.haveRealTimeConsolidation, P1.RealTimeConsolitationUpdate, P1.IsLimitedEvaluetionNumber, P1.IsPartialSave
                        ,AL.ParNotConformityRule_Id AS tipoAlerta, AL.Value AS valorAlerta, AL.IsReaudit AS IsReaudit, P1.HasCompleteEvaluation AS HasCompleteEvaluation, P1.HasGroupLevel2 AS HasGroupLevel2, P1.EditLevel2 AS EditLevel2, P1.IsFixedEvaluetionNumber AS IsFixedEvaluetionNumber 
                        ,C.Id AS ParCluster_Id
                        FROM ParLevel1 P1  (nolock)                                                                                                         
                        INNER JOIN (SELECT ParLevel1_Id FROM ParLevel3Level2Level1 where Active = 1 GROUP BY ParLevel1_Id) P321                                     
                        ON P321.ParLevel1_Id = P1.Id                                                                                               
                        LEFT JOIN ParNotConformityRuleXLevel AL    (nolock)                                                                                 
                        ON AL.ParLevel1_Id = P1.Id   AND AL.IsActive = 1                                                                                               
                        INNER JOIN (SELECT ParLevel1_Id FROM (select * from parGoal (nolock)  where IsActive = 1 and (ParCompany_Id is null or ParCompany_Id = '" + ParCompany_Id + @"') and EffectiveDate <= CAST('" + dateCollection.ToString("yyyy-MM-dd") + @"'AS DATE)) A GROUP BY ParLevel1_Id) G  
                        ON P1.Id = G.ParLevel1_Id   
                        INNER JOIN ParCompanyCluster PC
						ON PC.ParCompany_Id = '" + ParCompany_Id + @"' AND PC.Active = 1
                       
						INNER JOIN ParCluster C
						on c.id = pc.parcluster_id
                       
                        WHERE 1 =1                                                                          
                        AND IsChildren = 0 
                        " + whereIsChildren + @"
                        AND P1.IsActive = 1 ";

            if (Level1ListId != "" && Level1ListId != null)
            {
                sql += " AND P1.Id IN (" + Level1ListId.Substring(0, Level1ListId.Length - 1) + ") ";
            }

            sql += @") A
                    where ParCriticalLevel_id is not null  
                    ORDER BY 5, 2";

            List<ParLevel1> parLevel1List = new List<ParLevel1>();

            using (Factory factory = new Factory("DefaultConnection"))
            {
                parLevel1List = factory.SearchQuery<ParLevel1>(sql).ToList();
            }

            return parLevel1List;
        }
    }

    public partial class Result_Level3_Photo
    {
        public int ID { get; set; }
        public Nullable<int> Result_Level3_Id { get; set; }
        public String Photo_Thumbnaills { get; set; }
        public String Photo { get; set; }
        public Nullable<double> Latitude { get; set; }
        public Nullable<double> Longitude { get; set; }
    }

    public partial class ParLevel1Alertas
    {

        public decimal Nivel1 { get; set; }
        public decimal Nivel2 { get; set; }
        public string Nivel3 { get; set; }
        public decimal VolumeAlerta { get; set; }
        public decimal Meta { get; set; }

        private SqlConnection db { get; set; }
        public ParLevel1Alertas() { }

        public ParLevel1Alertas(SqlConnection _db)
        {
            db = _db;
        }

        public ParLevel1Alertas getAlertas(ParLevel1 ParLevel1, int ParCompany_Id, DateTime DateCollect, int Shift_Id)
        {

            int ParLevel1_Id = ParLevel1.ParLevel1_Id;
            int ParCluster_Id = ParLevel1.ParCluster_Id;

            string _DataCollect = DateCollect.ToString("yyyyMMdd");

            string sql = $@"DECLARE @PARLEVEL1 INT = {ParLevel1_Id}
create table #pcc(hashKey int, id int, indicador int, av int, am int, peso float)
INSERT INTO #pcc
	SELECT
		1
	   ,3
	   ,3
	   ,1
	   ,1
	   ,1
SELECT
	CAST(SUM((VolumeAlerta * (Meta / 100))) AS VARCHAR) AS nivel3
   ,SUM((VolumeAlerta * (Meta / 100)) / 3 * 2) AS nivel2
   ,SUM((VolumeAlerta * (Meta / 100)) / 3) AS nivel1
   ,SUM(VolumeAlerta) AS VolumeAlerta
   ,AVG(Meta) AS Meta
FROM (SELECT
		CASE
			WHEN ParConsolidationType_Id = 1 THEN AV * AM * PESO
			WHEN ParConsolidationType_Id = 2 THEN AV * AM * (CASE
					WHEN PESO > 0 THEN 1
					ELSE 0
				END)
			ELSE AV * AM * PESO
		END AS VolumeAlerta
	   ,_META AS Meta
	   ,*
	FROM (SELECT
			RESULT.*
		   ,(SELECT
					ParConsolidationType_Id
				FROM ParLevel1(nolock)
				WHERE Id = @PARLEVEL1)
			AS ParConsolidationType_Id
		   ,(SELECT TOP 1
					PercentValue
				FROM ParGoal(nolock)
				WHERE ParLevel1_Id = @PARLEVEL1
				AND (ParCompany_Id = {ParCompany_Id}
				OR ParCompany_Id IS NULL)
				AND EffectiveDate <= CONVERT(DATE, '{_DataCollect}')
				AND IsActive = 1
				ORDER BY ParCompany_id DESC, EffectiveDate DESC)
			AS _META
		FROM (
			/*****PCC1b**************************************************************************************************************************************************************************/
			SELECT TOP 1
				*
			FROM (SELECT
					*
				FROM (SELECT TOP 1
						1 AS ""hashKey""
					   ,(SELECT
								Id
							FROM ParLevel1(nolock)
							WHERE hashKey = 1)
						AS ID
					   ,'PCC1b' AS INDICADOR
					   ,1 AS AV
					   ,COALESCE(Amostras, 0) * 2 AS AM
					   ,1 AS PESO
					FROM VolumePcc1b(nolock)
					WHERE ParCompany_id = { ParCompany_Id }
					AND CONVERT(DATE, Data) = CONVERT(DATE, '{ _DataCollect }')
                    AND (Shift_Id = { Shift_Id }
				    OR Shift_Id IS NULL)
				    ORDER BY Shift_Id, Data DESC) PCC) PCC1b
			/************************************************************************************************************************************************************************************/
			UNION ALL
			/*****CEP VÁCUO GRD******************************************************************************************************************************************************************/
			SELECT
				*
			FROM (SELECT TOP 1
					3 AS ""hashKey""
				   ,(SELECT
							Id
						FROM ParLevel1(nolock)
						WHERE hashKey = 3)
					AS ID
				   ,'CEP VÁCUO GRD' AS INDICADOR
				   ,Avaliacoes AS AV
				   ,Amostras * QtdadeFamiliaProduto AS AM
				   ,1 AS PESO
				FROM VolumeVacuoGRD(nolock)
				WHERE ParCompany_id = {ParCompany_Id}
				AND CONVERT(DATE, Data) <= CONVERT(DATE, '{_DataCollect}')
                AND (Shift_Id = { Shift_Id }
				OR Shift_Id IS NULL)
				ORDER BY Shift_Id, Data DESC) GRD
			/************************************************************************************************************************************************************************************/
			UNION ALL
			/*****CEP DESOSSA********************************************************************************************************************************************************************/
			SELECT
				*
			FROM (SELECT TOP 1
					2 AS ""hashKey""
				   ,(SELECT
							Id
						FROM ParLevel1(nolock)
						WHERE hashKey = 2)
					AS ID
				   ,'CEP DESOSSA' AS INDICADOR
				   ,Avaliacoes AS AV
				   ,Amostras * QtdadeFamiliaProduto AS AM
				   ,1 AS PESO
				FROM VolumeCepDesossa(nolock)
				WHERE ParCompany_id = {ParCompany_Id}
				AND CONVERT(DATE, Data) <= CONVERT(DATE, '{_DataCollect}')
                AND (Shift_Id = { Shift_Id }
				OR Shift_Id IS NULL)
				ORDER BY Shift_Id, Data DESC) DESOSSA
			/************************************************************************************************************************************************************************************/
			UNION ALL
			/*****CEP RECORTES*******************************************************************************************************************************************************************/
			SELECT
				*
			FROM (SELECT TOP 1
					4 AS ""hashKey""
				   ,(SELECT
							Id
						FROM ParLevel1(nolock)
						WHERE hashKey = 4)
					AS ID
				   ,'CEP RECORTES' AS INDICADOR
				   ,Avaliacoes AS AV
				   ,Amostras AS AM
				   ,1 AS PESO
				FROM VolumeCepRecortes(nolock)
				WHERE ParCompany_id = {ParCompany_Id}
				AND CONVERT(DATE, Data) <= CONVERT(DATE, '{_DataCollect}')
                AND (Shift_Id = { Shift_Id }
				OR Shift_Id IS NULL)
				ORDER BY Shift_Id, Data DESC) RECORTES
			/************************************************************************************************************************************************************************************/
			UNION ALL
			/*****OUTROS*************************************************************************************************************************************************************************/
			SELECT
				hashKey AS hasKey
			   ,Id AS Id
			   ,INDICADOR AS INDICADOR
			   ,AVALIACOES AS AV
			   ,AMOSTRAS AS AM
			   ,PESO_DA_TAREFA AS PESO
			FROM (SELECT
					INDICADOR.*
				   ,MON.Name AS MONITORAMENTO
				   ,TAR.Name AS TAREFA
				   ,MONITORAMENTOS.Weight AS ""PESO_DA_TAREFA""
				   ,(SELECT TOP 1
							Number
						FROM ParEvaluation(nolock)
						WHERE ParLevel2_Id = MON.Id
						AND (ParCompany_Id = {ParCompany_Id}
						OR ParCompany_Id IS NULL)
						AND (ParLevel1_id = @PARLEVEL1
						OR ParLevel1_id IS NULL)
						AND ParCluster_Id = {ParCluster_Id}
						ORDER BY ParCompany_Id DESC)
					AS AVALIACOES
				   ,(SELECT TOP 1
							Number
						FROM ParSample(nolock)
						WHERE ParLevel2_Id = MON.Id
						AND (ParCompany_Id = {ParCompany_Id}
						OR ParCompany_Id IS NULL)
						AND (ParLevel1_id = @PARLEVEL1
						OR ParLevel1_id IS NULL)
						AND ParCluster_Id = {ParCluster_Id}
						ORDER BY ParCompany_Id DESC)
					AS AMOSTRAS
				FROM
				----INDICADOR-------------------------------------------------- 
				(SELECT
						IND.Id AS ""Id""
					   ,IND.""hashKey"" AS ""hashKey""
					   ,IND.Name AS ""INDICADOR""
					   ,IND.hashKey AS ""CÓDIGO ESPECÍFICO""
					   ,Cons.Name AS ""TIPO_DE_CONSOLIDACAO""
					   ,IND.HasAlert AS ""EMITE_ALERTA ? ""
					   ,IND.IsSpecific AS ""ESPECÍFICO""
					   ,IND.IsSpecificNumberEvaluetion AS ""ESPECÍFICO - AVALIAÇÕES""
					   ,IND.IsSpecificNumberSample AS ""ESPECÍFICO - AMOSTRAS""
					   ,IND.IsFixedEvaluetionNumber AS ""ESPECÍFICO - FAMÍLIA DE PRODUTOS""
					FROM ParLevel1 IND (NOLOCK)
					LEFT JOIN ParConsolidationType Cons (NOLOCK)
						ON Cons.Id = IND.ParConsolidationType_Id
					WHERE IND.Id = @PARLEVEL1) INDICADOR
				INNER JOIN
				----MONITORAMENTO---------------------------------------------- 
				(SELECT
						@PARLEVEL1 AS INDICADOR
					   ,*
					FROM (SELECT
							I_M_T.ParLevel3Level2_Id
						   ,MAX(I_M_T.ParCompany_Id) AS ParCompany_Id
						FROM ParLevel3Level2Level1 I_M_T (NOLOCK)
						WHERE I_M_T.ParLevel1_Id = @PARLEVEL1
						AND I_M_T.Active = 1
						AND (I_M_T.ParCompany_Id = {ParCompany_Id}
						OR I_M_T.ParCompany_Id IS NULL)
						GROUP BY I_M_T.ParLevel3Level2_Id) M1
					INNER JOIN (SELECT
							(SELECT TOP 1
									Id
								FROM ParLevel3Level2(nolock)
								WHERE ParLevel2_Id = M_T.ParLevel2_Id
								AND ParLevel3_Id = M_T.ParLevel3_Id
								AND COALESCE(ParCompany_Id, 0) = MAX(COALESCE(M_T.ParCompany_Id, 0)))
							AS Id
						   ,(SELECT TOP 1
									Weight
								FROM ParLevel3Level2(nolock)
								WHERE ParLevel2_Id = M_T.ParLevel2_Id
								AND ParLevel3_Id = M_T.ParLevel3_Id
								AND COALESCE(ParCompany_Id, 0) = MAX(COALESCE(M_T.ParCompany_Id, 0)))
							AS Weight
						   ,M_T.ParLevel2_Id
						   ,M_T.ParLevel3_Id
						   ,MAX(M_T.ParCompany_Id) AS _ParCompany_Id
						FROM ParLevel3Level2 M_T (NOLOCK)
						INNER JOIN ParLevel3 TAR (NOLOCK)
							ON TAR.Id = M_T.ParLevel3_Id
							AND TAR.IsActive = 1
						INNER JOIN ParLevel2 MON (NOLOCK)
							ON MON.Id = M_T.ParLevel2_Id
							AND MON.IsActive = 1
						WHERE (M_T.ParCompany_Id = {ParCompany_Id}
						OR M_T.ParCompany_Id IS NULL)
						GROUP BY M_T.ParLevel2_Id
								,M_T.ParLevel3_Id) M2
						ON M1.ParLevel3Level2_Id = M2.Id) MONITORAMENTOS
					ON INDICADOR.ID = MONITORAMENTOS.INDICADOR
				INNER JOIN ParLevel2 MON (NOLOCK)
					ON MON.Id = MONITORAMENTOS.ParLevel2_Id
				INNER JOIN ParLevel3 TAR (NOLOCK)
					ON TAR.Id = MONITORAMENTOS.ParLevel3_Id
				WHERE (hashKey IS NULL
				OR hashKey = 5)) OUTROS
		/************************************************************************************************************************************************************************************/
		) RESULT
		WHERE RESULT.ID = @PARLEVEL1) SELECAO) FIM
HAVING SUM(VolumeAlerta) IS NOT NULL ";

            ParLevel1Alertas parLevel2List = null;
            using (Factory factory = new Factory("DefaultConnection"))
            {
                parLevel2List = factory.SearchQuery<ParLevel1Alertas>(sql).FirstOrDefault();
            }

            return parLevel2List;
        }
    }

    public partial class ParLevel2
    {

        public string Id { get; set; }

        public int ParLevel2_id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasSampleTotal { get; set; }

        public bool IsEmptyLevel3 { get; set; }
        public bool HasShowLevel03 { get; set; }
        public bool HasGroupLevel3 { get; set; }

        public DateTime AddDate { get; set; }
        public DateTime AlterDate { get; set; }

        public bool IsActive { get; set; }
        public bool HasTakePhoto { get; set; }

        public int ParNotConformityRule_id { get; set; }

        public int ParFrequency_Id { get; set; }
        public int ParDepartment_Id { get; set; }

        public decimal Value { get; set; }

        public bool IsReaudit { get; set; }

        private SqlConnection db { get; set; }

        public ParLevel2(SqlConnection _db)
        {
            db = _db;
        }

        public int getExisteAvaliacao(int ParCompany_Id, int ParLevel2_Id, int ParLevel1_Id)
        {

            string sql = "" +
                "\n  select count(1) from " +
                "\n ( " +
                "\n select * from ParEvaluation (nolock)  where ParLevel2_id = " + ParLevel2_Id + " and ParCompany_Id = " + ParCompany_Id + " AND IsActive = 1 " +
                "\n union all " +
                "\n select * from ParEvaluation (nolock)  where ParLevel2_id = " + ParLevel2_Id + " and ParCompany_Id is Null  AND IsActive = 1 " +
                "\n union all " +
                "\n select * from ParEvaluation (nolock)  where ParLevel2_id = " + ParLevel2_Id + " and ParLevel1_Id = " + ParLevel1_Id + "  AND IsActive = 1 " +
                "\n ) temAv ";

            SqlCommand command = new SqlCommand(sql, db);
            return command.ExecuteNonQuery();

        }

        public int getExisteAmostra(int ParCompany_Id, int ParLevel2_Id, int ParLevel1_Id)
        {

            string sql = "" +
                "\n  select count(1) from " +
                "\n ( " +
                "\n select * from ParSample (nolock)  where ParLevel2_id = " + ParLevel2_Id + " and ParCompany_Id = " + ParCompany_Id + " and IsActive = 1 " +
                "\n union all " +
                "\n select * from ParSample (nolock)  where ParLevel2_id = " + ParLevel2_Id + " and ParCompany_Id is Null  and IsActive = 1 " +
                "\n union all " +
                "\n select * from ParSample (nolock)  where ParLevel2_id = " + ParLevel2_Id + " and ParLevel1_Id = " + ParLevel1_Id + "  AND IsActive = 1 " +
                "\n ) temAm ";

            SqlCommand command = new SqlCommand(sql, db);
            return command.ExecuteNonQuery();
        }

        public ParLevel2()
        {

        }

        public ParLevel2 getById(int Id)
        {
            try
            {
                string sql = "SELECT * FROM ParLevel2 (nolock)  WHERE Id='" + Id + "'";

                ParLevel2 parLevel1List = null;
                using (Factory factory = new Factory("DefaultConnection"))
                {
                    parLevel1List = factory.SearchQuery<ParLevel2>(sql).FirstOrDefault();
                }
                return parLevel1List;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<ParLevel2> getLevel2ByIdLevel1(SGQDBContext.ParLevel1 parLevel1, DateTime dateCollection, int ParCompany_Id)
        {

            /****CONTROLE DE FAMÍLIA DE PRODUTOS*****/

            if (parLevel1.IsFixedEvaluetionNumber)
            {
                string sql = "   SELECT '" + parLevel1.ParCluster_Id + SyncServices.quebraProcesso + @"' + CAST(PL2.Id AS VARCHAR)  AS Id, PL2.Id as ParLevel2_Id, PL2.Name AS Name, PL2.HasSampleTotal, PL2.HasTakePhoto, PL2.IsEmptyLevel3, AL.ParNotConformityRule_id, AL.Value, AL.IsReaudit, PL2.ParFrequency_id " +
                             "\n FROM ParLevel3Level2 P32   (nolock)                                                                                                                             " +
                             "\n INNER JOIN ParLevel3Level2Level1 P321  (nolock)                                                                                                                 " +
                             "\n ON P321.ParLevel3Level2_Id = P32.Id and p321.active = 1                                                                                                                  " +
                             "\n INNER JOIN ParLevel2 PL2   (nolock)                                                                                                                             " +
                             "\n ON PL2.Id = P32.ParLevel2_Id                                                                                                                          " +
                             "\n  LEFT JOIN ParNotConformityRuleXLevel AL   (nolock)                                                                                                             " +
                             "\n  ON AL.ParLevel2_Id = PL2.Id  AND AL.IsActive = 1                                                                                                     " +
                             "\n INNER JOIN (SELECT * FROM ParLevel2ControlCompany PL (nolock)  INNER JOIN                                                                                       " +
                             "\n (SELECT MAX(InitDate) Data, ParCompany_Id AS UNIDADE FROM ParLevel2ControlCompany   (nolock)                                                                    " +
                             "\n where ParLevel1_Id = '" + parLevel1.ParLevel1_Id + "' AND CAST(InitDate AS DATE) <= '" + dateCollection.ToString("yyyy-MM-dd") + "'  and (ParCompany_Id =  " + ParCompany_Id + " or ParCompany_Id is null)   and IsActive = 1 " +

                             "\n -- GROUP BY ParCompany_Id) F1 ON (CAST(F1.data AS DATE) = CAST(PL.initDate AS DATE) AND PL.IsActive = 1) OR (CAST(f1.data AS DATE) = CAST(PL.initDate AS DATE) AND CAST(f1.data AS DATE) < CAST(PL.AlterDate AS DATE) AND PL.IsActive = 1) AND (F1.UNIDADE = PL.ParCompany_id                                                                " +
                             "\n GROUP BY ParCompany_Id) F1 ON (CAST(F1.data AS DATE) = CAST(PL.initDate AS DATE) AND F1.UNIDADE = PL.ParCompany_id) OR  (CAST(f1.data AS DATE) = CAST(PL.initDate AS DATE) AND F1.UNIDADE IS NULL)                                                              " +
                             "\n -- or F1.UNIDADE is null))  Familia                                                                                                                      " +
                             "\n )  Familia                                                                                                                      " +
                             "\n ON Familia.ParLevel2_Id = PL2.Id                                                                                                                      " +
                             "\n WHERE P321.ParLevel1_Id = " + parLevel1.ParLevel1_Id + "                                                                                                      " +
                             "\n AND PL2.IsActive = 1     " +
                             "\n AND (Familia.ParCompany_Id = " + ParCompany_Id + "  or Familia.ParCompany_Id IS NULL)                                                               " +
                             "\n and Familia.IsActive = 1 " +
                             "\n GROUP BY PL2.Id, PL2.Name, PL2.HasSampleTotal, PL2.IsEmptyLevel3, AL.ParNotConformityRule_Id, AL.IsReaudit, AL.Value, PL2.ParFrequency_id, PL2.HasTakePhoto             ";

                List<ParLevel2> parLevel2List = new List<ParLevel2>();

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    parLevel2List = factory.SearchQuery<ParLevel2>(sql);
                }

                return parLevel2List;

            }
            else
            {

                string sql = "\n SELECT '" + parLevel1.ParCluster_Id + SyncServices.quebraProcesso + @"' + CAST(PL2.Id AS VARCHAR)  AS Id, PL2.Id as ParLevel2_Id, PL2.Name AS Name, PL2.HasSampleTotal, PL2.HasTakePhoto, PL2.IsEmptyLevel3, AL.ParNotConformityRule_id, AL.Value, AL.IsReaudit,PL2.ParFrequency_id  " +
                         "\n FROM ParLevel3Level2 P32                                      " +
                         "\n INNER JOIN ParLevel3Level2Level1 P321                         " +
                         "\n ON P321.ParLevel3Level2_Id = P32.Id and p321.active = 1                           " +
                         "\n INNER JOIN ParLevel2 PL2                                      " +
                         "\n ON PL2.Id = P32.ParLevel2_Id                                  " +
                         "\n LEFT JOIN ParNotConformityRuleXLevel AL                                                                                   " +
                         "\n ON AL.ParLevel2_Id = PL2.Id     AND AL.IsActive = 1                                                                                             " +
                        "\n WHERE P321.ParLevel1_Id = '" + parLevel1.ParLevel1_Id + "'              " +
                         "\n AND PL2.IsActive = 1  AND P32.IsActive = 1 AND P321.Active = 1                                        " +
                         "\n AND " +
                         "\n  (select sum(a) from " +
                         "\n ( " +
                         "\n select number as a  from ParEvaluation (nolock)  where IsActive = 1 and ParLevel2_id = PL2.Id and ParCompany_Id = " + ParCompany_Id + " and ParLevel1_Id = " + parLevel1.ParLevel1_Id + " and ParCluster_Id = " + parLevel1.ParCluster_Id + " " +
                         "\n union all " +
                         "\n select number as a  from ParEvaluation (nolock)  where IsActive = 1 and ParLevel2_id = PL2.Id and ParCompany_Id is Null and ParLevel1_Id = " + parLevel1.ParLevel1_Id + " and ParCluster_Id = " + parLevel1.ParCluster_Id + " " +
                         "\n ) temAv) > 0 " +
                         "\n AND " +
                         "\n  (select sum(a) from " +
                         "\n ( " +
                         "\n select number as a  from ParSample  (nolock) where IsActive = 1 and ParLevel2_id = PL2.Id and ParCompany_Id = " + ParCompany_Id + " and ParLevel1_Id = " + parLevel1.ParLevel1_Id + " and ParCluster_Id = " + parLevel1.ParCluster_Id + " " +
                         "\n union all " +
                         "\n select number as a  from ParSample  (nolock) where IsActive = 1 and ParLevel2_id = PL2.Id and ParCompany_Id is Null and ParLevel1_Id = " + parLevel1.ParLevel1_Id + " and ParCluster_Id = " + parLevel1.ParCluster_Id + " " +
                         "\n ) temAm) > 0 " +
                         "\n GROUP BY PL2.Id, PL2.Name, PL2.HasSampleTotal, PL2.IsEmptyLevel3, AL.ParNotConformityRule_Id, AL.IsReaudit, AL.Value, PL2.ParFrequency_id, PL2.HasTakePhoto                 " +
                         "\n ";

                List<ParLevel2> parLevel2List = new List<ParLevel2>();

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    parLevel2List = factory.SearchQuery<ParLevel2>(sql);
                }

                return parLevel2List;
            }
        }
    }

    public partial class ParLevel2Evaluate
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public int Evaluate { get; set; }
        public int ParCluster_Id { get; set; }
        private SqlConnection db { get; set; }

        public ParLevel2Evaluate() { }
        public ParLevel2Evaluate(SqlConnection _db)
        {
            db = _db;
        }

        public IEnumerable<ParLevel2Evaluate> getEvaluate(ParLevel1 ParLevel1, int? ParCompany_Id, DateTime DateCollection, int Shift_Id)
        {

            string queryCompany = null;

            var date = DateCollection.ToString("yyyy-MM-dd");

            if (ParLevel1.hashKey == 2 && ParCompany_Id != null)
            {

                string sql = $@"SELECT '{ ParLevel1.ParCluster_Id + SyncServices.quebraProcesso }' + CAST(PL2.Id AS VARCHAR) AS Id,
                             { ParLevel1.ParCluster_Id } AS ParCluster_Id, PL2.Name AS Name,
                             (SELECT top 1 Avaliacoes 
                             FROM VolumeCepDesossa (nolock) 
                             WHERE Data = (SELECT MAX(DATA) 
                                         FROM VolumeCepDesossa (nolock)  
                                         WHERE ParCompany_id = { ParCompany_Id } 
                                         AND (Shift_Id = {Shift_Id} OR Shift_Id IS NULL)
                             AND CAST(DATA AS DATE) <= '{ date }') 
                             AND ParCompany_id = { ParCompany_Id } 
                             AND (Shift_Id = {Shift_Id} OR Shift_Id IS NULL)
                             ORDER BY Shift_Id DESC) AS Evaluate 
                             FROM ParLevel3Level2 P32  (nolock)
                             INNER JOIN ParLevel3Level2Level1 P321 (nolock)
                                 ON P321.ParLevel3Level2_Id = P32.Id
                             INNER JOIN ParLevel2 PL2 (nolock)
                                 ON PL2.Id = P32.ParLevel2_Id
                             WHERE P321.ParLevel1_Id = '{ ParLevel1.ParLevel1_Id }' and p321.Active = 1
                             GROUP BY PL2.Id, PL2.Name";

                List<ParLevel2Evaluate> parEvaluate = new List<ParLevel2Evaluate>();

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    parEvaluate = factory.SearchQuery<ParLevel2Evaluate>(sql);
                }

                return parEvaluate;
            }
            else if (ParLevel1.hashKey == 3 && ParCompany_Id != null)
            {

                string sql = $@"SELECT '{ ParLevel1.ParCluster_Id + SyncServices.quebraProcesso }' + CAST(PL2.Id AS VARCHAR)  AS Id,
                    { ParLevel1.ParCluster_Id } AS ParCluster_Id, 
                    PL2.Name AS Name,
                             (SELECT TOP 1 Avaliacoes FROM VolumeVacuoGRD (nolock)  
                                WHERE Data = (
                                    SELECT MAX(DATA) FROM VolumeVacuoGRD (nolock)  
                                    WHERE ParCompany_id = { ParCompany_Id }
                                    AND (Shift_Id = {Shift_Id} OR Shift_Id IS NULL)
                                AND CAST(DATA AS DATE) <= '{ date }') 
                                AND ParCompany_id = { ParCompany_Id }
                                AND (Shift_Id = {Shift_Id} OR Shift_Id IS NULL)
                                ORDER BY Shift_Id DESC) AS Evaluate
                             FROM ParLevel3Level2 P32  (nolock)
                             INNER JOIN ParLevel3Level2Level1 P321  (nolock)
                                ON P321.ParLevel3Level2_Id = P32.Id
                             INNER JOIN ParLevel2 PL2 (nolock)
                                ON PL2.Id = P32.ParLevel2_Id
                             WHERE P321.ParLevel1_Id = '{ ParLevel1.ParLevel1_Id }' and  p321.Active = 1
                             GROUP BY PL2.Id, PL2.Name";

                List<ParLevel2Evaluate> parEvaluate = new List<ParLevel2Evaluate>();

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    parEvaluate = factory.SearchQuery<ParLevel2Evaluate>(sql);
                }

                return parEvaluate;

            }
            else if (ParLevel1.hashKey == 4 && ParCompany_Id != null)
            {

                string sql = $@"SELECT '{ ParLevel1.ParCluster_Id + SyncServices.quebraProcesso }' + CAST(PL2.Id AS VARCHAR) AS Id,
                    { ParLevel1.ParCluster_Id } AS ParCluster_Id, PL2.Name AS Name,
                             (SELECT TOP 1 Avaliacoes FROM VolumeCepRecortes (nolock) WHERE Data = (
                                    SELECT MAX(DATA) FROM VolumeCepRecortes (nolock)  
                                    WHERE ParCompany_id = { ParCompany_Id } 
                                    AND (Shift_Id = {Shift_Id} OR Shift_Id IS NULL)
                                    AND CAST(DATA AS DATE) <= '{ date }') 
                                AND (Shift_Id = {Shift_Id} OR Shift_Id IS NULL)
                                and ParCompany_id = { ParCompany_Id } 
                                ORDER BY Shift_Id DESC) AS Evaluate
                             FROM ParLevel3Level2 P32 (nolock)
                             INNER JOIN ParLevel3Level2Level1 P321 (nolock)
                                ON P321.ParLevel3Level2_Id = P32.Id
                             INNER JOIN ParLevel2 PL2 (nolock)
                                ON PL2.Id = P32.ParLevel2_Id
                             WHERE P321.ParLevel1_Id = '{ ParLevel1.ParLevel1_Id }'  and P321.Active = 1
                             GROUP BY PL2.Id, PL2.Name";

                List<ParLevel2Evaluate> parEvaluate = new List<ParLevel2Evaluate>();

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    parEvaluate = factory.SearchQuery<ParLevel2Evaluate>(sql);
                }

                return parEvaluate;

            }
            else if (ParLevel1.hashKey == 1 && ParCompany_Id != null)
            {

                string sql = $@"SELECT '{ ParLevel1.ParCluster_Id + SyncServices.quebraProcesso }' + CAST(PL2.Id AS VARCHAR) AS Id,
                             { ParLevel1.ParCluster_Id } AS ParCluster_Id, PL2.Name AS Name,
                             (SELECT TOP 1 Avaliacoes FROM VolumePcc1b (nolock)  WHERE Data = (
                                        SELECT MAX(DATA) FROM VolumePcc1b (nolock)  WHERE ParCompany_id = { ParCompany_Id } 
                                        AND (Shift_Id = {Shift_Id} OR Shift_Id IS NULL)
                                        AND CAST(DATA AS DATE) <= '{ date }') 
                                and ParCompany_id = { ParCompany_Id } 
                                AND (Shift_Id = {Shift_Id} OR Shift_Id IS NULL)
                                ORDER BY Shift_Id DESC) AS Evaluate 
                             FROM ParLevel3Level2 P32 (nolock)
                             INNER JOIN ParLevel3Level2Level1 P321 (nolock)
                                ON P321.ParLevel3Level2_Id = P32.Id 
                             INNER JOIN ParLevel2 PL2 (nolock) 
                                ON PL2.Id = P32.ParLevel2_Id 
                             WHERE P321.ParLevel1_Id = '{ ParLevel1.ParLevel1_Id }'and P321.Active = 1 
                             GROUP BY PL2.Id, PL2.Name";

                List<ParLevel2Evaluate> parEvaluate = new List<ParLevel2Evaluate>();

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    parEvaluate = factory.SearchQuery<ParLevel2Evaluate>(sql);
                }

                return parEvaluate;
            }
            else
            {

                if (ParCompany_Id > 0)
                {
                    queryCompany = " AND (PE.ParCompany_Id = '" + ParCompany_Id + "') ";
                }
                else
                {
                    queryCompany = " AND PE.ParCompany_Id IS NULL ";
                }

                string queryLevel1 = " AND (PE.ParLevel1_Id = '" + ParLevel1.ParLevel1_Id + "' OR PE.ParLevel1_Id IS NULL) ";

                string sql = $@"SELECT '{ ParLevel1.ParCluster_Id + SyncServices.quebraProcesso }' + CAST(PL2.Id AS VARCHAR) AS Id, 
                    {ParLevel1.ParCluster_Id } AS ParCluster_Id, PL2.Name AS Name, +
                    PE.Number AS Evaluate FROM                                                                      
                                ParLevel3Level2 P32 (nolock)                                           
                                INNER JOIN ParLevel3Level2Level1 P321 (nolock)                          
                                    ON P321.ParLevel3Level2_Id = P32.Id and p321.Active = 1                                      
                                INNER JOIN ParLevel2 PL2 (nolock)                                       
                                    ON PL2.Id = P32.ParLevel2_Id                                              
                                INNER JOIN ParEvaluation PE (nolock)                                    
                                    ON PE.ParLevel2_Id = PL2.Id                                               
                                WHERE P321.ParLevel1_Id = '{ ParLevel1.ParLevel1_Id }' 
                                AND PE.IsActive = 1 
                                AND PE.ParCluster_Id = { ParLevel1.ParCluster_Id }
                                { queryCompany }
                                { queryLevel1 }
                             GROUP BY PL2.Id, PL2.Name, PE.Number, PE.AlterDate, PE.AddDate, PE.ParCompany_Id              
                             ORDER BY PE.ParCompany_Id  DESC, PE.AlterDate, PE.AddDate";

                List<ParLevel2Evaluate> parEvaluate = new List<ParLevel2Evaluate>();

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    parEvaluate = factory.SearchQuery<ParLevel2Evaluate>(sql);
                }

                return parEvaluate;

            }
        }
    }

    public partial class ParLevel2Sample
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Sample { get; set; }
        public int ParCluster_Id { get; set; }
        private SqlConnection db { get; set; }

        public ParLevel2Sample() { }

        public ParLevel2Sample(SqlConnection _db)
        {
            db = _db;
        }

        public IEnumerable<ParLevel2Sample> getSample(ParLevel1 ParLevel1, int? ParCompany_Id, DateTime DateCollection, int Shift_Id)
        {

            var date = DateCollection.ToString("yyyy-MM-dd");

            string queryCompany = null;

            if (ParLevel1.hashKey == 2 && ParCompany_Id != null)
            {

                string sql = $@"SELECT '{ ParLevel1.ParCluster_Id + SyncServices.quebraProcesso }' + CAST(PL2.Id AS VARCHAR)  AS Id,
                            { ParLevel1.ParCluster_Id } AS ParCluster_Id, 
                            PL2.Name AS Name,          
                            (SELECT TOP 1 Amostras 
                                   FROM VolumeCepDesossa (nolock)  
                                   WHERE Data = (SELECT MAX(DATA) 
                                                FROM VolumeCepDesossa (nolock)  
                                                WHERE ParCompany_id = { ParCompany_Id }
                                                AND (Shift_Id = {Shift_Id} OR Shift_Id IS NULL)
                                                AND CAST(DATA AS DATE) <= '{ date }') 
                                    and ParCompany_id = { ParCompany_Id } 
                                    AND (Shift_Id = {Shift_Id} OR Shift_Id IS NULL)
                                    ORDER BY Shift_Id DESC) AS Sample
                            FROM ParLevel3Level2 P32 (nolock)                            
                            INNER JOIN ParLevel3Level2Level1 P321 (nolock)   
                                ON P321.ParLevel3Level2_Id = P32.Id and p321.Active = 1                                     
                            INNER JOIN ParLevel2 PL2 (nolock)                       
                                ON PL2.Id = P32.ParLevel2_Id                        
                            WHERE P321.ParLevel1_Id = '{ ParLevel1.ParLevel1_Id }'                         
                            GROUP BY PL2.Id, PL2.Name";

                List<ParLevel2Sample> parSample = new List<ParLevel2Sample>();

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    parSample = factory.SearchQuery<ParLevel2Sample>(sql);
                }

                return parSample;

            }
            else if (ParLevel1.hashKey == 3 && ParCompany_Id != null)
            {

                string sql = $@"SELECT '{ ParLevel1.ParCluster_Id + SyncServices.quebraProcesso }' + CAST(PL2.Id AS VARCHAR)  AS Id,
                    { ParLevel1.ParCluster_Id } AS ParCluster_Id, 
                             PL2.Name AS Name, 
                             (SELECT TOP 1  Amostras FROM VolumeVacuoGRD (nolock) WHERE Data = (
                                        SELECT MAX(DATA) FROM VolumeVacuoGRD (nolock)  
                                        WHERE ParCompany_id = { ParCompany_Id } 
                                        AND (Shift_Id = {Shift_Id} OR Shift_Id IS NULL)
                                        AND CAST(DATA AS DATE) <= '{ date }') 
                                and ParCompany_id = { ParCompany_Id }
                                AND (Shift_Id = {Shift_Id} OR Shift_Id IS NULL)
                                ORDER BY Shift_Id DESC) AS Sample 
                             FROM ParLevel3Level2 P32 (nolock)
                             INNER JOIN ParLevel3Level2Level1 P321 (nolock)
                                ON P321.ParLevel3Level2_Id = P32.Id    and p321.Active = 1
                             INNER JOIN ParLevel2 PL2 (nolock)
                                ON PL2.Id = P32.ParLevel2_Id
                             WHERE P321.ParLevel1_Id = '{ ParLevel1.ParLevel1_Id }'
                             GROUP BY PL2.Id, PL2.Name";

                List<ParLevel2Sample> parSample = new List<ParLevel2Sample>();

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    parSample = factory.SearchQuery<ParLevel2Sample>(sql);
                }

                return parSample;


            }
            else if (ParLevel1.hashKey == 4 && ParCompany_Id != null)
            {

                string sql = $@"SELECT '{ ParLevel1.ParCluster_Id + SyncServices.quebraProcesso }' + CAST(PL2.Id AS VARCHAR) AS Id,
                    { ParLevel1.ParCluster_Id } AS ParCluster_Id, PL2.Name AS Name,
                             (SELECT  TOP 1 Amostras FROM VolumeCepRecortes (nolock) WHERE Data = (
                                        SELECT MAX(DATA) FROM VolumeCepRecortes (nolock)  
                                        WHERE ParCompany_id = { ParCompany_Id } 
                                        AND (Shift_Id = {Shift_Id} OR Shift_Id IS NULL)
                                        AND CAST(DATA AS DATE) <= '{ date }') 
                                and ParCompany_id = { ParCompany_Id } 
                                AND (Shift_Id = {Shift_Id} OR Shift_Id IS NULL)
                                ORDER BY Shift_Id DESC) AS Sample
                             FROM ParLevel3Level2 P32 (nolock)
                             INNER JOIN ParLevel3Level2Level1 P321 (nolock)
                                ON P321.ParLevel3Level2_Id = P32.Id and p321.Active = 1
                             INNER JOIN ParLevel2 PL2  (nolock)
                                ON PL2.Id = P32.ParLevel2_Id
                             WHERE P321.ParLevel1_Id = '{ ParLevel1.ParLevel1_Id }'
                             GROUP BY PL2.Id, PL2.Name";

                List<ParLevel2Sample> parSample = new List<ParLevel2Sample>();

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    parSample = factory.SearchQuery<ParLevel2Sample>(sql);
                }

                return parSample;

            }
            else if (ParLevel1.hashKey == 1 && ParCompany_Id != null)
            {

                string sql = $@"SELECT '{ ParLevel1.ParCluster_Id + SyncServices.quebraProcesso }' + CAST(PL2.Id AS VARCHAR)  AS Id,
                    { ParLevel1.ParCluster_Id } AS ParCluster_Id, 
                    PL2.Name AS Name, (SELECT TOP 1 Amostras FROM VolumePcc1b(nolock) 
                            WHERE Data = (
                                        SELECT MAX(DATA) FROM VolumePcc1b (nolock)  
                                        WHERE ParCompany_id = { ParCompany_Id } 
                                        AND (Shift_Id = {Shift_Id} OR Shift_Id IS NULL)
                                        AND CAST(DATA AS DATE) <= '{ date }') 
                            and ParCompany_id = { ParCompany_Id }
                            AND (Shift_Id = {Shift_Id} OR Shift_Id IS NULL)
                            ORDER BY Shift_Id DESC) AS Sample 
                            FROM ParLevel3Level2 P32 (nolock)
                            INNER JOIN ParLevel3Level2Level1 P321 (nolock)
                               ON P321.ParLevel3Level2_Id = P32.Id and p321.active = 1
                            INNER JOIN ParLevel2 PL2 (nolock)
                               ON PL2.Id = P32.ParLevel2_Id
                            WHERE P321.ParLevel1_Id = '{ ParLevel1.ParLevel1_Id }'
                            GROUP BY PL2.Id, PL2.Name";

                List<ParLevel2Sample> parSample = new List<ParLevel2Sample>();

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    parSample = factory.SearchQuery<ParLevel2Sample>(sql);
                }

                return parSample;

            }
            else
            {

                if (ParCompany_Id > 0)
                {
                    queryCompany = " AND PS.ParCompany_Id = '" + ParCompany_Id + "'";
                }
                else
                {
                    queryCompany = " AND PS.ParCompany_Id  IS NULL ";
                }

                string queryLevel1 = " AND (PS.ParLevel1_Id = '" + ParLevel1.ParLevel1_Id + "' OR PS.ParLevel1_Id IS NULL) ";

                string sql = $@"SELECT '{ ParLevel1.ParCluster_Id + SyncServices.quebraProcesso }' + CAST(PL2.Id AS VARCHAR) AS Id,
                            { ParLevel1.ParCluster_Id } AS ParCluster_Id, PL2.Name AS Name, PS.Number AS Sample FROM  
                             ParLevel3Level2 P32  (nolock)                                    
                             INNER JOIN ParLevel3Level2Level1 P321 (nolock)                 
                                ON P321.ParLevel3Level2_Id = P32.Id  and p321.active = 1                            
                             INNER JOIN ParLevel2 PL2 (nolock)                            
                                ON PL2.Id = P32.ParLevel2_Id                                     
                             INNER JOIN ParSample PS (nolock)                             
                                ON PS.ParLevel2_Id = PL2.Id                                      
                             WHERE P321.ParLevel1_Id = '{ ParLevel1.ParLevel1_Id }' AND PS.ParCluster_Id = { ParLevel1.ParCluster_Id } AND PS.IsActive = 1 
                             { queryCompany }
                             { queryLevel1 }
                             GROUP BY PL2.Id, PL2.Name, PS.Number, PS.ParCompany_Id, PS.AlterDate, PS.AddDate, PS.ParCompany_Id          
                             ORDER BY PS.ParCompany_Id desc, PS.AlterDate DESC, PS.AddDate DESC";

                List<ParLevel2Sample> parSample = new List<ParLevel2Sample>();

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    parSample = factory.SearchQuery<ParLevel2Sample>(sql);
                }

                return parSample;
            }
        }
    }

    public partial class ParLevel3
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int ParLevel3Group_Id { get; set; }
        public string ParLevel3Group_Name { get; set; }
        public int ParLevel3InputType_Id { get; set; }
        public string ParLevel3InputType_Name { get; set; }
        public int ParLevel3BoolFalse_Id { get; set; }
        public string ParLevel3BoolFalse_Name { get; set; }
        public int ParLevel3BoolTrue_Id { get; set; }
        public string ParLevel3BoolTrue_Name { get; set; }
        public decimal IntervalMin { get; set; }
        public decimal IntervalMax { get; set; }
        public string ParMeasurementUnit_Name { get; set; }
        public string DynamicValue { get; set; }
        public decimal Weight { get; set; }
        public decimal PunishmentValue { get; set; }
        public decimal WeiEvaluation { get; set; }
        public int ParCompany_Id { get; set; }
        public bool HasTakePhoto { get; set; }

        private SqlConnection db { get; set; }
        public ParLevel3() { }
        public ParLevel3(SqlConnection _db)
        {
            db = _db;
        }

        public IEnumerable<ParLevel3> getList()
        {

            string sql = "SELECT Id, Name FROM ParLevel3 (nolock) ";

            List<ParLevel3> parLevel3List = new List<ParLevel3>();

            using (Factory factory = new Factory("DefaultConnection"))
            {
                parLevel3List = factory.SearchQuery<ParLevel3>(sql);
            }

            return parLevel3List;

        }

        public IEnumerable<ParLevel3> getListPerLevel1Id(int ParLevel1_Id)
        {

            string sql = "SELECT P3.Id, P3.Name FROM ParLevel3Level2Level1 P321  (nolock) INNER JOIN ParLevel3Level2 P32 (nolock)  ON P32.Id = P321.ParLevel3Level2_Id INNER JOIN ParLevel3 P3 (nolock)  ON P3.Id = P32.ParLevel3_Id WHERE  p321.active = 1 and  P321.ParLevel1_Id = " + ParLevel1_Id.ToString();

            List<ParLevel3> parLevel3List = new List<ParLevel3>();

            using (Factory factory = new Factory("DefaultConnection"))
            {
                parLevel3List = factory.SearchQuery<ParLevel3>(sql);
            }

            return parLevel3List;

        }

        public IEnumerable<ParLevel3> getLevel3ByLevel2(SGQDBContext.ParLevel1 ParLevel1, SGQDBContext.ParLevel2 ParLevel2, int ParCompany_Id, DateTime DateCollect)
        {

            //Instanciamos variavel de data
            string dataInicio = null;
            string dataFim = null;

            //Pega a data pela regra da frequencia
            SyncServices.getFrequencyDate(ParLevel2.ParFrequency_Id, DateCollect, ref dataInicio, ref dataFim);

            string sql = "\n SELECT L3.Id AS Id, L3.Name AS Name, L3G.Id AS ParLevel3Group_Id, L3G.Name AS ParLevel3Group_Name, L3IT.Id AS ParLevel3InputType_Id, L3IT.Name AS ParLevel3InputType_Name, L3V.ParLevel3BoolFalse_Id AS ParLevel3BoolFalse_Id, L3BF.Name AS ParLevel3BoolFalse_Name, L3V.ParLevel3BoolTrue_Id AS ParLevel3BoolTrue_Id, L3BT.Name AS ParLevel3BoolTrue_Name, " +
                         "\n ISNULL(L3V.IntervalMin, -9999999999999.9) AS IntervalMin, ISNULL(L3V.IntervalMax, 9999999999999.9) AS IntervalMax, MU.Name AS ParMeasurementUnit_Name, L32.Weight AS Weight, L3V.ParCompany_Id  AS ParCompany_id1 , L32.ParCompany_Id AS ParCompany_id2, L3V.DynamicValue, L3.HasTakePhoto                                                                                                                                                                                                                                       " +
                         "\n FROM ParLevel3 L3      (nolock)                                                                                                                                                                                                                                                                                                                                       " +
                         "\n INNER JOIN ParLevel3Value L3V      (nolock)                                                                                                                                                                                                                                                                                                                           " +
                         "\n         ON L3V.Id = (SELECT top 1 id FROM ParLevel3Value  (nolock) where isactive = 1 and ParLevel3_id = L3.Id and (ParCompany_id =  " + ParCompany_Id + " or ParCompany_id is null) and (ParLevel1_id =  " + ParLevel1.ParLevel1_Id + " or ParLevel1_id is null) and (ParLevel2_id =  " + ParLevel2.ParLevel2_id + " or ParLevel2_id is null) order by ParCompany_Id desc) " +
                         "\n INNER JOIN ParLevel3InputType L3IT    (nolock)                                                                                                                                                                                                                                                                                                                        " +
                         "\n         ON L3IT.Id = L3V.ParLevel3InputType_Id                                                                                                                                                                                                                                                                                                              " +
                         "\n LEFT JOIN ParLevel3BoolFalse L3BF      (nolock)                                                                                                                                                                                                                                                                                                                       " +
                         "\n         ON L3BF.Id = L3V.ParLevel3BoolFalse_Id                                                                                                                                                                                                                                                                                                              " +
                         "\n LEFT JOIN ParLevel3BoolTrue L3BT    (nolock)                                                                                                                                                                                                                                                                                                                          " +
                         "\n         ON L3BT.Id = L3V.ParLevel3BoolTrue_Id                                                                                                                                                                                                                                                                                                               " +
                         "\n LEFT JOIN ParMeasurementUnit MU   (nolock)                                                                                                                                                                                                                                                                                                                            " +
                         "\n         ON MU.Id = L3V.ParMeasurementUnit_Id                                                                                                                                                                                                                                                                                                                " +
                         "\n LEFT JOIN ParLevel3Level2 L32    (nolock)                                                                                                                                                                                                                                                                                                                             " +
                         "\n         ON L32.ParLevel3_Id = L3.Id                                                                                                                                                                                                                                                                                                                         " +
                         "\n LEFT JOIN ParLevel3Group L3G   (nolock)                                                                                                                                                                                                                                                                                                                               " +
                         "\n         ON L3G.Id = L32.ParLevel3Group_Id                                                                                                                                                                                                                                                                                                                   " +
                         "\n INNER JOIN ParLevel2 L2     (nolock)                                                                                                                                                                                                                                                                                                                                  " +
                         "\n         ON L2.Id = L32.ParLevel2_Id                                                                                                                                                                                                                                                                                                                         " +
                         "\n INNER JOIN ParLevel3Level2Level1 AS L321  (nolock) ON L321.ParLevel3Level2_Id = L32.Id                                                                                                                                                                                                                                                                                 " +
                         "\n WHERE  L3.IsActive = 1 AND L32.IsActive = 1       and L321.active = 1                                                                                                                                                                                                                                                                                                           " +
                         "\n  AND L2.Id = '" + ParLevel2.ParLevel2_id + "' " +
                         "\n  AND(L32.ParCompany_Id = '" + ParCompany_Id + "' OR L32.ParCompany_Id IS NULL) " +
                         "\n  AND(L3V.ParCompany_Id = '" + ParCompany_Id + "' OR L3V.ParCompany_Id IS NULL) " +
                         "\n  AND L321.ParLevel1_Id='" + ParLevel1.ParLevel1_Id + "'                                                                                                        " +

                         "\n  GROUP BY " +
                            "\n    L321.ParLevel1_Id " +
                            "\n  , L2.Id " +
                            "\n  , L3G.Name " +
                            "\n  , L3.Name " +
                            "\n  , L3.Id " +
                            "\n  , L3G.Id " +
                            "\n  , L3IT.Id " +
                            "\n  , L3IT.Name " +
                            "\n  , L3V.ParLevel3BoolFalse_Id " +
                            "\n  , L3BF.Name " +
                            "\n  , L3V.ParLevel3BoolTrue_Id " +
                            "\n  , L3BT.Name " +
                            "\n  , L3V.IntervalMin " +
                            "\n  , L3V.IntervalMax " +
                            "\n  , MU.Name " +
                            "\n  , L32.Weight " +
                            "\n  , L3V.ParCompany_Id " +
                            "\n  , L32.ParCompany_Id " +
                            "\n  , L3.HasTakePhoto " +
                            "\n  , L3V.DynamicValue ";


            /*
             * MOCK GABRIEL PARA TESTE DE TRAZER TAREFAS DO OUTRO INDICADOR
             * 30/03/2017
             */

            string possuiIndicadorFilho = "SELECT cast(id as varchar(153)) as retorno FROM ParLevel1(nolock) WHERE ParLevel1Origin_Id = " + ParLevel1.ParLevel1_Id.ToString();
            string ParLevel1Origin_Id = "";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                var list = factory.SearchQuery<ResultadoUmaColuna>(possuiIndicadorFilho);
                for (var i = 0; i < list.Count(); i++)
                {
                    ParLevel1Origin_Id += list[i].retorno.ToString() + ", ";
                }
            }

            ParLevel1Origin_Id += "null";

            string sqlFilho = "";

            string sqlPeso = "L32.Weight";


            if (ParLevel1Origin_Id != "null")
            {
                string IndicadorFilhoPeso = "SELECT cast(PointsDestiny as varchar(3)) as retorno FROM ParLevel1  (nolock) WHERE ParLevel1Origin_Id = " + ParLevel1.ParLevel1_Id.ToString();

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    var list = factory.SearchQuery<ResultadoUmaColuna>(IndicadorFilhoPeso).ToList();

                    for (var i = 0; i < list.Count(); i++)
                    {
                        IndicadorFilhoPeso = list[i].retorno.ToString();
                    }
                }

                if (IndicadorFilhoPeso == "0")
                {
                    sqlPeso = "0";
                }

                string ParLevel1_IdFilho = "\n  AND L321.ParLevel1_Id IN (" + ParLevel1Origin_Id + ") \n  AND L2.Id = '" + ParLevel2.ParLevel2_id + "'";

                sqlFilho = " \n UNION ALL SELECT L3.Id AS Id, L3.Name AS Name, L3G.Id AS ParLevel3Group_Id, L3G.Name AS ParLevel3Group_Name, L3IT.Id AS ParLevel3InputType_Id, L3IT.Name AS ParLevel3InputType_Name, L3V.ParLevel3BoolFalse_Id AS ParLevel3BoolFalse_Id, L3BF.Name AS ParLevel3BoolFalse_Name, L3V.ParLevel3BoolTrue_Id AS ParLevel3BoolTrue_Id, L3BT.Name AS ParLevel3BoolTrue_Name, " +
                        "\n  ISNULL(L3V.IntervalMin, -9999999999999.9) AS IntervalMin, ISNULL(L3V.IntervalMax, 9999999999999.9) AS IntervalMax, MU.Name AS ParMeasurementUnit_Name, " + sqlPeso + " AS Weight, L3V.ParCompany_Id AS ParCompany_id1 , L32.ParCompany_Id AS ParCompany_id2, L3V.DynamicValue, L3.HasTakePhoto " +
                        "\n FROM ParLevel3 L3     (nolock)                                                                                                                                                                                                                                                                                                                                        " +
                        "\n INNER JOIN ParLevel3Value L3V     (nolock)                                                                                                                                                                                                                                                                                                                            " +
                        "\n         ON L3V.Id = (SELECT top 1 id FROM ParLevel3Value  (nolock) where isactive = 1 and ParLevel3_id = L3.Id and (ParCompany_id = " + ParCompany_Id + " or ParCompany_id is null) order by ParCompany_Id desc)                                                                                                                                                                                                                                                                                                                       " +
                        "\n INNER JOIN ParLevel3InputType L3IT  (nolock)                                                                                                                                                                                                                                                                                                                          " +
                        "\n         ON L3IT.Id = L3V.ParLevel3InputType_Id                                                                                                                                                                                                                                                                                                              " +
                        "\n LEFT JOIN ParLevel3BoolFalse L3BF   (nolock)                                                                                                                                                                                                                                                                                                                          " +
                        "\n         ON L3BF.Id = L3V.ParLevel3BoolFalse_Id                                                                                                                                                                                                                                                                                                              " +
                        "\n LEFT JOIN ParLevel3BoolTrue L3BT    (nolock)                                                                                                                                                                                                                                                                                                                          " +
                        "\n         ON L3BT.Id = L3V.ParLevel3BoolTrue_Id                                                                                                                                                                                                                                                                                                               " +
                        "\n LEFT JOIN ParMeasurementUnit MU   (nolock)                                                                                                                                                                                                                                                                                                                            " +
                        "\n         ON MU.Id = L3V.ParMeasurementUnit_Id                                                                                                                                                                                                                                                                                                                " +
                        "\n LEFT JOIN ParLevel3Level2 L32    (nolock)                                                                                                                                                                                                                                                                                                                             " +
                        "\n         ON L32.ParLevel3_Id = L3.Id                                                                                                                                                                                                                                                                                                                         " +
                        "\n LEFT JOIN ParLevel3Group L3G  (nolock)                                                                                                                                                                                                                                                                                                                                " +
                        "\n         ON L3G.Id = L32.ParLevel3Group_Id                                                                                                                                                                                                                                                                                                                   " +
                        "\n INNER JOIN ParLevel2 L2     (nolock)                                                                                                                                                                                                                                                                                                                                  " +
                        "\n         ON L2.Id = L32.ParLevel2_Id                                                                                                                                                                                                                                                                                                                         " +
                        "\n INNER JOIN ParLevel3Level2Level1 AS L321  (nolock) ON L321.ParLevel3Level2_Id = L32.Id                                                                                                                                                                                                                                                                                 " +
                        "\n WHERE  L3.IsActive = 1 AND L32.IsActive = 1   and L321.active = 1                                                                                                                                                                                                                                                                                                               " +

                        "\n  AND(L32.ParCompany_Id = '" + ParCompany_Id + "' OR L32.ParCompany_Id IS NULL) " +
                        "\n  AND(L3V.ParCompany_Id = '" + ParCompany_Id + "' OR L3V.ParCompany_Id IS NULL) \n " +
                        ParLevel1_IdFilho +


                        //queryResult + 


                        "\n  GROUP BY " +
           "\n    L321.ParLevel1_Id " +
           "\n  , L2.Id " +
           "\n  , L3G.Name " +
           "\n  , L3.Name " +
           "\n  , L3.Id " +
           "\n  , L3G.Id " +
           "\n  , L3IT.Id " +
           "\n  , L3IT.Name " +
           "\n  , L3V.ParLevel3BoolFalse_Id " +
           "\n  , L3BF.Name " +
           "\n  , L3V.ParLevel3BoolTrue_Id " +
           "\n  , L3BT.Name " +
           "\n  , L3V.IntervalMin " +
           "\n  , L3V.IntervalMax " +
           "\n  , MU.Name " +
           "\n  , L32.Weight " +
           "\n  , L3V.ParCompany_Id " +
           "\n  , L32.ParCompany_Id " +
           "\n  , L3.HasTakePhoto " +
           "\n  , L3V.DynamicValue ";



            }

            sql += sqlFilho;
            //ORDENAR POR TIPO DE INPUT

            //ORDENAR POR ORDEM ALFABÉTICA

            sql = "SELECT * FROM (" + sql;

            sql += "\n  ) TOTAL  ORDER BY ISNULL(TOTAL.ParLevel3Group_Name,'ZZZ') ASC, 2 ASC,  15  DESC , 16  DESC    ";

            List<ParLevel3> parLevel3List = new List<ParLevel3>();
            using (Factory factory = new Factory("DefaultConnection"))
            {
                parLevel3List = factory.SearchQuery<ParLevel3>(sql);
            }

            return parLevel3List;
        }
        /// <summary>
        /// Recupera as tarefas que foram feitas no Level2
        /// </summary>
        /// <param name="ParLevel1">Indicador</param>
        /// <param name="ParLevel2">Monitoramento</param>
        /// <param name="ParCompany_Id">Company</param>
        /// <param name="DateCollect">Data Coleta</param>
        /// <returns></returns>
        public IEnumerable<ParLevel3> getLevel3InLevel2(SGQDBContext.ParLevel1 ParLevel1, SGQDBContext.ParLevel2 ParLevel2, int ParCompany_Id, DateTime DateCollect)
        {
            try
            {

                //Instanciamos variavel de data
                string dataInicio = null;
                string dataFim = null;

                //Pega a data pela regra da frequencia
                SyncServices.getFrequencyDate(ParLevel2.ParFrequency_Id, DateCollect, ref dataInicio, ref dataFim);

                string sql = "SELECT " +
                             "R3.ParLevel3_Id AS Id " +
                             "FROM CollectionLevel2 C2 (nolock)  " +
                             "INNER JOIN ParLevel1 L1  (nolock) " +
                             "ON C2.ParLevel1_Id = L1.Id AND L1.IsPartialSave = 1 " +
                             "INNER JOIN Result_Level3 R3  (nolock) " +
                             "ON R3.CollectionLevel2_Id = C2.Id " +
                             "WHERE C2.UnitId = '" + ParCompany_Id + "' AND L1.Id='" + ParLevel1.ParLevel1_Id + "' AND C2.ParLevel2_Id='" + ParLevel2.ParLevel2_id + "' AND C2.CollectionDate BETWEEN '" + dataInicio + " 00:00:00' AND '" + dataFim + " 23:59:59' ";

                List<ParLevel3> parLevel3List = new List<ParLevel3>();
                using (Factory factory = new Factory("DefaultConnection"))
                {
                    parLevel3List = factory.SearchQuery<ParLevel3>(sql);
                }

                return parLevel3List;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public partial class Level2Result
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public int ParLevel1_Id { get; set; }
        public int ParLevel2_Id { get; set; }
        public int Unit_Id { get; set; }
        public int Shift { get; set; }
        public int Period { get; set; }
        public DateTime CollectionDate { get; set; }
        public int EvaluateLast { get; set; }
        public int SampleLast { get; set; }
        public int ConsolidationLevel2_Id { get; set; }

        private SqlConnection db { get; set; }
        public Level2Result() { }
        public Level2Result(SqlConnection _db)
        {
            db = _db;
        }

        public IEnumerable<Level2Result> getList(int ParLevel1_Id, int ParCompany_Id, string dataInicio, string dataFim)
        {

            string sql = "SELECT ParLevel1_Id, ParLevel2_Id, UnitId AS Unit_Id, Shift, Period, CollectionDate, MAX(EvaluationNumber) AS EvaluateLast, MAX(Sample) AS SampleLast, MAX(ConsolidationLevel2_Id) AS ConsolidationLevel2_Id " +
                         "FROM(SELECT CL2.ParLevel1_Id, CL2.ParLevel2_Id, CL2.UnitId, Shift, Period, CONVERT(date, CollectionDate) AS CollectionDate, EvaluationNumber, MAX(Sample) AS Sample, MAX(ConsolidationLevel2_Id) AS ConsolidationLevel2_Id " +
                         "FROM CollectionLevel2 CL2 (nolock)  " +
                         "INNER JOIN ConsolidationLevel2 CDL2 (nolock)  ON CL2.ConsolidationLevel2_Id = CDL2.ID " +
                         "INNER JOIN ConsolidationLevel1 CDL1 (nolock)  ON CDL2.ConsolidationLevel1_Id = CDL1.Id " +
                         "WHERE(CDL1.ParLevel1_Id = '" + ParLevel1_Id + "' AND CDL1.UnitId = '" + ParCompany_Id + "' AND CDL1.ConsolidationDate BETWEEN '" + dataInicio + " 00:00:00' AND '" + dataFim + " 23:59:59') " +
                         "GROUP BY CL2.ParLevel1_Id, CL2.ParLevel2_Id, CL2.UnitId, Shift, Period, CONVERT(date, CollectionDate), EvaluationNumber, ConsolidationLevel2_Id) AS ultimas_amostras " +
                         "GROUP BY ParLevel1_Id, ParLevel2_Id, UnitId, Shift, Period, CollectionDate, ConsolidationLevel2_Id ";

            List<Level2Result> Level2ResultList = new List<Level2Result>();
            using (Factory factory = new Factory("DefaultConnection"))
            {
                Level2ResultList = factory.SearchQuery<Level2Result>(sql);
            }

            return Level2ResultList;

        }
        public int getMaxSampe(int ConsolidationLevel2_Id, int EvaluationNumber)
        {
            try
            {
                string sql = "SELECT MAX(Sample) FROM CollectionLevel2 (nolock)  WHERE ConsolidationLevel2_Id = " + ConsolidationLevel2_Id + " AND EvaluationNumber = " + EvaluationNumber;
                var LastSample = db.Query<int>(sql).FirstOrDefault();
                return LastSample;
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }
        }
        public IEnumerable<Level2Result> getKeys(int ParLevel1_Id, int ParCompany_Id, string dataInicio, string dataFim)
        {

            string sql = "SELECT CL2.Id, CL2.ParLevel1_Id, CL2.ParLevel2_Id, CL2.UnitId, CL2.Shift, CL2.Period, CL2.EvaluationNumber, CL2.Sample, CL2.ConsolidationLevel2_Id, CL2.[Key] " +
                         "FROM CollectionLevel2 CL2  (nolock) " +
                         "WHERE CL2.ParLevel1_Id = '" + ParLevel1_Id + "' AND CL2.UnitId = '" + ParCompany_Id + "' AND CL2.CollectionDate BETWEEN '" + dataInicio + " 00:00:00' AND '" + dataFim + " 23:59:59' ";

            List<Level2Result> Level2ResultList = new List<Level2Result>();

            using (Factory factory = new Factory("DefaultConnection"))
            {
                Level2ResultList = factory.SearchQuery<Level2Result>(sql);
            }

            return Level2ResultList;
        }

    }

    public partial class ParLevel1ConsolidationXParFrequency
    {
        public int ParLevel1_Id { get; set; }
        public int ParFrequency_Id { get; set; }
        public bool IsPartialSave { get; set; }
        public int Id { get; set; }

        private SqlConnection db { get; set; }
        public ParLevel1ConsolidationXParFrequency() { }
        public ParLevel1ConsolidationXParFrequency(SqlConnection _db)
        {
            db = _db;
        }

        public IEnumerable<ParLevel1ConsolidationXParFrequency> getList(int ParCompany_Id, DateTime data)
        {
            try
            {

                DateTime data_ini = new DateTime(data.Year, data.Month, 1);
                DateTime data_fim = new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month));


                string sql = "SELECT CDL1.Id, CDL1.ParLevel1_Id, PL1.ParFrequency_Id, PL1.IsPartialSave FROM ConsolidationLevel1 CDL1 (nolock)  " +
                             "INNER JOIN ParLevel1 PL1 (nolock)  ON CDL1.ParLevel1_Id = PL1.Id WHERE CDL1.UnitId = '" + ParCompany_Id + "'" +
                             " AND CDL1.Consolidationdate BETWEEN '" + data_ini.ToString("yyyyMMdd") + " 00:00' and '" + data_fim.ToString("yyyyMMdd") + "  23:59:59'" +
                             " AND PL1.IsActive = 1" +
                             " GROUP BY CDL1.Id, CDL1.ParLevel1_Id, PL1.ParFrequency_Id,  PL1.IsPartialSave";

                List<ParLevel1ConsolidationXParFrequency> consolidation = new List<ParLevel1ConsolidationXParFrequency>();
                using (Factory factory = new Factory("DefaultConnection"))
                {
                    consolidation = factory.SearchQuery<ParLevel1ConsolidationXParFrequency>(sql);
                }

                return consolidation;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }

    public partial class ConsolidationResultL1L2
    {
        public int AlertLevelL1 { get; set; }
        public decimal WeiEvaluationL1 { get; set; }
        public decimal EvaluateTotalL1 { get; set; }
        public decimal DefectsTotalL1 { get; set; }
        public decimal WeiDefectsL1 { get; set; }
        public decimal TotalLevel3EvaluationL1 { get; set; }
        public decimal TotalLevel3WithDefectsL1 { get; set; }
        public int LastEvaluationAlertL1 { get; set; }
        public int LastLevel2AlertL1 { get; set; }

        public int AlertLevelL2 { get; set; }

        public decimal WeiEvaluationL2 { get; set; }
        public decimal DefectsL2 { get; set; }
        public decimal WeiDefectsL2 { get; set; }
        public int TotalLevel3WithDefectsL2 { get; set; }
        public int TotalLevel3EvaluationL2 { get; set; }

        public int EvaluatedResultL1 { get; set; }
        public int DefectsResultL1 { get; set; }

        public int EvaluateTotalL2 { get; set; }
        public int DefectsTotalL2 { get; set; }

        public int EvaluatedResultL2 { get; set; }
        public int DefectsResultL2 { get; set; }
        public bool haveCorrectiveAction { get; set; }
        public bool haveReaudit { get; set; }
        public int ReauditLevel { get; set; }
        public int ReauditNumber { get; set; }
        public bool IsReaudit { get; set; }
        public int CollectionLevel2_ID_CorrectiveAction { get; set; }
        public int CollectionLevel2_Period_CorrectiveAction { get; set; }
        public int More3DefectsEvaluate { get; set; }
        public int Phase { get; set; }
        public DateTime StartPhaseDate { get; set; }
        public int StartPhaseEvaluation { get; set; }
        private SqlConnection db { get; set; }
        public ConsolidationResultL1L2() { }
        public ConsolidationResultL1L2(SqlConnection _db)
        {
            db = _db;
        }

        public ConsolidationResultL1L2 getConsolidation(int ParLevel2_Id, int ParCompany_Id, int? Id)
        {
            var sql2 = "";

            if (Id != null)
            {
                sql2 = " AND CDL1.Id = " + Id;
            }

            string sql = "SELECT " +
                         "CDL1.AtualAlert AS AlertLevelL1, CDL1.WeiEvaluation AS WeiEvaluationL1, CDL1.EvaluateTotal AS EvaluateTotalL1, CDL1.DefectsTotal AS DefectsTotalL1, CDL1.WeiDefects AS WeiDefectsL1, CDL1.TotalLevel3Evaluation AS TotalLevel3EvaluationL1, CDL1.TotalLevel3WithDefects AS TotalLevel3WithDefectsL1, CDL1.LastEvaluationAlert AS LastEvaluationAlertL1, CDL1.LastLevel2Alert AS LastLevel2AlertL1, CDL1.EvaluatedResult AS EvaluatedResultL1, CDL1.DefectsResult AS DefectsResultL1, " +
                         "CDL2.AlertLevel AS AlertLevelL2, CDL2.WeiEvaluation AS WeiEvaluationL2, CDL2.DefectsTotal AS DefectsL2, CDL2.WeiDefects AS WeiDefectsL2, CDL2.TotalLevel3WithDefects AS TotalLevel3WithDefectsL2, CDL2.TotalLevel3Evaluation AS TotalLevel3EvaluationL2, CDL2.EvaluateTotal AS EvaluateTotalL2, CDL2.DefectsTotal AS DefectsTotalL2, CDL2.EvaluatedResult AS EvaluatedResultL2, CDL2.DefectsResult AS DefectsResultL2, CL2.HaveCorrectiveAction AS HaveCorrectiveAction, CL2.HaveReaudit AS HaveReaudit, CL2.ReauditIs AS ReauditIs, CL2.ReauditLevel AS ReauditLevel, CL2.ReauditNumber AS ReauditNumber, CL2.Phase AS Phase, CL2.StartPhaseDate AS StartPhaseDate, CL2.StartPhaseEvaluation AS StartPhaseEvaluation, MIN(CL2.Id) AS CollectionLevel2_ID_CorrectiveAction, MIN(CL2.Period) AS CollectionLevel2_Period_CorrectiveAction " +
                         "FROM ConsolidationLevel2 AS CDL2 (nolock)  " +
                         "INNER JOIN " +
                         "ConsolidationLevel1 AS CDL1 (nolock)  ON CDL2.ConsolidationLevel1_Id = CDL1.Id " +
                         "LEFT JOIN " +
                         "CollectionLevel2 CL2 (nolock)  ON CL2.ConsolidationLevel2_Id=CDL2.Id AND (CL2.HaveCorrectiveAction=1 OR CL2.HaveReaudit=1) AND CL2.ReauditIs = 0 " +
                         "WHERE(CDL2.ParLevel2_Id = " + ParLevel2_Id + ") AND (CDL1.UnitId = " + ParCompany_Id + ") " +

                         sql2 +

                         " GROUP BY CDL1.AtualAlert, CDL1.WeiEvaluation,CDL1.EvaluateTotal, CDL1.DefectsTotal, CDL1.WeiDefects,  CDL1.TotalLevel3Evaluation, CDL1.TotalLevel3WithDefects, CDL1.LastEvaluationAlert, CDL1.LastLevel2Alert, CDL1.EvaluatedResult, CDL1.DefectsResult, CDL2.AlertLevel, CDL2.WeiEvaluation, CDL2.DefectsTotal, CDL2.WeiDefects, CDL2.TotalLevel3WithDefects, CDL2.TotalLevel3Evaluation, CDL2.EvaluateTotal, CDL2.EvaluatedResult, CDL2.DefectsResult,  CL2.HaveCorrectiveAction, CL2.HaveReaudit, CL2.ReauditLevel, CL2.ReauditNumber, CL2.ReauditIs, CL2.Phase, CL2.StartPhaseDate, CL2.StartPhaseEvaluation";

            ConsolidationResultL1L2 consolidation = null;
            using (Factory factory = new Factory("DefaultConnection"))
            {
                consolidation = factory.SearchQuery<ConsolidationResultL1L2>(sql).FirstOrDefault();
            }

            return consolidation;
        }
    }

    public partial class ParLevelHeader
    {
        public int ParHeaderField_Id { get; set; }
        public string ParHeaderField_Name { get; set; }
        public string ParHeaderField_Description { get; set; }
        public int ParFieldType_Id { get; set; }
        public int IsRequired { get; set; }
        public bool LinkNumberEvaluetion { get; set; }
        public bool duplicate { get; set; }
        public string HeaderFieldGroup { get; set; }

        private SqlConnection db { get; set; }
        public ParLevelHeader() { }
        public ParLevelHeader(SqlConnection _db)
        {
            db = _db;
        }

        public IEnumerable<ParLevelHeader> getHeaderByLevel1(int ParLevel1_Id)
        {

            string sql = "SELECT PH.Id AS ParHeaderField_Id, PH.Name AS ParHeaderField_Name, PH.Description AS ParHeaderField_Description, PT.Id AS ParFieldType_Id, PH.LinkNumberEvaluetion AS LinkNumberEvaluetion, PH.IsRequired AS IsRequired, PH.duplicate, isnull(PL.HeaderFieldGroup,'-') as HeaderFieldGroup FROM ParLevel1XHeaderField PL (nolock)   " +
                         "\n LEFT JOIN ParHeaderField PH (nolock)  ON PH.Id = PL.ParHeaderField_Id                                                                                                                                    " +
                         "\n LEFT JOIN ParLevelDefiniton PD (nolock)  ON PH.ParLevelDefinition_Id = PD.Id                                                                                                                             " +
                         "\n LEFT JOIN ParFieldType PT (nolock)  ON PH.ParFieldType_Id = PT.Id                                                                                                                                        " +
                         "\n WHERE                                                                                                                                                                                          " +
                         "\n PD.Id = 1 AND                                                                                                                                                                                  " +
                         "\n PL.ParLevel1_Id = " + ParLevel1_Id + " AND                                                                                                                                                     " +
                         "\n PL.IsActive = 1 AND PH.IsActive = 1 AND PD.IsActive = 1                                                                                                                                        " +
                         "\n GROUP BY PH.Id, PH.Name, PT.Id, PH.IsRequired, PH.Description, PH.LinkNumberEvaluetion, ph.duplicate, PL.HeaderFieldGroup                                                                                                                             ";

            List<ParLevelHeader> parLevel3List = new List<ParLevelHeader>(); 

            using (Factory factory = new Factory("DefaultConnection"))
            {
                parLevel3List = factory.SearchQuery<ParLevelHeader>(sql).ToList();
            }

            return parLevel3List;
        }

        public IEnumerable<ParLevelHeader> getHeaderByLevel1Level2(int ParLevel1_Id, int ParLevel2_Id)
        {

            string sql = "SELECT PH.Id AS ParHeaderField_Id, PH.Name AS ParHeaderField_Name, PH.Description AS ParHeaderField_Description, PT.Id AS ParFieldType_Id, PH.LinkNumberEvaluetion AS LinkNumberEvaluetion, PH.IsRequired AS IsRequired, PH.duplicate, isnull(PL.HeaderFieldGroup,'-') as HeaderFieldGroup FROM ParLevel1XHeaderField PL  (nolock)  " +
                         "\n LEFT JOIN ParHeaderField PH  (nolock) ON PH.Id = PL.ParHeaderField_Id                                                                                                                                    " +
                         "\n LEFT JOIN ParLevelDefiniton PD  (nolock) ON PH.ParLevelDefinition_Id = PD.Id                                                                                                                             " +
                         "\n LEFT JOIN ParFieldType PT  (nolock) ON PH.ParFieldType_Id = PT.Id                                                                                                                                        " +
                         "\n WHERE                                                                                                                                                                                          " +
                         "\n PD.Id = 2 AND                                                                                                                                                                                  " +
                         "\n PL.ParLevel1_Id = " + ParLevel1_Id + " AND                                                                                                                                                     " +
                         "\n PL.IsActive = 1 AND PH.IsActive = 1 AND PD.IsActive = 1                                                                                                                                        " +
                         "\n GROUP BY PH.Id, PH.Name, PT.Id, PH.Description, PH.IsRequired, PH.LinkNumberEvaluetion, PH.duplicate, PL.HeaderFieldGroup;                                                                                                                             ";

            List<ParLevelHeader> parLevel3List = new List<ParLevelHeader>();

            using (Factory factory = new Factory("DefaultConnection"))
            {
                parLevel3List = factory.SearchQuery<ParLevelHeader>(sql).ToList();
            }

            return parLevel3List;
        }

        public bool isHeaderLeve2Exception(int ParLevel1_Id, int ParLevel2_Id, int HeaderField_Id)
        {
            string sql = "SELECT * FROM ParLevel2XHeaderField PHF (nolock)                                                    \n" +
                         "\n INNER JOIN ParHeaderField HF (nolock)  " +
                         "\n ON HF.Id = PHF.ParHeaderField_Id AND HF.IsActive = 1 " +
                         "\n WHERE PHF.ParLevel1_Id = " + ParLevel1_Id + "                                              \n" +
                         "\n AND PHF.ParLevel2_Id = " + ParLevel2_Id + "                                               \n" +
                         "\n AND PHF.ParHeaderField_Id = " + HeaderField_Id + "                                         \n" +
                         "\n AND PHF.IsActive = 1;                                                                    \n";

            List<ParLevelHeader> parLevel3List = new List<ParLevelHeader>();
            using (Factory factory = new Factory("DefaultConnection"))
            {
                parLevel3List = factory.SearchQuery<ParLevelHeader>(sql).ToList();
            }

            if (parLevel3List.Count() > 0)
                return true;
            else
                return false;
        }
    }

    public partial class ParFieldType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PunishmentValue { get; set; }
        public int IsDefaultOption { get; set; }

        private SqlConnection db { get; set; }
        public ParFieldType() { }
        public ParFieldType(SqlConnection _db)
        {
            db = _db;
        }

        public IEnumerable<ParFieldType> getMultipleValues(int ParHeaderField_Id)
        {

            string sql = "SELECT Id, Name, PunishmentValue, IsDefaultOption FROM ParMultipleValues (nolock)        " +
                         "WHERE ParHeaderField_Id = '" + ParHeaderField_Id + "' and IsActive = 1;        ";

            List<ParFieldType> multipleValues = new List<ParFieldType>();

            using (Factory factory = new Factory("DefaultConnection"))
            {
                multipleValues = factory.SearchQuery<ParFieldType>(sql).ToList();
            }

            return multipleValues;
        }

        public IEnumerable<ParFieldType> getIntegrationValues(int ParHeaderField_Id, string integracao, int ParCompany_Id)
        {
            string conexaoBR = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            db = new SqlConnection(conexaoBR);

            var sql = "SELECT null Id, null as Name, 0 as PunishmentValue, 0 as IsDefaultOption";

            var valores = integracao.Split('|');

            if (valores[0] == "Equipamento" || valores[0] == "Câmara" || valores[0] == "Ponto de Coleta" || valores[0] == "Detector de Metais" || valores[0] == "Setor" || valores[0] == "Tipo de Corte" || valores[0] == "Setores BPF")
            {
                var subtipo = "";

                if (string.IsNullOrEmpty(valores[1]))
                {
                    subtipo = "subtipo is null";
                }
                else
                {
                    subtipo = "subtipo = '" + valores[1] + "'";
                }

                sql = "\n SELECT Id, Nome as Name, 0 as PunishmentValue, 0 as IsDefaultOption " +
                             "\n FROM Equipamentos  With (nolock) " +
                             "\n WHERE (Tipo = '" + valores[0] + "' AND " + subtipo + ") " +
                             "\n AND ParCompany_id = " + ParCompany_Id;

            }
            else if (valores[0] == "Produto")
            {
                sql = "\n SELECT nCdProduto Id, cast(nCdProduto as varchar) + ' | ' + cNmProduto as Name, 0 as PunishmentValue, 0 as IsDefaultOption  " +
                      "\n FROM Produto  With (nolock) ";
            }
            else if (valores[0] == "familiaProduto")
            {

                sql = @"SELECT 
                        TT.cNrClassificacao as Id,
                        TT.cNmClassificacao as Name,
                        0 as  PunishmentValue,
                        0 as IsDefaultOption
                        FROM
                        (
                        SELECT CL.*, P.* FROM ClassificacaoProduto CP
                        FULL JOIN (
                        SELECT CC.cNmClassificacao as Grupo, C.*, C.nCdClassificacao as cod FROM Classificacao C
                        INNER JOIN (
	                        SELECT * FROM Classificacao 
	                        WHERE LEN(cNrClassificacao) = 5
                        ) CC
                        ON left(C.cNrClassificacao,5) = CC.cNrClassificacao
                        ) CL
                        ON CL.cod = CP.nCdClassificacao
                        LEFT JOIN PRODUTO P
                        ON P.nCdProduto = CP.nCdProduto
                        ) TT
                        WHERE Grupo = 'FAMILIA'
                        GROUP BY TT.cNrClassificacao, cNmClassificacao
                        ORDER BY 2";
            }
            else if (valores[0] == "ReprocessoEntrada" || valores[0] == "ReprocessoSaida")
            {
                sql = "\n SELECT 1 Id, 'Selecione' as Name, 0 as PunishmentValue, 0 as IsDefaultOption  " +
                      "\n  ";
            }

            List<ParFieldType> multipleValues = new List<ParFieldType>();

            using (Factory factory = new Factory("DefaultConnection"))
            {
                multipleValues = factory.SearchQuery<ParFieldType>(sql).ToList();
            }

            return multipleValues;
        }
    }

    public partial class Generico
    {
        public string id { get; set; }
        public string nome { get; set; }

        private SqlConnection db { get; set; }
        public Generico() { }
        public Generico(SqlConnection _db)
        {
            db = _db;
        }

        public List<Generico> getProdutos()
        {
            string conexaoBR = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            db = new SqlConnection(conexaoBR);

            var sql = "SELECT nCdProduto as id, cNmProduto as nome FROM Produto";

            List<Generico> lista = new List<Generico>();

            using (Factory factory = new Factory("DefaultConnection"))
            {
                lista = factory.SearchQuery<Generico>(sql).ToList();
            }

            return lista;
        }

        public List<Generico> getClusterGroupCompany(int ParCompany_Id) //Buscar os módulos
        {
            string conexaoBR = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            db = new SqlConnection(conexaoBR);

            var sql = "SELECT CG.Id as id, CG.Name AS nome  FROM ParCompanyCluster CC LEFT JOIN ParCluster C ON C.ID = CC.ParCluster_Id LEFT JOIN ParClusterGroup CG ON CG.Id = C.ParClusterGroup_Id WHERE CC.ParCompany_Id = " + ParCompany_Id + " and cc.Active = 1 and c.IsActive = 1 and CG.IsActive = 1 GROUP BY CG.Id, CG.Name";

            List<Generico> lista = new List<Generico>();

            using (Factory factory = new Factory("DefaultConnection"))
            {
                lista = factory.SearchQuery<Generico>(sql).ToList();
            }

            return lista;
        }

        public List<Generico> getClusterCompany(int ParCompany_Id, string ParClusterGroup_Id) //Buscar os processos
        {
            string conexaoBR = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            db = new SqlConnection(conexaoBR);

            var sql = "SELECT CC.ParCluster_Id as id, C.Name AS nome  FROM ParCompanyCluster CC LEFT JOIN ParCluster C ON C.ID = CC.ParCluster_Id WHERE CC.ParCompany_Id = " + ParCompany_Id + " and cc.Active = 1 and c.IsActive = 1 and C.ParClusterGroup_Id = " + ParClusterGroup_Id;

            List<Generico> lista = new List<Generico>();
            using (Factory factory = new Factory("DefaultConnection"))
            {
                lista = factory.SearchQuery<Generico>(sql).ToList();
            }

            return lista;
        }
    }

    public partial class ParLevel3Vinculado
    {
        public int Id { get; set; }
        public int ParCompany_Id { get; set; }
        public int ParLevel1_Id { get; set; }
        public int ParLevel2_Id { get; set; }
        public int ParLevel3_Id { get; set; }
        public int SampleNumber { get; set; }
        public int EvaluationNumber { get; set; }
        public String EvaluationInterval { get; set; }

        private SqlConnection db { get; set; }
        public ParLevel3Vinculado() { }
        public ParLevel3Vinculado(SqlConnection _db)
        {
            db = _db;
        }

        public List<ParLevel3Vinculado> getParLevel3Vinculado(int ParCompanyId)
        {
            var sql = string.Format(
                "SELECT * FROM ParLevel3EvaluationSample WHERE(ParCompany_Id = {0} OR ParCompany_Id IS NULL) AND IsActive = 1;",
                ParCompanyId);

            List<ParLevel3Vinculado> lista = new List<ParLevel3Vinculado>();
            using (Factory factory = new Factory("DefaultConnection"))
            {
                lista = factory.SearchQuery<ParLevel3Vinculado>(sql).ToList();
            }

            return lista;
        }
    }

    public partial class ParRelapse
    {
        public int Id { get; set; }
        public int ParFrequency_Id { get; set; }
        public int NcNumber { get; set; }
        public int EffectiveLength { get; set; }

        private SqlConnection db { get; set; }
        public ParRelapse() { }
        public ParRelapse(SqlConnection _db)
        {
            db = _db;
        }

        public IEnumerable<ParRelapse> getRelapses(int ParLevel1_Id)
        {
            string sql = "SELECT Id, ParFrequency_Id, NcNumber, EffectiveLength FROM ParRelapse  (nolock)                 " +
                         "WHERE ParLevel1_Id = '" + ParLevel1_Id + "' and IsActive = 1;                         ";

            List<ParRelapse> parRelapses = new List<ParRelapse>();
            using (Factory factory = new Factory("DefaultConnection"))
            {
                parRelapses = factory.SearchQuery<ParRelapse>(sql).ToList();
            }

            return parRelapses;
        }
    }

    public partial class Result_Level3
    {
        public int Id { get; set; }
        private SqlConnection db { get; set; }
        public Result_Level3() { }
        public Result_Level3(SqlConnection _db)
        {
            db = _db;
        }

        public Result_Level3 get(int CollectionLevel2_Id, int ParLevel3_Id)
        {

            string sql = "SELECT Id FROM Result_Level3  (nolock)          " +
                         "WHERE ParLevel3_Id = '" + ParLevel3_Id + "' and " +
                         "CollectionLevel2_Id = " + CollectionLevel2_Id + ";";

            Result_Level3 parResultLevel3 = null;
            using (Factory factory = new Factory("DefaultConnection"))
            {
                parResultLevel3 = factory.SearchQuery<Result_Level3>(sql).FirstOrDefault();
            }

            return parResultLevel3;
        }
    }

    public partial class ParLevel1VariableProduction
    {
        public int Id { get; set; }
        public string Name { get; set; }

        private SqlConnection db { get; set; }
        public ParLevel1VariableProduction() { }
        public ParLevel1VariableProduction(SqlConnection _db)
        {
            db = _db;
        }
        public IEnumerable<ParLevel1VariableProduction> getVariable(int ParLevel1_Id)
        {
            string sql = "select P.Id, P.Name from ParLevel1VariableProductionXLevel1 PL  (nolock) left join " +
                         "ParLevel1VariableProduction P (nolock)  on P.Id = PL.ParLevel1VariableProduction_Id " +
                         " where PL.ParLevel1_Id = " + ParLevel1_Id + "; ";

            List<ParLevel1VariableProduction> list = new List<ParLevel1VariableProduction>();
            using (Factory factory = new Factory("DefaultConnection"))
            {
                list = factory.SearchQuery<ParLevel1VariableProduction>(sql).ToList();
            }

            return list;
        }
    }

    public partial class ParConfSGQContext
    {

        public int Id { get; set; }
        public bool HaveUnitLogin { get; set; }
        public bool HaveShitLogin { get; set; }
        private SqlConnection db { get; set; }
        public ParConfSGQContext() { }
        public ParConfSGQContext(SqlConnection _db)
        {
            db = _db;
        }

        public ParConfSGQContext get()
        {
            string sql = "SELECT Id, HaveUnitLogin, HaveShitLogin FROM ParConfSGQ (nolock) ";

            ParConfSGQContext conf = null;
            using (Factory factory = new Factory("DefaultConnection"))
            {
                conf = factory.SearchQuery<ParConfSGQContext>(sql).FirstOrDefault();
            }

            return conf;

        }
    }

    public partial class UserSGQ
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Login { get; set; }
        public int ParCompany_Id { get; set; }
        public string ParCompany_Name { get; set; }
        public string Role { get; set; }
        private SqlConnection db { get; set; }

        public UserSGQ()
        {
        }

        public UserSGQ(SqlConnection _db)
        {
            db = _db;
        }

        public UserSGQ getUserByLoginOrId(string userLogin = null, int id = 0)
        {

            string where = "WHERE U.name = '" + userLogin + "'";
            if (id > 0)
            {
                where = "WHERE U.Id = '" + id + "'";
            }

            string sql = "SELECT U.Id, U.Name AS Login, U.Password, U.FullName AS Name, U.ParCompany_Id , PC.Name AS ParCompany_Name, PxU.Role FROM UserSgq U  (nolock) " +
                         "LEFT JOIN ParCompany PC  (nolock) ON U.ParCompany_Id = PC.Id   " +
                         "LEFT JOIN ParCompanyXUserSgq PxU  (nolock) ON U.ParCompany_Id = PxU.ParCompany_Id AND PxU.UserSgq_Id = U.Id " +
                        where;

            UserSGQ user = null;
            using (Factory factory = new Factory("DefaultConnection"))
            {
                user = factory.SearchQuery<UserSGQ>(sql).FirstOrDefault();
            }

            return user;
        }
    }

    public partial class ParCompanyXUserSgq
    {
        public int UserSGQ_Id { get; set; }
        public string UserSGQ_Name { get; set; }
        public string UserSGQ_Login { get; set; }
        public string UserSGQ_Pass { get; set; }

        public string Role { get; set; }
        public int ParCompany_Id { get; set; }

        public string ParCompany_Name { get; set; }

        /// <summary>
        /// Retorna todos os usuários da unidade
        /// </summary>
        /// <param name="ParCompany_Id"></param>
        /// <returns></returns>
        /// 
        private SqlConnection db { get; set; }
        public ParCompanyXUserSgq() { }

        public ParCompanyXUserSgq(SqlConnection _db)
        {
            db = _db;
        }

        public IEnumerable<ParCompanyXUserSgq> getCompanyUsers(int ParCompany_Id)
        {

            string sql = "select U.Id AS UserSGQ_Id, U.Name AS UserSGQ_Login, U.FullName AS UserSGQ_Name, U.Password AS UserSGQ_Pass, U.Role, PxC.Role AS Role, C.Id ParCompany_Id, C.Name ParCompany_Name from ParCompanyXUserSgq PxC (nolock)  " +
                         "INNER JOIN ParCompany C  (nolock) ON PxC.ParCompany_Id = c.Id                                                                                                                                                          " +
                         "INNER JOIN UserSgq U  (nolock) ON PxC.UserSgq_Id = u.Id                                                                                                                                                                " +
                         "WHERE PxC.ParCompany_Id='" + ParCompany_Id + "'                                                                                                                                                              " +
                         "ORDER BY C.Name ASC                                                                                                                                                                                          ";

            List<ParCompanyXUserSgq> users = new List<ParCompanyXUserSgq>();
            using (Factory factory = new Factory("DefaultConnection"))
            {
                users = factory.SearchQuery<ParCompanyXUserSgq>(sql).ToList();
            }

            return users;
        }

        /// <summary>
        /// Retorna todas as unidades do usuário
        /// </summary>
        /// <param name="UserSgq_Id"></param>
        /// <returns></returns>
        public IEnumerable<ParCompanyXUserSgq> getUserCompany(int UserSgq_Id)
        {

            string sql = "select U.Id AS UserSGQ_Id, U.Name AS UserSGQ_Login, U.FullName AS UserSGQ_Name, U.Password AS UserSGQ_Pass, U.Role, PxC.Role AS Role, C.Id ParCompany_Id, C.Name ParCompany_Name from ParCompanyXUserSgq PxC  (nolock) " +
                         "INNER JOIN ParCompany C  (nolock) ON PxC.ParCompany_Id = c.Id                                                                                                                                                          " +
                         "INNER JOIN UserSgq U  (nolock) ON PxC.UserSgq_Id = u.Id                                                                                                                                                                " +
                         "WHERE PxC.UserSgq_Id='" + UserSgq_Id + "'                                                                                                                                                              " +
                         "ORDER BY C.Name ASC                                                                                                                                                                                          ";

            List<ParCompanyXUserSgq> companys = new List<ParCompanyXUserSgq>();

            using (Factory factory = new Factory("DefaultConnection"))
            {
                companys = factory.SearchQuery<ParCompanyXUserSgq>(sql).ToList();
            }

            return companys;
        }

    }

    public partial class RoleXUserSgq
    {
        public string HashKey { get; set; }
        public int Type { get; set; }
        public string RoleJBS { get; set; }
        public string RoleSGQ { get; set; }

        private SqlConnection db { get; set; }
        public RoleXUserSgq() { }
        public RoleXUserSgq(SqlConnection _db)
        {
            db = _db;
        }

        /// <summary>
        /// Retorna todos as permissões do usuário
        /// </summary>
        /// <param name="UserSGQ_Id"></param>
        /// <returns></returns>
        public IEnumerable<RoleXUserSgq> getRoles(int UserSGQ_Id, int ParCompany_id)
        {

            string sql = "SELECT DISTINCT TC.HashKey as HashKey, RT.Id as Type, TJbs.Role as RoleJBS, Tsgq.Role as RoleSGQ FROM ScreenComponent TC  (nolock) " +
                         "LEFT JOIN RoleType RT (nolock)  on RT.Id = TC.Type                                                                          " +
                         "LEFT JOIN RoleSGQ TSgq (nolock)  ON Tsgq.ScreenComponent_Id = TC.Id                                                         " +
                         "LEFT JOIN RoleJBS TJbs (nolock)  ON TJbs.ScreenComponent_Id = TC.Id                                                         " +
                         "LEFT JOIN ParCompanyXUserSgq CU (nolock)  ON (CU.Role = TJbs.Role OR TJbs.Role IS NULL)                                       " +
                         "LEFT JOIN UserSgq U  (nolock) ON (U.Role = Tsgq.Role OR Tsgq.Role IS NULL)                                                    " +
                         "WHERE U.Id = CU.UserSgq_Id AND                                                                                          " +
                         "CU.ParCompany_Id = " + ParCompany_id + " AND                                                                       " +
                         "U.id = " + UserSGQ_Id + ";                                                                                         ";

            List<RoleXUserSgq> users = new List<RoleXUserSgq>();

            using (Factory factory = new Factory("DefaultConnection"))
            {
                users = factory.SearchQuery<RoleXUserSgq>(sql).ToList();
            }

            return users;
        }
    }

    public partial class VolumePcc1b
    {
        public int Id { get; set; }
        public int VolumeAnimais { get; set; }
        public int Quartos { get; set; }
        public int Avaliacoes { get; set; }
        public int Amostras { get; set; }

        private SqlConnection db { get; set; }
        public VolumePcc1b() { }
        public VolumePcc1b(SqlConnection _db)
        {
            db = _db;
        }
        public IEnumerable<VolumePcc1b> getVolumePcc1b(int Indicador, int Unidade)
        {
            string sql = "select VP.Id VP.VolumeAnimais, VP.Quartos, VP.Avaliacoes, VP.Amostras from VolumePcc1b (nolock)  VP where VP.Indicador = " + Indicador + " and VP.Unidade = " + Unidade + "; ";

            List<VolumePcc1b> list = new List<VolumePcc1b>();
            using (Factory factory = new Factory("DefaultConnection"))
            {
                list = factory.SearchQuery<VolumePcc1b>(sql).ToList();
            }

            return list;
        }
    }

    public partial class CaracteristicaTipificacao
    {
        public String nCdCaracteristica { get; set; }
        public String cNmCaracteristica { get; set; }
        public String cNrCaracteristica { get; set; }
        public String cSgCaracteristica { get; set; }
        public String cIdentificador { get; set; }

        private SqlConnection db { get; set; }
        public CaracteristicaTipificacao() { }
        public CaracteristicaTipificacao(SqlConnection _db)
        {
            db = _db;
        }

        public IEnumerable<CaracteristicaTipificacao> getCaracteristicasTipificacao(int id)
        {
            string sql = "select null nCdCaracteristica, null cNmCaracteristica, null cNrCaracteristica, null cSgCaracteristica, null cIdentificador ";
            if (GlobalConfig.Brasil)
            {
                sql = "select CP.nCdCaracteristica, CP.cNmCaracteristica, CP.cNrCaracteristica, CP.cSgCaracteristica, CP.cIdentificador" +
                      " from CaracteristicaTipificacao CP (nolock)  where LEN(CP.cNrCaracteristica) >= 5 and SUBSTRING(CP.cNrCaracteristica, 1, 3) = '" + id + "';";
            }

            List<CaracteristicaTipificacao> list = new List<CaracteristicaTipificacao>();

            using (Factory factory = new Factory("DefaultConnection"))
            {
                list = factory.SearchQuery<CaracteristicaTipificacao>(sql).ToList();
            }

            return list;
        }

        public IEnumerable<CaracteristicaTipificacao> getAreasParticipantes()
        {
            string sql = "select CP.nCdCaracteristica, CP.cNmCaracteristica, CP.cNrCaracteristica, CP.cSgCaracteristica, CP.cIdentificador" +
                         " from AreasParticipantes CP  (nolock) where LEN(cNrCaracteristica) >= 5;";

            List<CaracteristicaTipificacao> list = new List<CaracteristicaTipificacao>();
            using (Factory factory = new Factory("DefaultConnection"))
            {
                list = factory.SearchQuery<CaracteristicaTipificacao>(sql).ToList();
            }

            return list;
        }

        public IEnumerable<CaracteristicaTipificacao> getCaracteristicasTipificacaoUnico(int ncdcaracteristica)
        {
            string sql = "select CP.nCdCaracteristica, CP.cNmCaracteristica, CP.cNrCaracteristica, CP.cSgCaracteristica, CP.cIdentificador" +
                         " from CaracteristicaTipificacao CP (nolock)  where cNrCaracteristica = " + ncdcaracteristica;

            List<CaracteristicaTipificacao> list = new List<CaracteristicaTipificacao>();
            using (Factory factory = new Factory("DefaultConnection"))
            {
                list = factory.SearchQuery<CaracteristicaTipificacao>(sql).ToList();
            }

            return list;
        }

        public IEnumerable<CaracteristicaTipificacao> getAreasParticipantesUnico()
        {
            string sql = "select CP.nCdCaracteristica, CP.cNmCaracteristica, CP.cNrCaracteristica, CP.cSgCaracteristica, CP.cIdentificador" +
                         " from AreasParticipantes CP  (nolock) where cNrCaracteristica = 0209;";

            List<CaracteristicaTipificacao> list = new List<CaracteristicaTipificacao>();

            using (Factory factory = new Factory("DefaultConnection"))
            {
                list = factory.SearchQuery<CaracteristicaTipificacao>(sql).ToList();
            }

            return list;
        }

    }

    public partial class VerificacaoTipificacaoTarefaIntegracao
    {

        public int Id { get; set; }
        public int TarefaId { get; set; }
        public int CaracteristicaTipificacaoId { get; set; }

        private SqlConnection db { get; set; }
        public VerificacaoTipificacaoTarefaIntegracao() { }
        public VerificacaoTipificacaoTarefaIntegracao(SqlConnection _db)
        {
            db = _db;
        }

        public IEnumerable<VerificacaoTipificacaoTarefaIntegracao> getTarefa(int caracteristicatipificacaoid)
        {

            string sql = "select Id, TarefaId, CaracteristicaTipificacaoId from VerificacaoTipificacaoTarefaIntegracao (nolock)  where CaracteristicaTipificacaoId = " + caracteristicatipificacaoid;

            List<VerificacaoTipificacaoTarefaIntegracao> list = new List<VerificacaoTipificacaoTarefaIntegracao>();

            using (Factory factory = new Factory("DefaultConnection"))
            {
                list = factory.SearchQuery<VerificacaoTipificacaoTarefaIntegracao>(sql).ToList();
            }

            return list;
        }

    }

    public partial class CollectionLevel2Consolidation
    {
        public int ConsolidationLevel2_Id { get; set; }

        public int ParLevel2_Id { get; set; }

        public decimal WeiEvaluationTotal { get; set; }
        public decimal DefectsTotal { get; set; }
        public decimal WeiDefectsTotal { get; set; }
        public int TotalLevel3Evaluation { get; set; }
        public int TotalLevel3WithDefects { get; set; }
        public int LastEvaluationAlert { get; set; }
        public int LastLevel2Alert { get; set; }
        public int EvaluatedResult { get; set; }
        public int DefectsResult { get; set; }

        private SqlConnection db { get; set; }
        public CollectionLevel2Consolidation() { }
        public CollectionLevel2Consolidation(SqlConnection _db)
        {
            db = _db;
        }

        public CollectionLevel2Consolidation getConsolidation(int ConsolidationLevel2_Id, int ParLevel2_Id)
        {
            try
            {

                string sql = "SELECT ConsolidationLevel2_Id, ParLevel2_Id, SUM(WeiEvaluation) AS [WeiEvaluationTotal], SUM(Defects) AS [DefectsTotal], SUM(WeiDefects) AS[WeiDefectsTotal], SUM(TotalLevel3WithDefects) AS [TotalLevel3WithDefects], SUM(TotalLevel3Evaluation) AS [TotalLevel3Evaluation], MAX(LastEvaluationAlert) AS LastEvaluationAlert, (SELECT top 1 LastLevel2Alert FROM CollectionLevel2 WHERE Id = max(c2.id)) AS LastLevel2Alert, SUM(EvaluatedResult) AS EvaluatedResult, SUM(DefectsResult) AS DefectsResult " +
                             "FROM CollectionLevel2 C2  (nolock) WHERE ConsolidationLevel2_Id = " + ConsolidationLevel2_Id + " AND ParLevel2_Id = " + ParLevel2_Id + " AND NotEvaluatedIs=0" +
                             "group by ConsolidationLevel2_Id, ParLevel2_Id";

                CollectionLevel2Consolidation consolidationLevel2 = null;

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    consolidationLevel2 = factory.SearchQuery<CollectionLevel2Consolidation>(sql).FirstOrDefault();
                }

                return consolidationLevel2;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }

    public partial class ConsolidationLevel1XConsolidationLevel2
    {

        public decimal WeiEvaluation { get; set; } //OK
        public decimal EvaluateTotal { get; set; } //OK
        public decimal DefectsTotal { get; set; }
        public decimal WeiDefects { get; set; }
        public decimal TotalLevel3Evaluation { get; set; }
        public decimal TotalLevel3WithDefects { get; set; }
        public int LastEvaluationAlert { get; set; }
        public int LastLevel2Alert { get; set; }

        public int EvaluatedResult { get; set; }
        public int DefectsResult { get; set; }

        private SqlConnection db { get; set; }
        public ConsolidationLevel1XConsolidationLevel2() { }
        public ConsolidationLevel1XConsolidationLevel2(SqlConnection _db)
        {
            db = _db;
        }

        public ConsolidationLevel1XConsolidationLevel2 getConsolidation(int ConsolidationLevel1_Id)
        {
            try
            {

                string sql = "select SUM(WeiEvaluation) AS WeiEvaluation, SUM(EvaluateTotal) AS EvaluateTotal, SUM(DefectsTotal) AS DefectsTotal, SUM(WeiDefects) AS WeiDefects,  SUM(TotalLevel3Evaluation) AS TotalLevel3Evaluation, SUM(TotalLevel3WithDefects) AS TotalLevel3WithDefects, MAX(LastEvaluationAlert) AS LastEvaluationAlert, (SELECT top 1 LastLevel2Alert FROM CollectionLevel2 (nolock)  WHERE Id = max(c2.id)) AS LastLevel2Alert, SUM(EvaluatedResult) AS EvaluatedResult, SUM(DefectsResult) AS DefectsResult FROM ConsolidationLevel2 C2 (nolock)  where ConsolidationLevel1_Id=" + ConsolidationLevel1_Id + "";

                ConsolidationLevel1XConsolidationLevel2 consolidationLevel1 = null;
                using (Factory factory = new Factory("DefaultConnection"))
                {
                    consolidationLevel1 = factory.SearchQuery<ConsolidationLevel1XConsolidationLevel2>(sql).FirstOrDefault();
                }

                return consolidationLevel1;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }

    public partial class ConsolidationLevel1
    {
        public int Id { get; set; }
        public int UnitId { get; set; }
        public int DepartmentId { get; set; }
        public int ParLevel1_Id { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime? AlterDate { get; set; }
        public DateTime ConsolidationDate { get; set; }
        public int Evaluation { get; set; }
        public int AtualAlert { get; set; }
        public decimal WeiEvaluation { get; set; }
        public decimal EvaluateTotal { get; set; }
        public decimal DefectsTotal { get; set; }
        public decimal WeiDefects { get; set; }
        public int TotalLevel3Evaluation { get; set; }
        public int TotalLevel3WithDefects { get; set; }
        public int LastEvaluationAlert { get; set; }
        public int LastLevel2Alert { get; set; }
        public int EvaluatedResult { get; set; }
        public int DefectsResult { get; set; }

        private SqlConnection db { get; set; }
        public ConsolidationLevel1() { }
        public ConsolidationLevel1(SqlConnection _db)
        {
            db = _db;
        }
        public ConsolidationLevel1 getConsolidation(int ParCompany_Id, int ParLevel1_Id, DateTime collectionDate, int Shift, int Period, string cluster)
        {
            try
            {
                string sql = @"
                            SELECT CL1.* FROM ConsolidationLevel1 cl1 with (nolock) 
                            LEFT JOIN ConsolidationLevel1XCluster C1C
                            ON C1C.ConsolidationLevel1_Id = Cl1.Id
                            WHERE UnitId = '" + ParCompany_Id + "' " +
                            "AND ParLevel1_Id= '" + ParLevel1_Id + "' " +
                            "AND SHIFT = " + Shift + " " +
                            "and period = " + Period + " " +
                            "AND CONVERT(date, ConsolidationDate) = '" + collectionDate.ToString("yyyy-MM-dd") + @"'
                             AND (C1C.ParCluster_Id = " + cluster + " OR C1C.ParCluster_Id IS NULL)";

                //SqlConnection db = new SqlConnection(conexao);

                ConsolidationLevel1 obj = null;
                using (Factory factory = new Factory("DefaultConnection"))
                {
                    obj = factory.SearchQuery<ConsolidationLevel1>(sql).FirstOrDefault();
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Deu merda 3 ", ex);
            }
        }

    }

    public partial class ConsolidationLevel2
    {
        public int Id { get; set; }
        public int ConsolidationLevel1_Id { get; set; }
        public int ParLevel2_Id { get; set; }
        public int UnitId { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime? AlterDate { get; set; }
        public int AlertLevel { get; set; }
        public DateTime ConsolidationDate { get; set; }
        public decimal WeiEvaluation { get; set; }
        public decimal EvaluateTotal { get; set; }
        public decimal DefectsTotal { get; set; }
        public decimal WeiDefects { get; set; }
        public int TotalLevel3Evaluation { get; set; }
        public int TotalLevel3WithDefects { get; set; }
        public int LastEvaluationAlert { get; set; }
        public int LastLevel2Alert { get; set; }
        public int EvaluatedResult { get; set; }
        public int DefectsResult { get; set; }

        private SqlConnection db { get; set; }
        public ConsolidationLevel2() { }
        public ConsolidationLevel2(SqlConnection _db)
        {
            db = _db;
        }
        public ConsolidationLevel2 getConsolidation(int ParCompany_Id, int ParLevel1_Id, DateTime collectionDate)
        {
            try
            {
                string sql = "SELECT * FROM ConsolidationLevel2 (nolock)  WHERE UnitId = " + ParCompany_Id + " AND ParLevel1_Id= " + ParLevel1_Id + " AND ConsolidationDate BETWEEN '" + collectionDate.ToString("yyyy-MM-dd") + " 00:00' AND '" + collectionDate.ToString("yyyy-MM-dd") + " 23:59:59'";

                /**
                 * ADD PARAMETER FORLINI
                 * DECLARA TODAS AS COLUNAS!! AO INVES DO *
                 * INSERIR ÍNDICE NO CONSOLIDATIONLEVEL1 DENTRO DO CONOLIDATIONLEVEL2
                 */

                ConsolidationLevel2 obj = null;

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    obj = factory.SearchQuery<ConsolidationLevel2>(sql).FirstOrDefault();
                }

                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ConsolidationLevel2 getByConsolidationLevel1(int ParCompany_Id, int ConsolidationLevel1_Id, int ParLevel2_Id, string cluster)
        {
            try
            {
                string sql = @"

                            SELECT c2.Id, 
                            ConsolidationLevel1_Id, 
                            UnitId, 
                            ParLevel2_Id, 
                            ConsolidationDate, 
                            WeiEvaluation, 
                            EvaluateTotal, 
                            DefectsTotal, 
                            WeiDefects, 
                            TotalLevel3Evaluation, 
                            TotalLevel3WithDefects, 
                            EvaluatedResult 
                            FROM ConsolidationLevel2 c2 with (nolock)
                            LEFT JOIN ConsolidationLevel2XCluster C2C
                            ON C2C.ConsolidationLevel2_id = c2.Id
                            WHERE ConsolidationLevel1_Id = '" + ConsolidationLevel1_Id + "' " +
                            " AND ParLevel2_Id= '" + ParLevel2_Id + "' " +
                            " AND UnitId='" + ParCompany_Id + "'" +
                            " AND (C2C.ParCluster_Id = '" + cluster + "' OR C2C.ParCluster_Id IS NULL)";



                ConsolidationLevel2 obj = null;
                using (Factory factory = new Factory("DefaultConnection"))
                {
                    obj = factory.SearchQuery<ConsolidationLevel2>(sql).FirstOrDefault();
                }

                return obj;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ConsolidationLevel2 getByConsolidationLevel1(int ParCompany_Id, int ConsolidationLevel1_Id, int ParLevel2_Id, int reaudit, string reauditNumber, string cluster)
        {
            try
            {
                string sql = @"
                            SELECT 
                            c2.Id, 
                            c2.ConsolidationLevel1_Id, 
                            c2.UnitId, 
                            c2.ParLevel2_Id, 
                            c2.ConsolidationDate, 
                            c2.WeiEvaluation, 
                            c2.EvaluateTotal, 
                            c2.DefectsTotal, 
                            c2.WeiDefects, 
                            c2.TotalLevel3Evaluation, 
                            c2.TotalLevel3WithDefects, 
                            c2.EvaluatedResult, 
                            c2.ReauditIs, 
                            c2.ReauditNumber 
                            FROM ConsolidationLevel2 c2 with (nolock) 
                            LEFT JOIN ConsolidationLevel2XCluster C2C
                            ON C2C.ConsolidationLevel2_id = c2.Id
                            WHERE c2.ConsolidationLevel1_Id = '" + ConsolidationLevel1_Id + "' " +
                            "AND c2.ParLevel2_Id= '" + ParLevel2_Id + "' " +
                            "AND c2.UnitId='" + ParCompany_Id + "' " +
                            "AND c2.ReauditIs=" + reaudit + " " +
                            "and c2.reauditnumber=" + reauditNumber + " " +
                            " AND (C2C.ParCluster_Id = '" + cluster + "' OR C2C.ParCluster_Id IS NULL)";


                ConsolidationLevel2 obj = null;
                using (Factory factory = new Factory("DefaultConnection"))
                {
                    obj = factory.SearchQuery<ConsolidationLevel2>(sql).FirstOrDefault();
                }

                return obj;

            }
            catch (Exception)
            {
                throw;
            }
        }

    }

    public partial class CollectionJson
    {
        public int Id { get; set; }
        public int Unit_Id { get; set; }
        public int Shift { get; set; }
        public int Period { get; set; }
        public int level01_Id { get; set; }
        public DateTime Level01CollectionDate { get; set; }
        public int level02_Id { get; set; }
        public int Evaluate { get; set; }
        public int Sample { get; set; }

        public int AuditorId { get; set; }
        public DateTime Level02CollectionDate { get; set; }
        public string Level02HeaderJson { get; set; }
        public string Level03ResultJSon { get; set; }
        public string CorrectiveActionJson { get; set; }
        public bool Reaudit { get; set; }
        public int ReauditNumber { get; set; }
        public int ReauditLevel { get; set; }
        public bool haveReaudit { get; set; }
        public bool haveCorrectiveAction { get; set; }
        public string Device_Id { get; set; }
        public string AppVersion { get; set; }
        public string Ambient { get; set; }
        public bool IsProcessed { get; set; }
        public string Device_Mac { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime? AlterDate { get; }
        public string Key { get; set; }
        public string TTP { get; set; }

        private SqlConnection db { get; set; }
        public CollectionJson() { }
        public CollectionJson(SqlConnection _db)
        {
            db = _db;
        }

        public IEnumerable<CollectionJson> getJson(string sql)
        {
            try
            {

                List<CollectionJson> list = new List<CollectionJson>();

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    list = factory.SearchQuery<CollectionJson>(sql).ToList();
                }

                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public partial class ParCounter
    {
        public string Counter { get; set; }
        public string Local { get; set; }
        public string Level { get; set; }
        public string indicador { get; set; }

        private SqlConnection db { get; set; }
        public ParCounter() { }
        public ParCounter(SqlConnection _db)
        {
            db = _db;
        }

        public IEnumerable<ParCounter> GetParLevelXParCounterList(ParLevel1 ParLevel1, ParLevel2 ParLevel2, int Level)
        {
            try
            {

                string[] lista;
                string ParCluster_IdL2 = "0";
                string ParCluster_IdL1 = "0";

                if (ParLevel2 != null)
                {
                    lista = ParLevel2.Id.Replace(SyncServices.quebraProcesso, "|").Split('|');
                    ParCluster_IdL2 = lista.Length > 1 ? lista[0] : "0";
                }

                if (ParLevel1 != null)
                {
                    ParCluster_IdL1 = ParLevel1.ParCluster_Id.ToString();
                }


                var ParLevel1_Id = ParLevel1 == null ? 0 : ParLevel1.ParLevel1_Id;
                var ParLevel2_Id = ParLevel2 == null ? 0 : ParLevel2.ParLevel2_id;

                if (ParLevel1_Id > 0 || ParLevel2_Id > 0)
                {
                    string sql = "";
                    if (ParLevel1_Id > 0)
                    {

                        sql = "SELECT Distinct PO.level, PC.Name as Counter, PO.Name as Local, '" + ParCluster_IdL1 + SyncServices.quebraProcesso + "' + CAST(PL.ParLevel1_Id AS VARCHAR) AS indicador FROM ParCounterXLocal PL (nolock)  " +
                              "LEFT JOIN ParCounter PC (nolock)  ON PL.ParCounter_Id = PC.Id " +
                              "LEFT JOIN ParLocal PO  (nolock) ON PO.Id = PL.ParLocal_Id " +
                              "WHERE PL.ParLevel1_Id = " + ParLevel1_Id + " " +
                              "AND PL.ParLevel2_Id IS NULL " +
                              "AND PC.Level = " + Level +
                              "AND PL.IsActive = 1";

                    }
                    else if (ParLevel2_Id > 0)
                    {

                        sql = "SELECT Distinct PO.level, PC.Name as Counter, PO.Name as Local, '" + ParCluster_IdL2 + SyncServices.quebraProcesso + "' + CAST(PL.ParLevel2_Id AS VARCHAR) AS indicador FROM ParCounterXLocal PL (nolock)  " +
                              "LEFT JOIN ParCounter PC (nolock)  ON PL.ParCounter_Id = PC.Id " +
                              "LEFT JOIN ParLocal PO (nolock)  ON PO.Id = PL.ParLocal_Id " +
                              "WHERE PL.ParLevel1_Id IS NULL " +
                              "AND PL.ParLevel2_Id= " + ParLevel2_Id + " " +
                              "AND PC.Level = " + Level +
                              "AND PL.IsActive = 1";


                    }

                    List<ParCounter> list = new List<ParCounter>();

                    using (Factory factory = new Factory("DefaultConnection"))
                    {
                        list = factory.SearchQuery<ParCounter>(sql).ToList();
                    }
                    return list;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }

    public partial class NotConformityRule
    {
        public int Id { get; set; }
        public decimal Value { get; set; }

        private SqlConnection db { get; set; }
        public NotConformityRule() { }
        public NotConformityRule(SqlConnection _db)
        {
            db = _db;
        }
        public NotConformityRule getParNCRule(int NCR_Id, int P2_Id)
        {
            try
            {
                string sql = "SELECT NCL.Id as Id, NCL.Value FROM ParNotConformityRuleXLevel NCL (nolock)  LEFT JOIN          " +
                             "   ParNotConformityRule NCR (nolock)  ON NCR.Id = NCL.ParNotConformityRule_Id                   " +
                             "   LEFT JOIN ParLevel2 P2  (nolock) ON P2.Id = NCL.ParLevel2_Id                                 " +
                             "   WHERE NCR.Id = " + NCR_Id + " AND P2.Id = " + P2_Id + " AND NCL.IsActive = 1;                                     ";

                NotConformityRule obj = null;
                using (Factory factory = new Factory("DefaultConnection"))
                {
                    obj = factory.SearchQuery<NotConformityRule>(sql).FirstOrDefault();
                }
                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

    public partial class CollectionLevel2
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public int ConsolidationLevel2_Id { get; set; }
        public int ParLevel1_Id { get; set; }
        public int ParLevel2_Id { get; set; }
        public int UnitId { get; set; }
        public int AuditorId { get; set; }
        public int Shift { get; set; }
        public int Period { get; set; }
        public int Phase { get; set; }
        public bool ReauditIs { get; set; }
        public int ReauditNumber { get; set; }
        public int ReauditLevel { get; set; }
        public DateTime CollectionDate { get; set; }
        public DateTime? StartPhaseDate { get; set; }
        public int EvaluationNumber { get; set; }
        public int Sample { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime? AlterDate { get; set; }
        public bool ConsecutiveFailureIs { get; set; }
        public int ConsecutiveFailureTotal { get; set; }
        public bool NotEvaluatedIs { get; set; }
        public bool Duplicated { get; set; }
        public bool HaveCorrectiveAction { get; set; }
        public bool HaveReaudit { get; set; }
        public bool HavePhase { get; set; }
        public bool Completed { get; set; }
        public int ParFrequency_Id { get; set; }
        public int AlertLevel { get; set; }
        public int Sequential { get; set; }
        public int Side { get; set; }
        public decimal WeiEvaluation { get; set; }
        public decimal Defects { get; set; }
        public decimal WeiDefects { get; set; }
        public int TotalLevel3WithDefects { get; set; }
        public int TotalLevel3Evaluation { get; set; }
        public int LastEvaluationAlert { get; set; }
        public int LastLevel2Alert { get; set; }
        public int EvaluatedResult { get; set; }
        public int DefectsResult { get; set; }
        public bool IsEmptyLevel3 { get; set; }

        private SqlConnection db { get; set; }
        public CollectionLevel2() { }
        public CollectionLevel2(SqlConnection _db)
        {
            db = _db;
        }
        public CollectionLevel2 GetByKey(string key)
        {
            try
            {
                string sql = "SELECT * FROM CollectionLevel2 (nolock)  WHERE [Key] = '" + key + "'";

                CollectionLevel2 obj = null;

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    obj = factory.SearchQuery<CollectionLevel2>(sql).FirstOrDefault();
                }
                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

    public partial class UpdateCollectionLevel2
    {
        private SqlConnection db { get; set; }
        public UpdateCollectionLevel2() { }
        public UpdateCollectionLevel2(SqlConnection _db)
        {
            db = _db;
        }
        public void UpdateIsReauditByKey(string Key, bool IsReaudit, int HaveReaudit, int ReauditNumber, int ReauditLevel)
        {
            try
            {
                string sql = "";

                if (IsReaudit == true && HaveReaudit == 1)
                {
                    sql = "UPDATE CollectionLevel2 SET ReauditLevel = '" + ReauditLevel + "', HaveReaudit = '" + HaveReaudit + "', ReauditNumber = '" + ReauditNumber + "' WHERE [Key] = '" + Key + "'";
                }
                else if (IsReaudit == true && HaveReaudit == 0)
                {
                    sql = "UPDATE CollectionLevel2 SET HaveReaudit = 0, ReauditNumber = " + ReauditNumber + " WHERE [Key] = '" + Key + "'";
                }
                db.Execute(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateIsReauditConsolidationLevel1(bool IsReaudit, int HaveReaudit, int ReauditNumber, int ConsolidationLevel1_Id)
        {
            try
            {
                string sql = "";

                if (HaveReaudit == 1)
                {
                    sql = "UPDATE CollectionLevel2 " +
                            "SET ReauditLevel = 1, HaveReaudit = '" + HaveReaudit + "', ReauditNumber = '" + ReauditNumber + "' " +
                            "FROM CollectionLevel2 (nolock)  " +
                            "WHERE ConsolidationLevel2_Id " +
                        "IN(SELECT Id FROM ConsolidationLevel2 WHERE ConsolidationLevel1_Id = " + ConsolidationLevel1_Id + ") AND ReauditIs = 0";
                }
                else if (HaveReaudit == 0)
                {
                    sql = "UPDATE CollectionLevel2 " +
                            "SET ReauditLevel = 1, HaveReaudit = 0, ReauditNumber = 0 " +
                            "FROM CollectionLevel2  (nolock) " +
                            "WHERE ConsolidationLevel2_Id " +
                        "IN(SELECT Id FROM ConsolidationLevel2 (nolock)  WHERE ConsolidationLevel1_Id = " + ConsolidationLevel1_Id + ")  AND ReauditIs = 0 " +
                        "AND " +
                        "(SELECT Count(*) " +
                            "FROM CollectionLevel2 (nolock)  " +
                            "WHERE ReauditIs = 1 AND ReauditNumber = '" + ReauditNumber + "' " +
                            "AND ConsolidationLevel2_Id IN(SELECT Id FROM ConsolidationLevel2 (nolock)  WHERE ConsolidationLevel1_Id = " + ConsolidationLevel1_Id + "))" +
                        " = " +
                        "(SELECT Count(*) " +
                            "FROM CollectionLevel2 (nolock)  " +
                            "WHERE ReauditIs = 0 " +
                        "AND ConsolidationLevel2_Id IN(SELECT Id FROM ConsolidationLevel2 (nolock)  WHERE ConsolidationLevel1_Id = " + ConsolidationLevel1_Id + "))";

                }

                db.Execute(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public partial class ResultPhase
    {
        public int Id { get; set; }
        public int ParLevel1_Id { get; set; }
        public int ParLevel2_Id { get; set; }
        public string CollectionDate { get; set; }
        public int Period { get; set; }
        public int Shift { get; set; }
        public int EvaluationNumber { get; set; }
        public int Phase { get; set; }
        public int CountPeriod { get; set; }
        public int CountShift { get; set; }

        private SqlConnection db { get; set; }
        public ResultPhase() { }
        public ResultPhase(SqlConnection _db)
        {
            db = _db;
        }
        public List<ResultPhase> GetByMonth(int ParCompany_Id, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                string sql = "SELECT   " +
                            "Id, " +
                            "ParLevel1_Id, " +
                            "ParLevel2_Id, " +
                            "FORMAT(CollectionDate, 'MMddyyyy') as CollectionDate, " +
                            "Period, " +
                            "Shift, " +
                            "Phase, " +
                            "EvaluationNumber " +
                            "FROM CollectionLevel2 c1                                                                                       " +
                            "WHERE CollectionDate                                                                                           " +
                            "BETWEEN '" + StartDate.ToString("yyyyMMdd") + " 00:00'  and '" + EndDate.ToString("yyyyMMdd") + "  23:59:59' and   " +
                            "Phase > 0  and UnitId = " + ParCompany_Id + "                                                                   " +
                            "AND CONCAT(c1.ParLevel1_id, c1.ParLevel2_Id, CAST(c1.CollectionDate AS VARCHAR(500))) IN                       " +
                            "  (SELECT CONCAT(c1b.ParLevel1_id, c1b.ParLevel2_Id, CAST(MAX(c1b.CollectionDate) AS VARCHAR(500)))            " +
                            "                                                                                                               " +
                            "      FROM CollectionLevel2 c1b  (nolock)                                                                                " +
                            "                                                                                                               " +
                            "          WHERE c1b.Phase > 0                                                                                  " +
                            "                                                                                                               " +
                            "          AND c1b.CollectionDate BETWEEN '" + StartDate.ToString("yyyyMMdd") + " 00:00' and '" + EndDate.ToString("yyyyMMdd") + "  23:59:59'                                     " +
                            "          AND c1b.UnitId = " + ParCompany_Id + "                                                                       " +
                            "      GROUP BY c1b.ParLevel1_id, c1b.ParLevel2_Id                                                              " +
                            "  )                                                                                                            ";


                List<ResultPhase> list = new List<ResultPhase>();
                using (Factory factory = new Factory("DefaultConnection"))
                {
                    list = factory.SearchQuery<ResultPhase>(sql).ToList();
                }

                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public partial class ResultPhaseFrequency
    {
        public int ParFrequency_Id { get; set; }

        private SqlConnection db { get; set; }
        public ResultPhaseFrequency() { }
        public ResultPhaseFrequency(SqlConnection _db)
        {
            db = _db;
        }
        public ResultPhaseFrequency GetPhaseFrequency(int ParLevel1_Id, int Phase)
        {
            try
            {
                string sql = "SELECT * FROM (                                                                               " +
                             "   SELECT ROW_NUMBER() OVER(ORDER BY Id ASC) AS Phase, p.ParFrequency_Id AS ParFrequency_Id   " +
                             "   FROM ParRelapse p  (nolock) WHERE ParLevel1_Id = " + ParLevel1_Id + " AND IsActive = 1                " +
                             "   ) AS T                                                                                     " +
                             "   WHERE T.Phase = " + Phase + "                                                               " +
                             "                                                                                              ";


                ResultPhaseFrequency obj = null;
                using (Factory factory = new Factory("DefaultConnection"))
                {
                    obj = factory.SearchQuery<ResultPhaseFrequency>(sql).FirstOrDefault();
                }
                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public partial class ResultLevel2Period
    {
        public DateTime CollectionDate { get; set; }
        public int Period { get; set; }

        private SqlConnection db { get; set; }
        public ResultLevel2Period() { }
        public ResultLevel2Period(SqlConnection _db)
        {
            db = _db;
        }
        public List<ResultLevel2Period> GetResultLevel2Period(int Id, int ParCompany_Id, int ParLevel1_Id, int ParLevel2_Id, DateTime StartDate, DateTime EndDate, int Shift)
        {
            try
            {
                string sql = "SELECT CAST(CollectionDate as date) as CollectionDate, Period, Shift                                                      " +
                             "FROM CollectionLevel2  (nolock) WHERE  Id >= " + Id + " AND UnitId = " + ParCompany_Id + "  AND  Shift = " + Shift + "  AND          " +
                             "CollectionDate BETWEEN '" + StartDate.ToString("yyyyMMdd") + " 00:00' AND '" + EndDate.ToString("yyyyMMdd") + " 23:59:59'    " +
                             "GROUP BY CAST(CollectionDate as date), Period, Shift ORDER BY 1";

                List<ResultLevel2Period> obj = new List<ResultLevel2Period>();
                using (Factory factory = new Factory("DefaultConnection"))
                {
                    obj = factory.SearchQuery<ResultLevel2Period>(sql).ToList();
                }

                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public partial class ResultEvaluationDefects
    {
        public int Defects { get; set; }
        public int EvaluationNumber { get; set; }
        public int Sample { get; set; }
        public int Period { get; set; }
        public int Shift { get; set; }

        private SqlConnection db { get; set; }
        public ResultEvaluationDefects() { }
        public ResultEvaluationDefects(SqlConnection _db)
        {
            db = _db;
        }
        public List<ResultEvaluationDefects> GetByDay(int ParCompany_Id, DateTime Date, int ParLevel1_Id)
        {
            try
            {
                string sql = "SELECT SUM(Defects) AS Defects, EvaluationNumber, Sample, Period, Shift from CollectionLevel2 (nolock)                                " +
                                "WHERE                                                                                  " +
                                "ParLevel1_Id = " + ParLevel1_Id + " AND                                                 " +
                                "CAST(CollectionDate as date) = CAST('" + Date.ToString("yyyyMMdd") + "' as DATE)        " +
                                "GROUP BY EvaluationNumber, Sample, Period, Shift; ";

                List<ResultEvaluationDefects> list = new List<ResultEvaluationDefects>();
                using (Factory factory = new Factory("DefaultConnection"))
                {
                    list = factory.SearchQuery<ResultEvaluationDefects>(sql).ToList();
                }

                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public partial class ResultadoUmaColuna
    {
        public string retorno { get; set; }
    }
}