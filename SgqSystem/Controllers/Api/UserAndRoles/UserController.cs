using DTO.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using SgqSystem.Handlres;
using SgqSystem.Secirity;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using SgqService.ViewModels;

namespace SgqSystem.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/User")]
    public class UserController : BaseApiController
    {

        private readonly IUserDomain _userDomain;

        private SgqServiceBusiness.Api.UserController business;
        public UserController(IUserDomain userDomain)
        {
            _userDomain = userDomain;
            business = new SgqServiceBusiness.Api.UserController(userDomain);
        }

        public GenericReturn<UserDTO> Post([FromBody] UserViewModel userVm)
        {
            return business.Post(userVm);
        }

        [HttpPost]
        [HandleApi()]
        [Route("AuthenticationLogin")]
        public GenericReturn<UserDTO> AuthenticationLogin([FromBody] UserViewModel userVm)
        {
            return business.AuthenticationLogin(userVm);
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
            return business.GetAllUserByUnit(unidadeId);
        }

        [HttpPost]
        [Route("VerifyPassiveSiginInLoginScreen")]
        public bool VerifyPassiveSiginInLoginScreen()
        {
            return business.VerifyPassiveSiginInLoginScreen();
        }
    }

}