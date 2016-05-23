using Application.Interface;
using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVC.Controllers
{
    public class UserController : ApiController
    {

        //private readonly IUserAppService _userAppService;

        //public UserController(IUserAppService userAppService)
        //{ 
        //    _userAppService = userAppService;
        //}

        // GET: api/User
        public IEnumerable<string> Get()
        {
           var user = new User(){ Name="teste", Password = "123"};
           //_userRepositorie.Autorizado(user);
            return new string[] { "value1", "value2" };
        }

        // GET: api/User/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/User
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/User/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }
    }
}
