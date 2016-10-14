using DTO.DTO;

namespace Dominio.Interfaces.Services
{
    public interface IExampleDomain
    {

        ContextExampleDTO AddUpdateExample(ContextExampleDTO paramsDto);
       
    }
}
