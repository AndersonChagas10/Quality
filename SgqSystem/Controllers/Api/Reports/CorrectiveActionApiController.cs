using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Reports
{
    [RoutePrefix("api/CorrectiveAction")]
    public class CorrectiveActionApiController : ApiController
    {
        //private readonly ICorrectiveActionDomain _correctiveActionAppService;
        //private readonly IUserDomain _userAppService;

        //public CorrectiveActionApiController(ICorrectiveActionDomain correctiveActionAppService, IUserDomain userAppService)
        //{
        //    _correctiveActionAppService = correctiveActionAppService;
        //    _userAppService = userAppService;
        //}

        //[Route("SalvarAcaoCorretiva")]
        //[HttpPost]
        //public GenericReturn<CorrectiveActionDTO> SalvarAcaoCorretiva([FromBody]CorrectiveActionViewModel model)
        //{
        //    return _correctiveActionAppService.SalvarAcaoCorretiva(model);
        //}

        //[Route("GetCorrectiveAction")]
        //[HttpPost]
        //public GenericReturn<List<CorrectiveActionDTO>> GetCorrectiveAction([FromBody]FormularioParaRelatorioViewModel model)
        //{
        //    return _correctiveActionAppService.GetCorrectiveAction(model);
        //}

        //[Route("LoginSlaughterTechinical")]
        //[HttpPost]
        //public GenericReturn<UserDTO> LoginSlaughterTechinical([FromBody]UserViewModel model)
        //{
        //    var result = _userAppService.AuthenticationLogin(model);
        //    return result;
        //}

        //[Route("VerificarAcaoCorretivaIncompleta")]
        //[HttpPost]
        //public GenericReturn<CorrectiveActionDTO> VerificarAcaoCorretivaIncompleta([FromBody]CorrectiveActionViewModel model)
        //{
        //    var result = _correctiveActionAppService.VerificarAcaoCorretivaIncompleta(model);
        //    return result;
        //}

        //[Route("GetCorrectiveActionById")]
        //[HttpPost]
        //public GenericReturn<CorrectiveActionDTO> GetCorrectiveActionById([FromBody]int id)
        //{
        //    return _correctiveActionAppService.GetCorrectiveActionById(id);
        //}
    }
}