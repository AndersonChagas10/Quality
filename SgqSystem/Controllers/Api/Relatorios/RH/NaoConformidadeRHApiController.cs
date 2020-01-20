using ADOFactory;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api.Relatorios.RH
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/NaoConformidadeRH")]
    public class NaoConformidadeRHApiController : BaseApiController
    {
        private List<NaoConformidadeRHResultsSet> _mock { get; set; }
        private List<NaoConformidadeRHResultsSet> _list { get; set; }

        [HttpPost]
        [Route("GraficoHolding")]
        public List<NaoConformidadeRHResultsSet> GraficoHolding([FromBody] DTO.DataCarrierFormularioNew form)
        {
            var whereDepartment = "";
            var whereSecao = "";
            var whereCargo = "";
            var whereStructure = "";
            var whereUnit = "";
            var whereCluster = "";
            var whereClusterGroup = "";

            if (form.ParDepartment_Ids.Length > 0)
            {
                whereDepartment = $@" AND L2.Centro_De_Custo_Id in ({string.Join(",", form.ParDepartment_Ids)}) ";
            }

            if (form.ParSecao_Ids.Length > 0)
            {
                whereSecao = $@" AND L2.Secao_Id in ({string.Join(",", form.ParSecao_Ids)}) ";
            }

            if (form.ParCargo_Ids.Length > 0)
            {
                whereCargo = $@" AND L2.Cargo_Id in ({string.Join(",", form.ParCargo_Ids)}) ";
            }

            if (form.ParCompany_Ids.Length > 0 && form.ParCompany_Ids[0] > 0)
            {
                whereUnit = $@"AND L2.UnitId in ({ string.Join(",", form.ParCompany_Ids) }) ";
            }

            if (form.ParClusterGroup_Ids.Length > 0)
            {
                whereClusterGroup = $@"AND PCG.Id in (" + string.Join(",", form.ParClusterGroup_Ids) + ")";
            }

            if (form.ParCluster_Ids.Length > 0)
            {
                whereCluster = $@"AND PC.Id in (" + string.Join(",", form.ParCluster_Ids) + ")";
            }

            if (form.ParStructure_Ids.Length > 0)
            {
                whereStructure = $@"AND L2.Regional in ({string.Join(",", form.ParStructure_Ids)})";
            }

            var query = $@"
                
                 DECLARE @DATAINICIAL DATETIME = '{ form.startDate.ToString("yyyy-MM-dd")} {" 00:00:00"}'
                 DECLARE @DATAFINAL   DATETIME = '{ form.endDate.ToString("yyyy-MM-dd") } {" 23:59:59"}'

                SELECT 
	                Holding.NAME AS HoldingName,
                    Holding.Id as Holding_Id,
	                SUM(WeiEvaluation) AS AV,
	                SUM(WeiDefects) AS NC,
	                SUM(WeiDefects)/SUM(WeiEvaluation)*100 AS [PROC]
	                FROM DW.Cubo_Coleta_L2 L2 WITH (NOLOCK)
	                INNER JOIN ParCompany C WITH (NOLOCK) ON L2.Unitid = C.ID
					LEFT JOIN ParVinculoPeso PVP ON L2.ParLevel2_Id = PVP.ParLevel2_Id
						AND L2.ParLevel1_Id = PVP.ParLevel1_Id
						AND L2.Cargo_Id = PVP.ParCargo_Id
						AND L2.ParFrequency_Id = PVP.ParFrequencyId
				    LEFT JOIN ParCluster PC ON PVP.ParCluster_Id = PC.Id
		            LEFT JOIN ParClusterGroup PCG ON PC.ParClusterGroup_Id = PCG.Id
					LEft Join (select * from ParStructure where ParStructureGroup_Id = 1) Holding on L2.Holding = Holding.Id
	                WHERE 1=1
	                AND CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
                    {whereStructure}
                    {whereUnit}
                    {whereDepartment}
                    {whereSecao}
                    {whereCargo}
                    {whereCluster}
                    {whereClusterGroup}
                GROUP BY 
	                Holding.NAME, Holding.Id 
                ";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeRHResultsSet>(query).ToList();
            }

            return _list;
        }

        [HttpPost]
        [Route("GraficoNegocio")]
        public List<NaoConformidadeRHResultsSet> GraficoNegocio([FromBody] DTO.DataCarrierFormularioNew form)
        {
            var whereDepartment = "";
            var whereSecao = "";
            var whereCargo = "";
            var whereStructure = "";
            var whereUnit = "";
            var whereCluster = "";
            var whereClusterGroup = "";

            if (form.ParDepartment_Ids.Length > 0)
            {
                whereDepartment = $@" AND L2.Centro_De_Custo_Id in ({string.Join(",", form.ParDepartment_Ids)}) ";
            }

            if (form.ParSecao_Ids.Length > 0)
            {
                whereSecao = $@" AND L2.Secao_Id in ({string.Join(",", form.ParSecao_Ids)}) ";
            }

            if (form.ParCargo_Ids.Length > 0)
            {
                whereCargo = $@" AND L2.Cargo_Id in ({string.Join(",", form.ParCargo_Ids)}) ";
            }

            if (form.ParCompany_Ids.Length > 0 && form.ParCompany_Ids[0] > 0)
            {
                whereUnit = $@"AND L2.UnitId in ({ string.Join(",", form.ParCompany_Ids) }) ";
            }

            if (form.ParClusterGroup_Ids.Length > 0)
            {
                whereClusterGroup = $@"AND PCG.Id in (" + string.Join(",", form.ParClusterGroup_Ids) + ")";
            }

            if (form.ParCluster_Ids.Length > 0)
            {
                whereCluster = $@"AND PC.Id in (" + string.Join(",", form.ParCluster_Ids) + ")";
            }

            if (form.ParStructure_Ids.Length > 0)
            {
                whereStructure = $@"AND L2.Regional in ({string.Join(",", form.ParStructure_Ids)})";
            }

            var query = $@"
                
                 DECLARE @DATAINICIAL DATETIME = '{ form.startDate.ToString("yyyy-MM-dd")} {" 00:00:00"}'
                 DECLARE @DATAFINAL   DATETIME = '{ form.endDate.ToString("yyyy-MM-dd") } {" 23:59:59"}'

                SELECT 
	                GrupoDeEmpresa.NAME AS GrupoDeEmpresaName,
                    GrupoDeEmpresa.Id as GrupoDeEmpresa_Id,
	                SUM(WeiEvaluation) AS AV,
	                SUM(WeiDefects) AS NC,
	                SUM(WeiDefects)/SUM(WeiEvaluation)*100 AS [PROC]
	                FROM DW.Cubo_Coleta_L2 L2 WITH (NOLOCK)
	                INNER JOIN ParCompany C WITH (NOLOCK) ON L2.Unitid = C.ID
					LEFT JOIN ParVinculoPeso PVP ON L2.ParLevel2_Id = PVP.ParLevel2_Id
						AND L2.ParLevel1_Id = PVP.ParLevel1_Id
						AND L2.Cargo_Id = PVP.ParCargo_Id
						AND L2.ParFrequency_Id = PVP.ParFrequencyId
				    LEFT JOIN ParCluster PC ON PVP.ParCluster_Id = PC.Id
		            LEFT JOIN ParClusterGroup PCG ON PC.ParClusterGroup_Id = PCG.Id
					LEft Join (select * from ParStructure where ParStructureGroup_Id = 1) Holding on L2.Holding = Holding.Id
					LEft Join (select * from ParStructure where ParStructureGroup_Id = 2) GrupoDeEmpresa on L2.GrupoDeEmpresa = GrupoDeEmpresa.Id
	                WHERE 1=1
	                AND CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
                    {whereStructure}
                    {whereUnit}
                    {whereDepartment}
                    {whereSecao}
                    {whereCargo}
                    {whereCluster}
                    {whereClusterGroup}
					AND Holding.Id = {form.Param["holding_Id"]}
                GROUP BY 
	                GrupoDeEmpresa.NAME, GrupoDeEmpresa.Id 
                ";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeRHResultsSet>(query).ToList();
            }

            return _list;
        }

        [HttpPost]
        [Route("GraficoRegional")]
        public List<NaoConformidadeRHResultsSet> GraficoRegional([FromBody] DTO.DataCarrierFormularioNew form)
        {
            var whereDepartment = "";
            var whereSecao = "";
            var whereCargo = "";
            var whereStructure = "";
            var whereUnit = "";
            var whereCluster = "";
            var whereClusterGroup = "";

            if (form.ParDepartment_Ids.Length > 0)
            {
                whereDepartment = $@" AND L2.Centro_De_Custo_Id in ({string.Join(",", form.ParDepartment_Ids)}) ";
            }

            if (form.ParSecao_Ids.Length > 0)
            {
                whereSecao = $@" AND L2.Secao_Id in ({string.Join(",", form.ParSecao_Ids)}) ";
            }

            if (form.ParCargo_Ids.Length > 0)
            {
                whereCargo = $@" AND L2.Cargo_Id in ({string.Join(",", form.ParCargo_Ids)}) ";
            }

            if (form.ParCompany_Ids.Length > 0 && form.ParCompany_Ids[0] > 0)
            {
                whereUnit = $@"AND L2.UnitId in ({ string.Join(",", form.ParCompany_Ids) }) ";
            }

            if (form.ParClusterGroup_Ids.Length > 0)
            {
                whereClusterGroup = $@"AND PCG.Id in (" + string.Join(",", form.ParClusterGroup_Ids) + ")";
            }

            if (form.ParCluster_Ids.Length > 0)
            {
                whereCluster = $@"AND PC.Id in (" + string.Join(",", form.ParCluster_Ids) + ")";
            }

            if (form.ParStructure_Ids.Length > 0)
            {
                whereStructure = $@"AND L2.Regional in ({string.Join(",", form.ParStructure_Ids)})";
            }

            var query = $@"

                 DECLARE @DATAINICIAL DATETIME = '{ form.startDate.ToString("yyyy-MM-dd")} {" 00:00:00"}'
                 DECLARE @DATAFINAL   DATETIME = '{ form.endDate.ToString("yyyy-MM-dd") } {" 23:59:59"}'

                SELECT 
	                Regional.NAME AS RegionalName,
                    Regional.Id as Regional_Id,
	                SUM(WeiEvaluation) AS AV,
	                SUM(WeiDefects) AS NC,
	                SUM(WeiDefects)/SUM(WeiEvaluation)*100 AS [PROC]
	                FROM DW.Cubo_Coleta_L2 L2 WITH (NOLOCK)
	                INNER JOIN ParCompany C WITH (NOLOCK) ON L2.Unitid = C.ID
					LEFT JOIN ParVinculoPeso PVP ON L2.ParLevel2_Id = PVP.ParLevel2_Id
						AND L2.ParLevel1_Id = PVP.ParLevel1_Id
						AND L2.Cargo_Id = PVP.ParCargo_Id
						AND L2.ParFrequency_Id = PVP.ParFrequencyId
				    LEFT JOIN ParCluster PC ON PVP.ParCluster_Id = PC.Id
		            LEFT JOIN ParClusterGroup PCG ON PC.ParClusterGroup_Id = PCG.Id
					LEFT JOIN (select * from ParStructure where ParStructureGroup_Id = 1) Holding on L2.Holding = Holding.Id
					LEFT JOIN (select * from ParStructure where ParStructureGroup_Id = 2) GrupoDeEmpresa on L2.GrupoDeEmpresa = GrupoDeEmpresa.Id
					LEFT JOIN (select * from ParStructure where ParStructureGroup_Id = 3) Regional on L2.Regional = Regional.Id
	                WHERE 1=1
	                AND CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
                    {whereStructure}
                    {whereUnit}
                    {whereDepartment}
                    {whereSecao}
                    {whereCargo}
                    {whereCluster}
                    {whereClusterGroup}
				    AND Holding.Id = {form.Param["holding_Id"]}
					AND GrupoDeEmpresa.Id = {form.Param["grupoEmpresa_Id"]} 
                GROUP BY 
	                Regional.NAME, Regional.Id 
                ";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeRHResultsSet>(query).ToList();
            }

            return _list;
        }


        [HttpPost]
        [Route("GraficoUnidades")]
        public List<NaoConformidadeRHResultsSet> GraficoUnidades([FromBody] DTO.DataCarrierFormularioNew form)
        {

            //CommonLog.SaveReport(form, "Relatorio_Nao_Conformidade");

            var whereDepartment = "";
            var whereSecao = "";
            var whereCargo = "";
            var whereStructure = "";
            var whereUnit = "";
            //var whereShift = "";
            var whereCluster = "";
            //var whereCriticalLevel = "";
            var whereClusterGroup = "";

            if (form.ParDepartment_Ids.Length > 0)
            {
                whereDepartment = $@" AND L2.Centro_De_Custo_Id in ({string.Join(",", form.ParDepartment_Ids)}) ";
            }

            if (form.ParSecao_Ids.Length > 0)
            {
                whereSecao = $@" AND L2.Secao_Id in ({string.Join(",", form.ParSecao_Ids)}) ";
            }

            if (form.ParCargo_Ids.Length > 0)
            {
                whereCargo = $@" AND L2.Cargo_Id in ({string.Join(",", form.ParCargo_Ids)}) ";
            }


            //if (form.Shift_Ids.Length > 0)
            //{
            //    whereShift = "\n AND CL1.Shift in (" + string.Join(",", form.Shift_Ids) + ")";
            //}

            if (form.ParCompany_Ids.Length > 0 && form.ParCompany_Ids[0] > 0)
            {
                whereUnit = $@"AND L2.UnitId in ({ string.Join(",", form.ParCompany_Ids) }) ";
            }
            //else
            //{
            //    whereUnit = $@"AND UNI.Id IN (SELECT
            //    				ParCompany_Id
            //    			FROM ParCompanyXUserSgq
            //    			WHERE UserSgq_Id = { form.Param["auditorId"] })";
            //}

            if (form.ParClusterGroup_Ids.Length > 0)
            {
                whereClusterGroup = $@"AND PCG.Id in (" + string.Join(",", form.ParClusterGroup_Ids) + ")";
            }

            if (form.ParCluster_Ids.Length > 0)
            {
                whereCluster = $@"AND PC.Id in (" + string.Join(",", form.ParCluster_Ids) + ")";
            }

            if (form.ParStructure_Ids.Length > 0)
            {
                whereStructure = $@"AND L2.Regional in ({string.Join(",", form.ParStructure_Ids)})";
            }

            //if (form.ParCriticalLevel_Ids.Length > 0)
            //{
            //    whereCriticalLevel = $@" AND PLC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")"; 
            //        //" AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")) ";

            //}

            var query = $@"
                 DECLARE @DATAINICIAL DATETIME = '{ form.startDate.ToString("yyyy-MM-dd")} {" 00:00:00"}'
                 DECLARE @DATAFINAL   DATETIME = '{ form.endDate.ToString("yyyy-MM-dd") } {" 23:59:59"}'

                SELECT 
	                C.NAME AS UnidadeName,
                    C.Id as Unidade_Id,
	                SUM(WeiEvaluation) AS AV,
	                SUM(WeiDefects) AS NC,
	                SUM(WeiDefects)/SUM(WeiEvaluation)*100 AS [PROC]
	                FROM DW.Cubo_Coleta_L2 L2 WITH (NOLOCK)
	                INNER JOIN ParCompany C WITH (NOLOCK)
		                ON L2.Unitid = C.ID
					LEFT JOIN ParVinculoPeso PVP
		                ON L2.ParLevel2_Id = PVP.ParLevel2_Id
						AND L2.ParLevel1_Id = PVP.ParLevel1_Id
						AND L2.Cargo_Id = PVP.ParCargo_Id
						AND L2.ParFrequency_Id = PVP.ParFrequencyId
						--AND L2.Centro_De_Custo_Id = PVP.ParDepartment_Id
				    LEFT JOIN ParCluster PC
		                ON PVP.ParCluster_Id = PC.Id
		            LEFT JOIN ParClusterGroup PCG
		                ON PC.ParClusterGroup_Id = PCG.Id
	                LEFT JOIN (select * from ParStructure where ParStructureGroup_Id = 1) Holding on L2.Holding = Holding.Id
					LEFT JOIN (select * from ParStructure where ParStructureGroup_Id = 2) GrupoDeEmpresa on L2.GrupoDeEmpresa = GrupoDeEmpresa.Id
					LEFT JOIN (select * from ParStructure where ParStructureGroup_Id = 3) Regional on L2.Regional = Regional.Id
	                WHERE 1=1
	                AND CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL

                    {whereStructure}
                    {whereUnit}
                    {whereDepartment}
                    {whereSecao}
                    {whereCargo}
                    {whereCluster}
                    {whereClusterGroup}
                    AND Holding.Id = {form.Param["holding_Id"]}
					AND GrupoDeEmpresa.Id = {form.Param["grupoEmpresa_Id"]} 
					AND Regional.Id = {form.Param["regional_Id"]} 
                GROUP BY 
	                C.NAME, C.Id ";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeRHResultsSet>(query).ToList();
            }

            return _list;
        }

        [HttpPost]
        [Route("GraficoDepartamentos")]
        public List<NaoConformidadeRHResultsSet> GraficoDepartamentos([FromBody] DTO.DataCarrierFormularioNew form)
        {

            var whereDepartment = "";
            var whereSecao = "";
            var whereCargo = "";
            var whereStructure = "";
            var whereUnit = "";
            //var whereShift = "";
            var whereCluster = "";
            //var whereCriticalLevel = "";
            var whereClusterGroup = "";

            if (form.ParDepartment_Ids.Length > 0)
            {
                whereDepartment = $@" AND L2.Centro_De_Custo_Id in ({string.Join(",", form.ParDepartment_Ids)}) ";
            }

            if (form.ParSecao_Ids.Length > 0)
            {
                whereSecao = $@" AND L2.Secao_Id in ({string.Join(",", form.ParSecao_Ids)}) ";
            }

            if (form.ParCargo_Ids.Length > 0)
            {
                whereCargo = $@" AND L2.Cargo_Id in ({string.Join(",", form.ParCargo_Ids)}) ";
            }


            //if (form.Shift_Ids.Length > 0)
            //{
            //    whereShift = "\n AND CL1.Shift in (" + string.Join(",", form.Shift_Ids) + ")";
            //}

            if (form.ParCompany_Ids.Length > 0 && form.ParCompany_Ids[0] > 0)
            {
                whereUnit = $@"AND L2.UnitId in ({ string.Join(",", form.ParCompany_Ids) }) ";
            }
            //else
            //{
            //    whereUnit = $@"AND UNI.Id IN (SELECT
            //    				ParCompany_Id
            //    			FROM ParCompanyXUserSgq
            //    			WHERE UserSgq_Id = { form.Param["auditorId"] })";
            //}

            if (form.ParClusterGroup_Ids.Length > 0)
            {
                whereClusterGroup = $@"AND PCG.Id in (" + string.Join(",", form.ParClusterGroup_Ids) + ")";
            }

            if (form.ParCluster_Ids.Length > 0)
            {
                whereCluster = $@"AND PC.Id in (" + string.Join(",", form.ParCluster_Ids) + ")";
            }

            if (form.ParStructure_Ids.Length > 0)
            {
                whereStructure = $@"AND L2.Regional in ({string.Join(",", form.ParStructure_Ids)})";
            }

            //if (form.ParCriticalLevel_Ids.Length > 0)
            //{
            //    whereCriticalLevel = $@" AND PLC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")"; 
            //        //" AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")) ";

            //}

            var query = $@"
             
                         DECLARE @DATAINICIAL DATETIME = '" + form.startDate.ToString("yyyy-MM-dd") + " 00:00:00" + $@"'
                                                                                                                                                                                                                                            
                         DECLARE @DATAFINAL   DATETIME = '" + form.endDate.ToString("yyyy-MM-dd") + " 23:59:59" + $@"'
                                                                          
            SELECT 
	            D1.NAME AS DepartamentoName,
	            D1.Id AS Departamento_Id,
	            SUM(WeiEvaluation) AS AV,
	            SUM(WeiDefects) AS NC,
	            SUM(WeiDefects)/SUM(WeiEvaluation)*100 AS [PROC]
	            FROM DW.Cubo_Coleta_L2 L2 WITH (NOLOCK)
	            INNER JOIN ParCompany C WITH (NOLOCK)
		            ON L2.Unitid = C.ID
	            INNER JOIN ParLevel1 L WITH (NOLOCK)
		            ON L2.Parlevel1_Id = L.ID
	            INNER JOIN ParLevel2 M WITH (NOLOCK)
		            ON L2.Parlevel2_Id = M.ID
				INNER JOIN ParDepartment D
					ON L2.Centro_De_Custo_Id = D.ID
				INNER JOIN ParDepartment D1
					ON L2.Secao_Id = D1.ID
				INNER JOIN ParCargo CG
					ON L2.Cargo_Id = CG.ID
                LEFT JOIN ParVinculoPeso PVP
		                ON L2.ParLevel2_Id = PVP.ParLevel2_Id
						AND L2.ParLevel1_Id = PVP.ParLevel1_Id
						AND L2.Cargo_Id = PVP.ParCargo_Id
						AND L2.ParFrequency_Id = PVP.ParFrequencyId
						--AND L2.Centro_De_Custo_Id = PVP.ParDepartment_Id
				LEFT JOIN ParCluster PC
		                ON PVP.ParCluster_Id = PC.Id
		        LEFT JOIN ParClusterGroup PCG
		                ON PC.ParClusterGroup_Id = PCG.Id
                LEFT JOIN (select * from ParStructure where ParStructureGroup_Id = 1) Holding on L2.Holding = Holding.Id
				LEFT JOIN (select * from ParStructure where ParStructureGroup_Id = 2) GrupoDeEmpresa on L2.GrupoDeEmpresa = GrupoDeEmpresa.Id
				LEFT JOIN (select * from ParStructure where ParStructureGroup_Id = 3) Regional on L2.Regional = Regional.Id
	            WHERE 1=1
                AND L2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
                AND C.Name  = '{ form.Param["unitName"] }'
                
                {whereStructure}
                {whereUnit}
                {whereDepartment}
                {whereSecao}
                {whereCargo}
                {whereCluster}
                {whereClusterGroup}
                AND Holding.Id = {form.Param["holding_Id"]}
				AND GrupoDeEmpresa.Id = {form.Param["grupoEmpresa_Id"]} 
				AND Regional.Id = {form.Param["regional_Id"]} 
            GROUP BY 
	            D1.NAME, D1.Id
            ORDER BY 4 DESC 
";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeRHResultsSet>(query).ToList();
            }

            return _list;

        }

        [HttpPost]
        [Route("GraficoDepartamentosPorShift")]
        public List<NaoConformidadeRHResultsSet> GraficoDepartamentosPorShift([FromBody] DTO.DataCarrierFormularioNew form)
        {

            var whereDepartment = "";
            var whereShift = "";
            var whereCriticalLevel = "";

            if (form.ParDepartment_Ids.Length > 0)
            {
                whereDepartment = "\n AND L2.ParDepartment_Id in (" + string.Join(",", form.ParDepartment_Ids) + ")";
            }

            if (form.Param["departmentName"] != null && form.Param["departmentName"].ToString() != "")
            {
                whereDepartment = "\n AND D.Name = '" + form.Param["departmentName"] + "'";
            }

            if (form.Shift_Ids.Length > 0)
            {
                whereShift = "\n AND CL1.Shift in (" + string.Join(",", form.Shift_Ids) + ")";
            }

            if (form.ParCriticalLevel_Ids.Length > 0)
            {
                whereCriticalLevel = $@" AND PLC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")";
                //" AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")) ";

            }

            var query = @"
             
                         DECLARE @DATAINICIAL DATETIME = '" + form.startDate.ToString("yyyy-MM-dd") + @"'
                                                                                                                                                                                                                                            
                         DECLARE @DATAFINAL   DATETIME = '" + form.endDate.ToString("yyyy-MM-dd") + @"'
                                                                                                                                                                                                                                            
                         DECLARE @VOLUMEPCC int
                                                                          
                         DECLARE @ParCompany_id INT
            SELECT
            	@ParCompany_id = ID
            FROM PARCOMPANY
            WHERE NAME = '" + form.Param["unitName"] + @"'
                         CREATE TABLE #AMOSTRATIPO4 ( 
                         UNIDADE INT NULL, 
                         INDICADOR INT NULL, 
                         AM INT NULL, 
                         DEF_AM INT NULL 
                         )
            INSERT INTO #AMOSTRATIPO4
            	SELECT
            		UNIDADE
            	   ,INDICADOR
            	   ,COUNT(1) AM
            	   ,SUM(DEF_AM) DEF_AM
            	FROM (SELECT
            			CAST(C2.CollectionDate AS DATE) AS DATA
            		   ,C.Id AS UNIDADE
            		   ,C2.ParLevel1_Id AS INDICADOR
            		   ,C2.EvaluationNumber AS AV
            		   ,C2.Sample AS AM
            		   ,CASE
            				WHEN SUM(C2.WeiDefects) = 0 THEN 0
            				ELSE 1
            			END DEF_AM
            		FROM CollectionLevel2 C2 (NOLOCK)
            		INNER JOIN ParLevel1 L1 (NOLOCK)
            			ON L1.Id = C2.ParLevel1_Id
            		INNER JOIN ParCompany C (NOLOCK)
            			ON C.Id = C2.UnitId
            		WHERE CAST(C2.CollectionDate AS DATE) BETWEEN @DATAINICIAL AND @DATAFINAL
            		AND C2.NotEvaluatedIs = 0
            		AND C2.Duplicated = 0
            		AND L1.ParConsolidationType_Id = 4
            		GROUP BY C.Id
            				,ParLevel1_Id
            				,EvaluationNumber
            				,Sample
            				,CAST(CollectionDate AS DATE)) TAB
            	GROUP BY UNIDADE
            			,INDICADOR
            --------------------------------                                                                                                                     
            
            SELECT TOP 1
            	@VOLUMEPCC = SUM(Quartos)
            FROM VolumePcc1b(nolock)
            WHERE ParCompany_id = @ParCompany_id
            AND Data BETWEEN @DATAINICIAL AND @DATAFINAL
                                                                                                                                                                          
                          DECLARE @NAPCC INT
            
            SELECT
            	@NAPCC =
            	COUNT(1)
            FROM (SELECT
            		COUNT(1) AS NA
            	FROM CollectionLevel2 C2 (NOLOCK)
            	LEFT JOIN Result_Level3 C3 (NOLOCK)
            		ON C3.CollectionLevel2_Id = C2.Id
            	WHERE CONVERT(DATE, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL
            	AND C2.ParLevel1_Id = (SELECT TOP 1
            			id
            		FROM Parlevel1
            		WHERE Hashkey = 1)
            	AND C2.UnitId = @ParCompany_Id
            	AND IsNotEvaluate = 1
            	GROUP BY C2.ID) NA
            WHERE NA = 2
            --------------------------------  
            SELECT
               concat(DepartamentoName, ' - Shift ', Case when Shift = 1 then 'A' else 'B' END) as 'dataX'   
               ,UnidadeName
               ,Unidade_Id
               ,SUM([proc]) AS 'proc'
                  ,SUM(Meta) as Meta
               ,SUM(NC) AS NC
               ,SUM(Av) AS Av
               ,DepartamentoName
               ,Departamento_Id
               ,Shift
            FROM (SELECT
            		CONVERT(VARCHAR(153), Unidade) AS UnidadeName
            	   ,CONVERT(VARCHAR(153), Unidade_Id) AS Unidade_Id
            		--,CONVERT(VARCHAR(153), level1_Id) AS Indicador_Id
            		--,CONVERT(VARCHAR(153), Level1Name) AS IndicadorName
            	   ,ProcentagemNc AS [proc]
            	   ,(CASE
            			WHEN IsRuleConformity = 1 THEN (100 - META)
            			ELSE Meta
            		END) AS Meta
            	   ,NC
            	   ,Av
            	   ,DepartamentoName
            	   ,CONVERT(VARCHAR(153), Departamento_Id) AS Departamento_Id
            	--,IsRuleConformity
                  ,Shift
            	FROM (SELECT
            			Unidade
            		   ,IsRuleConformity
            		   ,Unidade_Id
            			--,Level1Name
            			--,level1_Id
            		   ,SUM(avSemPeso) AS av
            		   ,SUM(ncSemPeso) AS nc
            		   ,CASE
            				WHEN SUM(AV) IS NULL OR
            					SUM(AV) = 0 THEN 0
            				ELSE SUM(NC) / SUM(AV) * 100
            			END AS ProcentagemNc
            		   ,MAX(Meta) AS Meta
            		   ,DepartamentoName
            		   ,Departamento_Id
                       ,Shift
            		FROM (SELECT
            				IND.Id AS level1_Id
            			   ,IND.IsRuleConformity
            			   ,IND.Name AS Level1Name
            			   ,UNI.Id AS Unidade_Id
            			   ,UNI.Name AS Unidade
                           ,CL1.Shift AS Shift
            			   ,CASE
            					WHEN IND.HashKey = 1 THEN @VOLUMEPCC - @NAPCC
            					WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiEvaluation
            					WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation
            					WHEN IND.ParConsolidationType_Id = 3 THEN CL2.EvaluatedResult
            					WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
            					WHEN IND.ParConsolidationType_Id = 5 THEN CL2.WeiEvaluation
            					WHEN IND.ParConsolidationType_Id = 6 THEN CL2.WeiEvaluation
            					ELSE 0
            				END AS Av
            			   ,CASE
            					WHEN IND.HashKey = 1 THEN @VOLUMEPCC - @NAPCC
            					WHEN IND.ParConsolidationType_Id = 1 THEN CL2.EvaluateTotal
            					WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation
            					WHEN IND.ParConsolidationType_Id = 3 THEN CL2.EvaluatedResult
            					WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
            					WHEN IND.ParConsolidationType_Id = 5 THEN CL2.EvaluateTotal
            					WHEN IND.ParConsolidationType_Id = 6 THEN CL2.EvaluateTotal
            					ELSE 0
            				END AS AvSemPeso
            			   ,CASE
            					WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiDefects
            					WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiDefects
            					WHEN IND.ParConsolidationType_Id = 3 THEN CL2.DefectsResult
            					WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM
            					WHEN IND.ParConsolidationType_Id = 5 THEN CL2.WeiDefects
            					WHEN IND.ParConsolidationType_Id = 6 THEN CL2.TotalLevel3WithDefects
            					ELSE 0
            				END AS NC
            			   ,CASE
            					WHEN IND.ParConsolidationType_Id = 1 THEN CL2.DefectsTotal
            					WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiDefects
            					WHEN IND.ParConsolidationType_Id = 3 THEN CL2.DefectsResult
            					WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM
            					WHEN IND.ParConsolidationType_Id = 5 THEN CL2.DefectsTotal
            					WHEN IND.ParConsolidationType_Id = 6 THEN CL2.TotalLevel3WithDefects
            					ELSE 0
            				END AS NCSemPeso
            			   ,CASE
            
            					WHEN (SELECT
            								COUNT(1)
            							FROM ParGoal G
            							WHERE G.ParLevel1_id = CL1.ParLevel1_Id
            							AND (G.ParCompany_id = CL1.UnitId
            							OR G.ParCompany_id IS NULL)
            							AND G.EffectiveDate <= @DATAFINAL)
            						> 0 THEN (SELECT TOP 1
            								ISNULL(G.PercentValue, 0)
            							FROM ParGoal G
            							WHERE G.ParLevel1_id = CL1.ParLevel1_Id
            							AND (G.ParCompany_id = CL1.UnitId
            							OR G.ParCompany_id IS NULL)
            							AND G.EffectiveDate <= @DATAFINAL
            							ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)
            
            					ELSE (SELECT TOP 1
            								ISNULL(G.PercentValue, 0)
            							FROM ParGoal G
            							WHERE G.ParLevel1_id = CL1.ParLevel1_Id
            							AND (G.ParCompany_id = CL1.UnitId
            							OR G.ParCompany_id IS NULL)
                                        AND G.EffectiveDate <= @DATAFINAL
            							ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)
            				END
            				AS Meta
            			   ,D.Name AS DepartamentoName
            			   ,D.Id AS Departamento_Id
            			FROM ConsolidationLevel1 CL1 (NOLOCK)
            			INNER JOIN ParLevel1 IND (NOLOCK)
            				ON IND.Id = CL1.ParLevel1_Id
                            AND IND.ID != 43
            			INNER JOIN ConsolidationLevel2 CL2 WITH (NOLOCK)
            				ON CL2.ConsolidationLevel1_id = CL1.Id
            			INNER JOIN ParLevel2 L2 WITH (NOLOCK)
            				ON CL2.ParLevel2_id = L2.Id
            			INNER JOIN ParDepartment D WITH (NOLOCK)
            				ON L2.ParDepartment_Id = D.Id
            			INNER JOIN ParCompany UNI (NOLOCK)
            				ON UNI.Id = CL1.UnitId
            			LEFT JOIN #AMOSTRATIPO4 A4 (NOLOCK)
            				ON A4.UNIDADE = UNI.Id
            				AND A4.INDICADOR = IND.ID
                        LEFT JOIN ParLevel1XCluster PLC
							ON PLC.ParLevel1_Id = CL1.ParLevel1_Id 
            			WHERE CL1.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
            			AND UNI.Name = '" + form.Param["unitName"] + @"'
                        " + whereDepartment + @"
                        " + whereShift + @"
                        " + whereCriticalLevel + @"
            		--AND D.Id = 2
            		) S1
            		GROUP BY Unidade
            				,Unidade_Id
            				 --,Level1Name
            				 --,level1_Id
            				,IsRuleConformity
            				,DepartamentoName
            				,Departamento_Id
                            ,Shift) S2
            	WHERE nc > 0) A
            GROUP BY UnidadeName
            		,Unidade_Id
            		,DepartamentoName
            		,Departamento_Id
                    ,Shift
            ORDER BY 5 DESC
            DROP TABLE #AMOSTRATIPO4 ";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeRHResultsSet>(query).ToList();
            }
            return _list;

        }

        [HttpPost]
        [Route("GraficoIndicadorDepartamento")]
        public List<NaoConformidadeRHResultsSet> GraficoIndicadorDepartamento([FromBody] DTO.DataCarrierFormularioNew form)
        {
            //_list = CriaMockGraficoNcPorUnidadeIndicador();

            //    public string Indicador_Id { get; set; }
            //public string IndicadorName { get; set; }
            //public string Unidade_Id { get; set; }
            //public string UnidadeName { get; set; }
            //public string Monitoramento_Id { get; set; }
            //public string MonitoramentoName { get; set; }
            //public string Tarefa_Id { get; set; }
            //public string TarefaName { get; set; }
            //public decimal Nc { get; set; }
            //public decimal Av { get; set; }
            //public decimal Meta { get; set; }
            //public decimal Proc { get; internal set; }

            var query = "" +

                "\n DECLARE @DATAINICIAL DATETIME = '" + form.startDate.ToString("yyyy-MM-dd") + "'                                                                                                                                                                                                                    " +
                "\n DECLARE @DATAFINAL   DATETIME = '" + form.endDate.ToString("yyyy-MM-dd") + "'                                                                                                                                                                                                                    " +

                "\n DECLARE @VOLUMEPCC int                                                  " +
                "\n DECLARE @ParCompany_id INT                                              " +

                "\n SELECT @ParCompany_id = ID FROM PARCOMPANY WHERE NAME = '" + form.Param["unitName"] + "'" +

                "\n CREATE TABLE #AMOSTRATIPO4 ( " +

                "\n UNIDADE INT NULL, " +
                "\n INDICADOR INT NULL, " +
                "\n DATA DATETIME, " +
                "\n AM INT NULL, " +
                "\n DEF_AM INT NULL " +
                "\n ) " +

                "\n INSERT INTO #AMOSTRATIPO4 " +

                "\n SELECT " +
                "\n  UNIDADE, INDICADOR, DATA, " +
                "\n COUNT(1) AM " +
                "\n ,SUM(DEF_AM) DEF_AM " +
                "\n FROM " +
                "\n ( " +
                "\n     SELECT " +
                "\n     cast(C2.CollectionDate as DATE) AS DATA " +
                "\n     , C.Id AS UNIDADE " +
                "\n     , C2.ParLevel1_Id AS INDICADOR " +
                "\n     , C2.EvaluationNumber AS AV " +
                "\n     , C2.Sample AS AM " +
                "\n     , case when SUM(C2.WeiDefects) = 0 then 0 else 1 end DEF_AM " +
                "\n     FROM CollectionLevel2 C2  (nolock)" +
                "\n     INNER JOIN ParLevel1 L1  (nolock)" +
                "\n     ON L1.Id = C2.ParLevel1_Id " +

                "\n     INNER JOIN ParCompany C  (nolock)" +
                "\n     ON C.Id = C2.UnitId " +
                "\n     where cast(C2.CollectionDate as DATE) BETWEEN @DATAINICIAL AND @DATAFINAL " +
                "\n     and C2.NotEvaluatedIs = 0 " +
                "\n     and C2.Duplicated = 0 " +
                "\n     and L1.ParConsolidationType_Id = 4 " +
                "\n     group by C.Id, ParLevel1_Id, EvaluationNumber, Sample, cast(CollectionDate as DATE) " +
                "\n ) TAB " +
                "\n GROUP BY UNIDADE, INDICADOR, DATA " +

                "\n --------------------------------                                                                                                                     " +
                "\n                                                                                                                                                      " +
                "\n  SELECT TOP 1 @VOLUMEPCC = SUM(Quartos) FROM VolumePcc1b  (nolock) WHERE ParCompany_id = @ParCompany_id AND Data BETWEEN @DATAINICIAL AND @DATAFINAL " +
                "\n                                                                                                                                                      " +
                "\n                                                                                                                                                      " +
                "\n  DECLARE @NAPCC INT                                                                                                                                  " +
                "\n                                                                                                                                                      " +
                "\n                                                                                                                                                      " +
                "\n  SELECT                                                                                                                                              " +
                "\n         @NAPCC =                                                                                                                                     " +
                "\n           COUNT(1)                                                                                                                                   " +
                "\n           FROM                                                                                                                                       " +
                "\n      (                                                                                                                                               " +
                "\n               SELECT                                                                                                                                 " +
                "\n               COUNT(1) AS NA                                                                                                                         " +
                "\n               FROM CollectionLevel2 C2(nolock)                                                                                                       " +
                "\n               LEFT JOIN Result_Level3 C3(nolock)                                                                                                     " +
                "\n               ON C3.CollectionLevel2_Id = C2.Id                                                                                                      " +
                "\n               WHERE convert(date, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL                                                             " +
                "\n               AND C2.ParLevel1_Id = (SELECT top 1 id FROM Parlevel1 where Hashkey = 1)                                                               " +
                "\n               AND C2.UnitId = @ParCompany_Id                                                                                                         " +
                "\n               AND IsNotEvaluate = 1                                                                                                                  " +
                "\n               GROUP BY C2.ID                                                                                                                         " +
                "\n           ) NA                                                                                                                                       " +
                "\n           WHERE NA = 2                                                                                                                               " +
                "\n  --------------------------------                                                                                                                    " +



                "\n SELECT " +

                "\n  CONVERT(varchar(153), Unidade) as UnidadeName" +
                "\n ,CONVERT(varchar(153), Unidade_Id) as Unidade_Id" +
                "\n ,CONVERT(varchar(153), level1_Id) as Indicador_Id" +
                "\n ,CONVERT(varchar(153), Level1Name) as IndicadorName" +


                "\n ,ProcentagemNc as [proc] " +

               "\n ,IIF(IsRuleConformity = 1, (100 - META), Meta) AS Meta  " +
               "\n ,NC " +
               "\n ,Av " +
                "\n FROM " +
                "\n ( " +
                "\n     SELECT " +
                "\n       Unidade  " +
                "\n     , IsRuleConformity " +
                "\n     , Unidade_Id " +
                "\n     , Level1Name " +
                "\n     , level1_Id " +

                "\n     , sum(avSemPeso) as av " +
                "\n     , sum(ncSemPeso) as nc " +
                "\n     , IIF(sum(AV) IS NULL OR sum(AV) = 0, 0, sum(NC) / sum(AV) * 100) AS ProcentagemNc " +
                "\n     , max(Meta) as Meta" +

                "\n     FROM " +
                "\n     ( " +
                "\n         SELECT " +
                "\n          IND.Id         AS level1_Id " +
                "\n         , IND.IsRuleConformity " +
                "\n         , IND.Name       AS Level1Name " +
                "\n         , UNI.Id         AS Unidade_Id " +
                "\n         , UNI.Name       AS Unidade " +
                "\n         , CASE " +
                "\n         WHEN IND.HashKey = 1 THEN @VOLUMEPCC - @NAPCC " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 3 THEN CL2.EvaluatedResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM" +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN CL2.WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN CL2.WeiEvaluation " +


                "\n         ELSE 0 " +
                "\n        END AS Av " +

                "\n       , CASE " +
                "\n         WHEN IND.HashKey = 1 THEN @VOLUMEPCC - @NAPCC " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN CL2.EvaluateTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 3 THEN CL2.EvaluatedResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM" +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN CL2.EvaluateTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN CL2.EvaluateTotal " +
                "\n         ELSE 0 " +
                "\n        END AS AvSemPeso " +

                "\n         , CASE " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id = 3 THEN CL2.DefectsResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM" +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN CL2.WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN CL2.TotalLevel3WithDefects " +
                "\n         ELSE 0 " +

                "\n         END AS NC " +

                "\n         , CASE " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN CL2.DefectsTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id = 3 THEN CL2.DefectsResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM" +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN CL2.DefectsTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN CL2.TotalLevel3WithDefects " +
                "\n         ELSE 0 " +

                "\n         END AS NCSemPeso " +
               "\n  ,                                                                                                                                                                                                                                                                  " +
               "\n  CASE                                                                                                                                                                                                                                                               " +
               "\n                                                                                                                                                                                                                                                                     " +
               "\n     WHEN(SELECT COUNT(1) FROM ParGoal G WHERE G.ParLevel1_id = CL1.ParLevel1_Id AND(G.ParCompany_id = CL1.UnitId OR G.ParCompany_id IS NULL) AND G.EffectiveDate <= @DATAFINAL) > 0 THEN                                                                                                   " +
               "\n         (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G WHERE G.ParLevel1_id = CL1.ParLevel1_Id AND(G.ParCompany_id = CL1.UnitId OR G.ParCompany_id IS NULL) AND G.EffectiveDate <= @DATAFINAL ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)                                         " +
               "\n                                                                                                                                                                                                                                                                     " +
               "\n     ELSE                                                                                                                                                                                                                                                            " +
               "\n         (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G WHERE G.ParLevel1_id = CL1.ParLevel1_Id AND(G.ParCompany_id = CL1.UnitId OR G.ParCompany_id IS NULL) AND G.EffectiveDate <= @DATAFINAL ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)                                                                      " +
               "\n  END                                                                                                                                                                                                                                                                " +
               "\n  AS Meta                                                                                                                                                                                                                                                            " +
                "\n         FROM ConsolidationLevel1 CL1  (nolock)" +
                "\n         INNER JOIN ParLevel1 IND  (nolock)" +
                "\n            ON IND.Id = CL1.ParLevel1_Id  " +
                "\n            AND ISNULL(IND.ShowScorecard,1) = 1 " +
                "\n            AND IND.IsActive = 1 " +
                "\n            AND IND.ID != 43 " +
                "\n         INNER JOIN ConsolidationLevel2 CL2 with (nolock) " +
                "\n         ON CL2.ConsolidationLevel1_id = CL1.Id " +
                "\n         INNER JOIN ParLevel2 L2 with (nolock) " +
                "\n         ON CL2.ParLevel2_id = L2.Id " +
                "\n         INNER JOIN ParDepartment D with (nolock) " +
                "\n         ON L2.ParDepartment_Id = D.Id " +


                "\n         INNER JOIN ParCompany UNI  (nolock)" +
                "\n         ON UNI.Id = CL1.UnitId " +
                "\n         LEFT JOIN #AMOSTRATIPO4 A4  (nolock)" +
                "\n         ON A4.UNIDADE = UNI.Id " +
                "\n         AND A4.INDICADOR = IND.ID " +
                "\n         AND A4.DATA = CL1.ConsolidationDate " +

                "\n         WHERE CL1.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL " +
                "\n         AND UNI.Name = '" + form.Param["unitName"] + "'" +
                "\n         -- AND (TotalLevel3WithDefects > 0 AND TotalLevel3WithDefects IS NOT NULL) " +

                "\n         AND D.Id = 2 " +

                "\n     ) S1 " +

                "\n     GROUP BY Unidade, Unidade_Id, Level1Name, level1_Id, IsRuleConformity  " +

                "\n ) S2 " +
                "\n WHERE ProcentagemNc <> 0  " +
                "\n ORDER BY 5 DESC" +

                "\n  DROP TABLE #AMOSTRATIPO4 ";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeRHResultsSet>(query).ToList();
            }

            return _list;

        }

        [HttpPost]
        [Route("GraficoIndicador")]
        public List<NaoConformidadeRHResultsSet> GraficoIndicador([FromBody] DTO.DataCarrierFormularioNew form)
        {
            var whereDepartment = "";
            var whereDepartmentFiltro = "";
            var whereSecao = "";
            var whereCargo = "";
            var whereStructure = "";
            var whereUnit = "";
            //var whereShift = "";
            var whereCluster = "";
            //var whereCriticalLevel = "";
            var whereClusterGroup = "";

            // Filtro = Gráfico Anterior

            if (form.Param["departmentName"] != null && form.Param["departmentName"].ToString() != "")
            {
                whereDepartmentFiltro = $@"AND D.Name = '{form.Param["departmentName"]}'";
            }

            // Filtro = Pré-seleção
            if (form.ParDepartment_Ids.Length > 0)
            {
                whereDepartment = $@" AND L2.Centro_De_Custo_Id in ({string.Join(",", form.ParDepartment_Ids)}) ";
            }

            if (form.ParSecao_Ids.Length > 0)
            {
                whereSecao = $@" AND L2.Secao_Id in ({string.Join(",", form.ParSecao_Ids)}) ";
            }

            if (form.ParCargo_Ids.Length > 0)
            {
                whereCargo = $@" AND L2.Cargo_Id in ({string.Join(",", form.ParCargo_Ids)}) ";
            }


            //if (form.Shift_Ids.Length > 0)
            //{
            //    whereShift = "\n AND CL1.Shift in (" + string.Join(",", form.Shift_Ids) + ")";
            //}

            if (form.ParCompany_Ids.Length > 0 && form.ParCompany_Ids[0] > 0)
            {
                whereUnit = $@"AND L2.UnitId in ({ string.Join(",", form.ParCompany_Ids) }) ";
            }
            //else
            //{
            //    whereUnit = $@"AND UNI.Id IN (SELECT
            //    				ParCompany_Id
            //    			FROM ParCompanyXUserSgq
            //    			WHERE UserSgq_Id = { form.Param["auditorId"] })";
            //}

            if (form.ParClusterGroup_Ids.Length > 0)
            {
                whereClusterGroup = $@"AND PCG.Id in (" + string.Join(",", form.ParClusterGroup_Ids) + ")";
            }

            if (form.ParCluster_Ids.Length > 0)
            {
                whereCluster = $@"AND PC.Id in (" + string.Join(",", form.ParCluster_Ids) + ")";
            }

            if (form.ParStructure_Ids.Length > 0)
            {
                whereStructure = $@"AND L2.Regional in ({string.Join(",", form.ParStructure_Ids)})";
            }

            //if (form.ParCriticalLevel_Ids.Length > 0)
            //{
            //    whereCriticalLevel = $@" AND PLC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")"; 
            //        //" AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")) ";

            //}

            var query = $@"
             DECLARE @DATAINICIAL DATETIME = '{ form.startDate.ToString("yyyy-MM-dd") } 00:00:00'
                                                                                                                                                                                                                    
             DECLARE @DATAFINAL   DATETIME = '{ form.endDate.ToString("yyyy-MM-dd") } 23:59:59'
        


            SELECT 
	            L.NAME AS IndicadorName,
	            L.Id AS Indicador_Id,
	            SUM(WeiEvaluation) AS AV,
	            SUM(WeiDefects) AS NC,
	            SUM(WeiDefects)/SUM(WeiEvaluation)*100 AS [PROC]
	            FROM DW.Cubo_Coleta_L2 L2 WITH (NOLOCK)
	            INNER JOIN ParCompany C WITH (NOLOCK)
		            ON L2.Unitid = C.ID
	            INNER JOIN ParLevel1 L WITH (NOLOCK)
		            ON L2.Parlevel1_Id = L.ID
	            INNER JOIN ParDepartment D WITH (NOLOCK)
		            ON L2.Secao_Id = D.ID
                LEFT JOIN ParVinculoPeso PVP
		                ON L2.ParLevel2_Id = PVP.ParLevel2_Id
						AND L2.ParLevel1_Id = PVP.ParLevel1_Id
						AND L2.Cargo_Id = PVP.ParCargo_Id
						AND L2.ParFrequency_Id = PVP.ParFrequencyId
						--AND L2.Centro_De_Custo_Id = PVP.ParDepartment_Id
				LEFT JOIN ParCluster PC
		                ON PVP.ParCluster_Id = PC.Id
		        LEFT JOIN ParClusterGroup PCG
		                ON PC.ParClusterGroup_Id = PCG.Id
                LEFT JOIN (select * from ParStructure where ParStructureGroup_Id = 1) Holding on L2.Holding = Holding.Id
				LEFT JOIN (select * from ParStructure where ParStructureGroup_Id = 2) GrupoDeEmpresa on L2.GrupoDeEmpresa = GrupoDeEmpresa.Id
				LEFT JOIN (select * from ParStructure where ParStructureGroup_Id = 3) Regional on L2.Regional = Regional.Id
	            WHERE 1=1
	            AND CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
		            AND C.Name = '{form.Param["unitName"] }'
                    

                {whereStructure}
                {whereUnit}
                {whereDepartment}
                {whereDepartmentFiltro}
                {whereSecao}
                {whereCargo}
                {whereCluster}
                {whereClusterGroup}
                AND Holding.Id = {form.Param["holding_Id"]}
				AND GrupoDeEmpresa.Id = {form.Param["grupoEmpresa_Id"]} 
				AND Regional.Id = {form.Param["regional_Id"]} 
            GROUP BY 
	            L.NAME, L.Id
            ORDER BY 4 DESC
 ";


            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeRHResultsSet>(query).ToList();
            }

            return _list;

        }

        [HttpPost]
        [Route("GraficoIndicadorPorShift")]
        public List<NaoConformidadeRHResultsSet> GraficoIndicadorPorShift([FromBody] DTO.DataCarrierFormularioNew form)
        {
            var whereDepartment = "";
            var whereShift = "";
            var whereCriticalLevel = "";
            var whereClusterGroup = "";
            var whereCluster = "";

            if (form.ParDepartment_Ids.Length > 0)
            {
                whereDepartment = $@"AND L2.ParDepartment_Id  in (" + string.Join(",", form.ParDepartment_Ids) + ") ";
            }

            if (form.Param["departmentName"] != null && form.Param["departmentName"].ToString() != "")
            {
                whereDepartment = $@"AND D.Name = '{ form.Param["departmentName"] }'";
            }

            if (form.Shift_Ids.Length > 0)
            {
                whereShift = $@"AND CL1.Shift  in (" + string.Join(",", form.Shift_Ids) + ") ";
            }

            if (form.ParCriticalLevel_Ids.Length > 0)
            {
                whereCriticalLevel = $@" AND PLC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")";
                //" AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")) ";

            }

            if (form.ParClusterGroup_Ids.Length > 0)
            {
                whereClusterGroup = $@"AND PCG.Id in (" + string.Join(",", form.ParClusterGroup_Ids) + ")";
            }

            if (form.ParCluster_Ids.Length > 0)
            {
                whereCluster = $@"AND UNI.Id IN(SELECT PCC.ParCompany_Id FROM ParCompanyCluster PCC WHERE pcc.ParCluster_Id in (" + string.Join(",", form.ParCluster_Ids) + ") AND PCC.Active = 1)";
            }

            var query = $@"
 DECLARE @DATAINICIAL DATETIME = '{ form.startDate.ToString("yyyy-MM-dd") }'
                                                                                                                                                                                                                    
 DECLARE @DATAFINAL   DATETIME = '{ form.endDate.ToString("yyyy-MM-dd") }'
                                                                                                                                                                                                                    
 DECLARE @VOLUMEPCC int
                                                  
 DECLARE @ParCompany_id INT
SELECT
	@ParCompany_id = ID
FROM PARCOMPANY
WHERE NAME = '{ form.Param["unitName"] }'
 CREATE TABLE #AMOSTRATIPO4 ( 
 UNIDADE INT NULL, 
 INDICADOR INT NULL, 
 AM INT NULL, 
 DEF_AM INT NULL 
 )
INSERT INTO #AMOSTRATIPO4
	SELECT
		UNIDADE
	   ,INDICADOR
       ,DATA
	   ,COUNT(1) AM
	   ,SUM(DEF_AM) DEF_AM
	FROM (SELECT
			CAST(C2.CollectionDate AS DATE) AS DATA
		   ,C.Id AS UNIDADE
		   ,C2.ParLevel1_Id AS INDICADOR
		   ,C2.EvaluationNumber AS AV
		   ,C2.Sample AS AM
		   ,CASE
				WHEN SUM(C2.WeiDefects) = 0 THEN 0
				ELSE 1
			END DEF_AM
		FROM CollectionLevel2 C2 (NOLOCK)
		INNER JOIN ParLevel1 L1 (NOLOCK)
			ON L1.Id = C2.ParLevel1_Id
		INNER JOIN ParCompany C (NOLOCK)
			ON C.Id = C2.UnitId
		WHERE CAST(C2.CollectionDate AS DATE) BETWEEN @DATAINICIAL AND @DATAFINAL
		AND C2.NotEvaluatedIs = 0
		AND C2.Duplicated = 0
		AND L1.ParConsolidationType_Id = 4
		GROUP BY C.Id
				,ParLevel1_Id
				,EvaluationNumber
				,Sample
				,CAST(CollectionDate AS DATE)) TAB
	GROUP BY UNIDADE
			,INDICADOR
            ,DATA
--------------------------------                                                                                                                     

SELECT TOP 1
	@VOLUMEPCC = SUM(Quartos)
FROM VolumePcc1b(nolock)
WHERE ParCompany_id = @ParCompany_id
AND Data BETWEEN @DATAINICIAL AND @DATAFINAL
 
                                                                                                                                                      
                                                                                                                                                      
  DECLARE @NAPCC INT


SELECT
	@NAPCC =
	COUNT(1)
FROM (SELECT
		COUNT(1) AS NA
	FROM CollectionLevel2 C2 (NOLOCK)
	LEFT JOIN Result_Level3 C3 (NOLOCK)
		ON C3.CollectionLevel2_Id = C2.Id
	WHERE CONVERT(DATE, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL
	AND C2.ParLevel1_Id = (SELECT TOP 1
			id
		FROM Parlevel1
		WHERE Hashkey = 1)
	AND C2.UnitId = @ParCompany_Id
	AND IsNotEvaluate = 1
	GROUP BY C2.ID) NA
WHERE NA = 2
--------------------------------                                                                                                                    
SELECT
	CONVERT(VARCHAR(153), Unidade) AS UnidadeName
   ,CONVERT(VARCHAR(153), Unidade_Id) AS Unidade_Id
   ,CONVERT(VARCHAR(153), level1_Id) AS Indicador_Id
   ,CONVERT(VARCHAR(153), Level1Name) AS IndicadorName
   ,ProcentagemNc AS [proc]
   ,(CASE
		WHEN IsRuleConformity = 1 THEN (100 - META)
		ELSE Meta
	END) AS Meta
   ,NC
   ,Av
   ,Shift
   ,CONCAT(Level1Name, ' - Shift ', Case when S2.Shift = 1 then 'A' else 'B' END) as 'dataX'
FROM (SELECT
		Unidade
	   ,IsRuleConformity
	   ,Unidade_Id
	   ,Level1Name
	   ,level1_Id
	   ,SUM(avSemPeso) AS av
	   ,SUM(ncSemPeso) AS nc
	   ,CASE
			WHEN SUM(AV) IS NULL OR
				SUM(AV) = 0 THEN 0
			ELSE SUM(NC) / SUM(AV) * 100
		END AS ProcentagemNc
	   ,MAX(Meta) AS Meta
	   ,Shift
	FROM (SELECT
			IND.Id AS level1_Id
		   ,IND.IsRuleConformity
		   ,IND.Name AS Level1Name
		   ,UNI.Id AS Unidade_Id
		   ,UNI.Name AS Unidade
		   ,CL1.Shift AS Shift
		   ,CASE
				WHEN IND.HashKey = 1 THEN @VOLUMEPCC - @NAPCC
				WHEN IND.ParConsolidationType_Id = 1 THEN SUM(CL2.WeiEvaluation)
				WHEN IND.ParConsolidationType_Id = 2 THEN SUM(CL2.WeiEvaluation)
				WHEN IND.ParConsolidationType_Id = 3 THEN SUM(CL2.EvaluatedResult)
				WHEN IND.ParConsolidationType_Id = 4 THEN SUM(A4.AM)
				WHEN IND.ParConsolidationType_Id = 5 THEN SUM(CL2.WeiEvaluation)
				WHEN IND.ParConsolidationType_Id = 6 THEN SUM(CL2.WeiEvaluation)
				ELSE 0
			END AS Av
		   ,CASE
				WHEN IND.HashKey = 1 THEN @VOLUMEPCC - @NAPCC
				WHEN IND.ParConsolidationType_Id = 1 THEN SUM(CL2.EvaluateTotal)
				WHEN IND.ParConsolidationType_Id = 2 THEN SUM(CL2.WeiEvaluation)
				WHEN IND.ParConsolidationType_Id = 3 THEN SUM(CL2.EvaluatedResult)
				WHEN IND.ParConsolidationType_Id = 4 THEN SUM(A4.AM)
				WHEN IND.ParConsolidationType_Id = 5 THEN SUM(CL2.EvaluateTotal)
				WHEN IND.ParConsolidationType_Id = 6 THEN SUM(CL2.EvaluateTotal)
				ELSE 0
			END AS AvSemPeso
		   ,CASE
				WHEN IND.ParConsolidationType_Id = 1 THEN SUM(CL2.WeiDefects)
				WHEN IND.ParConsolidationType_Id = 2 THEN SUM(CL2.WeiDefects)
				WHEN IND.ParConsolidationType_Id = 3 THEN SUM(CL2.DefectsResult)
				WHEN IND.ParConsolidationType_Id = 4 THEN SUM(A4.DEF_AM)
				WHEN IND.ParConsolidationType_Id = 5 THEN SUM(CL2.WeiDefects)
				WHEN IND.ParConsolidationType_Id = 6 THEN SUM(CL2.TotalLevel3WithDefects)
				ELSE 0
			END AS NC
		   ,CASE
				WHEN IND.ParConsolidationType_Id = 1 THEN SUM(CL2.DefectsTotal)
				WHEN IND.ParConsolidationType_Id = 2 THEN SUM(CL2.WeiDefects)
				WHEN IND.ParConsolidationType_Id = 3 THEN SUM(CL2.DefectsResult)
				WHEN IND.ParConsolidationType_Id = 4 THEN SUM(A4.DEF_AM)
				WHEN IND.ParConsolidationType_Id = 5 THEN SUM(CL2.DefectsTotal)
				WHEN IND.ParConsolidationType_Id = 6 THEN SUM(CL2.TotalLevel3WithDefects)
				ELSE 0
			END AS NCSemPeso
		   ,CASE

				WHEN (SELECT
							COUNT(1)
						FROM ParGoal G
						WHERE G.ParLevel1_id = CL1.ParLevel1_Id
						AND (G.ParCompany_id = CL1.UnitId
						OR G.ParCompany_id IS NULL)
						AND G.EffectiveDate <= @DATAFINAL)
					> 0 THEN (SELECT TOP 1
							ISNULL(G.PercentValue, 0)
						FROM ParGoal G
						WHERE G.ParLevel1_id = CL1.ParLevel1_Id
						AND (G.ParCompany_id = CL1.UnitId
						OR G.ParCompany_id IS NULL)
						AND G.EffectiveDate <= @DATAFINAL
						ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)

				ELSE (SELECT TOP 1
							ISNULL(G.PercentValue, 0)
						FROM ParGoal G
						WHERE G.ParLevel1_id = CL1.ParLevel1_Id
						AND (G.ParCompany_id = CL1.UnitId
						OR G.ParCompany_id IS NULL)
                        AND G.EffectiveDate <= @DATAFINAL
						ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)
			END
			AS Meta
		FROM ConsolidationLevel1 CL1 (NOLOCK)
		INNER JOIN ParLevel1 IND (NOLOCK)
			ON IND.Id = CL1.ParLevel1_Id
		INNER JOIN ParCompany UNI (NOLOCK)
			ON UNI.Id = CL1.UnitId
		LEFT JOIN #AMOSTRATIPO4 A4 (NOLOCK)
			ON A4.UNIDADE = UNI.Id
			AND A4.INDICADOR = IND.ID
			AND A4.DATA = CL1.ConsolidationDate
		INNER JOIN ConsolidationLevel2 CL2 WITH (NOLOCK)
			ON CL2.ConsolidationLevel1_id = CL1.Id
		INNER JOIN ParLevel2 L2 WITH (NOLOCK)
			ON CL2.ParLevel2_id = L2.Id
        LEFT JOIN ParLevel1XCluster PLC
							ON PLC.ParLevel1_Id = CL1.ParLevel1_Id 
		INNER JOIN ParDepartment D WITH (NOLOCK)
			ON L2.ParDepartment_Id = D.Id
        INNER JOIN ParCompany UNI (NOLOCK)
            ON UNI.Id = CL1.UnitId
            and UNI.IsActive = 1
        LEFT JOIN ParCompanyCluster PCC
		    ON PCC.ParCompany_Id = UNI.Id
            and PCC.Active = 1
        LEFT JOIN ParLevel1XCluster PLC
			ON PLC.ParLevel1_Id = CL1.ParLevel1_Id 
        LEFT JOIN ParCluster PC
		    ON PCC.ParCluster_Id = PC.Id
		LEFT JOIN ParClusterGroup PCG
		    ON PC.ParClusterGroup_Id = PCG.Id
		WHERE CL1.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
		AND UNI.Name = '{form.Param["unitName"] }'
        {whereDepartment}
        {whereShift}
        {whereCriticalLevel}
        -- AND (TotalLevel3WithDefects > 0 AND TotalLevel3WithDefects IS NOT NULL) 
		GROUP BY IND.ParConsolidationType_Id
				,IND.HashKey
				,IND.Id
				,IND.IsRuleConformity
				,IND.Name
				,UNI.Id
				,UNI.Name
				,CL1.ParLevel1_Id
				,CL1.UnitId
                ,cl1.Shift) S1
	GROUP BY Unidade
			,Unidade_Id
			,Level1Name
			,level1_Id
			,IsRuleConformity
            ,Shift) S2
WHERE nc > 0
ORDER BY 5 DESC
DROP TABLE #AMOSTRATIPO4 ";



            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeRHResultsSet>(query).ToList();
            }

            return _list;

        }

        [HttpPost]
        [Route("GraficoMonitoramento")]
        public List<NaoConformidadeRHResultsSet> GraficoMonitoramento([FromBody] DTO.DataCarrierFormularioNew form)
        {
            //_list = CriaMockGraficoMonitoramento();

            //var query = new NaoConformidadeRHResultsSet().Select(form._dataInicio, form._dataFim, form.unitId);



            var whereDepartment = "";
            var whereDepartmentFiltro = "";
            var whereSecao = "";
            var whereCargo = "";
            var whereStructure = "";
            var whereUnit = "";
            //var whereShift = "";
            var whereCluster = "";
            //var whereCriticalLevel = "";
            var whereClusterGroup = "";

            if (form.Param["departmentName"] != null && form.Param["departmentName"].ToString() != "")
            {
                whereDepartmentFiltro = $@"AND D.Name = '{form.Param["departmentName"]}'";
            }

            if (form.ParDepartment_Ids.Length > 0)
            {
                whereDepartment = $@" AND L2.Centro_De_Custo_Id in ({string.Join(",", form.ParDepartment_Ids)}) ";
            }

            if (form.ParSecao_Ids.Length > 0)
            {
                whereSecao = $@" AND L2.Secao_Id in ({string.Join(",", form.ParSecao_Ids)}) ";
            }

            if (form.ParCargo_Ids.Length > 0)
            {
                whereCargo = $@" AND L2.Cargo_Id in ({string.Join(",", form.ParCargo_Ids)}) ";
            }


            //if (form.Shift_Ids.Length > 0)
            //{
            //    whereShift = "\n AND CL1.Shift in (" + string.Join(",", form.Shift_Ids) + ")";
            //}

            if (form.ParCompany_Ids.Length > 0 && form.ParCompany_Ids[0] > 0)
            {
                whereUnit = $@"AND L2.UnitId in ({ string.Join(",", form.ParCompany_Ids) }) ";
            }
            //else
            //{
            //    whereUnit = $@"AND UNI.Id IN (SELECT
            //    				ParCompany_Id
            //    			FROM ParCompanyXUserSgq
            //    			WHERE UserSgq_Id = { form.Param["auditorId"] })";
            //}

            if (form.ParClusterGroup_Ids.Length > 0)
            {
                whereClusterGroup = $@"AND PCG.Id in (" + string.Join(",", form.ParClusterGroup_Ids) + ")";
            }

            if (form.ParCluster_Ids.Length > 0)
            {
                whereCluster = $@"AND PC.Id in (" + string.Join(",", form.ParCluster_Ids) + ")";
            }

            if (form.ParStructure_Ids.Length > 0)
            {
                whereStructure = $@"AND L2.Regional in ({string.Join(",", form.ParStructure_Ids)})";
            }

            //if (form.ParCriticalLevel_Ids.Length > 0)
            //{
            //    whereCriticalLevel = $@" AND PLC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")"; 
            //        //" AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")) ";

            //}

            var query = "" +

                $@" DECLARE @DATAINICIAL DATETIME = '{form.startDate.ToString("yyyy-MM-dd")} 00:00:00'
                 DECLARE @DATAFINAL   DATETIME = '{form.endDate.ToString("yyyy-MM-dd")} 23:59:59'       


            SELECT 
	            M.NAME AS MonitoramentoName,
	            M.Id AS Monitoramento_Id,
	            SUM(WeiEvaluation) AS AV,
	            SUM(WeiDefects) AS NC,
	            SUM(WeiDefects)/SUM(WeiEvaluation)*100 AS [PROC]
	            FROM DW.Cubo_Coleta_L2 L2 WITH (NOLOCK)
	            INNER JOIN ParCompany C WITH (NOLOCK)
		            ON L2.Unitid = C.ID
	            INNER JOIN ParLevel1 L WITH (NOLOCK)
		            ON L2.Parlevel1_Id = L.ID
	            INNER JOIN ParLevel2 M WITH (NOLOCK)
		            ON L2.Parlevel2_Id = M.ID
	            INNER JOIN ParDepartment D WITH (NOLOCK)
		            ON L2.Secao_Id = D.ID
                LEFT JOIN ParVinculoPeso PVP
		                ON L2.ParLevel2_Id = PVP.ParLevel2_Id
						AND L2.ParLevel1_Id = PVP.ParLevel1_Id
						AND L2.Cargo_Id = PVP.ParCargo_Id
						AND L2.ParFrequency_Id = PVP.ParFrequencyId
						--AND L2.Centro_De_Custo_Id = PVP.ParDepartment_Id
				LEFT JOIN ParCluster PC
		                ON PVP.ParCluster_Id = PC.Id
		        LEFT JOIN ParClusterGroup PCG
		                ON PC.ParClusterGroup_Id = PCG.Id
                LEFT JOIN (select * from ParStructure where ParStructureGroup_Id = 1) Holding on L2.Holding = Holding.Id
				LEFT JOIN (select * from ParStructure where ParStructureGroup_Id = 2) GrupoDeEmpresa on L2.GrupoDeEmpresa = GrupoDeEmpresa.Id
				LEFT JOIN (select * from ParStructure where ParStructureGroup_Id = 3) Regional on L2.Regional = Regional.Id
	            WHERE 1=1
                AND L2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
                	AND (C.Name = '{form.Param["unitName"] }' OR C.Initials = '{ form.Param["unitName"] }')
                	AND L.Name = '{form.Param["level1Name"]}' 
                    {whereDepartment}
                    {whereDepartmentFiltro}
                    {whereSecao}
                    {whereUnit}
                    {whereCargo}
                    {whereCluster}
                    {whereClusterGroup}
                AND Holding.Id = {form.Param["holding_Id"]}
				AND GrupoDeEmpresa.Id = {form.Param["grupoEmpresa_Id"]} 
				AND Regional.Id = {form.Param["regional_Id"]} 

            GROUP BY 
	            M.NAME, M.Id
            ORDER BY 4 DESC ";


            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeRHResultsSet>(query).ToList();
            }

            return _list;
        }

        [HttpPost]
        [Route("GraficoTarefasAcumuladas")]
        public List<NaoConformidadeRHResultsSet> GraficoTarefasAcumuladas([FromBody] DTO.DataCarrierFormularioNew form)
        {

            var whereDepartment = "";
            var whereDepartmentFiltro = "";
            var whereSecao = "";
            var whereCargo = "";
            var whereStructure = "";
            var whereUnit = "";
            //var whereShift = "";
            var whereCluster = "";
            //var whereCriticalLevel = "";
            var whereClusterGroup = "";

            if (form.Param["departmentName"] != null && form.Param["departmentName"].ToString() != "")
            {
                whereDepartmentFiltro = $@"AND D.Name = '{form.Param["departmentName"]}'";
            }

            if (form.ParDepartment_Ids.Length > 0)
            {
                whereDepartment = $@" AND L3.Centro_De_Custo_Id in ({string.Join(",", form.ParDepartment_Ids)}) ";
            }

            if (form.ParSecao_Ids.Length > 0)
            {
                whereSecao = $@" AND L3.Secao_Id in ({string.Join(",", form.ParSecao_Ids)}) ";
            }

            if (form.ParCargo_Ids.Length > 0)
            {
                whereCargo = $@" AND L3.Cargo_Id in ({string.Join(",", form.ParCargo_Ids)}) ";
            }


            //if (form.Shift_Ids.Length > 0)
            //{
            //    whereShift = "\n AND CL1.Shift in (" + string.Join(",", form.Shift_Ids) + ")";
            //}

            if (form.ParCompany_Ids.Length > 0 && form.ParCompany_Ids[0] > 0)
            {
                whereUnit = $@"AND L3.UnitId in ({ string.Join(",", form.ParCompany_Ids) }) ";
            }
            //else
            //{
            //    whereUnit = $@"AND UNI.Id IN (SELECT
            //    				ParCompany_Id
            //    			FROM ParCompanyXUserSgq
            //    			WHERE UserSgq_Id = { form.Param["auditorId"] })";
            //}

            if (form.ParClusterGroup_Ids.Length > 0)
            {
                whereClusterGroup = $@"AND PCG.Id in (" + string.Join(",", form.ParClusterGroup_Ids) + ")";
            }

            if (form.ParCluster_Ids.Length > 0)
            {
                whereCluster = $@"AND PC.Id in (" + string.Join(",", form.ParCluster_Ids) + ")";
            }

            if (form.ParStructure_Ids.Length > 0)
            {
                whereStructure = $@"AND L3.Regional in ({string.Join(",", form.ParStructure_Ids)})";
            }

            //if (form.ParCriticalLevel_Ids.Length > 0)
            //{
            //    whereCriticalLevel = $@" AND PLC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")"; 
            //        //" AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")) ";

            //}


            var queryGraficoTarefasAcumuladas = $@"
 
        SELECT 
	            T.NAME AS TarefaName,
	            T.ID AS Tarefa_Id,
	            SUM(WeiEvaluation) AS AV,
	            SUM(WeiDefects) AS NC,
	            SUM(WeiDefects)/SUM(WeiEvaluation)*100 AS [PROC]
	            FROM DW.Cubo_Coleta_L3 L3 WITH (NOLOCK)
	            INNER JOIN ParCompany C WITH (NOLOCK)
		            ON L3.Unitid = C.ID
	            INNER JOIN ParLevel1 L WITH (NOLOCK)
		            ON L3.Parlevel1_Id = L.ID
	            INNER JOIN ParLevel2 M WITH (NOLOCK)
		            ON L3.Parlevel2_Id = M.ID
	            INNER JOIN ParLevel3 T WITH (NOLOCK)
		            ON L3.Parlevel3_Id = T.ID
	            INNER JOIN ParDepartment D WITH (NOLOCK)
		            ON L3.Secao_Id = D.ID
                LEFT JOIN ParVinculoPeso PVP
		                ON L3.ParLevel2_Id = PVP.ParLevel2_Id
						AND L3.ParLevel1_Id = PVP.ParLevel1_Id
						AND L3.Cargo_Id = PVP.ParCargo_Id
						AND L3.ParFrequency_Id = PVP.ParFrequencyId
						--AND L3.Centro_De_Custo_Id = PVP.ParDepartment_Id
				    LEFT JOIN ParCluster PC
		                ON PVP.ParCluster_Id = PC.Id
		            LEFT JOIN ParClusterGroup PCG
		                ON PC.ParClusterGroup_Id = PCG.Id
                LEFT JOIN (select * from ParStructure where ParStructureGroup_Id = 1) Holding on L3.Holding = Holding.Id
				LEFT JOIN (select * from ParStructure where ParStructureGroup_Id = 2) GrupoDeEmpresa on L3.GrupoDeEmpresa = GrupoDeEmpresa.Id
				LEFT JOIN (select * from ParStructure where ParStructureGroup_Id = 3) Regional on L3.Regional = Regional.Id
	            WHERE 1=1
                 AND L.Name IN ('{ form.Param["level1Name"] }') 
                 AND C.Name = '{ form.Param["unitName"] }'
                 AND CollectionDate BETWEEN '{ form.startDate.ToString("yyyy-MM-dd") }' AND '{ form.endDate.ToString("yyyy-MM-dd") } 23:59:59'
                 {whereDepartment}
                 {whereDepartmentFiltro}
                 {whereSecao}
                 {whereCargo}
                 {whereUnit}
                 {whereCluster}
                 {whereClusterGroup}
                AND Holding.Id = {form.Param["holding_Id"]}
				AND GrupoDeEmpresa.Id = {form.Param["grupoEmpresa_Id"]} 
				AND Regional.Id = {form.Param["regional_Id"]} 
        GROUP BY 
	        T.NAME, T.ID
        ORDER BY 4 DESC
 ";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeRHResultsSet>(queryGraficoTarefasAcumuladas).ToList();
            }

            return _list;
        }

        [HttpPost]
        [Route("GraficoTarefa")]
        public List<NaoConformidadeRHResultsSet> GraficoTarefa([FromBody] DTO.DataCarrierFormularioNew form)
        {
            //_list = CriaMockGraficoTarefas();

            //var query = new NaoConformidadeRHResultsSet().Select(form._dataInicio, form._dataFim, form.unitId);

            //Av = av + i,
            //Nc = nc + i,
            //Proc = proc + i,
            //TarefaName = tarefaName + i.ToString()

            //var whereDepartmentFiltro = "";
            var whereDepartment = "";
            var whereShift = "";
            var whereClusterGroup = "";
            var whereCluster = "";
            var whereUnit = "";

            if (form.Shift_Ids.Length > 0)
            {
                whereShift = "\n AND L3.Shift   in (" + string.Join(",", form.Shift_Ids) + ") ";
            }

            if (form.ParCompany_Ids.Length > 0 && form.ParCompany_Ids[0] > 0)
            {
                whereUnit = $@"AND L3.UnitId in ({ string.Join(",", form.ParCompany_Ids) }) ";
            }

            if (form.ParClusterGroup_Ids.Length > 0)
            {
                whereClusterGroup = $@"AND PCG.Id in (" + string.Join(",", form.ParClusterGroup_Ids) + ")";
            }

            if (form.ParCluster_Ids.Length > 0)
            {
                whereCluster = $@"AND PC.Id in (" + string.Join(",", form.ParCluster_Ids) + ")";
            }

            if (form.Param["departmentName"] != null && form.Param["departmentName"].ToString() != "")
            {
                whereDepartment = " AND D.Name = '" + form.Param["departmentName"] + "'";
            }

            if (form.ParDepartment_Ids.Length > 0)
            {
                whereDepartment += $@" AND L3.Centro_De_Custo_Id in ({string.Join(",", form.ParDepartment_Ids)}) ";
            }

            var query = "" +

                $@" 
 
        SELECT 
	            T.NAME AS TarefaName,
	            T.ID AS Tarefa_Id,
	            SUM(WeiEvaluation) AS AV,
	            SUM(WeiDefects) AS NC,
	            SUM(WeiDefects)/SUM(WeiEvaluation)*100 AS [PROC]
	            FROM DW.Cubo_Coleta_L3 L3 WITH (NOLOCK)
	            INNER JOIN ParCompany C WITH (NOLOCK)
		            ON L3.Unitid = C.ID
	            INNER JOIN ParLevel1 L WITH (NOLOCK)
		            ON L3.Parlevel1_Id = L.ID
	            INNER JOIN ParLevel2 M WITH (NOLOCK)
		            ON L3.Parlevel2_Id = M.ID
	            INNER JOIN ParLevel3 T WITH (NOLOCK)
		            ON L3.Parlevel3_Id = T.ID
	            INNER JOIN ParDepartment D WITH (NOLOCK)
		            ON L3.Secao_Id = D.ID
                LEFT JOIN ParVinculoPeso PVP
		                ON L3.ParLevel2_Id = PVP.ParLevel2_Id
						AND L3.ParLevel1_Id = PVP.ParLevel1_Id
						AND L3.Cargo_Id = PVP.ParCargo_Id
						AND L3.ParFrequency_Id = PVP.ParFrequencyId
						--AND L3.Centro_De_Custo_Id = PVP.ParDepartment_Id
				    LEFT JOIN ParCluster PC
		                ON PVP.ParCluster_Id = PC.Id
		            LEFT JOIN ParClusterGroup PCG
		                ON PC.ParClusterGroup_Id = PCG.Id
                LEFT JOIN (select * from ParStructure where ParStructureGroup_Id = 1) Holding on L3.Holding = Holding.Id
				LEFT JOIN (select * from ParStructure where ParStructureGroup_Id = 2) GrupoDeEmpresa on L3.GrupoDeEmpresa = GrupoDeEmpresa.Id
				LEFT JOIN (select * from ParStructure where ParStructureGroup_Id = 3) Regional on L3.Regional = Regional.Id
	            WHERE 1=1
                 AND L.Name IN ('{ form.Param["level1Name"] }') 
                 AND M.Name = '{ form.Param["level2Name"] }'
                 AND C.Name = '{ form.Param["unitName"] }'
                 AND CollectionDate BETWEEN '{ form.startDate.ToString("yyyy-MM-dd") }' AND '{ form.endDate.ToString("yyyy-MM-dd") } 23:59:59'
                 {whereDepartment}
                 {whereCluster}
                 {whereClusterGroup}
                 {whereUnit}
                AND Holding.Id = {form.Param["holding_Id"]}
				AND GrupoDeEmpresa.Id = {form.Param["grupoEmpresa_Id"]} 
				AND Regional.Id = {form.Param["regional_Id"]} 
        GROUP BY 
	        T.NAME, T.ID
        ORDER BY 4 DESC
";


            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeRHResultsSet>(query).ToList();
            }

            return _list;
        }

        [HttpPost]
        [Route("PivotTable")]
        public dynamic PivotTable([FromBody] DTO.DataCarrierFormularioNew form)
        {
            using (SgqDbDevEntities dbSgq = new SgqDbDevEntities())
            {

                var level1Name = form.Param["level1Name"].ToString();
                var level2Name = form.Param["level2Name"].ToString();
                var centroDeCustoName = form.Param["level2Name"].ToString();
                var secaoName = form.Param["level2Name"].ToString();
                var cargoName = form.Param["level2Name"].ToString();
                var unitName = form.Param["unitName"].ToString();

                var parLevel1_Id = dbSgq.ParLevel1.Where(r => r.Name == level1Name).Select(r => r.Id).FirstOrDefault();

                var parLevel2_Id = dbSgq.ParLevel2.Where(r => r.Name == level2Name).Select(r => r.Id).FirstOrDefault();

                var parsecao_Id = dbSgq.ParDepartment.Where(r => r.Name == secaoName).Select(r => r.Id).FirstOrDefault();

                var unit_Id = dbSgq.ParCompany.Where(r => r.Name == unitName).Select(r => r.Id).FirstOrDefault();

                var sql = $@"
		 
		         -------------------------------------------------------------------------------------------------------------------------
		         --------	INPUTS					
		         -------------------------------------------------------------------------------------------------------------------------

		         DECLARE @DATEINI DATETIME = '{form.startDate.ToString("yyyy-MM-dd")} 00:00:00' DECLARE @DATEFIM DATETIME = '{form.endDate.ToString("yyyy-MM-dd")} 23:59:59';
		         DECLARE @UNITID VARCHAR(10) = '{unit_Id}', @PARLEVEL1_ID VARCHAR(10) = '{parLevel1_Id}',@PARLEVEL2_ID VARCHAR(10) = '{parLevel2_Id}',@SECAO_ID VARCHAR(10) = '{parsecao_Id}';

		         -------------------------------------------------------------------------------------------------------------------------
		         -------------------------------------------------------------------------------------------------------------------------		 
		   
		             DECLARE @DATAINICIAL DATETIME = @DATEINI;
		             DECLARE @DATAFINAL DATETIME = @DATEFIM;                     

SELECT 
	F.Name		 as Frequencia
	,S1.Name     as Holding
	,S2.Name     as GrupoEmpresa
	,S3.Name     as Regional
	,C.Name      as Unidade 
	,D1.Name     as CentroDeCusto
	,D2.Name     as Seção
	,CG.Name     as Cargo
	,L1.Name     as Indicador
	,L2.Name     as Monitoramento
	,L3.Name     as Tarefa
	,U.FullName  as Auditor
	,SUM(WeiEvaluation) as AV
	,SUM(WeiDefects) as NC
	,SUM(WeiEvaluation) as AVComPeso
	,SUM(WeiDefects) as NCComPeso
FROM DW.Cubo_Coleta_L3 C3
	LEFT JOIN ParFrequency F
		ON C3.ParFrequency_Id = F.ID
	LEFT JOIN ParStructure S1
		ON C3.Holding = S1.Id
	LEFT JOIN ParStructure S2
			ON C3.Holding = S2.Id
	LEFT JOIN ParStructure S3
			ON C3.Holding = S3.Id
	LEFT JOIN ParCompany C
			ON C3.UnitId = C.ID
	LEFT JOIN ParDepartment D1
			ON C3.Centro_De_Custo_Id = D1.ID
	LEFT JOIN ParDepartment D2
			ON C3.Secao_Id = D2.ID
	LEFT JOIN ParCargo CG
			ON C3.Cargo_Id = CG.ID
	LEFT JOIN ParLevel1 L1
			ON C3.ParLevel1_Id = L1.ID
	LEFT JOIN ParLevel2 L2
			ON C3.ParLevel2_Id = L2.ID
	LEFT JOIN ParLevel3 L3
			ON C3.ParLevel3_Id = L3.ID
	LEFT JOIN UserSgq U
			ON C3.AuditorId = U.ID
WHERE 1=1
	AND C3.Collectiondate BETWEEN @DATEINI AND @DATEFIM
	AND CASE WHEN @UNITID = '0' THEN '0' ELSE C3.unitid END = @UNITID
	AND CASE WHEN @PARLEVEL1_ID = '0' THEN '0' ELSE C3.ParLevel1_id END = @PARLEVEL1_ID
	AND CASE WHEN @PARLEVEL2_ID = '0' THEN '0' ELSE C3.ParLevel2_id END = @PARLEVEL2_ID
	AND CASE WHEN @SECAO_ID = '0' THEN '0' ELSE C3.Secao_Id END = @SECAO_ID
GROUP BY 
F.Name		 
,S1.Name     
,S2.Name     
,S3.Name     
,C.Name      
,D1.Name     
,D2.Name     
,CG.Name     
,L1.Name     
,L2.Name     
,L3.Name     
,U.FullName  

                ";
                return QueryNinja(dbSgq, sql);

                //return QueryNinja(dbSgq, "select top 1000 parlevel3_id, weight, parlevel3_name from Result_Level3");
            }
        }


        [HttpPost]
        [Route("GraficoTarefasAcumulada")]
        public List<NaoConformidadeRHResultsSet> GraficoTarefasAcumulada([FromBody] DTO.DataCarrierFormularioNew form)
        {
            //_list = CriaMockGraficoTarefasAcumuladas();

            //var query = new NaoConformidadeRHResultsSet().Select(form._dataInicio, form._dataFim, form.unitId);

            var query = "SELECT " +
                        "\n  " +
                        //     "\n  IND.Id AS level1_Id " +
                        //     "\n ,IND.Name AS Level1Name " +
                        //     "\n ,IND.Id AS level2_Id " +
                        //     "\n ,IND.Name AS Level2Name " +
                        //     "\n ,R3.ParLevel3_Id AS level3_Id " +
                        "\n R3.ParLevel3_Name AS TarefaName " +
                        //     "\n ,UNI.Name AS Unidade " +
                        //     "\n ,UNI.Id AS Unidade_Id " +
                        "\n ,SUM(R3.WeiDefects) AS Nc " +
                        "\n ,SUM(R3.WeiEvaluation) AS Av " +
                        "\n ,SUM(R3.WeiDefects) / SUM(R3.WeiEvaluation) * 100 AS [Proc] " +
                        "\n FROM Result_Level3 R3  (nolock)" +
                        "\n INNER JOIN CollectionLevel2 C2  (nolock)" +
                        "\n ON C2.Id = R3.CollectionLevel2_Id " +
                        "\n INNER JOIN ConsolidationLevel2 CL2  (nolock)" +
                        "\n ON CL2.Id = C2.ConsolidationLevel2_Id " +
                        "\n INNER JOIN ConsolidationLevel1 CL1  (nolock)" +
                        "\n ON CL1.Id = CL2.ConsolidationLevel1_Id " +
                        "\n INNER JOIN ParCompany UNI  (nolock)" +
                        "\n ON UNI.Id = CL1.UnitId " +
                        "\n INNER JOIN ParLevel1 IND   (nolock)" +
                        "\n ON IND.Id = CL1.ParLevel1_Id " +
                        "\n INNER JOIN ParLevel2 MON  (nolock)" +
                        "\n ON MON.Id = CL2.ParLevel2_Id " +
                        "\n WHERE IND.Name ='" + form.Param["level1Name"] + "' " +
                        "\n /* and MON.Id = 1 */" +
                        "\n 	AND UNI.Name = '" + form.Param["unitName"] + "'" +
                        "\n 	AND CL2.ConsolidationDate BETWEEN '" + form.startDate.ToString("yyyy-MM-dd") + "' AND '" + form.endDate.ToString("yyyy-MM-dd") + "'" +
                        "\n GROUP BY " +
                        "\n -- IND.Id " +
                        "\n -- ,IND.Name " +
                        "\n  R3.ParLevel3_Id " +
                        "\n ,R3.ParLevel3_Name " +
                        "\n -- ,UNI.Name " +
                        "\n -- ,UNI.Id " +
                        "\n HAVING (SUM(R3.WeiDefects) / SUM(R3.WeiEvaluation) * 100) > 0" +
                        "\n ORDER BY 4 DESC";


            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeRHResultsSet>(query).ToList();
            }

            return _list;
        }


        [HttpPost]
        [Route("GetHashDepartment/{centroCusto}")]
        public string GetHashDepartment(int centroCusto)
        {

            int? parent_Id = centroCusto;
            List<int> hash = new List<int>();

            using (Factory factory = new Factory("DefaultConnection"))
            {
                do
                {
                    var parDepartment = factory.SearchQuery<ParDepartment>("select * from ParDepartment where Id = " + parent_Id).FirstOrDefault();

                    hash.Add(parDepartment.Id);

                    parent_Id = parDepartment.Parent_Id;

                } while (parent_Id != null);

            }

            hash.Reverse();

            return string.Join("-", hash);

        }

        internal List<NaoConformidadeRHResultsSet> CriaMockGraficoUnidades()
        {

            #region Props
            var nc = 10;
            var av = 10;
            var proc = 20;
            var unidade = "Unidade";
            #endregion

            var list = new List<NaoConformidadeRHResultsSet>();
            for (int i = 0; i < 30; i++)
            {
                list.Add(new NaoConformidadeRHResultsSet() { Av = av + i, Nc = nc + i, Proc = proc + i, UnidadeName = unidade + i.ToString() });
                i += 10;
            }
            return list;
        }

        internal List<NaoConformidadeRHResultsSet> CriaMockGraficoNcPorUnidadeIndicador()
        {
            #region Props
            var nc = 10;
            var av = 10;
            var proc = 20;
            var Meta = 2;
            var indicadorName = "Indicador1";
            #endregion

            var list = new List<NaoConformidadeRHResultsSet>();
            for (int i = 0; i < 60; i++)
            {
                list.Add(new NaoConformidadeRHResultsSet()
                {
                    Av = av + i,
                    Nc = nc + i,
                    Proc = proc + i,
                    Meta = Meta + i - 5,
                    IndicadorName = indicadorName + i.ToString()
                });
                i += 10;
            }
            return list;
        }

        internal List<NaoConformidadeRHResultsSet> CriaMockGraficoMonitoramento()
        {
            //var Meta = 2;

            #region Prop
            var av = 10;
            var nc = 10;
            var proc = 20;
            var monitoramento = "Monitoramento";
            #endregion

            var list = new List<NaoConformidadeRHResultsSet>();
            for (int i = 0; i < 60; i++)
            {
                list.Add(new NaoConformidadeRHResultsSet()
                {
                    Av = av + i,
                    Nc = nc + i,
                    Proc = proc + i,
                    //Meta = Meta + i - 5,
                    MonitoramentoName = monitoramento + i.ToString()
                });
                i += 10;
            }
            return list;
        }

        internal List<NaoConformidadeRHResultsSet> CriaMockGraficoTarefas()
        {
            //var Meta = 2;

            #region Prop
            var av = 10;
            var nc = 10;
            var proc = 20;
            var tarefaName = "Tarefa";
            #endregion

            var list = new List<NaoConformidadeRHResultsSet>();
            for (int i = 0; i < 90; i++)
            {
                list.Add(new NaoConformidadeRHResultsSet()
                {
                    Av = av + i,
                    Nc = nc + i,
                    Proc = proc + i,
                    //Meta = Meta + i - 5,
                    TarefaName = tarefaName + i.ToString()
                });
                i += 10;
            }
            return list;
        }

        internal List<NaoConformidadeRHResultsSet> CriaMockGraficoTarefasAcumuladas()
        {
            //var Meta = 2;

            #region Prop
            var av = 10;
            var nc = 10;
            var proc = 20;
            var tarefaName = "TarefaAcumulada";
            #endregion

            var list = new List<NaoConformidadeRHResultsSet>();
            for (int i = 0; i < 90; i++)
            {
                list.Add(new NaoConformidadeRHResultsSet()
                {
                    Av = av + i,
                    Nc = nc + i,
                    Proc = proc + i,
                    //Meta = Meta + i - 5,
                    TarefaName = tarefaName + i.ToString()
                });
                i += 10;
            }
            return list;
        }

        private string GetUserUnits(int User)
        {
            using (var db = new SgqDbDevEntities())
            {
                return string.Join(",", db.ParCompanyXUserSgq.Where(r => r.UserSgq_Id == User).Select(r => r.ParCompany_Id).ToList());
            }
        }

    }

}


public class NaoConformidadeRHResultsSet
{

    internal string Select(DateTime _dataInicio, DateTime _dataFim, int unitId)
    {
        return "";
    }

    public string Indicador_Id { get; set; }
    public string IndicadorName { get; set; }
    public string GrupoDeEmpresa_Id { get; set; }
    public string GrupoDeEmpresaName { get; set; }
    public string Holding_Id { get; set; }
    public string HoldingName { get; set; }
    public string Regional_Id { get; set; }
    public string RegionalName { get; set; }
    public string DepartamentoName { get; set; }
    public string Departamento_Id { get; set; }
    public string Unidade_Id { get; set; }
    public string UnidadeName { get; set; }
    public string Monitoramento_Id { get; set; }
    public string MonitoramentoName { get; set; }
    public string Tarefa_Id { get; set; }
    public string TarefaName { get; set; }
    public decimal? Nc { get; set; }
    public decimal? Av { get; set; }
    public decimal? Meta { get; set; }
    public decimal? Proc { get; internal set; }
    public int? Shift { get; set; }
    public string dataX { get; set; }
}