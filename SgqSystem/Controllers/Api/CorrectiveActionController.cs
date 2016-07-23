using Application.Interface;
using DTO.DTO;
using SgqSystem.ViewModels;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [RoutePrefix("api/CorrectiveAction")]
    public class CorrectiveActionController : ApiController
    {

        private readonly ICorrectiveActionAppService _correctiveActionAppService;
        private readonly IUserAppService _userAppService;

        public CorrectiveActionController(ICorrectiveActionAppService correctiveActionAppService, IUserAppService userAppService)
        {
            _correctiveActionAppService = correctiveActionAppService;
            _userAppService = userAppService;
        }

        [Route("SalvarAcaoCorretiva")]
        [HttpPost]
        public string SalvarAcaoCorretiva([FromBody]CorrectiveActionViewModel model)
        {

           // var result = _correctiveActionAppService.SalvarAcaoCorretiva(model.CorrectiveAction);

            return "123";
        }


        [Route("LogarUsuarioSlaughter")]
        [HttpPost]
        public UserDTO LogarUsuarioSlaughter([FromBody]CorrectiveActionViewModel model)
        {
            var user = new UserDTO()
            {
                Name = model.SlaughterLogin,
                Password = model.SlaughterPassword
            };

            var result = _userAppService.AuthenticationLogin(user);

            return result.Retorno;
        }


        [Route("LogarUsuarioTechnical")]
        [HttpPost]
        public UserDTO LogarUsuarioTechnical([FromBody]CorrectiveActionViewModel model)
        {
            var user = new UserDTO()
            {
                Name = model.TechnicalLogin,
                Password = model.TechnicalPassword
            };

            var result = _userAppService.AuthenticationLogin(user);

            return result.Retorno;

        }

    }
}
