using DTO.DTO;
using DTO.Helpers;
using System.Collections.Generic;

namespace Dominio.Interfaces.Services
{
    public interface IUserDomain
    {
        GenericReturn<UserDTO> AuthenticationLogin(UserDTO user);
        GenericReturn<UserDTO> GetByName(string username);
        GenericReturn<List<UserDTO>> GetAllUserValidationAd(UserDTO userDto);
    }
}
