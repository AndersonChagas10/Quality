﻿using DTO.DTO.Params;

namespace Dominio.Interfaces.Services
{
    public interface IParamsDomain
    {

        ParamsDdl CarregaDropDownsParams();

        ParamsDTO AddUpdateLevel1(ParamsDTO paramsDto);
        ParLevel1DTO GetLevel1(int IdParLevel1);

        ParamsDTO AddUpdateLevel2(ParamsDTO paramsDto);
        ParamsDTO GetLevel2(int idParLevel2);

        ParamsDTO AddUpdateLevel3(ParamsDTO paramsDto);
        ParamsDTO GetLevel3(int idParLevel3, int idParLevel2);
        ParLevel3Level2DTO AddVinculoL3L2(int idLevel2, int idLevel3, decimal peso);
        //ParLevel3Level2Level1DTO AddVinculoL1L2(int idLevel1, int idLevel2Level3);
        ParLevel3Level2Level1DTO AddVinculoL1L2(int idLevel1, int idLevel2, int idLevel3);
    }
}
