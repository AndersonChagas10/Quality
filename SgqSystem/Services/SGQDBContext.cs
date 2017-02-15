using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System;
using System.Linq;
using Dominio;

namespace SGQDBContext
{
    public partial class ParLevel1
    {
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

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

        public ParLevel1()
        {

        }
        public ParLevel1 getById(int Id)
        {
            try
            {
                SqlConnection db = new SqlConnection(conexao);
                string sql = "SELECT * FROM ParLevel1 WHERE Id='" +  Id + "'";                                                                                
                var parLevel1List = db.Query<ParLevel1>(sql).FirstOrDefault();

                return parLevel1List;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<ParLevel1> getParLevel1ParCriticalLevelList(int ParCompany_Id)
        {
            SqlConnection db = new SqlConnection(conexao);
            string sql = " SELECT P1.Id, P1.Name, CL.Id AS ParCriticalLevel_Id, CL.Name AS ParCriticalLevel_Name, P1.HasSaveLevel2 AS HasSaveLevel2, P1.ParConsolidationType_Id AS ParConsolidationType_Id, P1.ParFrequency_Id AS ParFrequency_Id,     " +
                         " P1.HasNoApplicableLevel2 AS HasNoApplicableLevel2, P1.HasAlert, P1.IsSpecific, P1.hashKey, P1.haveRealTimeConsolidation, P1.RealTimeConsolitationUpdate, P1.IsLimitedEvaluetionNumber, P1.IsPartialSave" +
                         " ,AL.ParNotConformityRule_Id AS tipoAlerta, AL.Value AS valorAlerta, P1.HasCompleteEvaluation AS HasCompleteEvaluation                                                                                                                                     " +                               
                         " FROM ParLevel1 P1                                                                                                          " +
                         " INNER JOIN (SELECT ParLevel1_Id FROM ParLevel3Level2Level1 GROUP BY ParLevel1_Id) P321                                     " +
                         " ON P321.ParLevel1_Id = P1.Id                                                                                               " +
                         " INNER JOIN ParLevel1XCluster P1C                                                                                           " +
                         " ON P1C.ParLevel1_Id = P1.Id                                                                                                " +
                         " INNER JOIN ParCluster C                                                                                                    " +
                         " ON C.Id = P1C.ParCluster_Id                                                                                                " +
                         " INNER JOIN ParCompanyCluster CC                                                                                            " +
                         " ON CC.ParCluster_Id = P1C.ParCluster_Id                                                                                    " +
                         " INNER JOIN ParCriticalLevel CL                                                                                             " +
                         " ON CL.Id = P1C.ParCriticalLevel_Id                                                                                         " +
                         " INNER JOIN ParNotConformityRuleXLevel AL                                                                                   " +
                         " ON AL.ParLevel1_Id = P1.Id                                                                                                 " +

                         " WHERE CC.ParCompany_Id = '" + ParCompany_Id + "'                                                                           " +
                         " AND AL.IsActive = 1                                                                                                        " +
                         " AND P1.IsActive = 1                                                                                                        " +
                         " ORDER BY CL.Name                                                                                                           ";

            //var parLevel1List = (List<ParLevel1>)db.Query<ParLevel1>(sql);


            var parLevel1List = db.Query<ParLevel1>(sql);

            return parLevel1List;
        }
    }
    public partial class ParLevel1Alertas
    {
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
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
        public ParLevel1Alertas()
        {

        }
        public ParLevel1Alertas getAlertas(int ParLevel1_Id, int ParCompany_Id, DateTime DateCollect)
        {
            SqlConnection db = new SqlConnection(conexao);

            string _DataCollect = DateCollect.ToString("yyyyMMdd");

            string sql = "";

            sql =   "\n SELECT " +
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
                    "\n                  , (SELECT ParConsolidationType_Id FROM ParLevel1 WHERE Id = " + ParLevel1_Id + ") AS ParConsolidationType_Id " +
                    "\n                  , (SELECT TOP 1 PercentValue FROM ParGoal WHERE ParLevel1_Id = " + ParLevel1_Id + " AND(ParCompany_Id = " + ParCompany_Id + " OR ParCompany_Id IS NULL) ORDER BY ParCompany_id DESC) AS _META " +
                    "\n                FROM " +
                    "\n         ( " +
                    "\n             /*****PCC1b**************************************************************************************************************************************************************************/ " +
                    "\n             SELECT * FROM " +
                    "\n             (SELECT TOP 1 1 AS \"hashKey\" " +
                    "\n             , (SELECT Id FROM ParLevel1 WHERE hashKey = 1) AS ID " +
                    "\n             , 'PCC1b' AS INDICADOR, 1 AS AV, COALESCE(Amostras, 0) * 2 AS AM, 1 AS PESO FROM VolumePcc1b WHERE ParCompany_id = " + ParCompany_Id + " and CONVERT(DATE, Data) = CONVERT(DATE, '" + _DataCollect + "')) PCC " +
                    "\n               /************************************************************************************************************************************************************************************/ " +
                    "\n               UNION ALL " +
                    "\n            /*****CEP VÁCUO GRD******************************************************************************************************************************************************************/ " +
                    "\n            SELECT* FROM " +
                    "\n            (SELECT TOP 1 3 AS \"hashKey\" " +
                    "\n            , (SELECT Id FROM ParLevel1 WHERE hashKey = 3) AS ID " +
                    "\n             ,'CEP VÁCUO GRD' AS INDICADOR, Avaliacoes AS AV, Amostras *QtdadeFamiliaProduto AS AM, 1 AS PESO FROM VolumeVacuoGRD WHERE ParCompany_id = " + ParCompany_Id + " and CONVERT(DATE, Data) <= CONVERT(DATE, '" + _DataCollect + "') ORDER BY Data DESC) GRD " +
                    "\n            /************************************************************************************************************************************************************************************/ " +
                    "\n            UNION ALL " +
                    "\n            /*****CEP DESOSSA********************************************************************************************************************************************************************/ " +
                    "\n            SELECT* FROM " +
                    "\n            (SELECT TOP 1 2 AS \"hashKey\" " +
                    "\n            , (SELECT Id FROM ParLevel1 WHERE hashKey = 2) AS ID " +
                    "\n             ,'CEP DESOSSA' AS INDICADOR, Avaliacoes AS AV, Amostras *QtdadeFamiliaProduto AS AM, 1 AS PESO FROM VolumeCepDesossa WHERE ParCompany_id = " + ParCompany_Id + " and CONVERT(DATE, Data) <= CONVERT(DATE, '" + _DataCollect + "') ORDER BY Data DESC) DESOSSA " +
                    "\n            /************************************************************************************************************************************************************************************/ " +
                    "\n            UNION ALL " +
                    "\n            /*****CEP RECORTES*******************************************************************************************************************************************************************/ " +
                    "\n            SELECT* FROM " +
                    "\n            (SELECT TOP 1 4 AS \"hashKey\" " +
                    "\n            , (SELECT Id FROM ParLevel1 WHERE hashKey = 4) AS ID " +
                    "\n             ,'CEP RECORTES' AS INDICADOR, Avaliacoes AS AV, Amostras AS AM, 1 AS PESO FROM VolumeCepRecortes WHERE ParCompany_id = " + ParCompany_Id + " and CONVERT(DATE, Data) <= CONVERT(DATE, '" + _DataCollect + "') ORDER BY Data DESC) RECORTES " +
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
                    "\n                , (SELECT TOP 1 Number FROM ParEvaluation WHERE ParLevel2_Id = MON.Id AND(ParCompany_Id = " + ParCompany_Id + " OR ParCompany_Id IS NULL) ORDER BY ParCompany_Id DESC) AS AVALIACOES " +
                    "\n                ,(SELECT TOP 1 Number FROM ParSample     WHERE ParLevel2_Id = MON.Id AND(ParCompany_Id = " + ParCompany_Id + " OR ParCompany_Id IS NULL) ORDER BY ParCompany_Id DESC) AS AMOSTRAS " +
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
                    "\n                    FROM ParLevel1 IND " +
                    "\n                    LEFT JOIN ParConsolidationType Cons " +
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
                    "\n                        FROM ParLevel3Level2Level1 I_M_T " +
                    "\n                        WHERE I_M_T.ParLevel1_Id = " + ParLevel1_Id + " " +
                    "\n                        AND(I_M_T.ParCompany_Id = " + ParCompany_Id + " " +
                    "\n                         OR I_M_T.ParCompany_Id IS NULL) " +
                    "\n                        GROUP BY I_M_T.ParLevel3Level2_Id " +
                    "\n                    ) M1 " +
                    "\n                    INNER JOIN " +
                    "\n                    ( " +
                    "\n                        SELECT " +
                    "\n                         (SELECT TOP 1 Id     FROM ParLevel3Level2 WHERE ParLevel2_Id = M_T.ParLevel2_Id AND ParLevel3_Id = M_T.ParLevel3_Id AND COALESCE(ParCompany_Id, 0) = MAX(COALESCE(M_T.ParCompany_Id, 0))) AS Id " +
                    "\n                        , (SELECT TOP 1 Weight FROM ParLevel3Level2 WHERE ParLevel2_Id = M_T.ParLevel2_Id AND ParLevel3_Id = M_T.ParLevel3_Id AND COALESCE(ParCompany_Id, 0) = MAX(COALESCE(M_T.ParCompany_Id, 0))) AS Weight " +
                    "\n                        , M_T.ParLevel2_Id " +
                    "\n                        , M_T.ParLevel3_Id " +
                    "\n                        , MAX(M_T.ParCompany_Id) AS _ParCompany_Id " +
                    "\n                        FROM ParLevel3Level2 M_T " +
                    "\n                        INNER JOIN ParLevel3 TAR " +
                    "\n                        ON TAR.Id = M_T.ParLevel3_Id " +
                    "\n                        AND TAR.IsActive = 1 " +
                    "\n                        INNER JOIN ParLevel2 MON " +
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
                    "\n                INNER JOIN ParLevel2 MON " +
                    "\n                ON MON.Id = MONITORAMENTOS.ParLevel2_Id " +
                    "\n                INNER JOIN ParLevel3 TAR " +
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
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
        public int Id { get; set; }
        public string Name { get; set; }
        public bool HasSampleTotal { get; set; }

        public bool IsEmptyLevel3 { get; set; }
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
                SqlConnection db = new SqlConnection(conexao);
                string sql = "SELECT * FROM ParLevel2 WHERE Id='" + Id + "'";
                var parLevel1List = db.Query<ParLevel2>(sql).FirstOrDefault();
                return parLevel1List;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<ParLevel2> getLevel2ByIdLevel1(int ParLevel1_Id, int ParCompany_Id)
        {
            SqlConnection db = new SqlConnection(conexao);

            bool parLevel1Familia = false;

            using (var dbEf = new SgqDbDevEntities()) {
          
                //var L2EQuery = from L1 in dbEf.ParLevel1
                //               where L1.Id == ParLevel1_Id
                //               select L1;

                //var result = L2EQuery.FirstOrDefault();

                //if (result != null)
                //{
                //    parLevel1Familia = result.IsFixedEvaluetionNumber;
                //}

                var result = (from L1 in dbEf.ParLevel1
                              where L1.Id == ParLevel1_Id
                              select L1).FirstOrDefault();

                //var result = L2EQuery.FirstOrDefault();

                if (result != null)
                {
                    parLevel1Familia = result.IsFixedEvaluetionNumber;
                }
            }

        /****CONTROLE DE FAMÍLIA DE PRODUTOS*****/

            if(parLevel1Familia == true)
            {
                string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name, PL2.HasSampleTotal, PL2.IsEmptyLevel3                      " +
                             "FROM ParLevel3Level2 P32                                                                          " +
                             "INNER JOIN ParLevel3Level2Level1 P321                                                             " +
                             "ON P321.ParLevel3Level2_Id = P32.Id                                                               " +
                             "INNER JOIN ParLevel2 PL2                                                                          " +
                             "ON PL2.Id = P32.ParLevel2_Id                                                                      " +
                             "INNER JOIN (SELECT * FROM ParLevel2ControlCompany PL INNER JOIN                                   " +
                             "(SELECT MAX(InitDate) Data, ParCompany_Id AS UNIDADE FROM ParLevel2ControlCompany                 " +
                             "where ParLevel1_Id = '" + ParLevel1_Id + "'                                                       " +
                             "group by ParCompany_Id) F1 on f1.data = PL.initDate and (F1.UNIDADE = PL.ParCompany_id            " +
                             "or F1.UNIDADE is null))  Familia                                                                  " +
                             "ON Familia.ParLevel2_Id = PL2.Id                                                                  " +
                             "WHERE P321.ParLevel1_Id = '" + ParLevel1_Id + "'                                                  " +
                             "AND PL2.IsActive = 1                                                                              " +
                             "AND (Familia.ParCompany_Id = '" + ParCompany_Id + "'  or Familia.ParCompany_Id IS NULL)           " +
                             "GROUP BY PL2.Id, PL2.Name, PL2.HasSampleTotal, PL2.IsEmptyLevel3                                                     ";

                var parLevel2List = db.Query<ParLevel2>(sql);

                return parLevel2List;

            } 
            else
            {

                string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name, PL2.HasSampleTotal, PL2.IsEmptyLevel3 " +
                         "FROM ParLevel3Level2 P32                                      " +
                         "INNER JOIN ParLevel3Level2Level1 P321                         " +
                         "ON P321.ParLevel3Level2_Id = P32.Id                           " +
                         "INNER JOIN ParLevel2 PL2                                      " +
                         "ON PL2.Id = P32.ParLevel2_Id                                  " +
                         "WHERE P321.ParLevel1_Id = '" + ParLevel1_Id + "'              " +
                         "AND PL2.IsActive = 1                                          " +
                         "GROUP BY PL2.Id, PL2.Name, PL2.HasSampleTotal, PL2.IsEmptyLevel3                 ";

                var parLevel2List = db.Query<ParLevel2>(sql);

                return parLevel2List;
            }
        }
    }
    public partial class ParLevel2Evaluate
    {
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public int Id { get; set; }
        public string Name { get; set; }
        public int Evaluate { get; set; }
        //public int? ParCompany_Id { get; set; }

        public IEnumerable<ParLevel2Evaluate> getEvaluate(ParLevel1 ParLevel1, int? ParCompany_Id)
        {

            SqlConnection db = new SqlConnection(conexao);
            string queryCompany = null;
          

            if (ParLevel1.hashKey == 2 && ParCompany_Id != null)
            {

                string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name,              " +
                             "(SELECT Avaliacoes FROM VolumeCepDesossa WHERE Data = (SELECT MAX(DATA) FROM VolumeCepDesossa WHERE ParCompany_id = " + ParCompany_Id + ") and ParCompany_id = " + ParCompany_Id + ") AS Evaluate " +
                             "FROM                                                                        " +
                             "ParLevel3Level2 P32                                                         " +
                             "INNER JOIN ParLevel3Level2Level1 P321                                       " +
                             "ON P321.ParLevel3Level2_Id = P32.Id                                         " +
                             "INNER JOIN ParLevel2 PL2                                                    " +
                             "ON PL2.Id = P32.ParLevel2_Id                                                " +
                           
                             "WHERE P321.ParLevel1_Id = '" + ParLevel1.Id + "'                            " +
                             "GROUP BY PL2.Id, PL2.Name                                                   ";

                var parEvaluate = db.Query<ParLevel2Evaluate>(sql);


                return parEvaluate;


            }
            else if (ParLevel1.hashKey == 3 && ParCompany_Id != null)
            {

                string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name,              " +
                             "(SELECT Avaliacoes FROM VolumeVacuoGRD WHERE Data = (SELECT MAX(DATA) FROM VolumeVacuoGRD WHERE ParCompany_id = " + ParCompany_Id + ") and ParCompany_id = " + ParCompany_Id + ") AS Evaluate " +
                             "FROM                                                                        " +
                             "ParLevel3Level2 P32                                                         " +
                             "INNER JOIN ParLevel3Level2Level1 P321                                       " +
                             "ON P321.ParLevel3Level2_Id = P32.Id                                         " +
                             "INNER JOIN ParLevel2 PL2                                                    " +
                             "ON PL2.Id = P32.ParLevel2_Id                                                " +

                             "WHERE P321.ParLevel1_Id = '" + ParLevel1.Id + "'                            " +
                             "GROUP BY PL2.Id, PL2.Name                                                   ";

                var parEvaluate = db.Query<ParLevel2Evaluate>(sql);


                return parEvaluate;

                
            }
            else if (ParLevel1.hashKey == 4 && ParCompany_Id != null)
            {

                string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name,              " +
                             "(SELECT Avaliacoes FROM VolumeCepRecortes WHERE Data = (SELECT MAX(DATA) FROM VolumeCepRecortes WHERE ParCompany_id = " + ParCompany_Id + ") and ParCompany_id = " + ParCompany_Id + ") AS Evaluate " +
                             "FROM                                                                        " +
                             "ParLevel3Level2 P32                                                         " +
                             "INNER JOIN ParLevel3Level2Level1 P321                                       " +
                             "ON P321.ParLevel3Level2_Id = P32.Id                                         " +
                             "INNER JOIN ParLevel2 PL2                                                    " +
                             "ON PL2.Id = P32.ParLevel2_Id                                                " +

                             "WHERE P321.ParLevel1_Id = '" + ParLevel1.Id + "'                            " +
                             "GROUP BY PL2.Id, PL2.Name                                                   ";

                var parEvaluate = db.Query<ParLevel2Evaluate>(sql);


                return parEvaluate;

                
            }
            else if (ParLevel1.hashKey == 1 && ParCompany_Id != null)
            {

                string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name,              " +
                             "(SELECT Avaliacoes FROM VolumePcc1b WHERE Data = (SELECT MAX(DATA) FROM VolumePcc1b WHERE ParCompany_id = " + ParCompany_Id + ") and ParCompany_id = " + ParCompany_Id + ") AS Evaluate " +
                             "FROM                                                                        " +
                             "ParLevel3Level2 P32                                                         " +
                             "INNER JOIN ParLevel3Level2Level1 P321                                       " +
                             "ON P321.ParLevel3Level2_Id = P32.Id                                         " +
                             "INNER JOIN ParLevel2 PL2                                                    " +
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

                string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name, PE.Number AS Evaluate                " +
                             "FROM                                                                        " +
                             "ParLevel3Level2 P32                                                         " +
                             "INNER JOIN ParLevel3Level2Level1 P321                                       " +
                             "ON P321.ParLevel3Level2_Id = P32.Id                                         " +
                             "INNER JOIN ParLevel2 PL2                                                    " +
                             "ON PL2.Id = P32.ParLevel2_Id                                                " +
                             "INNER JOIN ParEvaluation PE                                                 " +
                             "ON PE.ParLevel2_Id = PL2.Id                                                 " +
                             "WHERE P321.ParLevel1_Id = '" + ParLevel1.Id + "'                            " +
                             queryCompany +
                             "GROUP BY PL2.Id, PL2.Name, PE.Number                                        ";

               // sql = "SELECT 67 AS Id, 'NC Desossa - Alcatra', 50 AS Evaluate";

                var parEvaluate = db.Query<ParLevel2Evaluate>(sql);
                return parEvaluate;

            }


            
        }



    }
    public partial class ParLevel2Sample
    {
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public int Id { get; set; }
        public string Name { get; set; }
        public int Sample { get; set; }
        //public int? ParCompany_Id { get; set; }

        public IEnumerable<ParLevel2Sample> getSample(ParLevel1 ParLevel1, int? ParCompany_Id)
        {

            SqlConnection db = new SqlConnection(conexao);
            string queryCompany = null;

            if (ParLevel1.hashKey == 2 && ParCompany_Id != null)
            {

                string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name,              " +
                             "(SELECT Amostras FROM VolumeCepDesossa WHERE Data = (SELECT MAX(DATA) FROM VolumeCepDesossa WHERE ParCompany_id = " + ParCompany_Id + ") and ParCompany_id = " + ParCompany_Id + ") AS Sample " +
                             "FROM                                                                        " +
                             "ParLevel3Level2 P32                                                         " +
                             "INNER JOIN ParLevel3Level2Level1 P321                                       " +
                             "ON P321.ParLevel3Level2_Id = P32.Id                                         " +
                             "INNER JOIN ParLevel2 PL2                                                    " +
                             "ON PL2.Id = P32.ParLevel2_Id                                                " +

                             "WHERE P321.ParLevel1_Id = '" + ParLevel1.Id + "'                            " +
                             "GROUP BY PL2.Id, PL2.Name                                                   ";

                var parSample = db.Query<ParLevel2Sample>(sql);


                return parSample;


            }
            else if (ParLevel1.hashKey == 3 && ParCompany_Id != null)
            {

                string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name,              " +
                             "(SELECT Amostras FROM VolumeVacuoGRD WHERE Data = (SELECT MAX(DATA) FROM VolumeVacuoGRD WHERE ParCompany_id = " + ParCompany_Id + ") and ParCompany_id = " + ParCompany_Id + ") AS Sample " +
                             "FROM                                                                        " +
                             "ParLevel3Level2 P32                                                         " +
                             "INNER JOIN ParLevel3Level2Level1 P321                                       " +
                             "ON P321.ParLevel3Level2_Id = P32.Id                                         " +
                             "INNER JOIN ParLevel2 PL2                                                    " +
                             "ON PL2.Id = P32.ParLevel2_Id                                                " +

                             "WHERE P321.ParLevel1_Id = '" + ParLevel1.Id + "'                            " +
                             "GROUP BY PL2.Id, PL2.Name                                                   ";

                var parSample = db.Query<ParLevel2Sample>(sql);


                return parSample;


            }
            else if (ParLevel1.hashKey == 4 && ParCompany_Id != null)
            {

                string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name,              " +
                             "(SELECT Amostras FROM VolumeCepRecortes WHERE Data = (SELECT MAX(DATA) FROM VolumeCepRecortes WHERE ParCompany_id = " + ParCompany_Id + ") and ParCompany_id = " + ParCompany_Id + ") AS Sample " +
                             "FROM                                                                        " +
                             "ParLevel3Level2 P32                                                         " +
                             "INNER JOIN ParLevel3Level2Level1 P321                                       " +
                             "ON P321.ParLevel3Level2_Id = P32.Id                                         " +
                             "INNER JOIN ParLevel2 PL2                                                    " +
                             "ON PL2.Id = P32.ParLevel2_Id                                                " +

                             "WHERE P321.ParLevel1_Id = '" + ParLevel1.Id + "'                            " +
                             "GROUP BY PL2.Id, PL2.Name                                                   ";

                var parSample = db.Query<ParLevel2Sample>(sql);


                return parSample;


            }
            else if (ParLevel1.hashKey == 1 && ParCompany_Id != null)
            {

                string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name,              " +
                             "(SELECT Amostras FROM VolumePcc1b WHERE Data = (SELECT MAX(DATA) FROM VolumePcc1b WHERE ParCompany_id = " + ParCompany_Id + ") and ParCompany_id = " + ParCompany_Id + ") AS Sample " +
                             "FROM                                                                        " +
                             "ParLevel3Level2 P32                                                         " +
                             "INNER JOIN ParLevel3Level2Level1 P321                                       " +
                             "ON P321.ParLevel3Level2_Id = P32.Id                                         " +
                             "INNER JOIN ParLevel2 PL2                                                    " +
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
                             "ParLevel3Level2 P32                                              " +
                             "INNER JOIN ParLevel3Level2Level1 P321                            " +
                             "ON P321.ParLevel3Level2_Id = P32.Id                              " +
                             "INNER JOIN ParLevel2 PL2                                         " +
                             "ON PL2.Id = P32.ParLevel2_Id                                     " +
                             "INNER JOIN ParSample PS                                          " +
                             "ON PS.ParLevel2_Id = PL2.Id                                      " +
                             "WHERE P321.ParLevel1_Id = '" + ParLevel1.Id + "'                 " +
                             queryCompany +
                             "GROUP BY PL2.Id, PL2.Name, PS.Number, PS.ParCompany_Id           ";

                var parSample = db.Query<ParLevel2Sample>(sql);

                return parSample;
            }
        }



    }
    public partial class ParLevel3
    {
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

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

        public IEnumerable<ParLevel3> getList()
        {
            SqlConnection db = new SqlConnection(conexao);
            string sql = "SELECT Id, Name FROM ParLevel3";
            var parLevel3List = db.Query<ParLevel3>(sql);

            return parLevel3List;

        }
        //
        public IEnumerable<ParLevel3> getLevel3ByLevel2(SGQDBContext.ParLevel1 ParLevel1, SGQDBContext.ParLevel2 ParLevel2, int ParCompany_Id, DateTime DateCollect)
        {
            SqlConnection db = new SqlConnection(conexao);

            var syncServices = new SgqSystem.Services.SyncServices();

            //Instanciamos variavel de data
            string dataInicio = null;
            string dataFim = null;

            //Pega a data pela regra da frequencia
            syncServices.getFrequencyDate(ParLevel1.ParFrequency_Id, DateCollect, ref dataInicio, ref dataFim);

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

            string sql = "SELECT L3.Id AS Id, L3.Name AS Name, L3G.Id AS ParLevel3Group_Id, L3G.Name AS ParLevel3Group_Name, L3IT.Id AS ParLevel3InputType_Id, L3IT.Name AS ParLevel3InputType_Name, L3V.ParLevel3BoolFalse_Id AS ParLevel3BoolFalse_Id, L3BF.Name AS ParLevel3BoolFalse_Name, L3V.ParLevel3BoolTrue_Id AS ParLevel3BoolTrue_Id, L3BT.Name AS ParLevel3BoolTrue_Name, " +
                         "L3V.IntervalMin AS IntervalMin, L3V.IntervalMax AS IntervalMax, MU.Name AS ParMeasurementUnit_Name, L32.Weight AS Weight, L32.ParCompany_Id                                                                                                                                                                                                                                     " +
                         "FROM ParLevel3 L3                                                                                                                                                                                                                                                                                                                                           " +
                         "INNER JOIN ParLevel3Value L3V                                                                                                                                                                                                                                                                                                                               " +
                         "        ON L3V.ParLevel3_Id = L3.Id AND L3V.IsActive = 1                                                                                                                                                                                                                                                                                                                        " +
                         "INNER JOIN ParLevel3InputType L3IT                                                                                                                                                                                                                                                                                                                          " +
                         "        ON L3IT.Id = L3V.ParLevel3InputType_Id                                                                                                                                                                                                                                                                                                              " +
                         "LEFT JOIN ParLevel3BoolFalse L3BF                                                                                                                                                                                                                                                                                                                           " +
                         "        ON L3BF.Id = L3V.ParLevel3BoolFalse_Id                                                                                                                                                                                                                                                                                                              " +
                         "LEFT JOIN ParLevel3BoolTrue L3BT                                                                                                                                                                                                                                                                                                                            " +
                         "        ON L3BT.Id = L3V.ParLevel3BoolTrue_Id                                                                                                                                                                                                                                                                                                               " +
                         "LEFT JOIN ParMeasurementUnit MU                                                                                                                                                                                                                                                                                                                             " +
                         "        ON MU.Id = L3V.ParMeasurementUnit_Id                                                                                                                                                                                                                                                                                                                " +
                         "LEFT JOIN ParLevel3Level2 L32                                                                                                                                                                                                                                                                                                                               " +
                         "        ON L32.ParLevel3_Id = L3.Id                                                                                                                                                                                                                                                                                                                         " +
                         "LEFT JOIN ParLevel3Group L3G                                                                                                                                                                                                                                                                                                                                " +
                         "        ON L3G.Id = L32.ParLevel3Group_Id                                                                                                                                                                                                                                                                                                                   " +
                         "INNER JOIN ParLevel2 L2                                                                                                                                                                                                                                                                                                                                     " +
                         "        ON L2.Id = L32.ParLevel2_Id                                                                                                                                                                                                                                                                                                                         " +
                         "INNER JOIN ParLevel3Level2Level1 AS L321 ON L321.ParLevel3Level2_Id = L32.Id                                                                                                                                                                                                                                                                                 " +
                         "WHERE  L3.IsActive = 1 AND L32.IsActive = 1                                                                                                                                                                                                                                                                                                                 " +
                         " AND L2.Id = '" + ParLevel2.Id + "' " +
                         " AND(L32.ParCompany_Id = '" + ParCompany_Id + "' OR L32.ParCompany_Id IS NULL) " +
                         " AND L321.ParLevel1_Id='" + ParLevel1.Id + "'                                                                                                        " +


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
            " , L32.ParCompany_Id " +
            "  ORDER BY L3IT.Id ASC, L3G.Name ASC, L3.Name ASC, L32.ParCompany_Id  DESC  ";

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
                var syncServices = new SgqSystem.Services.SyncServices();

                //Instanciamos variavel de data
                string dataInicio = null;
                string dataFim = null;

                //Pega a data pela regra da frequencia
                syncServices.getFrequencyDate(ParLevel1.ParFrequency_Id, DateCollect, ref dataInicio, ref dataFim);

                SqlConnection db = new SqlConnection(conexao);

                string sql = "SELECT R3.ParLevel3_Id AS Id FROM RESULT_LEVEL3 R3 " +
                              "INNER JOIN CollectionLevel2 C2 " +
                              "ON C2.Id = R3.CollectionLevel2_Id " +
                              "INNER JOIN( " +
                              "SELECT L3.Id as ParLevel3_Id, L2.Id as ParLevel2_Id, L321.ParLevel1_Id as ParLevel1_Id " +
                              "FROM ParLevel3 AS L3 INNER JOIN " +
                              "ParLevel3Value AS L3V ON L3V.ParLevel3_Id = L3.Id INNER JOIN " +
                              "ParLevel3InputType AS L3IT ON L3IT.Id = L3V.ParLevel3InputType_Id LEFT OUTER JOIN " +
                              "ParLevel3BoolFalse AS L3BF ON L3BF.Id = L3V.ParLevel3BoolFalse_Id LEFT OUTER JOIN " +
                              "ParLevel3BoolTrue AS L3BT ON L3BT.Id = L3V.ParLevel3BoolTrue_Id LEFT OUTER JOIN " +
                              "ParMeasurementUnit AS MU ON MU.Id = L3V.ParMeasurementUnit_Id LEFT OUTER JOIN " +
                              "ParLevel3Level2 AS L32 ON L32.ParLevel3_Id = L3.Id LEFT OUTER JOIN " +
                              "ParLevel3Group AS L3G ON L3G.Id = L32.ParLevel3Group_Id INNER JOIN " +
                              "ParLevel2 AS L2 ON L2.Id = L32.ParLevel2_Id INNER JOIN " +
                              "ParLevel3Level2Level1 AS L321 ON L321.ParLevel3Level2_Id = L32.Id " +
                              "WHERE(L3.IsActive = 1) AND(L32.IsActive = 1) AND(L2.Id = '" + ParLevel2.Id + "') AND(L32.ParCompany_Id = '" + ParCompany_Id + "' OR " +
                              "                  L32.ParCompany_Id IS NULL) AND L321.ParLevel1_Id = '" + ParLevel1.Id + "' " +
                              "GROUP BY L321.ParLevel1_Id, L2.Id, L3.Id, L3.Name, L3G.Id, L3G.Name, L3IT.Id, L3IT.Name, L3V.ParLevel3BoolFalse_Id, L3BF.Name, L3V.ParLevel3BoolTrue_Id, L3BT.Name, L3V.IntervalMin, L3V.IntervalMax, MU.Name, L32.Weight, " +
                              "                   L32.ParCompany_Id " +
                              ") TAREFAS " +
                              "ON TAREFAS.ParLevel3_Id = R3.ParLevel3_Id AND TAREFAS.ParLevel2_Id = C2.ParLevel2_Id AND TAREFAS.ParLevel1_Id = C2.ParLevel1_Id " +
                              "AND C2.UnitId = '" + ParCompany_Id + "' " +
                              "AND C2.CollectionDate BETWEEN '" + dataInicio + " 00:00:00' AND '" + dataFim + " 23:59:59' ";


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
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

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

        public IEnumerable<Level2Result> getList(int ParLevel1_Id, int ParCompany_Id, string dataInicio, string dataFim)
        {

            SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT ParLevel1_Id, ParLevel2_Id, UnitId AS Unit_Id, Shift, Period, CollectionDate, MAX(EvaluationNumber) AS EvaluateLast, MAX(Sample) AS SampleLast, MAX(ConsolidationLevel2_Id) AS ConsolidationLevel2_Id " +
                         "FROM(SELECT CL2.ParLevel1_Id, CL2.ParLevel2_Id, CL2.UnitId, Shift, Period, CONVERT(date, CollectionDate) AS CollectionDate, EvaluationNumber, MAX(Sample) AS Sample, MAX(ConsolidationLevel2_Id) AS ConsolidationLevel2_Id " +
                         "FROM CollectionLevel2 CL2 " +
                         "INNER JOIN ConsolidationLevel2 CDL2 ON CL2.ConsolidationLevel2_Id = CDL2.ID " +
                         "INNER JOIN ConsolidationLevel1 CDL1 ON CDL2.ConsolidationLevel1_Id = CDL1.Id " +
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

                SqlConnection db = new SqlConnection(conexao);

                string sql = "SELECT MAX(Sample) FROM CollectionLevel2 WHERE ConsolidationLevel2_Id = " + ConsolidationLevel2_Id + " AND EvaluationNumber = " + EvaluationNumber;
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

            SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT CL2.Id, CL2.ParLevel1_Id, CL2.ParLevel2_Id, CL2.UnitId, CL2.Shift, CL2.Period, CL2.EvaluationNumber, CL2.Sample, CL2.ConsolidationLevel2_Id, CL2.[Key] " +
                         "FROM CollectionLevel2 CL2 " +
                         "WHERE CL2.ParLevel1_Id = '" + ParLevel1_Id + "' AND CL2.UnitId = '" + ParCompany_Id + "' AND CL2.CollectionDate BETWEEN '" + dataInicio + " 00:00:00' AND '" + dataFim + " 23:59:59' ";
            var Level2ResultList = db.Query<Level2Result>(sql);
            return Level2ResultList;
        }

    }


    public partial class ParLevel1ConsolidationXParFrequency
    {
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public int ParLevel1_Id { get; set; }
        public int ParFrequency_Id { get; set; }
        public bool IsPartialSave { get; set; }
        public int Id { get; set; }

        public IEnumerable<ParLevel1ConsolidationXParFrequency> getList(int ParCompany_Id, DateTime data)
        {
            try
            {
                SqlConnection db = new SqlConnection(conexao);

                string sql = "SELECT CDL1.Id, CDL1.ParLevel1_Id, PL1.ParFrequency_Id, PL1.IsPartialSave FROM ConsolidationLevel1 CDL1 " +
                             "INNER JOIN ParLevel1 PL1 ON CDL1.ParLevel1_Id = PL1.Id WHERE CDL1.UnitId = '" + ParCompany_Id + "'" +
                             " AND CDL1.Consolidationdate BETWEEN '" + data.ToString("yyyyMMdd") + " 00:00' and '" + data.ToString("yyyyMMdd") + " 23:59'" +
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
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

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

        public int CollectionLevel2_ID_CorrectiveAction { get; set; }

        public int CollectionLevel2_Period_CorrectiveAction { get; set; }
        public ConsolidationResultL1L2 getConsolidation(int ParLevel2_Id, int ParCompany_Id, int? Id)
        {

            SqlConnection db = new SqlConnection(conexao);
            var sql2 = "";

            if (Id != null)
            {
                sql2 = " AND CDL1.Id = " + Id;
            }

            string sql = "SELECT " +
                         "CDL1.AtualAlert AS AlertLevelL1, CDL1.WeiEvaluation AS WeiEvaluationL1, CDL1.EvaluateTotal AS EvaluateTotalL1, CDL1.DefectsTotal AS DefectsTotalL1, CDL1.WeiDefects AS WeiDefectsL1, CDL1.TotalLevel3Evaluation AS TotalLevel3EvaluationL1, CDL1.TotalLevel3WithDefects AS TotalLevel3WithDefectsL1, CDL1.LastEvaluationAlert AS LastEvaluationAlertL1, CDL1.LastLevel2Alert AS LastLevel2AlertL1, CDL1.EvaluatedResult AS EvaluatedResultL1, CDL1.DefectsResult AS DefectsResultL1, " +
                         "CDL2.AlertLevel AS AlertLevelL2, CDL2.WeiEvaluation AS WeiEvaluationL2, CDL2.DefectsTotal AS DefectsL2, CDL2.WeiDefects AS WeiDefectsL2, CDL2.TotalLevel3WithDefects AS TotalLevel3WithDefectsL2, CDL2.TotalLevel3Evaluation AS TotalLevel3EvaluationL2, CDL2.EvaluateTotal AS EvaluateTotalL2, CDL2.DefectsTotal AS DefectsTotalL2, CDL2.EvaluatedResult AS EvaluatedResultL2, CDL2.DefectsResult AS DefectsResultL2, CL2.HaveCorrectiveAction AS HaveCorrectiveAction, MIN(CL2.Id) AS CollectionLevel2_ID_CorrectiveAction, MIN(CL2.Period) AS CollectionLevel2_Period_CorrectiveAction " +
                         "FROM ConsolidationLevel2 AS CDL2 " +
                         "INNER JOIN " +
                         "ConsolidationLevel1 AS CDL1 ON CDL2.ConsolidationLevel1_Id = CDL1.Id " +
                         "LEFT JOIN " +
                         "CollectionLevel2 CL2 ON CL2.ConsolidationLevel2_Id=CDL2.Id AND CL2.HaveCorrectiveAction=1 " +
                         "WHERE(CDL2.ParLevel2_Id = " + ParLevel2_Id + ") AND (CDL1.UnitId = " + ParCompany_Id + ") " +
                         
                         sql2 +

                         " GROUP BY CDL1.AtualAlert, CDL1.WeiEvaluation,CDL1.EvaluateTotal, CDL1.DefectsTotal, CDL1.WeiDefects,  CDL1.TotalLevel3Evaluation, CDL1.TotalLevel3WithDefects, CDL1.LastEvaluationAlert, CDL1.LastLevel2Alert, CDL1.EvaluatedResult, CDL1.DefectsResult, CDL2.AlertLevel, CDL2.WeiEvaluation, CDL2.DefectsTotal, CDL2.WeiDefects, CDL2.TotalLevel3WithDefects, CDL2.TotalLevel3Evaluation, CDL2.EvaluateTotal, CDL2.EvaluatedResult, CDL2.DefectsResult,  CL2.HaveCorrectiveAction";

            var consolidation = db.Query<ConsolidationResultL1L2>(sql).FirstOrDefault();

            return consolidation;
        }
    }

    public partial class ParLevelHeader
    {
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public int ParHeaderField_Id { get; set; }
        public string ParHeaderField_Name { get; set; }
        public int ParFieldType_Id { get; set; }
        public int IsRequired { get; set; }

        public IEnumerable<ParLevelHeader> getHeaderByLevel1(int ParLevel1_Id)
        {
            SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT PH.Id AS ParHeaderField_Id, PH.Name AS ParHeaderField_Name, PT.Id AS ParFieldType_Id, PH.IsRequired AS IsRequired FROM ParLevel1XHeaderField PL  " +
                         "LEFT JOIN ParHeaderField PH ON PH.Id = PL.ParHeaderField_Id                                                                                                                                    " +
                         "LEFT JOIN ParLevelDefiniton PD ON PH.ParLevelDefinition_Id = PD.Id                                                                                                                             " +
                         "LEFT JOIN ParFieldType PT ON PH.ParFieldType_Id = PT.Id                                                                                                                                        " +
                         "WHERE                                                                                                                                                                                          " +
                         "PD.Id = 1 AND                                                                                                                                                                                  " +
                         "PL.ParLevel1_Id = " + ParLevel1_Id + " AND                                                                                                                                                     " +
                         "PL.IsActive = 1 AND PH.IsActive = 1 AND PD.IsActive = 1                                                                                                                                        " +
                         "GROUP BY PH.Id, PH.Name, PT.Id, PH.IsRequired;                                                                                                                             ";



            var parLevel3List = db.Query<ParLevelHeader>(sql);

            return parLevel3List;
        }

        public IEnumerable<ParLevelHeader> getHeaderByLevel1Level2(int ParLevel1_Id, int ParLevel2_Id)
        {
            SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT PH.Id AS ParHeaderField_Id, PH.Name AS ParHeaderField_Name, PT.Id AS ParFieldType_Id, PH.IsRequired AS IsRequired FROM ParLevel1XHeaderField PL  " +
                         "LEFT JOIN ParHeaderField PH ON PH.Id = PL.ParHeaderField_Id                                                                                                                                    " +
                         "LEFT JOIN ParLevelDefiniton PD ON PH.ParLevelDefinition_Id = PD.Id                                                                                                                             " +
                         "LEFT JOIN ParFieldType PT ON PH.ParFieldType_Id = PT.Id                                                                                                                                        " +
                         "WHERE                                                                                                                                                                                          " +
                         "PD.Id = 2 AND                                                                                                                                                                                  " +
                         "PL.ParLevel1_Id = " + ParLevel1_Id + " AND                                                                                                                                                     " +
                         "PL.IsActive = 1 AND PH.IsActive = 1 AND PD.IsActive = 1                                                                                                                                        " +
                         "GROUP BY PH.Id, PH.Name, PT.Id, PH.IsRequired;                                                                                                                             ";

            var parLevel3List = db.Query<ParLevelHeader>(sql);

            return parLevel3List;
        }

        public bool isHeaderLeve2Exception(int ParLevel1_Id, int ParLevel2_Id, int HeaderField_Id)
        {
            SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT * FROM ParLevel2XHeaderField                                                   \n"+
                         "WHERE ParLevel1_Id = "+ ParLevel1_Id +"                                              \n"+
                         "AND ParLevel2_Id = "+ ParLevel2_Id + "                                               \n"+
                         "AND ParHeaderField_Id = " + HeaderField_Id+"                                         \n"+
                         "AND IsActive = 1;                                                                    \n";                                                                                                                            

            var parLevel3List = db.Query<ParLevelHeader>(sql);

            if (parLevel3List.Count() > 0)
                return true;
            else
                return false;
        }
    }
    public partial class ParFieldType
    {
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PunishmentValue { get; set; }
        public int IsDefaultOption { get; set; }

        public IEnumerable<ParFieldType> getMultipleValues(int ParHeaderField_Id)
        {
            SqlConnection db = new SqlConnection(conexao);


            string sql = "SELECT Id, Name, PunishmentValue, IsDefaultOption FROM ParMultipleValues                       " +
                         "WHERE ParHeaderField_Id = '" + ParHeaderField_Id + "' and IsActive = 1;        ";

            var multipleValues = db.Query<ParFieldType>(sql);

            return multipleValues;
        }
    }

    public partial class ParLevel1VariableProduction
    {
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ParLevel1VariableProduction> getVariable(int ParLevel1_Id)
        {
            SqlConnection db = new SqlConnection(conexao);

            string sql = "select P.Id, P.Name from ParLevel1VariableProductionXLevel1 PL left join " +
                         "ParLevel1VariableProduction P on P.Id = PL.ParLevel1VariableProduction_Id " +
                         " where PL.ParLevel1_Id = " + ParLevel1_Id + "; ";

            var list = db.Query<ParLevel1VariableProduction>(sql);

            return list;
        }
    }

    public partial class ParConfSGQContext
    {
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public int Id { get; set; }
        public bool HaveUnitLogin { get; set; }
        public bool HaveShitLogin { get; set; }
        public ParConfSGQContext get()
        {
            SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT Id, HaveUnitLogin, HaveShitLogin FROM ParConfSGQ";

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


        public UserSGQ getUserByLoginOrId(string userLogin=null, int id=0)
        {
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

            SqlConnection db = new SqlConnection(conexao);

            //string sql = "SELECT U.Id, U.Name AS Login, U.Password, U.FullName AS Name, U.ParCompany_Id , C.Name AS ParCompany_Name, PxU.Role " +
            //             "FROM                                                                                                                " +
            //             "UserSgq U                                                                                                           " +
            //             "INNER JOIN ParCompany C ON U.ParCompany_Id = C.Id                                                                   " +
            //             "INNER JOIN ParCompanyXUserSgq PxU ON U.Id = PxU.UserSgq_Id                                                          " +
            //             "WHERE U.Name = '" + userLogin + "' AND PxU.ParCompany_Id = C.Id                                                     ";

            string where = "WHERE U.name = '" + userLogin + "'";
            if(id > 0)
            {
                where = "WHERE U.Id = '" + id + "'";
            }

            string sql = "SELECT U.Id, U.Name AS Login, U.Password, U.FullName AS Name, U.ParCompany_Id , PC.Name AS ParCompany_Name, PxU.Role FROM UserSgq U " +
                         "LEFT JOIN ParCompany PC ON U.ParCompany_Id = PC.Id   " +
                         "LEFT JOIN ParCompanyXUserSgq PxU ON U.ParCompany_Id = PxU.ParCompany_Id AND PxU.UserSgq_Id = U.Id " +
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
        public IEnumerable<ParCompanyXUserSgq> getCompanyUsers(int ParCompany_Id)
        {
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

            SqlConnection db = new SqlConnection(conexao);

            string sql = "select U.Id AS UserSGQ_Id, U.Name AS UserSGQ_Login, U.FullName AS UserSGQ_Name, U.Password AS UserSGQ_Pass, U.Role, PxC.Role AS Role, C.Id ParCompany_Id, C.Name ParCompany_Name from ParCompanyXUserSgq PxC " +
                         "INNER JOIN ParCompany C ON PxC.ParCompany_Id = c.Id                                                                                                                                                          " +
                         "INNER JOIN UserSgq U ON PxC.UserSgq_Id = u.Id                                                                                                                                                                " +
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
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

            SqlConnection db = new SqlConnection(conexao);

            string sql = "select U.Id AS UserSGQ_Id, U.Name AS UserSGQ_Login, U.FullName AS UserSGQ_Name, U.Password AS UserSGQ_Pass, U.Role, PxC.Role AS Role, C.Id ParCompany_Id, C.Name ParCompany_Name from ParCompanyXUserSgq PxC " +
                         "INNER JOIN ParCompany C ON PxC.ParCompany_Id = c.Id                                                                                                                                                          " +
                         "INNER JOIN UserSgq U ON PxC.UserSgq_Id = u.Id                                                                                                                                                                " +
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

        /// <summary>
        /// Retorna todos as permissões do usuário
        /// </summary>
        /// <param name="UserSGQ_Id"></param>
        /// <returns></returns>
        public IEnumerable<RoleXUserSgq> getRoles(int UserSGQ_Id, int ParCompany_id)
        {
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

            SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT TC.HashKey as HashKey, RT.Id as Type, TJbs.Role as RoleJBS, Tsgq.Role as RoleSGQ FROM ScreenComponent TC " +
                         "LEFT JOIN RoleType RT on RT.Id = TC.Type                                                                          " +
                         "LEFT JOIN RoleSGQ TSgq ON Tsgq.ScreenComponent_Id = TC.Id                                                         " +
                         "LEFT JOIN RoleJBS TJbs ON TJbs.ScreenComponent_Id = TC.Id                                                         " +
                         "LEFT JOIN ParCompanyXUserSgq CU ON (CU.Role = TJbs.Role OR TJbs.Role IS NULL)                                       " +
                         "LEFT JOIN UserSgq U ON (U.Role = Tsgq.Role OR Tsgq.Role IS NULL)                                                    " +
                         "WHERE U.Id = CU.UserSgq_Id AND                                                                                          " +
                         "CU.ParCompany_Id = "+ ParCompany_id + " AND                                                                       " +
                         "U.id = "+ UserSGQ_Id + ";                                                                                         " ;

            var users = db.Query<RoleXUserSgq>(sql);

            return users;
        }
    }
    public partial class VolumePcc1b
    {
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public int Id { get; set; }
        public int VolumeAnimais { get; set; }
        public int Quartos { get; set; }
        public int Avaliacoes { get; set; }
        public int Amostras { get; set; }

        public IEnumerable<VolumePcc1b> getVolumePcc1b(int Indicador, int Unidade)
        {
            SqlConnection db = new SqlConnection(conexao);

            string sql = "select VP.Id VP.VolumeAnimais, VP.Quartos, VP.Avaliacoes, VP.Amostras from VolumePcc1b VP where VP.Indicador = " + Indicador + " and VP.Unidade = " + Unidade + "; ";

            var list = db.Query<VolumePcc1b>(sql);

            return list;
        }
    }
    public partial class CaracteristicaTipificacao
    {
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["SGQ_GlobalADO"].ConnectionString;

        public String nCdCaracteristica { get; set; }
        public String cNmCaracteristica { get; set; }
        public String cNrCaracteristica { get; set; }
        public String cSgCaracteristica { get; set; }
        public String cIdentificador { get; set; }

        public IEnumerable<CaracteristicaTipificacao> getCaracteristicasTipificacao(int id)
        {
            SqlConnection db = new SqlConnection(conexao);

            string sql = "select CP.nCdCaracteristica, CP.cNmCaracteristica, CP.cNrCaracteristica, CP.cSgCaracteristica, CP.cIdentificador" +
                         " from CaracteristicaTipificacao CP where LEN(CP.cNrCaracteristica) >= 5 and SUBSTRING(CP.cNrCaracteristica, 1, 3) = '" + id + "';";

            var list = db.Query<CaracteristicaTipificacao>(sql);

            return list;
        }

        public IEnumerable<CaracteristicaTipificacao> getAreasParticipantes()
        {
            SqlConnection db = new SqlConnection(conexao);

            string sql = "select CP.nCdCaracteristica, CP.cNmCaracteristica, CP.cNrCaracteristica, CP.cSgCaracteristica, CP.cIdentificador" +
                         " from AreasParticipantes CP where LEN(cNrCaracteristica) >= 5;";

            var list = db.Query<CaracteristicaTipificacao>(sql);

            return list;
        }

        public IEnumerable<CaracteristicaTipificacao> getCaracteristicasTipificacaoUnico(int ncdcaracteristica)
        {
            SqlConnection db = new SqlConnection(conexao);

            string sql = "select CP.nCdCaracteristica, CP.cNmCaracteristica, CP.cNrCaracteristica, CP.cSgCaracteristica, CP.cIdentificador" +
                         " from CaracteristicaTipificacao CP where cNrCaracteristica = " + ncdcaracteristica;

            var list = db.Query<CaracteristicaTipificacao>(sql);

            return list;
        }

        public IEnumerable<CaracteristicaTipificacao> getAreasParticipantesUnico()
        {
            SqlConnection db = new SqlConnection(conexao);

            string sql = "select CP.nCdCaracteristica, CP.cNmCaracteristica, CP.cNrCaracteristica, CP.cSgCaracteristica, CP.cIdentificador" +
                         " from AreasParticipantes CP where cNrCaracteristica = 0209;";

            var list = db.Query<CaracteristicaTipificacao>(sql);

            return list;
        }


    }
    public partial class VerificacaoTipificacaoTarefaIntegracao
    {
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["SGQ_GlobalADO"].ConnectionString;

        public int Id { get; set; }
        public int TarefaId { get; set; }
        public int CaracteristicaTipificacaoId { get; set; }

        public IEnumerable<VerificacaoTipificacaoTarefaIntegracao> getTarefa(int caracteristicatipificacaoid)
        {
            SqlConnection db = new SqlConnection(conexao);

            string sql = "select Id, TarefaId, CaracteristicaTipificacaoId from VerificacaoTipificacaoTarefaIntegracao where CaracteristicaTipificacaoId = " + caracteristicatipificacaoid;

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

        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public CollectionLevel2Consolidation getConsolidation(int ConsolidationLevel2_Id, int ParLevel2_Id)
        {
            try
            {
                SqlConnection db = new SqlConnection(conexao);

                string sql = "SELECT ConsolidationLevel2_Id, ParLevel2_Id, SUM(WeiEvaluation) AS [WeiEvaluationTotal], SUM(Defects) AS [DefectsTotal], SUM(WeiDefects) AS[WeiDefectsTotal], SUM(TotalLevel3WithDefects) AS [TotalLevel3WithDefects], SUM(TotalLevel3Evaluation) AS [TotalLevel3Evaluation], MAX(LastEvaluationAlert) AS LastEvaluationAlert, (SELECT top 1 LastLevel2Alert FROM CollectionLevel2 WHERE Id = max(c2.id)) AS LastLevel2Alert, SUM(EvaluatedResult) AS EvaluatedResult, SUM(DefectsResult) AS DefectsResult " +
                             "FROM CollectionLevel2 C2 WHERE ConsolidationLevel2_Id = " + ConsolidationLevel2_Id + " AND ParLevel2_Id = " + ParLevel2_Id + " AND NotEvaluatedIs=0" +
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


        public decimal WeiEvaluation{get;set;} //OK
        public decimal EvaluateTotal { get; set; } //OK
        public decimal DefectsTotal { get; set; } 
        public decimal WeiDefects { get; set; }
        public decimal TotalLevel3Evaluation { get; set; }
        public decimal TotalLevel3WithDefects { get; set; }
        public int LastEvaluationAlert { get; set; }
        public int LastLevel2Alert { get; set; }

        public int EvaluatedResult { get; set; }
        public int DefectsResult { get; set; }


        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public ConsolidationLevel1XConsolidationLevel2 getConsolidation(int ConsolidationLevel1_Id)
        {
            try
            {
                SqlConnection db = new SqlConnection(conexao);

                string sql = "select  SUM(WeiEvaluation) AS WeiEvaluation, SUM(EvaluateTotal) AS EvaluateTotal, SUM(DefectsTotal) AS DefectsTotal, SUM(WeiDefects) AS WeiDefects,  SUM(TotalLevel3Evaluation) AS TotalLevel3Evaluation, SUM(TotalLevel3WithDefects) AS TotalLevel3WithDefects, MAX(LastEvaluationAlert) AS LastEvaluationAlert, (SELECT top 1 LastLevel2Alert FROM CollectionLevel2 WHERE Id = max(c2.id)) AS LastLevel2Alert, SUM(EvaluatedResult) AS EvaluatedResult, SUM(DefectsResult) AS DefectsResult FROM ConsolidationLevel2 C2 where ConsolidationLevel1_Id=" + ConsolidationLevel1_Id + "";

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

        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public ConsolidationLevel1 getConsolidation(int ParCompany_Id, int ParLevel1_Id, DateTime collectionDate)
        {
            try
            {
                string sql = "SELECT * FROM ConsolidationLevel1 WHERE UnitId = '" + ParCompany_Id + "' AND ParLevel1_Id= '" + ParLevel1_Id + "' AND CONVERT(date, ConsolidationDate) = '" + collectionDate.ToString("yyyy-MM-dd") + "'";

                SqlConnection db = new SqlConnection(conexao);
                var obj = db.Query<ConsolidationLevel1>(sql).FirstOrDefault();
                return obj;
            }
            catch (Exception)
            {
                throw;
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

        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public ConsolidationLevel2 getConsolidation(int ParCompany_Id, int ParLevel1_Id, DateTime collectionDate)
        {
            try
            {
                string sql = "SELECT * FROM ConsolidationLevel2 WHERE UnitId = '" + ParCompany_Id + "' AND ParLevel1_Id= '" + ParLevel1_Id + "' AND CONVERT(date, ConsolidationDate) = '" + collectionDate.ToString("yyyy-MM-dd") + "'";

                SqlConnection db = new SqlConnection(conexao);
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
                string sql = "SELECT Id, ConsolidationLevel1_Id, UnitId, ParLevel2_Id, ConsolidationDate, WeiEvaluation, EvaluateTotal, DefectsTotal, WeiDefects, TotalLevel3Evaluation, TotalLevel3WithDefects, EvaluatedResult FROM ConsolidationLevel2 WHERE ConsolidationLevel1_Id = '" + ConsolidationLevel1_Id + "' AND ParLevel2_Id= '" + ParLevel2_Id + "' AND UnitId='" + ParCompany_Id + "'";
                SqlConnection db = new SqlConnection(conexao);
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
        public int Unit_Id { get; }
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
        public bool haveReaudit { get; set; }
        public bool haveCorrectiveAction { get; set; }
        public string Device_Id { get; set; }
        public string AppVersion { get; set; }
        public string Ambient { get; set; }
        public bool IsProcessed { get; set; }
        public string Device_Mac { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime? AlterDate { get; set; }
        public string Key { get; set; }
        public string TTP { get; set; }
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;


        public IEnumerable<CollectionJson> getJson(string sql)
        {
            try
            {
                SqlConnection db = new SqlConnection(conexao);
                var list = db.Query<CollectionJson>(sql);
                return list;
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

        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public CollectionLevel2 GetByKey(string key)
        {
            try
            {
                string sql = "SELECT * FROM CollectionLevel2 WHERE [Key] = '" + key + "'";

                SqlConnection db = new SqlConnection(conexao);
                var obj = db.Query<CollectionLevel2>(sql).FirstOrDefault();
                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}