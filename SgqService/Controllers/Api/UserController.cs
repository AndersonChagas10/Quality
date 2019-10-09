//using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using DTO.Interfaces.Services;
using SgqService.Handlres;
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