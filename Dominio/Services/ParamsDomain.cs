using System;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;

namespace Dominio.Services
{
    public class ParamsDomain : IParamsDomain
    {
     

        public ParamsDTO AddUpdateParLevel1(ParamsDTO paramsDto)
        {
            try
            {

            }
            catch (Exception e)
            {
                new GenericReturn<ParamsDTO>(e, )
                throw;
            }
        }
    }
}
