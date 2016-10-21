﻿using DTO.DTO.Params;

namespace Dominio.Interfaces.Services
{
    public interface IParamsDomain
    {

        ParamsDdl CarregaDropDownsParams();

        ParamsDTO AddUpdateLevel1(ParamsDTO paramsDto);
        ParamsDTO GetLevel1(int IdParLevel1);

    }
}
