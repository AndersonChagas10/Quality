using Application.Interface;
using DTO.DTO;
using Dominio.Interfaces.Services;

namespace Application.AppClass
{
    public class ParamsApp : IParamsApp
    {

        #region Construtor

        private IParamsDomain _paramsDomain;

        public ParamsApp(IParamsDomain paramsDomain)
        {
            _paramsDomain = paramsDomain;
        }

        #endregion

        #region Metodos

        public ParamsDTO AddUpdateParLevel1()
        {
            return _paramsDomain.AddUpdateParLevel1();
        } 

        #endregion

    }
}
