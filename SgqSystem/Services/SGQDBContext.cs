using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System;
using System.Linq;
using Dominio;
using System.Threading;
using System.Collections;
using SgqSystem.Services;
using DTO;

namespace SGQDBContext
{
    public partial class ParLevel1
    {
        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public int Id { get; set; }
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
        public decimal tipoAlerta { get; set; }
        public decimal valorAlerta { get; set; }
        public bool HasCompleteEvaluation { get; set; }
        public bool IsReaudit { get; set; }
        public bool EditLevel2 { get; set; }
        public bool IsSpecificNumberEvaluetion { get; set; }
        public bool IsFixedEvaluetionNumber { get; set; }

        public bool HasGroupLevel2 { get; set; }
        public bool HasTakePhoto { get; set; }
        public bool ShowScorecard { get; set; }

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
                //SqlConnection db = new SqlConnection(conexao);
                string sql = "SELECT * FROM ParLevel1 (nolock)  WHERE Id='" + Id + "'";
                var parLevel1List = db.Query<ParLevel1>(sql).FirstOrDefault();

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
                //SqlConnection db = new SqlConnection(conexao);
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
                var parLevel1List = db.Query<ParLevel1>(sql).ToList();

                return parLevel1List;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<ParLevel1> getParLevel1ParCriticalLevelList(int ParCompany_Id, string Level1ListId)
        {

            /*
             * MOCK GABRIEL PARA TESTE DE TRAZER TAREFAS DO OUTRO INDICADOR
             * 30/03/2017
             */

            //string whereIsChildren = "  ";
            string whereIsChildren = " AND IsChildren = 0 ";

            //SqlConnection db = new SqlConnection(conexao);
            string sql = "\n SELECT P1.Id, P1.Name, P1.HasTakePhoto, CL.Id AS ParCriticalLevel_Id, CL.Name AS ParCriticalLevel_Name, P1.HasSaveLevel2 AS HasSaveLevel2, P1.ParConsolidationType_Id AS ParConsolidationType_Id, P1.ParFrequency_Id AS ParFrequency_Id,     " +
                         "\n P1.HasNoApplicableLevel2 AS HasNoApplicableLevel2, P1.HasAlert, P1.IsSpecific, P1.hashKey, P1.haveRealTimeConsolidation, P1.RealTimeConsolitationUpdate, P1.IsLimitedEvaluetionNumber, P1.IsPartialSave" +
                         "\n ,AL.ParNotConformityRule_Id AS tipoAlerta, AL.Value AS valorAlerta, AL.IsReaudit AS IsReaudit, P1.HasCompleteEvaluation AS HasCompleteEvaluation, P1.HasGroupLevel2 AS HasGroupLevel2, P1.EditLevel2 AS EditLevel2, P1.IsFixedEvaluetionNumber AS IsFixedEvaluetionNumber " +
                         "\n FROM ParLevel1 P1  (nolock)                                                                                                         " +
                         "\n INNER JOIN (SELECT ParLevel1_Id FROM ParLevel3Level2Level1 GROUP BY ParLevel1_Id) P321                                     " +
                         "\n ON P321.ParLevel1_Id = P1.Id                                                                                               " +
                         "\n INNER JOIN ParLevel1XCluster P1C  (nolock)                                                                                           " +
                         "\n ON P1C.ParLevel1_Id = P1.Id                                                                                                " +
                         "\n INNER JOIN ParCluster C    (nolock)                                                                                                  " +
                         "\n ON C.Id = P1C.ParCluster_Id                                                                                                " +
                         "\n INNER JOIN ParCompanyCluster CC   (nolock)                                                                                           " +
                         "\n ON CC.ParCluster_Id = P1C.ParCluster_Id  and CC.Active = 1                                                               " +
                         "\n INNER JOIN ParCriticalLevel CL     (nolock)                                                                                          " +
                         "\n ON CL.Id = P1C.ParCriticalLevel_Id                                                                                         " +
                         "\n LEFT JOIN ParNotConformityRuleXLevel AL    (nolock)                                                                                 " +
                         "\n ON AL.ParLevel1_Id = P1.Id   AND AL.IsActive = 1                                                                                               " +

                         "\n INNER JOIN (SELECT ParLevel1_Id FROM (select * from parGoal (nolock)  where IsActive = 1 and (ParCompany_Id is null or ParCompany_Id = '" + ParCompany_Id + "')) A GROUP BY ParLevel1_Id) G  " +
                         "\n ON P1.Id = G.ParLevel1_Id                                                                                        " +

                         "\n WHERE CC.ParCompany_Id = '" + ParCompany_Id + "'                                                                           " +
                         "\n " + whereIsChildren + "                                                                                                       " +
                         "\n AND P1.IsActive = 1 AND C.IsActive = 1 AND P1C.IsActive = 1 AND CC.Active = 1                                                                                                       ";
            if (Level1ListId != "" && Level1ListId != null)
            {
                sql += " AND P1.Id IN (" + Level1ListId.Substring(0, Level1ListId.Length - 1) + ") ";
            }

            sql += "\n ORDER BY CL.Name, P1.Name                                                                                                           ";

            //var parLevel1List = (List<ParLevel1>)db.Query<ParLevel1>(sql);


            var parLevel1List = db.Query<ParLevel1>(sql);

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
        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
        public decimal Nivel1 { get; set; }
        public decimal Nivel2 { get; set; }
        public string Nivel3 { get; set; }
        public decimal VolumeAlerta { get; set; }
        public decimal Meta { get; set; }


        //public DateTime DataCollect { get; set; }
        //public int? Evaluate { get; set; }
        //public int? Sample { get; set; }
        //public int? ParCompany_Id_Evaluate { get; set; }
        //public int? ParCompany_Id_Sample { get; set; }
        private SqlConnection db { get; set; }
        public ParLevel1Alertas() { }
        public ParLevel1Alertas(SqlConnection _db)
        {
            db = _db;
        }
        public ParLevel1Alertas getAlertas(int ParLevel1_Id, int ParCompany_Id, DateTime DateCollect)
        {


            string _DataCollect = DateCollect.ToString("yyyyMMdd");

            string sql = "";

            sql = "\n SELECT " +
                    "\n cast(SUM((VolumeAlerta * (Meta/100))) as varchar)		AS nivel3    " +
                    "\n ,SUM((VolumeAlerta * (Meta / 100)) / 3 * 2)   AS nivel2 " +
                    "\n , SUM((VolumeAlerta * (Meta / 100)) / 3)     AS nivel1 " +
                    "\n , sum(VolumeAlerta) AS VolumeAlerta " +
                    "\n , AVG(Meta) AS Meta " +
                    "\n   FROM " +
                    "\n   ( " +
                    "\n       SELECT " +
                    "\n       CASE " +
                    "\n           WHEN ParConsolidationType_Id = 1 THEN AV * AM * PESO " +
                    "\n           WHEN ParConsolidationType_Id = 2 THEN AV * AM * (CASE WHEN PESO > 0 THEN 1 ELSE 0 END) " +
                    "\n        ELSE AV * AM * PESO " +
                    "\n        END AS VolumeAlerta " +
                    "\n       , _META AS Meta " +
                    "\n       , * " +
                    "\n       FROM " +
                    "\n           ( " +
                    "\n           SELECT  RESULT.* " +
                    "\n                  , (SELECT ParConsolidationType_Id FROM ParLevel1  (nolock) WHERE Id = " + ParLevel1_Id + ") AS ParConsolidationType_Id " +
                    "\n                  , (SELECT TOP 1 PercentValue FROM ParGoal (nolock)  WHERE ParLevel1_Id = " + ParLevel1_Id + " AND(ParCompany_Id = " + ParCompany_Id + " OR ParCompany_Id IS NULL) ORDER BY ParCompany_id DESC) AS _META " +
                    "\n                FROM " +
                    "\n         ( " +
                    "\n             /*****PCC1b**************************************************************************************************************************************************************************/ " +
                    "\n             SELECT * FROM " +
                    "\n             (SELECT TOP 1 1 AS \"hashKey\" " +
                    "\n             , (SELECT Id FROM ParLevel1  (nolock) WHERE hashKey = 1) AS ID " +
                    "\n             , 'PCC1b' AS INDICADOR, 1 AS AV, COALESCE(Amostras, 0) * 2 AS AM, 1 AS PESO FROM VolumePcc1b (nolock)  WHERE ParCompany_id = " + ParCompany_Id + " and CONVERT(DATE, Data) = CONVERT(DATE, '" + _DataCollect + "')) PCC " +
                    "\n               /************************************************************************************************************************************************************************************/ " +
                    "\n               UNION ALL " +
                    "\n            /*****CEP VÁCUO GRD******************************************************************************************************************************************************************/ " +
                    "\n            SELECT * FROM " +
                    "\n            (SELECT TOP 1 3 AS \"hashKey\" " +
                    "\n            , (SELECT Id FROM ParLevel1  (nolock) WHERE hashKey = 3) AS ID " +
                    "\n             ,'CEP VÁCUO GRD' AS INDICADOR, Avaliacoes AS AV, Amostras *QtdadeFamiliaProduto AS AM, 1 AS PESO FROM VolumeVacuoGRD (nolock)  WHERE ParCompany_id = " + ParCompany_Id + " and CONVERT(DATE, Data) <= CONVERT(DATE, '" + _DataCollect + "') ORDER BY Data DESC) GRD " +
                    "\n            /************************************************************************************************************************************************************************************/ " +
                    "\n            UNION ALL " +
                    "\n            /*****CEP DESOSSA********************************************************************************************************************************************************************/ " +
                    "\n            SELECT * FROM " +
                    "\n            (SELECT TOP 1 2 AS \"hashKey\" " +
                    "\n            , (SELECT Id FROM ParLevel1 (nolock)  WHERE hashKey = 2) AS ID " +
                    "\n             ,'CEP DESOSSA' AS INDICADOR, Avaliacoes AS AV, Amostras *QtdadeFamiliaProduto AS AM, 1 AS PESO FROM VolumeCepDesossa (nolock)  WHERE ParCompany_id = " + ParCompany_Id + " and CONVERT(DATE, Data) <= CONVERT(DATE, '" + _DataCollect + "') ORDER BY Data DESC) DESOSSA " +
                    "\n            /************************************************************************************************************************************************************************************/ " +
                    "\n            UNION ALL " +
                    "\n            /*****CEP RECORTES*******************************************************************************************************************************************************************/ " +
                    "\n            SELECT* FROM " +
                    "\n            (SELECT TOP 1 4 AS \"hashKey\" " +
                    "\n            , (SELECT Id FROM ParLevel1  (nolock) WHERE hashKey = 4) AS ID " +
                    "\n             ,'CEP RECORTES' AS INDICADOR, Avaliacoes AS AV, Amostras AS AM, 1 AS PESO FROM VolumeCepRecortes (nolock)  WHERE ParCompany_id = " + ParCompany_Id + " and CONVERT(DATE, Data) <= CONVERT(DATE, '" + _DataCollect + "') ORDER BY Data DESC) RECORTES " +
                    "\n             /************************************************************************************************************************************************************************************/ " +
                    "\n             UNION ALL " +
                    "\n             /*****OUTROS*************************************************************************************************************************************************************************/ " +
                    "\n             SELECT " +
                    "\n             hashKey AS hasKey " +
                    "\n			,Id AS Id " +
                    "\n			,INDICADOR AS INDICADOR " +
                    "\n			,AVALIACOES AS AV " +
                    "\n			,AMOSTRAS AS AM " +
                    "\n			,PESO_DA_TAREFA AS PESO " +
                    "\n            FROM " +
                    "\n            ( " +
                    "\n                SELECT " +
                    "\n                 INDICADOR.* " +
                    "\n               , MON.Name AS MONITORAMENTO " +
                    "\n                , TAR.Name AS TAREFA " +
                    "\n                , MONITORAMENTOS.Weight AS \"PESO_DA_TAREFA\" " +
                    "\n                , (SELECT TOP 1 Number FROM ParEvaluation (nolock)  WHERE ParLevel2_Id = MON.Id AND(ParCompany_Id = " + ParCompany_Id + " OR ParCompany_Id IS NULL) ORDER BY ParCompany_Id DESC) AS AVALIACOES " +
                    "\n                ,(SELECT TOP 1 Number FROM ParSample (nolock)      WHERE ParLevel2_Id = MON.Id AND(ParCompany_Id = " + ParCompany_Id + " OR ParCompany_Id IS NULL) ORDER BY ParCompany_Id DESC) AS AMOSTRAS " +
                    "\n                FROM " +
                    "\n                ----INDICADOR-------------------------------------------------- " +
                    "\n                ( " +
                    "\n                    SELECT " +
                    "\n                     IND.Id                             AS \"Id\" " +
                    "\n                    , IND.\"hashKey\"                      AS \"hashKey\" " +
                    "\n                    , IND.Name                           AS \"INDICADOR\" " +
                    "\n                    , IND.hashKey                        AS \"CÓDIGO ESPECÍFICO\" " +
                    "\n                    , Cons.Name                          AS \"TIPO_DE_CONSOLIDACAO\" " +
                    "\n                    , IND.HasAlert                       AS \"EMITE_ALERTA ? \" " +
                    "\n                    , IND.IsSpecific                     AS \"ESPECÍFICO\" " +
                    "\n                    , IND.IsSpecificNumberEvaluetion     AS \"ESPECÍFICO - AVALIAÇÕES\" " +
                    "\n                    , IND.IsSpecificNumberSample         AS \"ESPECÍFICO - AMOSTRAS\" " +
                    "\n                    , IND.IsFixedEvaluetionNumber        AS \"ESPECÍFICO - FAMÍLIA DE PRODUTOS\" " +
                    "\n                    FROM ParLevel1 IND  (nolock) " +
                    "\n                    LEFT JOIN ParConsolidationType Cons  (nolock) " +
                    "\n                    ON Cons.Id = IND.ParConsolidationType_Id " +
                    "\n                   WHERE IND.Id = " + ParLevel1_Id + " " +
                    "\n                )INDICADOR " +
                    "\n                INNER JOIN " +
                    "\n                ----MONITORAMENTO---------------------------------------------- " +
                    "\n                ( " +
                    "\n                    SELECT " +
                    "\n                     " + ParLevel1_Id + " AS INDICADOR " +
                    "\n                    , * " +
                    "\n                    FROM " +
                    "\n                    ( " +
                    "\n                        SELECT " +
                    "\n                         I_M_T.ParLevel3Level2_Id " +
                    "\n                        , MAX(I_M_T.ParCompany_Id) AS ParCompany_Id " +
                    "\n                        FROM ParLevel3Level2Level1 I_M_T  (nolock) " +
                    "\n                        WHERE I_M_T.ParLevel1_Id = " + ParLevel1_Id + " " +
                    "\n                        AND(I_M_T.ParCompany_Id = " + ParCompany_Id + " " +
                    "\n                         OR I_M_T.ParCompany_Id IS NULL) " +
                    "\n                        GROUP BY I_M_T.ParLevel3Level2_Id " +
                    "\n                    ) M1 " +
                    "\n                    INNER JOIN " +
                    "\n                    ( " +
                    "\n                        SELECT " +
                    "\n                         (SELECT TOP 1 Id     FROM ParLevel3Level2  (nolock) WHERE ParLevel2_Id = M_T.ParLevel2_Id AND ParLevel3_Id = M_T.ParLevel3_Id AND COALESCE(ParCompany_Id, 0) = MAX(COALESCE(M_T.ParCompany_Id, 0))) AS Id " +
                    "\n                        , (SELECT TOP 1 Weight FROM ParLevel3Level2  (nolock) WHERE ParLevel2_Id = M_T.ParLevel2_Id AND ParLevel3_Id = M_T.ParLevel3_Id AND COALESCE(ParCompany_Id, 0) = MAX(COALESCE(M_T.ParCompany_Id, 0))) AS Weight " +
                    "\n                        , M_T.ParLevel2_Id " +
                    "\n                        , M_T.ParLevel3_Id " +
                    "\n                        , MAX(M_T.ParCompany_Id) AS _ParCompany_Id " +
                    "\n                        FROM ParLevel3Level2 M_T  (nolock) " +
                    "\n                        INNER JOIN ParLevel3 TAR (nolock)  " +
                    "\n                        ON TAR.Id = M_T.ParLevel3_Id " +
                    "\n                        AND TAR.IsActive = 1 " +
                    "\n                        INNER JOIN ParLevel2 MON (nolock)  " +
                    "\n                        ON MON.Id = M_T.ParLevel2_Id " +
                    "\n                        AND MON.IsActive = 1 " +
                    "\n                        WHERE(M_T.ParCompany_Id = " + ParCompany_Id + " " +
                    "\n                        OR M_T.ParCompany_Id IS NULL) " +
                    "\n                        GROUP BY " +
                    "\n                         M_T.ParLevel2_Id " +
                    "\n                        , M_T.ParLevel3_Id " +
                    "\n                    ) M2 " +
                    "\n                    ON M1.ParLevel3Level2_Id = M2.Id " +
                    "\n				) MONITORAMENTOS " +
                    "\n                ON INDICADOR.ID = MONITORAMENTOS.INDICADOR " +
                    "\n                INNER JOIN ParLevel2 MON (nolock)  " +
                    "\n                ON MON.Id = MONITORAMENTOS.ParLevel2_Id " +
                    "\n                INNER JOIN ParLevel3 TAR  (nolock) " +
                    "\n                ON TAR.Id = MONITORAMENTOS.ParLevel3_Id " +
                    "\n                WHERE hashKey IS NULL " +
                    "\n			) OUTROS " +
                    "\n/************************************************************************************************************************************************************************************/ " +
                    "\n		) RESULT " +
                    "\n        WHERE RESULT.ID = " + ParLevel1_Id + " " +
                    "\n	) SELECAO " +
                    "\n) FIM  ";


            var parLevel2List = db.Query<ParLevel1Alertas>(sql).FirstOrDefault();

            return parLevel2List;
        }
    }

    public partial class ParLevel2
    {
        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
        public int Id { get; set; }
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

        public int getExisteAvaliacao(int ParCompany_Id, int ParLevel2_Id)
        {

            //SqlConnection db = new SqlConnection(conexao);

            string sql = "" +
                "\n  select count(1) from " +
                "\n ( " +
                "\n select * from ParEvaluation (nolock)  where ParLevel2_id = " + ParLevel2_Id + " and ParCompany_Id = " + ParCompany_Id + " AND IsActive = 1 " +
                "\n union all " +
                "\n select * from ParEvaluation (nolock)  where ParLevel2_id = " + ParLevel2_Id + " and ParCompany_Id is Null  AND IsActive = 1 " +
                "\n ) temAv ";

            SqlCommand command = new SqlCommand(sql, db);
            return command.ExecuteNonQuery();

        }

        public int getExisteAmostra(int ParCompany_Id, int ParLevel2_Id)
        {

            //SqlConnection db = new SqlConnection(conexao);

            string sql = "" +
                "\n  select count(1) from " +
                "\n ( " +
                "\n select * from ParSample (nolock)  where ParLevel2_id = " + ParLevel2_Id + " and ParCompany_Id = " + ParCompany_Id + " and IsActive = 1 " +
                "\n union all " +
                "\n select * from ParSample (nolock)  where ParLevel2_id = " + ParLevel2_Id + " and ParCompany_Id is Null  and IsActive = 1 " +
                "\n ) temAm ";

            SqlCommand command = new SqlCommand(sql, db);
            return command.ExecuteNonQuery();
        }

        //public int? Evaluate { get; set; }
        //public int? Sample { get; set; }
        //public int? ParCompany_Id_Evaluate { get; set; }
        //public int? ParCompany_Id_Sample { get; set; }
        public ParLevel2()
        {

        }
        public ParLevel2 getById(int Id)
        {
            try
            {
                //SqlConnection db = new SqlConnection(conexao);
                string sql = "SELECT * FROM ParLevel2 (nolock)  WHERE Id='" + Id + "'";
                var parLevel1List = db.Query<ParLevel2>(sql).FirstOrDefault();
                return parLevel1List;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<ParLevel2> getLevel2ByIdLevel1(SGQDBContext.ParLevel1 parLevel1, DateTime dateCollection, int ParCompany_Id)
        {
            //SqlConnection db = new SqlConnection(conexao);            

            //bool parLevel1Familia = false;

            //using (var dbEf = new SgqDbDevEntities())
            //{

            //    var level1 = parLevel1db.getByFamilia(dateCollection, ParLevel1_Id);

            //    if(level1.Count() > 0 || parLevel1db.IsFixedEvaluetionNumber == true)
            //    {
            //        parLevel1Familia = true;
            //    }
            //}

            /****CONTROLE DE FAMÍLIA DE PRODUTOS*****/

            if (parLevel1.IsFixedEvaluetionNumber == true)
            {
                string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name, PL2.HasSampleTotal, PL2.HasTakePhoto, PL2.IsEmptyLevel3, AL.ParNotConformityRule_id, AL.Value, AL.IsReaudit, PL2.ParFrequency_id " +
                             "\n FROM ParLevel3Level2 P32   (nolock)                                                                                                                             " +
                             "\n INNER JOIN ParLevel3Level2Level1 P321  (nolock)                                                                                                                 " +
                             "\n ON P321.ParLevel3Level2_Id = P32.Id                                                                                                                   " +
                             "\n INNER JOIN ParLevel2 PL2   (nolock)                                                                                                                             " +
                             "\n ON PL2.Id = P32.ParLevel2_Id                                                                                                                          " +
                             "\n  LEFT JOIN ParNotConformityRuleXLevel AL   (nolock)                                                                                                             " +
                             "\n  ON AL.ParLevel2_Id = PL2.Id  AND AL.IsActive = 1                                                                                                     " +
                             "\n INNER JOIN (SELECT * FROM ParLevel2ControlCompany PL (nolock)  INNER JOIN                                                                                       " +
                             "\n (SELECT MAX(InitDate) Data, ParCompany_Id AS UNIDADE FROM ParLevel2ControlCompany   (nolock)                                                                    " +
                             "\n where ParLevel1_Id = '" + parLevel1.Id + "' AND CAST(InitDate AS DATE) <= '" + dateCollection.ToString("yyyy-MM-dd") + "'  and (ParCompany_Id =  " + ParCompany_Id + " or ParCompany_Id is null)   and IsActive = 1 " +

                             "\n GROUP BY ParCompany_Id) F1 ON (CAST(F1.data AS DATE) = CAST(PL.initDate AS DATE) AND PL.IsActive = 1) OR (CAST(f1.data AS DATE) = CAST(PL.initDate AS DATE) AND CAST(f1.data AS DATE) < CAST(PL.AlterDate AS DATE) AND PL.IsActive = 1) AND (F1.UNIDADE = PL.ParCompany_id                                                                " +
                             "\n or F1.UNIDADE is null))  Familia                                                                                                                      " +
                             "\n ON Familia.ParLevel2_Id = PL2.Id                                                                                                                      " +
                             "\n WHERE P321.ParLevel1_Id = " + parLevel1.Id + "                                                                                                      " +
                             "\n AND PL2.IsActive = 1     " +
                             "\n AND (Familia.ParCompany_Id = " + ParCompany_Id + "  or Familia.ParCompany_Id IS NULL)                                                               " +
                             "\n GROUP BY PL2.Id, PL2.Name, PL2.HasSampleTotal, PL2.IsEmptyLevel3, AL.ParNotConformityRule_Id, AL.IsReaudit, AL.Value, PL2.ParFrequency_id, PL2.HasTakePhoto             ";

                var parLevel2List = db.Query<ParLevel2>(sql);

                return parLevel2List;

            }
            else
            {





                string sql = "\n SELECT PL2.Id AS Id, PL2.Name AS Name, PL2.HasSampleTotal, PL2.HasTakePhoto, PL2.IsEmptyLevel3, AL.ParNotConformityRule_id, AL.Value, AL.IsReaudit,PL2.ParFrequency_id  " +
                         "\n FROM ParLevel3Level2 P32                                      " +
                         "\n INNER JOIN ParLevel3Level2Level1 P321                         " +
                         "\n ON P321.ParLevel3Level2_Id = P32.Id                           " +
                         "\n INNER JOIN ParLevel2 PL2                                      " +
                         "\n ON PL2.Id = P32.ParLevel2_Id                                  " +
                         "\n LEFT JOIN ParNotConformityRuleXLevel AL                                                                                   " +
                         "\n ON AL.ParLevel2_Id = PL2.Id     AND AL.IsActive = 1                                                                                             " +
                        "\n WHERE P321.ParLevel1_Id = '" + parLevel1.Id + "'              " +
                         "\n AND PL2.IsActive = 1  AND P32.IsActive = 1 AND P321.Active = 1                                        " +

                         "\n AND " +
                         "\n  (select sum(a) from " +
                         "\n ( " +
                         "\n select number as a  from ParEvaluation (nolock)  where IsActive = 1 and ParLevel2_id = PL2.Id and ParCompany_Id = " + ParCompany_Id + " " +
                         "\n union all " +
                         "\n select number as a  from ParEvaluation (nolock)  where IsActive = 1 and ParLevel2_id = PL2.Id and ParCompany_Id is Null " +
                         "\n ) temAv) > 0 " +

                         "\n AND " +
                         "\n  (select sum(a) from " +
                         "\n ( " +
                         "\n select number as a  from ParSample  (nolock) where IsActive = 1 and ParLevel2_id = PL2.Id and ParCompany_Id = " + ParCompany_Id + " " +
                         "\n union all " +
                         "\n select number as a  from ParSample  (nolock) where IsActive = 1 and ParLevel2_id = PL2.Id and ParCompany_Id is Null " +
                         "\n ) temAm) > 0 " +

                         "\n GROUP BY PL2.Id, PL2.Name, PL2.HasSampleTotal, PL2.IsEmptyLevel3, AL.ParNotConformityRule_Id, AL.IsReaudit, AL.Value, PL2.ParFrequency_id, PL2.HasTakePhoto                 " +
                         "\n ";

                var parLevel2List = db.Query<ParLevel2>(sql);

                return parLevel2List;
            }
        }
    }

    public partial class ParLevel2Evaluate
    {
        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public int Id { get; set; }
        public string Name { get; set; }
        public int Evaluate { get; set; }
        //public int? ParCompany_Id { get; set; }

        private SqlConnection db { get; set; }
        public ParLevel2Evaluate() { }
        public ParLevel2Evaluate(SqlConnection _db)
        {
            db = _db;
        }

        public IEnumerable<ParLevel2Evaluate> getEvaluate(ParLevel1 ParLevel1, int? ParCompany_Id, DateTime DateCollection)
        {

            //SqlConnection db = new SqlConnection(conexao);
            string queryCompany = null;

            var date = DateCollection.ToString("yyyy-MM-dd");


            if (ParLevel1.hashKey == 2 && ParCompany_Id != null)
            {

                string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name,              " +
                             "(SELECT top 1 Avaliacoes FROM VolumeCepDesossa (nolock)  WHERE Data = (SELECT MAX(DATA) FROM VolumeCepDesossa (nolock)  WHERE ParCompany_id = " + ParCompany_Id + " AND CAST(DATA AS DATE) <= '" + date + "') and ParCompany_id = " + ParCompany_Id + " ORDER BY ID DESC) AS Evaluate " +
                             "FROM                                                                        " +
                             "ParLevel3Level2 P32  (nolock)                                                         " +
                             "INNER JOIN ParLevel3Level2Level1 P321  (nolock)                                       " +
                             "ON P321.ParLevel3Level2_Id = P32.Id                                         " +
                             "INNER JOIN ParLevel2 PL2    (nolock)                                                  " +
                             "ON PL2.Id = P32.ParLevel2_Id                                                " +

                             "WHERE P321.ParLevel1_Id = '" + ParLevel1.Id + "'                            " +
                             "GROUP BY PL2.Id, PL2.Name                                                   ";

                var parEvaluate = db.Query<ParLevel2Evaluate>(sql);


                return parEvaluate;


            }
            else if (ParLevel1.hashKey == 3 && ParCompany_Id != null)
            {

                string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name,              " +
                             "(SELECT TOP 1 Avaliacoes FROM VolumeVacuoGRD (nolock)  WHERE Data = (SELECT MAX(DATA) FROM VolumeVacuoGRD (nolock)  WHERE ParCompany_id = " + ParCompany_Id + " AND CAST(DATA AS DATE) <= '" + date + "') and ParCompany_id = " + ParCompany_Id + " ORDER BY ID DESC) AS Evaluate " +
                             "FROM                                                                        " +
                             "ParLevel3Level2 P32  (nolock)                                                         " +
                             "INNER JOIN ParLevel3Level2Level1 P321  (nolock)                                       " +
                             "ON P321.ParLevel3Level2_Id = P32.Id                                         " +
                             "INNER JOIN ParLevel2 PL2     (nolock)                                                 " +
                             "ON PL2.Id = P32.ParLevel2_Id                                                " +

                             "WHERE P321.ParLevel1_Id = '" + ParLevel1.Id + "'                            " +
                             "GROUP BY PL2.Id, PL2.Name                                                   ";

                var parEvaluate = db.Query<ParLevel2Evaluate>(sql);


                return parEvaluate;


            }
            else if (ParLevel1.hashKey == 4 && ParCompany_Id != null)
            {

                string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name,              " +
                             "(SELECT TOP 1 Avaliacoes FROM VolumeCepRecortes (nolock)  WHERE Data = (SELECT MAX(DATA) FROM VolumeCepRecortes (nolock)  WHERE ParCompany_id = " + ParCompany_Id + " AND CAST(DATA AS DATE) <= '" + date + "') and ParCompany_id = " + ParCompany_Id + " ORDER BY ID DESC) AS Evaluate " +
                             "FROM                                                                        " +
                             "ParLevel3Level2 P32   (nolock)                                                        " +
                             "INNER JOIN ParLevel3Level2Level1 P321    (nolock)                                     " +
                             "ON P321.ParLevel3Level2_Id = P32.Id                                         " +
                             "INNER JOIN ParLevel2 PL2      (nolock)                                                " +
                             "ON PL2.Id = P32.ParLevel2_Id                                                " +

                             "WHERE P321.ParLevel1_Id = '" + ParLevel1.Id + "'                            " +
                             "GROUP BY PL2.Id, PL2.Name                                                   ";

                var parEvaluate = db.Query<ParLevel2Evaluate>(sql);


                return parEvaluate;


            }
            else if (ParLevel1.hashKey == 1 && ParCompany_Id != null)
            {

                string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name,              " +
                             "(SELECT TOP 1 Avaliacoes FROM VolumePcc1b (nolock)  WHERE Data = (SELECT MAX(DATA) FROM VolumePcc1b (nolock)  WHERE ParCompany_id = " + ParCompany_Id + " AND CAST(DATA AS DATE) <= '" + date + "') and ParCompany_id = " + ParCompany_Id + " ORDER BY ID DESC) AS Evaluate " +
                             "FROM                                                                        " +
                             "ParLevel3Level2 P32    (nolock)                                                       " +
                             "INNER JOIN ParLevel3Level2Level1 P321   (nolock)                                      " +
                             "ON P321.ParLevel3Level2_Id = P32.Id                                         " +
                             "INNER JOIN ParLevel2 PL2     (nolock)                                                 " +
                             "ON PL2.Id = P32.ParLevel2_Id                                                " +

                             "WHERE P321.ParLevel1_Id = '" + ParLevel1.Id + "'                            " +
                             "GROUP BY PL2.Id, PL2.Name                                                   ";

                var parEvaluate = db.Query<ParLevel2Evaluate>(sql);


                return parEvaluate;


            }
            else
            {

                if (ParCompany_Id > 0)
                {
                    queryCompany = " AND PE.ParCompany_Id = '" + ParCompany_Id + "'";
                }
                else
                {
                    queryCompany = " AND PE.ParCompany_Id IS NULL ";
                }

                string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name, PE.Number AS Evaluate                " +
                             "FROM                                                                        " +
                             "ParLevel3Level2 P32    (nolock)                                                       " +
                             "INNER JOIN ParLevel3Level2Level1 P321   (nolock)                                      " +
                             "ON P321.ParLevel3Level2_Id = P32.Id                                         " +
                             "INNER JOIN ParLevel2 PL2   (nolock)                                                   " +
                             "ON PL2.Id = P32.ParLevel2_Id                                                " +
                             "INNER JOIN ParEvaluation PE   (nolock)                                                " +
                             "ON PE.ParLevel2_Id = PL2.Id                                                 " +
                             "WHERE P321.ParLevel1_Id = '" + ParLevel1.Id + "'                            " +
                             " AND PE.IsActive = 1 " +
                             queryCompany +
                             "GROUP BY PL2.Id, PL2.Name, PE.Number, PE.AlterDate, PE.AddDate, PE.ParCompany_Id              " +
                             "ORDER BY PE.ParCompany_Id  DESC, PE.AlterDate, PE.AddDate                                           ";

                // sql = "SELECT 67 AS Id, 'NC Desossa - Alcatra', 50 AS Evaluate";

                var parEvaluate = db.Query<ParLevel2Evaluate>(sql);
                return parEvaluate;

            }



        }



    }
    public partial class ParLevel2Sample
    {
        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public int Id { get; set; }
        public string Name { get; set; }
        public int Sample { get; set; }
        //public int? ParCompany_Id { get; set; }

        private SqlConnection db { get; set; }
        public ParLevel2Sample() { }
        public ParLevel2Sample(SqlConnection _db)
        {
            db = _db;
        }

        public IEnumerable<ParLevel2Sample> getSample(ParLevel1 ParLevel1, int? ParCompany_Id, DateTime DateCollection)
        {

            var date = DateCollection.ToString("yyyy-MM-dd");

            //SqlConnection db = new SqlConnection(conexao);
            string queryCompany = null;

            if (ParLevel1.hashKey == 2 && ParCompany_Id != null)
            {

                string sql = "SELECT  PL2.Id AS Id, PL2.Name AS Name,              " +
                             "(SELECT TOP 1 Amostras FROM VolumeCepDesossa (nolock)  WHERE Data = (SELECT MAX(DATA) FROM VolumeCepDesossa (nolock)  WHERE ParCompany_id = " + ParCompany_Id + " AND CAST(DATA AS DATE) <= '" + date + "') and ParCompany_id = " + ParCompany_Id + " ORDER BY ID DESC) AS Sample " +
                             "FROM                                                                        " +
                             "ParLevel3Level2 P32      (nolock)                                                     " +
                             "INNER JOIN ParLevel3Level2Level1 P321   (nolock)                                      " +
                             "ON P321.ParLevel3Level2_Id = P32.Id                                         " +
                             "INNER JOIN ParLevel2 PL2     (nolock)                                                 " +
                             "ON PL2.Id = P32.ParLevel2_Id                                                " +

                             "WHERE P321.ParLevel1_Id = '" + ParLevel1.Id + "'                            " +
                             "GROUP BY PL2.Id, PL2.Name                                                   ";

                var parSample = db.Query<ParLevel2Sample>(sql);


                return parSample;


            }
            else if (ParLevel1.hashKey == 3 && ParCompany_Id != null)
            {

                string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name,              " +
                             "(SELECT TOP 1  Amostras FROM VolumeVacuoGRD  (nolock) WHERE Data = (SELECT MAX(DATA) FROM VolumeVacuoGRD (nolock)  WHERE ParCompany_id = " + ParCompany_Id + " AND CAST(DATA AS DATE) <= '" + date + "') and ParCompany_id = " + ParCompany_Id + " ORDER BY ID DESC) AS Sample " +
                             "FROM                                                                        " +
                             "ParLevel3Level2 P32   (nolock)                                                        " +
                             "INNER JOIN ParLevel3Level2Level1 P321       (nolock)                                  " +
                             "ON P321.ParLevel3Level2_Id = P32.Id                                         " +
                             "INNER JOIN ParLevel2 PL2     (nolock)                                                 " +
                             "ON PL2.Id = P32.ParLevel2_Id                                                " +

                             "WHERE P321.ParLevel1_Id = '" + ParLevel1.Id + "'                            " +
                             "GROUP BY PL2.Id, PL2.Name                                                   ";

                var parSample = db.Query<ParLevel2Sample>(sql);


                return parSample;


            }
            else if (ParLevel1.hashKey == 4 && ParCompany_Id != null)
            {

                string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name,              " +
                             "(SELECT  TOP 1 Amostras FROM VolumeCepRecortes  (nolock) WHERE Data = (SELECT MAX(DATA) FROM VolumeCepRecortes (nolock)  WHERE ParCompany_id = " + ParCompany_Id + " AND CAST(DATA AS DATE) <= '" + date + "') and ParCompany_id = " + ParCompany_Id + " ORDER BY ID DESC) AS Sample " +
                             "FROM                                                                        " +
                             "ParLevel3Level2 P32    (nolock)                                                       " +
                             "INNER JOIN ParLevel3Level2Level1 P321  (nolock)                                       " +
                             "ON P321.ParLevel3Level2_Id = P32.Id                                         " +
                             "INNER JOIN ParLevel2 PL2  (nolock)                                                    " +
                             "ON PL2.Id = P32.ParLevel2_Id                                                " +

                             "WHERE P321.ParLevel1_Id = '" + ParLevel1.Id + "'                            " +
                             "GROUP BY PL2.Id, PL2.Name                                                   ";

                var parSample = db.Query<ParLevel2Sample>(sql);


                return parSample;


            }
            else if (ParLevel1.hashKey == 1 && ParCompany_Id != null)
            {

                string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name,              " +
                             "(SELECT TOP 1 Amostras FROM VolumePcc1b (nolock)  WHERE Data = (SELECT MAX(DATA) FROM VolumePcc1b (nolock)  WHERE ParCompany_id = " + ParCompany_Id + " AND CAST(DATA AS DATE) <= '" + date + "') and ParCompany_id = " + ParCompany_Id + " ORDER BY ID DESC) AS Sample " +
                             "FROM                                                                        " +
                             "ParLevel3Level2 P32  (nolock)                                                         " +
                             "INNER JOIN ParLevel3Level2Level1 P321     (nolock)                                    " +
                             "ON P321.ParLevel3Level2_Id = P32.Id                                         " +
                             "INNER JOIN ParLevel2 PL2        (nolock)                                              " +
                             "ON PL2.Id = P32.ParLevel2_Id                                                " +

                             "WHERE P321.ParLevel1_Id = '" + ParLevel1.Id + "'                            " +
                             "GROUP BY PL2.Id, PL2.Name                                                   ";

                var parSample = db.Query<ParLevel2Sample>(sql);


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

                string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name, PS.Number AS Sample FROM  " +
                             "ParLevel3Level2 P32  (nolock)                                              " +
                             "INNER JOIN ParLevel3Level2Level1 P321   (nolock)                           " +
                             "ON P321.ParLevel3Level2_Id = P32.Id                              " +
                             "INNER JOIN ParLevel2 PL2     (nolock)                                      " +
                             "ON PL2.Id = P32.ParLevel2_Id                                     " +
                             "INNER JOIN ParSample PS     (nolock)                                       " +
                             "ON PS.ParLevel2_Id = PL2.Id                                      " +
                             "WHERE P321.ParLevel1_Id = '" + ParLevel1.Id + "'                 " +
                             " AND PS.IsActive = 1 " +
                             queryCompany +
                             "GROUP BY PL2.Id, PL2.Name, PS.Number, PS.ParCompany_Id, PS.AlterDate, PS.AddDate, PS.ParCompany_Id           " +
                             "ORDER BY PS.ParCompany_Id desc, PS.AlterDate DESC, PS.AddDate DESC                                ";

                var parSample = db.Query<ParLevel2Sample>(sql);

                return parSample;
            }
        }



    }
    public partial class ParLevel3
    {
        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

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
            //SqlConnection db = new SqlConnection(conexao);
            string sql = "SELECT Id, Name FROM ParLevel3 (nolock) ";
            var parLevel3List = db.Query<ParLevel3>(sql);

            return parLevel3List;

        }

        public IEnumerable<ParLevel3> getListPerLevel1Id(int ParLevel1_Id)
        {
            //SqlConnection db = new SqlConnection(conexao);
            string sql = "SELECT P3.Id, P3.Name FROM ParLevel3Level2Level1 P321  (nolock) INNER JOIN ParLevel3Level2 P32 (nolock)  ON P32.Id = P321.ParLevel3Level2_Id INNER JOIN ParLevel3 P3 (nolock)  ON P3.Id = P32.ParLevel3_Id WHERE P321.ParLevel1_Id = " + ParLevel1_Id.ToString();
            var parLevel3List = db.Query<ParLevel3>(sql);

            return parLevel3List;

        }

        //
        public IEnumerable<ParLevel3> getLevel3ByLevel2(SGQDBContext.ParLevel1 ParLevel1, SGQDBContext.ParLevel2 ParLevel2, int ParCompany_Id, DateTime DateCollect)
        {
            //SqlConnection db = new SqlConnection(conexao);

            //var syncServices = new SgqSystem.Services.SyncServices();

            //Instanciamos variavel de data
            string dataInicio = null;
            string dataFim = null;

            //Pega a data pela regra da frequencia
            SyncServices.getFrequencyDate(ParLevel2.ParFrequency_Id, DateCollect, ref dataInicio, ref dataFim);

            //string queryResult = null;
            //if(ParLevel1.IsPartialSave == true)
            //{
            //    queryResult = "/*tirar as tarefas que já foram lançadas no período */" +

            //                  "AND L3.Id NOT IN " +
            //                  "( " +
            //                  "SELECT R3.ParLevel3_Id FROM RESULT_LEVEL3 R3 " +
            //                  "INNER JOIN CollectionLevel2 C2 " +
            //                  "ON C2.Id = R3.CollectionLevel2_Id " +
            //                  "INNER JOIN( " +
            //                  "SELECT L3.Id as ParLevel3_Id, L2.Id as ParLevel2_Id, L321.ParLevel1_Id as ParLevel1_Id " +
            //                  "FROM ParLevel3 AS L3 INNER JOIN " +
            //                  "ParLevel3Value AS L3V ON L3V.ParLevel3_Id = L3.Id INNER JOIN " +
            //                  "ParLevel3InputType AS L3IT ON L3IT.Id = L3V.ParLevel3InputType_Id LEFT OUTER JOIN " +
            //                  "ParLevel3BoolFalse AS L3BF ON L3BF.Id = L3V.ParLevel3BoolFalse_Id LEFT OUTER JOIN " +
            //                  "ParLevel3BoolTrue AS L3BT ON L3BT.Id = L3V.ParLevel3BoolTrue_Id LEFT OUTER JOIN " +
            //                  "ParMeasurementUnit AS MU ON MU.Id = L3V.ParMeasurementUnit_Id LEFT OUTER JOIN " +
            //                  "ParLevel3Level2 AS L32 ON L32.ParLevel3_Id = L3.Id LEFT OUTER JOIN " +
            //                  "ParLevel3Group AS L3G ON L3G.Id = L32.ParLevel3Group_Id INNER JOIN " +
            //                  "ParLevel2 AS L2 ON L2.Id = L32.ParLevel2_Id INNER JOIN " +
            //                  "ParLevel3Level2Level1 AS L321 ON L321.ParLevel3Level2_Id = L32.Id " +
            //                  "WHERE(L3.IsActive = 1) AND(L32.IsActive = 1) AND(L2.Id = '" + ParLevel2.Id + "') AND(L32.ParCompany_Id = '" + ParCompany_Id + "' OR " +
            //                  "                  L32.ParCompany_Id IS NULL) AND L321.ParLevel1_Id = '" + ParLevel1.Id + "' " +
            //                  "GROUP BY L321.ParLevel1_Id, L2.Id, L3.Id, L3.Name, L3G.Id, L3G.Name, L3IT.Id, L3IT.Name, L3V.ParLevel3BoolFalse_Id, L3BF.Name, L3V.ParLevel3BoolTrue_Id, L3BT.Name, L3V.IntervalMin, L3V.IntervalMax, MU.Name, L32.Weight, " +
            //                  "                   L32.ParCompany_Id " +
            //                  ") TAREFAS " +
            //                  "ON TAREFAS.ParLevel3_Id = R3.ParLevel3_Id AND TAREFAS.ParLevel2_Id = C2.ParLevel2_Id AND TAREFAS.ParLevel1_Id = C2.ParLevel1_Id " +
            //                  "AND C2.UnitId = '" + ParCompany_Id + "' " +
            //                  "AND C2.CollectionDate BETWEEN '" + dataInicio + " 00:00:00' AND '" + dataFim + " 23:59:59' " +

            //                   ") " +
            //                    "/****************************************************/ ";

            //}

            string sql = "\n SELECT L3.Id AS Id, L3.Name AS Name, L3G.Id AS ParLevel3Group_Id, L3G.Name AS ParLevel3Group_Name, L3IT.Id AS ParLevel3InputType_Id, L3IT.Name AS ParLevel3InputType_Name, L3V.ParLevel3BoolFalse_Id AS ParLevel3BoolFalse_Id, L3BF.Name AS ParLevel3BoolFalse_Name, L3V.ParLevel3BoolTrue_Id AS ParLevel3BoolTrue_Id, L3BT.Name AS ParLevel3BoolTrue_Name, " +
                         "\n ISNULL(L3V.IntervalMin, -9999999999999.9) AS IntervalMin, ISNULL(L3V.IntervalMax, 9999999999999.9) AS IntervalMax, MU.Name AS ParMeasurementUnit_Name, L32.Weight AS Weight, L3V.ParCompany_Id , L32.ParCompany_Id                                                                                                                                                                                                                                       " +
                         "\n FROM ParLevel3 L3      (nolock)                                                                                                                                                                                                                                                                                                                                       " +
                         "\n INNER JOIN ParLevel3Value L3V      (nolock)                                                                                                                                                                                                                                                                                                                           " +
                         "           ON L3V.Id = (SELECT top 1 id FROM ParLevel3Value  (nolock) where isactive = 1 and ParLevel3_id = L3.Id and (ParCompany_id = " + ParCompany_Id + " or ParCompany_id is null) order by ParCompany_Id desc" +
                        // "\n and (ParLevel1_id = " + ParLevel1.Id + " or ParLevel1_id is null) and (ParLevel2_id = " + ParLevel2.Id + " or ParLevel2_id is null) order by ParCompany_Id desc, ParLevel2_Id desc, ParLevel1_Id desc)                                                                                                       " +
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
                         "\n WHERE  L3.IsActive = 1 AND L32.IsActive = 1                                                                                                                                                                                                                                                                                                                 " +
                         "\n  AND L2.Id = '" + ParLevel2.Id + "' " +
                         "\n  AND(L32.ParCompany_Id = '" + ParCompany_Id + "' OR L32.ParCompany_Id IS NULL) " +
                         "\n  AND(L3V.ParCompany_Id = '" + ParCompany_Id + "' OR L3V.ParCompany_Id IS NULL) " +
                         "\n  AND L321.ParLevel1_Id='" + ParLevel1.Id + "'                                                                                                        " +


                         //queryResult + 


                         "\n  GROUP BY " +
                            "\n    L321.ParLevel1_Id " +
                            "\n  , L2.Id " +
                            "\n  , L3.Id " +
                            "\n  , L3.Name " +
                            "\n  , L3G.Id " +
                            "\n  , L3G.Name " +
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
                            "\n  , L32.ParCompany_Id ";


            /*
             * MOCK GABRIEL PARA TESTE DE TRAZER TAREFAS DO OUTRO INDICADOR
             * 30/03/2017
             */

            string possuiIndicadorFilho = "SELECT cast(id as varchar(153)) as retorno FROM ParLevel1  (nolock) WHERE ParLevel1Origin_Id = " + ParLevel1.Id.ToString();
            string ParLevel1Origin_Id = "";

            using (var db = new Dominio.SgqDbDevEntities())
            {
                var list = db.Database.SqlQuery<ResultadoUmaColuna>(possuiIndicadorFilho).ToList();

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
                string IndicadorFilhoPeso = "SELECT cast(PointsDestiny as varchar(3)) as retorno FROM ParLevel1  (nolock) WHERE ParLevel1Origin_Id = " + ParLevel1.Id.ToString();

                using (var db = new Dominio.SgqDbDevEntities())
                {
                    var list = db.Database.SqlQuery<ResultadoUmaColuna>(IndicadorFilhoPeso).ToList();

                    for (var i = 0; i < list.Count(); i++)
                    {
                        IndicadorFilhoPeso = list[i].retorno.ToString();
                    }
                }

                if (IndicadorFilhoPeso == "0")
                {
                    sqlPeso = "0";
                }

                string ParLevel1_IdFilho = " AND L321.ParLevel1_Id IN (" + ParLevel1Origin_Id + ")";

                sqlFilho = "UNION ALL SELECT L3.Id AS Id, L3.Name AS Name, L3G.Id AS ParLevel3Group_Id, L3G.Name AS ParLevel3Group_Name, L3IT.Id AS ParLevel3InputType_Id, L3IT.Name AS ParLevel3InputType_Name, L3V.ParLevel3BoolFalse_Id AS ParLevel3BoolFalse_Id, L3BF.Name AS ParLevel3BoolFalse_Name, L3V.ParLevel3BoolTrue_Id AS ParLevel3BoolTrue_Id, L3BT.Name AS ParLevel3BoolTrue_Name, " +
                        " ISNULL(L3V.IntervalMin, -9999999999999.9) AS IntervalMin, ISNULL(L3V.IntervalMax, 9999999999999.9) AS IntervalMax, MU.Name AS ParMeasurementUnit_Name, " + sqlPeso + " AS Weight, L3V.ParCompany_Id , L32.ParCompany_Id                                                                                                                                                                                                                                   " +
                        "FROM ParLevel3 L3     (nolock)                                                                                                                                                                                                                                                                                                                                        " +
                        "INNER JOIN ParLevel3Value L3V     (nolock)                                                                                                                                                                                                                                                                                                                            " +
                        "        ON L3V.Id = (SELECT top 1 id FROM ParLevel3Value  (nolock) where isactive = 1 and ParLevel3_id = L3.Id and (ParCompany_id = " + ParCompany_Id + " or ParCompany_id is null) order by ParCompany_Id desc)                                                                                                                                                                                                                                                                                                                       " +
                        "INNER JOIN ParLevel3InputType L3IT  (nolock)                                                                                                                                                                                                                                                                                                                          " +
                        "        ON L3IT.Id = L3V.ParLevel3InputType_Id                                                                                                                                                                                                                                                                                                              " +
                        "LEFT JOIN ParLevel3BoolFalse L3BF   (nolock)                                                                                                                                                                                                                                                                                                                          " +
                        "        ON L3BF.Id = L3V.ParLevel3BoolFalse_Id                                                                                                                                                                                                                                                                                                              " +
                        "LEFT JOIN ParLevel3BoolTrue L3BT    (nolock)                                                                                                                                                                                                                                                                                                                          " +
                        "        ON L3BT.Id = L3V.ParLevel3BoolTrue_Id                                                                                                                                                                                                                                                                                                               " +
                        "LEFT JOIN ParMeasurementUnit MU   (nolock)                                                                                                                                                                                                                                                                                                                            " +
                        "        ON MU.Id = L3V.ParMeasurementUnit_Id                                                                                                                                                                                                                                                                                                                " +
                        "LEFT JOIN ParLevel3Level2 L32    (nolock)                                                                                                                                                                                                                                                                                                                             " +
                        "        ON L32.ParLevel3_Id = L3.Id                                                                                                                                                                                                                                                                                                                         " +
                        "LEFT JOIN ParLevel3Group L3G  (nolock)                                                                                                                                                                                                                                                                                                                                " +
                        "        ON L3G.Id = L32.ParLevel3Group_Id                                                                                                                                                                                                                                                                                                                   " +
                        "INNER JOIN ParLevel2 L2     (nolock)                                                                                                                                                                                                                                                                                                                                  " +
                        "        ON L2.Id = L32.ParLevel2_Id                                                                                                                                                                                                                                                                                                                         " +
                        "INNER JOIN ParLevel3Level2Level1 AS L321  (nolock) ON L321.ParLevel3Level2_Id = L32.Id                                                                                                                                                                                                                                                                                 " +
                        "WHERE  L3.IsActive = 1 AND L32.IsActive = 1                                                                                                                                                                                                                                                                                                                 " +

                        " AND(L32.ParCompany_Id = '" + ParCompany_Id + "' OR L32.ParCompany_Id IS NULL) " +
                        " AND(L3V.ParCompany_Id = '" + ParCompany_Id + "' OR L3V.ParCompany_Id IS NULL) " +
                        ParLevel1_IdFilho +


                        //queryResult + 


                        " GROUP BY " +
           "   L321.ParLevel1_Id " +
           " , L2.Id " +
           " , L3.Id " +
           " , L3.Name " +
           " , L3G.Id " +
           " , L3G.Name " +
           " , L3IT.Id " +
           " , L3IT.Name " +
           " , L3V.ParLevel3BoolFalse_Id " +
           " , L3BF.Name " +
           " , L3V.ParLevel3BoolTrue_Id " +
           " , L3BT.Name " +
           " , L3V.IntervalMin " +
           " , L3V.IntervalMax " +
           " , MU.Name " +
           " , L32.Weight " +
           "\n  , L3V.ParCompany_Id " +
           " , L32.ParCompany_Id ";



            }

            sql += sqlFilho;

            sql += "  ORDER BY 5 ASC, 4 ASC, 2 ASC, 15  DESC , 16  DESC  ";

            var parLevel3List = db.Query<ParLevel3>(sql);

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
                //var syncServices = new SgqSystem.Services.SyncServices();

                //Instanciamos variavel de data
                string dataInicio = null;
                string dataFim = null;

                //Pega a data pela regra da frequencia
                SyncServices.getFrequencyDate(ParLevel2.ParFrequency_Id, DateCollect, ref dataInicio, ref dataFim);

                //SqlConnection db = new SqlConnection(conexao);



                string sql = "SELECT " +
                             "R3.ParLevel3_Id AS Id " +
                             "FROM CollectionLevel2 C2 (nolock)  " +
                             "INNER JOIN ParLevel1 L1  (nolock) " +
                             "ON C2.ParLevel1_Id = L1.Id AND L1.IsPartialSave = 1 " +
                             "INNER JOIN Result_Level3 R3  (nolock) " +
                             "ON R3.CollectionLevel2_Id = C2.Id " +
                             "WHERE C2.UnitId = '" + ParCompany_Id + "' AND L1.Id='" + ParLevel1.Id + "' AND C2.ParLevel2_Id='" + ParLevel2.Id + "' AND C2.CollectionDate BETWEEN '" + dataInicio + " 00:00:00' AND '" + dataFim + " 23:59:59' ";

                //string sql = "SELECT R3.ParLevel3_Id AS Id FROM RESULT_LEVEL3 R3 " +
                //              "INNER JOIN CollectionLevel2 C2 " +
                //              "ON C2.Id = R3.CollectionLevel2_Id " +
                //              "INNER JOIN( " +
                //              "SELECT L3.Id as ParLevel3_Id, L2.Id as ParLevel2_Id, L321.ParLevel1_Id as ParLevel1_Id " +
                //              "FROM ParLevel3 AS L3 INNER JOIN " +
                //              "ParLevel3Value AS L3V ON L3V.ParLevel3_Id = L3.Id INNER JOIN " +
                //              "ParLevel3InputType AS L3IT ON L3IT.Id = L3V.ParLevel3InputType_Id LEFT OUTER JOIN " +
                //              "ParLevel3BoolFalse AS L3BF ON L3BF.Id = L3V.ParLevel3BoolFalse_Id LEFT OUTER JOIN " +
                //              "ParLevel3BoolTrue AS L3BT ON L3BT.Id = L3V.ParLevel3BoolTrue_Id LEFT OUTER JOIN " +
                //              "ParMeasurementUnit AS MU ON MU.Id = L3V.ParMeasurementUnit_Id LEFT OUTER JOIN " +
                //              "ParLevel3Level2 AS L32 ON L32.ParLevel3_Id = L3.Id LEFT OUTER JOIN " +
                //              "ParLevel3Group AS L3G ON L3G.Id = L32.ParLevel3Group_Id INNER JOIN " +
                //              "ParLevel2 AS L2 ON L2.Id = L32.ParLevel2_Id INNER JOIN " +
                //              "ParLevel3Level2Level1 AS L321 ON L321.ParLevel3Level2_Id = L32.Id " +
                //              "WHERE(L3.IsActive = 1) AND(L32.IsActive = 1) AND(L2.Id = '" + ParLevel2.Id + "') AND(L32.ParCompany_Id = '" + ParCompany_Id + "' OR " +
                //              "                  L32.ParCompany_Id IS NULL) AND L321.ParLevel1_Id = '" + ParLevel1.Id + "' " +
                //              "GROUP BY L321.ParLevel1_Id, L2.Id, L3.Id, L3.Name, L3G.Id, L3G.Name, L3IT.Id, L3IT.Name, L3V.ParLevel3BoolFalse_Id, L3BF.Name, L3V.ParLevel3BoolTrue_Id, L3BT.Name, L3V.IntervalMin, L3V.IntervalMax, MU.Name, L32.Weight, " +
                //              "                   L32.ParCompany_Id " +
                //              ") TAREFAS " +
                //              "ON TAREFAS.ParLevel3_Id = R3.ParLevel3_Id AND TAREFAS.ParLevel2_Id = C2.ParLevel2_Id AND TAREFAS.ParLevel1_Id = C2.ParLevel1_Id " +
                //              "AND C2.UnitId = '" + ParCompany_Id + "' " +
                //              "AND C2.CollectionDate BETWEEN '" + dataInicio + " 00:00:00' AND '" + dataFim + " 23:59:59' ";


                var parLevel3List = db.Query<ParLevel3>(sql);

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
        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

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
        //public int Sequential { get; set; }
        //public int Side { get; set; }

        private SqlConnection db { get; set; }
        public Level2Result() { }
        public Level2Result(SqlConnection _db)
        {
            db = _db;
        }

        public IEnumerable<Level2Result> getList(int ParLevel1_Id, int ParCompany_Id, string dataInicio, string dataFim)
        {


            //SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT ParLevel1_Id, ParLevel2_Id, UnitId AS Unit_Id, Shift, Period, CollectionDate, MAX(EvaluationNumber) AS EvaluateLast, MAX(Sample) AS SampleLast, MAX(ConsolidationLevel2_Id) AS ConsolidationLevel2_Id " +
                         "FROM(SELECT CL2.ParLevel1_Id, CL2.ParLevel2_Id, CL2.UnitId, Shift, Period, CONVERT(date, CollectionDate) AS CollectionDate, EvaluationNumber, MAX(Sample) AS Sample, MAX(ConsolidationLevel2_Id) AS ConsolidationLevel2_Id " +
                         "FROM CollectionLevel2 CL2 (nolock)  " +
                         "INNER JOIN ConsolidationLevel2 CDL2 (nolock)  ON CL2.ConsolidationLevel2_Id = CDL2.ID " +
                         "INNER JOIN ConsolidationLevel1 CDL1 (nolock)  ON CDL2.ConsolidationLevel1_Id = CDL1.Id " +
                         "WHERE(CDL1.ParLevel1_Id = '" + ParLevel1_Id + "' AND CDL1.UnitId = '" + ParCompany_Id + "' AND CDL1.ConsolidationDate BETWEEN '" + dataInicio + " 00:00:00' AND '" + dataFim + " 23:59:59') " +
                         "GROUP BY CL2.ParLevel1_Id, CL2.ParLevel2_Id, CL2.UnitId, Shift, Period, CONVERT(date, CollectionDate), EvaluationNumber, ConsolidationLevel2_Id) AS ultimas_amostras " +
                         "GROUP BY ParLevel1_Id, ParLevel2_Id, UnitId, Shift, Period, CollectionDate, ConsolidationLevel2_Id ";


            var Level2ResultList = db.Query<Level2Result>(sql);

            return Level2ResultList;

        }
        public int getMaxSampe(int ConsolidationLevel2_Id, int EvaluationNumber)
        {
            try
            {

                //SqlConnection db = new SqlConnection(conexao);

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

            //SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT CL2.Id, CL2.ParLevel1_Id, CL2.ParLevel2_Id, CL2.UnitId, CL2.Shift, CL2.Period, CL2.EvaluationNumber, CL2.Sample, CL2.ConsolidationLevel2_Id, CL2.[Key] " +
                         "FROM CollectionLevel2 CL2  (nolock) " +
                         "WHERE CL2.ParLevel1_Id = '" + ParLevel1_Id + "' AND CL2.UnitId = '" + ParCompany_Id + "' AND CL2.CollectionDate BETWEEN '" + dataInicio + " 00:00:00' AND '" + dataFim + " 23:59:59' ";
            var Level2ResultList = db.Query<Level2Result>(sql);
            return Level2ResultList;
        }

    }


    public partial class ParLevel1ConsolidationXParFrequency
    {
        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

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
                //SqlConnection db = new SqlConnection(conexao);

                DateTime data_ini = new DateTime(data.Year, data.Month, 1);
                DateTime data_fim = new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month));


                string sql = "SELECT CDL1.Id, CDL1.ParLevel1_Id, PL1.ParFrequency_Id, PL1.IsPartialSave FROM ConsolidationLevel1 CDL1 (nolock)  " +
                             "INNER JOIN ParLevel1 PL1 (nolock)  ON CDL1.ParLevel1_Id = PL1.Id WHERE CDL1.UnitId = '" + ParCompany_Id + "'" +
                             " AND CDL1.Consolidationdate BETWEEN '" + data_ini.ToString("yyyyMMdd") + " 00:00' and '" + data_fim.ToString("yyyyMMdd") + " 23:59'" +
                             " AND PL1.IsActive = 1" +
                             " GROUP BY CDL1.Id, CDL1.ParLevel1_Id, PL1.ParFrequency_Id,  PL1.IsPartialSave";

                var consolidation = db.Query<ParLevel1ConsolidationXParFrequency>(sql);

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
        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

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

            //SqlConnection db = new SqlConnection(conexao);
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

            var consolidation = db.Query<ConsolidationResultL1L2>(sql).FirstOrDefault();

            return consolidation;
        }
    }

    public partial class ParLevelHeader
    {
        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public int ParHeaderField_Id { get; set; }
        public string ParHeaderField_Name { get; set; }
        public string ParHeaderField_Description { get; set; }
        public int ParFieldType_Id { get; set; }
        public int IsRequired { get; set; }
        public bool LinkNumberEvaluetion { get; set; }
        public bool duplicate { get; set; }

        private SqlConnection db { get; set; }
        public ParLevelHeader() { }
        public ParLevelHeader(SqlConnection _db)
        {
            db = _db;
        }

        public IEnumerable<ParLevelHeader> getHeaderByLevel1(int ParLevel1_Id)
        {
            //SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT PH.Id AS ParHeaderField_Id, PH.Name AS ParHeaderField_Name, PH.Description AS ParHeaderField_Description, PT.Id AS ParFieldType_Id, PH.LinkNumberEvaluetion AS LinkNumberEvaluetion, PH.IsRequired AS IsRequired, PH.duplicate FROM ParLevel1XHeaderField PL (nolock)   " +
                         "LEFT JOIN ParHeaderField PH (nolock)  ON PH.Id = PL.ParHeaderField_Id                                                                                                                                    " +
                         "LEFT JOIN ParLevelDefiniton PD (nolock)  ON PH.ParLevelDefinition_Id = PD.Id                                                                                                                             " +
                         "LEFT JOIN ParFieldType PT (nolock)  ON PH.ParFieldType_Id = PT.Id                                                                                                                                        " +
                         "WHERE                                                                                                                                                                                          " +
                         "PD.Id = 1 AND                                                                                                                                                                                  " +
                         "PL.ParLevel1_Id = " + ParLevel1_Id + " AND                                                                                                                                                     " +
                         "PL.IsActive = 1 AND PH.IsActive = 1 AND PD.IsActive = 1                                                                                                                                        " +
                         "GROUP BY PH.Id, PH.Name, PT.Id, PH.IsRequired, PH.Description, PH.LinkNumberEvaluetion, ph.duplicate                                                                                                                             ";



            var parLevel3List = db.Query<ParLevelHeader>(sql);

            return parLevel3List;
        }

        public IEnumerable<ParLevelHeader> getHeaderByLevel1Level2(int ParLevel1_Id, int ParLevel2_Id)
        {
            //SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT PH.Id AS ParHeaderField_Id, PH.Name AS ParHeaderField_Name, PH.Description AS ParHeaderField_Description, PT.Id AS ParFieldType_Id, PH.LinkNumberEvaluetion AS LinkNumberEvaluetion, PH.IsRequired AS IsRequired, PH.duplicate FROM ParLevel1XHeaderField PL  (nolock)  " +
                         "LEFT JOIN ParHeaderField PH  (nolock) ON PH.Id = PL.ParHeaderField_Id                                                                                                                                    " +
                         "LEFT JOIN ParLevelDefiniton PD  (nolock) ON PH.ParLevelDefinition_Id = PD.Id                                                                                                                             " +
                         "LEFT JOIN ParFieldType PT  (nolock) ON PH.ParFieldType_Id = PT.Id                                                                                                                                        " +
                         "WHERE                                                                                                                                                                                          " +
                         "PD.Id = 2 AND                                                                                                                                                                                  " +
                         "PL.ParLevel1_Id = " + ParLevel1_Id + " AND                                                                                                                                                     " +
                         "PL.IsActive = 1 AND PH.IsActive = 1 AND PD.IsActive = 1                                                                                                                                        " +
                         "GROUP BY PH.Id, PH.Name, PT.Id, PH.Description, PH.IsRequired, PH.LinkNumberEvaluetion, PH.duplicate;                                                                                                                             ";

            var parLevel3List = db.Query<ParLevelHeader>(sql);

            return parLevel3List;
        }

        public bool isHeaderLeve2Exception(int ParLevel1_Id, int ParLevel2_Id, int HeaderField_Id)
        {
            //SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT * FROM ParLevel2XHeaderField PHF (nolock)                                                    \n" +
                         "\n INNER JOIN ParHeaderField HF (nolock)  " +
                         "\n ON HF.Id = PHF.ParHeaderField_Id AND HF.IsActive = 1 " +
                         "\n WHERE PHF.ParLevel1_Id = " + ParLevel1_Id + "                                              \n" +
                         "\n AND PHF.ParLevel2_Id = " + ParLevel2_Id + "                                               \n" +
                         "\n AND PHF.ParHeaderField_Id = " + HeaderField_Id + "                                         \n" +
                         "\n AND PHF.IsActive = 1;                                                                    \n";

            var parLevel3List = db.Query<ParLevelHeader>(sql);

            if (parLevel3List.Count() > 0)
                return true;
            else
                return false;
        }
    }
    public partial class ParFieldType
    {
        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

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
            //SqlConnection db = new SqlConnection(conexao);


            string sql = "SELECT Id, Name, PunishmentValue, IsDefaultOption FROM ParMultipleValues (nolock)        " +
                         "WHERE ParHeaderField_Id = '" + ParHeaderField_Id + "' and IsActive = 1;        ";

            var multipleValues = db.Query<ParFieldType>(sql);

            return multipleValues;
        }

        public IEnumerable<ParFieldType> getIntegrationValues(int ParHeaderField_Id, string integracao, int ParCompany_Id)
        {
            string conexaoBR = System.Configuration.ConfigurationManager.ConnectionStrings["SGQ_GlobalADO"].ConnectionString;
            db = new SqlConnection(conexaoBR);

            var sql = "SELECT null Id, null as Name, 0 as PunishmentValue, 0 as IsDefaultOption";

            var valores = integracao.Split('|');

            if (valores[0] == "Equipamento" || valores[0] == "Câmara" || valores[0] == "Ponto de Coleta" || valores[0] == "Detector de Metais")
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
                //sql = "\n SELECT Id as Id, Name as Name, 0 as PunishmentValue, 0 as IsDefaultOption  " +
                //      "\n FROM UserSgq With (nolock) ";

                sql = @"SELECT 
                        TT.cNrClassificacao as Id,
                        TT.cNmClassificacao as Name,
                        0 as  PunishmentValue,
                        0 as IsDefaultOption
                        FROM
                        (
                        SELECT CL.*, P.* FROM ClassificacaoProduto CP
                        INNER JOIN (
                        SELECT CC.cNmClassificacao as Grupo, C.*, C.nCdClassificacao as cod FROM Classificacao C
                        INNER JOIN (
	                        SELECT * FROM Classificacao 
	                        WHERE LEN(cNrClassificacao) = 5
                        ) CC
                        ON left(C.cNrClassificacao,5) = CC.cNrClassificacao
                        ) CL
                        ON CL.cod = CP.nCdClassificacao
                        INNER JOIN PRODUTO P
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

            var multipleValues = db.Query<ParFieldType>(sql);

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
            string conexaoBR = System.Configuration.ConfigurationManager.ConnectionStrings["SGQ_GlobalADO"].ConnectionString;
            db = new SqlConnection(conexaoBR);

            var sql = "SELECT nCdProduto as id, cNmProduto as nome FROM Produto";

            var lista = db.Query<Generico>(sql).ToList();

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

            var lista = db.Query<ParLevel3Vinculado>(sql).ToList();

            return lista;
        }
    }

    public partial class ParRelapse
    {
        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

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
            //SqlConnection db = new SqlConnection(conexao);


            string sql = "SELECT Id, ParFrequency_Id, NcNumber, EffectiveLength FROM ParRelapse  (nolock)                 " +
                         "WHERE ParLevel1_Id = '" + ParLevel1_Id + "' and IsActive = 1;                         ";

            var parRelapses = db.Query<ParRelapse>(sql);

            return parRelapses;
        }
    }
    public partial class Result_Level3
    {
        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public int Id { get; set; }
        private SqlConnection db { get; set; }
        public Result_Level3() { }
        public Result_Level3(SqlConnection _db)
        {
            db = _db;
        }

        public Result_Level3 get(int CollectionLevel2_Id, int ParLevel3_Id)
        {
            //SqlConnection db = new SqlConnection(conexao);


            string sql = "SELECT Id FROM Result_Level3  (nolock)          " +
                         "WHERE ParLevel3_Id = '" + ParLevel3_Id + "' and " +
                         "CollectionLevel2_Id = " + CollectionLevel2_Id + ";";

            var parResultLevel3 = db.Query<Result_Level3>(sql).FirstOrDefault();

            return parResultLevel3;
        }
    }

    public partial class ParLevel1VariableProduction
    {
        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

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
            //SqlConnection db = new SqlConnection(conexao);

            string sql = "select P.Id, P.Name from ParLevel1VariableProductionXLevel1 PL  (nolock) left join " +
                         "ParLevel1VariableProduction P (nolock)  on P.Id = PL.ParLevel1VariableProduction_Id " +
                         " where PL.ParLevel1_Id = " + ParLevel1_Id + "; ";

            var list = db.Query<ParLevel1VariableProduction>(sql);

            return list;
        }
    }

    public partial class ParConfSGQContext
    {
        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

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
            //SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT Id, HaveUnitLogin, HaveShitLogin FROM ParConfSGQ (nolock) ";

            var conf = db.Query<ParConfSGQContext>(sql).FirstOrDefault();

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
            //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

            //SqlConnection db = new SqlConnection(conexao);

            //string sql = "SELECT U.Id, U.Name AS Login, U.Password, U.FullName AS Name, U.ParCompany_Id , C.Name AS ParCompany_Name, PxU.Role " +
            //             "FROM                                                                                                                " +
            //             "UserSgq U                                                                                                           " +
            //             "INNER JOIN ParCompany C ON U.ParCompany_Id = C.Id                                                                   " +
            //             "INNER JOIN ParCompanyXUserSgq PxU ON U.Id = PxU.UserSgq_Id                                                          " +
            //             "WHERE U.Name = '" + userLogin + "' AND PxU.ParCompany_Id = C.Id                                                     ";

            string where = "WHERE U.name = '" + userLogin + "'";
            if (id > 0)
            {
                where = "WHERE U.Id = '" + id + "'";
            }

            string sql = "SELECT U.Id, U.Name AS Login, U.Password, U.FullName AS Name, U.ParCompany_Id , PC.Name AS ParCompany_Name, PxU.Role FROM UserSgq U  (nolock) " +
                         "LEFT JOIN ParCompany PC  (nolock) ON U.ParCompany_Id = PC.Id   " +
                         "LEFT JOIN ParCompanyXUserSgq PxU  (nolock) ON U.ParCompany_Id = PxU.ParCompany_Id AND PxU.UserSgq_Id = U.Id " +
                        where;

            var user = db.Query<UserSGQ>(sql).FirstOrDefault();

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
            //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

            //SqlConnection db = new SqlConnection(conexao);

            string sql = "select U.Id AS UserSGQ_Id, U.Name AS UserSGQ_Login, U.FullName AS UserSGQ_Name, U.Password AS UserSGQ_Pass, U.Role, PxC.Role AS Role, C.Id ParCompany_Id, C.Name ParCompany_Name from ParCompanyXUserSgq PxC (nolock)  " +
                         "INNER JOIN ParCompany C  (nolock) ON PxC.ParCompany_Id = c.Id                                                                                                                                                          " +
                         "INNER JOIN UserSgq U  (nolock) ON PxC.UserSgq_Id = u.Id                                                                                                                                                                " +
                         "WHERE PxC.ParCompany_Id='" + ParCompany_Id + "'                                                                                                                                                              " +
                         "ORDER BY C.Name ASC                                                                                                                                                                                          ";

            var users = db.Query<ParCompanyXUserSgq>(sql);

            return users;
        }
        /// <summary>
        /// Retorna todas as unidades do usuário
        /// </summary>
        /// <param name="UserSgq_Id"></param>
        /// <returns></returns>
        public IEnumerable<ParCompanyXUserSgq> getUserCompany(int UserSgq_Id)
        {
            //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

            //SqlConnection db = new SqlConnection(conexao);

            string sql = "select U.Id AS UserSGQ_Id, U.Name AS UserSGQ_Login, U.FullName AS UserSGQ_Name, U.Password AS UserSGQ_Pass, U.Role, PxC.Role AS Role, C.Id ParCompany_Id, C.Name ParCompany_Name from ParCompanyXUserSgq PxC  (nolock) " +
                         "INNER JOIN ParCompany C  (nolock) ON PxC.ParCompany_Id = c.Id                                                                                                                                                          " +
                         "INNER JOIN UserSgq U  (nolock) ON PxC.UserSgq_Id = u.Id                                                                                                                                                                " +
                         "WHERE PxC.UserSgq_Id='" + UserSgq_Id + "'                                                                                                                                                              " +
                         "ORDER BY C.Name ASC                                                                                                                                                                                          ";


            var companys = db.Query<ParCompanyXUserSgq>(sql);

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
            //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

            //SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT DISTINCT TC.HashKey as HashKey, RT.Id as Type, TJbs.Role as RoleJBS, Tsgq.Role as RoleSGQ FROM ScreenComponent TC  (nolock) " +
                         "LEFT JOIN RoleType RT (nolock)  on RT.Id = TC.Type                                                                          " +
                         "LEFT JOIN RoleSGQ TSgq (nolock)  ON Tsgq.ScreenComponent_Id = TC.Id                                                         " +
                         "LEFT JOIN RoleJBS TJbs (nolock)  ON TJbs.ScreenComponent_Id = TC.Id                                                         " +
                         "LEFT JOIN ParCompanyXUserSgq CU (nolock)  ON (CU.Role = TJbs.Role OR TJbs.Role IS NULL)                                       " +
                         "LEFT JOIN UserSgq U  (nolock) ON (U.Role = Tsgq.Role OR Tsgq.Role IS NULL)                                                    " +
                         "WHERE U.Id = CU.UserSgq_Id AND                                                                                          " +
                         "CU.ParCompany_Id = " + ParCompany_id + " AND                                                                       " +
                         "U.id = " + UserSGQ_Id + ";                                                                                         ";

            var users = db.Query<RoleXUserSgq>(sql);

            return users;
        }
    }
    public partial class VolumePcc1b
    {
        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

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
            //SqlConnection db = new SqlConnection(conexao);

            string sql = "select VP.Id VP.VolumeAnimais, VP.Quartos, VP.Avaliacoes, VP.Amostras from VolumePcc1b (nolock)  VP where VP.Indicador = " + Indicador + " and VP.Unidade = " + Unidade + "; ";

            var list = db.Query<VolumePcc1b>(sql);

            return list;
        }
    }
    public partial class CaracteristicaTipificacao
    {
        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["SGQ_GlobalADO"].ConnectionString;

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
            //SqlConnection db = new SqlConnection(conexao);

            string sql = "select null nCdCaracteristica, null cNmCaracteristica, null cNrCaracteristica, null cSgCaracteristica, null cIdentificador ";
            if (GlobalConfig.Brasil)
            {
                sql = "select CP.nCdCaracteristica, CP.cNmCaracteristica, CP.cNrCaracteristica, CP.cSgCaracteristica, CP.cIdentificador" +
                      " from CaracteristicaTipificacao CP (nolock)  where LEN(CP.cNrCaracteristica) >= 5 and SUBSTRING(CP.cNrCaracteristica, 1, 3) = '" + id + "';";
            }

            var list = db.Query<CaracteristicaTipificacao>(sql);

            return list;
        }

        public IEnumerable<CaracteristicaTipificacao> getAreasParticipantes()
        {
            //SqlConnection db = new SqlConnection(conexao);

            string sql = "select CP.nCdCaracteristica, CP.cNmCaracteristica, CP.cNrCaracteristica, CP.cSgCaracteristica, CP.cIdentificador" +
                         " from AreasParticipantes CP  (nolock) where LEN(cNrCaracteristica) >= 5;";

            var list = db.Query<CaracteristicaTipificacao>(sql);

            return list;
        }

        public IEnumerable<CaracteristicaTipificacao> getCaracteristicasTipificacaoUnico(int ncdcaracteristica)
        {
            //SqlConnection db = new SqlConnection(conexao);

            string sql = "select CP.nCdCaracteristica, CP.cNmCaracteristica, CP.cNrCaracteristica, CP.cSgCaracteristica, CP.cIdentificador" +
                         " from CaracteristicaTipificacao CP (nolock)  where cNrCaracteristica = " + ncdcaracteristica;

            var list = db.Query<CaracteristicaTipificacao>(sql);

            return list;
        }

        public IEnumerable<CaracteristicaTipificacao> getAreasParticipantesUnico()
        {
            //SqlConnection db = new SqlConnection(conexao);

            string sql = "select CP.nCdCaracteristica, CP.cNmCaracteristica, CP.cNrCaracteristica, CP.cSgCaracteristica, CP.cIdentificador" +
                         " from AreasParticipantes CP  (nolock) where cNrCaracteristica = 0209;";

            var list = db.Query<CaracteristicaTipificacao>(sql);

            return list;
        }


    }
    public partial class VerificacaoTipificacaoTarefaIntegracao
    {
        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["SGQ_GlobalADO"].ConnectionString;

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
            //SqlConnection db = new SqlConnection(conexao);

            string sql = "select Id, TarefaId, CaracteristicaTipificacaoId from VerificacaoTipificacaoTarefaIntegracao (nolock)  where CaracteristicaTipificacaoId = " + caracteristicatipificacaoid;

            var list = db.Query<VerificacaoTipificacaoTarefaIntegracao>(sql);

            return list;
        }
    }

    public partial class CollectionLevel2Consolidation
    {
        public int ConsolidationLevel2_Id { get; set; }

        public int ParLevel2_Id { get; set; }

        public decimal WeiEvaluationTotal { get; set; }
        // public decimal EvaluateTotal { get; set; }
        public decimal DefectsTotal { get; set; }
        public decimal WeiDefectsTotal { get; set; }
        public int TotalLevel3Evaluation { get; set; }
        public int TotalLevel3WithDefects { get; set; }
        public int LastEvaluationAlert { get; set; }
        public int LastLevel2Alert { get; set; }
        public int EvaluatedResult { get; set; }
        public int DefectsResult { get; set; }

        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

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
                //SqlConnection db = new SqlConnection(conexao);

                string sql = "SELECT ConsolidationLevel2_Id, ParLevel2_Id, SUM(WeiEvaluation) AS [WeiEvaluationTotal], SUM(Defects) AS [DefectsTotal], SUM(WeiDefects) AS[WeiDefectsTotal], SUM(TotalLevel3WithDefects) AS [TotalLevel3WithDefects], SUM(TotalLevel3Evaluation) AS [TotalLevel3Evaluation], MAX(LastEvaluationAlert) AS LastEvaluationAlert, (SELECT top 1 LastLevel2Alert FROM CollectionLevel2 WHERE Id = max(c2.id)) AS LastLevel2Alert, SUM(EvaluatedResult) AS EvaluatedResult, SUM(DefectsResult) AS DefectsResult " +
                             "FROM CollectionLevel2 C2  (nolock) WHERE ConsolidationLevel2_Id = " + ConsolidationLevel2_Id + " AND ParLevel2_Id = " + ParLevel2_Id + " AND NotEvaluatedIs=0" +
                             "group by ConsolidationLevel2_Id, ParLevel2_Id";

                var consolidationLevel2 = db.Query<CollectionLevel2Consolidation>(sql).FirstOrDefault();

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
        //public int ParLevel1_Id { get; set; }


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

        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public ConsolidationLevel1XConsolidationLevel2 getConsolidation(int ConsolidationLevel1_Id)
        {
            try
            {
                //SqlConnection db = new SqlConnection(conexao);

                string sql = "select  SUM(WeiEvaluation) AS WeiEvaluation, SUM(EvaluateTotal) AS EvaluateTotal, SUM(DefectsTotal) AS DefectsTotal, SUM(WeiDefects) AS WeiDefects,  SUM(TotalLevel3Evaluation) AS TotalLevel3Evaluation, SUM(TotalLevel3WithDefects) AS TotalLevel3WithDefects, MAX(LastEvaluationAlert) AS LastEvaluationAlert, (SELECT top 1 LastLevel2Alert FROM CollectionLevel2 (nolock)  WHERE Id = max(c2.id)) AS LastLevel2Alert, SUM(EvaluatedResult) AS EvaluatedResult, SUM(DefectsResult) AS DefectsResult FROM ConsolidationLevel2 C2 (nolock)  where ConsolidationLevel1_Id=" + ConsolidationLevel1_Id + "";

                var consolidationLevel1 = db.Query<ConsolidationLevel1XConsolidationLevel2>(sql).FirstOrDefault();

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

        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
        private SqlConnection db { get; set; }
        public ConsolidationLevel1() { }
        public ConsolidationLevel1(SqlConnection _db)
        {
            db = _db;
        }
        public ConsolidationLevel1 getConsolidation(int ParCompany_Id, int ParLevel1_Id, DateTime collectionDate, int Shift, int Period)
        {
            try
            {
                string sql = "SELECT * FROM ConsolidationLevel1 (nolock) WHERE UnitId = '" + ParCompany_Id + "' AND ParLevel1_Id= '" + ParLevel1_Id + "' AND SHIFT = " + Shift + " and period = "
                + Period + " AND CONVERT(date, ConsolidationDate) = '" + collectionDate.ToString("yyyy-MM-dd") + "'";

                //SqlConnection db = new SqlConnection(conexao);
                var obj = db.Query<ConsolidationLevel1>(sql).FirstOrDefault();
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

        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

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
                string sql = "SELECT * FROM ConsolidationLevel2 (nolock)  WHERE UnitId = " + ParCompany_Id + " AND ParLevel1_Id= " + ParLevel1_Id + " AND ConsolidationDate BETWEEN '" + collectionDate.ToString("yyyy-MM-dd") + " 00:00' AND '" + collectionDate.ToString("yyyy-MM-dd") + " 23:59:90.9999'";

                //SqlConnection db = new SqlConnection(conexao);

                /**
                 * ADD PARAMETER FORLINI
                 * DECLARA TODAS AS COLUNAS!! AO INVES DO *
                 * INSERIR ÍNDICE NO CONSOLIDATIONLEVEL1 DENTRO DO CONOLIDATIONLEVEL2
                 */

                var obj = db.Query<ConsolidationLevel2>(sql).FirstOrDefault();
                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ConsolidationLevel2 getByConsolidationLevel1(int ParCompany_Id, int ConsolidationLevel1_Id, int ParLevel2_Id)
        {
            try
            {
                string sql = "SELECT Id, ConsolidationLevel1_Id, UnitId, ParLevel2_Id, ConsolidationDate, WeiEvaluation, EvaluateTotal, DefectsTotal, WeiDefects, TotalLevel3Evaluation, TotalLevel3WithDefects, EvaluatedResult FROM ConsolidationLevel2 (nolock)  WHERE ConsolidationLevel1_Id = '" + ConsolidationLevel1_Id + "' AND ParLevel2_Id= '" + ParLevel2_Id + "' AND UnitId='" + ParCompany_Id + "'";
                //SqlConnection db = new SqlConnection(conexao);
                var obj = db.Query<ConsolidationLevel2>(sql).FirstOrDefault();
                return obj;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public ConsolidationLevel2 getByConsolidationLevel1(int ParCompany_Id, int ConsolidationLevel1_Id, int ParLevel2_Id, int reaudit, string reauditNumber)
        {
            try
            {
                string sql = "SELECT Id, ConsolidationLevel1_Id, UnitId, ParLevel2_Id, ConsolidationDate, WeiEvaluation, EvaluateTotal, DefectsTotal, WeiDefects, TotalLevel3Evaluation, TotalLevel3WithDefects, EvaluatedResult, ReauditIs, ReauditNumber FROM ConsolidationLevel2 (nolock)  WHERE ConsolidationLevel1_Id = '" +
                    ConsolidationLevel1_Id + "' AND ParLevel2_Id= '" + ParLevel2_Id + "' AND UnitId='" + ParCompany_Id + "' AND ReauditIs=" + reaudit + " and reauditnumber=" + reauditNumber + ";";
                //SqlConnection db = new SqlConnection(conexao);
                var obj = db.Query<ConsolidationLevel2>(sql).FirstOrDefault();
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
        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

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
                //SqlConnection db = new SqlConnection(conexao);
                var list = db.Query<CollectionJson>(sql);
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

        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        private SqlConnection db { get; set; }
        public ParCounter() { }
        public ParCounter(SqlConnection _db)
        {
            db = _db;
        }

        public IEnumerable<ParCounter> GetParLevelXParCounterList(int ParLevel1_Id, int ParLevel2_Id, int Level)
        {
            try
            {
                if (ParLevel1_Id > 0 || ParLevel2_Id > 0)
                {
                    string sql = "";
                    if (ParLevel1_Id > 0)
                    {
                        /*
                        sql = "SELECT PC.Name FROM ParCounterXLocal PL                                                      " +
                                 "   LEFT JOIN ParCounter PC ON PL.ParCounter_Id = PC.Id                                    " +
                                 "   LEFT JOIN ParLocal PO ON PO.Id = PL.ParLocal_Id                                        " +
                                 "   WHERE PL.ParLevel1_Id = " + ParLevel1_Id +
                                 "   AND PL.ParLevel2_Id IS NULL                                                            " +
                                 "   AND PO.Name = '" + Local + "'                                                             " +
                                 "   AND PC.Level = " + Level + " AND PL.IsActive = 1;                                      ";
                        */

                        sql = "SELECT Distinct PO.level, PC.Name as Counter, PO.Name as Local, PL.ParLevel1_Id AS indicador FROM ParCounterXLocal PL (nolock)  " +
                              "LEFT JOIN ParCounter PC (nolock)  ON PL.ParCounter_Id = PC.Id " +
                              "LEFT JOIN ParLocal PO  (nolock) ON PO.Id = PL.ParLocal_Id " +
                              "WHERE PL.ParLevel1_Id = " + ParLevel1_Id + " " +
                              "AND PL.ParLevel2_Id IS NULL " +
                              "AND PC.Level = " + Level +
                              "AND PL.IsActive = 1";

                    }
                    else if (ParLevel2_Id > 0)
                    {
                        /*
                        sql = "SELECT PC.Name FROM ParCounterXLocal PL                                                      " +
                                 "   LEFT JOIN ParCounter PC ON PL.ParCounter_Id = PC.Id                                    " +
                                 "   LEFT JOIN ParLocal PO ON PO.Id = PL.ParLocal_Id                                        " +
                                 "   WHERE PL.ParLevel1_Id IS NULL                                                          " +
                                 "   AND PL.ParLevel2_Id = " + ParLevel2_Id +
                                 "   AND PO.Name = '" + Local + "'                                                             " +
                                 "   AND PC.Level = " + Level + " AND PL.IsActive = 1;                                      ";
                        */

                        sql = "SELECT Distinct PO.level, PC.Name as Counter, PO.Name as Local, PL.ParLevel2_Id AS indicador FROM ParCounterXLocal PL (nolock)  " +
                              "LEFT JOIN ParCounter PC (nolock)  ON PL.ParCounter_Id = PC.Id " +
                              "LEFT JOIN ParLocal PO (nolock)  ON PO.Id = PL.ParLocal_Id " +
                              "WHERE PL.ParLevel1_Id IS NULL " +
                              "AND PL.ParLevel2_Id= " + ParLevel2_Id + " " +
                              "AND PC.Level = " + Level +
                              "AND PL.IsActive = 1";


                    }

                    //SqlConnection db = new SqlConnection(conexao);
                    var list = db.Query<ParCounter>(sql);
                    return list;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

    }

    public partial class NotConformityRule
    {
        public int Id { get; set; }
        public decimal Value { get; set; }

        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
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

                //SqlConnection db = new SqlConnection(conexao);
                var obj = db.Query<NotConformityRule>(sql).FirstOrDefault();
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

        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

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

                //SqlConnection db = new SqlConnection(conexao);
                var obj = db.Query<CollectionLevel2>(sql).FirstOrDefault();
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
        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
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

                //SqlConnection db = new SqlConnection(conexao);
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

                //SqlConnection db = new SqlConnection(conexao);
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

        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
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
                            "BETWEEN '" + StartDate.ToString("yyyyMMdd") + " 00:00'  and '" + EndDate.ToString("yyyyMMdd") + " 23:59' and   " +
                            "Phase > 0  and UnitId = " + ParCompany_Id + "                                                                   " +
                            "AND CONCAT(c1.ParLevel1_id, c1.ParLevel2_Id, CAST(c1.CollectionDate AS VARCHAR(500))) IN                       " +
                            "  (SELECT CONCAT(c1b.ParLevel1_id, c1b.ParLevel2_Id, CAST(MAX(c1b.CollectionDate) AS VARCHAR(500)))            " +
                            "                                                                                                               " +
                            "      FROM CollectionLevel2 c1b  (nolock)                                                                                " +
                            "                                                                                                               " +
                            "          WHERE c1b.Phase > 0                                                                                  " +
                            "                                                                                                               " +
                            "          AND c1b.CollectionDate BETWEEN '" + StartDate.ToString("yyyyMMdd") + " 00:00' and '" + EndDate.ToString("yyyyMMdd") + " 23:59'                                     " +
                            "          AND c1b.UnitId = " + ParCompany_Id + "                                                                       " +
                            "      GROUP BY c1b.ParLevel1_id, c1b.ParLevel2_Id                                                              " +
                            "  )                                                                                                            ";


                //SqlConnection db = new SqlConnection(conexao);
                var list = db.Query<ResultPhase>(sql).ToList();
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


                //SqlConnection db = new SqlConnection(conexao);
                var obj = db.Query<ResultPhaseFrequency>(sql).FirstOrDefault();
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
                             "CollectionDate BETWEEN '" + StartDate.ToString("yyyyMMdd") + " 00:00' AND '" + EndDate.ToString("yyyyMMdd") + " 23:59'    " +
                             "GROUP BY CAST(CollectionDate as date), Period, Shift ORDER BY 1";

                var obj = db.Query<ResultLevel2Period>(sql).ToList();
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

        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
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

                //SqlConnection db = new SqlConnection(conexao);
                var list = db.Query<ResultEvaluationDefects>(sql).ToList();
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    /*
         * novo metodo getConsolidation
         * Autor: Gabriel Nunes
         * Data: 2017 04 28
         */
    public partial class ResultadoUmaColuna
    {
        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public string retorno { get; set; }

    }

}