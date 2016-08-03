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

    }




}