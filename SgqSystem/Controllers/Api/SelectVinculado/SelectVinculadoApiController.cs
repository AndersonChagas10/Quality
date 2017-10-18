using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DTO;
using DTO.DTO;
using DTO.DTO.Params;
using Dominio;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace SgqSystem.Controllers.Api.SelectVinculado
{
    [HandleApi()]
    [RoutePrefix("api/SelectVinculadoApi")]
    public class SelectVinculadoApiController : ApiController
    {

        [HttpPost]
        [Route("GetParCluster")]
        public List<ParClusterDTO> GetParCluster(List<UnitDTO> teste)
        {
            var lista = "";

            foreach (var item in teste)
            {
                lista += item.Id.ToString() + ",";
            }

            lista = lista.Remove(lista.Length - 1);

            var query = @"SELECT
                        DISTINCT
                        	PC.Id
                           ,pc.Name
                        FROM ParCluster PC
                        LEFT JOIN ParCompanyCluster PCC
                        	ON PC.Id = PCC.ParCluster_Id
                        LEFT JOIN ParCompany UNIT
                        	ON PCC.ParCompany_Id = UNIT.Id
                        WHERE UNIT.Id IN (" + lista + @")";

            using (var db = new SgqDbDevEntities())
            {
                var retorno = db.Database.SqlQuery<ParClusterDTO>(query).ToList();
                return retorno;
            }
        }

        [HttpPost]
        [Route("GetParStructure")]
        public List<ParStructureDTO> GetParStructure(JObject JForm)
        {

            dynamic Form = JForm;

            var lista = "";

            int? ClusterId = Form.Cluster;
            var whereCluster = "";

            if (ClusterId != null)
            {
                whereCluster = "AND PC.Id = " + ClusterId;
            }

            foreach (var unit in Form.UnitList)
            {
                lista += unit.Id.ToString() + ",";
            }

            lista = lista.Remove(lista.Length - 1);

            var query = @"SELECT
                        DISTINCT
                        	ps.Id
                           ,ps.Name
                        FROM ParCluster PC
                        LEFT JOIN ParCompanyCluster PCC
                        	ON PC.Id = PCC.ParCluster_Id
                        LEFT JOIN ParCompany UNIT
                        	ON PCC.ParCompany_Id = UNIT.Id
                        LEFT JOIN ParCompanyXStructure UNITXSTRUCT
                        	ON UNITXSTRUCT.ParCompany_Id = UNIT.Id
                        LEFT JOIN ParStructure PS
                        	ON PS.Id = UNITXSTRUCT.ParStructure_Id
                        		AND PS.ParStructureParent_Id = 1
                        WHERE UNIT.Id IN (" + lista + @")
                        " + whereCluster + @"
                        AND PS.Id IS NOT NULL";

            using (var db = new SgqDbDevEntities())
            {
                var retorno = db.Database.SqlQuery<ParStructureDTO>(query).ToList();
                return retorno;
            }
        }

        [HttpPost]
        [Route("GetParCompany")]
        public List<ParCompanyDTO> GetParCompany(JObject JForm)
        {
            dynamic Form = JForm;

            var lista = "";
            var whereStructure = "";
            var whereCluster = "";
            int? ClusterId = Form.Cluster;
            int? Structure = Form.Structure;

            if (Structure != null)
            {
                whereStructure = "and PS.Id = " + Structure;
            }

            if (ClusterId != null)
            {
                whereCluster = "AND PC.Id = " + ClusterId;
            }

            foreach (var unit in Form.UnitList)
            {
                lista += unit.Id.ToString() + ",";
            }

            lista = lista.Remove(lista.Length - 1);

            var query = @"SELECT
                        DISTINCT
                        	UNIT.Id
                           ,UNIT.Name
                        FROM ParCluster PC
                        LEFT JOIN ParCompanyCluster PCC
                        	ON PC.Id = PCC.ParCluster_Id
                        LEFT JOIN ParCompany UNIT
                        	ON PCC.ParCompany_Id = UNIT.Id
                        LEFT JOIN ParCompanyXStructure UNITXSTRUCT
                        	ON UNITXSTRUCT.ParCompany_Id = UNIT.Id
                        LEFT JOIN ParStructure PS
                        	ON PS.Id = UNITXSTRUCT.ParStructure_Id
                        		AND PS.ParStructureParent_Id = 1
                        WHERE UNIT.Id IN (" + lista + @")
                        " + whereCluster + @"
                        " + whereStructure + @"";

            using (var db = new SgqDbDevEntities())
            {
                var retorno = db.Database.SqlQuery<ParCompanyDTO>(query).ToList();
                return retorno;
            }
        }

        [HttpPost]
        [Route("GetParCriticalLevel")]
        public List<ParCriticalLevelDTO> GetParCriticalLevel(JObject JForm)
        {
            dynamic Form = JForm;

            var whereCluster = "";
            int? ClusterId = Form.Cluster;

            if (ClusterId != null)
            {
                whereCluster = "AND cc.ParCluster_Id = " + ClusterId;
            }

            var query = @"SELECT
                    	distinct cl.Id, cl.Name
                    FROM ParLevel1XCluster cc
                    INNER JOIN ParCriticalLevel cl
                    	ON cc.ParCriticalLevel_Id = cl.id
                    WHERE 1 = 1
                    " + whereCluster + @"";

            using (var db = new SgqDbDevEntities())
            {
                var retorno = db.Database.SqlQuery<ParCriticalLevelDTO>(query).ToList();
                return retorno;
            }
        }

        [HttpPost]
        [Route("GetLevel1ParCriticalLevel")]
        public List<ParCriticalLevelDTO> GetLevel1ParCriticalLevel(JObject JForm)
        {
            dynamic Form = JForm;

            var whereCriticalLevel = "";
            var whereCluster = "";

            int? CriticalLevel = Form.CriticalLevel;
            int? Cluster = Form.Cluster;

            if (CriticalLevel != null)
            {
                whereCriticalLevel = "AND pcl.Id = " + CriticalLevel;
            }

            if (Cluster != null)
            {
                whereCluster = "AND plc.ParCluster_Id = " + Cluster;
            }

            var query = @"SELECT
                        DISTINCT
                        	l1.Name
                           ,L1.ID
                        FROM ParLevel1XCluster plc
                        INNER JOIN ParCompanyCluster pcc
                        	ON pcc.ParCluster_Id = plc.ParCluster_Id
                        INNER JOIN ParCompany pc
                        	ON pcc.ParCompany_Id = pc.Id
                        INNER JOIN ParCompanyCluster CC
                        	ON CC.ParCompany_Id = PC.Id
                        INNER JOIN ParLevel1 L1
                        	ON plc.ParLevel1_Id = l1.Id
                        INNER JOIN ParCriticalLevel pcl
                        	ON plc.ParCriticalLevel_Id = pcl.Id
                        INNER JOIN ParCompanyXStructure PCS
                        	ON PCS.ParCompany_Id = PC.Id
                        INNER JOIN ParStructure PS
                        	ON PS.ID = PCS.ParStructure_Id
                        	AND PS.ParStructureParent_Id = 1
                        WHERE 1 = 1
                        AND plc.IsActive = 1
                        " + whereCriticalLevel + @"
                        " + whereCluster + @"
                        --AND PS.Id = 3
                        --AND PC.Id IN (15,11)
                        ORDER BY L1.Name";

            using (var db = new SgqDbDevEntities())
            {
                var retorno = db.Database.SqlQuery<ParCriticalLevelDTO>(query).ToList();
                return retorno;
            }
        }
    }
}
