using Application.Interface;
using AutoMapper;
using Dominio.Entities;
using SgqSystem.ViewModels;
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


        // GET: api/Teste
        public bool Get(string name, string pass)
        {
            return true;//_userAppService.AuthenticationLogin(name, pass);
        }

        // GET: api/Teste/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Teste
        public GenericReturnViewModel<UserViewModel> Post([FromBody] UserViewModel user)
        {
            var query = _userAppService.AuthenticationLogin(user.Name, user.Password);
            var result = Mapper.Map<GenericReturn<User>, GenericReturnViewModel<UserViewModel>>(query);
            return result;
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