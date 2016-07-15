using DTO.DTO;
using DTO.Helpers;

namespace Application.Interface
{
    public interface IUserAppService 
    {
        GenericReturn<UserDTO> AuthenticationLogin(UserDTO user);
    }
}
