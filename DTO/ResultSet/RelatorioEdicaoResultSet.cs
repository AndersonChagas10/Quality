using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ResultSet
{
    public class RelatorioEdicaoResultSet
    {
        public int ID_collectionlv2 { get; set; }
        public string Indicador { get; set; }
        public string Monitoramento { get; set; }
        public string Tarefa { get { return _result_Level3?.ParLevel3_Name; } }

        public System.DateTime _Data_Coleta { get; set; }
        public string Data_Coleta { get { return _Data_Coleta.ToShortDateString(); } }
        public string Hora_Coleta { get { return _Data_Coleta.ToShortTimeString(); } }
        public System.DateTime _Data_Alteracao { get; set; }
        public string Data_Alteracao { get { return _Data_Alteracao.ToShortDateString(); } }
        public string Hora_Alteracao { get { return _Data_Alteracao.ToShortTimeString(); } }
        public string Nome_Cabecalho { get { return _CollectionLevel2XParHeaderField?.ParHeaderField_Name; } }

        public string HeaderField_Editado { get; set; }
        public string Resultado
        {
            get
            {
                DateTime data;
                if (DateTime.TryParse(_result_Level3?.Value, out data))
                {
                    return data.ToString("dd/MM/yyyy");
                }
                else if (_CollectionLevel2XParHeaderField?.ParHeaderField_ValueName != null)
                {
                    return _CollectionLevel2XParHeaderField?.ParHeaderField_ValueName;
                }
                else
                {
                    return _result_Level3?.Value;
                }
            }
        }

        private string _valor_Texto { get; set; }

        public string Valor_Texto
        {
            get
            {
                DateTime data;
                if (DateTime.TryParse(this._valor_Texto, out data))
                {
                    return data.ToString("dd/MM/yyyy");
                }
                else if (_result_Level3?.ValueText != "undefined" && _result_Level3?.ValueText != "null" && _result_Level3?.ValueText != null)
                {
                    return _result_Level3?.ValueText;
                }
                else
                {
                    return this._valor_Texto;
                }
            }
            set { _valor_Texto = value; }
        }

        public int? Evaluation { get { return Convert.ToInt32(_result_Level3?.Evaluation ?? _CollectionLevel2XParHeaderField?.Evaluation); } }
        public Nullable<int> Sample { get { return _CollectionLevel2XParHeaderField?.Sample; } }
        public string Usuario_Coleta { get; set; }
        public string Usuario_Altera { get; set; }
        public Nullable<int> ParReason_Id { get; set; }
        public string Motivo { get; set; }

        public string DescMotivo { get; set; }
        public string ORIGINAL_EDITADO { get; set; }

        public string IntervalMin { get { return _result_Level3?.IntervalMin; } }
        public string IntervalMax { get { return _result_Level3?.IntervalMax; } }
        public bool IsConform { get { return Convert.ToBoolean(_result_Level3?.IsConform); } }
        public string Conforme { get; set; }
        public int EvaluationNumber { get; set; }
        public string AVALIADO_NAO_AVALIADO { get { return Convert.ToString(_result_Level3?.IsNotEvaluate == false ? 0 : 1); } }
        public int Avaliado { get; set; }
        public string Avaliacao { get; set; }
        public string Amostra { get; set; }
        public System.DateTime Data_Adicao { get; set; }
        public string _Data_Adicao { get { return Data_Adicao.ToShortDateString(); } }
        public string Av_Peso { get { return Convert.ToString(Convert.ToInt32(_result_Level3?.WeiEvaluation)); } }
        public string NC_Peso { get { return Convert.ToString(Convert.ToInt32(_result_Level3?.WeiDefects)); } }
        public string CamposCabecalho { get; set; }
        public string json { get; set; }
        private Dominio.Result_Level3 _result_Level3
        {
            get
            {
                try
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<Dominio.Result_Level3>(json);
                }
                catch
                {
                    return null;
                }
            }
        }
        private Dominio.CollectionLevel2XParHeaderField _CollectionLevel2XParHeaderField
        {
            get
            {
                try
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<Dominio.CollectionLevel2XParHeaderField>(json);
                }
                catch
                {
                    return null;
                }
            }
        }

        public string SelectEdicaoCabecalho(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var sqlModulo = "";
            var sqlTurno = "";
            var sqlUnidade = "";
            var sqlLevel1 = "";
            var sqlLevel2 = "";
            var sqlLevel3 = "";
            var sqlDepartment = "";
            var sqlCargo = "";
            var formatDate = "";
            var sqlSgqMonitor = "";
            var sqlParReason = "";

            #region Filtros

            if (form.ParModule_Ids.Length > 0)
            {
                sqlModulo = $"\n AND plx.ParModule_Id in ({string.Join(",", form.ParModule_Ids)})";
            }

            if (form.Shift_Ids.Length > 0)
            {
                sqlTurno = $"\n AND [Shift] in ({string.Join(",", form.Shift_Ids)})";
            }

            if (form.ParCompany_Ids.Length > 0)
            {
                sqlUnidade = $"\n AND cl.UnitId in ({string.Join(",", form.ParCompany_Ids)})";
            }

            if (form.ParLevel1_Ids.Length > 0)
            {
                sqlLevel1 = $"\n AND cl.ParLevel1_id in ({string.Join(",", form.ParLevel1_Ids)})";
            }

            if (form.ParLevel2_Ids.Length > 0)
            {
                sqlLevel2 = $"\n AND cl.ParLevel2_Id in ({string.Join(",", form.ParLevel2_Ids)})";
            }

            if (form.ParLevel3_Ids.Length > 0)
            {
                sqlLevel3 = $"\n AND rl.ParLevel3_Id  in ({string.Join(",", form.ParLevel3_Ids)})";
            }

            if (form.userSgqMonitor_Ids.Length > 0)
            {
                sqlSgqMonitor = $"\n AND cl.AuditorId  in ({string.Join(",", form.userSgqMonitor_Ids)})";
            }

            if (form.ParReason_Ids.Length > 0)
            {
                sqlParReason = $"\n AND lt.ParReason_Id  in ({string.Join(",", form.ParReason_Ids)})";
            }

            if (form.ParSecao_Ids.Length > 0)
            {
                var sqlDepartamentoPelaHash = "";
                foreach (var item in form.ParSecao_Ids)
                {
                    sqlDepartamentoPelaHash += $@"OR PD.Hash like '{item}|%'
                            OR PD.Hash like '%|{item}|%'
                            OR PD.Hash = '{item}'";
                }
                sqlDepartment = $@" AND (PD.Id in ({string.Join(",", form.ParSecao_Ids)}) 
                             {sqlDepartamentoPelaHash})";
            }
            else if (form.ParDepartment_Ids.Length > 0)
            {
                var sqlDepartamentoPelaHash = "";
                foreach (var item in form.ParDepartment_Ids)
                {
                    sqlDepartamentoPelaHash += $@"OR PD.Hash like '{item}|%'
                            OR PD.Hash like '%|{item}|%'
                            OR PD.Hash = '{item}'";
                }
                sqlDepartment = $@" AND (PD.Id in ({string.Join(",", form.ParDepartment_Ids)}) 
                             {sqlDepartamentoPelaHash})";
            }

            if (form.ParCargo_Ids.Length > 0)
            {
                sqlCargo = $"\n AND PCargo.Id  in ({string.Join(",", form.ParCargo_Ids)})";
            }

            if (GlobalConfig.Eua)
            {
                formatDate = "CONVERT(varchar, CONVERT(DATE, CL2HF2.value, 111), 101)";
            }
            else
            {
                formatDate = "CONVERT(varchar, CONVERT(DATE, CL2HF2.value, 111), 103)";
            }

            #endregion

            var query = $@"
                    --DECLARE @dtinicio varchar(30)
                    --DECLARE @dtfinal varchar(30)
                    --SET @dtinicio = '2019-01-30 00:00:51'
                    --SET @dtfinal = '2019-12-31 23:59:59'
                    
                    SELECT
                    	cl.Id ID_collectionlv2
                       ,p1.Name AS Indicador
                       ,p2.Name AS Monitoramento
                       ,cl.CollectionDate AS _Data_Coleta
                       ,lt.AddDate AS _Data_Alteracao
                       ,CASE
                    		WHEN CLHF.ParFieldType_Id = 1 OR
                    			CLHF.ParFieldType_Id = 3 THEN PMV.Name
                    		WHEN CLHF.ParFieldType_Id = 2 THEN CASE
                    				WHEN HF.Description = 'Produto' THEN CAST(PRD.nCdProduto AS VARCHAR(500)) + ' - ' + PRD.cNmProduto
                    				ELSE EQP.Nome
                    			END
                    		WHEN CLHF.ParFieldType_Id = 6 THEN CONVERT(VARCHAR, CLHF.Value, 103)
                    		ELSE CLHF.Value
                    	END Valor_Texto
                    	--,CLHF.Evaluation
                    	--,CLHF.Sample
                       ,USC.Name AS Usuario_Coleta
                       ,USA.Name AS Usuario_Alteracao
                       ,lt.ParReason_Id
                       ,pr.Motivo
                       ,lt.Motivo AS DescMotivo
                       ,CASE
                    		WHEN lt.ParReason_Id IS NULL THEN 'Original'
                    		ELSE 'Editado'
                    	END AS 'ORIGINAL_EDITADO'
                       ,cl.AddDate AS Data_Adicao
                       ,lt.json AS json
                    
                    FROM
                    -- Log
                    LogTrack lt
                    
                    --Edição Cabeçalho
                    
                    -- Coleta  
                    INNER JOIN CollectionLevel2XParHeaderField CLHF WITH (NOLOCK)
                    	ON CLHF.Id = lt.Json_Id
                    
                    LEFT JOIN CollectionLevel2 cl WITH (NOLOCK)
                    	ON cl.Id = CLHF.CollectionLevel2_Id
                    
                    LEFT JOIN ParLevel1XModule plx WITH (NOLOCK)
                    	ON plx.ParLevel1_Id = cl.ParLevel1_Id
                    
                    -- Parametrizacao
                    LEFT JOIN ParLevel1 p1 WITH (NOLOCK)
                    	ON p1.Id = cl.ParLevel1_Id
                    
                    LEFT JOIN ParLevel2 p2 WITH (NOLOCK)
                    	ON p2.Id = cl.ParLevel2_Id
                    
                    LEFT JOIN ParLevel1XModule pxm WITH (NOLOCK)
                    	ON pxm.ParLevel1_Id = p1.Id
                    
                    
                    LEFT JOIN ParCompany pc WITH (NOLOCK)
                    	ON pc.Id = cl.UnitId
                    
                    -- Dim Usuario
                    
                    LEFT JOIN UserSgq USC WITH (NOLOCK)
                    	ON USC.Id = cl.AuditorId
                    
                    LEFT JOIN UserSgq USA WITH (NOLOCK)
                    	ON USA.Id = lt.UserSgq_Id
                    
                    -- Motivos
                    LEFT JOIN ParReason pr WITH (NOLOCK)
                    	ON pr.Id = lt.ParReason_Id
                    
                    
                    LEFT JOIN ParMultipleValues PMV WITH (NOLOCK)
                    	ON CLHF.Value = CAST(PMV.Id AS VARCHAR(500))
                    
                    LEFT JOIN Produto PRD WITH (NOLOCK)
                    	ON CAST(PRD.nCdProduto AS VARCHAR(500)) = CLHF.Value
                    
                    
                    LEFT JOIN ParHeaderField HF WITH (NOLOCK)
                    	ON CLHF.ParHeaderField_Id = HF.Id
                    
                    LEFT JOIN Equipamentos EQP WITH (NOLOCK)
                    	ON CAST(EQP.Id AS VARCHAR(500)) = CLHF.Value
                    		AND EQP.ParCompany_Id = cl.UnitId
                    		AND CLHF.ParFieldType_Id = 2
                    WHERE 1 = 1
                    
                    --      AND cl.CollectionDate BETWEEN '20190310 00:00' AND '20200310  23:59:59' -- Filtro Data
                    AND lt.Tabela = 'CollectionLevel2XParHeaderField'-- Tabela
                    { sqlModulo }                         
                    { sqlTurno }                         
                    { sqlUnidade }
                    { sqlLevel1 } 
                    { sqlLevel2 } 
                    { sqlSgqMonitor }
                    { sqlParReason }
                    GROUP BY cl.Id
                    		,p1.Name
                    		,p2.Name
                    		,cl.CollectionDate
                    		,lt.AddDate
                    		,CLHF.Value
                    		,CLHF.Evaluation
                    		,CLHF.Sample
                    		,USC.Name
                    		,USA.Name
                    		,lt.ParReason_Id
                    		,pr.Motivo
                    		,lt.Motivo
                    		,cl.AddDate
                    		,lt.json
                    		,PMV.Name
                    		,CLHF.ParFieldType_Id
                    		,HF.Description
                    		,PRD.nCdProduto
                    		,PRD.cNmProduto
                    		,EQP.Nome
                    
                    ORDER BY lt.AddDate ASC
                    
                    DROP TABLE #CollectionLevel2XParHeaderField2";

            return query;
        }

        public string SelectEdicaoResultado(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var sqlModulo = "";
            var sqlTurno = "";
            var sqlUnidade = "";
            var sqlLevel1 = "";
            var sqlLevel2 = "";
            var sqlLevel3 = "";
            var sqlDepartment = "";
            var sqlCargo = "";
            var formatDate = "";
            var sqlSgqMonitor = "";
            var sqlParReason = "";


            if (form.ParModule_Ids.Length > 0)
            {
                sqlModulo = $"\n AND plx.ParModule_Id in ({string.Join(",", form.ParModule_Ids)})";
            }

            if (form.Shift_Ids.Length > 0)
            {
                sqlTurno = $"\n AND [Shift] in ({string.Join(",", form.Shift_Ids)})";
            }

            if (form.ParCompany_Ids.Length > 0)
            {
                sqlUnidade = $"\n AND cl.UnitId in ({string.Join(",", form.ParCompany_Ids)})";
            }

            if (form.ParLevel1_Ids.Length > 0)
            {
                sqlLevel1 = $"\n AND cl.ParLevel1_id in ({string.Join(",", form.ParLevel1_Ids)})";
            }

            if (form.ParLevel2_Ids.Length > 0)
            {
                sqlLevel2 = $"\n AND cl.ParLevel2_Id in ({string.Join(",", form.ParLevel2_Ids)})";
            }

            if (form.ParLevel3_Ids.Length > 0)
            {
                sqlLevel3 = $"\n AND rl.ParLevel3_Id  in ({string.Join(",", form.ParLevel3_Ids)})";
            }

            if (form.userSgqMonitor_Ids.Length > 0)
            {
                sqlSgqMonitor = $"\n AND cl.AuditorId  in ({string.Join(",", form.userSgqMonitor_Ids)})";
            }

            if (form.ParReason_Ids.Length > 0)
            {
                sqlParReason = $"\n AND lt.ParReason_Id  in ({string.Join(",", form.ParReason_Ids)})";
            }

            if (form.ParSecao_Ids.Length > 0)
            {
                var sqlDepartamentoPelaHash = "";
                foreach (var item in form.ParSecao_Ids)
                {
                    sqlDepartamentoPelaHash += $@"OR PD.Hash like '{item}|%'
                            OR PD.Hash like '%|{item}|%'
                            OR PD.Hash = '{item}'";
                }
                sqlDepartment = $@" AND (PD.Id in ({string.Join(",", form.ParSecao_Ids)}) 
                             {sqlDepartamentoPelaHash})";
            }
            else if (form.ParDepartment_Ids.Length > 0)
            {
                var sqlDepartamentoPelaHash = "";
                foreach (var item in form.ParDepartment_Ids)
                {
                    sqlDepartamentoPelaHash += $@"OR PD.Hash like '{item}|%'
                            OR PD.Hash like '%|{item}|%'
                            OR PD.Hash = '{item}'";
                }
                sqlDepartment = $@" AND (PD.Id in ({string.Join(",", form.ParDepartment_Ids)}) 
                             {sqlDepartamentoPelaHash})";
            }

            if (form.ParCargo_Ids.Length > 0)
            {
                sqlCargo = $"\n AND PCargo.Id  in ({string.Join(",", form.ParCargo_Ids)})";
            }

            if (GlobalConfig.Eua)
            {
                formatDate = "CONVERT(varchar, CONVERT(DATE, CL2HF2.value, 111), 101)";
            }
            else
            {
                formatDate = "CONVERT(varchar, CONVERT(DATE, CL2HF2.value, 111), 103)";
            }

            var query = $@" 
               --DECLARE @dtinicio varchar(30), @dtfinal varchar(30)
	        	--SET @dtinicio = '2019-01-30 00:00:51'
	        	--SET @dtfinal = '2019-12-31 23:59:59'

                SELECT
							CL2HF.CollectionLevel2_Id
						   ,STUFF((SELECT DISTINCT
									', ' + CONCAT(HF.Name, ': ', CASE
										WHEN CL2HF2.ParFieldType_Id = 1 OR
											CL2HF2.ParFieldType_Id = 3 THEN PMV.Name
										WHEN CL2HF2.ParFieldType_Id = 2 THEN CASE
												WHEN HF.Description = 'Produto' THEN CAST(PRD.nCdProduto AS VARCHAR(500)) + ' - ' + PRD.cNmProduto
												ELSE EQP.Nome
											END
										WHEN CL2HF2.ParFieldType_Id = 6 THEN CONVERT(VARCHAR, CL2HF2.Value, 103)
										ELSE CL2HF2.Value
									END)
								FROM CollectionLevel2XParHeaderField CL2HF2 (NOLOCK)
								LEFT JOIN CollectionLevel2 CL2 (NOLOCK)
									ON CL2.Id = CL2HF2.CollectionLevel2_Id
								LEFT JOIN ParHeaderField HF (NOLOCK)
									ON CL2HF2.ParHeaderField_Id = HF.Id
								LEFT JOIN ParLevel2 L2 (NOLOCK)
									ON L2.Id = CL2.ParLevel2_Id
								LEFT JOIN ParMultipleValues PMV (NOLOCK)
									ON CL2HF2.Value = CAST(PMV.Id AS VARCHAR(500))
									AND CL2HF2.ParFieldType_Id <> 2
								LEFT JOIN Equipamentos EQP (NOLOCK)
									ON CAST(EQP.Id AS VARCHAR(500)) = CL2HF2.Value
									AND EQP.ParCompany_Id = CL2.UnitId
									AND CL2HF2.ParFieldType_Id = 2
								LEFT JOIN Produto PRD WITH (NOLOCK)
									ON CAST(PRD.nCdProduto AS VARCHAR(500)) = CL2HF2.Value
									AND CL2HF2.ParFieldType_Id = 2
								WHERE CL2HF2.CollectionLevel2_Id = CL2HF.CollectionLevel2_Id
								FOR XML PATH (''))
							, 1, 1, '') AS HeaderFieldLis INTO #CollectionLevel2XParHeaderField2
						FROM CollectionLevel2XParHeaderField CL2HF (NOLOCK)
						INNER JOIN CollectionLevel2 CL2 (NOLOCK)
							ON CL2.Id = CL2HF.CollectionLevel2_Id
						LEFT JOIN ParHeaderField HF (NOLOCK)
							ON CL2HF.ParHeaderField_Id = HF.Id
						LEFT JOIN ParLevel2 L2 (NOLOCK)
							ON L2.Id = CL2.ParLevel2_Id
						GROUP BY CL2HF.CollectionLevel2_Id

	        	SELECT
	        		p1.Name AS Indicador
	        	   ,p2.Name AS Monitoramento
	        	   --,p3.Name AS Tarefa
	        	   ,cl.CollectionDate as _Data_Coleta
				   ,cl.AlterDate as _Data_Alteracao
	        	   --,rl.Value as Resultado 
				   --,rl.ValueText as Valor_Texto 
	        	    ,CASE
	        		 	WHEN rl.isconform = 1 THEN 'Conforme'
	        		 	ELSE 'Não Conforme'
	        		 END AS Conforme
	        	   --,CASE
	        		--	WHEN rl.IsNotEvaluate = 0 THEN 'Avaliado'
	        			--ELSE 'Não Avaliado'
	        		--END AS 'AVALIADO_NAO_AVALIADO'

	        	   ,USC.Name AS Usuario_Coleta
	        	   ,USA.Name AS Usuario_Alteracao
	        	   ,lt.ParReason_Id
	        	   ,pr.Motivo
	        	   ,lt.Motivo as DescMotivo
	        	   ,CASE
                       WHEN lt.ParReason_Id IS NULL THEN 'Original'

                       ELSE 'Editado'

                   END AS 'ORIGINAL_EDITADO'
				   --,rl.WeiEvaluation AS Av_Peso
				   --,rl.IsNotEvaluate AS  Avaliado
				   --,cl.WeiDefects AS NC_Peso
				   ,cl.EvaluationNumber as Avaliacao
				   ,cl.Sample as Amostra
				   ,clxp.HeaderFieldLis AS CamposCabecalho
				   ,cl.AddDate as Data_Adicao
				   ,lt.json


                FROM
               -- Log
               LogTrack lt

               -- Coleta
               INNER JOIN Result_Level3 rl(NOLOCK)

                   ON rl.Id = lt.Json_Id

               LEFT JOIN CollectionLevel2 cl(NOLOCK)

                   ON cl.Id = rl.CollectionLevel2_Id

			    LEFT JOIN #CollectionLevel2XParHeaderField2 clxp
			    ON clxp.CollectionLevel2_Id = cl.Id

                LEFT JOIN ParLevel1XModule plx 
					ON plx.ParLevel1_Id = cl.ParLevel1_Id

               -- Parametrizacao
               LEFT JOIN ParLevel1 p1(NOLOCK)

                   ON p1.Id = cl.ParLevel1_Id

               LEFT JOIN ParLevel2 p2(NOLOCK)

                   ON p2.Id = cl.ParLevel2_Id

               LEFT JOIN ParLevel3 p3(NOLOCK)

                   ON p3.Id = rl.ParLevel3_Id

				LEFT JOIN ParCompany pc
							ON pc.Id = cl.UnitId

               -- Dim Usuario

               LEFT JOIN UserSgq USC(NOLOCK)

                   ON USC.Id = cl.AuditorId

               LEFT JOIN UserSgq USA(NOLOCK)

                   ON USA.Id = lt.UserSgq_Id

               -- Motivos
               LEFT JOIN ParReason pr(NOLOCK)

                   ON pr.Id = lt.ParReason_Id


               WHERE 1 = 1

               AND cl.CollectionDate BETWEEN '{ dtInit } 00:00' AND '{ dtF }  23:59:59' -- Filtro Data
               AND Tabela = 'Result_Level3'-- Tabela
                         { sqlModulo }                         
                         { sqlTurno }                         
                         { sqlUnidade }                        
                         { sqlLevel1 } 
                         { sqlLevel2 }
                         { sqlLevel3 }
                         { sqlSgqMonitor }
                         { sqlParReason }

              ORDER BY cl.CollectionDate, p1.Name, p2.Name,p3.name, lt.AddDate ASC ";

            return query;
        }
    }
}
