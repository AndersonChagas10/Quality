using Application.Interface;
using AutoMapper;
using Dominio.Entities;
using SgqSystem.ViewModels;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ResultOldController : ApiController
    {

        private readonly IResultOldAppService _resultOldAppService;

        public ResultOldController(IResultOldAppService resultOldAppService)
        {
            _resultOldAppService = resultOldAppService;
        }


        // POST: api/ResultOld
        public GenericReturnViewModel<ResultOldViewModel> Post([FromBody] ResultOldViewModel result)
        {
            var resultToSave = Mapper.Map<ResultOldViewModel, ResultOld>(result);
            var response = _resultOldAppService.Salvar(resultToSave);
            var responseViewModel = Mapper.Map<GenericReturn<ResultOld>, GenericReturnViewModel<ResultOldViewModel>>(response);
            return responseViewModel;
        }

    }
}
