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

        public GenericReturn<User> AuthenticationLogin(string name, string password)
        {
            return _userService.AuthenticationLogin(name, password);
        }

    }

}
