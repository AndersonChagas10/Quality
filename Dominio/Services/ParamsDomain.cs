using Dominio.Interfaces.Services;
using DTO.DTO;
using Dominio.Interfaces.Repositories;

namespace Dominio.Services
{
    /// <summary>
    /// Rns para parametrização.
    /// </summary>
    public class ParamsDomain : IParamsDomain
    {

        #region Constructor

        private IBaseRepository<Example> _baseRepoParamLevel1;

        public ParamsDomain(IBaseRepository<Example> baseRepoParamLevel1)
        {
            _baseRepoParamLevel1 = baseRepoParamLevel1;
        }

        #endregion

        #region Metods

        /// <summary>
        /// Salva parametrização Level1
        /// 
        /// Rn1 Valida objeto
        /// Rn2 Salva objeto
        /// Rn3 Retorna objeto atualizado.
        /// 
        /// </summary>
        /// <param name="paramsDto"></param>
        /// <returns></returns>
        public ParamsDTO AddUpdateLevel1(ParamsDTO paramsDto)
        {

            //paramsDto.IsValid();
            //Params saveParamLevel1 = Mapper.Map<Params>(paramsDto);
            //_baseRepoParamLevel1.AddOrUpdate(saveParamLevel1);
            //paramsDto = Mapper.Map<ParamsDTO>(saveParamLevel1);

            //return paramsDto;
            return paramsDto;
        } 




        #endregion
    }
}
