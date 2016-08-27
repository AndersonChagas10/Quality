using DTO.DTO;
using DTO.Helpers;

namespace Application.Interface
{
    public interface IUserApp
    {
        GenericReturn<UserDTO> AuthenticationLogin(UserDTO user);
        GenericReturn<UserDTO> GetByName(string user);
    }
}
