using DTO.DTO.Params;

namespace Dominio.Interfaces.Services
{
    public interface IParamsDomain
    {

        ParamsDTO AddUpdateLevel1(ParamsDTO paramsDto);
        ParamsDTO GetLevel1(int IdParLevel1);

        ParamsDTO AddUpdateLevel2(ParamsDTO paramsDto);

        ParamsDTO AddUpdateLevel3(ParamsDTO paramsDto);

    }
}
