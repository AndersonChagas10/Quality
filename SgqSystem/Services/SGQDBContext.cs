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
        public string Name { get; set; }
        public int ParCriticalLevel_Id { get; set; }
        public string ParCriticalLevel_Name { get; set; }
        public bool HasSaveLevel2 { get; set; }
        public bool HasNoApplicableLevel2 { get; set; }
        public int ParConsolidationType_Id { get; set; }
        public int ParFrequency_Id { get; set; }
        public bool HasAlert { get; set; }
        public bool IsSpecific { get; set; }
        public ParLevel1()
        {

        }

        public IEnumerable<ParLevel1> getParLevel1ParCriticalLevelList(int ParCompany_Id)
        {
            SqlConnection db = new SqlConnection(conexao);
            string sql = " SELECT P1.Id, P1.Name, CL.Id AS ParCriticalLevel_Id, CL.Name AS ParCriticalLevel_Name, P1.HasSaveLevel2 AS HasSaveLevel2, P1.ParConsolidationType_Id AS ParConsolidationType_Id, P1.ParFrequency_Id AS ParFrequency_Id,     " +
                         " P1.HasNoApplicableLevel2 AS HasNoApplicableLevel2, P1.HasAlert, P1.IsSpecific                                                                          " +
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
                         " WHERE CC.ParCompany_Id = '" + ParCompany_Id + "'                                                                           " +
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
        public decimal Nivel3 { get; set; }

        //public DateTime DataCollect { get; set; }
        //public int? Evaluate { get; set; }
        //public int? Sample { get; set; }
        //public int? ParCompany_Id_Evaluate { get; set; }
        //public int? ParCompany_Id_Sample { get; set; }
        public ParLevel1Alertas()
        {

        }
        public ParLevel1Alertas getAlertas(int ParLevel1_Id, int ParCompany_Id, DateTime DataCollect)
        {
            SqlConnection db = new SqlConnection(conexao);

            string sql = "";

            sql += "	SELECT									                                                                                                    			";
            sql += "	(Nivel3) as nivel3                                                                                                                                      ";
            sql += "	,(Nivel3 / 3) * 2 as nivel2				                                                                                                    			";
            sql += "	,(Nivel3 / 3) as nivel1					                                                                                                    			";
            sql += "	FROM									                                                                                                    			";
            sql += "											                                                                                                    			";
            sql += "	(										                                                                                                    			";
            sql += "											                                                                                                    			";
            sql += "		SELECT								                                                                                                    			";
            sql += "											                                                                                                    			";
            sql += "		 [Unidade]							                                                                                                    			";
            sql += "		,[Cód. do Indicador]				                                                                                                    			";
            sql += "		,[Indicador]						                                                                                                    			";
            sql += "		,[Tipo de Consolidação]				                                                                                                    			";
            sql += "		,CASE 								                                                                                                    			";
            sql += "			WHEN [Tipo de Consolidação] = 1 THEN SUM([Número de Avaliações] * [Número de Amostras] * [Peso da Tarefa])										";
            sql += "			WHEN [Tipo de Consolidação] = 3 THEN SUM([Número de Avaliações] * [Número de Amostras])												            ";
            sql += "			WHEN [Tipo de Consolidação] = 2 THEN SUM([Número de Avaliações] * [Número de Amostras] * [Peso da Tarefa])										";
            sql += "		 END AS [Volume total de alerta]													                                                                ";
            sql += "		 ,[Meta do indicador]													                                                                            ";
            sql += "		 ,CASE 											                                                                                                    ";
            sql += "			WHEN [Tipo de Consolidação] = 1 THEN SUM([Número de Avaliações] * [Número de Amostras] * [Peso da Tarefa]) * ([Meta do indicador]/100)			";
            sql += "			WHEN [Tipo de Consolidação] = 3 THEN SUM([Número de Avaliações] * [Número de Amostras]) * ([Meta do indicador]/100)								";
            sql += "			WHEN [Tipo de Consolidação] = 2 THEN SUM([Número de Avaliações] * [Número de Amostras] * [Peso da Tarefa]) * ([Meta do indicador]/100)			";
            sql += "		 END AS Nivel3							                                                                                                    		";
            sql += "												                                                                                                    		";
            sql += "		FROM 									                                                                                                    		";
            sql += "												                                                                                                    		";
            sql += "		(										                                                                                                    		";
            sql += "												                                                                                                    		";
            sql += "		SELECT									                                                                                                    		";
            sql += "												                                                                                                    		";
            sql += "			 NULL                            AS [Unidade]							                                                        				";
            sql += "			,L1.Id                           AS [Cód. do Indicador]					                                                        				";
            sql += "			,L1.Name                         AS [Indicador]							                                                        				";
            sql += "			,L1.ParConsolidationType_Id      AS [Tipo de Consolidação]				                                                        				";
            sql += "			,L1.IsSpecific                   AS [Indicador contém valores específicos por unidades?]											    	    ";
            sql += "			,L1.IsSpecificNumberEvaluetion   AS [Indicador permite que monitoramentos tenham Numero de Avaliações diferentes?]								";
            sql += "			,L1.IsSpecificNumberSample       AS [Indicador permite que monitoramentos tenham Numero de Amostras diferentes?]								";
            sql += "			,L1.IsSpecificLevel3             AS [Indicador permite que unidades tenham tarefas diferentes?]												    ";           sql += "			,L1.IsSpecificGoal               AS [Indicador permite que unidades tenham metas diferentes?]												";
            sql += "			,L1.IsRuleConformity             AS [Tipo de Não conformidade. (Menor ou maior que a meta)]												        ";
            sql += "			,L2.Id                           AS [Cód. do Monitoramento]			                                                        					";
            sql += "			,L2.Name                         AS [Monitoramento]					                                                        					";
            sql += "			,L2.ParFrequency_Id              AS [Frequencia do Monitoramento]	                                                        					";
            sql += "			,AV.Number                       AS [Número de Avaliações]			                                                        					";
            sql += "			,AM.Number                       AS [Número de Amostras]			                                                        					";
            sql += "			,L3.Id							 AS [Cód. da Tarefa]				                                                                            ";
            sql += "			,L3.Name                         AS [Tarefa]						                                                        					";
            sql += "			,P32.Weight                      AS [Peso da Tarefa]				                                                        					";
            sql += "			,Meta.PercentValue               AS [Meta do indicador]				                                                        					";
            sql += "		FROM       ParLevel3Level2Level1 P321						                                                                                    			                                                        				";
            sql += "		INNER JOIN ParLevel3Level2 P32								                                                                                  		";
            sql += "				ON P32.Id = P321.ParLevel3Level2_Id					                                                                   						";
            sql += "		INNER JOIN ParLevel3 L3										                                                                               			";
            sql += "				ON L3.Id = P32.ParLevel3_Id							                                                                           				";
            sql += "		INNER JOIN ParLevel2 L2										                                                                               			";
            sql += "				ON L2.Id = P32.ParLevel2_Id							                                                                           				";
            sql += "		INNER JOIN ParEvaluation AV									                                                                           				";
            sql += "				ON AV.ParLevel2_Id = L2.Id							                                                                           				";
            sql += "		INNER JOIN ParSample AM										                                                                               			";
            sql += "				ON AM.ParLevel2_Id = L2.Id							                                                                           				";
            sql += "		INNER JOIN ParLevel1 L1										                                                                               			";
            sql += "				ON L1.Id = P321.ParLevel1_Id						                                                                       					";
            sql += "		INNER JOIN ParGoal Meta										                                                                               			";
            sql += "				ON Meta.ParLevel1_Id = L1.Id						                                                          	                            ";
            sql += "														                                                                                         	        ";
            sql += "			 WHERE 1=1											                                                                             	            ";
            sql += "			   AND P321.ParLevel1_Id  = '" + ParLevel1_Id + "' 		                                                             							";
            sql += "			   AND P32.ParCompany_Id  IS NULL						                                                             						    ";
            sql += "			   AND AV.ParCompany_Id   IS NULL						                                                             						    ";
            sql += "			   AND AM.ParCompany_Id   IS NULL						                                                            						    ";
            sql += "			   AND Meta.ParCompany_Id IS NULL						                                                            						    ";
            sql += "			   AND P321.Active        = 1							                                                            					        ";
            sql += "			   AND P32.IsActive       = 1							                                                            					        ";
            sql += "			   AND L3.IsActive        = 1							                                                            					        ";
            sql += "			   AND L2.IsActive        = 1							                                                            					        ";
            sql += "			   AND L1.IsActive        = 1							                                                            					        ";
            sql += "			   AND AV.IsActive        = 1							                                                            					        ";
            sql += "			   AND AM.IsActive        = 1							                                                            					        ";
            sql += "			   AND Meta.IsActive      = 1							                                                            					        ";
            sql += "												                                                                                                 			";
            sql += "		) V										                                                                                            			    ";
            sql += "												                                                                                            			    ";
            sql += "		GROUP BY [Unidade], [Cód. do Indicador], [Indicador], [Tipo de Consolidação], [Meta do indicador]		                    						";
            sql += "									                                                                                    			            			";
            sql += "	) META_POR_INDICADOR			                                                                                    						    		";
                                                                                                                                                                       

            var parLevel2List = db.Query<ParLevel1Alertas>(sql).FirstOrDefault();

            return parLevel2List;
        }
    }

    public partial class ParLevel2
    {
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
        public int Id { get; set; }
        public string Name { get; set; }
        //public int? Evaluate { get; set; }
        //public int? Sample { get; set; }
        //public int? ParCompany_Id_Evaluate { get; set; }
        //public int? ParCompany_Id_Sample { get; set; }
        public ParLevel2()
        {

        }
        public IEnumerable<ParLevel2> getLevel2ByIdLevel1(int ParLevel1_Id, int ParCompany_Id)
        {
            SqlConnection db = new SqlConnection(conexao);

            bool parLevel1Familia = false;

            using (var dbEf = new SgqDbDevEntities()) {
          
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
                string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name                                                             " +
                             "FROM ParLevel3Level2 P32                                                                          " +
                             "INNER JOIN ParLevel3Level2Level1 P321                                                             " +
                             "ON P321.ParLevel3Level2_Id = P32.Id                                                               " +
                             "INNER JOIN ParLevel2 PL2                                                                          " +
                             "ON PL2.Id = P32.ParLevel2_Id                                                                      " +
                             "INNER JOIN (SELECT * FROM ParLevel2ControlCompany PL Left Join (SELECT MAX(InitDate) Data, ParCompany_Id AS UNIDADE FROM ParLevel2ControlCompany group by ParCompany_Id) F1 on f1.data = PL.initDate and (F1.UNIDADE = PL.ParCompany_id or F1.UNIDADE is null))  Familia                                                        " +
                             "ON Familia.ParLevel2_Id = PL2.Id                                                                  " +
                             "WHERE P321.ParLevel1_Id = '" + ParLevel1_Id + "'                                                  " +
                             "AND PL2.IsActive = 1                                                                              " +
                             "AND (Familia.ParCompany_Id = '" + ParCompany_Id + "'  or Familia.ParCompany_Id IS NULL)           " +
                             "GROUP BY PL2.Id, PL2.Name                                                                         ";

                var parLevel2List = db.Query<ParLevel2>(sql);

                return parLevel2List;

            } 
            else
            {

                string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name                        " +
                         "FROM ParLevel3Level2 P32                                     " +
                         "INNER JOIN ParLevel3Level2Level1 P321                        " +
                         "ON P321.ParLevel3Level2_Id = P32.Id                          " +
                         "INNER JOIN ParLevel2 PL2                                     " +
                         "ON PL2.Id = P32.ParLevel2_Id                                 " +
                         "WHERE P321.ParLevel1_Id = '" + ParLevel1_Id + "'             " +
                         "AND PL2.IsActive = 1                                         " +
                         "GROUP BY PL2.Id, PL2.Name                                    ";

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
          

            if (ParLevel1.Id == 2 && ParCompany_Id != null)
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
            else if (ParLevel1.Id == 22 && ParCompany_Id != null)
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
            else if (ParLevel1.Id == 23 && ParCompany_Id != null)
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
            else if (ParLevel1.Id == 3 && ParCompany_Id != null)
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

            if (ParLevel1.Id == 2 && ParCompany_Id != null)
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
            else if (ParLevel1.Id == 22 && ParCompany_Id != null)
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
            else if (ParLevel1.Id == 23 && ParCompany_Id != null)
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
            else if (ParLevel1.Id == 3 && ParCompany_Id != null)
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


        public IEnumerable<ParLevel3> getList()
        {
            SqlConnection db = new SqlConnection(conexao);
            string sql = "SELECT Id, Name FROM ParLevel3";
            var parLevel3List = db.Query<ParLevel3>(sql);

            return parLevel3List;

        }
        public IEnumerable<ParLevel3> getLevel3ByLevel2(int ParLevel2_Id)
        {
            SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT L3.Id AS Id, L3.Name AS Name, L3G.Id AS ParLevel3Group_Id, L3G.Name AS ParLevel3Group_Name, L3IT.Id AS ParLevel3InputType_Id, L3IT.Name AS ParLevel3InputType_Name, L3V.ParLevel3BoolFalse_Id AS ParLevel3BoolFalse_Id, L3BF.Name AS ParLevel3BoolFalse_Name, L3V.ParLevel3BoolTrue_Id AS ParLevel3BoolTrue_Id, L3BT.Name AS ParLevel3BoolTrue_Name, " +
                         "L3V.IntervalMin AS IntervalMin, L3V.IntervalMax AS IntervalMax, MU.Name AS ParMeasurementUnit_Name, L32.Weight AS Weight                                                                                                                                                                                                                                    " +
                         "FROM ParLevel3 L3                                                                                                                                                                                                                                                                                                                                           " +
                         "INNER JOIN ParLevel3Value L3V                                                                                                                                                                                                                                                                                                                               " +
                         "        ON L3V.ParLevel3_Id = L3.Id                                                                                                                                                                                                                                                                                                                         " +
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
                         "                                                                                                                                                                                                                                                                                                                                                            " +
                         "WHERE  L3.IsActive = 1 AND L32.IsActive = 1                                                                                                                                                                                                                                                                                                                                                    " +
                         " AND L2.Id = '" + ParLevel2_Id + "'                                                                                                                                                                                                                                                                                                                             ";

            var parLevel3List = db.Query<ParLevel3>(sql);

            return parLevel3List;
        }
    }

    public partial class Level2Result
    {
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public int ParLevel1_Id { get; set; }
        public int ParLevel2_Id { get; set; }
        public int Unit_Id { get; set; }
        public int Shift { get; set; }
        public int Period { get; set; }
        public DateTime CollectionDate { get; set; }
        public int EvaluateLast { get; set; }
        public int SampleLast { get; set; }
        public int ConsolidationLevel2_Id { get; set; }

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

            //string sql = "SELECT                                                           " +
            //            "   ParLevel1_Id                                                   " +
            //            "    , ParLevel2_Id                                                " +
            //            "    , UnitId      AS Unit_Id                                      " +
            //            "    , Shift                                                       " +
            //            "    , Period                                                      " +
            //            "    , CollectionDate                                              " +
            //            "    ,max(EvaluationNumber) as EvaluateLast                        " +
            //            "    ,max(Sample) as SampleLast                                    " +
            //            "    ,max(ConsolidationLevel2_Id)AS ConsolidationLevel2_Id         " +
            //            "   FROM                                                           " +
            //            "   (                                                              " +
            //            "    SELECT                                                        " +
            //            "                                                                  " +
            //            "    ParLevel1_Id                                                  " +
            //            "    , ParLevel2_Id                                                " +
            //            "    , UnitId                                                      " +
            //            "    , Shift                                                       " +
            //            "    , Period                                                      " +
            //            "    , convert(date, CollectionDate) as CollectionDate             " +
            //            "    , EvaluationNumber                                            " +
            //            "    , max(Sample) as Sample                                       " +
            //            "    , max(ConsolidationLevel2_Id)AS ConsolidationLevel2_Id        " +
            //            "                                                                  " +
            //            "    FROM CollectionLevel2                                         " +
            //            "                                                                  " +
            //            "    where UnitId = '" + UnidadeId + "'                            " +
            //            "                                                                  " +
            //            "    group by                                                      " +
            //            "    ParLevel1_Id                                                  " +
            //            "    , ParLevel2_Id                                                " +
            //            "    , UnitId                                                      " +
            //            "    , Shift                                                       " +
            //            "    , Period                                                      " +
            //            "    , convert(date, CollectionDate)                               " +
            //            "    , EvaluationNumber                                            " +
            //            "    , ConsolidationLevel2_Id                                      " +
            //            "   ) ultimas_amostras                                             " +
            //            "                                                                  " +
            //            "   group by                                                       " +
            //            "   ParLevel1_Id                                                   " +
            //            "   , ParLevel2_Id                                                 " +
            //            "   , UnitId                                                       " +
            //            "   , Shift                                                        " +
            //            "   , Period                                                       " +
            //            "   , CollectionDate                                               " +
            //            "   ,ConsolidationLevel2_Id                                        ";

            var Level2ResultList = db.Query<Level2Result>(sql);

            return Level2ResultList;

        }
    }
    public partial class ParLevel1ConsolidationXParFrequency
    {
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public int ParLevel1_Id { get; set; }
        public int ParFrequency_Id { get; set; }

        public IEnumerable<ParLevel1ConsolidationXParFrequency> getList(int ParCompany_Id)
        {
            SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT CDL1.ParLevel1_Id, PL1.ParFrequency_Id FROM ConsolidationLevel1 CDL1 " +
                         "INNER JOIN ParLevel1 PL1 ON CDL1.ParLevel1_Id = PL1.Id WHERE CDL1.UnitId = '" + ParCompany_Id + "' GROUP BY CDL1.ParLevel1_Id, PL1.ParFrequency_Id";

            var consolidation = db.Query<ParLevel1ConsolidationXParFrequency>(sql);

            return consolidation;
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


        public ConsolidationResultL1L2 getConsolidation(int ParLevel2_Id, int ParCompany_Id)
        {

            SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT " +
                         "CDL1.AtualAlert AS AlertLevelL1, CDL1.WeiEvaluation AS WeiEvaluationL1, CDL1.EvaluateTotal AS EvaluateTotalL1, CDL1.DefectsTotal AS DefectsTotalL1, CDL1.WeiDefects AS WeiDefectsL1, CDL1.TotalLevel3Evaluation AS TotalLevel3EvaluationL1, CDL1.TotalLevel3WithDefects AS TotalLevel3WithDefectsL1, CDL1.LastEvaluationAlert AS LastEvaluationAlertL1, CDL1.EvaluatedResult AS EvaluatedResultL1, CDL1.DefectsResult AS DefectsResultL1, " +
                         "CDL2.AlertLevel AS AlertLevelL2, CDL2.WeiEvaluation AS WeiEvaluationL2, CDL2.DefectsTotal AS DefectsL2, CDL2.WeiDefects AS WeiDefectsL2, CDL2.TotalLevel3WithDefects AS TotalLevel3WithDefectsL2, CDL2.TotalLevel3Evaluation AS TotalLevel3EvaluationL2, CDL2.EvaluateTotal AS EvaluateTotalL2, CDL2.DefectsTotal AS DefectsTotalL2, CDL2.EvaluatedResult AS EvaluatedResultL2, CDL2.DefectsResult AS DefectsResultL2 " +
                         "FROM ConsolidationLevel2 AS CDL2 " +
                         "INNER JOIN " +
                         "ConsolidationLevel1 AS CDL1 ON CDL2.ConsolidationLevel1_Id = CDL1.Id " +
                         "WHERE(CDL2.ParLevel2_Id = " + ParLevel2_Id + ") AND (CDL1.UnitId = " + ParCompany_Id + ")";

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

        public IEnumerable<ParLevelHeader> getHeaderByLevel1(int ParLevel1_Id)
        {
            SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT PH.Id AS ParHeaderField_Id, PH.Name AS ParHeaderField_Name, PT.Id AS ParFieldType_Id FROM ParLevel2XHeaderField PL  " +
                         "LEFT JOIN ParHeaderField PH ON PH.Id = PL.ParHeaderField_Id                                                              " +
                         "LEFT JOIN ParLevelDefiniton PD ON PH.ParLevelDefinition_Id = PD.Id                                                       " +
                         "LEFT JOIN ParFieldType PT ON PH.ParFieldType_Id = PT.Id                                                                  " +
                         "WHERE                                                                                                                    " +
                         "PD.Id = 1 AND                                                                                                            " +
                         "PL.ParLevel1_Id = " + ParLevel1_Id + " AND                                                                                " +
                         "PL.IsActive = 1 AND PH.IsActive = 1 AND PD.IsActive = 1                                                                  " +
                         "GROUP BY PH.Id, PH.Name, PT.Id;                                                                                          ";



            var parLevel3List = db.Query<ParLevelHeader>(sql);

            return parLevel3List;
        }

        public IEnumerable<ParLevelHeader> getHeaderByLevel1Level2(int ParLevel1_Id, int ParLevel2_Id)
        {
            SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT PH.Id AS ParHeaderField_Id, PH.Name AS ParHeaderField_Name, PT.Id AS ParFieldType_Id FROM ParLevel2XHeaderField PL " +
                         "  LEFT JOIN ParHeaderField PH ON PH.Id = PL.ParHeaderField_Id                                                             " +
                         "  LEFT JOIN ParLevelDefiniton PD ON PH.ParLevelDefinition_Id = PD.Id                                                      " +
                         "  LEFT JOIN ParFieldType PT ON PH.ParFieldType_Id = PT.Id                                                                 " +
                         "  WHERE                                                                                                                   " +
                         "  PD.Id = 2 AND                                                                                                           " +
                         "  PL.ParLevel1_Id = " + ParLevel1_Id + " AND PL.ParLevel2_Id = " + ParLevel2_Id + " AND                                   " +
                         "  PL.IsActive = 1 AND PH.IsActive = 1 and PD.IsActive = 1;                                                                ";

            var parLevel3List = db.Query<ParLevelHeader>(sql);

            return parLevel3List;
        }
    }
    public partial class ParFieldType
    {
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PunishmentValue { get; set; }

        public IEnumerable<ParFieldType> getMultipleValues(int ParHeaderField_Id)
        {
            SqlConnection db = new SqlConnection(conexao);


            string sql = "SELECT Id, Name, PunishmentValue FROM ParMultipleValues                       " +
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
    public partial class ParConfSGQ
    {
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public int Id { get; set; }
        public bool HaveUnitLogin { get; set; }
        public bool HaveShitLogin { get; set; }
        public ParConfSGQ get()
        {
            SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT Id, HaveUnitLogin, HaveShitLogin FROM ParConfSGQ";

            var conf = db.Query<ParConfSGQ>(sql).FirstOrDefault();

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
        public UserSGQ getUserByLogin(string userLogin)
        {
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

            SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT U.Id, U.Name AS Login, U.Password, U.FullName AS Name, U.ParCompany_Id , C.Name AS ParCompany_Name, PxU.Role " +
                         "FROM                                                                                                                " +
                         "UserSgq U                                                                                                           " +
                         "INNER JOIN ParCompany C ON U.ParCompany_Id = C.Id                                                                   " +
                         "INNER JOIN ParCompanyXUserSgq PxU ON U.Id = PxU.UserSgq_Id                                                          " +
                         "WHERE U.Name = '" + userLogin + "' AND PxU.ParCompany_Id = C.Id                                                     ";

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
                         "WHERE PxC.ParCompany_Id='" + ParCompany_Id + "'                                                                                                                                                              ";

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
                         "WHERE PxC.UserSgq_Id='" + UserSgq_Id + "'                                                                                                                                                              ";

            var companys = db.Query<ParCompanyXUserSgq>(sql);

            return companys;
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

        public int nCdCaracteristica { get; set; }
        public String cNmCaracteristica { get; set; }
        public int cNrCaracteristica { get; set; }
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

    public partial class ConsolidationLevel2
    {

        public int Id { get; set; }
        public int ConsolidationLevel1_Id { get; set; }
        public int ParLevel2_Id { get; set; }
        public DateTime ConsolidationDate { get; set; }
        public decimal WeiEvaluation { get; set; }
        public decimal EvaluateTotal { get; set; }
        public decimal DefectsTotal { get; set; }

        public decimal WeiDefects { get; set; }
        public int TotalLevel3Evaluation { get; set; }
        public int TotalLevel3WithDefects { get; set; }
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;


        public ConsolidationLevel2 getbYConsolidationLevel1(int ConsolidationLevel1_Id, int ParLevel2_Id)
        {

            SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT Id, ConsolidationLevel1_Id, ParLevel2_Id, ConsolidationDate, WeiEvaluation, EvaluateTotal, DefectsTotal, WeiDefects, TotalLevel3Evaluation, TotalLevel3WithDefects FROM ConsolidationLevel2 WHERE ConsolidationLevel1_Id = '" + ConsolidationLevel1_Id + "' AND ParLevel2_Id= '" + ParLevel2_Id + "'";

            var consolidationLevel2 = db.Query<ConsolidationLevel2>(sql).LastOrDefault();

            return consolidationLevel2;
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
        public int EvaluatedResult { get; set; }
        public int DefectsResult { get; set; }

        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public CollectionLevel2Consolidation getConsolidation(int ConsolidationLevel2_Id, int ParLevel2_Id)
        {
            SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT ConsolidationLevel2_Id, ParLevel2_Id, SUM(WeiEvaluation) AS [WeiEvaluationTotal], SUM(Defects) AS [DefectsTotal], SUM(WeiDefects) AS[WeiDefectsTotal], SUM(TotalLevel3WithDefects) AS [TotalLevel3WithDefects], SUM(TotalLevel3Evaluation) AS [TotalLevel3Evaluation], MAX(LastEvaluationAlert) AS LastEvaluationAlert, SUM(EvaluatedResult) AS EvaluatedResult, SUM(DefectsResult) AS DefectsResult " +
                         "FROM CollectionLevel2 WHERE ConsolidationLevel2_Id = " + ConsolidationLevel2_Id + " AND ParLevel2_Id = " + ParLevel2_Id + " " +
                         "group by ConsolidationLevel2_Id, ParLevel2_Id";

            var consolidationLevel2 = db.Query<CollectionLevel2Consolidation>(sql).FirstOrDefault();

            return consolidationLevel2;
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

        public int EvaluatedResult { get; set; }
        public int DefectsResult { get; set; }


        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public ConsolidationLevel1XConsolidationLevel2 getConsolidation(int ConsolidationLevel1_Id)
        {
            SqlConnection db = new SqlConnection(conexao);

            //string sql = "SELECT CL1.ParLevel1_Id, COUNT(CL2.EvaluateTotal) AS EvaluationTotal, SUM(CL2.DefectsTotal) AS DefectsTotal FROM ConsolidationLevel2 CL2 " +
            //             "INNER JOIN ConsolidationLevel1 CL1 ON CL2.ConsolidationLevel1_Id = CL1.Id " +
            //             "WHERE ConsolidationLevel1_Id = " + ConsolidationLevel1_Id + " " +
            //             "GROUP BY CL2.ConsolidationLevel1_Id, CL1.ParLevel1_Id";


            string sql = "select  SUM(WeiEvaluation) AS WeiEvaluation, SUM(EvaluateTotal) AS EvaluateTotal, SUM(DefectsTotal) AS DefectsTotal, SUM(WeiDefects) AS WeiDefects,  SUM(TotalLevel3Evaluation) AS TotalLevel3Evaluation, SUM(TotalLevel3WithDefects) AS TotalLevel3WithDefects, MAX(LastEvaluationAlert) AS LastEvaluationAlert, SUM(EvaluatedResult) AS EvaluatedResult, SUM(DefectsResult) AS DefectsResult FROM ConsolidationLevel2 where ConsolidationLevel1_Id=" + ConsolidationLevel1_Id + "";

            var consolidationLevel1 = db.Query<ConsolidationLevel1XConsolidationLevel2>(sql).FirstOrDefault();

            return consolidationLevel1;
        }

      
    }
  }