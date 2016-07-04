using Application.Interface;
using AutoMapper;
using Dominio.Entities;
using Dominio.Helpers;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
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

        // POST: api/SaveList
        [Route("api/SalvarLista")]
        public GenericReturnViewModel<ResultOldViewModel> SalvarLista([FromBody] List<ResultOldViewModel> obj)
        {
            List<ResultOld> objToSave;
            try
            {
                objToSave = Mapper.Map<List<ResultOldViewModel>, List<ResultOld>>(obj);
                var result = _resultOldAppService.SalvarLista(objToSave);
                return Mapper.Map<GenericReturn<ResultOld>, GenericReturnViewModel<ResultOldViewModel>>(result);
            }
            catch (Exception e)
            {
                return new GenericReturnViewModel<ResultOldViewModel>(e, e.Message, "");
            }
        }

    }
}
