using DTO.DTO;

namespace Dominio.Interfaces.Services
{
    public interface IParamsDomain
    {

        ParamsDTO Integrate();

        ParamsDTO AddUpdateParLevel1();
        ParamsDTO AddUpdateParLevel2();
        ParamsDTO AddUpdateParLevel3();

        ParamsDTO AddUpdateCompany();

    }
}
