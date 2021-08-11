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
        private readonly AcaoService _acaoService;

        public AcompanhamentoApiController(AcompanhamentoAcaoService acompanhamentoService,
            AcaoService acaoService, ApplicationConfig applicationConfig) : base(applicationConfig)
        {
            _acompanhamentoService = acompanhamentoService;
            _acaoService = acaoService;
        }

        [Route("Post/Acompanhamento/{id}")]
        [HttpPost]
        public AcaoViewModel Post([FromUri] int id, [FromBody] AcompanhamentoAcaoInputModel objAcompanhamentoAcao)
        {
            try
            {
                int acompanhamentoId = _acompanhamentoService.SalvarAcompanhamentoComNotificaveis(id, objAcompanhamentoAcao);

                _acaoService.AlterarStatusComBaseNoAcompanhamento(id, objAcompanhamentoAcao);

                _acaoService.EnviarEmail(acompanhamentoId, id);
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }
    }
}