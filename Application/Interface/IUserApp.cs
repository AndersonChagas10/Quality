using DTO.DTO;
using DTO.Helpers;
using System.Collections.Generic;

namespace Application.Interface
{
    public interface IUserApp
    {
        GenericReturn<UserDTO> AuthenticationLogin(UserDTO user);
        GenericReturn<UserDTO> GetByName(string user);
        GenericReturn<List<UserDTO>> GetAllUserValidationAd(UserDTO userDto);
    }
}
