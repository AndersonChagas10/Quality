using ADOFactory;
using Dominio;
using Newtonsoft.Json.Linq;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.RelatoriosBrasil
{
    [RoutePrefix("api/SIFReports")]
    public class SIFReportsApiController : BaseApiController
    {

        [HttpPost]
        [Route("Get")]
        public Fdp Get([FromBody] FormularioParaRelatorioViewModel form)
        {
            var query = $@"SELECT
	em_coluna.Sequential AS Numero
   ,CASE
		WHEN 1 > 0 THEN 1
		ELSE 0
	END AS 'Esquerdo'
   ,CASE
		WHEN 2 > 0 THEN 1
		ELSE 0
	END AS 'Direito'
   ,em_coluna.Initials
FROM (SELECT
		C2.Sequential
	   ,C2.Side
	   ,C2.Defects
	   ,PC.Initials
	FROM CollectionLevel2 C2
	LEFT JOIN ParCompany pc
		ON pc.Id = C2.UnitId
	WHERE 1 = 1
	AND ParLevel2_Id IN (SELECT
			ParLevel2_Id
		FROM ParLevel2Level1
		WHERE ParLevel1_Id = {form.level1Id})
	AND C2.ParLevel2_Id = {form.level2Id}
	AND C2.UnitId = 14 -- {form.unitId} 
--and Shift = 1
--AND CAST(CollectionDate AS DATE) = CAST(GETDATE() AS DATE) -- {form._dataInicioSQL}
) em_linha
PIVOT (SUM(Defects) FOR Side IN ([1], [2])) em_coluna
ORDER BY em_coluna.Sequential";

            var retorno = new Fdp();

            using (SgqDbDevEntities dbSgq = new SgqDbDevEntities())
            {
                retorno.Dados = QueryNinja(dbSgq, query);

                if (retorno.Dados.Count > 0)
                {
                    retorno.InitialTime = getInitialTime(form, dbSgq);

                    retorno.FinalTime = getFinalTime(form, dbSgq);
                }

            }

            return retorno;
        }


        public DateTime getInitialTime(FormularioParaRelatorioViewModel form, SgqDbDevEntities dbSgq)
        {

            var query = $@"SELECT
	MIN(CollectionDate)
FROM CollectionLevel2
WHERE 1 = 1
AND UnitId = 14 -- { form.unitId }
AND ParLevel2_Id IN (SELECT
		ParLevel2_Id
	FROM ParLevel2Level1
	WHERE ParLevel1_Id = { form.level1Id })
AND ParLevel2_Id = { form.level2Id }
-- AND CAST(CollectionDate AS DATE) = CAST(GETDATE() AS DATE)";


            return dbSgq.Database.SqlQuery<DateTime>(query).FirstOrDefault();

        }

        public DateTime getFinalTime(FormularioParaRelatorioViewModel form, SgqDbDevEntities dbSgq)
        {
            var query = $@"SELECT
	MAX(CollectionDate)
FROM CollectionLevel2
WHERE 1 = 1
AND UnitId = 14 --{ form.unitId }
AND ParLevel2_Id IN (SELECT
		ParLevel2_Id
	FROM ParLevel2Level1
	WHERE ParLevel1_Id = { form.level1Id })
AND ParLevel2_Id = { form.level2Id }
-- AND CAST(CollectionDate AS DATE) = CAST(GETDATE() AS DATE)";


            return dbSgq.Database.SqlQuery<DateTime>(query).FirstOrDefault();
        }
    }

    public class Fdp
    {
        public dynamic Dados { get; set; }
        public DateTime InitialTime { get; set; }
        public DateTime FinalTime { get; set; }
    }
}
