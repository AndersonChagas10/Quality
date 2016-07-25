using Application.Interface;
using AutoMapper;
using DTO.DTO;
using DTO.Helpers;
using SgqSystem.ViewModels;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {

        private readonly IUserApp _userAppService;

        public UserController(IUserApp userAppService)
        {
            _userAppService = userAppService;
        }

        // POST: api/Teste
        public GenericReturn<UserDTO> Post([FromBody] UserViewModel userVm)
        {
            return _userAppService.AuthenticationLogin(userVm);
        }

    }




}