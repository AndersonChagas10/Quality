using ADOFactory;
using Dominio;
using DTO;
using DTO.Formulario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Formulario
{
    [RoutePrefix("api/Formulario")]
    public class FormularioApiController : BaseApiController
    {

        public FormularioApiController()
        {

        }

        [HttpPost]
        [Route("GetFilteredParCompany")]
        public List<Select3ViewModel> GetFilteredParCompany(string search, [FromBody] DataCarrierFormularioNew form)
        {

            var whereStructure = "";
            var whereClusterGroup = "";
            var whereCluster = "";

            //Module

            //ParStructure
            if (form.ParStructure_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure_Ids) })";
            }

            if (form.ParStructure1_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure1_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure1_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure1_Ids) })";
            }

            //Grupo de empresa
            if (form.ParStructure2_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure2_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure2_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure2_Ids) })";
            }

            //Regional
            if (form.ParStructure3_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure3_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure3_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure3_Ids) })";
            }

            //Grupo de Cluster
            if (form.ParClusterGroup_Ids.Length > 0)
            {
                whereClusterGroup = $" AND PCG.Id IN ({ string.Join(",", form.ParClusterGroup_Ids) })";
            }

            //Cluster
            if (form.ParClusterGroup_Ids.Length > 0)
                whereCluster = $" AND PCL.Id IN ({ string.Join(",", form.ParCluster_Ids) })";


            using (var factory = new Factory("DefaultConnection"))
            {

                var query = $@"SELECT DISTINCT TOP 500
                        	PC.Id, PC.Name
                        FROM ParCompany PC WITH (NOLOCK)
                        LEFT JOIN ParCompanyXStructure PCXS WITH (NOLOCK) ON PC.Id = PCXS.ParCompany_Id AND PCXS.Active = 1 
						LEFT JOIN ParStructure PS WITH (NOLOCK) ON PS.Id = PCXS.ParStructure_Id
						LEFT JOIN ParStructure PS1 WITH (NOLOCK) ON PS.ParStructureParent_Id = PS1.ID
                        LEFT JOIN ParEvaluationXDepartmentXCargo PEDC WITH (NOLOCK) on (PEDC.ParCompany_Id = PC.Id OR PEDC.ParCompany_Id IS NULL) AND PEDC.IsActive = 1
                        INNER JOIN ParCluster PCL WITH (NOLOCK) on (PCL.Id = PEDC.ParCluster_Id OR PEDC.ParCluster_Id IS NULL)
                        INNER JOIN ParClusterGroup PCG WITH (NOLOCK) on PCG.Id = PCL.ParClusterGroup_Id
                        WHERE 1 = 1
                        AND PC.IsActive = 1
                        --Filtros
                        AND PC.Name like '%{search}%'
                        {whereStructure}
                        {whereClusterGroup}
                        {whereCluster}";

                var retorno = factory.SearchQuery<Select3ViewModel>(query).ToList();

                return retorno;

            }
        }

        [HttpPost]
        [Route("GetFilteredParCriticalLevels")]
        public List<Select3ViewModel> GetFilteredParCriticalLevels(string search, [FromBody] DataCarrierFormularioNew form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {
                //var filtroStructure = form.ParStructure_Ids.Length > 0 ? $@"AND PCXS.Id IN ({ string.Join(",", form.ParStructure_Ids) })" : "";

                var query = $@"SELECT DISTINCT TOP 500
                        	PCL.Id, PCL.Name
                        FROM ParCriticalLevel PCL WITH (NOLOCK)
						WHERE 1 = 1
                        AND PCL.IsActive = 1
                        AND PCL.Name like '%{search}%'";

                var retorno = factory.SearchQuery<Select3ViewModel>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredParGroupParLevel1s")]
        public List<ParGroupParLevel1> GetFilteredParGroupParLevel1s(string search, [FromBody] DataCarrierFormularioNew form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {

                var query = $@" SELECT DISTINCT TOP 500
                        	Id, Name from ParGroupParLevel1 WITH (NOLOCK) Where IsActive = 1 AND Name like '%{search}%'";

                var retorno = factory.SearchQuery<ParGroupParLevel1>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredParClusters")]
        public List<Select3ViewModel> GetFilteredParClusters(string search, [FromBody] DataCarrierFormularioNew form)
        {
            var whereClusterGroup = "";

            if (form.ParClusterGroup_Ids.Length > 0)
            {
                whereClusterGroup = $" AND PCL.ParClusterGroup_Id IN ({string.Join(",", form.ParClusterGroup_Ids)})";
            }

            using (var factory = new Factory("DefaultConnection"))
            {

                var query = $@"SELECT DISTINCT TOP 500
                        	PCL.Id, PCL.Name
                        FROM ParCluster PCL WITH (NOLOCK)
						WHERE 1 = 1
                        AND PCL.IsActive = 1
                        { whereClusterGroup }
                        AND PCL.Name like '%{search}%'";

                var retorno = factory.SearchQuery<Select3ViewModel>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredParClustersGroup")]
        public List<Select3ViewModel> GetFilteredParClustersGroup(string search, [FromBody] DataCarrierFormularioNew form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {

                var query = $@"SELECT DISTINCT TOP 500
                        	PCLG.Id, PCLG.Name
                        FROM ParClusterGroup PCLG WITH (NOLOCK)
						WHERE 1 = 1
                        AND PCLG.IsActive = 1
                        AND PCLG.Name like '%{search}%'";

                var retorno = factory.SearchQuery<Select3ViewModel>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredShift")]
        public List<Select3ViewModel> GetFilteredShift(string search, [FromBody] DataCarrierFormularioNew form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {
                var query = $@"SELECT DISTINCT TOP 500 ID, Description as Name FROM shift WITH (NOLOCK)
                        WHERE Description like '%{search}%'";

                var retorno = factory.SearchQuery<Select3ViewModel>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredParDepartment")]
        public List<Select3ViewModel> GetFilteredParDepartment(string search, [FromBody] DataCarrierFormularioNew form)
        {
            //if (GlobalConfig.SESMT)
            //{

            var sqlParCompany = "";
            var whereClusterGroup = "";
            var whereStructure = "";
            var whereCluster = "";

            /*Aqui filtro de se utliza filtros*/

            //Module

            //ParStructure
            if (form.ParStructure_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure_Ids) })";
            }

            if (form.ParStructure1_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure1_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure1_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure1_Ids) })";
            }

            //Grupo de empresa
            if (form.ParStructure2_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure2_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure2_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure2_Ids) }) --Grupo de Empresa";
            }

            //Regional
            if (form.ParStructure3_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure3_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure3_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure3_Ids) }) --Regional";
            }

            //Grupo de Cluster
            if (form.ParClusterGroup_Ids.Length > 0)
            {
                whereClusterGroup = $" AND PCG.Id IN ({ string.Join(",", form.ParClusterGroup_Ids) }) --Grupo de cluster";
            }

            //Cluster
            if (form.ParClusterGroup_Ids.Length > 0)
                whereCluster = $" AND PCL.Id IN ({ string.Join(",", form.ParCluster_Ids) }) --Cluster";

            if (form.ParCompany_Ids.Length > 0)
            {
                sqlParCompany = $@" AND PVP.ParCompany_Id IN ({string.Join(",", form.ParCompany_Ids)}) --Unidade ";
            }


            using (var factory = new Factory("DefaultConnection"))
            {

                //var query = $@"SELECT DISTINCT TOP 500 PD.Id,PD.Name FROM ParDepartment PD 
                //                WHERE 1=1 
                //                AND PD.Active = 1 
                //                AND PD.Name like '%{search}%'
                //                AND (PD.Parent_Id IS NULL OR PD.Parent_Id = 0) " + sqlParCompany;

                //var query = $@"
                //            SELECT DISTINCT TOP 500 PD.Id,PD.Name 
                //            FROM ParDepartment PD WITH (NOLOCK)
                //            LEFT JOIN ParEvaluationXDepartmentXCargo PEDC WITH (NOLOCK) on (PEDC.ParDepartment_Id = PD.Id OR PEDC.ParDepartment_Id IS NULL) AND PEDC.IsActive = 1
                //            LEFT JOIN ParCompany PC WITH (NOLOCK) on (PEDC.ParCompany_Id = PC.Id OR PEDC.ParCompany_Id IS NULL) AND PEDC.IsActive = 1
                //            LEFT JOIN ParCompanyXStructure PCXS WITH (NOLOCK) ON PC.Id = PCXS.ParCompany_Id AND PCXS.Active = 1
                //            LEFT JOIN ParStructure PS WITH (NOLOCK) ON PS.Id = PCXS.ParStructure_Id
                //            LEFT JOIN ParStructure PS1 WITH (NOLOCK) ON PS.ParStructureParent_Id = PS1.ID
                //            LEFT JOIN ParCluster PCL WITH (NOLOCK) on (PCL.Id = PEDC.ParCluster_Id OR PEDC.ParCluster_Id IS NULL)
                //            LEFT JOIN ParClusterGroup PCG WITH (NOLOCK) on PCG.Id = PCL.ParClusterGroup_Id
                //            WHERE 1=1 
                //            AND PD.Active = 1 
                //            AND PD.Name like '%{search}%'
                //            {whereClusterGroup}
                //            {whereCluster}
                //            {whereStructure}
                //            {sqlParCompany}
                //            AND (PD.Parent_Id IS NULL OR PD.Parent_Id = 0)";

                var query = $@"
                            SELECT DISTINCT TOP 500
                            	CentroCusto.*
                            FROM ParDepartment Secao
                            INNER JOIN (SELECT --*
                            	DISTINCT PVP.ParDepartment_Id AS Id
                            	FROM ParVinculoPeso PVP WITH (NOLOCK)
                            	INNER JOIN ParCluster PCL WITH (NOLOCK) ON PCL.Id = PVP.ParCluster_Id
                            	INNER JOIN ParClusterGroup PCG WITH (NOLOCK) ON PCG.Id = PCL.ParClusterGroup_Id
                            	INNER JOIN ParCompany PC WITH (NOLOCK) ON (PVP.ParCompany_Id = PC.Id OR PVP.ParCompany_Id IS NULL)
                            	LEFT JOIN ParCompanyXStructure PCXS WITH (NOLOCK) ON PC.id = PCXS.ParCompany_Id AND PCXS.Active = 1
                            	LEFT JOIN ParStructure PS WITH (NOLOCK) ON PS.Id = PCXS.ParStructure_Id
                                LEFT JOIN ParStructure PS1 WITH (NOLOCK) ON PS.ParStructureParent_Id = PS1.ID
                            	WHERE 1 = 1
                            	AND PVP.IsActive = 1
                                {whereClusterGroup}
                                {whereCluster}
                                {whereStructure}
                            	{sqlParCompany} 	
                            	) PVP
                            	ON (PVP.Id = Secao.Id OR PVP.Id IS NULL) AND Secao.Active = 1
                            INNER JOIN ParDepartment CentroCusto ON CentroCusto.Id = Secao.Parent_Id AND CentroCusto.Active = 1
                            WHERE 1 = 1
                            AND Secao.Active = 1
                            AND CentroCusto.Name like '%{search}%'
                            AND (CentroCusto.Parent_Id IS NULL OR CentroCusto.Parent_Id = 0)";

                var retorno = factory.SearchQuery<Select3ViewModel>(query).ToList();

                return retorno;

                //}
            }
        }

        [HttpPost]
        [Route("GetFilteredParDepartmentFilho")]
        public List<Select3ViewModel> GetFilteredParDepartmentFilho(string search, [FromBody] DataCarrierFormularioNew form)
        {

            var whereClusterGroup = "";
            var whereStructure = "";
            var whereCluster = "";
            var whereParcompany = "";
            var whereParDepartment = "";

            //Module

            //ParStructure
            if (form.ParStructure_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure_Ids) })";
            }

            if (form.ParStructure1_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure1_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure1_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure1_Ids) })";
            }

            //Grupo de empresa
            if (form.ParStructure2_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure2_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure2_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure2_Ids) })";
            }

            //Regional
            if (form.ParStructure3_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure3_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure3_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure3_Ids) })";
            }

            //Grupo de Cluster
            if (form.ParClusterGroup_Ids.Length > 0)
            {
                whereClusterGroup = $" AND PCG.Id IN ({ string.Join(",", form.ParClusterGroup_Ids) })";
            }

            //Cluster
            if (form.ParClusterGroup_Ids.Length > 0)
                whereCluster = $" AND PCL.Id IN ({ string.Join(",", form.ParCluster_Ids) })";

            if (form.ParCompany_Ids != null && form.ParCompany_Ids.Length > 0)
            {
                whereParcompany = $@" AND PVP.ParCompany_Id IN ({string.Join(",", form.ParCompany_Ids)})";
            }

            if(form.ParDepartment_Ids != null && form.ParDepartment_Ids.Length > 0)
            {
                whereParDepartment = $@" AND CentroCusto.Id IN ({string.Join(",", form.ParDepartment_Ids)})";
            }

            using (var factory = new Factory("DefaultConnection"))
            {
                //var retornoFormulario = new FormularioViewModel();

                //retornoFormulario.ParDepartments = GetParDepartments(form, factory);

                //var sqlDepartamentoPelaHash = "";

                //if (form.ParDepartment_Ids.Length > 0 && retornoFormulario.ParDepartments.Count > 0)
                //{
                //    sqlDepartamentoPelaHash += $@"AND PD.Hash in ({string.Join(",", form.ParDepartment_Ids)})
                //            AND (PD.Parent_Id IS not NULL OR PD.Parent_Id <> 0)  ";
                //}


                //var query = $@"SELECT DISTINCT TOP 500 PD.Hash,PD.Id,PD.Name 
                //FROM ParDepartment PD WITH (NOLOCK)
                //LEFT JOIN ParEvaluationXDepartmentXCargo PEDC WITH (NOLOCK) on (PEDC.ParDepartment_Id = PD.Id OR PEDC.ParDepartment_Id IS NULL) AND PEDC.IsActive = 1
                //LEFT JOIN ParCompany PC WITH (NOLOCK) on (PEDC.ParCompany_Id = PC.Id OR PEDC.ParCompany_Id IS NULL) AND PEDC.IsActive = 1
                //LEFT JOIN ParCompanyXStructure PCXS WITH (NOLOCK) ON PC.Id = PCXS.ParCompany_Id AND PCXS.Active = 1
                //LEFT JOIN ParStructure PS WITH (NOLOCK) ON PS.Id = PCXS.ParStructure_Id
                //LEFT JOIN ParStructure PS1 WITH (NOLOCK) ON PS.ParStructureParent_Id = PS1.ID
                //LEFT JOIN ParCluster PCL WITH (NOLOCK) on (PCL.Id = PEDC.ParCluster_Id OR PEDC.ParCluster_Id IS NULL)
                //LEFT JOIN ParClusterGroup PCG WITH (NOLOCK) on PCG.Id = PCL.ParClusterGroup_Id
                //WHERE 1=1 
                //AND PD.Active = 1 
                //{whereClusterGroup}
                //{whereCluster}
                //{whereStructure}
                //AND PD.Name like '%{search}%'
                //AND PD.Parent_Id IS NOT NULL " + sqlDepartamentoPelaHash;

                var query = $@"--Query de seção
                            SELECT DISTINCT TOP 500
                            	Secao.*
                            FROM ParDepartment Secao
                            INNER JOIN (SELECT --*
                            	DISTINCT PVP.ParDepartment_Id AS Id
                            	FROM ParVinculoPeso PVP WITH (NOLOCK)
                            	INNER JOIN ParCluster PCL WITH (NOLOCK) ON PCL.Id = PVP.ParCluster_Id
                            	INNER JOIN ParClusterGroup PCG WITH (NOLOCK) ON PCG.Id = PCL.ParClusterGroup_Id
                            	INNER JOIN ParCompany PC WITH (NOLOCK) ON (PVP.ParCompany_Id = PC.Id OR PVP.ParCompany_Id IS NULL)
                            	LEFT JOIN ParCompanyXStructure PCXS WITH (NOLOCK) ON PC.id = PCXS.ParCompany_Id AND PCXS.Active = 1
                            	LEFT JOIN ParStructure PS WITH (NOLOCK) ON PS.Id = PCXS.ParStructure_Id
                                LEFT JOIN ParStructure PS1 WITH (NOLOCK) ON PS.ParStructureParent_Id = PS1.ID
                            	WHERE 1 = 1
                            	AND PVP.IsActive = 1
                                {whereClusterGroup}
                                {whereCluster}
                                {whereStructure}
                                {whereParcompany}
                            	) PVP
                            	ON (PVP.Id = Secao.Id OR PVP.Id IS NULL) AND Secao.Active = 1
                            INNER JOIN ParDepartment CentroCusto ON CentroCusto.Id = Secao.Parent_Id AND CentroCusto.Active = 1
                            WHERE 1 = 1
                            AND Secao.Active = 1
                            {whereParDepartment}
                            AND (CentroCusto.Parent_Id IS NULL OR CentroCusto.Parent_Id = 0)";

                var retorno = factory.SearchQuery<Select3ViewModel>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredParCargo")]
        public List<Select3ViewModel> GetFilteredParCargo(string search, [FromBody] DataCarrierFormularioNew form)
        {

            var sqlParCompany = "";
            var whereClusterGroup = "";
            var whereStructure = "";
            var whereCluster = "";
            var whereCentroCusto = "";
            var whereSecao = "";

            //Module

            //ParStructure
            if (form.ParStructure_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure_Ids) })";
            }

            if (form.ParStructure1_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure1_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure1_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure1_Ids) })";
            }

            //Grupo de empresa
            if (form.ParStructure2_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure2_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure2_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure2_Ids) })";
            }

            //Regional
            if (form.ParStructure3_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure3_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure3_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure3_Ids) })";
            }

            //Grupo de Cluster
            if (form.ParClusterGroup_Ids.Length > 0)
            {
                whereClusterGroup = $" AND PCG.Id IN ({ string.Join(",", form.ParClusterGroup_Ids) })";
            }

            //Cluster
            if (form.ParClusterGroup_Ids.Length > 0)
                whereCluster = $" AND PCL.Id IN ({ string.Join(",", form.ParCluster_Ids) })";

            if (form.ParCompany_Ids.Length > 0)
            {
                sqlParCompany = $@" AND PVP.ParCompany_Id in ({string.Join(",", form.ParCompany_Ids)}) ";
            }

            if (form.ParDepartment_Ids.Length > 0)
                whereCentroCusto = $@"AND CentroCusto.Id IN ({string.Join(",", form.ParDepartment_Ids)})";


            if (form.ParDepartment_Ids.Length > 0)
                whereSecao = $@"AND Secao.Id IN ({string.Join(",", form.ParSecao_Ids)})";

            using (var factory = new Factory("DefaultConnection"))
            {
                //var retornoFormulario = new FormularioViewModel();
                //retornoFormulario.ParDepartments = GetParDepartments(form, factory);
                //retornoFormulario.ParSecoes = GetParSecoes(form, factory, retornoFormulario);
                //var parDepartment_Ids = form.ParSecao_Ids.Length > 0 ? form.ParSecao_Ids.ToList() : retornoFormulario.ParSecoes?.Select(x => x.Id).ToList();


                //var sqlParDepartment = "";

                //if (form.ParCompany_Ids.Length > 0 && parDepartment_Ids.Count > 0)
                //{
                //    sqlParDepartment = $@" AND PCXD.ParDepartment_Id IN ({string.Join(",", parDepartment_Ids)})";
                //}

                //var query = $@"SELECT DISTINCT TOP 500 PC.Id,PC.Name FROM ParCargo PC WITH (NOLOCK)
                //        LEFT JOIN ParCargoXDepartment PCXD WITH (NOLOCK) ON PCXD.ParCargo_Id = PC.Id
                //        LEFT JOIN ParEvaluationXDepartmentXCargo PEDC WITH (NOLOCK) on (PEDC.ParDepartment_Id = PCXD.ParDepartment_Id OR PEDC.ParDepartment_Id IS NULL) AND PEDC.IsActive = 1
                //        LEFT JOIN ParCompany PCN WITH (NOLOCK) on (PEDC.ParCompany_Id = PC.Id OR PEDC.ParCompany_Id IS NULL) AND PEDC.IsActive = 1
                //        LEFT JOIN ParCompanyXStructure PCXS WITH (NOLOCK) ON PC.Id = PCXS.ParCompany_Id AND PCXS.Active = 1
                //        LEFT JOIN ParStructure PS WITH (NOLOCK) ON PS.Id = PCXS.ParStructure_Id
                //        LEFT JOIN ParStructure PS1 WITH (NOLOCK) ON PS.ParStructureParent_Id = PS1.ID
                //        LEFT JOIN ParCluster PCL WITH (NOLOCK) on (PCL.Id = PEDC.ParCluster_Id OR PEDC.ParCluster_Id IS NULL)
                //        LEFT JOIN ParClusterGroup PCG WITH (NOLOCK) on PCG.Id = PCL.ParClusterGroup_Id
                //        WHERE 1 = 1
                //        AND PC.Name like '%{search}%'
                //        { sqlParDepartment }
                //        { whereClusterGroup }
                //        { whereCluster }
                //        { whereStructure }";

                var query = $@"--Query de seção
                SELECT DISTINCT TOP 500
                	Cargo.*
                FROM ParCargo Cargo
                INNER JOIN ParCargoXDepartment PCXD on PCXD.ParCargo_Id = Cargo.Id
                INNER JOIN ParDepartment Secao on PCXD.ParDepartment_Id = Secao.Id
                INNER JOIN (SELECT --*
                	DISTINCT
                		PVP.ParDepartment_Id AS Id
                	FROM ParVinculoPeso PVP WITH (NOLOCK)
                	INNER JOIN ParCluster PCL WITH (NOLOCK) ON PCL.Id = PVP.ParCluster_Id
                	INNER JOIN ParClusterGroup PCG WITH (NOLOCK) ON PCG.Id = PCL.ParClusterGroup_Id
                	INNER JOIN ParCompany PC WITH (NOLOCK) ON (PVP.ParCompany_Id = PC.Id OR PVP.ParCompany_Id IS NULL)
                	LEFT JOIN ParCompanyXStructure PCXS WITH (NOLOCK) ON PC.Id = PCXS.ParCompany_Id AND PCXS.Active = 1
                	LEFT JOIN ParStructure PS WITH (NOLOCK) ON PS.Id = PCXS.ParStructure_Id
                	LEFT JOIN ParStructure PS1 WITH (NOLOCK) ON PS.ParStructureParent_Id = PS1.Id
                	WHERE 1 = 1
                	AND PVP.IsActive = 1
                	{whereClusterGroup}
                	{whereCluster}
                    {whereStructure}
                	{sqlParCompany}
                	) PVP ON (PVP.Id = Secao.Id OR PVP.Id IS NULL) AND Secao.Active = 1
                INNER JOIN ParDepartment CentroCusto ON CentroCusto.Id = Secao.Parent_Id AND CentroCusto.Active = 1
                WHERE 1 = 1
                AND Secao.Active = 1
                {whereCentroCusto}
                {whereSecao}
                AND (CentroCusto.Parent_Id IS NULL OR CentroCusto.Parent_Id = 0)";

                var retorno = factory.SearchQuery<Select3ViewModel>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredParLevel1")]
        public List<Select3ViewModel> GetFilteredParLevel1(string search, [FromBody] DataCarrierFormularioNew form)
        {
            //Module

            //ParStructure
            var sqlParCompany = "";
            var whereClusterGroup = "";
            var whereStructure = "";
            var whereCluster = "";
            var whereCentroCusto = "";
            var whereSecao = "";
            var whereCargo = "";

            //Module

            //ParStructure
            if (form.ParStructure_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure_Ids) })";
            }

            if (form.ParStructure1_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure1_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure1_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure1_Ids) })";
            }

            //Grupo de empresa
            if (form.ParStructure2_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure2_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure2_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure2_Ids) })";
            }

            //Regional
            if (form.ParStructure3_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure3_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure3_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure3_Ids) })";
            }

            //Grupo de Cluster
            if (form.ParClusterGroup_Ids.Length > 0)
            {
                whereClusterGroup = $" AND PCG.Id IN ({ string.Join(",", form.ParClusterGroup_Ids) })";
            }

            //Cluster
            if (form.ParClusterGroup_Ids.Length > 0)
                whereCluster = $" AND PCL.Id IN ({ string.Join(",", form.ParCluster_Ids) })";

            if (form.ParCompany_Ids.Length > 0)
            {
                sqlParCompany = $@" AND PVP.ParCompany_Id in ({string.Join(",", form.ParCompany_Ids)}) ";
            }

            if (form.ParDepartment_Ids.Length > 0)
                whereCentroCusto = $@"AND CentroCusto.Id IN ({string.Join(",", form.ParDepartment_Ids)})";


            if (form.ParDepartment_Ids.Length > 0)
                whereSecao = $@"AND Secao.Id IN ({string.Join(",", form.ParSecao_Ids)})";

            if (form.ParCargo_Ids.Length > 0)
                whereCargo = $@" AND PVP.ParCargo_Id IN ({string.Join(",", form.ParCargo_Ids)})";

            using (var factory = new Factory("DefaultConnection"))
            {
                //var retornoFormulario = new FormularioViewModel();
                //retornoFormulario.ParDepartments = GetParDepartments(form, factory);
                //retornoFormulario.ParSecoes = GetParSecoes(form, factory, retornoFormulario);
                //retornoFormulario.ParCargos = GetParCargos(form, factory, form.ParSecao_Ids.Length > 0 ? form.ParSecao_Ids.ToList() : retornoFormulario.ParSecoes?.Select(x => x.Id).ToList());
                //var parDepartment_Ids = form.ParSecao_Ids.Length > 0 ? form.ParSecao_Ids.ToList() : retornoFormulario.ParSecoes?.Select(x => x.Id).ToList();

                //string sqlWhereFilter = "";

                //if (form.ParCompany_Ids.Length > 0 && parDepartment_Ids.Count > 0)
                //{
                //    sqlWhereFilter += $@"AND (PVP.ParDepartment_Id IN ({ string.Join(",", parDepartment_Ids)}) OR PVP.ParDepartment_Id IS NULL)";
                //}

                //var query = $@"SELECT DISTINCT TOP 500 PL1.ID, PL1.NAME 
                //                FROM parLevel1 PL1  
                //                LEFT JOIN ParVinculoPeso PVP WITH (NOLOCK) ON PVP.ParLevel1_Id = PL1.Id 
                //                LEFT JOIN ParCompany PC WITH (NOLOCK) on (PVP.ParCompany_Id = PC.Id OR PVP.ParCompany_Id IS NULL) AND PVP.IsActive = 1
                //                LEFT JOIN ParCompanyXStructure PCXS WITH (NOLOCK) ON PC.Id = PCXS.ParCompany_Id AND PCXS.Active = 1
                //                LEFT JOIN ParStructure PS WITH (NOLOCK) ON PS.Id = PCXS.ParStructure_Id
                //                LEFT JOIN ParStructure PS1 WITH (NOLOCK) ON PS.ParStructureParent_Id = PS1.ID
                //                LEFT JOIN ParCluster PCL WITH (NOLOCK) on (PCL.Id = PVP.ParCluster_Id OR PVP.ParCluster_Id IS NULL)
                //                LEFT JOIN ParClusterGroup PCG WITH (NOLOCK) on PCG.Id = PCL.ParClusterGroup_Id
                //                WHERE 1 = 1
                //                { sqlWhereFilter }
                //                { sqlParCompany }
                //                { whereClusterGroup }
                //                { whereStructure }
                //                { whereCluster }
                //                AND PL1.ISACTIVE = 1
                //                AND Pl1.Name like '%{search}%'";

                var query = $@"
                        SELECT DISTINCT TOP 500
                        	PL1.*
                        FROM ParLevel1 PL1
                        INNER JOIN (SELECT 
                        	DISTINCT
                        		PVP.*
                        	FROM ParVinculoPeso PVP WITH (NOLOCK)
                        	INNER JOIN ParCluster PCL WITH (NOLOCK) ON PCL.Id = PVP.ParCluster_Id
                        	INNER JOIN ParClusterGroup PCG WITH (NOLOCK) ON PCG.Id = PCL.ParClusterGroup_Id
                        	INNER JOIN ParCompany PC WITH (NOLOCK) ON (PVP.ParCompany_Id = PC.Id OR PVP.ParCompany_Id IS NULL)
                        	LEFT JOIN ParCompanyXStructure PCXS WITH (NOLOCK) ON PC.Id = PCXS.ParCompany_Id AND PCXS.Active = 1
                        	LEFT JOIN ParStructure PS WITH (NOLOCK) ON PS.Id = PCXS.ParStructure_Id
                        	LEFT JOIN ParStructure PS1 WITH (NOLOCK) ON PS.ParStructureParent_Id = PS1.Id
                        	WHERE 1 = 1
                        	AND PVP.IsActive = 1
                        	{whereClusterGroup}
                        	{whereCluster}
                            {whereStructure}
                        	{sqlParCompany}
                            {whereCargo}
                        	) PVP ON PVP.ParLevel1_Id = PL1.Id
                        INNER JOIN ParCargo Cargo WITH (NOLOCK) ON (PVP.ParCargo_Id = Cargo.Id OR PVP.ParCargo_Id IS NULL)
                        INNER JOIN ParDepartment Secao WITH (NOLOCK) ON (PVP.ParDepartment_Id = Secao.Id OR PVP.ParDepartment_Id IS NULL) AND Secao.Parent_Id IS NOT NULL AND Secao.Active = 1
                        INNER JOIN ParDepartment CentroCusto WITH (NOLOCK) ON CentroCusto.Id = Secao.Parent_Id AND CentroCusto.Active = 1 
                        WHERE 1 = 1
                        {whereCentroCusto}
                        {whereSecao}";

                var retorno = factory.SearchQuery<Select3ViewModel>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredParLevel2")]
        public List<Select3ViewModel> GetFilteredParLevel2(string search, [FromBody] DataCarrierFormularioNew form)
        {

            var sqlParCompany = "";
            var whereClusterGroup = "";
            var whereStructure = "";
            var whereCluster = "";
            var whereCentroCusto = "";
            var whereSecao = "";
            var whereCargo = "";
            var whereParLevel1 = "";

            //Module

            //ParStructure
            if (form.ParStructure_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure_Ids) })";
            }

            if (form.ParStructure1_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure1_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure1_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure1_Ids) })";
            }

            //Grupo de empresa
            if (form.ParStructure2_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure2_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure2_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure2_Ids) })";
            }

            //Regional
            if (form.ParStructure3_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure3_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure3_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure3_Ids) })";
            }

            //Grupo de Cluster
            if (form.ParClusterGroup_Ids.Length > 0)
            {
                whereClusterGroup = $" AND PCG.Id IN ({ string.Join(",", form.ParClusterGroup_Ids) })";
            }

            //Cluster
            if (form.ParClusterGroup_Ids.Length > 0)
                whereCluster = $" AND PCL.Id IN ({ string.Join(",", form.ParCluster_Ids) })";

            if (form.ParCompany_Ids.Length > 0)
            {
                sqlParCompany = $@" AND PVP.ParCompany_Id in ({string.Join(",", form.ParCompany_Ids)}) ";
            }

            if (form.ParDepartment_Ids.Length > 0)
                whereCentroCusto = $@"AND CentroCusto.Id IN ({string.Join(",", form.ParDepartment_Ids)})";


            if (form.ParDepartment_Ids.Length > 0)
                whereSecao = $@"AND Secao.Id IN ({string.Join(",", form.ParSecao_Ids)})";

            if(form.ParLevel1_Ids.Length > 0)
                whereParLevel1 = $@"AND PVP.ParLevel1_Id IN ({string.Join(",", form.ParLevel1_Ids)})";

            using (var factory = new Factory("DefaultConnection"))
            {
                //var retornoFormulario = new FormularioViewModel();
                //retornoFormulario.ParStructures = GetParStructure(form, factory);
                //retornoFormulario.ParCompanies = GetParCompanies(form, factory);
                //retornoFormulario.Shifts = GetShifts(factory);
                //retornoFormulario.ParDepartments = GetParDepartments(form, factory);
                //retornoFormulario.ParSecoes = GetParSecoes(form, factory, retornoFormulario);
                //retornoFormulario.ParCargos = GetParCargos(form, factory, form.ParSecao_Ids.Length > 0 ? form.ParSecao_Ids.ToList() : retornoFormulario.ParSecoes?.Select(x => x.Id).ToList());
                //retornoFormulario.ParLevel1s = GetParLevel1s(form, factory, form.ParSecao_Ids.Length > 0 ? form.ParSecao_Ids.ToList() : retornoFormulario.ParSecoes?.Select(x => x.Id).ToList());
                //var parLevel1_Ids = form.ParLevel1_Ids.Length > 0 ? form.ParLevel1_Ids.ToList() : retornoFormulario.ParLevel1s.Select(x => x.Id).ToList();


                //string sqlFilter = "";
                //if (parLevel1_Ids.Count > 0)
                //{
                //    sqlFilter = $@"  WHERE (PVP.ParLevel1_Id IN ({string.Join(",", parLevel1_Ids)}) OR PVP.ParLevel1_Id IS NULL)";
                //}

                //var query = $@"SELECT DISTINCT TOP 500 PL2.ID, PL2.NAME 
                //FROM parLevel2 PL2 WITH (NOLOCK) 
                //LEFT JOIN ParVinculoPeso PVP WITH (NOLOCK) ON PVP.ParLevel2_Id = PL2.Id
                //LEFT JOIN ParCompany PC WITH (NOLOCK) on (PVP.ParCompany_Id = PC.Id OR PVP.ParCompany_Id IS NULL) AND PVP.IsActive = 1
                //LEFT JOIN ParCompanyXStructure PCXS WITH (NOLOCK) ON PC.Id = PCXS.ParCompany_Id AND PCXS.Active = 1
                //LEFT JOIN ParStructure PS WITH (NOLOCK) ON PS.Id = PCXS.ParStructure_Id
                //LEFT JOIN ParStructure PS1 WITH (NOLOCK) ON PS.ParStructureParent_Id = PS1.ID
                //LEFT JOIN ParCluster PCL WITH (NOLOCK) on (PCL.Id = PVP.ParCluster_Id OR PVP.ParCluster_Id IS NULL)
                //LEFT JOIN ParClusterGroup PCG WITH (NOLOCK) on PCG.Id = PCL.ParClusterGroup_Id
                //{ sqlFilter }
                //{ sqlParCompany }
                //{ whereClusterGroup }
                //{ whereStructure }
                //{ whereCluster }

                //AND Pl2.Name like '%{search}%'";

                var query = $@"SELECT DISTINCT TOP 500
                        	PL2.*
                        FROM ParLevel2 PL2
                        INNER JOIN (SELECT 
                        	DISTINCT
                        		PVP.*
                        	FROM ParVinculoPeso PVP WITH (NOLOCK)
                        	INNER JOIN ParCluster PCL WITH (NOLOCK) ON PCL.Id = PVP.ParCluster_Id
                        	INNER JOIN ParClusterGroup PCG WITH (NOLOCK) ON PCG.Id = PCL.ParClusterGroup_Id
                        	INNER JOIN ParCompany PC WITH (NOLOCK) ON (PVP.ParCompany_Id = PC.Id OR PVP.ParCompany_Id IS NULL)
                        	LEFT JOIN ParCompanyXStructure PCXS WITH (NOLOCK) ON PC.Id = PCXS.ParCompany_Id AND PCXS.Active = 1
                        	LEFT JOIN ParStructure PS WITH (NOLOCK) ON PS.Id = PCXS.ParStructure_Id
                        	LEFT JOIN ParStructure PS1 WITH (NOLOCK) ON PS.ParStructureParent_Id = PS1.Id
                        	WHERE 1 = 1
                        	AND PVP.IsActive = 1
                        	{whereClusterGroup}
                        	{whereCluster}
                            {whereStructure}
                        	{sqlParCompany}
                            {whereCargo}
                            {whereParLevel1}
                        	) PVP ON PVP.ParLevel2_Id = PL2.Id
                        INNER JOIN ParCargo Cargo WITH (NOLOCK) ON (PVP.ParCargo_Id = Cargo.Id OR PVP.ParCargo_Id IS NULL)
                        INNER JOIN ParDepartment Secao WITH (NOLOCK) ON (PVP.ParDepartment_Id = Secao.Id OR PVP.ParDepartment_Id IS NULL) AND Secao.Parent_Id IS NOT NULL AND Secao.Active = 1
                        INNER JOIN ParDepartment CentroCusto WITH (NOLOCK) ON CentroCusto.Id = Secao.Parent_Id AND CentroCusto.Active = 1 
                        WHERE 1 = 1
                        {whereCentroCusto}
                        {whereSecao}";

                var retorno = factory.SearchQuery<Select3ViewModel>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredParLevel3")]
        public List<Select3ViewModel> GetFilteredParLevel3(string search, [FromBody] DataCarrierFormularioNew form)
        {
            var sqlParCompany = "";
            var whereClusterGroup = "";
            var whereStructure = "";
            var whereCluster = "";
            var whereCentroCusto = "";
            var whereSecao = "";
            var whereCargo = "";
            var whereParLevel1 = "";
            var whereParLevel2 = "";

            //Module

            //ParStructure
            if (form.ParStructure_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure_Ids) })";
            }

            if (form.ParStructure1_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure1_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure1_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure1_Ids) })";
            }

            //Grupo de empresa
            if (form.ParStructure2_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure2_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure2_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure2_Ids) })";
            }

            //Regional
            if (form.ParStructure3_Ids.Length > 0)
            {
                whereStructure = $@" AND PCXS.Id IN ({ string.Join(",", form.ParStructure3_Ids) }) 
                 OR  PS.Id IN ({ string.Join(",", form.ParStructure3_Ids) }) 
                 OR  PS1.Id IN ({ string.Join(",", form.ParStructure3_Ids) })";
            }

            //Grupo de Cluster
            if (form.ParClusterGroup_Ids.Length > 0)
            {
                whereClusterGroup = $" AND PCG.Id IN ({ string.Join(",", form.ParClusterGroup_Ids) })";
            }

            //Cluster
            if (form.ParClusterGroup_Ids.Length > 0)
                whereCluster = $" AND PCL.Id IN ({ string.Join(",", form.ParCluster_Ids) })";

            if (form.ParCompany_Ids.Length > 0)
            {
                sqlParCompany = $@" AND PVP.ParCompany_Id in ({string.Join(",", form.ParCompany_Ids)}) ";
            }

            if (form.ParDepartment_Ids.Length > 0)
                whereCentroCusto = $@"AND CentroCusto.Id IN ({string.Join(",", form.ParDepartment_Ids)})";


            if (form.ParDepartment_Ids.Length > 0)
                whereSecao = $@"AND Secao.Id IN ({string.Join(",", form.ParSecao_Ids)})";


            if (form.ParCargo_Ids.Length > 0)
                whereCargo = $@" AND PVP.ParCargo_Id IN ({string.Join(",", form.ParCargo_Ids)})";

            if (form.ParLevel1_Ids.Length > 0)
                whereParLevel1 = $@"AND PVP.ParLevel1_Id IN ({string.Join(",", form.ParLevel1_Ids)})";

            if (form.ParLevel1_Ids.Length > 0)
                whereParLevel2 = $@"AND PVP.ParLevel2_Id IN ({string.Join(",", form.ParLevel2_Ids)})";

            using (var factory = new Factory("DefaultConnection"))
            {
                //var retornoFormulario = new FormularioViewModel();
                //retornoFormulario.ParStructures = GetParStructure(form, factory);
                //retornoFormulario.ParCompanies = GetParCompanies(form, factory);
                //retornoFormulario.Shifts = GetShifts(factory);
                //retornoFormulario.ParDepartments = GetParDepartments(form, factory);
                //retornoFormulario.ParSecoes = GetParSecoes(form, factory, retornoFormulario);
                //retornoFormulario.ParCargos = GetParCargos(form, factory, form.ParSecao_Ids.Length > 0 ? form.ParSecao_Ids.ToList() : retornoFormulario.ParSecoes?.Select(x => x.Id).ToList());
                //retornoFormulario.ParLevel1s = GetParLevel1s(form, factory, form.ParSecao_Ids.Length > 0 ? form.ParSecao_Ids.ToList() : retornoFormulario.ParSecoes?.Select(x => x.Id).ToList());
                //var parLevel1_Ids = form.ParLevel1_Ids.Length > 0 ? form.ParLevel1_Ids.ToList() : retornoFormulario.ParLevel1s.Select(x => x.Id).ToList();
                //retornoFormulario.ParLevel2s = GetParLevel2s(form, factory, parLevel1_Ids);
                //var parLevel2_Ids = form.ParLevel2_Ids.Length > 0 ? form.ParLevel2_Ids.ToList() : retornoFormulario.ParLevel2s.Select(x => x.Id).ToList();

                //string sqlFilter = "";
                //if (parLevel1_Ids.Count > 0 || parLevel2_Ids.Count > 0)
                //{
                //    sqlFilter = $@"";
                //    if (parLevel1_Ids.Count > 0)
                //    {
                //        sqlFilter += $@" AND (PVP.ParLevel1_Id IN ({ string.Join(",", parLevel1_Ids)}) OR PVP.ParLevel1_Id IS NULL) ";
                //    }
                //    if (parLevel2_Ids.Count > 0)
                //    {
                //        sqlFilter += $@" AND (PVP.ParLevel2_Id IN ({ string.Join(",", parLevel2_Ids)}) OR PVP.ParLevel2_Id IS NULL) ";
                //    }
                //}

                //var query = $@"SELECT DISTINCT TOP 500 PL3.ID, PL3.NAME 
                //FROM parLevel3 PL3 WITH (NOLOCK) 
                //LEFT JOIN ParVinculoPeso PVP WITH (NOLOCK) ON PVP.ParLevel3_Id = PL3.Id
                //LEFT JOIN ParCompany PC WITH (NOLOCK) on (PVP.ParCompany_Id = PC.Id OR PVP.ParCompany_Id IS NULL) AND PVP.IsActive = 1
                //LEFT JOIN ParCompanyXStructure PCXS WITH (NOLOCK) ON PC.Id = PCXS.ParCompany_Id AND PCXS.Active = 1
                //LEFT JOIN ParStructure PS WITH (NOLOCK) ON PS.Id = PCXS.ParStructure_Id
                //LEFT JOIN ParStructure PS1 WITH (NOLOCK) ON PS.ParStructureParent_Id = PS1.ID
                //LEFT JOIN ParCluster PCL WITH (NOLOCK) on (PCL.Id = PVP.ParCluster_Id OR PVP.ParCluster_Id IS NULL)
                //LEFT JOIN ParClusterGroup PCG WITH (NOLOCK) on PCG.Id = PCL.ParClusterGroup_Id
                //WHERE 1 = 1
                //{ sqlFilter }
                //{ sqlParCompany }
                //{ whereClusterGroup }
                //{ whereStructure }
                //{ whereCluster }
                //AND Pl3.Name like '%{search}%'";

                var query = $@"
                        SELECT DISTINCT TOP 500
                        	PL3.*
                        FROM ParLevel3 PL3
                        INNER JOIN (SELECT 
                        	DISTINCT
                        		PVP.*
                        	FROM ParVinculoPeso PVP WITH (NOLOCK)
                        	INNER JOIN ParCluster PCL WITH (NOLOCK) ON PCL.Id = PVP.ParCluster_Id
                        	INNER JOIN ParClusterGroup PCG WITH (NOLOCK) ON PCG.Id = PCL.ParClusterGroup_Id
                        	INNER JOIN ParCompany PC WITH (NOLOCK) ON (PVP.ParCompany_Id = PC.Id OR PVP.ParCompany_Id IS NULL)
                        	LEFT JOIN ParCompanyXStructure PCXS WITH (NOLOCK) ON PC.Id = PCXS.ParCompany_Id AND PCXS.Active = 1
                        	LEFT JOIN ParStructure PS WITH (NOLOCK) ON PS.Id = PCXS.ParStructure_Id
                        	LEFT JOIN ParStructure PS1 WITH (NOLOCK) ON PS.ParStructureParent_Id = PS1.Id
                        	WHERE 1 = 1
                        	AND PVP.IsActive = 1
                        	{whereClusterGroup}
                        	{whereCluster}
                            {whereStructure}
                        	{sqlParCompany}
                            {whereCargo}
                            {whereParLevel1}
                            {whereParLevel2}
                        	) PVP ON PVP.ParLevel3_Id = PL3.Id
                        INNER JOIN ParCargo Cargo WITH (NOLOCK) ON (PVP.ParCargo_Id = Cargo.Id OR PVP.ParCargo_Id IS NULL)
                        INNER JOIN ParDepartment Secao WITH (NOLOCK) ON (PVP.ParDepartment_Id = Secao.Id OR PVP.ParDepartment_Id IS NULL) AND Secao.Parent_Id IS NOT NULL AND Secao.Active = 1
                        INNER JOIN ParDepartment CentroCusto WITH (NOLOCK) ON CentroCusto.Id = Secao.Parent_Id AND CentroCusto.Active = 1 
                        WHERE 1 = 1
                        {whereCentroCusto}
                        {whereSecao}";

                var retorno = factory.SearchQuery<Select3ViewModel>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredParStructure")]
        public List<Select3ViewModel> GetFilteredParStructure(string search, [FromBody] DataCarrierFormularioNew form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {
                var wSG = "";

                if (form.ParStructureGroup_Ids.Length > 0)
                {
                    wSG += "AND ParStructureGroup_Id IN(" + string.Join(",", form.ParStructureGroup_Ids) + ") ";
                }

                var query = $@"
                    SELECT DISTINCT TOP 500 ID, Name FROM ParStructure WITH (NOLOCK)
                    WHERE 1=1
                    AND Name like '%{search}%'
                    {wSG}";

                var retorno = factory.SearchQuery<Select3ViewModel>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredParStructure1")]
        public List<Select3ViewModel> GetFilteredParStructure1(string search, [FromBody] DataCarrierFormularioNew form)
        {

            var whereStructParent = "";

            if (form.ParStructure2_Ids.Length > 0)
                whereStructParent = $" AND Structure2.Id IN ({string.Join(",", form.ParStructure2_Ids)})";

            if (form.ParStructure3_Ids.Length > 0)
                whereStructParent += $" AND Structure3.Id IN ({string.Join(",", form.ParStructure3_Ids)})";


            using (var factory = new Factory("DefaultConnection"))
            {

                var query = $@"
                            SELECT DISTINCT Structure1.* FROM (SELECT * FROM ParStructure WITH (NOLOCK) where ParStructureGroup_Id = 1) Structure1 
                            LEFT JOIN (SELECT * FROM ParStructure WITH (NOLOCK) where ParStructureGroup_Id = 2) Structure2 on Structure1.Id = Structure2.ParStructureParent_Id
                            LEFT JOIN (SELECT * FROM ParStructure WITH (NOLOCK) where ParStructureGroup_Id = 3) Structure3 on Structure2.Id = Structure3.ParStructureParent_Id
                            WHERE Structure1.ParStructureGroup_Id = 1 
                            {whereStructParent}
                            AND Structure1.Name like '%{search}%'";

                var retorno = factory.SearchQuery<Select3ViewModel>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredParStructure2")]
        public List<Select3ViewModel> GetFilteredParStructure2(string search, [FromBody] DataCarrierFormularioNew form)
        {
            var whereStructParent = "";

            if (form.ParStructure1_Ids.Length > 0)
                whereStructParent = $" AND Structure1.Id IN ({string.Join(",", form.ParStructure1_Ids)})";

            if (form.ParStructure3_Ids.Length > 0)
                whereStructParent += $" AND Structure3.Id IN ({string.Join(",", form.ParStructure3_Ids)})";

            using (var factory = new Factory("DefaultConnection"))
            {

                var query = $@"
                            SELECT DISTINCT Structure2.* FROM (SELECT * FROM ParStructure WITH (NOLOCK) where ParStructureGroup_Id = 1) Structure1 
                            LEFT JOIN (SELECT * FROM ParStructure WITH (NOLOCK) where ParStructureGroup_Id = 2) Structure2 on Structure1.Id = Structure2.ParStructureParent_Id
                            LEFT JOIN (SELECT * FROM ParStructure WITH (NOLOCK) where ParStructureGroup_Id = 3) Structure3 on Structure2.Id = Structure3.ParStructureParent_Id
                            WHERE Structure2.ParStructureGroup_Id = 2
                            {whereStructParent}
                            AND Structure1.Name like '%{search}%'";

                var retorno = factory.SearchQuery<Select3ViewModel>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredParStructure3")]
        public List<Select3ViewModel> GetFilteredParStructure3(string search, [FromBody] DataCarrierFormularioNew form)
        {
            var whereStructParent = "";

            if (form.ParStructure1_Ids.Length > 0)
                whereStructParent = $" AND Structure1.Id IN ({string.Join(",", form.ParStructure1_Ids)})";

            if (form.ParStructure2_Ids.Length > 0)
                whereStructParent += $" AND Structure2.Id IN ({string.Join(",", form.ParStructure2_Ids)})";


            using (var factory = new Factory("DefaultConnection"))
            {

                var query = $@"
                            SELECT DISTINCT Structure3.* FROM (SELECT * FROM ParStructure WITH (NOLOCK) where ParStructureGroup_Id = 1) Structure1 
                            LEFT JOIN (SELECT * FROM ParStructure WITH (NOLOCK) where ParStructureGroup_Id = 2) Structure2 on Structure1.Id = Structure2.ParStructureParent_Id
                            LEFT JOIN (SELECT * FROM ParStructure WITH (NOLOCK) where ParStructureGroup_Id = 3) Structure3 on Structure2.Id = Structure3.ParStructureParent_Id
                            WHERE Structure3.ParStructureGroup_Id = 3
                            {whereStructParent}
                            AND Structure1.Name like '%{search}%'";

                var retorno = factory.SearchQuery<Select3ViewModel>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredParStructureGroup")]
        public List<Select3ViewModel> GetFilteredParStructureGroup(string search, [FromBody] DataCarrierFormularioNew form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {
                var query = $@"SELECT DISTINCT TOP 500 ID, Name FROM ParStructureGroup WITH (NOLOCK)
                    WHERE Name like '%{search}%'";

                var retorno = factory.SearchQuery<Select3ViewModel>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredParModule")]
        public List<ParModule> GetFilteredParModule(string search, [FromBody] DataCarrierFormularioNew form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {
                var query = $@"SELECT DISTINCT TOP 500 Id, Name FROM ParModule WITH (NOLOCK)
                    WHERE IsActive = 1 AND Name like '%{search}%'";

                var retorno = factory.SearchQuery<ParModule>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredPeso")]
        public List<Peso> GetFilteredPeso(string search, [FromBody] DataCarrierFormularioNew form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {
                var query = $@"SELECT '' AS Id, 'Todos' as Name
                            UNION ALL
                            SELECT DISTINCT TOP 500 
                            cast(CONVERT(DECIMAL(18,2), Weight) as varchar(255)) AS Id, 
                            cast(CONVERT(DECIMAL(18,2), Weight) as varchar(255)) AS Name
                            FROM parlevel3level2 WITH (NOLOCK) WHERE weight like '%{search}%' AND IsActive = 1 order by Id asc ";

                var retorno = factory.SearchQuery<Peso>(query).ToList();

                return retorno;
            }
        }


        [HttpPost]
        [Route("GetFilteredUserSgqSurpervisor")]
        public List<UserSgq> GetFilteredUserSgqSurpervisor(string search, [FromBody] DataCarrierFormularioNew form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {
                var query = $@"SELECT DISTINCT TOP 500 Id, Name from UserSgq WITH (NOLOCK) Where role like '%Supervisor%' AND Name like '%{search}%'";

                var retorno = factory.SearchQuery<UserSgq>(query).ToList();

                return retorno;
            }
        }


        [HttpPost]
        [Route("GetFilteredUserSgqByCompany")]
        public List<UserSgq> GetFilteredUserSgqByCompany(string search, [FromBody] DataCarrierFormularioNew form)
        {

            SgqDbDevEntities db = new SgqDbDevEntities();
            var usuarios_Ids = db.ParCompanyXUserSgq.Where(u => u.ParCompany_Id == form.ParCompany_Ids.FirstOrDefault()).Select(x => x.UserSgq_Id).ToList();
            var query = "";

            using (var factory = new Factory("DefaultConnection"))
            {
                if (usuarios_Ids.Count > 0)
                    query = $@"SELECT DISTINCT TOP 500 Id, Name from UserSgq WITH (NOLOCK) Where Name like '%{search}%' AND ParCompany_Id IN(" + string.Join(",", usuarios_Ids) + ")";
                else
                    query = $@"SELECT DISTINCT TOP 500 Id, Name from UserSgq WITH (NOLOCK) Where Name like '%{search}%'";

                var retorno = factory.SearchQuery<UserSgq>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetForm")]
        public FormularioViewModel GetForm([FromBody] DataCarrierFormularioNew form)
        {
            var retornoFormulario = new FormularioViewModel();

            using (var factory = new Factory("DefaultConnection"))
            {
                retornoFormulario.ParStructures = GetParStructure(form, factory);
                retornoFormulario.ParStructuresRegional = GetParStructureRegional(form, factory);
                retornoFormulario.ParCompanies = GetParCompanies(form, factory);
                retornoFormulario.Shifts = GetShifts(factory);
                retornoFormulario.ParDepartments = GetParDepartments(form, factory);
                retornoFormulario.ParSecoes = GetParSecoes(form, factory, retornoFormulario);
                retornoFormulario.ParGroupParLevel1s = GetParGroupParLevel1s(form, factory);
                retornoFormulario.ParCargos = GetParCargos(form, factory, form.ParSecao_Ids.Length > 0 ? form.ParSecao_Ids.ToList() : retornoFormulario.ParSecoes?.Select(x => x.Id).ToList());
                retornoFormulario.ParLevel1s = GetParLevel1s(form, factory, form.ParSecao_Ids.Length > 0 ? form.ParSecao_Ids.ToList() : retornoFormulario.ParSecoes?.Select(x => x.Id).ToList());
                var listaParLevel1_Ids = form.ParLevel1_Ids.Length > 0 ? form.ParLevel1_Ids.ToList() : retornoFormulario.ParLevel1s.Select(x => x.Id).ToList();
                retornoFormulario.ParLevel2s = GetParLevel2s(form, factory, listaParLevel1_Ids);
                var listaParLevel2_Ids = form.ParLevel2_Ids.Length > 0 ? form.ParLevel2_Ids.ToList() : retornoFormulario.ParLevel2s.Select(x => x.Id).ToList();
                retornoFormulario.ParLevel3s = GetParLevel3s(form, factory, listaParLevel1_Ids, listaParLevel2_Ids);

            }

            return retornoFormulario;

        }

        private List<ParStructure> GetParStructure(DataCarrierFormularioNew form, Factory factory)
        {
            var sql = "Select Id, Name from ParStructure WITH (NOLOCK) where Active = 1";

            var retorno = factory.SearchQuery<ParStructure>(sql).ToList();

            return retorno;
        }

        private List<ParStructure> GetParStructureRegional(DataCarrierFormularioNew form, Factory factory)
        {
            var sql = "Select Id, Name from ParStructure WITH (NOLOCK) where ParStructureGroup_Id = 2 AND Active = 1";

            var retorno = factory.SearchQuery<ParStructure>(sql).ToList();

            return retorno;
        }

        private List<ParGroupParLevel1> GetParGroupParLevel1s(DataCarrierFormularioNew form, Factory factory)
        {
            var sql = "Select Id, Name from ParGroupParLevel1 WITH (NOLOCK) where AND Active = 1";

            var retorno = factory.SearchQuery<ParGroupParLevel1>(sql).ToList();

            return retorno;
        }

        private List<ParCompany> GetParCompanies(DataCarrierFormularioNew form, Factory factory)
        {

            var filtroStructure = form.ParStructure_Ids.Length > 0 ? $@"AND PCXS.Id IN ({ string.Join(",", form.ParStructure_Ids) })" : "";

            var query = $@"SELECT
                        	PC.Id, PC.Name
                        FROM ParCompany PC WITH (NOLOCK)
                        LEFT JOIN ParCompanyXStructure PCXS WITH (NOLOCK) ON PC.Id = PCXS.ParCompany_Id AND PCXS.Active = 1
                        WHERE 1 = 1
                        AND PC.IsActive = 1
                        --Filtros
                        {filtroStructure}";

            var retorno = factory.SearchQuery<ParCompany>(query).ToList();

            return retorno;
        }

        private List<Shift> GetShifts(Factory factory)
        {
            var query = "SELECT * FROM shift";

            var retorno = factory.SearchQuery<Shift>(query).ToList();

            return retorno;
        }

        private List<ParDepartment> GetParDepartments(DataCarrierFormularioNew form, Factory factory)
        {
            var sqlParCompany = "";
            if (form.ParCompany_Ids.Length > 0)
            {
                sqlParCompany = $@" AND PD.ParCompany_Id in ({string.Join(",", form.ParCompany_Ids)}) ";
            }

            var query = $@"SELECT Distinct PD.Id,PD.Name FROM ParDepartment PD WITH (NOLOCK)
                            WHERE 1=1 
                            AND PD.Active = 1 
                            AND (PD.Parent_Id IS NULL OR PD.Parent_Id = 0) " + sqlParCompany;

            var retorno = factory.SearchQuery<ParDepartment>(query).ToList();

            return retorno;
        }

        private List<ParDepartment> GetParSecoes(DataCarrierFormularioNew form, Factory factory, FormularioViewModel retornoFormulario)
        {
            string sqlParDepartment = "";
            if (form.ParCompany_Ids.Length > 0 && retornoFormulario.ParDepartments.Count > 0)
            {
                var sqlDepartamentoPelaHash = "";
                foreach (var item in retornoFormulario.ParDepartments)
                {
                    sqlDepartamentoPelaHash += $@"OR PD.Hash like '{item.Id}|%'
                            OR PD.Hash like '%|{item.Id}|%'
                            OR PD.Hash = '{item.Id}'";
                }
                sqlParDepartment = $@" AND (PD.Id in ({string.Join(",", retornoFormulario.ParDepartments.Select(x => x.Id))}) 
                             {sqlDepartamentoPelaHash})";
            }

            var query = $@"SELECT Distinct PD.Id,PD.Name  FROM ParDepartment PD WITH (NOLOCK)
                WHERE 1=1 
                AND PD.Active = 1 
                AND PD.Parent_Id IS NOT NULL " + sqlParDepartment;

            var retorno = factory.SearchQuery<ParDepartment>(query).ToList();

            return retorno;
        }

        private List<ParCargo> GetParCargos(DataCarrierFormularioNew form, Factory factory, List<int> parDepartment_Ids)
        {
            var sqlParDepartment = "";
            if (parDepartment_Ids.Count > 0)
            {
                sqlParDepartment = $@" AND PCXD.ParDepartment_Id IN ({string.Join(",", parDepartment_Ids)})";
            }

            var query = $@"SELECT Distinct PC.Id,PC.Name FROM ParCargo PC WITH (NOLOCK)
                        LEFT JOIN ParCargoXDepartment PCXD WITH (NOLOCK) ON PCXD.ParCargo_Id = PC.Id
                        WHERE 1 = 1
                        {sqlParDepartment}";

            var retorno = factory.SearchQuery<ParCargo>(query).ToList();

            return retorno;
        }

        private List<ParLevel1> GetParLevel1s(DataCarrierFormularioNew form, Factory factory, List<int> parDepartment_Ids)
        {

            string sqlFilter = "";
            if (form.ParCompany_Ids.Length > 0 && parDepartment_Ids.Count > 0)
            {
                sqlFilter = $@" LEFT JOIN ParVinculoPeso PVP WITH (NOLOCK) ON PVP.ParLevel1_Id = PL1.Id WHERE 1 = 1 ";
                sqlFilter += $@"AND (PVP.ParDepartment_Id IN ({ string.Join(",", parDepartment_Ids)})";
                sqlFilter += $@"OR PVP.ParDepartment_Id IS NULL) ";
            }
            var query = "SELECT DISTINCT PL1.ID, PL1.NAME FROM parLevel1 PL1 WITH (NOLOCK)" + sqlFilter;

            var retorno = factory.SearchQuery<ParLevel1>(query).ToList();

            return retorno;
        }

        private List<ParLevel2> GetParLevel2s(DataCarrierFormularioNew form, Factory factory, List<int> parLevel1_Ids)
        {
            string sqlFilter = "";
            if (parLevel1_Ids.Count > 0)
            {
                sqlFilter = $@" LEFT JOIN ParVinculoPeso PVP WITH (NOLOCK) ON PVP.ParLevel2_Id = PL2.Id
                                WHERE PVP.ParLevel1_Id IN ({string.Join(",", parLevel1_Ids)})";
            }

            var query = "SELECT DISTINCT PL2.ID, PL2.NAME FROM parLevel2 PL2 WITH (NOLOCK)" + sqlFilter;

            var retorno = factory.SearchQuery<ParLevel2>(query).ToList();

            return retorno;
        }

        private List<ParLevel3> GetParLevel3s(DataCarrierFormularioNew form, Factory factory, List<int> parLevel1_Ids, List<int> parLevel2_Ids)
        {

            string sqlFilter = "";
            if (parLevel1_Ids.Count > 0 || parLevel2_Ids.Count > 0)
            {
                sqlFilter = $@" LEFT JOIN ParVinculoPeso PVP WITH (NOLOCK) ON PVP.ParLevel2_Id = PL3.Id WHERE 1 = 1 ";
                if (parLevel1_Ids.Count > 0)
                {
                    sqlFilter += $@"AND PVP.ParLevel1_Id IN ({ string.Join(",", parLevel1_Ids)})";
                }
                if (parLevel2_Ids.Count > 0)
                {
                    sqlFilter += $@"AND PVP.ParLevel2_Id IN ({ string.Join(",", parLevel2_Ids)})";
                }
            }

            var query = "SELECT DISTINCT PL3.ID, PL3.NAME FROM parLevel3 PL3 WITH (NOLOCK) " + sqlFilter;

            var retorno = factory.SearchQuery<ParLevel3>(query).ToList();

            return retorno;
        }

        public class Peso
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
    }
}
