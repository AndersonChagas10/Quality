using Data.PlanoDeAcao.Repositorio;
using Dominio;
using DTO.PlanoDeAcao;
using Services.PlanoDeAcao;
using System;
using System.Linq;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.PlanoDeAcao
{
    [RoutePrefix("api/AcompanhamentoApi")]

    public class AcompanhamentoApiController : BaseApiController
    {
        private readonly IAcompanhamentoAcaoService _acompanhamentoService;

        public AcompanhamentoApiController(IAcompanhamentoAcaoService acompanhamentoService)
        {
            _acompanhamentoService = acompanhamentoService;
        }

        [Route("Post/Acompanhamento/{id}")]
        [HttpPost]
        public AcaoViewModel Post([FromUri] int id, [FromBody] AcompanhamentoAcaoInputModel objAcompanhamentoAcao)
        {
            try
            {
                var usuarioLogado = base.GetUsuarioLogado();

                _acompanhamentoService.SalvarAcompanhamentoComNotificaveis(id, objAcompanhamentoAcao, usuarioLogado);
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }
    }
}