using Conformity.Application.Core.PlanoDeAcao;
using Conformity.Domain.Core.DTOs;
using Conformity.Infra.CrossCutting;
using Dominio;
using System;
using System.Linq;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.PlanoDeAcao
{
    [RoutePrefix("api/AcompanhamentoApi")]

    public class AcompanhamentoApiController : BaseApiController
    {
        private readonly ApplicationConfig _applicationConfig;
        private readonly AcompanhamentoAcaoService _acompanhamentoService;

        public AcompanhamentoApiController(AcompanhamentoAcaoService acompanhamentoService
            , ApplicationConfig applicationConfig)
        {
            _acompanhamentoService = acompanhamentoService;
            _applicationConfig = applicationConfig;
        }

        [Route("Post/Acompanhamento/{id}")]
        [HttpPost]
        public AcaoViewModel Post([FromUri] int id, [FromBody] AcompanhamentoAcaoInputModel objAcompanhamentoAcao)
        {
            try
            {
                _acompanhamentoService.SalvarAcompanhamentoComNotificaveis(id, objAcompanhamentoAcao);
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }
    }
}