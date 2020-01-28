﻿using ADOFactory;
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

            using (var factory = new Factory("DefaultConnection"))
            {
                var filtroStructure =
                                   form.ParStructure_Ids.Length > 0
                                 ? $@"  AND PCXS.Id IN ({ string.Join(",", form.ParStructure_Ids) }) 
                                        OR  PS.Id IN ({ string.Join(",", form.ParStructure_Ids) }) 
                                        OR  PS1.Id IN ({ string.Join(",", form.ParStructure_Ids) })" : "";

                var query = $@"SELECT DISTINCT TOP 500
                        	PC.Id, PC.Name
                        FROM ParCompany PC
                        LEFT JOIN ParCompanyXStructure PCXS ON PC.Id = PCXS.ParCompany_Id AND PCXS.Active = 1
						LEFT JOIN ParStructure PS ON PS.Id = PCXS.ParStructure_Id
						LEFT JOIN ParStructure PS1 ON PS.ParStructureParent_Id = PS1.ID
                        WHERE 1 = 1
                        AND PC.IsActive = 1
                        --Filtros
                        AND PC.Id IN ({ GetUserUnitsIds(form.ShowUserCompanies) })
                        AND PC.Name like '%{search}%'
                        {filtroStructure}";

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
                        FROM ParCriticalLevel PCL
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
                        	Id, Name from ParGroupParLevel1 Where IsActive = 1 AND Name like '%{search}%'";

                var retorno = factory.SearchQuery<ParGroupParLevel1>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredParClusters")]
        public List<Select3ViewModel> GetFilteredParClusters(string search, [FromBody] DataCarrierFormularioNew form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {

                var query = $@"SELECT DISTINCT TOP 500
                        	PCL.Id, PCL.Name
                        FROM ParCluster PCL
						WHERE 1 = 1
                        AND PCL.IsActive = 1
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
                        FROM ParClusterGroup PCLG
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
                var query = $@"SELECT DISTINCT TOP 500 ID, Description as Name FROM shift
                        WHERE Description like '%{search}%'";

                var retorno = factory.SearchQuery<Select3ViewModel>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredParDepartment")]
        public List<Select3ViewModel> GetFilteredParDepartment(string search, [FromBody] DataCarrierFormularioNew form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {
                var sqlParCompany = "";
                if (form.ParCompany_Ids.Length > 0)
                {
                    sqlParCompany = $@" AND PD.ParCompany_Id in ({string.Join(",", form.ParCompany_Ids)}) ";
                }

                var query = $@"SELECT DISTINCT TOP 500 PD.Id,PD.Name FROM ParDepartment PD 
                                WHERE 1=1 
                                AND PD.Active = 1 
                                AND PD.Name like '%{search}%'
                                AND PD.ParCompany_Id IN ({GetUserUnitsIds(form.ShowUserCompanies)})
                                AND (PD.Parent_Id IS NULL OR PD.Parent_Id = 0) " + sqlParCompany;

                var retorno = factory.SearchQuery<Select3ViewModel>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredParDepartmentFilho")]
        public List<Select3ViewModel> GetFilteredParDepartmentFilho(string search, [FromBody] DataCarrierFormularioNew form)
        {

            using (var factory = new Factory("DefaultConnection"))
            {
                var retornoFormulario = new FormularioViewModel();
                retornoFormulario.ParDepartments = GetParDepartments(form, factory);

                var sqlDepartamentoPelaHash = "";
                if (form.ParDepartment_Ids.Length > 0 && retornoFormulario.ParDepartments.Count > 0)
                {
                    sqlDepartamentoPelaHash += $@"AND PD.Hash in ({string.Join(",", form.ParDepartment_Ids)})
                            AND (PD.Parent_Id IS not NULL OR PD.Parent_Id <> 0)  ";
                }

                if (form.ParCompany_Ids != null && form.ParCompany_Ids.Length > 0)
                {
                    sqlDepartamentoPelaHash += $@"AND ParCompany_Id in ({string.Join(",", form.ParCompany_Ids)})";
                }

                var query = $@"SELECT DISTINCT TOP 500 PD.Hash,PD.Id,PD.Name  FROM ParDepartment PD 
                WHERE 1=1 
                AND PD.Active = 1 
                AND PD.Name like '%{search}%'
                AND PD.ParCompany_Id IN ({GetUserUnitsIds(form.ShowUserCompanies)})
                AND PD.Parent_Id IS NOT NULL " + sqlDepartamentoPelaHash;

                var retorno = factory.SearchQuery<Select3ViewModel>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredParCargo")]
        public List<Select3ViewModel> GetFilteredParCargo(string search, [FromBody] DataCarrierFormularioNew form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {
                var retornoFormulario = new FormularioViewModel();
                retornoFormulario.ParDepartments = GetParDepartments(form, factory);
                retornoFormulario.ParSecoes = GetParSecoes(form, factory, retornoFormulario);
                var parDepartment_Ids = form.ParSecao_Ids.Length > 0 ? form.ParSecao_Ids.ToList() : retornoFormulario.ParSecoes?.Select(x => x.Id).ToList();



                var sqlParDepartment = "";
                if (form.ParCompany_Ids.Length > 0 && parDepartment_Ids.Count > 0)
                {
                    sqlParDepartment = $@" AND PCXD.ParDepartment_Id IN ({string.Join(",", parDepartment_Ids)})";
                }

                var query = $@"SELECT DISTINCT TOP 500 PC.Id,PC.Name FROM ParCargo PC
                        LEFT JOIN ParCargoXDepartment PCXD ON PCXD.ParCargo_Id = PC.Id
                        WHERE 1 = 1
                        AND PC.Name like '%{search}%'
                        {sqlParDepartment}";

                var retorno = factory.SearchQuery<Select3ViewModel>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredParLevel1")]
        public List<Select3ViewModel> GetFilteredParLevel1(string search, [FromBody] DataCarrierFormularioNew form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {
                var retornoFormulario = new FormularioViewModel();
                retornoFormulario.ParDepartments = GetParDepartments(form, factory);
                retornoFormulario.ParSecoes = GetParSecoes(form, factory, retornoFormulario);
                retornoFormulario.ParCargos = GetParCargos(form, factory, form.ParSecao_Ids.Length > 0 ? form.ParSecao_Ids.ToList() : retornoFormulario.ParSecoes?.Select(x => x.Id).ToList());
                var parDepartment_Ids = form.ParSecao_Ids.Length > 0 ? form.ParSecao_Ids.ToList() : retornoFormulario.ParSecoes?.Select(x => x.Id).ToList();

                string sqlFilter = "";
                string sqlWhereFilter = "";
                if (form.ParCompany_Ids.Length > 0 && parDepartment_Ids.Count > 0)
                {
                    sqlFilter = $@" LEFT JOIN ParVinculoPeso PVP ON PVP.ParLevel1_Id = PL1.Id ";
                    sqlWhereFilter += $@"AND (PVP.ParDepartment_Id IN ({ string.Join(",", parDepartment_Ids)}) OR PVP.ParDepartment_Id IS NULL)";
                }
                var query = $@"SELECT DISTINCT TOP 500 PL1.ID, PL1.NAME 
                                FROM parLevel1 PL1 
                                {sqlFilter}
                                WHERE 1 = 1
                                {sqlWhereFilter}
                                AND PL1.ISACTIVE = 1
                                AND Pl1.Name like '%{search}%'";

                var retorno = factory.SearchQuery<Select3ViewModel>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredParLevel2")]
        public List<Select3ViewModel> GetFilteredParLevel2(string search, [FromBody] DataCarrierFormularioNew form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {
                var retornoFormulario = new FormularioViewModel();
                retornoFormulario.ParStructures = GetParStructure(form, factory);
                retornoFormulario.ParCompanies = GetParCompanies(form, factory);
                retornoFormulario.Shifts = GetShifts(factory);
                retornoFormulario.ParDepartments = GetParDepartments(form, factory);
                retornoFormulario.ParSecoes = GetParSecoes(form, factory, retornoFormulario);
                retornoFormulario.ParCargos = GetParCargos(form, factory, form.ParSecao_Ids.Length > 0 ? form.ParSecao_Ids.ToList() : retornoFormulario.ParSecoes?.Select(x => x.Id).ToList());
                retornoFormulario.ParLevel1s = GetParLevel1s(form, factory, form.ParSecao_Ids.Length > 0 ? form.ParSecao_Ids.ToList() : retornoFormulario.ParSecoes?.Select(x => x.Id).ToList());
                var parLevel1_Ids = form.ParLevel1_Ids.Length > 0 ? form.ParLevel1_Ids.ToList() : retornoFormulario.ParLevel1s.Select(x => x.Id).ToList();


                string sqlFilter = "";
                if (parLevel1_Ids.Count > 0)
                {
                    sqlFilter = $@" LEFT JOIN ParVinculoPeso PVP ON PVP.ParLevel2_Id = PL2.Id
                                WHERE PVP.ParLevel1_Id IN ({string.Join(",", parLevel1_Ids)}) OR PVP.ParLevel1_Id IS NULL";
                }

                var query = $@"SELECT DISTINCT TOP 500 PL2.ID, PL2.NAME FROM parLevel2 PL2 {sqlFilter}
                AND Pl2.Name like '%{search}%'";

                var retorno = factory.SearchQuery<Select3ViewModel>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredParLevel3")]
        public List<Select3ViewModel> GetFilteredParLevel3(string search, [FromBody] DataCarrierFormularioNew form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {
                var retornoFormulario = new FormularioViewModel();
                retornoFormulario.ParStructures = GetParStructure(form, factory);
                retornoFormulario.ParCompanies = GetParCompanies(form, factory);
                retornoFormulario.Shifts = GetShifts(factory);
                retornoFormulario.ParDepartments = GetParDepartments(form, factory);
                retornoFormulario.ParSecoes = GetParSecoes(form, factory, retornoFormulario);
                retornoFormulario.ParCargos = GetParCargos(form, factory, form.ParSecao_Ids.Length > 0 ? form.ParSecao_Ids.ToList() : retornoFormulario.ParSecoes?.Select(x => x.Id).ToList());
                retornoFormulario.ParLevel1s = GetParLevel1s(form, factory, form.ParSecao_Ids.Length > 0 ? form.ParSecao_Ids.ToList() : retornoFormulario.ParSecoes?.Select(x => x.Id).ToList());
                var parLevel1_Ids = form.ParLevel1_Ids.Length > 0 ? form.ParLevel1_Ids.ToList() : retornoFormulario.ParLevel1s.Select(x => x.Id).ToList();
                retornoFormulario.ParLevel2s = GetParLevel2s(form, factory, parLevel1_Ids);
                var parLevel2_Ids = form.ParLevel2_Ids.Length > 0 ? form.ParLevel2_Ids.ToList() : retornoFormulario.ParLevel2s.Select(x => x.Id).ToList();

                string sqlFilter = "";
                if (parLevel1_Ids.Count > 0 || parLevel2_Ids.Count > 0)
                {
                    sqlFilter = $@" LEFT JOIN ParVinculoPeso PVP ON PVP.ParLevel3_Id = PL3.Id WHERE 1 = 1 ";
                    if (parLevel1_Ids.Count > 0)
                    {
                        sqlFilter += $@" AND PVP.ParLevel1_Id IN ({ string.Join(",", parLevel1_Ids)}) OR PVP.ParLevel1_Id IS NULL ";
                    }
                    if (parLevel2_Ids.Count > 0)
                    {
                        sqlFilter += $@" AND PVP.ParLevel2_Id IN ({ string.Join(",", parLevel2_Ids)}) OR PVP.ParLevel2_Id IS NULL ";
                    }
                }

                var query = $@"SELECT DISTINCT TOP 500 PL3.ID, PL3.NAME FROM parLevel3 PL3 {sqlFilter}
                AND Pl3.Name like '%{search}%'";

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
                    SELECT DISTINCT TOP 500 ID, Name FROM ParStructure
                    WHERE 1=1
                    AND Name like '%{search}%'
                    {wSG}";

                var retorno = factory.SearchQuery<Select3ViewModel>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredParStructureRegional")]
        public List<Select3ViewModel> GetFilteredParStructureRegional(string search, [FromBody] DataCarrierFormularioNew form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {
                var query = $@"SELECT DISTINCT TOP 500 ID, Name FROM ParStructure
                    WHERE ParStructureGroup_Id = 2 AND Name like '%{search}%'";

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
                var query = $@"SELECT DISTINCT TOP 500 ID, Name FROM ParStructureGroup
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
                var query = $@"SELECT DISTINCT TOP 500 Id, Name FROM ParModule
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
                            FROM parlevel3level2 WHERE weight like '%{search}%' AND IsActive = 1 order by Id asc ";

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
                var query = $@"SELECT DISTINCT TOP 500 Id, Name from UserSgq Where role like '%Supervisor%' AND Name like '%{search}%'";

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
                if(usuarios_Ids.Count > 0)
                     query = $@"SELECT DISTINCT TOP 500 Id, Name from UserSgq Where Name like '%{search}%' AND ParCompany_Id IN(" + string.Join(",", usuarios_Ids) + ")";
                else
                     query = $@"SELECT DISTINCT TOP 500 Id, Name from UserSgq Where Name like '%{search}%'";

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
            var sql = "Select Id, Name from ParStructure where Active = 1";

            var retorno = factory.SearchQuery<ParStructure>(sql).ToList();

            return retorno;
        }

        private List<ParStructure> GetParStructureRegional(DataCarrierFormularioNew form, Factory factory)
        {
            var sql = "Select Id, Name from ParStructure where ParStructureGroup_Id = 2 AND Active = 1";

            var retorno = factory.SearchQuery<ParStructure>(sql).ToList();

            return retorno;
        }

        private List<ParGroupParLevel1> GetParGroupParLevel1s(DataCarrierFormularioNew form, Factory factory)
        {
            var sql = "Select Id, Name from ParGroupParLevel1 where AND Active = 1";

            var retorno = factory.SearchQuery<ParGroupParLevel1>(sql).ToList();

            return retorno;
        }

        private List<ParCompany> GetParCompanies(DataCarrierFormularioNew form, Factory factory)
        {

            var filtroStructure = form.ParStructure_Ids.Length > 0 ? $@"AND PCXS.Id IN ({ string.Join(",", form.ParStructure_Ids) })" : "";

            var query = $@"SELECT
                        	PC.Id, PC.Name
                        FROM ParCompany PC
                        LEFT JOIN ParCompanyXStructure PCXS ON PC.Id = PCXS.ParCompany_Id AND PCXS.Active = 1
                        WHERE 1 = 1
                        AND PC.IsActive = 1
                        --Filtros
                        AND PC.Id IN ({GetUserUnitsIds(form.ShowUserCompanies)})
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

            var query = $@"SELECT Distinct PD.Id,PD.Name FROM ParDepartment PD 
                            WHERE 1=1 
                            AND PD.Active = 1 
                            AND PD.ParCompany_Id IN ({GetUserUnitsIds(form.ShowUserCompanies)})
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

            var query = $@"SELECT Distinct PD.Id,PD.Name  FROM ParDepartment PD 
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

            var query = $@"SELECT Distinct PC.Id,PC.Name FROM ParCargo PC
                        LEFT JOIN ParCargoXDepartment PCXD ON PCXD.ParCargo_Id = PC.Id
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
                sqlFilter = $@" LEFT JOIN ParVinculoPeso PVP ON PVP.ParLevel1_Id = PL1.Id WHERE 1 = 1 ";
                sqlFilter += $@"AND (PVP.ParDepartment_Id IN ({ string.Join(",", parDepartment_Ids)})";
                sqlFilter += $@"OR PVP.ParDepartment_Id IS NULL) ";
            }
            var query = "SELECT DISTINCT PL1.ID, PL1.NAME FROM parLevel1 PL1 " + sqlFilter;

            var retorno = factory.SearchQuery<ParLevel1>(query).ToList();

            return retorno;
        }

        private List<ParLevel2> GetParLevel2s(DataCarrierFormularioNew form, Factory factory, List<int> parLevel1_Ids)
        {
            string sqlFilter = "";
            if (parLevel1_Ids.Count > 0)
            {
                sqlFilter = $@" LEFT JOIN ParVinculoPeso PVP ON PVP.ParLevel2_Id = PL2.Id
                                WHERE PVP.ParLevel1_Id IN ({string.Join(",", parLevel1_Ids)})";
            }

            var query = "SELECT DISTINCT PL2.ID, PL2.NAME FROM parLevel2 PL2 " + sqlFilter;

            var retorno = factory.SearchQuery<ParLevel2>(query).ToList();

            return retorno;
        }

        private List<ParLevel3> GetParLevel3s(DataCarrierFormularioNew form, Factory factory, List<int> parLevel1_Ids, List<int> parLevel2_Ids)
        {

            string sqlFilter = "";
            if (parLevel1_Ids.Count > 0 || parLevel2_Ids.Count > 0)
            {
                sqlFilter = $@" LEFT JOIN ParVinculoPeso PVP ON PVP.ParLevel2_Id = PL3.Id WHERE 1 = 1 ";
                if (parLevel1_Ids.Count > 0)
                {
                    sqlFilter += $@"AND PVP.ParLevel1_Id IN ({ string.Join(",", parLevel1_Ids)})";
                }
                if (parLevel2_Ids.Count > 0)
                {
                    sqlFilter += $@"AND PVP.ParLevel2_Id IN ({ string.Join(",", parLevel2_Ids)})";
                }
            }

            var query = "SELECT DISTINCT PL3.ID, PL3.NAME FROM parLevel3 PL3 " + sqlFilter;

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
