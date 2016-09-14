﻿using Application.Interface;
using DTO.DTO;
using DTO.Helpers;
using SgqSystem.Secirity;
using SgqSystem.ViewModels;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {

        private readonly IUserApp _userApp;

        public UserController(IUserApp userApp)
        {
            _userApp = userApp;
        }

        // POST: api/Teste
        public GenericReturn<UserDTO> Post([FromBody] UserViewModel userVm)
        {
            return _userApp.AuthenticationLogin(userVm);
        }

        [Route("AuthenticationLogin")]
        [HttpPost]
        public GenericReturn<UserDTO> AuthenticationLogin([FromBody] UserViewModel userVm)
        {
            return _userApp.AuthenticationLogin(userVm);
        }

        [Route("GetAllUserValidationAd")]
        [HttpPost]
        public GenericReturn<List<UserDTO>> GetAllUserValidationAd(UserViewModel user)
        {
            return _userApp.GetAllUserValidationAd(user);
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