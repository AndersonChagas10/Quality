﻿using Application.Interface;
using DTO.DTO;
using DTO.Helpers;
using SgqSystem.ViewModels;
using System.Collections.Generic;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [RoutePrefix("api/CorrectiveAction")]
    public class CorrectiveActionController : ApiController
    {

        private readonly ICorrectiveActionApp _correctiveActionAppService;
        private readonly IUserApp _userAppService;

        public CorrectiveActionController(ICorrectiveActionApp correctiveActionAppService, IUserApp userAppService)
        {
            _correctiveActionAppService = correctiveActionAppService;
            _userAppService = userAppService;
        }

        [Route("SalvarAcaoCorretiva")]
        [HttpPost]
        public GenericReturn<CorrectiveActionDTO> SalvarAcaoCorretiva([FromBody]CorrectiveActionViewModel model)
        {
            return _correctiveActionAppService.SalvarAcaoCorretiva(model);
        }

        [Route("GetCorrectiveAction")]
        [HttpPost]
        public GenericReturn<List<CorrectiveActionDTO>> GetCorrectiveAction([FromBody]FormularioParaRelatorioViewModel model)
        {
            return _correctiveActionAppService.GetCorrectiveAction(model);
        }

        [Route("LoginSlaughterTechinical")]
        [HttpPost]
        public GenericReturn<UserDTO> LoginSlaughterTechinical([FromBody]UserViewModel model)
        {
            var result = _userAppService.AuthenticationLogin(model);
            return result;
        }

        [Route("VerificarAcaoCorretivaIncompleta")]
        [HttpPost]
        public GenericReturn<CorrectiveActionDTO> VerificarAcaoCorretivaIncompleta([FromBody]CorrectiveActionViewModel model)
        {
            var result = _correctiveActionAppService.VerificarAcaoCorretivaIncompleta(model);
            return result;
        }

        [Route("GetCorrectiveActionById")]
        [HttpPost]
        public GenericReturn<CorrectiveActionDTO> GetCorrectiveActionById([FromBody]int id)
        {
            return _correctiveActionAppService.GetCorrectiveActionById(id);
        }
    }
}
