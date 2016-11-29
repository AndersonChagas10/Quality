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

        public CompanyDomain(
            IBaseRepository<ParCompany> baseRepoParCompany
            )
        {
            _baseRepoParCompany = baseRepoParCompany;
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

            //parCompanyDTO.example.IsValid(); //Rn1
            ParCompany parCompanySalvar = Mapper.Map<ParCompany>(parCompanyDTO);

            _baseRepoParCompany.AddOrUpdateNotCommit(parCompanySalvar);
            _baseRepoParCompany.Commit();

            parCompanyDTO.Id = parCompanySalvar.Id; 

            return parCompanyDTO;
        }

        public ParStructureDTO AddUpdateParStructure(ParStructureDTO parStructureDTO)
        {
            throw new NotImplementedException();
        }

        public ParStructureGroupDTO AddUpdateParStructureGroup(ParStructureGroupDTO parStructureGroupDTO)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}