﻿using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using SgqSystem.Handlres;
using SgqSystem.Secirity;
using SgqSystem.ViewModels;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [HandleApi()]
    [RoutePrefix("api/User")]
    public class UserController : ApiController
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

        [Route("AuthenticationLogin")]
        [HttpPost]
        public GenericReturn<UserDTO> AuthenticationLogin([FromBody] UserViewModel userVm)
        {
            return _userDomain.AuthenticationLogin(userVm);
        }

        [Route("GetAllUserValidationAd")]
        [HttpPost]
        public GenericReturn<List<UserDTO>> GetAllUserValidationAd(UserViewModel user)
        {
            return _userDomain.GetAllUserValidationAd(user);
        }

        [Route("VerifyPassiveSiginInLoginScreen")]
        [HttpPost]
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