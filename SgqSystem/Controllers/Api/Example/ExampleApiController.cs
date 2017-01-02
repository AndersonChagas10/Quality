﻿using Dominio.Interfaces.Services;
using DTO.DTO;
using SgqSystem.Handlres;
using SgqSystem.ViewModels;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Example
{
    [HandleApi()]
    [RoutePrefix("api/Example")]
    public class ExampleApiController : ApiController
    {

        #region Construtor para injeção de dependencia

        private IExampleDomain _exampleDomain;

        public ExampleApiController(IExampleDomain exampleDomain)
        {
            _exampleDomain = exampleDomain;
        }

        #endregion

        #region Metodos disponíveis na API

        [HttpPost]
        [Route("AddExample")]
        public ContextExampleDTO AddExample([FromBody] ContextExampleViewModel paramsViewModel)
        {
           return _exampleDomain.AddUpdateExample(paramsViewModel);
        }

        #endregion

        [HttpPost]
        [HandleApi()]
        [Route("ExemploPost/{param1}/{param2}")]
        public string ExemploPost(string param1, string param2)
        {
            return param1 + "," + param2;
        }

        [HttpGet]
        [Route("ExemploGet/{param1}/{param2}")]
        public string ExemploGet(string param1, string param2)
        {
            return param1 + "," + param2;
        }

        [HttpGet]
        [Route("ExemploGet2/{idLevel1}/{idLevel2}/{idLevel3}")]
        public string ExemploGet2(int idLevel1, int idLevel2, int idLevel3)
        {
            return "a";
            //returna paramsViewModel;
        }

         [HttpGet]
        [Route("ExemploGet3/{idLevel1}/{idLevel2}/{idLevel3}")]
        public string ExemploGet3(string idLevel1, string idLevel2, string idLevel3)
        {
            return "a";
            //returna paramsViewModel;
        }
    }
}
