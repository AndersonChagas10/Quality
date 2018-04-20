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
using ADOFactory;

namespace SgqSystem.Controllers.Api.SelectVinculado
{
    [HandleApi()]
    [RoutePrefix("api/SelectVinculadoApi")]
    public class SelectVinculadoApiController : BaseApiController
    {

        [HttpPost]
        [Route("GetParClusterGroup")]
        public List<ParClusterDTO> GetParClusterGroup([FromBody] int UserId)
        {
            var retorno = new List<ParClusterDTO>();

            if (UserId > 0)
            {

                string unidadesUsuario = GetUserUnits(UserId);

                var query = $@"SELECT
                        DISTINCT
                        	PCG.Id
                           ,PCG.Name
                        FROM ParClusterGroup PCG
                        LEFT JOIN ParCluster PC
                        	ON PC.ParClusterGroup_Id = PCG.Id
                        LEFT JOIN ParCompanyCluster PCC
                        	ON PC.Id = PCC.ParCluster_Id
                        LEFT JOIN ParCompany UNIT
                        	ON PCC.ParCompany_Id = UNIT.Id
                        WHERE UNIT.Id IN ({ unidadesUsuario })";

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    retorno = factory.SearchQuery<ParClusterDTO>(query).ToList();
                }

            }

            return retorno;
        }

        [HttpPost]
        [Route("GetParCluster")]
        public List<ParClusterDTO> GetParCluster([FromBody] ModelForm model)
        {
            var retorno = new List<ParClusterDTO>();

            if (model.UserId > 0)
            {
                string unidadesUsuario = GetUserUnits(model.UserId);
                string whereClusterGroup = "";

                if (model.ClusterGroup > 0)
                {
                    whereClusterGroup = $@"AND PCG.Id = { model.ClusterGroup }";
                }

                var query = $@"SELECT
                        DISTINCT
                        	PC.Id
                           ,pc.Name
                        FROM ParCluster PC
                        LEFT JOIN ParCompanyCluster PCC
                        	ON PC.Id = PCC.ParCluster_Id
                        LEFT JOIN ParCompany UNIT
                        	ON PCC.ParCompany_Id = UNIT.Id
                        LEFT JOIN ParClusterGroup PCG
                        	ON PC.ParClusterGroup_Id = PCG.Id
                        WHERE UNIT.Id IN ({ unidadesUsuario })
                        { whereClusterGroup }";

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    retorno = factory.SearchQuery<ParClusterDTO>(query).ToList();
                }
            }

            return retorno;
        }

        [HttpPost]
        [Route("GetParStructure")]
        public List<ParStructureDTO> GetParStructure([FromBody] ModelForm model)
        {
            var retorno = new List<ParStructureDTO>();

            if (model.UserId > 0)
            {

                var whereCluster = "";
                var unidadesUsuario = GetUserUnits(model.UserId);

                if (model.Cluster > 0)
                {
                    whereCluster = "AND PC.Id = " + model.Cluster;
                }
                else
                if (model.ClusterArr.Length > 0)
                {
                    whereCluster = $"AND PC.Id IN ({ string.Join(",", model.ClusterArr) })";
                }

                var query = $@"SELECT
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
                        WHERE UNIT.Id IN ({unidadesUsuario})
                        { whereCluster }
                        AND PS.Id IS NOT NULL";

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    retorno = factory.SearchQuery<ParStructureDTO>(query).ToList();
                }

            }

            return retorno;
        }

        [HttpPost]
        [Route("GetParCompany")]
        public List<ParCompanyDTO> GetParCompany([FromBody] ModelForm model)
        {
            var retorno = new List<ParCompanyDTO>();

            if (model.UserId > 0)
            {

                var whereStructure = "";
                var whereCluster = "";
                var unidadesUsuario = GetUserUnits(model.UserId);

                if (model.Structure > 0)
                {
                    whereStructure = "and PS.Id = " + model.Structure;
                }
                else
                if (model.StructureArr.Length > 0)
                {
                    whereStructure = $"AND PS.Id IN ({ string.Join(",", model.StructureArr) })";
                }

                if (model.Cluster > 0)
                {
                    whereCluster = "AND PC.Id = " + model.Cluster;
                }
                else
                if (model.ClusterArr.Length > 0)
                {
                    whereCluster = $"AND PC.Id IN ({ string.Join(",", model.ClusterArr) })";
                }

                var query = $@"SELECT
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
                        WHERE UNIT.Id IN ({unidadesUsuario})
                        { whereCluster }
                        { whereStructure } ";

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    retorno = factory.SearchQuery<ParCompanyDTO>(query).ToList();
                }

            }

            return retorno;
        }

        [HttpPost]
        [Route("GetParLevel1Group")]
        public List<ParCriticalLevelDTO> GetParLevel1Group([FromBody] ModelForm model)
        {
            var retorno = new List<ParCriticalLevelDTO>();



            //var whereCluster = "";

            //if (model.Cluster > 0)
            //{
            //    whereCluster = "AND cc.ParCluster_Id = " + model.Cluster;
            //}
            //else
            //    if (model.ClusterArr.Length > 0)
            //{
            //    whereCluster = $"AND cc.ParCluster_Id IN ({ string.Join(",", model.ClusterArr) })";
            //}

            var query = $@"SELECT
                    	distinct p1.Id, p1.Name
                    FROM ParGroupParLevel1 p1
                    WHERE 1 = 1
                    AND IsActive = 1
                    ";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                retorno = factory.SearchQuery<ParCriticalLevelDTO>(query).ToList();
            }


            return retorno;
        }

        [HttpPost]
        [Route("GetParCriticalLevel")]
        public List<ParCriticalLevelDTO> GetParCriticalLevel([FromBody] ModelForm model)
        {
            var retorno = new List<ParCriticalLevelDTO>();



            var whereCluster = "";

            if (model.Cluster > 0)
            {
                whereCluster = "AND cc.ParCluster_Id = " + model.Cluster;
            }
            else
                if (model.ClusterArr.Length > 0)
            {
                whereCluster = $"AND cc.ParCluster_Id IN ({ string.Join(",", model.ClusterArr) })";
            }

            var query = $@"SELECT
                    	distinct cl.Id, cl.Name
                    FROM ParLevel1XCluster cc
                    INNER JOIN ParCriticalLevel cl
                    	ON cc.ParCriticalLevel_Id = cl.id
                    WHERE 1 = 1
                    { whereCluster }";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                retorno = factory.SearchQuery<ParCriticalLevelDTO>(query).ToList();
            }


            return retorno;
        }

        [HttpPost]
        [Route("GetLevel1ParCriticalLevel")]
        public List<ParLevel1DTO> GetLevel1ParCriticalLevel([FromBody] ModelForm model)
        {
            var retorno = new List<ParLevel1DTO>();


            var whereCriticalLevel = "";
            var whereCluster = "";

            if (model.CriticalLevel > 0)
            {
                whereCriticalLevel = "AND pcl.Id = " + model.CriticalLevel;
            }
            else
                if (model.CriticalLevelArr.Length > 0)
            {
                whereCluster = $"AND pcl.Id IN ({ string.Join(",", model.CriticalLevelArr) })";
            }

            if (model.Cluster > 0)
            {
                whereCluster = "AND plc.ParCluster_Id = " + model.Cluster;
            }
            else
                if (model.ClusterArr.Length > 0)
            {
                whereCluster = $"AND plc.ParCluster_Id IN ({ string.Join(",", model.ClusterArr) })";
            }

            var query = $@"SELECT
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
                        { whereCriticalLevel }
                        { whereCluster }
                        --AND PS.Id = 3
                        --AND PC.Id IN (15,11)
                        ORDER BY L1.Name";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                retorno = factory.SearchQuery<ParLevel1DTO>(query).ToList();
            }


            return retorno;
        }

        [HttpPost]
        [Route("GetLevel1ParLevel1Group")]
        public List<ParLevel1DTO> GetLevel1ParLevel1Group([FromBody] ModelForm model)
        {
            var retorno = new List<ParLevel1DTO>();


            var whereCriticalLevel = "";
            var whereCluster = "";
            var whereLevel1Group = "";

            if (model.CriticalLevel > 0)
            {
                whereCriticalLevel = "AND pcl.Id = " + model.CriticalLevel;
            }
            else
                if (model.CriticalLevelArr.Length > 0)
            {
                whereCriticalLevel = $"AND pcl.Id IN ({ string.Join(",", model.CriticalLevelArr) })";
            }

            if (model.Cluster > 0)
            {
                whereCluster = "AND plc.ParCluster_Id = " + model.Cluster;
            }
            else
                if (model.ClusterArr.Length > 0)
            {
                whereCluster = $"AND plc.ParCluster_Id IN ({ string.Join(",", model.ClusterArr) })";
            }

            if (model.groupParLevel1IdArr.Length > 0)
            {
                whereLevel1Group = $"AND PGP1.ParGroupParLevel1_Id IN ({ string.Join(",", model.groupParLevel1IdArr) })"; ;
            }

                var query = $@"SELECT
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
                        LEFT JOIN ParGroupParLevel1XParLevel1 PGP1
	                        ON L1.ID = PGP1.ParLevel1_Id
	                        AND PGP1.IsActive = 1
                        WHERE 1 = 1
                        AND plc.IsActive = 1
                        AND L1.IsActive = 1
                        { whereCriticalLevel }
                        { whereCluster }
                        { whereLevel1Group }
                        ORDER BY L1.Name";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                retorno = factory.SearchQuery<ParLevel1DTO>(query).ToList();
            }


            return retorno;
        }


        private string GetUserUnits(int User)
        {
            using (var db = new SgqDbDevEntities())
            {
                return string.Join(",", db.ParCompanyXUserSgq.Where(r => r.UserSgq_Id == User).Select(r => r.ParCompany_Id).ToList());
            }
        }

    }

    public class ModelForm
    {
        public int UserId { get; set; }
        public int CriticalLevel { get; set; }
        public int Cluster { get; set; }
        public int ClusterGroup { get; set; }
        public int Structure { get; set; }
        public int[] StructureArr { get; set; } = new int[] { };
        public int[] ClusterArr { get; set; } = new int[] { };
        public int[] CriticalLevelArr { get; set; } = new int[] { };
        public int[] groupParLevel1IdArr { get; set; } = new int[] { };

        public int[] Level1IdArr { get; set; } = new int[] { };
        public int[] Level2IdArr { get; set; } = new int[] { };
        public int[] Level3IdArr { get; set; } = new int[] { };

    }
}
