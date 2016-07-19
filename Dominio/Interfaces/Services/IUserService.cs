using DTO.DTO;
using DTO.Helpers;

namespace Dominio.Interfaces.Services
{
    public interface IUserService 
    {
        GenericReturn<UserDTO> AuthenticationLogin(UserDTO user);
    }
}
