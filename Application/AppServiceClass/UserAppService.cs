using Application.Interface;
using Dominio.Entities;
using Dominio.Interfaces.Services;

namespace Application.AppServiceClass
{
    public class UserAppService :  IUserAppService
    {
        
        private readonly IUserService _userService;
        
        public UserAppService(IUserService userService)
        {
            _userService = userService;
        }

        public GenericReturn<UserSgq> AuthenticationLogin(UserSgq user)
        {
            return _userService.AuthenticationLogin(user);
        }

    }

}
