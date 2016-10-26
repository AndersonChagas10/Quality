using DTO.DTO.Params;

namespace Dominio.Interfaces.Services
{
    public interface IParamsDomain
    {

        ParamsDdl CarregaDropDownsParams();

        ParamsDTO AddUpdateLevel1(ParamsDTO paramsDto);
        ParLevel1DTO GetLevel1(int IdParLevel1);
        ParamsDTO AddUpdateLevel2(ParamsDTO paramsDto);
    }
}
