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
                return new ParamsDTO();
            }
            catch (Exception e)
            {
                //Salva Log
                new GenericReturn<ParamsDTO>(e, "");
                throw;
            }
        }
    }
}
