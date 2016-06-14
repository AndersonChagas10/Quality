using Application.Interface;
using AutoMapper;
using Dominio.Entities;
using MVC.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MVC.API.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TesteController : ApiController
    {

        private readonly IUserAppService _userAppService;

        public TesteController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }


        // GET: api/Teste
        public bool Get(string name, string pass)
        {
            return true;//_userAppService.Autorizado(name, pass);
        }

        // GET: api/Teste/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Teste
        public GenericReturnViewModel<UserViewModel> Post(string name, string pass)
        {
            var query =  _userAppService.Autorizado(name, pass);
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

