using Conformity.Application.Core.PlanoDeAcao;
using Conformity.Domain.Core.DTOs;
using Conformity.Infra.CrossCutting;
using System;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.PlanoDeAcao
{
    [RoutePrefix("api/AcompanhamentoApi")]

    public class AcompanhamentoApiController : BaseAuthenticatedApiController
    {
        private readonly AcompanhamentoAcaoService _acompanhamentoService;

        public AcompanhamentoApiController(AcompanhamentoAcaoService acompanhamentoService
            , ApplicationConfig applicationConfig) : base(applicationConfig)
        {
            _acompanhamentoService = acompanhamentoService;
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