using Application.Interface;
using AutoMapper;
using Dominio.Entities;
using Dominio.Helpers;
using SgqSystem.ViewModels;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {

        private readonly IUserAppService _userAppService;

        public UserController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        // POST: api/Teste
        public GenericReturnViewModel<UserViewModel> Post([FromBody] UserViewModel user)
        {
            try
            {
                var UserModel = Mapper.Map<UserViewModel, User>(user);
                var queryResult = _userAppService.AuthenticationLogin(UserModel);
                var userLogado = Mapper.Map<GenericReturn<User>, GenericReturnViewModel<UserViewModel>>(queryResult);
                return userLogado;
            }
            catch (Exception e)
            {
                return new GenericReturnViewModel<UserViewModel>(e, e.Message, "");
            }
        }

        // PUT: api/Teste/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Teste/5
        public void Delete(int id)
        {
        }
    }




}