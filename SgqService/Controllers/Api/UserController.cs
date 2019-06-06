﻿//using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using DTO.Interfaces.Services;
using SgqService.Handlres;
using SgqService.Security;
using SgqService.ViewModels;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqService.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/User")]
    public class UserController : BaseApiController
    {
        private readonly IUserDomain _userDomain;

        public UserController(IUserDomain userDomain)
        {
            _userDomain = userDomain;
        }

        // POST: api/Teste
        public GenericReturn<UserDTO> Post([FromBody] UserViewModel userVm)
        {
            return _userDomain.AuthenticationLogin(userVm);
        }

        [HttpPost]
        [HandleApi()]
        [Route("AuthenticationLogin")]
        public GenericReturn<UserDTO> AuthenticationLogin([FromBody] UserViewModel userVm)
        {
            return _userDomain.AuthenticationLogin(userVm);
        }

        [HttpPost]
        [Route("GetAllUserValidationAd")]
        public GenericReturn<List<UserDTO>> GetAllUserValidationAd(UserViewModel user)
        {
            return _userDomain.GetAllUserValidationAd(user);
        }

        [HttpPost]
        [Route("GetAllUserByUnit/{unidadeId}")]
        public List<UserDTO> GetAllUserByUnit(int unidadeId)
        {
            return _userDomain.GetAllUserByUnit(unidadeId);
        }

        [HttpPost]
        [Route("VerifyPassiveSiginInLoginScreen")]
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