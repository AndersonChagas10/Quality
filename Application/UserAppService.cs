using Application.Interface;
using Dominio.Entities;
using Dominio.Interfaces.Services;

namespace Application
{
    public class UserAppService :  IUserAppService
    {
        
        private readonly IUserService _userService;
        
        public UserAppService(IUserService userService)
        {
            _userService = userService;
        }

        public GenericReturn<User> AuthenticationLogin(User user)
        {
            return _userService.AuthenticationLogin(user);
        }

    }

}
