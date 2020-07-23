using ADOFactory;
using DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Relatorios.SGCOVID
{
    [RoutePrefix("api/RelatorioResultadoCovid")]
    public class RelatorioResultadoCovidApiController : ApiController
    {
        [HttpPost]
        [Route("Get")]
        public List<JObject> GetRelatorio([FromBody] DataCarrierFormularioNew form)
        {
            string whereDate = $"AND cast(C2.CollectionDate as date) BETWEEN cast('{form.startDate.ToString("yyyy-MM-dd")}' as date) AND cast('{ form.endDate.ToString("yyyy-MM-dd") }' as date) ";

            string whereCluster = "";
            if (form.ParCluster_Ids.Length > 0)
                whereCluster = $"C2xC.ParCluster_Id in ({string.Join(",", form.ParCluster_Ids)})";

            string whereStructure2 = "";
            if (form.ParStructure2_Ids.Length > 0)
                whereStructure2 = $"S2.ID in ({string.Join(",", form.ParStructure2_Ids)})";

            string whereStructure3 = $"";
            if (form.ParStructure3_Ids.Length > 0)
                whereStructure3 = $"S1.ID in ({string.Join(",", form.ParStructure3_Ids)})";

            string whereUnidade = "";
            if (form.ParCompany_Ids.Length > 0)
                whereUnidade = $"C2.UnitId in ({string.Join(",", form.ParCompany_Ids)}";

            //string sqlQuery = GetDicionarioEstatico("queryRelatorioLaboratorio");

            string sqlQuery = $@"

DECLARE @FREQUENCIA INT = {(form.ParFrequency_Ids.Count() == 0 ? 0 : form.ParFrequency_Ids[0])}

SELECT
	CASE
		WHEN (@FREQUENCIA = 4) THEN CAST((DAY(CAST(C2.CollectionDate AS DATE)) + (DATEPART(dw, DATEADD(MONTH, DATEDIFF(MONTH, 0, CAST(C2.CollectionDate AS DATE)), 0)) - 1) - 1) / 7 + 1 AS VARCHAR(10))
		WHEN (@FREQUENCIA = 5) THEN CAST((DAY(CAST(C2.CollectionDate AS DATE)) + (DATEPART(dw, DATEADD(MONTH, DATEDIFF(MONTH, 0, CAST(C2.CollectionDate AS DATE)), 0)) - 1) - 1) / 15 + 1 AS VARCHAR(10))
		WHEN (@FREQUENCIA = 6) THEN CAST(DATEPART(MONTH, CAST(C2.CollectionDate AS DATE)) AS VARCHAR(10))
		ELSE CAST(CONVERT(CHAR, CAST(C2.CollectionDate AS DATE), 103) AS VARCHAR(10))
	END AS __Data
   ,xCG.Name AS 'Grupo de Formulário ID'
   ,xCG.Name AS 'Grupo de Formulário'
   ,xC.Id AS 'Formulário ID'
   ,xC.Name AS 'Formulário'
   ,C2.ParFrequency_Id
   ,S3.ID AS Holding
   ,S3.NAME AS Holding
   ,S2.ID AS 'Grupo De Empresa Id'
   ,S2.NAME AS GrupoDeEmpresa
   ,S1.ID AS 'Regional Id'
   ,S1.NAME AS Regional
   ,C2.UnitId AS 'Unidade Id'
   ,UPPER(C.Initials) AS 'Unidade Sigla'
   ,UPPER(C.NAME) AS Unidade
   ,C2.ParLevel1_Id AS 'Indicador Id'
   ,L1.Name AS Indicador
   ,C2.ParLevel2_Id AS 'Monitoramento Id'
   ,L2.Name AS Monitoramento
   ,R3.ParLevel3_Id AS 'Tarefa Id'
   ,L3.Name AS Tarefa
   ,C2.AuditorId 'Auditor Id'
   ,UPPER(U.FullName) AS Auditor
   ,SUM(CASE
		WHEN R3.IsConform = 0 THEN 1
		ELSE 1
	END) AS [Quantidade]
   ,SUM(CASE
		WHEN R3.IsConform = 0 THEN 0
		ELSE 1
	END) AS [Conforme]
   ,SUM(CASE
		WHEN R3.IsConform = 0 THEN 1
		ELSE 0
	END) AS [Não Conforme]
   ,SUM(CAST(R3.IsNotEvaluate AS DECIMAL(38, 10))) AS NA
   ,SUM(R3.WeiEvaluation) AS PontosTotais
   ,SUM(R3.WeiEvaluation) - SUM(R3.WeiDefects) AS PontosAtingidos
   ,SUM(R3.WeiEvaluation) AS AV
   ,SUM(R3.WeiDefects) AS NC
   ,SUM(R3.WeiEvaluation) - SUM(R3.WeiDefects) AS C
FROM CollectionLevel2 C2 WITH (NOLOCK)
INNER JOIN Result_Level3 R3 WITH (NOLOCK)
	ON C2.ID = R3.CollectionLevel2_Id
INNER JOIN CollectionLevel2XCluster C2xC WITH (NOLOCK)
	ON C2.ID = C2xC.CollectionLevel2_Id --AND C2xC.ParCluster_Id IN(13, 29, 31)
INNER JOIN ParCluster xC WITH (NOLOCK)
	ON C2xC.ParCluster_Id = xC.Id
INNER JOIN ParClusterGroup xCG WITH (NOLOCK)
	ON xC.ParClusterGroup_Id = xCG.Id
INNER JOIN CollectionLevel2XParDepartment C2xD WITH (NOLOCK)
	ON C2.ID = C2xD.CollectionLevel2_Id
INNER JOIN ParDepartment D WITH (NOLOCK)
	ON C2xD.ParDepartment_Id = D.Id
INNER JOIN ParDepartment D1 WITH (NOLOCK)
	ON D.Parent_Id = D1.Id
INNER JOIN UserSgq U WITH (NOLOCK)
	ON C2.AuditorId = U.Id
INNER JOIN CollectionLevel2XParCargo C2xG WITH (NOLOCK)
	ON C2.ID = C2xG.CollectionLevel2_Id
INNER JOIN ParCargo xG WITH (NOLOCK)
	ON C2xG.ParCargo_Id = xG.ID
INNER JOIN ParCompany C WITH (NOLOCK)
	ON C2.UnitId = C.ID
INNER JOIN ParLevel1 L1 WITH (NOLOCK)
	ON C2.ParLevel1_Id = L1.ID
INNER JOIN ParLevel2 L2 WITH (NOLOCK)
	ON C2.ParLevel2_Id = L2.ID
INNER JOIN ParLevel3 L3 WITH (NOLOCK)
	ON R3.ParLevel3_Id = L3.ID
INNER JOIN ParCompanyXStructure CS WITH (NOLOCK)
	ON CS.ParCompany_Id = c2.UnitId
INNER JOIN ParStructure S1 WITH (NOLOCK)
	ON CS.ParStructure_Id = S1.Id
INNER JOIN ParStructure S2 WITH (NOLOCK)
	ON S1.ParStructureParent_Id = S2.Id
INNER JOIN ParStructure S3 WITH (NOLOCK)
	ON S2.ParStructureParent_Id = S3.Id
WHERE 1 = 1
{whereUnidade}
{whereDate}
{whereCluster}
{whereStructure2}
{whereStructure3}
GROUP BY CASE
			 WHEN (@FREQUENCIA = 4) THEN CAST((DAY(CAST(C2.CollectionDate AS DATE)) + (DATEPART(dw, DATEADD(MONTH, DATEDIFF(MONTH, 0, CAST(C2.CollectionDate AS DATE)), 0)) - 1) - 1) / 7 + 1 AS VARCHAR(10))
			 WHEN (@FREQUENCIA = 5) THEN CAST((DAY(CAST(C2.CollectionDate AS DATE)) + (DATEPART(dw, DATEADD(MONTH, DATEDIFF(MONTH, 0, CAST(C2.CollectionDate AS DATE)), 0)) - 1) - 1) / 15 + 1 AS VARCHAR(10))
			 WHEN (@FREQUENCIA = 6) THEN CAST(DATEPART(MONTH, CAST(C2.CollectionDate AS DATE)) AS VARCHAR(10))
			 ELSE CAST(CONVERT(CHAR, CAST(C2.CollectionDate AS DATE), 103) AS VARCHAR(10))
		 END
		,CASE
			 WHEN (@FREQUENCIA = 4) THEN CAST((DAY(CAST(C2.CollectionDate AS DATE)) + (DATEPART(dw, DATEADD(MONTH, DATEDIFF(MONTH, 0, CAST(C2.CollectionDate AS DATE)), 0)) - 1) - 1) / 7 + 1 AS VARCHAR(10))
			 WHEN (@FREQUENCIA = 5) THEN CAST((DAY(CAST(C2.CollectionDate AS DATE)) + (DATEPART(dw, DATEADD(MONTH, DATEDIFF(MONTH, 0, CAST(C2.CollectionDate AS DATE)), 0)) - 1) - 1) / 15 + 1 AS VARCHAR(10))
			 WHEN (@FREQUENCIA = 6) THEN CAST(DATEPART(MONTH, CAST(C2.CollectionDate AS DATE)) AS VARCHAR(10))
			 ELSE CAST(CONVERT(CHAR, CAST(C2.CollectionDate AS DATE)) AS VARCHAR(10))
		 END
		,C2.ParFrequency_Id
		,S2.ParStructureParent_Id
		,S1.ParStructureParent_Id
		,CS.ParStructure_Id
		,C2.UnitId
		,C2.ParLevel1_Id
		,C2.ParLevel2_Id
		,R3.ParLevel3_Id
		,C2.AuditorId
		,xCG.Id
		,xCG.Name
		,xC.Id
		,xC.Name
		,C2xC.ParCluster_Id
		,S3.ID
		,S3.NAME
		,S2.ID
		,S2.NAME
		,S1.ID
		,S1.NAME
		,L1.NAME
		,L2.NAME
		,L3.NAME
		,C.NAME
		,U.FullName
		,C.Initials
ORDER BY CASE
	WHEN (@FREQUENCIA = 4) THEN CAST((DAY(CAST(C2.CollectionDate AS DATE)) + (DATEPART(dw, DATEADD(MONTH, DATEDIFF(MONTH, 0, CAST(C2.CollectionDate AS DATE)), 0)) - 1) - 1) / 7 + 1 AS VARCHAR(10))
	WHEN (@FREQUENCIA = 5) THEN CAST((DAY(CAST(C2.CollectionDate AS DATE)) + (DATEPART(dw, DATEADD(MONTH, DATEDIFF(MONTH, 0, CAST(C2.CollectionDate AS DATE)), 0)) - 1) - 1) / 15 + 1 AS VARCHAR(10))
	WHEN (@FREQUENCIA = 6) THEN CAST(DATEPART(MONTH, CAST(C2.CollectionDate AS DATE)) AS VARCHAR(10))
	ELSE CAST(CONVERT(CHAR, CAST(C2.CollectionDate AS DATE)) AS VARCHAR(10))
END";


            var retorno = new List<JObject>();

            using (var db = new Factory("DefaultConnection"))
            {

                retorno = db.QueryNinjaADO(sqlQuery);
            }

            return retorno;
        }
    }
}
