using Data.PlanoDeAcao.Repositorio;
using Dominio;
using DTO.PlanoDeAcao;
using System;
using System.Linq;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.PlanoDeAcao
{
    [RoutePrefix("api/AcompanhamentoApi")]

    public class AcompanhamentoApiController : BaseApiController
    {
        private readonly IAcompanhamentoRepository _acompanhamentoRepository;

        public AcompanhamentoApiController(IAcompanhamentoRepository acompanhamentoRepository)
        {
            _acompanhamentoRepository = acompanhamentoRepository;
        }

        [Route("Post/Acompanhamento/{id}")]
        [HttpPost]
        public AcaoViewModel Post([FromUri] int id, [FromBody] AcompanhamentoAcaoInputModel objAcompanhamentoAcao)
        {
            try
            {
                var usuarioLogado = base.GetUsuarioLogado();

                var listaNotificar = objAcompanhamentoAcao.ListaNotificar
                    .Select(x =>
                        new AcompanhamentoAcaoXNotificar()
                        {
                            UserSgq_Id = x.Id
                        }
                    ).ToList();

                var acompanhamento = new AcompanhamentoAcao()
                {
                    ListaNotificar = listaNotificar,
                    Observacao = objAcompanhamentoAcao.Observacao,
                    Status = objAcompanhamentoAcao.Status,
                    Acao_Id = id,
                    UserSgq_Id = usuarioLogado.Id
                };
                _acompanhamentoRepository.SalvarAcompanhamentoAcao(acompanhamento);
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }
    }
}