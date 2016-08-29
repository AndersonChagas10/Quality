using Application.Interface;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using System.Collections.Generic;

namespace Application.AppServiceClass
{
    public class UserApp : IUserApp
    {

        private readonly IUserDomain _userService;

        public UserApp(IUserDomain userService)
        {
            _userService = userService;
        }

        public GenericReturn<UserDTO> AuthenticationLogin(UserDTO user)
        {
            return _userService.AuthenticationLogin(user);
        }

        public GenericReturn<UserDTO> GetByName(string username)
        {
            return _userService.GetByName(username);
        }

        public GenericReturn<List<UserDTO>> GetAllUserValidationAd(UserDTO user)
        {
            return _userService.GetAllUserValidationAd(user);
        }

    }

}
