using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ResultSet
{
    public class RelatorioEdicaoResultSet
    {

        ////Edição Cabeçalho
        //public System.DateTime Data { get; set; }
        ////public string _Data { get { return Data.ToShortDateString(); /*+ " " + Data.ToShortTimeString();*/ } }
        ////public string _Hora { get { return Data.ToShortTimeString(); } }

        public int ID_collectionlv2 { get; set; }
        public string Indicador { get; set; }
        public string Monitoramento { get; set; }
        public string Tarefa { get; set; }
       
        public System.DateTime _Data_Coleta { get; set; }
        public string Data_Coleta { get { return _Data_Coleta.ToShortDateString(); } }
        public string Hora_Coleta { get { return _Data_Coleta.ToShortTimeString(); } }
        public System.DateTime _Data_Alteracao { get; set; }
        public string Data_Alteracao { get { return _Data_Alteracao.ToShortDateString(); } }
        public string Hora_Alteracao { get { return _Data_Alteracao.ToShortTimeString(); } }
        public string HeaderField_Original { get; set; }
        public string HeaderField_Editado { get; set; }
        public string Value { get; set; }
        public Nullable<int> Evaluation { get; set; }
        public Nullable<int> Sample { get; set; }
        public string Usuario_Coleta { get; set; }
        public string Usuario_Altera { get; set; }
        public Nullable<int> ParReason_Id { get; set; }
        public string Motivo { get; set; }

        public string DescMotivo { get; set; }
        public string ORIGINAL_EDITADO { get; set; }

        public string IntervalMin { get; set; }
        public string IntervalMax { get; set; }
        public bool IsConform { get; set; }
        public string Conforme { get; set; }
        public int EvaluationNumber { get; set; }
        public string AVALIADO_NAO_AVALIADO { get; set; }

        public string SelectEdicaoCabecalho(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var sqlTurno = "";
            var sqlUnidade = "";
            var sqlLevel1 = "";
            var sqlLevel2 = "";
            var sqlLevel3 = "";
            var sqlDepartment = "";
            var sqlCargo = "";
            var formatDate = "";

            if (form.Shift_Ids.Length > 0)
            {
                sqlTurno = $"\n AND [Shift] in ({string.Join(",", form.Shift_Ids)})";
            }

            if (form.ParCompany_Ids.Length > 0)
            {
                sqlUnidade = $"\n AND UnitId in ({string.Join(",", form.ParCompany_Ids)})";
            }

            if (form.ParLevel1_Ids.Length > 0)
            {
                sqlLevel1 = $"\n AND ParLevel1_id in ({string.Join(",", form.ParLevel1_Ids)})";
            }

            if (form.ParLevel2_Ids.Length > 0)
            {
                sqlLevel2 = $"\n AND ParLevel2_Id in ({string.Join(",", form.ParLevel2_Ids)})";
            }

            if (form.ParLevel3_Ids.Length > 0)
            {
                sqlLevel3 = $"\n AND L3.Id  in ({string.Join(",", form.ParLevel3_Ids)})";
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

	        	DECLARE @dtinicio varchar(30), @dtfinal varchar(30)
	        	--SET @dtinicio = '2019-01-30 00:00:51'
	        	--SET @dtfinal = '2019-12-31 23:59:59'

	        	SELECT 
	        		cl.Id ID_collectionlv2
	        	   ,p1.Name AS Indicador
	        	   ,p2.Name AS Monitoramento
	        	   ,cl.CollectionDate as _Data_Coleta
	        	   ,lt.AddDate as _Data_Alteracao
	        	   ,CASE
	        			WHEN cl2hf.headerfieldlis = ' Acesso: Via câmera' 
	        			THEN 'Acesso: In loco'
	        			ELSE 'Acesso: Via câmera'
	        		END AS HeaderField_Original
	        	   ,cl2hf.headerfieldlis HeaderField_Editado
	        	   ,CLHF.Value
	        	   ,CLHF.Evaluation
	        	   ,CLHF.Sample
	        	   ,USC.Name AS Usuario_Coleta
	        	   ,USA.Name AS Usuario_Altera
	        	   ,lt.ParReason_Id
	        	   ,pr.Motivo
	        	   ,lt.Motivo as DescMotivo
	        	   ,CASE
	        			WHEN lt.ParReason_Id IS NULL 
	        			THEN 'ORIGINAL'
	        			ELSE 'EDITADO'
	        		END AS 'ORIGINAL_EDITADO'

               FROM
               -- Log
               LogTrack lt

               -- Coleta
               INNER JOIN CollectionLevel2XParHeaderField CLHF(NOLOCK)

                   ON CLHF.Id = lt.Json_Id

               LEFT JOIN CollectionLevel2 cl(NOLOCK)

                   ON cl.Id = CLHF.CollectionLevel2_Id

               LEFT JOIN #CollectionLevel2XParHeaderField2 cl2hf (NOLOCK)
	        		ON cl2hf.CollectionLevel2_Id = cl.Id

               -- Parametrizacao
               LEFT JOIN ParLevel1 p1(NOLOCK)

                   ON p1.Id = cl.ParLevel1_Id

               LEFT JOIN ParLevel2 p2(NOLOCK)

                   ON p2.Id = cl.ParLevel2_Id


               -- Dim Usuario

               LEFT JOIN UserSgq USC(NOLOCK)

                   ON USC.Id = cl.AuditorId

               LEFT JOIN UserSgq USA(NOLOCK)

                   ON USA.Id = lt.UserSgq_Id

               -- Cabeçalho
               LEFT JOIN ParHeaderField ph(NOLOCK)

                   ON ph.Id = CLHF.ParHeaderField_Id


               -- Motivos
               LEFT JOIN ParReason pr(NOLOCK)

                   ON pr.Id = lt.ParReason_Id

               INNER JOIN ParHeaderField pf(NOLOCK)

                   ON pf.Id = CLHF.ParHeaderField_Id

               INNER JOIN ParMultipleValues pmv(NOLOCK)

                   ON pmv.Id = pf.Id


               WHERE 1 = 1

               AND cl.CollectionDate BETWEEN '{ dtInit } 00:00' AND '{ dtF }  23:59:59' -- Filtro Data

               AND lt.Tabela = 'CollectionLevel2XParHeaderField'-- Tabela
               

              GROUP BY cl.Id,p1.Name
	        	   ,p2.Name
	        	   ,cl.CollectionDate
	        	   ,lt.AddDate
	        	   ,cl2hf.headerfieldlis
	        	   ,CLHF.Value
	        	   ,CLHF.Evaluation
	        	   ,CLHF.Sample
	        	   ,USC.Name
	        	   ,USA.Name
	        	   ,lt.ParReason_Id
	        	   ,pr.Motivo
	        	   ,lt.Motivo

               order by lt.AddDate asc


               DROP TABLE #CollectionLevel2XParHeaderField2


            ";

            return query;
        }

        public string SelectEdicaoResultado(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var sqlTurno = "";
            var sqlUnidade = "";
            var sqlLevel1 = "";
            var sqlLevel2 = "";
            var sqlLevel3 = "";
            var sqlDepartment = "";
            var sqlCargo = "";
            var formatDate = "";

            if (form.Shift_Ids.Length > 0)
            {
                sqlTurno = $"\n AND [Shift] in ({string.Join(",", form.Shift_Ids)})";
            }

            if (form.ParCompany_Ids.Length > 0)
            {
                sqlUnidade = $"\n AND UnitId in ({string.Join(",", form.ParCompany_Ids)})";
            }

            if (form.ParLevel1_Ids.Length > 0)
            {
                sqlLevel1 = $"\n AND ParLevel1_id in ({string.Join(",", form.ParLevel1_Ids)})";
            }

            if (form.ParLevel2_Ids.Length > 0)
            {
                sqlLevel2 = $"\n AND ParLevel2_Id in ({string.Join(",", form.ParLevel2_Ids)})";
            }

            if (form.ParLevel3_Ids.Length > 0)
            {
                sqlLevel3 = $"\n AND L3.Id  in ({string.Join(",", form.ParLevel3_Ids)})";
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
	        		p1.Name AS Indicador
	        	   ,p2.Name AS Monitoramento
	        	   ,p3.Name AS Tarefa
	        	   ,cl.CollectionDate as _Data_Coleta
	        	   ,lt.AddDate as _Data_Alteracao
	        	   ,rl.IntervalMin
	        	   ,rl.IntervalMax
	        	   ,rl.Value
	        	   ,rl.IsConform
	        	   ,CASE
	        			WHEN rl.isconform = 1 THEN 'CONFORME'
	        			ELSE 'NAO CONFORME'
	        		END AS Conforme

	        	   ,cl.EvaluationNumber
	        	   ,CASE
	        			WHEN rl.IsNotEvaluate = 0 THEN 'AVALIADO'
	        			ELSE 'NAO AVALIADO'
	        		END AS 'AVALIADO_NAO_AVALIADO'

	        	   ,USC.Name AS Usuario_Coleta
	        	   ,USA.Name AS Usuario_Altera
	        	   ,lt.ParReason_Id
	        	   ,pr.Motivo
	        	   ,lt.Motivo as DescMotivo
	        	   ,CASE
                       WHEN lt.ParReason_Id IS NULL THEN 'ORIGINAL'

                       ELSE 'EDITADO'

                   END AS 'ORIGINAL_EDITADO'


               FROM
               -- Log
               LogTrack lt

               -- Coleta
               INNER JOIN Result_Level3 rl(NOLOCK)

                   ON rl.Id = lt.Json_Id

               LEFT JOIN CollectionLevel2 cl(NOLOCK)

                   ON cl.Id = rl.CollectionLevel2_Id

               -- Parametrizacao
               LEFT JOIN ParLevel1 p1(NOLOCK)

                   ON p1.Id = cl.ParLevel1_Id

               LEFT JOIN ParLevel2 p2(NOLOCK)

                   ON p2.Id = cl.ParLevel2_Id

               LEFT JOIN ParLevel3 p3(NOLOCK)

                   ON p3.Id = rl.ParLevel3_Id

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

              ORDER BY cl.CollectionDate, p1.Name, p2.Name,p3.name, lt.AddDate ASC ";

            return query;
        }
    }
}
