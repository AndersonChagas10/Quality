using AutoMapper;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using DTO.DTO;
using System;

namespace Dominio.Services
{
    public class ExampleDomain : IExampleDomain
    {
        #region Construtor

        private IBaseRepository<Example> _baseRepoExample;

        public ExampleDomain(
            IBaseRepository<Example> baseRepoExample
            )
        {
            _baseRepoExample = baseRepoExample;
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
        /// <param name="paramsDto"></param>
        /// <returns></returns>
        public ContextExampleDTO AddUpdateExample(ContextExampleDTO paramsDto)
        {

            paramsDto.example.IsValid(); //Rn1
            Example exampleSalvar = Mapper.Map<Example>(paramsDto.example);

            _baseRepoExample.AddOrUpdateNotCommit(exampleSalvar);//Rn2
            _baseRepoExample.Commit();

            paramsDto.example.Id = exampleSalvar.Id; //Rn3

            return paramsDto;
        } 

        #endregion
    }
}