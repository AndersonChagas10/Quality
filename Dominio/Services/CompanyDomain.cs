using System;
using AutoMapper;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using DTO.DTO.Params;
using System.Collections.Generic;
using DTO.Helpers;
using System.Linq;
using System.Data.Entity;

namespace Dominio.Services
{
    public class CompanyDomain : ICompanyDomain
    {
        #region Construtor

        private IBaseRepository<ParCompany> _baseRepoParCompany;
        private IBaseRepository<ParStructure> _baseRepoParStructure;
        private IBaseRepository<ParStructureGroup> _baseRepoParStructureGroup;
        private IBaseRepository<ParCompanyXStructure> _baseRepoParCompanyXStructure;
        private IBaseRepository<ParCompanyCluster> _baseRepoParCompanyCluster;

        public CompanyDomain(
            IBaseRepository<ParCompany> baseRepoParCompany,
            IBaseRepository<ParStructure> baseRepoParStructure,
            IBaseRepository<ParStructureGroup> baseRepoParStructureGroup,
            IBaseRepository<ParCompanyXStructure> baseRepoParCompanyXStructure,
            IBaseRepository<ParCompanyCluster> baseRepoParCompanyCluster
            )
        {
            _baseRepoParCompany = baseRepoParCompany;
            _baseRepoParStructure = baseRepoParStructure;
            _baseRepoParStructureGroup = baseRepoParStructureGroup;
            _baseRepoParCompanyXStructure = baseRepoParCompanyXStructure;
            _baseRepoParCompanyCluster = baseRepoParCompanyCluster;
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Se valido, classe paramsDto.example (Example) é salva em Banco de dados.
        /// 
        /// Rn1 - Valida entidade.
        /// Rn2 - Salva/Update entidade no Banco de Dados.
        /// Rn3 - Retorna entidade com Id.
        /// 
        /// </summary>
        /// <param name="parCompanyDTO"></param>
        /// <returns></returns>
        public ParCompanyDTO AddUpdateParCompany(ParCompanyDTO parCompanyDTO)
        {
            ParCompany parCompanySalvar = Mapper.Map<ParCompany>(parCompanyDTO);

            _baseRepoParCompany.AddOrUpdateNotCommit(parCompanySalvar);
            _baseRepoParCompany.Commit();

            parCompanyDTO.Id = parCompanySalvar.Id;

            if (parCompanyDTO.ListParCompanyCluster != null)
                foreach (var parCompanyCluster in parCompanyDTO.ListParCompanyCluster)
                {
                    parCompanyCluster.ParCompany_Id = parCompanyDTO.Id;
                    ParCompanyCluster parCompanyClusterSalvar = Mapper.Map<ParCompanyCluster>(parCompanyCluster);
                    _baseRepoParCompanyCluster.AddOrUpdateNotCommit(parCompanyClusterSalvar);
                    _baseRepoParCompanyCluster.Commit();
                }

            if (parCompanyDTO.ListParCompanyXStructure != null)
                foreach (var parCompanyXStructure in parCompanyDTO.ListParCompanyXStructure)
                {
                    parCompanyXStructure.ParCompany_Id = parCompanyDTO.Id;
                    ParCompanyXStructure parCompanyXStructureSalvar = Mapper.Map<ParCompanyXStructure>(parCompanyXStructure);
                    _baseRepoParCompanyXStructure.AddOrUpdateNotCommit(parCompanyXStructureSalvar);
                    _baseRepoParCompanyXStructure.Commit();
                }

            return parCompanyDTO;
        }

        public void SaveParCompany(ParCompany parCompany)
        {

            if (parCompany.Id == 0)
            {
                _baseRepoParCompany.Add(parCompany);
            }
            else
            {
                Guard.verifyDate(parCompany, "AlterDate");
                _baseRepoParCompany.Update(parCompany);
            }
        }

        public void SaveParCompanyCluster(List<ParCompanyCluster> listParCompanyCluster, ParCompany parCompany)
        {
            List<ParCompanyCluster> dbList = _baseRepoParCompanyCluster.GetAll().Where(r => r.ParCompany_Id == parCompany.Id && r.Active == true).ToList();

            foreach (ParCompanyCluster companyCluster in dbList)
            {
                ParCompanyCluster save = listParCompanyCluster.Where(r => r.ParCluster_Id == companyCluster.ParCluster_Id &&
                                            r.ParCompany_Id == companyCluster.ParCompany_Id &&
                                            r.Active == true).FirstOrDefault();

                if (save == null)
                {
                    companyCluster.Active = false;
                    Guard.verifyDate(companyCluster, "AlterDate");
                    _baseRepoParCompanyCluster.Update(companyCluster);
                }
                else
                {
                    save.ParCompany_Id = companyCluster.ParCompany_Id;
                    save.Id = companyCluster.Id;
                    Guard.verifyDate(companyCluster, "AlterDate");
                    _baseRepoParCompanyCluster.Update(companyCluster);
                }
                listParCompanyCluster.Remove(save);
            }

            foreach (ParCompanyCluster companyCluster in listParCompanyCluster)
            {
                companyCluster.Active = true;
                companyCluster.ParCompany_Id = parCompany.Id;
                _baseRepoParCompanyCluster.Add(companyCluster);
            }
        }

        public void SaveParCompanyXStructure(List<ParCompanyXStructure> listParCompanyXStructure, ParCompany parCompany)
        {
            List<ParCompanyXStructure> dbList = _baseRepoParCompanyXStructure.GetAll().Where(r => r.ParCompany_Id == parCompany.Id && r.Active == true).ToList();

            foreach (ParCompanyXStructure companyStructure in dbList)
            {
                ParCompanyXStructure save = listParCompanyXStructure.Where(r => r.ParStructure_Id == companyStructure.ParStructure_Id &&
                                            r.ParCompany_Id == companyStructure.ParCompany_Id &&
                                            r.Active == true).FirstOrDefault();

                if (save == null)
                {
                    companyStructure.Active = false;
                    Guard.verifyDate(companyStructure, "AlterDate");
                    _baseRepoParCompanyXStructure.Update(companyStructure);
                }
                else
                {
                    save.ParCompany_Id = companyStructure.ParCompany_Id;
                    save.Id = companyStructure.Id;
                    Guard.verifyDate(companyStructure, "AlterDate");
                    _baseRepoParCompanyXStructure.Update(companyStructure);
                }
                listParCompanyXStructure.Remove(save);
            }

            if(listParCompanyXStructure != null)
                foreach (ParCompanyXStructure companyStructure in listParCompanyXStructure)
                {
                    companyStructure.Active = true;
                    companyStructure.ParCompany_Id = parCompany.Id;
                    _baseRepoParCompanyXStructure.Add(companyStructure);
                }


        }


        public ParCompanyXStructureDTO AddUpdateParCompanyXStructureDTO(ParCompanyXStructureDTO parCompanyXStructureDTO)
        {
            throw new NotImplementedException();
        }

        public ParStructureDTO AddUpdateParStructure(ParStructureDTO parStructureDTO)
        {
            ParStructure parStructureSalvar = Mapper.Map<ParStructure>(parStructureDTO);

            _baseRepoParStructure.AddOrUpdateNotCommit(parStructureSalvar);
            _baseRepoParStructure.Commit();

            parStructureDTO.Id = parStructureSalvar.Id;

            return parStructureDTO;
        }

        public ParStructureGroupDTO AddUpdateParStructureGroup(ParStructureGroupDTO parStructureGroupDTO)
        {
            ParStructureGroup parStructureGroupSalvar = Mapper.Map<ParStructureGroup>(parStructureGroupDTO);

            _baseRepoParStructureGroup.AddOrUpdateNotCommit(parStructureGroupSalvar);
            _baseRepoParStructureGroup.Commit();

            parStructureGroupDTO.Id = parStructureGroupSalvar.Id;

            return parStructureGroupDTO;
        }

        #endregion
    }
}