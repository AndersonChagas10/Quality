using DTO.DTO;
using DTO.Helpers;

namespace Dominio.Interfaces.Services
{
    public interface IUserDomain 
    {
        GenericReturn<UserDTO> AuthenticationLogin(UserDTO user);
    }
}
