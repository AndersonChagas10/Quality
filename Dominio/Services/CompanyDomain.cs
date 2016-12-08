using System;
using AutoMapper;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using DTO.DTO.Params;

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