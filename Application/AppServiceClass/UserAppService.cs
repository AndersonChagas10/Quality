using Application.Interface;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;

namespace Application.AppServiceClass
{
    public class UserAppService :  IUserAppService
    {
        
        private readonly IUserService _userService;
        
        public UserAppService(IUserService userService)
        {
            _userService = userService;
        }

        public GenericReturn<UserDTO> AuthenticationLogin(UserDTO user)
        {
            return _userService.AuthenticationLogin(user);
        }

    }

}
