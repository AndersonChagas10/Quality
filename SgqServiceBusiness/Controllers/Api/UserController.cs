//using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using DTO.Interfaces.Services;
using SgqService.ViewModels;
using SgqServiceBusiness.Security;
using System.Collections.Generic;

namespace SgqServiceBusiness.Api
{
    public class UserController
    {
        private readonly IUserDomain _userDomain;

        public UserController(IUserDomain userDomain)
        {
            _userDomain = userDomain;
        }

        // POST: api/Teste
        public GenericReturn<UserDTO> Post(UserViewModel userVm)
        {
            return _userDomain.AuthenticationLogin(userVm);
        }
        public GenericReturn<UserDTO> AuthenticationLogin(UserViewModel userVm)
        {
            return _userDomain.AuthenticationLogin(userVm);
        }

        public GenericReturn<List<UserDTO>> GetAllUserValidationAd(UserViewModel user)
        {
            return _userDomain.GetAllUserValidationAd(user);
        }

        public List<UserDTO> GetAllUserByUnit(int unidadeId)
        {
            return _userDomain.GetAllUserByUnit(unidadeId);
        }

        public bool VerifyPassiveSiginInLoginScreen()
        {
            if (!(string.IsNullOrEmpty(SessionPersister.Username)))
            {
                return true;
            }
            return false;
        }
    }

}