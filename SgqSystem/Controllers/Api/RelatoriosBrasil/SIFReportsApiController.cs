using ADOFactory;
using Dominio;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
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
        public dynamic Get([FromBody] FormularioParaRelatorioViewModel form)
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
FROM (SELECT
		Sequential
	   ,Side
	   ,Defects
	FROM CollectionLevel2
	WHERE 1 = 1
	AND ParLevel2_Id IN (SELECT
			ParLevel2_Id
		FROM ParLevel2Level1
		WHERE ParLevel1_Id = {form.level1Id})
	--AND ParLevel2_Id = {form.level2Id} -- Dianteiro 66 ou 97 Traseiro
	--AND UnitId = {form.unitId} 
	--and Shift = {form.shift}
	--AND CAST(CollectionDate AS DATE) = CAST(GETDATE() AS DATE)
    ) em_linha
PIVOT (SUM(Defects) FOR Side IN ([1], [2])) em_coluna
order by em_coluna.Sequential";

            using (SgqDbDevEntities dbSgq = new SgqDbDevEntities())
            {
                return QueryNinja(dbSgq, query);
            }
        }
    }
}
