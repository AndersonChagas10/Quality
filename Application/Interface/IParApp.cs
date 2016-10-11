using DTO.DTO;

namespace Application.Interface
{
    public interface IParApp
    {

        ParamsDTO Integrate();

        ParamsDTO AddUpdateParLevel1();
        ParamsDTO AddUpdateParLevel2();
        ParamsDTO AddUpdateParLevel3();

        ParamsDTO AddUpdateCompany();

    }
}
