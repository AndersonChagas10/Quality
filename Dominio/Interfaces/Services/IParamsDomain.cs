﻿using DTO.DTO.Params;
using System.Collections.Generic;

namespace Dominio.Interfaces.Services
{
    public interface IParamsDomain
    {

        List<ParLevel1DTO> GetAllLevel1();

        ParamsDdl CarregaDropDownsParams();

        ParamsDTO AddUpdateLevel1(ParamsDTO paramsDto);
        ParLevel1DTO GetLevel1(int IdParLevel1);

        ParamsDTO AddUpdateLevel2(ParamsDTO paramsDto);
        ParamsDTO GetLevel2(int idParLevel2, int idParLevel3, int idParLevel1);
        ParLevel3GroupDTO RemoveParLevel3Group(int Id);


        ParamsDTO AddUpdateLevel3(ParamsDTO paramsDto);
        ParamsDTO GetLevel3(int idParLevel3, int? idParLevel2);
        //ParLevel3Level2DTO AddVinculoL3L2(int idLevel2, int idLevel3, decimal peso);
        //ParLevel3Level2Level1DTO AddVinculoL1L2(int idLevel1, int idLevel2Level3);
        List<ParLevel3Level2Level1DTO> AddVinculoL1L2(int idLevel1, int idLevel2, int idLevel3, int? userId, int? companyId = null);
        ParLevel3Level2DTO AddVinculoL3L2(int idLevel2, int idLevel3, decimal peso, int? groupLevel2);
        bool RemVinculoL1L2(int idLevel1, int idLevel2);
        bool VerificaShowBtnRemVinculoL1L2(int idLevel1, int idLevel2);
        bool SetRequiredCamposCabecalho(int id, int required);
        ParMultipleValues SetDefaultMultiplaEscolha(int idHeader, int idMultiple);
        ParLevel2XHeaderField AddRemoveParHeaderLevel2(ParLevel2XHeaderField parLevel2XHeaderField);

       
    }
}
