using Dominio.Interfaces.Services;
using DTO.DTO;
using SgqSystem.Handlres;
using SgqSystem.ViewModels;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Example
{
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
        [HandleApi()]
        [Route("AddExample")]
        public ContextExampleDTO AddExample([FromBody] ContextExampleViewModel paramsViewModel)
        {
           return _exampleDomain.AddUpdateExample(paramsViewModel);
        }

        #endregion
    }
}
